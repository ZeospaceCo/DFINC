using System;
using System.Collections.Generic;
using System.Transactions;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data;
using System.Web;

namespace Zeospace.DeskPlus.SNS.BLL
{
    public class BoardTalk : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 댓글 저장
        /// </summary>
        /// <param name="_shortinfo">The _shortinfo.</param>
        /// <returns></returns>
        public bool Add(BE.BoardTalkInfo _shortinfo, string _preLang)
        {
            if (_shortinfo == null) return false;

            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    //짧은 댓글 저장
                    DAL.BoardTalk dalSr = new DAL.BoardTalk();
                    dalSr.Add(_shortinfo);
             
                    ts.Complete();
                }

                SendAlarm(_shortinfo, _preLang);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add : " + ex);
                return false;
            }
        }
        #endregion ADD()

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2015-10-20
        /// 작성목적 : 알람
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_preLang"></param>
        /// <returns></returns>
        public void SendAlarm(BE.BoardTalkInfo _shortinfo, string _preLang)
        {
            // 코드관리로 처리
            DataView dvAlarm = AppSettingHelper.GetSysCode(_shortinfo.CompanyID, "AlarmSNS");
            if (dvAlarm.Count.Equals(0)) return;

            Zeospace.DeskPlus.Board.BLL.Lists listBiz = new Zeospace.DeskPlus.Board.BLL.Lists();
            Zeospace.DeskPlus.Board.DAL.Lists listDal = new Zeospace.DeskPlus.Board.DAL.Lists();

            var list = listDal.GetList(_shortinfo.CompanyID, _shortinfo.CategoryID, _shortinfo.ListID);

            int recvUserID = 0;
            string recvName = "";
            string recvTitle = "";

            //원본글 작성자에게 알람 발송하기
            foreach (var listInfo in list)
            {
                recvUserID = listInfo.WriteUserID; //받을 사람 (원본글 작성자 아이디)
                recvName = UtilHelper.GetLangName(
                    listInfo.NameBase.ToReverseXSS(), listInfo.NameKo.ToReverseXSS(), listInfo.NameEn.ToReverseXSS(), listInfo.NameJa.ToReverseXSS(),
                    listInfo.NameFr.ToReverseXSS(), listInfo.NameZhcn.ToReverseXSS(), listInfo.NameZhtw.ToReverseXSS(), _preLang);

                recvTitle = listInfo.Title.ToReverseXSS(); //제목

                listInfo.WriteUserID = _shortinfo.UserID; //보내는 사용자 ID
                listInfo.WriteUserName = _shortinfo.UserName;//보내는 사람
                listInfo.AlarmContent = _shortinfo.Content; //알람내용

                Zeospace.DeskPlus.Account.DAL.UserEnvironment dal = new Zeospace.DeskPlus.Account.DAL.UserEnvironment();
                List<Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo> listUser = dal.GetUserEnv(listInfo.CompanyID, listInfo.WriteUserID);

                string AlarmSNS = string.Empty;
                string AlarmMail = string.Empty;
                string AlarmMessenger = string.Empty;
                string AlarmMobile = string.Empty;
                string AlarmSMS = string.Empty;

                if (listUser != null)
                {
                    foreach (Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo infoUser in listUser)
                    {
                        AlarmSNS = infoUser.AlarmSNS;
                    }
                }

                if (AlarmSNS.IndexOf("1-").Equals(0))
                {
                    AlarmMail = (AlarmSNS.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                    AlarmMessenger = (AlarmSNS.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                    AlarmMobile = (AlarmSNS.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                    AlarmSMS = (AlarmSNS.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                }
                else
                {
                    AlarmMessenger = "0";
                    AlarmMail = "0";
                    AlarmMobile = "0";
                    AlarmSMS = "0";
                }

                foreach (DataRowView dr in dvAlarm)
                {
                    string method = dr["CodeID"].ToString().ToUpper();
                    switch (method)
                    {
                        case "MESSENGER":
                            if (AlarmMessenger.Equals("1"))
                            {
                                SendMessenger(listInfo, listInfo.ListID, recvUserID);
                            }
                            break;
                    }
                }

                // 모바일사용의 경우, 무조건 입력
                if (AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "IsMobile", "Y").Equals("Y"))
                {
                    SendMobile(listInfo, listInfo.ListID, recvUserID);
                } 

            }

            //짧은댓글 기재자들에게 알람 발송하기
            DAL.BoardTalk dalSr = new DAL.BoardTalk();
            var dtr = dalSr.GetShortReplyList(_shortinfo.CompanyID, _shortinfo.CategoryID, _shortinfo.ListID);

            //짧은댓글 작성자 아이디 정보
            List<int> userList = new List<int>();

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        //짧은댓글 작성자와 원본작성자와 동일하면 보내지 않는다.
                        if (recvUserID != dtr["UserID"].ToString().ToNumber<int>())
                        {
                            //다중 댓글 중복자에게 보내지 않도록 한다. 1통만 보내자.~~~~
                            if (!userList.Exists(c => c == dtr["UserID"].ToString().ToNumber<int>()))
                            {
                                string AlarmSNS = dtr["AlarmSNS"].ToString();
                                string AlarmMail = string.Empty;
                                string AlarmMessenger = string.Empty;
                                string AlarmMobile = string.Empty;
                                string AlarmSMS = string.Empty;

                                if (AlarmSNS.IndexOf("1-").Equals(0))
                                {
                                    AlarmMail = (AlarmSNS.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                                    AlarmMessenger = (AlarmSNS.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                                    AlarmMobile = (AlarmSNS.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                                    AlarmSMS = (AlarmSNS.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                                }
                                else
                                {
                                    AlarmMessenger = "0";
                                    AlarmMail = "0";
                                    AlarmMobile = "0";
                                    AlarmSMS = "0";
                                }

                                Zeospace.DeskPlus.Board.BE.ListInfo listInfo = new Zeospace.DeskPlus.Board.BE.ListInfo();

                                listInfo.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                                listInfo.CategoryID = dtr["CategoryID"].ToString().ToNumber<int>();
                                listInfo.ListID = dtr["ListID"].ToString().ToNumber<int>();

                                listInfo.Title = recvTitle; //알람 제목
                                listInfo.AlarmContent = _shortinfo.Content; //알람내용
                                listInfo.WriteUserID = recvUserID; //보내는 사용자 ID (게시판 작성자)
                                listInfo.WriteUserName = recvName;//보내는 사람   (게시판 작성자 이름)

                                userList.Add(dtr["UserID"].ToString().ToNumber<int>());

                                foreach (DataRowView dr in dvAlarm)
                                {
                                    string method = dr["CodeID"].ToString().ToUpper();
                                    switch (method)
                                    {
                                        case "MESSENGER":
                                            if (AlarmMessenger.Equals("1"))
                                            {
                                                SendMessenger(listInfo, listInfo.ListID, dtr["UserID"].ToString().ToNumber<int>());    
                                            }
                                            break;
                                    }
                                }

                                // 모바일사용의 경우, 무조건 입력
                                if (AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "IsMobile", "Y").Equals("Y"))
                                {
                                    SendMobile(listInfo, listInfo.ListID, dtr["UserID"].ToString().ToNumber<int>());    
                                }                                                              
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 메신져 보내기
        /// 작성자     : 이지원
        /// 최초작성일 : 2017-04-28
        /// </summary>
        /// <param name="_listInfo">The _list info.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_receiverID">The _receiver ID.</param>
        /// <returns></returns>
        public bool SendMessenger(Board.BE.ListInfo _listInfo, int _listID, int _receiverID)
        {
            try
            {
                Zeospace.DeskPlus.Common.BLL.Alarm biz = new Common.BLL.Alarm();

                Zeospace.DeskPlus.Common.BE.AlarmInfo alarmInfo = new Zeospace.DeskPlus.Common.BE.AlarmInfo();
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

                alarmInfo.TargetUrl = alarmTargetURL;
                alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
                alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
                alarmInfo.WriteDate = System.DateTime.Now;
                alarmInfo.AlarmType = 1;
                alarmInfo.KindSubject = "SNS";
                alarmInfo.ViewOption = "01";
                alarmInfo.Store = 1;
                alarmInfo.UserState = 0;

                return biz.Add(alarmInfo, Zeospace.DeskPlus.Common.BE.AlarmEnum.SuSoft.ToString());
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SendAlarm : " + ex);
                throw;
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 모바일 보내기
        /// 작성자     : 이지원
        /// 최초작성일 : 2017-04-28
        /// </summary>
        /// <param name="_listInfo">The _list info.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_receiverID">The _receiver ID.</param>
        /// <returns></returns>
        public bool SendMobile(Board.BE.ListInfo _listInfo, int _listID, int _receiverID)
        {
            try
            {

                Zeospace.DeskPlus.Common.BLL.Alarm biz = new Common.BLL.Alarm();

                Zeospace.DeskPlus.Common.BE.AlarmInfo alarmInfo = new Zeospace.DeskPlus.Common.BE.AlarmInfo();
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
                alarmInfo.AlarmType = 4;
                alarmInfo.KindSubject = "SNS";
                alarmInfo.ViewOption = "01";
                alarmInfo.Store = 1;
                alarmInfo.UserState = 0;

                return biz.Add(alarmInfo, Zeospace.DeskPlus.Common.BE.AlarmEnum.SuSoft.ToString());
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SendAlarm : " + ex);
                throw;
            }
        }
    }
}