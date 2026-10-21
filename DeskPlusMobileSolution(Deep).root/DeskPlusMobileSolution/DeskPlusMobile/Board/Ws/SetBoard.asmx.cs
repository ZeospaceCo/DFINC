using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Helper.WebComponent;

namespace Zeospace.DeskPlusMobile.Web.Board.Ws
{
    /// <summary>
    /// SetBoard의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class SetBoard : Helper.MobileBaseService
    {
        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 댓글 등록
        /// 작성일자 : 2011-07-13
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddShortReply(string p)
        {
            SetDecryptString(p);
            int categoryID = GetDecryptParam("CID", "0").ToNumber<int>();
            int listID = GetDecryptParam("ID", "0").ToNumber<int>();
            string content = GetDecryptParam("Content", "");

            try
            {
                if (categoryID <= 0 || UserID <= 0) return false;

                //통합게시판 회사아이디
                int boaComID = BLL.Category.GetCompanyCategoryInfo(categoryID);

                Zeospace.DeskPlusMobile.Web.Board.Ws.BE.ShortReplyInfo shortReplyInfo
                    = new Zeospace.DeskPlusMobile.Web.Board.Ws.BE.ShortReplyInfo();

                shortReplyInfo.CompanyID = boaComID;
                shortReplyInfo.CategoryID = categoryID;
                shortReplyInfo.ListID = listID;
                shortReplyInfo.UserID = UserID;
                shortReplyInfo.ShortReplyID = 0;
                shortReplyInfo.ParentID = 0;
                shortReplyInfo.GroupID = 0;
                shortReplyInfo.ListOrder = 0;
                shortReplyInfo.Depth = 0;
                shortReplyInfo.Content = Server.UrlDecode(content);
                shortReplyInfo.UserName = UserInfo.Name;

                Zeospace.DeskPlusMobile.Web.Board.Ws.Data.ShortReply biz
                    = new Zeospace.DeskPlusMobile.Web.Board.Ws.Data.ShortReply();

                if (!biz.Add(shortReplyInfo))
                {
                    return false;
                }

                // 알람 발송
                Ws.Data.List listDal = new Data.List();
                DataSet ds = listDal.GetBoardInfo(boaComID, categoryID, listID);

                int recvUserID = 0;


                // 알람-코드관리로 처리
                DataView dvApp = XmlDataBaseHelper.GetSysCode(boaComID, "AlarmBoard");

                //원본글 작성자에게 알람 발송하기
                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        recvUserID = dr["WriteUserID"].ToString().ToNumber<int>(); //받을 사람 (원본글 작성자 아이디)
                        shortReplyInfo.Title = dr["Title"].ToString().ToReverseXSS();             //제목

                        if (dvApp.Count > 0)
                        {

                            foreach (DataRowView dr2 in dvApp)
                            {
                                string method = dr2["CodeID"].ToString().ToUpper();
                                switch (method)
                                {
                                    case "MESSENGER":
                                        SendAlarmShortReply(shortReplyInfo, recvUserID, 1);
                                        break;

                                    case "MOBILE":
                                        SendAlarmShortReply(shortReplyInfo, recvUserID, 4);
                                        break;
                                }
                            }
                        }
                    }
                }

                //댓글 기재자들에게 알람 발송하기
                Ws.Data.ShortReply replyDal = new Data.ShortReply();
                DataSet dsReply = replyDal.GetShortReplyList(boaComID, categoryID, listID);
                List<int> userList = new List<int>();

                if (dsReply != null)
                {
                    foreach (DataRow dr in dsReply.Tables[0].Rows)
                    {
                        //댓글 작성자와 원본작성자와 동일하면 보내지 않는다.
                        if (recvUserID != dr["UserID"].ToString().ToNumber<int>())
                        {
                            //다중 댓글 중복자에게 보내지 않도록 한다. 1통만 보내자.~~~~
                            if (!userList.Exists(c => c == dr["UserID"].ToString().ToNumber<int>()))
                            {

                                if (dvApp.Count > 0)
                                {

                                    foreach (DataRowView dr2 in dvApp)
                                    {
                                        string method = dr2["CodeID"].ToString().ToUpper();
                                        switch (method)
                                        {
                                            case "MESSENGER":
                                                SendAlarmShortReply(shortReplyInfo, dr["UserID"].ToString().ToNumber<int>(), 1);
                                                break;
                                            case "MOBILE":
                                                SendAlarmShortReply(shortReplyInfo, dr["UserID"].ToString().ToNumber<int>(), 4);
                                                break;
                                        }
                                    }
                                }

                                userList.Add(dr["UserID"].ToString().ToNumber<int>());
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("AddShortReply", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성목적 : 대댓글 등록
        /// 작성일자 : 2018-10-12
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddShortReplyReplys(string p)
        {
            SetDecryptString(p);
            int categoryID = GetDecryptParam("CID", "0").ToNumber<int>();
            int listID = GetDecryptParam("ID", "0").ToNumber<int>();
            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int shortReplyID = GetDecryptParam("shortReplyID", "0").ToNumber<int>();
            string content = GetDecryptParam("Content", "");

            try
            {
                if (categoryID <= 0 || UserID <= 0) return false;

                Zeospace.DeskPlusMobile.Web.Board.Ws.BE.ShortReplyInfo shortReplyInfo
                    = new Zeospace.DeskPlusMobile.Web.Board.Ws.BE.ShortReplyInfo();

                Zeospace.DeskPlusMobile.Web.Board.Ws.Data.ShortReply biz
                    = new Zeospace.DeskPlusMobile.Web.Board.Ws.Data.ShortReply();

                DataSet dsReplyID = biz.GetShortReplyListID(companyID, categoryID, listID, shortReplyID);
                if (dsReplyID != null)
                {
                    foreach (DataRow dr in dsReplyID.Tables[0].Rows)
                    {
                        shortReplyInfo.CompanyID = companyID;
                        shortReplyInfo.CategoryID = categoryID;
                        shortReplyInfo.ListID = listID;
                        shortReplyInfo.UserID = UserID;
                        shortReplyInfo.ShortReplyID = shortReplyID;
                        shortReplyInfo.ParentID = shortReplyID;
                        shortReplyInfo.GroupID = dr["GroupID"].ToString().ToNumber<int>();
                        shortReplyInfo.ListOrder = dr["ListOrder"].ToString().ToNumber<int>() + 1;
                        shortReplyInfo.Depth = dr["Depth"].ToString().ToNumber<int>() + 1;
                        shortReplyInfo.Content = Server.UrlDecode(content);
                        shortReplyInfo.UserName = UserInfo.Name;
                    }
                }

                if (!biz.Add(shortReplyInfo))
                {
                    return false;
                }

                // 알람 발송
                Ws.Data.List listDal = new Data.List();
                DataSet ds = listDal.GetBoardInfo(companyID, categoryID, listID);

                int recvUserID = 0;


                // 알람-코드관리로 처리
                DataView dvApp = XmlDataBaseHelper.GetSysCode(companyID, "AlarmBoard");

                //원본글 작성자에게 알람 발송하기
                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        recvUserID = dr["WriteUserID"].ToString().ToNumber<int>(); //받을 사람 (원본글 작성자 아이디)
                        shortReplyInfo.Title = dr["Title"].ToString().ToReverseXSS();             //제목

                        if (dvApp.Count > 0)
                        {

                            foreach (DataRowView dr2 in dvApp)
                            {
                                string method = dr2["CodeID"].ToString().ToUpper();
                                switch (method)
                                {
                                    case "MESSENGER":
                                        SendAlarmShortReply(shortReplyInfo, recvUserID, 1);
                                        break;

                                    case "MOBILE":
                                        SendAlarmShortReply(shortReplyInfo, recvUserID, 4);
                                        break;
                                }
                            }
                        }
                    }
                }

                //댓글 기재자들에게 알람 발송하기
                Ws.Data.ShortReply replyDal = new Data.ShortReply();
                DataSet dsReply = replyDal.GetShortReplyList(companyID, categoryID, listID);
                List<int> userList = new List<int>();

                if (dsReply != null)
                {
                    foreach (DataRow dr in dsReply.Tables[0].Rows)
                    {
                        //댓글 작성자와 원본작성자와 동일하면 보내지 않는다.
                        if (recvUserID != dr["UserID"].ToString().ToNumber<int>())
                        {
                            //다중 댓글 중복자에게 보내지 않도록 한다. 1통만 보내자.~~~~
                            if (!userList.Exists(c => c == dr["UserID"].ToString().ToNumber<int>()))
                            {

                                if (dvApp.Count > 0)
                                {

                                    foreach (DataRowView dr2 in dvApp)
                                    {
                                        string method = dr2["CodeID"].ToString().ToUpper();
                                        switch (method)
                                        {
                                            case "MESSENGER":
                                                SendAlarmShortReply(shortReplyInfo, dr["UserID"].ToString().ToNumber<int>(), 1);
                                                break;
                                            case "MOBILE":
                                                SendAlarmShortReply(shortReplyInfo, dr["UserID"].ToString().ToNumber<int>(), 4);
                                                break;
                                        }
                                    }
                                }

                                userList.Add(dr["UserID"].ToString().ToNumber<int>());
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("AddShortReplyReplys", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성목적 : 댓글 수정
        /// 작성일자 : 2018-10-15
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModShortReply(string p)
        {
            SetDecryptString(p);
            int categoryID = GetDecryptParam("CID", "0").ToNumber<int>();
            int listID = GetDecryptParam("ID", "0").ToNumber<int>();
            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int shortReplyID = GetDecryptParam("shortReplyID", "0").ToNumber<int>();
            string content = GetDecryptParam("Content", "");

            try
            {
                if (categoryID <= 0 || UserID <= 0) return false;

                Zeospace.DeskPlusMobile.Web.Board.Ws.BE.ShortReplyInfo shortReplyInfo
                    = new Zeospace.DeskPlusMobile.Web.Board.Ws.BE.ShortReplyInfo();

                Zeospace.DeskPlusMobile.Web.Board.Ws.Data.ShortReply biz
                    = new Zeospace.DeskPlusMobile.Web.Board.Ws.Data.ShortReply();

                shortReplyInfo.CompanyID = companyID;
                shortReplyInfo.CategoryID = categoryID;
                shortReplyInfo.ListID = listID;
                shortReplyInfo.UserID = UserID;
                shortReplyInfo.ShortReplyID = shortReplyID;
                shortReplyInfo.Content = Server.UrlDecode(content);

                if (!biz.Mod(shortReplyInfo))
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("ModShortReply", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성목적 : 댓글 삭제
        /// 작성일자 : 2018-10-15
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelShortReply(string p)
        {
            SetDecryptString(p);
            int categoryID = GetDecryptParam("CID", "0").ToNumber<int>();
            int listID = GetDecryptParam("ID", "0").ToNumber<int>();
            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int shortReplyID = GetDecryptParam("shortReplyID", "0").ToNumber<int>();
            int groupID = GetDecryptParam("groupID", "0").ToNumber<int>();

            try
            {
                if (categoryID <= 0 || UserID <= 0) return false;

                Zeospace.DeskPlusMobile.Web.Board.Ws.BE.ShortReplyInfo shortReplyInfo
                    = new Zeospace.DeskPlusMobile.Web.Board.Ws.BE.ShortReplyInfo();

                Zeospace.DeskPlusMobile.Web.Board.Ws.Data.ShortReply biz
                    = new Zeospace.DeskPlusMobile.Web.Board.Ws.Data.ShortReply();

                shortReplyInfo.CompanyID = companyID;
                shortReplyInfo.CategoryID = categoryID;
                shortReplyInfo.ListID = listID;
                shortReplyInfo.GroupID = groupID;
                shortReplyInfo.ShortReplyID = shortReplyID;

                if (!biz.Del(shortReplyInfo))
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("DelShortReply", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성목적 : 댓글 추천 / 비추천 참여 여부
        /// 작성일자 : 2018-10-16
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetGoodCount(string p)
        {
            SetDecryptString(p);
            int categoryID = GetDecryptParam("CID", "0").ToNumber<int>();
            int listID = GetDecryptParam("ID", "0").ToNumber<int>();
            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int shortReplyID = GetDecryptParam("shortReplyID", "0").ToNumber<int>();

            try
            {
                if (categoryID <= 0 || UserID <= 0) return 0;

                Zeospace.DeskPlusMobile.Web.Board.Ws.BE.ShortReplyGoodInfo info
                    = new Zeospace.DeskPlusMobile.Web.Board.Ws.BE.ShortReplyGoodInfo();

                Zeospace.DeskPlusMobile.Web.Board.Ws.Data.ShortReplyGood biz
                    = new Zeospace.DeskPlusMobile.Web.Board.Ws.Data.ShortReplyGood();

                info.CompanyID = companyID;
                info.CategoryID = categoryID;
                info.ListID = listID;
                info.ShortReplyID = shortReplyID;
                info.UserID = UserID;

                int result = biz.GetGoodCount(info);

                return result;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetGoodCount", ex);
                return 0;
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성목적 : 댓글 추천 / 비추천
        /// 작성일자 : 2018-10-16
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int AddShortReplyGood(string p)
        {
            SetDecryptString(p);
            int categoryID = GetDecryptParam("CID", "0").ToNumber<int>();
            int listID = GetDecryptParam("ID", "0").ToNumber<int>();
            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int shortReplyID = GetDecryptParam("shortReplyID", "0").ToNumber<int>();
            int good = GetDecryptParam("good", "0").ToNumber<int>();
            int bad = GetDecryptParam("bad", "0").ToNumber<int>();

            try
            {
                if (categoryID <= 0 || UserID <= 0) return 0;

                Zeospace.DeskPlusMobile.Web.Board.Ws.BE.ShortReplyGoodInfo info
                    = new Zeospace.DeskPlusMobile.Web.Board.Ws.BE.ShortReplyGoodInfo();

                Zeospace.DeskPlusMobile.Web.Board.Ws.Data.ShortReplyGood biz
                    = new Zeospace.DeskPlusMobile.Web.Board.Ws.Data.ShortReplyGood();

                info.CompanyID = companyID;
                info.CategoryID = categoryID;
                info.ListID = listID;
                info.ShortReplyID = shortReplyID;
                info.UserID = UserID;
                info.Good = good;
                info.Bad = bad;

                biz.Add(info);

                return 1;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetGoodCount", ex);
                return 0;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 글 등록
        /// 작성일자 : 2015-06-29
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string AddContent(string p)
        {
            SetDecryptString(p);
            int categoryID = GetDecryptParam("CID", "0").ToNumber<int>();
            string CatHeader = GetDecryptParam("CatHeader", "");
            string fontColor = GetDecryptParam("FontColor", "");
            string fontSize = GetDecryptParam("FontSize", "");
            string isBold = GetDecryptParam("IsBold", "");
            string isOptionModify = GetDecryptParam("IsOptionModify", "");
            string isOptionTop = GetDecryptParam("IsOptionTop", "");
            string isOptionTopDay = GetDecryptParam("IsOptionTopDay", "");
            string isPopUp = GetDecryptParam("IsPopUp", "");
            string isOptionAnonymity = GetDecryptParam("IsOptionAnonymity", "");
            string optionAlarm = GetDecryptParam("OptionAlarm", "");
            string categoryName = GetDecryptParam("CategoryName", "");
            string startDate = GetDecryptParam("StartDate", "");
            string endDate = GetDecryptParam("EndDate", "");
            string fileYN = GetDecryptParam("FileYN", "");
            string screenShot = GetDecryptParam("ScreenShot", "");
            string swfUploadGUID = GetDecryptParam("SwfUploadGUID", "N");
            string title = GetDecryptParam("Title", "");
            string content = GetDecryptParam("Content", "");
            string optionDownload = GetDecryptParam("OptionDownload", "0");

            try
            {
                if (categoryID <= 0 || UserID <= 0) return "";

                //통합게시판 회사아이디
                int boaComID = BLL.Category.GetCompanyCategoryInfo(categoryID);

                Ws.BE.ListInfo info = new Ws.BE.ListInfo();
                info.CompanyID = boaComID;
                info.CategoryID = categoryID;
                info.ListID = 0;
                info.GroupID = 0;
                info.ListOrder = 0;
                info.Depth = 0;

                info.Title = Server.UrlDecode(title);

                string tmpContent = @"<HTML><HEAD><META content=IE=5 http-equiv=X-UA-Compatible><META content='text/html; charset=utf-8' http-equiv=Content-Type>
<STYLE>p {margin-top:2px;margin-bottom:2px;}</STYLE><META name=GENERATOR content='TAGFREE Active Designer'></HEAD><BODY>" + Server.UrlDecode(content) + "</BODY></HTML>";

                info.Content = tmpContent;
                info.MobileContent = Server.UrlDecode(content);

                info.OwnerDeptID = UserInfo.DeptID;
                info.OptionModify = isOptionModify;
                info.OptionTop = isOptionTop;
                info.OptionTopDay = isOptionTopDay.ToNumber<int>();
                info.HeaderID = CatHeader.ToNumber<int>();
                info.WriteUserID = UserID;
                info.WriteDate = GetYearMonth();

                info.OptionAnonymity = isOptionAnonymity;
                info.WriteUserName = UserInfo.Name;

                if (isOptionAnonymity == "1")
                {
                    info.WriteUserName = (PreLang == "ko") ? "익명" : "NoName";
                }

                info.PassWord = string.Empty;
                info.ScreenShot = "";

                Ws.BE.ListEtcInfo etcInfo = new Ws.BE.ListEtcInfo();
                etcInfo.CompanyID = boaComID;
                etcInfo.CategoryID = categoryID;
                etcInfo.Etc1 = "";

                string etc = fontColor + "," + fontSize + "," + isBold;

                etcInfo.Etc2 = etc;
                etcInfo.Etc3 = isPopUp;
                etcInfo.Etc4 = (isPopUp == "1") ? DateHelper.GetCultureDate(startDate, "yyyyMMdd", PreLangReal) : "";
                etcInfo.Etc5 = (isPopUp == "1") ? DateHelper.GetCultureDate(endDate, "yyyyMMdd", PreLangReal) : "";
                etcInfo.Etc6 = "";
                etcInfo.Etc7 = "";
                etcInfo.Etc8 = Convert.ToInt32(optionDownload); ;
                etcInfo.Etc9 = 0;

                Data.List dalList = new Data.List();
                int listID = dalList.Add(info);

                Data.ListEtc dalEtc = new Data.ListEtc();
                etcInfo.ListID = listID;
                dalEtc.Add(etcInfo);

                if (fileYN != "")
                {
                    if (!AddFile(boaComID, listID, categoryID, swfUploadGUID, screenShot))
                    {
                        return "";
                    }

                    if (listID > 0)
                    {
                        #region DOC_STREAM
                        var docStreamYN = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLBoard, "DocStreamYN");

                        if (docStreamYN == "Y")
                        {
                            Data.AttachFiles files = new Data.AttachFiles();
                            DocStream ds = new DocStream();
                            try
                            {
                                var newDSList = files.GetAttachFiles(boaComID, categoryID, listID).Tables[0].Rows;
                                for (int i = 0; i < newDSList.Count; i++)
                                {
                                    ds.AddDocStream(boaComID, Convert.ToInt32(newDSList[i]["AttachFileID"].ToString()), "BOA_ATTACH");
                                    ds.UpdateFileID(boaComID, Convert.ToInt32(newDSList[i]["AttachFileID"].ToString()), "BOA_ATTACH");
                                }
                            }
                            catch (Exception ex)
                            {
                                var DSList = files.GetAttachFiles(boaComID, categoryID, listID).Tables[0].Rows;
                                for (int i = 0; i < DSList.Count; i++)
                                {
                                    ds.DelDocStream(boaComID, Convert.ToInt32(DSList[i]["AttachFileID"].ToString()), "BOA_ATTACH");
                                }
                                AppException.WriteException("BOA_ATTACH_Addition_DS : " + ex.Message);
                            }

                        }
                        #endregion
                    }
                }

                // 알람-코드관리로 처리
                DataView dvApp = XmlDataBaseHelper.GetSysCode(CompanyID, "AlarmBoard");
                if (dvApp.Count > 0)
                {

                    foreach (DataRowView dr in dvApp)
                    {
                        string method = dr["CodeID"].ToString().ToUpper();
                        switch (method)
                        {
                            case "MESSENGER":
                                //1.알람;
                                if (optionAlarm == "1")
                                {
                                    string alarmContent = AlarmContentTemplate(categoryName, info.Title, info.WriteUserName,
                                        System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString(), "", PreLang);

                                    info.AlarmContent = alarmContent;
                                    SendAlarmUser(info, listID, 1);
                                }
                                break;

                            case "MOBILE":
                                if (optionAlarm == "4")
                                {
                                    string alarmContent = AlarmContentTemplate(categoryName, info.Title, info.WriteUserName,
                                   System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString(), "", PreLang);

                                    info.AlarmContent = alarmContent;
                                    SendAlarmUser(info, listID, 4);
                                }
                                break;

                            case "SMS":
                                if (optionAlarm == "2")
                                {
                                    string smsContent = "{0}님이 '{1}' 글내용을 작성했습니다.";
                                    smsContent = string.Format(smsContent, info.WriteUserName, info.Title);
                                    info.SMSContent = smsContent;
                                    info.SMSLoginID = UserInfo.LoginID;
                                    info.SMSMobilePhone = UserInfo.MobilePhone;

                                    SendSMSUser(info);
                                }
                                break;
                        }
                    }
                }

                return listID.ToString();
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("AddContent", ex);
                return "";
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 파일추가
        /// 작성일자 : 2015-06-29
        /// </summary>
        /// <param name="_boaComID"></param>
        /// <param name="_listID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_swfUploadGUID"></param>
        /// <param name="_screenShot"></param>
        /// <returns></returns>
        public bool AddFile(int _boaComID, int _listID, int _categoryID, string _swfUploadGUID, string _screenShot)
        {
            try
            {
                string uploadPath = Server.MapPath("/DeskPlusMobile/FileData/" + CompanyID + "/Users/" + UserID + "/Mobile/" + _swfUploadGUID + "/");
                string yearMonth = System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString().PadLeft(2, '0');
                string targetPath = Server.MapPath("/DeskPlusMobile/FileData/" + CompanyID + "/Board/" + yearMonth + "/" + _listID + "/");
                string targetWebPath = "/DeskPlusEIP/FileData/" + CompanyID + "/Board/" + yearMonth + "/" + _listID + "/";
                string targetMobileWebPath = "/DeskPlusMobile/FileData/" + CompanyID + "/Board/" + yearMonth + "/" + _listID + "/";

                if (Directory.Exists(uploadPath))
                {
                    if (!Directory.Exists(targetPath)) Directory.CreateDirectory(targetPath);

                    DirectoryInfo DirInfo = new DirectoryInfo(uploadPath);

                    var files = new DirectoryInfo(uploadPath).EnumerateFiles().OrderBy(f => f.LastWriteTime);

                    string sourceFile = "";
                    string destinationFile = "";
                    string fileGUID = "";

                    foreach (var item in files)
                    {
                        if (item.Name != "Thumbs.db")
                        {
                            sourceFile = uploadPath + item.Name;
                            FileInfo sourceFileInfo = new FileInfo(sourceFile);

                            //파일명은 GUID 이며, 확장자는 원본과 동일하다.(.ZEO 사용안함)
                            fileGUID = System.Guid.NewGuid().ToString() + sourceFileInfo.Extension;
                            if (!targetPath.EndsWith(@"\")) targetPath += @"\";
                            destinationFile = targetPath + fileGUID;

                            File.Copy(sourceFile, destinationFile, true);

                            BE.AttachInfo info = new BE.AttachInfo();
                            info.CompanyID = _boaComID;
                            info.CategoryID = _categoryID;
                            info.ListID = _listID;
                            info.FilePath = targetWebPath;
                            info.FileName = sourceFileInfo.Name;
                            info.FileType = sourceFileInfo.Extension;
                            info.FileLength = sourceFileInfo.Length;
                            info.FileGUID = fileGUID;

                            AddFile(info);

                            HttpContext context = HttpContext.Current;

                            if (string.Compare(info.FileType, ".GIF", true) == 0 || string.Compare(info.FileType, ".JPG", true) == 0
                                    || string.Compare(info.FileType, ".PNG", true) == 0 || string.Compare(info.FileType, ".BMP", true) == 0 || string.Compare(info.FileType, ".EMF", true) == 0
                                    || string.Compare(info.FileType, ".TIF", true) == 0 || string.Compare(info.FileType, ".JPEG", true) == 0 || string.Compare(info.FileType, ".PJPG", true) == 0)
                            {
                                System.Drawing.Image image = System.Drawing.Image.FromFile(destinationFile);

                                #region 600 사이즈 이미지 (미리보기)

                                //최대 가로, 세로 600
                                string imgSizeSmall600 = GetThumnailSize(destinationFile, 600, 600);
                                Bitmap imgFile600 = new Bitmap(destinationFile);
                                Bitmap thumbnail600 = new Bitmap(imgFile600, imgSizeSmall600.Split(',')[0].ToNumber<int>(), imgSizeSmall600.Split(',')[1].ToNumber<int>());
                                //작은 이미지 Small_ 만들기
                                thumbnail600.Save(context.Server.MapPath(targetMobileWebPath + "Small_" + info.FileGUID), System.Drawing.Imaging.ImageFormat.Jpeg);
                                imgFile600.Dispose();
                                thumbnail600.Dispose();

                                #endregion 600 사이즈 이미지 (미리보기)

                                #region 350 사이즈 이미지 (스마트폰)

                                //최대 가로, 세로 350
                                string imgSizeSmall350 = GetThumnailSize(destinationFile, 350, 350);
                                Bitmap imgFile350 = new Bitmap(destinationFile);
                                Bitmap thumbnail350 = new Bitmap(imgFile350, imgSizeSmall350.Split(',')[0].ToNumber<int>(), imgSizeSmall350.Split(',')[1].ToNumber<int>());
                                //작은 이미지 Smart_ 만들기
                                thumbnail350.Save(context.Server.MapPath(targetMobileWebPath + "Smart_" + info.FileGUID), System.Drawing.Imaging.ImageFormat.Jpeg);
                                imgFile350.Dispose();
                                thumbnail350.Dispose();

                                #endregion 350 사이즈 이미지 (스마트폰)

                                if (_screenShot.ToUpper() == info.FileName.ToUpper())
                                {
                                    Data.List dal = new Data.List();
                                    dal.ModByScreenShot(_boaComID, _categoryID, _listID, info.FilePath + "Thum_" + info.FileGUID);

                                    //원본 파일
                                    string imgSize = GetThumnailSize(destinationFile, 230, 160);
                                    System.Drawing.Image thumb = image.GetThumbnailImage(imgSize.Split(',')[0].ToNumber<int>(), imgSize.Split(',')[1].ToNumber<int>(),
                                            delegate() { return false; }, (IntPtr)0);

                                    //스크린샷 만들기 파일명 앞에 Thum_ 붙이기
                                    thumb.Save(context.Server.MapPath(targetMobileWebPath + "Thum_" + info.FileGUID));
                                }
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("AddFile", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 섬네일 이미지 가로,세로 비율값 얻어내기
        /// </summary>
        /// <param name="_sourcePath">The _source path.</param>
        /// <param name="_width">The _width.</param>
        /// <param name="_height">The _height.</param>
        /// <returns></returns>
        public static string GetThumnailSize(string _sourcePath, int _width, int _height)
        {
            Bitmap sourceImage = new Bitmap(_sourcePath);
            ImageFormat sourceFormat = sourceImage.RawFormat;

            decimal ratio = 0;
            int newWidth = 0;
            int newHeight = 0;

            if (sourceImage.Width < _width && sourceImage.Height < _height)
            {
                newWidth = sourceImage.Width;
                newHeight = sourceImage.Height;
            }
            else
            {
                if (sourceImage.Width > sourceImage.Height)
                {
                    ratio = (decimal)_width / sourceImage.Width;
                    newWidth = _width;
                    newHeight = (int)(sourceImage.Height * ratio);
                }
                else
                {
                    ratio = (decimal)_height / sourceImage.Height;
                    newHeight = _height;
                    newWidth = (int)(sourceImage.Width * ratio);
                }
            }

            sourceImage.Dispose();

            return newWidth + "," + newHeight;
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 파일추가
        /// 작성일자 : 2015-06-29
        /// </summary>
        /// <param name="_attachInfo"></param>
        /// <returns></returns>
        public bool AddFile(BE.AttachInfo _attachInfo)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                                new SqlParameter("@CompanyID", SqlDbType.Int)
                                               ,new SqlParameter("@CategoryID", SqlDbType.Int)
                                               ,new SqlParameter("@ListID", SqlDbType.Int)
                                               ,new SqlParameter("@FilePath", SqlDbType.NVarChar,1000)
                                               ,new SqlParameter("@FileName", SqlDbType.NVarChar,500)
                                               ,new SqlParameter("@FileType", SqlDbType.NVarChar,5)
                                               ,new SqlParameter("@FileLength", SqlDbType.BigInt)
                                               ,new SqlParameter("@FileGUID", SqlDbType.VarChar,50)
                };

                parms[0].Value = _attachInfo.CompanyID;
                parms[1].Value = _attachInfo.CategoryID;
                parms[2].Value = _attachInfo.ListID;
                parms[3].Value = _attachInfo.FilePath;
                parms[4].Value = _attachInfo.FileName;
                parms[5].Value = _attachInfo.FileType;
                parms[6].Value = _attachInfo.FileLength;
                parms[7].Value = _attachInfo.FileGUID;

                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLBoard, "INS_PROC_Attach");
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, "dsAddFile", parms);

                return true;
            }
            catch (SqlException ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("AddFile(" + _attachInfo.ListID.ToString() + "," + _attachInfo.FileName + ")", ex);
                return false;
            }
        }

        /// <summary>
        /// 현재 년월 가져오기 [200601]
        /// </summary>
        /// <returns></returns>
        public static string GetYearMonth()
        {
            string nowYearMon = "";
            if (DateTime.Now.Month.ToString().Length == 1)
            {
                nowYearMon = DateTime.Now.Year.ToString() + "0" + DateTime.Now.Month.ToString();
            }
            else
            {
                nowYearMon = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString();
            }
            return nowYearMon;
        }

        /// <summary>
        /// Alarms the content template.
        /// </summary>
        /// <param name="_catName">Name of the _cat.</param>
        /// <param name="_subject">The _subject.</param>
        /// <param name="_name">The _name.</param>
        /// <param name="_startTime">The _start time.</param>
        /// <param name="_deptName">Name of the _dept.</param>
        /// <param name="_preLang">The _pre lang.</param>
        /// <returns></returns>
        public string AlarmContentTemplate(string _catName, string _subject, string _name, string _startTime, string _deptName, string _preLang)
        {
            string contHtml = "";
            XmlDocument confirmXml = null;

            string xmlPath = FileHelper.GetServerMapPath("/DeskPlusMobile/XmlData/BoardMessenger/" + _preLang + ".xml");
            confirmXml = new XmlDocument();
            confirmXml.Load(xmlPath);
            XmlNode tmpXmlNode = confirmXml.DocumentElement;

            contHtml = (tmpXmlNode.SelectSingleNode("/ContentInfo/ContentHTML") != null) ? System.Web.HttpUtility.HtmlDecode(tmpXmlNode.SelectSingleNode("/ContentInfo/ContentHTML").InnerText) : "";

            contHtml = contHtml.Replace("$SUBJECT$", _subject);          //문서제목
            contHtml = contHtml.Replace("$TIME$", _startTime);    //날짜

            contHtml = contHtml.Replace("$DEPTNAME$", _deptName);    //부서
            contHtml = contHtml.Replace("$CATEGORYTITLE$", _catName);    //카테고리명
            contHtml = contHtml.Replace("$NAME$", _name);    //이름

            return contHtml;
        }

        /// <summary>
        /// 알람발송자 명단 가져오기
        /// </summary>
        /// <param name="_listInfo"></param>
        /// <param name="_listID"></param>
        public void SendAlarmUser(BE.ListInfo _listInfo, int _listID, int _AlarmType)
        {
            string sendUserIDs = GetReadUser(_listInfo.CompanyID, _listInfo.CategoryID);
            Account.Data.User userBiz = new Account.Data.User();

            if (!string.IsNullOrEmpty(sendUserIDs))
            {
                //전체 알람 발송
                if (sendUserIDs == "all")
                {
                    List<int> alUser = new List<int>();

                    if (_listInfo.CompanyID == 0) //전사게시판
                    {
                        DataSet ds = userBiz.GetUserAll();
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            _listInfo.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();

                            string AlarmBoard = string.Empty;
                            string AlarmMail = string.Empty;
                            string AlarmMessenger = string.Empty;
                            string AlarmMobile = string.Empty;
                            string AlarmSMS = string.Empty;

                            AlarmBoard = dr["AlarmBoard"].ToString();

                            if (AlarmBoard.IndexOf("1-").Equals(0))
                            {
                                AlarmMail = (AlarmBoard.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                                AlarmMessenger = (AlarmBoard.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                                AlarmMobile = (AlarmBoard.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                                AlarmSMS = (AlarmBoard.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                            }
                            else
                            {
                                AlarmMessenger = "0";
                                AlarmMail = "0";
                                AlarmMobile = "0";
                                AlarmSMS = "0";
                            }

                            if (_AlarmType.ToString() == "1")
                            {
                                if (AlarmMessenger.Equals("1"))
                                {
                                    SendAlarmADD(_listInfo, _listID, dr["UserID"].ToString().ToNumber<int>(), _AlarmType);
                                }
                            }

                            if (_AlarmType.ToString() == "4")
                            {
                                SendAlarmADD(_listInfo, _listID, dr["UserID"].ToString().ToNumber<int>(), _AlarmType);
                            }
                        }
                    }
                    else
                    {
                        DataSet ds = userBiz.GetUserCompany(_listInfo.CompanyID); //계열사 사용자 가져오기
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {

                            string AlarmBoard = string.Empty;
                            string AlarmMail = string.Empty;
                            string AlarmMessenger = string.Empty;
                            string AlarmMobile = string.Empty;
                            string AlarmSMS = string.Empty;

                            AlarmBoard = dr["AlarmBoard"].ToString();

                            if (AlarmBoard.IndexOf("1-").Equals(0))
                            {
                                AlarmMail = (AlarmBoard.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                                AlarmMessenger = (AlarmBoard.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                                AlarmMobile = (AlarmBoard.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                                AlarmSMS = (AlarmBoard.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                            }
                            else
                            {
                                AlarmMessenger = "0";
                                AlarmMail = "0";
                                AlarmMobile = "0";
                                AlarmSMS = "0";
                            }

                            if (_AlarmType.ToString() == "1")
                            {
                                if (AlarmMessenger.Equals("1"))
                                {
                                    SendAlarmADD(_listInfo, _listID, dr["UserID"].ToString().ToNumber<int>(), _AlarmType);
                                }
                            }

                            if (_AlarmType.ToString() == "4")
                            {
                                SendAlarmADD(_listInfo, _listID, dr["UserID"].ToString().ToNumber<int>(), _AlarmType);
                            }
                        }
                    }
                }
                else //부서 및 사용자 알람 발송
                {
                    sendUserIDs = sendUserIDs.TrimEnd(',');

                    List<string> userList = sendUserIDs.Split(',').ToList<string>();
                    IEnumerable<string> userDis = userList.Distinct(); //사용자 중복제거

                    if (_listInfo.CompanyID == 0)
                    {
                        foreach (string _userID in userDis)
                        {
                            DataSet ds = userBiz.GetUser(_userID.ToNumber<int>());
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                _listInfo.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();

                                string AlarmBoard = string.Empty;
                                string AlarmMail = string.Empty;
                                string AlarmMessenger = string.Empty;
                                string AlarmMobile = string.Empty;
                                string AlarmSMS = string.Empty;

                                AlarmBoard = dr["AlarmBoard"].ToString();

                                if (AlarmBoard.IndexOf("1-").Equals(0))
                                {
                                    AlarmMail = (AlarmBoard.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                                    AlarmMessenger = (AlarmBoard.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                                    AlarmMobile = (AlarmBoard.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                                    AlarmSMS = (AlarmBoard.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                                }
                                else
                                {
                                    AlarmMessenger = "0";
                                    AlarmMail = "0";
                                    AlarmMobile = "0";
                                    AlarmSMS = "0";
                                }

                                if (_AlarmType.ToString() == "1")
                                {
                                    if (AlarmMessenger.Equals("1"))
                                    {
                                        SendAlarmADD(_listInfo, _listID, _userID.ToNumber<int>(), _AlarmType);
                                    }
                                }

                                if (_AlarmType.ToString() == "4")
                                {
                                    SendAlarmADD(_listInfo, _listID, _userID.ToNumber<int>(), _AlarmType);
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (string _userID in userDis)
                        {
                            DataSet ds = userBiz.GetUser(_userID.ToNumber<int>());
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {

                                string AlarmBoard = string.Empty;
                                string AlarmMail = string.Empty;
                                string AlarmMessenger = string.Empty;
                                string AlarmMobile = string.Empty;
                                string AlarmSMS = string.Empty;

                                AlarmBoard = dr["AlarmBoard"].ToString();

                                if (AlarmBoard.IndexOf("1-").Equals(0))
                                {
                                    AlarmMail = (AlarmBoard.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                                    AlarmMessenger = (AlarmBoard.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                                    AlarmMobile = (AlarmBoard.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                                    AlarmSMS = (AlarmBoard.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                                }
                                else
                                {
                                    AlarmMessenger = "0";
                                    AlarmMail = "0";
                                    AlarmMobile = "0";
                                    AlarmSMS = "0";
                                }

                                if (_AlarmType.ToString() == "1")
                                {
                                    if (AlarmMessenger.Equals("1"))
                                    {
                                        SendAlarmADD(_listInfo, _listID, _userID.ToNumber<int>(), _AlarmType);
                                    }
                                }

                                if (_AlarmType.ToString() == "4")
                                {
                                    SendAlarmADD(_listInfo, _listID, _userID.ToNumber<int>(), _AlarmType);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// SMS 발송하기
        /// </summary>
        /// <param name="_listInfo"></param>
        public void SendSMSUser(BE.ListInfo _listInfo)
        {
            string sendUserIDs = GetReadUser(_listInfo.CompanyID, _listInfo.CategoryID);
            Account.Data.User userBiz = new Account.Data.User();

            if (!string.IsNullOrEmpty(sendUserIDs))
            {
                //전체 알람 발송
                if (sendUserIDs == "all")
                {
                    List<int> alUser = new List<int>();

                    if (_listInfo.CompanyID == 0) //전사게시판
                    {
                        DataSet ds = userBiz.GetUserAll();

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            string AlarmBoard = string.Empty;
                            string AlarmMail = string.Empty;
                            string AlarmMessenger = string.Empty;
                            string AlarmMobile = string.Empty;
                            string AlarmSMS = string.Empty;

                            AlarmBoard = dr["AlarmBoard"].ToString();

                            if (AlarmBoard.IndexOf("1-").Equals(0))
                            {
                                AlarmMail = (AlarmBoard.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                                AlarmMessenger = (AlarmBoard.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                                AlarmMobile = (AlarmBoard.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                                AlarmSMS = (AlarmBoard.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                            }
                            else
                            {
                                AlarmMessenger = "0";
                                AlarmMail = "0";
                                AlarmMobile = "0";
                                AlarmSMS = "0";
                            }

                            if (AlarmSMS.Equals("1"))
                            {
                                _listInfo.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                                //SendSMS(_listInfo.CompanyID, _listInfo.WriteUserID, _listInfo.SMSLoginID, _listInfo.SMSContent, true, _listInfo.SMSMobilePhone);
                                SendSMS(_listInfo.CompanyID, _listInfo.WriteUserID, _listInfo.SMSLoginID, _listInfo.SMSContent, false, dr["MobilePhone"].ToString());
                            }
                        }
                    }
                    else
                    {
                        DataSet ds = userBiz.GetUserCompany(_listInfo.CompanyID);

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            string AlarmBoard = string.Empty;
                            string AlarmMail = string.Empty;
                            string AlarmMessenger = string.Empty;
                            string AlarmMobile = string.Empty;
                            string AlarmSMS = string.Empty;

                            AlarmBoard = dr["AlarmBoard"].ToString();

                            if (AlarmBoard.IndexOf("1-").Equals(0))
                            {
                                AlarmMail = (AlarmBoard.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                                AlarmMessenger = (AlarmBoard.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                                AlarmMobile = (AlarmBoard.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                                AlarmSMS = (AlarmBoard.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                            }
                            else
                            {
                                AlarmMessenger = "0";
                                AlarmMail = "0";
                                AlarmMobile = "0";
                                AlarmSMS = "0";
                            }

                            if (AlarmSMS.Equals("1"))
                            {
                                SendSMS(_listInfo.CompanyID, _listInfo.WriteUserID, _listInfo.SMSLoginID, _listInfo.SMSContent, false, dr["MobilePhone"].ToString());
                                //SendSMS(_listInfo.CompanyID, _listInfo.WriteUserID, _listInfo.SMSLoginID,_listInfo.SMSContent, true, _listInfo.SMSMobilePhone);
                            }
                        }
                    }
                }
                else //부서 및 사용자 알람 발송
                {
                    sendUserIDs = sendUserIDs.TrimEnd(',');

                    List<string> userList = sendUserIDs.Split(',').ToList<string>();
                    IEnumerable<string> userDis = userList.Distinct(); //사용자 중복제거

                    if (_listInfo.CompanyID == 0)
                    {
                        foreach (string _userID in userDis)
                        {
                            DataSet ds = userBiz.GetUser(_userID.ToNumber<int>());
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                string AlarmBoard = string.Empty;
                                string AlarmMail = string.Empty;
                                string AlarmMessenger = string.Empty;
                                string AlarmMobile = string.Empty;
                                string AlarmSMS = string.Empty;

                                AlarmBoard = dr["AlarmBoard"].ToString();

                                if (AlarmBoard.IndexOf("1-").Equals(0))
                                {
                                    AlarmMail = (AlarmBoard.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                                    AlarmMessenger = (AlarmBoard.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                                    AlarmMobile = (AlarmBoard.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                                    AlarmSMS = (AlarmBoard.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                                }
                                else
                                {
                                    AlarmMessenger = "0";
                                    AlarmMail = "0";
                                    AlarmMobile = "0";
                                    AlarmSMS = "0";
                                }

                                if (AlarmSMS.Equals("1"))
                                {
                                    _listInfo.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                                    SendSMS(_listInfo.CompanyID, _listInfo.WriteUserID, _listInfo.SMSLoginID, _listInfo.SMSContent, false, dr["MobilePhone"].ToString());
                                }
                            }
                        }
                    }
                    else
                    {
                        DataSet ds = userBiz.GetUserMobile(_listInfo.CompanyID, sendUserIDs);

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            string AlarmBoard = string.Empty;
                            string AlarmMail = string.Empty;
                            string AlarmMessenger = string.Empty;
                            string AlarmMobile = string.Empty;
                            string AlarmSMS = string.Empty;

                            AlarmBoard = dr["AlarmBoard"].ToString();

                            if (AlarmBoard.IndexOf("1-").Equals(0))
                            {
                                AlarmMail = (AlarmBoard.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                                AlarmMessenger = (AlarmBoard.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                                AlarmMobile = (AlarmBoard.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                                AlarmSMS = (AlarmBoard.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                            }
                            else
                            {
                                AlarmMessenger = "0";
                                AlarmMail = "0";
                                AlarmMobile = "0";
                                AlarmSMS = "0";
                            }

                            if (AlarmSMS.Equals("1"))
                            {
                                //부서 및 사용자 알람 발송
                                SendSMS(_listInfo.CompanyID, _listInfo.WriteUserID, _listInfo.SMSLoginID, _listInfo.SMSContent, false, dr["mobilephone"].ToString());
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 특정 게시판 카테고리 읽기 권한자 (all : 전체, 사용자아이디)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        private string GetReadUser(int _companyID, int _categoryID)
        {
            Data.Authority auth = new Data.Authority();
            Account.BLL.User bllUser = new Account.BLL.User();
            Account.Data.Dept dalDept = new Account.Data.Dept();

            DataSet ds = auth.GetAuthority(_companyID, _categoryID);
            string sendUserIDs = string.Empty;

            if (ds != null)
            {
                string type = string.Empty;
                int ownerID;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    type = dr["type"].ToString();
                    ownerID = dr["ownerID"].ToString().ToNumber<int>();

                    //회사별
                    if (type == "G" && ownerID == 0)
                    {
                        sendUserIDs = "all";
                        break;
                    }
                    else if (type == "G" && ownerID > 0) //회사별
                    {
                        DataSet dsDept = dalDept.GetDept(ownerID);
                        string deptUserList = "";

                        if (dsDept != null)
                        {
                            foreach (DataRow drDept in dsDept.Tables[0].Rows)
                            {
                                //실제 회사 아이디로 검사
                                deptUserList = bllUser.GetChildDeptUserList(drDept["CompanyID"].ToString().ToNumber<int>(), ownerID);
                                sendUserIDs += (deptUserList.Length > 0) ? deptUserList + "," : deptUserList;
                            }
                        }
                    }
                    else if (type == "M" && ownerID > 0) //사용자별
                    {
                        sendUserIDs += ownerID.ToString() + ",";
                    }
                }
            }

            return sendUserIDs;
        }

        /// <summary>
        /// 알람보내기
        /// </summary>
        /// <param name="_listInfo"></param>
        /// <param name="_listID"></param>
        /// <param name="_receiverID"></param>
        /// <returns></returns>
        public bool SendAlarmADD(BE.ListInfo _listInfo, int _listID, int _receiverID, int _AlarmType)
        {
            Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo alarmInfo = new Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo();

            alarmInfo.CompanyID = _listInfo.CompanyID;
            alarmInfo.SenderID = _listInfo.WriteUserID;
            alarmInfo.SenderName = _listInfo.WriteUserName;
            alarmInfo.SenderMail = "";
            alarmInfo.SenderPhone = "";
            alarmInfo.ReceiverID = _receiverID;
            alarmInfo.ReceiverName = "";
            alarmInfo.ReceiverMail = "";
            alarmInfo.ReceiverPhone = "";
            alarmInfo.Status = "";
            alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.Subject = _listInfo.Title.ToReverseXSS();
            alarmInfo.Content = _listInfo.AlarmContent.ToReverseXSS();

            string httpType = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "HTTP_TYPE");
            string fullDomain = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "FULLDOMAIN");
            string alarmTargetURL = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "AlarmTargetURL");

            alarmTargetURL = string.Format(alarmTargetURL, httpType, fullDomain);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("categoryID", _listInfo.CategoryID.ToString());
            paraDic.Add("listID", _listID.ToString());

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            alarmTargetURL = alarmTargetURL + "?p=" + encparam;

            alarmInfo.TargetUrl = alarmTargetURL;
            alarmInfo.TargetMobileUrl = "/DeskPlusMobile/Board/AlarmView.aspx?p=" + encparam;
            alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = _AlarmType;
            alarmInfo.KindSubject = "Board";
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            Zeospace.DeskPlusMobile.Web.Messenger.BLL.Alarm biz = new Messenger.BLL.Alarm();
            return biz.Add(alarmInfo);
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-07-16
        /// 작성목적 : 댓글 알람
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_listID"></param>
        /// <param name="_receiverID"></param>
        /// <returns></returns>
        public bool SendAlarmShortReply(BE.ShortReplyInfo _info, int _receiverID, int _AlarmType)
        {
            Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo alarmInfo = new Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo();

            alarmInfo.CompanyID = _info.CompanyID;
            alarmInfo.SenderID = _info.UserID;
            alarmInfo.SenderName = _info.UserName;
            alarmInfo.SenderMail = "";
            alarmInfo.SenderPhone = "";
            alarmInfo.ReceiverID = _receiverID;
            alarmInfo.ReceiverName = "";
            alarmInfo.ReceiverMail = "";
            alarmInfo.ReceiverPhone = "";
            alarmInfo.Status = "";
            alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.Subject = _info.Title.ToReverseXSS();
            alarmInfo.Content = _info.Content.ToReverseXSS();

            string httpType = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "HTTP_TYPE");
            string fullDomain = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "FULLDOMAIN");
            string alarmTargetURL = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "AlarmTargetURL");

            alarmTargetURL = string.Format(alarmTargetURL, httpType, fullDomain);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();

            paraDic.Add("categoryID", _info.CategoryID.ToString());
            paraDic.Add("listID", _info.ListID.ToString());

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            alarmTargetURL = alarmTargetURL + "?p=" + encparam;

            alarmInfo.TargetUrl = alarmTargetURL;
            alarmInfo.TargetMobileUrl = "/DeskPlusMobile/Board/AlarmView.aspx?p=" + encparam;
            alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = _AlarmType;
            alarmInfo.KindSubject = "Board";
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            Zeospace.DeskPlusMobile.Web.Messenger.BLL.Alarm biz = new Messenger.BLL.Alarm();
            return biz.Add(alarmInfo);
        }

        /// <summary>
        /// 다수의 사용자 SMS 발송하기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_loginID"></param>
        /// <param name="_subject"></param>
        /// <param name="_useSendAllUsers"></param>
        /// <param name="_mobilePhone"></param>
        /// <returns></returns>
        public bool SendSMS(int _companyID, int _userID, string _loginID, string _subject, bool _useSendAllUsers, string _mobilePhone)
        {
            if (_subject.Length > 0)
            {
                if (_useSendAllUsers)
                {
                    SendSMS(_companyID, _userID, _loginID, _subject, "", _useSendAllUsers);
                }
                else
                {
                    SendSMS(_companyID, _userID, _loginID, _subject, _mobilePhone, _useSendAllUsers);
                }
            }

            return true;
        }

        /// <summary>
        /// 개별 사용자 SMS 발송하기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_loginID"></param>
        /// <param name="_subject"></param>
        /// <param name="_receiverMobile"></param>
        /// <param name="_useSendAllUsers"></param>
        /// <returns></returns>
        private bool SendSMS(int _companyID, int _userID, string _loginID, string _subject, string _receiverMobile, bool _useSendAllUsers)
        {
            try
            {
                Zeospace.DeskPlusMobile.Web.SMS.SMSInfo smsInfo = new SMS.SMSInfo();
                smsInfo.CompanyID = _companyID;
                smsInfo.UserID = _userID;
                smsInfo.LoginID = _loginID;
                smsInfo.InterfaceID = "";
                smsInfo.Gubun = "";
                smsInfo.Content = _subject;
                smsInfo.ReceivePhone = "";
                smsInfo.Status = "0"; //즉시
                smsInfo.UseSendAllUsers = _useSendAllUsers;//모든 사용자에게 발송여부
                smsInfo.ReceivePhone = _receiverMobile.ToNull().Trim(); //수신자 전화번호

                Account.Data.User userDal = new Account.Data.User();
                DataSet ds = userDal.GetUserCompany(_companyID, _userID);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string sendUserMP = GetMobilePhone(dr["MobilePhone"].ToString());
                    if (sendUserMP.Length > 0)
                    {
                        //발신자 전화번호
                        smsInfo.ReplyPhone1 = sendUserMP.Split(',')[0];
                        smsInfo.ReplyPhone2 = sendUserMP.Split(',')[1];
                        smsInfo.ReplyPhone3 = sendUserMP.Split(',')[2];
                    }

                    smsInfo.WriteUserName = dr["NameBase"].ToString(); //발신자 이름
                }

                //수신자 전화번호
                string recvUserMP = GetMobilePhone(smsInfo.ReceivePhone);
                if (recvUserMP.Length > 0)
                {
                    smsInfo.ReceivePhone1 = recvUserMP.Split(',')[0];
                    smsInfo.ReceivePhone2 = recvUserMP.Split(',')[1];
                    smsInfo.ReceivePhone3 = recvUserMP.Split(',')[2];
                }

                if (smsInfo.UseSendAllUsers == true || smsInfo.ReceivePhone1.Length > 0)
                {
                    Zeospace.DeskPlusMobile.Web.SMS.ControlSendSMS sendSMS = new Zeospace.DeskPlusMobile.Web.SMS.ControlSendSMS();
                    sendSMS.Transfer(smsInfo);
                }

                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("SendSMS", ex);
                throw;
            }
        }

        /// <summary>
        /// 송수신 전화번호 3단 만들기
        /// </summary>
        /// <param name="_mobilePhone">The _mobile phone.</param>
        /// <returns></returns>
        private string GetMobilePhone(string _mobilePhone)
        {
            _mobilePhone = _mobilePhone.Trim();
            if (_mobilePhone.Length == 0) return "";

            string mobile1 = "";
            string mobile2 = "";
            string mobile3 = "";

            if (_mobilePhone.Length > 7)
            {
                _mobilePhone = System.Text.RegularExpressions.Regex.Replace(_mobilePhone, @"[-~.,)(]{1}", "");
                _mobilePhone = _mobilePhone.Replace(" ", "");

                switch (_mobilePhone.Length)
                {
                    case 10:
                        mobile1 = _mobilePhone.Substring(0, 3);
                        mobile2 = _mobilePhone.Substring(3, 3);
                        mobile3 = _mobilePhone.Substring(6, 4);
                        break;

                    case 11:
                        mobile1 = _mobilePhone.Substring(0, 3);
                        mobile2 = _mobilePhone.Substring(3, 4);
                        mobile3 = _mobilePhone.Substring(7, 4);
                        break;

                    default:
                        mobile1 = _mobilePhone.Substring(0, 3);
                        mobile2 = _mobilePhone.Substring(3, 3);
                        mobile3 = _mobilePhone.Substring(6, _mobilePhone.Length - 6);
                        break;
                }

                return mobile1 + "," + mobile2 + "," + mobile3;
            }

            return "";
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : Spool 폴더 파일삭제
        /// 작성일자 : 2015-06-29
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelSpoolFileList(string p)
        {
            SetDecryptString(p);
            string fileName = GetDecryptParam("fileName", "");
            string swfUploadGUID = GetDecryptParam("swfUploadGUID", "");

            try
            {
                // 파일 삭제
                string filePath = "/DeskPlusMobile/FileData/" + CompanyID + "/Users/" + UserID + "/Mobile/" + swfUploadGUID + "/" + fileName;

                if (File.Exists(Server.MapPath(filePath)))
                {
                    System.IO.File.Delete(Server.MapPath(filePath));
                }

                return true;
            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("DelSpoolFileList()", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 임시저장
        /// 작성일자 : 2015-11-09
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string AddDraft(string p)
        {
            SetDecryptString(p);

            string CatHeader = GetDecryptParam("CatHeader", "");
            string fontColor = GetDecryptParam("FontColor", "");
            string fontSize = GetDecryptParam("FontSize", "");
            string isBold = GetDecryptParam("IsBold", "");
            string isOptionModify = GetDecryptParam("IsOptionModify", "");
            string isOptionTop = GetDecryptParam("IsOptionTop", "");
            string isOptionTopDay = GetDecryptParam("IsOptionTopDay", "");
            string isPopUp = GetDecryptParam("IsPopUp", "");
            string isOptionAnonymity = GetDecryptParam("IsOptionAnonymity", "");
            string startDate = GetDecryptParam("StartDate", "");
            string endDate = GetDecryptParam("EndDate", "");
            string title = GetDecryptParam("Title", "");
            string content = GetDecryptParam("Content", "");

            try
            {
                if (UserID <= 0) return "";

                Ws.BE.ListInfo info = new Ws.BE.ListInfo();
                info.CompanyID = CompanyID;
                info.HeaderID = CatHeader.ToNumber<int>();
                info.FontColor = fontColor;
                info.FontSize = fontSize;
                info.Bold = isBold;
                info.OptionModify = isOptionModify;
                info.OptionAnonymity = isOptionAnonymity;
                info.OptionTop = isOptionTop;
                info.OptionTopDay = isOptionTopDay.ToNumber<int>();
                info.PopUp = isPopUp;
                info.StartDate = startDate;
                info.EndDate = endDate;
                info.Title = Server.UrlDecode(title).ToReverseXSS();
                info.Content = Server.UrlDecode(content).ToReverseXSS();
                info.WriteUserID = UserID;

                Data.List dalList = new Data.List();
                int listID = dalList.AddDraft(info);

                return listID.ToString();
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("AddDraft", ex);
                return "";
            }
        }
    }
}