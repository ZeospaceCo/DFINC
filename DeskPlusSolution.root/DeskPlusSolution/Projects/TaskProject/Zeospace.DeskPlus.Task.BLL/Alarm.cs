using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using Zeospace.DeskPlus.Common.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Xml;

namespace Zeospace.DeskPlus.Task.BLL
{
    public class Alarm : Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region 알람

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-29
        /// 작성목적 : 알람보내기
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_isType"></param>
        /// <returns></returns>
        public void SendAlarm(BE.ListInfo _info, int _receiverID, string _isType)
        {
            #region 로직
            /*
             */
            #endregion

            #region 소스

            // 코드관리로 처리
            DataView dvAlarm = AppSettingHelper.GetSysCode(_info.CompanyID, "AlarmTask");

            string AlarmTask = string.Empty;
            string AlarmMail = string.Empty;
            string AlarmMessenger = string.Empty;
            string AlarmMobile = string.Empty;
            string AlarmSMS = string.Empty;

            Zeospace.DeskPlus.Account.DAL.UserEnvironment dal = new Zeospace.DeskPlus.Account.DAL.UserEnvironment();
            List<Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo> listUser = dal.GetUserEnv(_info.CompanyID, _receiverID);

            if (listUser != null)
            {
                foreach (Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo infoUser in listUser)
                {
                    AlarmTask = infoUser.AlarmTask;
                }
            }

            if (AlarmTask.IndexOf("1-").Equals(0))
            {
                AlarmMail = (AlarmTask.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                AlarmMessenger = (AlarmTask.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                AlarmMobile = (AlarmTask.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                AlarmSMS = (AlarmTask.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
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
                            SendMessenger(_info, _receiverID, _isType);
                        }
                        break;
                }
            }

            // 모바일사용의 경우, 무조건 입력
            if (AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "IsMobile", "Y").Equals("Y"))
            {
                SendMobile(_info, _receiverID, _isType);
            }

            #endregion
        }

        /// <summary>
        /// 메신저
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_isType"></param>
        /// <returns></returns>
        private bool SendMessenger(BE.ListInfo _info, int _receiverID, string _isType)
        {
            Zeospace.DeskPlus.Common.BLL.Alarm biz = new Common.BLL.Alarm();

            AlarmInfo alarmInfo = new AlarmInfo();
            alarmInfo.CompanyID = _info.CompanyID;
            alarmInfo.SenderID = _info.WriteUserID;
            alarmInfo.SenderName = _info.SenderName;
            alarmInfo.SenderMail = "";
            alarmInfo.SenderPhone = "";
            alarmInfo.ReceiverID = _receiverID;
            alarmInfo.ReceiverName = "";
            alarmInfo.ReceiverMail = "";
            alarmInfo.ReceiverPhone = "";
            alarmInfo.Status = "";
            alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);

            string[] arrAlarm = AlarmTemplate(_info.Title, _info.PreLang, _isType);
            if (arrAlarm == null) return false;

            alarmInfo.Subject = arrAlarm[0];
            alarmInfo.Content = arrAlarm[1];

            string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
            string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
            string alarmTargetURL = AppSettingHelper.GetAppSetting(AppSettingType.TASK, "AlarmTargetURL", "{0}{1}/DeskPlusEIP/Task/Common/ViewPopupTotal.aspx");

            alarmTargetURL = string.Format(alarmTargetURL, baseHttp, fullDomain);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("listID", _info.ListID.ToString());
            paraDic.Add("groupID", _info.ListID.ToString());

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            alarmTargetURL = alarmTargetURL + "?p=" + encparam;

            alarmInfo.TargetUrl = alarmTargetURL;
            alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = 1;
            alarmInfo.KindSubject = "TASK";
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            return biz.Add(alarmInfo, AlarmEnum.SuSoft.ToString());
        }

        /// <summary>
        /// 모바일
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_isType"></param>
        /// <returns></returns>
        private bool SendMobile(BE.ListInfo _info, int _receiverID, string _isType)
        {
            Zeospace.DeskPlus.Common.BLL.Alarm biz = new Common.BLL.Alarm();

            AlarmInfo alarmInfo = new AlarmInfo();
            alarmInfo.CompanyID = _info.CompanyID;
            alarmInfo.SenderID = _info.WriteUserID;
            alarmInfo.SenderName = _info.SenderName;
            alarmInfo.SenderMail = "";
            alarmInfo.SenderPhone = "";
            alarmInfo.ReceiverID = _receiverID;
            alarmInfo.ReceiverName = "";
            alarmInfo.ReceiverMail = "";
            alarmInfo.ReceiverPhone = "";
            alarmInfo.Status = "";
            alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);

            string[] arrAlarm = AlarmTemplate(_info.Title, _info.PreLang, _isType);
            if (arrAlarm == null) return false;

            alarmInfo.Subject = arrAlarm[0];
            alarmInfo.Content = arrAlarm[1];

            string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
            string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
            string alarmTargetURL = AppSettingHelper.GetAppSetting(AppSettingType.TASK, "AlarmTargetURL", "{0}{1}/DeskPlusEIP/Task/Common/ViewPopupTotal.aspx");

            alarmTargetURL = string.Format(alarmTargetURL, baseHttp, fullDomain);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("listID", _info.ListID.ToString());
            paraDic.Add("groupID", _info.ListID.ToString());

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            alarmTargetURL = alarmTargetURL + "?p=" + encparam;

            string alarmMobileURL = "/DeskPlusMobile/Task/AlarmView.aspx?p=" + encparam;

            alarmInfo.TargetUrl = alarmTargetURL;
            alarmInfo.TargetMobileUrl = alarmMobileURL;
            alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = 4;
            alarmInfo.KindSubject = "TASK";
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            return biz.Add(alarmInfo, AlarmEnum.SuSoft.ToString());
        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-29
        /// 작성목적 : 전달 알람보내기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <param name="_senderID"></param>
        /// <param name="_senderName"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_title"></param>
        /// <param name="_preLang"></param>
        /// <returns></returns>
        public void SendAlarmForward(int _companyID, int _listID, int _senderID, string _senderName, int _receiverID, string _title, string _preLang)
        {
            #region 로직
            /*
             */
            #endregion

            #region 소스

            // 코드관리로 처리
            DataView dvAlarm = AppSettingHelper.GetSysCode(_companyID, "AlarmTask");

            string AlarmTask = string.Empty;
            string AlarmMail = string.Empty;
            string AlarmMessenger = string.Empty;
            string AlarmMobile = string.Empty;
            string AlarmSMS = string.Empty;

            Zeospace.DeskPlus.Account.DAL.UserEnvironment dal = new Zeospace.DeskPlus.Account.DAL.UserEnvironment();
            List<Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo> listUser = dal.GetUserEnv(_companyID, _receiverID);

            if (listUser != null)
            {
                foreach (Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo infoUser in listUser)
                {
                    AlarmTask = infoUser.AlarmTask;
                }
            }

            if (AlarmTask.IndexOf("1-").Equals(0))
            {
                AlarmMail = (AlarmTask.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                AlarmMessenger = (AlarmTask.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                AlarmMobile = (AlarmTask.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                AlarmSMS = (AlarmTask.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
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
                            SendMessengerForward(_companyID, _listID, _senderID, _senderName, _receiverID, _title, _preLang);
                        }
                        break;
                }
            }

            // 모바일사용의 경우, 무조건 입력
            if (AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "IsMobile", "Y").Equals("Y"))
            {
                SendMobileForward(_companyID, _listID, _senderID, _senderName, _receiverID, _title, _preLang);
            }

            #endregion
        }

        /// <summary>
        /// 전달알람 - 메신저
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <param name="_senderID"></param>
        /// <param name="_senderName"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_title"></param>
        /// <param name="_preLang"></param>
        /// <returns></returns>
        private bool SendMessengerForward(int _companyID, int _listID, int _senderID, string _senderName, int _receiverID, string _title, string _preLang)
        {
            Zeospace.DeskPlus.Common.BLL.Alarm biz = new Common.BLL.Alarm();

            AlarmInfo alarmInfo = new AlarmInfo();
            alarmInfo.CompanyID = _companyID;
            alarmInfo.SenderID = _senderID;
            alarmInfo.SenderName = _senderName;
            alarmInfo.SenderMail = "";
            alarmInfo.SenderPhone = "";
            alarmInfo.ReceiverID = _receiverID;
            alarmInfo.ReceiverName = "";
            alarmInfo.ReceiverMail = "";
            alarmInfo.ReceiverPhone = "";
            alarmInfo.Status = "";
            alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);

            string[] arrAlarm = AlarmTemplate(_title, _preLang, "FORWARD");
            if (arrAlarm == null) return false;

            alarmInfo.Subject = arrAlarm[0];
            alarmInfo.Content = arrAlarm[1];

            string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
            string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
            string alarmTargetURL = AppSettingHelper.GetAppSetting(AppSettingType.TASK, "AlarmForwardURL", "{0}{1}/DeskPlusEIP/Task/Common/ViewPopup.aspx");

            alarmTargetURL = string.Format(alarmTargetURL, baseHttp, fullDomain);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("listID", _listID.ToString());
            paraDic.Add("groupID", _listID.ToString());
            paraDic.Add("viewing", "Y");

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            alarmTargetURL = alarmTargetURL + "?p=" + encparam;

            alarmInfo.TargetUrl = alarmTargetURL;
            alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = 1;
            alarmInfo.KindSubject = "TASK";
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            return biz.Add(alarmInfo, AlarmEnum.SuSoft.ToString());
        }

        /// <summary>
        /// 전달알람 - 모바일
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <param name="_senderID"></param>
        /// <param name="_senderName"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_title"></param>
        /// <param name="_preLang"></param>
        /// <returns></returns>
        private bool SendMobileForward(int _companyID, int _listID, int _senderID, string _senderName, int _receiverID, string _title, string _preLang)
        {
            Zeospace.DeskPlus.Common.BLL.Alarm biz = new Common.BLL.Alarm();

            AlarmInfo alarmInfo = new AlarmInfo();
            alarmInfo.CompanyID = _companyID;
            alarmInfo.SenderID = _senderID;
            alarmInfo.SenderName = _senderName;
            alarmInfo.SenderMail = "";
            alarmInfo.SenderPhone = "";
            alarmInfo.ReceiverID = _receiverID;
            alarmInfo.ReceiverName = "";
            alarmInfo.ReceiverMail = "";
            alarmInfo.ReceiverPhone = "";
            alarmInfo.Status = "";
            alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);

            string[] arrAlarm = AlarmTemplate(_title, _preLang, "FORWARD");
            if (arrAlarm == null) return false;

            alarmInfo.Subject = arrAlarm[0];
            alarmInfo.Content = arrAlarm[1];

            string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
            string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
            string alarmTargetURL = AppSettingHelper.GetAppSetting(AppSettingType.TASK, "AlarmForwardURL", "{0}{1}/DeskPlusEIP/Task/Common/ViewPopup.aspx");

            alarmTargetURL = string.Format(alarmTargetURL, baseHttp, fullDomain);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("listID", _listID.ToString());
            paraDic.Add("groupID", _listID.ToString());
            paraDic.Add("viewing", "Y");

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            alarmTargetURL = alarmTargetURL + "?p=" + encparam;

            string alarmMobileURL = "/DeskPlusMobile/Task/AlarmView.aspx?p=" + encparam;

            alarmInfo.TargetUrl = alarmTargetURL;
            alarmInfo.TargetMobileUrl = alarmMobileURL;
            alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = 4;
            alarmInfo.KindSubject = "TASK";
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            return biz.Add(alarmInfo, AlarmEnum.SuSoft.ToString());
        }

        #endregion

        #region 요청알람

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-29
        /// 작성목적 : 요청 알람
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool AlarmRequest(BE.ListInfo _info)
        {
            if (_info == null) return false;

            try
            {
                //수신, 참조
                DAL.ListTO toDal = new DAL.ListTO();
                var dtrTOCC = toDal.GetListAlarm(_info.CompanyID, _info.ListID.ToString());

                if (dtrTOCC != null)
                {
                    if (dtrTOCC.HasRows)
                    {
                        while (dtrTOCC.Read())
                        {
                            SendAlarm(_info, dtrTOCC["UserID"].ToString().ToNumber<int>(), dtrTOCC["IsType"].ToString());
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AlarmRequest (" + _info.ListID.ToString() + ") : " + ex);
                return false;
            }
        }

        #endregion

        #region 알람 (접수, 완료, 재검토)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-29
        /// 작성목적 : 알람 (접수, 완료, 재검토, 답글)
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool AlarmEtc(BE.ListInfo _info, string _gubun)
        {
            if (_info == null) return false;

            try
            {
                //요청자 
                int reuqestUserID = 0;

                DAL.List dalList = new DAL.List();
                var dtr = dalList.GetList(_info.CompanyID, _info.ListID.ToString());

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            _info.Title = dtr["Title"].ToString(); //제목 
                            reuqestUserID = dtr["WriteUserID"].ToString().ToNumber<int>();

                            if (_info.WriteUserID != dtr["WriteUserID"].ToString().ToNumber<int>())
                            {
                                SendAlarm(_info, dtr["WriteUserID"].ToString().ToNumber<int>(), _gubun);
                            }
                        }
                    }
                }

                //수신, 참조
                DAL.ListTO toDal = new DAL.ListTO();
                var dtrTOCC = toDal.GetListAlarmEtc(_info.CompanyID, _info.ListID.ToString());

                if (dtrTOCC != null)
                {
                    if (dtrTOCC.HasRows)
                    {
                        while (dtrTOCC.Read())
                        {
                            if (_info.WriteUserID != dtrTOCC["UserID"].ToString().ToNumber<int>())
                            {
                                if (reuqestUserID != dtrTOCC["UserID"].ToString().ToNumber<int>())
                                {
                                    SendAlarm(_info, dtrTOCC["UserID"].ToString().ToNumber<int>(), _gubun);
                                }
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AlarmEtc (" + _info.ListID.ToString() + ") : " + ex);
                return false;
            }
        }

        #endregion

        #region 전달알람

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-29
        /// 작성목적 : 전달알람
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <param name="_senderID"></param>
        /// <param name="_senderName"></param>
        /// <param name="_userList"></param>
        /// <param name="_preLang"></param>
        /// <returns></returns>
        public bool AlarmForward(int _companyID, int _listID, int _senderID, string _senderName, string _userList, string _preLang)
        {
            try
            {
                //제목 정보 가져오기
                string title = "";
                DAL.List dalList = new DAL.List();
                var dtr = dalList.GetList(_companyID, _listID.ToString());

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            title = dtr["Title"].ToString(); //제목 
                        }
                    }
                }

                foreach (string forwardUserID in _userList.Split(','))
                {
                    if (forwardUserID.ToNumber<int>() > 0)
                    {
                        SendAlarmForward(_companyID, _listID, _senderID, _senderName, forwardUserID.ToNumber<int>(), title, _preLang);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AlarmEtc (" + _listID.ToString() + ") : " + ex);
                return false;
            }
        }

        #endregion

        #region 알람 내용 템플릿

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2015-04-29
        /// 작성목적 : 알람 내용 템플릿
        /// </summary>
        /// <param name="_title"></param>
        /// <param name="_preLang"></param>
        /// <param name="_isType"></param>
        /// <returns></returns>
        private string[] AlarmTemplate(string _title, string _preLang, string _isType)
        {
            try
            {
                XmlDocument confirmXml = null;
                string xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Task/App_Data/" + _preLang + ".xml");
                confirmXml = new XmlDocument();
                confirmXml.Load(xmlPath);
                XmlNode tmpXmlNode = confirmXml.DocumentElement;

                string[] arrRtnval = new string[2];
                arrRtnval[0] = tmpXmlNode.SelectSingleNode("/ALARM/MESSENGER/" + _isType).InnerText + _title.ToReverseXSS();
                arrRtnval[1] = (tmpXmlNode.SelectSingleNode("/ALARM/MESSENGER/ContentHTML" + _isType) != null) ? System.Web.HttpUtility.HtmlDecode(tmpXmlNode.SelectSingleNode("/ALARM/MESSENGER/ContentHTML" + _isType).InnerText) : "";
                arrRtnval[1] = arrRtnval[1].Replace("$SUBJECT$", _title.ToReverseXSS());          //문서제목

                return arrRtnval;   // [0] : Subj / [1] : Content

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Task.AlarmTemplate : " + ex);
                throw;
            }
        }

        #endregion 
    }
}
