using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Xml;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Board.BE;
using Zeospace.DeskPlus.Common.BE;

namespace Zeospace.DeskPlus.Board.BLL
{
    public class Lists : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region GET

        #region 페이지 이동 모듈

        /// <summary>
        /// 작성자 : 심재운
        /// 리스트 이동 페이지 경로 반환(리스트, 중요표시)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <returns></returns>
        public string GoListPage(int _companyID, int _userID, int _categoryID)
        {
            try
            {
                string goPage = "List.aspx";

                DAL.Category dal = new DAL.Category();
                DataSet ds = dal.GetCategory(_companyID, _categoryID);

                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string optionListType = dr["OptionListType"].ToString();//게시판 리스트 형식

                        //리스트 형식을 전체로 설정할 경우, 사용자 설정한 리스트형식으로 반환한다.
                        if (optionListType == "0")
                        {
                            DAL.CategoryUser dalCatUser = new DAL.CategoryUser();

                            //BL : 게시판 TL : 임시저장 IL : 중요표시
                            List<CategoryUserInfo> al = dalCatUser.GetList(_companyID, _categoryID, _userID, "BL");

                            foreach (CategoryUserInfo info in al)
                            {
                                if (info.ListType == "1") goPage = "List.aspx";
                                if (info.ListType == "2") goPage = "BlogList.aspx";
                                if (info.ListType == "3") goPage = "GalleryList.aspx";
                            }
                        }
                        else
                        {
                            if (optionListType == "1") goPage = "List.aspx";
                            if (optionListType == "2") goPage = "BlogList.aspx";
                            if (optionListType == "3") goPage = "GalleryList.aspx";
                        }
                    }
                }

                return goPage;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GoListPage : " + ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 임시글 리스트 이동 페이지 경로 반환
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public string GoListPageTemp(int _companyID, int _userID)
        {
            try
            {
                string goPage = "List.aspx";
                DAL.CategoryUser dal = new DAL.CategoryUser();

                //BL : 게시판 TL : 임시저장 IL : 중요표시
                List<CategoryUserInfo> al = dal.GetList(_companyID, 0, _userID, "TL");
                //리스트 형식을 전체로 설정할 경우, 사용자 설정한 리스트형식으로 반환한다.
                foreach (CategoryUserInfo info in al)
                {
                    if (info.ListType == "1") goPage = "List.aspx";
                    if (info.ListType == "2") goPage = "BlogList.aspx";
                    if (info.ListType == "3") goPage = "GalleryList.aspx";
                }

                return goPage;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GoListPageTemp : " + ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 중요 리스트 이동 페이지 경로 반환
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public string GoListPageImpo(int _companyID, int _userID)
        {
            try
            {
                string goPage = "ListImpo.aspx";
                DAL.CategoryUser dal = new DAL.CategoryUser();

                //BL : 게시판 TL : 임시저장 IL : 중요표시
                List<CategoryUserInfo> al = dal.GetList(_companyID, 0, _userID, "IL");
                //리스트 형식을 전체로 설정할 경우, 사용자 설정한 리스트형식으로 반환한다.
                foreach (CategoryUserInfo info in al)
                {
                    if (info.ListType == "1") goPage = "ListImpo.aspx";
                    if (info.ListType == "2") goPage = "BlogListImpo.aspx";
                    if (info.ListType == "3") goPage = "GalleryListImpo.aspx";
                }

                return goPage;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GoListPageImpo : " + ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 관심대상자 이동 페이지 경로 반환
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public string GoListPageInterested(int _companyID, int _userID)
        {
            try
            {
                string goPage = "ListInterested.aspx";
                DAL.CategoryUser dal = new DAL.CategoryUser();

                //BL : 게시판 TL : 임시저장 IL : 중요표시 IP : 관심대상자
                List<CategoryUserInfo> al = dal.GetList(_companyID, 0, _userID, "IP");
                //리스트 형식을 전체로 설정할 경우, 사용자 설정한 리스트형식으로 반환한다.
                foreach (CategoryUserInfo info in al)
                {
                    if (info.ListType == "1") goPage = "ListInterested.aspx";
                    if (info.ListType == "2") goPage = "BlogListInterested.aspx";
                    if (info.ListType == "3") goPage = "GalleryListInterested.aspx";
                }

                return goPage;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GoListPageInterested : " + ex);
                throw;
            }
        }

        #endregion 페이지 이동 모듈

        #endregion GET

        #region ADD()

        /*
        /// <summary>
        /// 게시물 작성
        /// </summary>
        /// <param name="_listinfo">The _listinfo.</param>
        /// <param name="_etcInfo">The _etc info.</param>
        /// <returns></returns>
        public int Add(BE.ListInfo _listinfo, BE.ListEtcInfo _etcInfo)
        {
            if (_listinfo == null) return -1;

            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.Lists dalList = new DAL.Lists();
                    int listID = dalList.Add(_listinfo);

                    #region 첨부파일 추가

                    if (_listinfo.HiddenSpoolPath != "" && _listinfo.HiddenSpoolPath != null)
                    {
                        HttpContext context = HttpContext.Current;
                        DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_listinfo.HiddenSpoolPath));
                        if (di.Exists)
                        {
                            BLL.AttachFile attachDal = new AttachFile();
                            attachDal.Add(_listinfo.CompanyID, _listinfo.CategoryID, listID, _listinfo.ScreenShot,
                                _listinfo.HiddenSpoolPath, "");
                        }
                    }

                    #endregion 첨부파일 추가

                    DAL.ListEtc dalEtc = new DAL.ListEtc();
                    _etcInfo.ListID = listID;
                    dalEtc.Add(_etcInfo);

                    // 메신저 알림
                    if (!string.IsNullOrEmpty(_listinfo.AlarmContent))
                    {
                        if (_listinfo.AlarmUser == ":0")
                        {
                            SendAlarmUser(_listinfo, listID);
                        }
                        else
                        {
                            DAL.ListAlarm dalAlarm = new DAL.ListAlarm();
                            dalAlarm.Add(_listinfo.CompanyID, _listinfo.CategoryID, listID, _listinfo.AlarmUser, _listinfo.WriteUserID);

                            SendAlarmUserSelect(_listinfo, listID);
                        }
                    }

                    if (!string.IsNullOrEmpty(_listinfo.SMSContent))
                    {
                        SendSMSUser(_listinfo);
                    }

                    ts.Complete();

                    return listID;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add : " + ex);
                throw;
            }
        }
        */

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-03-20
        /// 작성목적 : 게시물 작성 링크URL 추가
        /// </summary>
        /// <param name="_listinfo">The _listinfo.</param>
        /// <param name="_etcInfo">The _etc info.</param>
        /// <returns></returns>
        public int AddLinkUrl(BE.ListInfo _listinfo, BE.ListEtcInfo _etcInfo, List<BE.LinkURLInfo> _linkURLInfo)
        {
            if (_listinfo == null) return -1;

            try
            {
                int listID = 0;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.Lists dalList = new DAL.Lists();
                    listID = dalList.Add(_listinfo);

                    #region 첨부파일 추가
                    if (_listinfo.HiddenSpoolPath != "" && _listinfo.HiddenSpoolPath != null)
                    {
                        HttpContext context = HttpContext.Current;
                        DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_listinfo.HiddenSpoolPath));
                        if (di.Exists)
                        {
                            BLL.AttachFile attachDal = new AttachFile();
                            attachDal.Add(_listinfo.CompanyID, _listinfo.CategoryID, listID, _listinfo.ScreenShot,
                                _listinfo.HiddenSpoolPath, "");
                        }
                    }
                    #endregion 첨부파일 추가

                    #region 링크 URL 추가
                    BLL.LinkURL linkURLDal = new LinkURL();
                    linkURLDal.Add(_listinfo.CompanyID, _listinfo.CategoryID, listID, _linkURLInfo);
                    #endregion 링크 URL 추가

                    DAL.ListEtc dalEtc = new DAL.ListEtc();
                    _etcInfo.ListID = listID;
                    dalEtc.Add(_etcInfo);

                    DAL.ListAlarm dalAlarm = new DAL.ListAlarm();
                    dalAlarm.Add(_listinfo.CompanyID, _listinfo.CategoryID, listID, _listinfo.AlarmUser, _listinfo.WriteUserID);

                    #region 알람발송
                    SendAlarm(_listinfo, listID);
                    #endregion 알람발송

                    ts.Complete();                    
                }

                return listID;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddLinkUrl : " + ex);
                throw;
            }
        }

        #endregion ADD()

        #region MOD()

        /*
        /// <summary>
        /// 작성자 : 심재운
        /// 글수정 (_screenGubun, 0 : 기존파일   1: 신규파일)
        /// </summary>
        /// <param name="_listinfo">The _listinfo.</param>
        /// <param name="_etcInfo">The _etc info.</param>
        /// <param name="_screenGubun">The _screen gubun.</param>
        /// <returns></returns>
        public bool Mod(BE.ListInfo _listinfo, BE.ListEtcInfo _etcInfo, string _screenGubun)
        {
            try
            {
                if (_listinfo != null)
                {
                    using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                    {
                        DAL.Lists dalList = new DAL.Lists();
                        dalList.Mod(_listinfo, _screenGubun);

                        #region 첨부파일 추가
                        if (_listinfo.HiddenSpoolPath != "" && _listinfo.HiddenSpoolPath != null)
                        {
                            HttpContext context = HttpContext.Current;
                            DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_listinfo.HiddenSpoolPath));
                            if (di.Exists)
                            {
                                BLL.AttachFile attachDal = new AttachFile();
                                attachDal.Add(_listinfo.CompanyID, _listinfo.CategoryID, _listinfo.ListID, _listinfo.ScreenShot,
                                    _listinfo.HiddenSpoolPath, _listinfo.WriteDate);
                            }
                        }
                        #endregion 첨부파일 추가

                        DAL.ListEtc dalEtc = new DAL.ListEtc();
                        dalEtc.Mod(_etcInfo);


                        // 메신저 알림
                        DAL.ListAlarm dalAlarm = new DAL.ListAlarm();

                        dalAlarm.Del(_listinfo.CompanyID, _listinfo.CategoryID, _listinfo.ListID);
                        dalAlarm.Add(_listinfo.CompanyID, _listinfo.CategoryID, _listinfo.ListID, _listinfo.AlarmUser, _listinfo.WriteUserID);
          
                        if (!string.IsNullOrEmpty(_listinfo.AlarmContent))
                        {
                            if (_listinfo.AlarmUser == ":0")
                            {
                                SendAlarmUser(_listinfo, _listinfo.ListID);
                            }
                            else
                            {
                                SendAlarmUserSelect(_listinfo, _listinfo.ListID);
                            }
                        }

                        //if (!string.IsNullOrEmpty(_listinfo.SMSContent))
                        //{
                        //    SendSMSUser(_listinfo);
                        //} 

                        ts.Complete();

                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Mod : " + ex);
                throw;
            }
        }
        */

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-03-20
        /// 작성목적 : 게시물 작성 링크URL 수정
        /// </summary>
        /// <param name="_listinfo">The _listinfo.</param>
        /// <param name="_etcInfo">The _etc info.</param>
        /// <param name="_screenGubun">The _screen gubun.</param>
        /// <returns></returns>
        public bool ModLinkUrl(BE.ListInfo _listinfo, BE.ListEtcInfo _etcInfo, string _screenGubun, List<BE.LinkURLInfo> _linkURLInfo)
        {
            try
            {
                if (_listinfo != null)
                {
                    using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                    {
                        DAL.Lists dalList = new DAL.Lists();
                        dalList.Mod(_listinfo, _screenGubun);

                        #region 첨부파일 추가
                        if (_listinfo.HiddenSpoolPath != "" && _listinfo.HiddenSpoolPath != null)
                        {
                            HttpContext context = HttpContext.Current;
                            DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_listinfo.HiddenSpoolPath));
                            if (di.Exists)
                            {
                                BLL.AttachFile attachDal = new AttachFile();
                                attachDal.Add(_listinfo.CompanyID, _listinfo.CategoryID, _listinfo.ListID, _listinfo.ScreenShot,
                                    _listinfo.HiddenSpoolPath, _listinfo.WriteDate);
                            }
                        }
                        #endregion 첨부파일 추가

                        #region 링크 URL 추가
                        BLL.LinkURL linkURLDal = new LinkURL();

                        linkURLDal.Add(_listinfo.CompanyID, _listinfo.CategoryID, _listinfo.ListID, _linkURLInfo);
                        #endregion 링크 URL 추가

                        DAL.ListEtc dalEtc = new DAL.ListEtc();
                        dalEtc.Mod(_etcInfo);


                        // 메신저 알림
                        DAL.ListAlarm dalAlarm = new DAL.ListAlarm();

                        dalAlarm.Del(_listinfo.CompanyID, _listinfo.CategoryID, _listinfo.ListID);
                        dalAlarm.Add(_listinfo.CompanyID, _listinfo.CategoryID, _listinfo.ListID, _listinfo.AlarmUser, _listinfo.WriteUserID);

                        ts.Complete();                        
                    }

                    #region 알람발송
                    SendAlarm(_listinfo, _listinfo.ListID);
                    #endregion 알람발송

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModLinkUrl : " + ex);
                throw;
            }
        }

        #endregion MOD()

        #region DEL()

        /// <summary>
        /// 작성자 : 심재운
        /// 삭제
        /// </summary>
        /// <param name="_listInfo">The _list info.</param>
        /// <returns></returns>
        public bool Del(ListInfo _listInfo)
        {
            try
            {
                if (_listInfo == null)
                {
                    return false;
                }
                else
                {
                    using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                    {
                        //1.내용 삭제
                        DAL.Lists dalList = new DAL.Lists();
                        dalList.Del(_listInfo.CompanyID, _listInfo.ListID);

                        //2. 조회 삭제
                        DAL.ListViewers dalViewer = new DAL.ListViewers();
                        dalViewer.Del(_listInfo.CompanyID, _listInfo.CategoryID, _listInfo.ListID);

                        //3. 짧은 댓글 삭제
                        DAL.ShortReply dalShort = new DAL.ShortReply();
                        dalShort.Del(_listInfo.CompanyID, _listInfo.CategoryID, _listInfo.ListID);

                        //4. 기타 삭제
                        DAL.ListEtc dalEtc = new DAL.ListEtc();
                        dalEtc.Del(_listInfo.CompanyID, _listInfo.CategoryID, _listInfo.ListID);

                        //5. 첨부파일 실제 파일 삭제하기
                        DAL.AttachFiles dalAttach = new DAL.AttachFiles();
                        List<BE.AttachFileInfo> alAttach = dalAttach.GetAttachFileLists(_listInfo.CompanyID, _listInfo.CategoryID, _listInfo.ListID);

                        //5.1. 첨부파일 삭제
                        bool response = dalAttach.Del(_listInfo.CompanyID, _listInfo.CategoryID, _listInfo.ListID);

                        if (response)
                        {
                            foreach (BE.AttachFileInfo info in alAttach)
                            {
                                Base.ApplicationFrameworks.Helper.FileHelper.DelFolder(info.FilePath, false);
                            }
                        }

                        // pdf파일삭제
                        Zeospace.DeskPlus.Common.BLL.CreatePDF pdfBiz = new DeskPlus.Common.BLL.CreatePDF();
                        pdfBiz.DelPdf(_listInfo.CompanyID, "BOARD", _listInfo.ListID);

                        ts.Complete();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("CategoryTx[Del] : " + ex);
                throw;
            }
        }

        #endregion DEL()

        #region 알람

        /// <summary>
        /// 작성목적   : 알람발송
        /// 작성자     : 이지원
        /// 최초작성일 : 2017-05-15
        /// </summary>
        /// <param name="_listInfo">The _list info.</param>
        /// <param name="_listID">The _list ID.</param>
        public void SendAlarm(BE.ListInfo _listinfo, int _listID)
        {
            try
            {
                // 코드관리로 처리
                DataView dvAlarm = AppSettingHelper.GetSysCode(_listinfo.CompanyID, "AlarmBoard");

                foreach (DataRowView dr in dvAlarm)
                {
                    string method = dr["CodeID"].ToString().ToUpper();
                    switch (method)
                    {
                        case "MESSENGER":
                            // 메신저 알림
                            if (!string.IsNullOrEmpty(_listinfo.AlarmContent))
                            {
                                if (_listinfo.AlarmUser == ":0")
                                {
                                    SendAlarmUser(_listinfo, _listID, 1);
                                }
                                else
                                {
                                    SendAlarmUserSelect(_listinfo, _listID, 1);
                                }
                            }
                            break;

                        case "SMS":
                            if (!string.IsNullOrEmpty(_listinfo.SMSContent))
                            {
                                SendSMSUser(_listinfo);
                            }
                            break;
                    }
                }

                // 모바일사용의 경우, 무조건 입력
                if (AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "IsMobile", "Y").Equals("Y"))
                {
                    // 메신저 알림
                    if (!string.IsNullOrEmpty(_listinfo.AlarmContent))
                    {
                        if (_listinfo.AlarmUser == ":0")
                        {
                            SendAlarmUser(_listinfo, _listID, 4);
                        }
                        else
                        {
                            SendAlarmUserSelect(_listinfo, _listID, 4);
                        }
                    }
                }  
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SendAlarm : " + ex);
                throw;
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 알람발송자 명단 가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-24
        /// </summary>
        /// <param name="_listInfo">The _list info.</param>
        /// <param name="_listID">The _list ID.</param>
        public void SendAlarmUser(BE.ListInfo _listInfo, int _listID, int _AlarmType)
        {
            try
            {
                string sendUserIDs = GetReadUser(_listInfo.CompanyID, _listInfo.CategoryID, true);

                DAL.ListAlarm dalAlarm = new DAL.ListAlarm();
                Account.DAL.User userBiz = new Account.DAL.User();

                if (!string.IsNullOrEmpty(sendUserIDs))
                {
                    //전체 알람 발송
                    if (sendUserIDs == "all")
                    {
                        
                        Dictionary<int, string> alUser = new Dictionary<int, string>();

                        if (_listInfo.CompanyID == 0) //전사게시판
                        {
                            Zeospace.DeskPlus.Common.DAL.Company biz = new DeskPlus.Common.DAL.Company();
                            List<Zeospace.DeskPlus.Common.BE.CompanyInfo> alCom = biz.GetCompany(true);
                            foreach (Zeospace.DeskPlus.Common.BE.CompanyInfo infoCom in alCom)
                            {
                                _listInfo.CompanyID = infoCom.CompanyID;

                                //alUser = dalAlarm.GetUserIDList(infoCom.CompanyID); //전체 사용자 가져오기

                                alUser = userBiz.GetUserIDListInfo(infoCom.CompanyID, true); //전체 사용자 가져오기

                                foreach (KeyValuePair<int, string> userID in alUser)
                                {
                                    string AlarmBoard = string.Empty;
                                    string AlarmMail = string.Empty;
                                    string AlarmMessenger = string.Empty;
                                    string AlarmMobile = string.Empty;
                                    string AlarmSMS = string.Empty;

                                    AlarmBoard = userID.Value;

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
                                            SendAlarm(_listInfo, _listID, userID.Key, _AlarmType);
                                        }
                                    }

                                    if (_AlarmType.ToString() == "4")
                                    {
                                        SendAlarm(_listInfo, _listID, userID.Key, _AlarmType);
                                    }
                                }
                            }
                        }
                        else
                        {
                            //alUser = dalAlarm.GetUserIDList(_listInfo.CompanyID); //계열사 사용자 가져오기

                            alUser = userBiz.GetUserIDListInfo(_listInfo.CompanyID, true); //계열사 사용자 가져오기

                            foreach (KeyValuePair<int, string> userID in alUser)
                            {
                                string AlarmBoard = string.Empty;
                                string AlarmMail = string.Empty;
                                string AlarmMessenger = string.Empty;
                                string AlarmMobile = string.Empty;
                                string AlarmSMS = string.Empty;

                                AlarmBoard = userID.Value;

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
                                        SendAlarm(_listInfo, _listID, userID.Key, _AlarmType);
                                    }
                                }

                                if (_AlarmType.ToString() == "4")
                                {
                                    SendAlarm(_listInfo, _listID, userID.Key, _AlarmType);
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
                                List<UserInfo> alUser = userBiz.GetAllUserInfo(_listInfo.CompanyID, _userID.ToNumber<int>(), true, true);

                                foreach (Account.BE.UserInfo info in alUser)
                                {
                                    _listInfo.CompanyID = info.CompanyID;

                                    string AlarmBoard = string.Empty;
                                    string AlarmMail = string.Empty;
                                    string AlarmMessenger = string.Empty;
                                    string AlarmMobile = string.Empty;
                                    string AlarmSMS = string.Empty;

                                    AlarmBoard = info.AlarmBoard;

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
                                            //부서 및 사용자 알람 발송
                                            SendAlarm(_listInfo, _listID, _userID.ToNumber<int>(), _AlarmType);
                                        }
                                    }

                                    if (_AlarmType.ToString() == "4")
                                    {
                                        //부서 및 사용자 알람 발송
                                        SendAlarm(_listInfo, _listID, _userID.ToNumber<int>(), _AlarmType);
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (string _userID in userDis)
                            {
                                List<UserInfo> alUser = userBiz.GetAllUserInfo(_listInfo.CompanyID, _userID.ToNumber<int>(), true, true);

                                foreach (Account.BE.UserInfo info in alUser)
                                {
                                    string AlarmBoard = string.Empty;
                                    string AlarmMail = string.Empty;
                                    string AlarmMessenger = string.Empty;
                                    string AlarmMobile = string.Empty;
                                    string AlarmSMS = string.Empty;

                                    AlarmBoard = info.AlarmBoard;

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
                                            //부서 및 사용자 알람 발송
                                            SendAlarm(_listInfo, _listID, _userID.ToNumber<int>(), _AlarmType);
                                        }
                                    }

                                    if (_AlarmType.ToString() == "4")
                                    {
                                        //부서 및 사용자 알람 발송
                                        SendAlarm(_listInfo, _listID, _userID.ToNumber<int>(), _AlarmType);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SendAlarmUser : " + ex);
                throw;
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 알람발송자 명단 가져오기
        /// 작성자     : 이지원
        /// 최초작성일 : 2017-02-08
        /// </summary>
        /// <param name="_listInfo">The _list info.</param>
        /// <param name="_listID">The _list ID.</param>
        public void SendAlarmUserSelect(BE.ListInfo _listInfo, int _listID, int _AlarmType)
        {
            try
            {
                DAL.ListAlarm dalAlarm = new DAL.ListAlarm();

                Dictionary<int, string> alUser = new Dictionary<int, string>();

                if (_listInfo.CompanyID == 0) //전사게시판
                {
                    Zeospace.DeskPlus.Common.DAL.Company biz = new DeskPlus.Common.DAL.Company();
                    List<Zeospace.DeskPlus.Common.BE.CompanyInfo> alCom = biz.GetCompany(true);
                    foreach (Zeospace.DeskPlus.Common.BE.CompanyInfo infoCom in alCom)
                    {
                        _listInfo.CompanyID = infoCom.CompanyID;

                        alUser = dalAlarm.GetAlarm(0, infoCom.CompanyID, _listInfo.CategoryID, _listID); //전체 사용자 가져오기

                        foreach (KeyValuePair<int, string> userID in alUser)
                        {
                            string AlarmBoard = string.Empty;
                            string AlarmMail = string.Empty;
                            string AlarmMessenger = string.Empty;
                            string AlarmMobile = string.Empty;
                            string AlarmSMS = string.Empty;

                            AlarmBoard = userID.Value;

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
                                    SendAlarm(_listInfo, _listID, userID.Key, _AlarmType);
                                }
                            }

                            if (_AlarmType.ToString() == "4")
                            {
                                SendAlarm(_listInfo, _listID, userID.Key, _AlarmType);
                            }
                        }
                    }
                }
                else
                {
                    alUser = dalAlarm.GetAlarm(_listInfo.CompanyID, _listInfo.CompanyID, _listInfo.CategoryID, _listID); //계열사 사용자 가져오기

                    foreach (KeyValuePair<int, string> userID in alUser)
                    {
                        string AlarmBoard = string.Empty;
                        string AlarmMail = string.Empty;
                        string AlarmMessenger = string.Empty;
                        string AlarmMobile = string.Empty;
                        string AlarmSMS = string.Empty;

                        AlarmBoard = userID.Value;

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
                                SendAlarm(_listInfo, _listID, userID.Key, _AlarmType);
                            }
                        }

                        if (_AlarmType.ToString() == "4")
                        {
                            SendAlarm(_listInfo, _listID, userID.Key, _AlarmType);
                        }
                    }
                }                    
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SendAlarmUserSelect : " + ex);
                throw;
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 알람 보내기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-24
        /// </summary>
        /// <param name="_listInfo">The _list info.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_receiverID">The _receiver ID.</param>
        /// <returns></returns>
        public bool SendAlarm(BE.ListInfo _listInfo, int _listID, int _receiverID, int _AlarmType)
        {
            try
            {

                Zeospace.DeskPlus.Common.BLL.Alarm biz = new Common.BLL.Alarm();

                AlarmInfo alarmInfo = new AlarmInfo();
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

                string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
                string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
                string alarmTargetURL = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "AlarmTargetURL", "");
                alarmTargetURL = string.Format(alarmTargetURL, baseHttp, fullDomain);

                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("categoryID", _listInfo.CategoryID.ToString());
                paraDic.Add("listID", _listID.ToString());

                HttpContext context = HttpContext.Current;
                string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
                alarmTargetURL = alarmTargetURL + "?p=" + encparam;

                string alarmMobileURL = "/DeskPlusMobile/Board/AlarmView.aspx?p=" + encparam;

                alarmInfo.TargetUrl = alarmTargetURL;
                alarmInfo.TargetMobileUrl = alarmMobileURL;
                alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
                alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
                alarmInfo.WriteDate = System.DateTime.Now;
                alarmInfo.AlarmType = _AlarmType;
                alarmInfo.KindSubject = "Board";
                alarmInfo.ViewOption = "01";
                alarmInfo.Store = 1;
                alarmInfo.UserState = 0;

                return biz.Add(alarmInfo, AlarmEnum.SuSoft.ToString());
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SendAlarm : " + ex);
                throw;
            }
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
        public string AlarmContentTemplate(string _catName, string _subject, string _name,
            string _startTime, string _deptName, string _preLang)
        {
            try
            {

                string contHtml = "";
                XmlDocument confirmXml = null;

                string xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Common/App_Data/" + _preLang + "/Board_ContentMessengerInfo.xml");
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
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AlarmContentTemplate : " + ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// SMS 발송하기
        /// </summary>
        /// <param name="_listInfo">The _list info.</param>
        public void SendSMSUser(BE.ListInfo _listInfo)
        {
            try
            {

                string sendUserIDs = GetReadUser(_listInfo.CompanyID, _listInfo.CategoryID, true);

                DAL.ListAlarm dalAlarm = new DAL.ListAlarm();
                Account.DAL.User userBiz = new Account.DAL.User();

                if (!string.IsNullOrEmpty(sendUserIDs))
                {
                    Common.BLL.SMS bll = new Common.BLL.SMS();

                    //전체 알람 발송
                    if (sendUserIDs == "all")
                    {
                        Dictionary<string, string> alUser = new Dictionary<string, string>();

                        if (_listInfo.CompanyID == 0) //전사게시판
                        {
                            Zeospace.DeskPlus.Common.DAL.Company biz = new DeskPlus.Common.DAL.Company();
                            List<Zeospace.DeskPlus.Common.BE.CompanyInfo> alCom = biz.GetCompany(true);
                            foreach (Zeospace.DeskPlus.Common.BE.CompanyInfo infoCom in alCom)
                            {
                                _listInfo.CompanyID = infoCom.CompanyID;

                                alUser = dalAlarm.GetUserSMSList(infoCom.CompanyID);

                                foreach (KeyValuePair<string, string> userID in alUser)
                                {
                                    string AlarmBoard = string.Empty;
                                    string AlarmMail = string.Empty;
                                    string AlarmMessenger = string.Empty;
                                    string AlarmMobile = string.Empty;
                                    string AlarmSMS = string.Empty;

                                    AlarmBoard = userID.Value;

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
                                        bll.SendSMS(_listInfo.CompanyID, _listInfo.WriteUserID, _listInfo.SMSLoginID, _listInfo.SMSContent, false, userID.Key);
                                        //bll.SendSMS(_listInfo.CompanyID, _listInfo.WriteUserID, _listInfo.SMSLoginID,_listInfo.SMSContent, true, _listInfo.SMSMobilePhone);
                                    }
                                }
                            }
                        }
                        else
                        {
                            alUser = dalAlarm.GetUserSMSList(_listInfo.CompanyID);

                            foreach (KeyValuePair<string, string> userID in alUser)
                            {
                                string AlarmBoard = string.Empty;
                                string AlarmMail = string.Empty;
                                string AlarmMessenger = string.Empty;
                                string AlarmMobile = string.Empty;
                                string AlarmSMS = string.Empty;

                                AlarmBoard = userID.Value;

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
                                    bll.SendSMS(_listInfo.CompanyID, _listInfo.WriteUserID, _listInfo.SMSLoginID, _listInfo.SMSContent, false, userID.Key);
                                    //bll.SendSMS(_listInfo.CompanyID, _listInfo.WriteUserID, _listInfo.SMSLoginID, _listInfo.SMSContent, true, _listInfo.SMSMobilePhone);
                                }
                            }
                        }
                    }
                    else //부서 및 사용자 알람 발송
                    {
                        sendUserIDs = sendUserIDs.TrimEnd(',');
                        Account.DAL.User userDal = new Account.DAL.User();

                        List<string> userList = sendUserIDs.Split(',').ToList<string>();
                        IEnumerable<string> userDis = userList.Distinct(); //사용자 중복제거

                        if (_listInfo.CompanyID == 0)
                        {
                            /*
                            userBiz = new Account.DAL.User();

                            foreach (string _userID in userDis)
                            {
                                List<UserInfo> alUser = userBiz.GetAllUserInfo(_listInfo.CompanyID, _userID.ToNumber<int>(), true, true);
                                foreach (Account.BE.UserInfo info in alUser)
                                {
                                    _listInfo.CompanyID = info.CompanyID;

                                    //부서 및 사용자 알람 발송
                                    bll.SendSMS(_listInfo.CompanyID, _listInfo.WriteUserID, _listInfo.SMSLoginID,_listInfo.SMSContent, false, info.MobilePhone);
                                }
                            }
                            */

                            foreach (string _userID in userDis)
                            {
                                List<UserInfo> alUser = userBiz.GetAllUserInfo(_listInfo.CompanyID, _userID.ToNumber<int>(), true, true);
                                foreach (Account.BE.UserInfo info in alUser)
                                {
                                    _listInfo.CompanyID = info.CompanyID;

                                    string AlarmBoard = string.Empty;
                                    string AlarmMail = string.Empty;
                                    string AlarmMessenger = string.Empty;
                                    string AlarmMobile = string.Empty;
                                    string AlarmSMS = string.Empty;

                                    AlarmBoard = info.AlarmBoard;

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
                                        bll.SendSMS(_listInfo.CompanyID, _listInfo.WriteUserID, _listInfo.SMSLoginID, _listInfo.SMSContent, false, info.MobilePhone);
                                        //bll.SendSMS(_listInfo.CompanyID, _listInfo.WriteUserID, _listInfo.SMSLoginID,_listInfo.SMSContent, false, receiverMobile);
                                    }                                    
                                }
                            }
                        }
                        else
                        {
                            /*
                            List<string> mobileList = userDal.GetMobileByUsers(_listInfo.CompanyID, sendUserIDs, true);

                            foreach (string receiverMobile in mobileList)
                            {
                                //부서 및 사용자 알람 발송
                                bll.SendSMS(_listInfo.CompanyID, _listInfo.WriteUserID, _listInfo.SMSLoginID,_listInfo.SMSContent, false, receiverMobile);
                            }
                            */

                            foreach (string _userID in userDis)
                            {
                                List<UserInfo> alUser = userBiz.GetAllUserInfo(_listInfo.CompanyID, _userID.ToNumber<int>(), true, true);

                                foreach (Account.BE.UserInfo info in alUser)
                                {
                                    string AlarmBoard = string.Empty;
                                    string AlarmMail = string.Empty;
                                    string AlarmMessenger = string.Empty;
                                    string AlarmMobile = string.Empty;
                                    string AlarmSMS = string.Empty;

                                    AlarmBoard = info.AlarmBoard;

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
                                        bll.SendSMS(_listInfo.CompanyID, _listInfo.WriteUserID, _listInfo.SMSLoginID, _listInfo.SMSContent, false, info.MobilePhone);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SendSMSUser : " + ex);
                throw;
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 특정 게시판 카테고리 읽기 권한자 (all : 전체, 사용자아이디)
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-24
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_activeDept">if set to <c>true</c> [_active dept].</param>
        /// <returns></returns>
        private string GetReadUser(int _companyID, int _categoryID, bool _activeDept)
        {
            try
            {

                DAL.Authority auth = new DAL.Authority();
                Account.BLL.User bllUser = new Account.BLL.User();
                Account.DAL.Dept dalDept = new Account.DAL.Dept();

                DataTableReader dtr = auth.GetAuthority(_companyID, _categoryID);
                string sendUserIDs = string.Empty;

                if (dtr != null)
                {
                    string type = string.Empty;
                    int ownerID;

                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            type = dtr["type"].ToString();
                            ownerID = dtr["ownerID"].ToString().ToNumber<int>();

                            //회사별
                            if (type == "G" && ownerID == 0)
                            {
                                sendUserIDs = "all";
                                break;
                            }
                            else if (type == "G" && ownerID > 0) //회사별
                            {
                                var alDeptList = dalDept.GetDeptLists(ownerID, _activeDept);
                                string deptUserList = "";
                                foreach (Account.BE.DeptInfo dInfo in alDeptList)
                                {
                                    //실제 회사 아이디로 검사
                                    deptUserList = bllUser.GetChildDeptUserList(dInfo.CompanyID, ownerID, _activeDept);
                                    sendUserIDs += (deptUserList.Length > 0) ? deptUserList + "," : deptUserList;
                                }
                            }
                            else if (type == "M" && ownerID > 0) //사용자별
                            {
                                sendUserIDs += ownerID.ToString() + ",";
                            }
                        }
                    }
                }

                return sendUserIDs;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetReadUser : " + ex);
                throw;
            }
        }

        #endregion 알람
    }

    public class ArraySorter<T>
        where T : IComparable
    {
        static public void SortDescending(T[] array)
        {
            Array.Sort<T>(array, s_Comparer);
        }

        static private ReverseComparer s_Comparer = new ReverseComparer();

        private class ReverseComparer : IComparer<T>
        {
            public int Compare(T object1, T object2)
            {
                return -((IComparable)object1).CompareTo(object2);
            }
        }
    }
}