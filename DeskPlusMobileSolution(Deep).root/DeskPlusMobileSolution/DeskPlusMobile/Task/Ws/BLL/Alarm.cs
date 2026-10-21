using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Helper.WebComponent;
using System.Xml;
using System.Data;

namespace Zeospace.DeskPlusMobile.Web.Task.Ws.BLL
{
    public class Alarm
    {

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-05-14
        /// 작성목적 : 알람보내기
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_isType"></param>
        /// <returns></returns>
        public void SendAlarm(BE.ListInfo _info, DataSet _receiverList, string _isType)
        {
            try
            {
                DataView dvTask = XmlDataBaseHelper.GetSysCode(_info.CompanyID, "AlarmTask");

                foreach (DataRowView dr in dvTask)
                {
                    string method = dr["CodeID"].ToString().ToUpper();
                    switch (method)
                    {
                        case "MESSENGER":
                            SendMessenger(_info, _receiverList, _isType);
                            break;
                    }
                }

                // 모바일사용의 경우, 무조건 입력                
                SendMobile(_info, _receiverList, _isType);
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("SendAlarm('" + _info.ListID + "')", ex);
            }
        }

        /// <summary>
        /// 메신저
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_isType"></param>
        private void SendMessenger(BE.ListInfo _info, DataSet _receiverList, string _isType)
        {
            string AlarmTask = string.Empty;
            string AlarmMessenger = string.Empty;

            foreach (DataRow dr in _receiverList.Tables[0].Rows)
            {
                AlarmTask = dr["AlarmTask"].ToString();

                if (AlarmTask.IndexOf("1-").Equals(0))
                    AlarmMessenger = (AlarmTask.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                else
                    AlarmMessenger = "0";

                if (AlarmMessenger.Equals("1"))
                {
                    Zeospace.DeskPlusMobile.Web.Messenger.BLL.Alarm alarm = new Zeospace.DeskPlusMobile.Web.Messenger.BLL.Alarm();

                    Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo alarmInfo = new Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo();
                    alarmInfo.CompanyID = _info.CompanyID;
                    alarmInfo.SenderID = _info.WriteUserID;
                    alarmInfo.SenderName = _info.SenderName;
                    alarmInfo.SenderMail = "";
                    alarmInfo.SenderPhone = "";
                    alarmInfo.ReceiverID = dr["UserID"].ToString().ToNumber<int>();
                    alarmInfo.ReceiverName = "";
                    alarmInfo.ReceiverMail = "";
                    alarmInfo.ReceiverPhone = "";
                    alarmInfo.Status = "";
                    alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);

                    string[] arrAlarm = AlarmTemplate(_info.Title, _info.PreLang, _isType);
                    if (arrAlarm == null) return;

                    alarmInfo.Subject = arrAlarm[0];
                    alarmInfo.Content = arrAlarm[1];

                    string baseHttp = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "HTTP_TYPE");
                    string fullDomain = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "FULLDOMAIN");
                    string alarmTargetURL = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Task, "AlarmTargetURL");

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
                    alarmInfo.AlarmType = 1;    // 알람형식 : 1=메신저 2=메일 3=SMS 4=모바일
                    alarmInfo.KindSubject = "TASK";
                    alarmInfo.ViewOption = "01";
                    alarmInfo.Store = 1;
                    alarmInfo.UserState = 0;

                    alarm.Add(alarmInfo);
                }
            }
        }

        /// <summary>
        /// 모바일
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_isType"></param>
        private void SendMobile(BE.ListInfo _info, DataSet _receiverList, string _isType)
        {
            foreach (DataRow dr in _receiverList.Tables[0].Rows)
            {
                Zeospace.DeskPlusMobile.Web.Messenger.BLL.Alarm alarm = new Zeospace.DeskPlusMobile.Web.Messenger.BLL.Alarm();

                Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo alarmInfo = new Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo();
                alarmInfo.CompanyID = _info.CompanyID;
                alarmInfo.SenderID = _info.WriteUserID;
                alarmInfo.SenderName = _info.SenderName;
                alarmInfo.SenderMail = "";
                alarmInfo.SenderPhone = "";
                alarmInfo.ReceiverID = dr["UserID"].ToString().ToNumber<int>();
                alarmInfo.ReceiverName = "";
                alarmInfo.ReceiverMail = "";
                alarmInfo.ReceiverPhone = "";
                alarmInfo.Status = "";
                alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);

                string[] arrAlarm = AlarmTemplate(_info.Title, _info.PreLang, _isType);
                if (arrAlarm == null) return;

                alarmInfo.Subject = arrAlarm[0];
                alarmInfo.Content = arrAlarm[1];

                string baseHttp = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "HTTP_TYPE");
                string fullDomain = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "FULLDOMAIN");
                string alarmTargetURL = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Task, "AlarmTargetURL");

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
                alarmInfo.AlarmType = 4;    // 알람형식 : 1=메신저 2=메일 3=SMS 4=모바일
                alarmInfo.KindSubject = "TASK";
                alarmInfo.ViewOption = "01";
                alarmInfo.Store = 1;
                alarmInfo.UserState = 0;

                alarm.Add(alarmInfo);
            }

        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-05-14
        /// 작성목적 : 알람 (접수, 완료, 재검토, 답글)
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool AlarmEtc(BE.ListInfo _info, string _gubun)
        {
            if (_info == null) return false;

            try
            {
                DataSet ds = null;

                //요청자 
                if (_info.WriteUserID != _info.RequestUserID)
                {
                    //요청자정보 가져오기
                    ds = GetUserInfo(_info.CompanyID, _info.RequestUserID);

                    if (ds != null)
                    {
                        SendAlarm(_info, ds, _gubun);
                    }
                }

                //수신, 참조
                Data.ListTO dal = new Data.ListTO();
                ds = dal.GetListAlarmEtc(_info.CompanyID, _info.ListID);

                if (ds != null)
                {
                    SendAlarm(_info, ds, _gubun);
                }

                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("AlarmEtc", ex);
                return false;
            }
        }

        #region 요청자 정보
        /// <summary>
        /// 요청자 정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        private DataSet GetUserInfo(int _companyID, int _userID)
        {
            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAccount, "SEL_QRY_ACCOUNTDEPT");
                proc = string.Format(proc, _companyID, _userID);

                return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, proc, "GetUserInfo", null);
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetUserInfo()", ex);
                throw;
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
                string xmlPath = FileHelper.GetServerMapPath("/DeskPlusMobile/XmlData/Task/" + _preLang + ".xml");
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
                Helper.AppException.WriteException("AlarmTemplate", ex);
                throw;
            }
        }

        #endregion
    }
}