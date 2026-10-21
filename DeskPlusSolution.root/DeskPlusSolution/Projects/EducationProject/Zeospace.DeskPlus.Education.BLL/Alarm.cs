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

namespace Zeospace.DeskPlus.Education.BLL
{
    public class Alarm : Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region 알람

        /// <summary>
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_isType"></param>
        /// <returns></returns>
        public void SendAlarm(BE.ListInfo _info, int _receiverID, string _isType)
        {
            SendMessenger(_info, _receiverID, AlarmTemplate(_info.Title, _isType, LoadAlarmTemplate()));
        }

        /// <summary>
        /// 메신저
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="arrAlarm"></param>
        /// <returns></returns>
        private bool SendMessenger(BE.ListInfo _info, int _receiverID, string[] arrAlarm)
        {
            if (arrAlarm == null) return false;

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

            alarmInfo.Subject = arrAlarm[0];
            alarmInfo.Content = arrAlarm[1];

            string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
            string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
            string alarmTargetURL = "{0}{1}/DeskPlusEIP/Education/Content/ViewPopupTotal.aspx";

            alarmTargetURL = string.Format(alarmTargetURL, baseHttp, fullDomain);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("listID", _info.ListID.ToString());

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            alarmTargetURL = alarmTargetURL + "?p=" + encparam;

            alarmInfo.TargetUrl = alarmTargetURL;
            alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = 1;
            alarmInfo.KindSubject = "EDUCATION";
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            return biz.Add(alarmInfo, AlarmEnum.SuSoft.ToString());
        }

        #endregion

        #region 요청알람

        /// <summary>
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool AlarmRequest(BE.ListInfo _info)
        {
            if (_info == null) return false;

            try
            {
                DAL.ListTO toDal = new DAL.ListTO();
                bool allSucceeded = true;

                using (DataTableReader dtrTO = toDal.GetListAlarm(_info.CompanyID, _info.ListID.ToString()))
                {
                    if (dtrTO != null && dtrTO.HasRows)
                    {
                        XmlDocument templateXml = LoadAlarmTemplate();
                        Dictionary<string, string[]> templates = new Dictionary<string, string[]>();

                        while (dtrTO.Read())
                        {
                            string receiverID = "";

                            try
                            {
                                receiverID = dtrTO["UserID"].ToString();
                                string isType = dtrTO["IsType"].ToString();
                                string[] arrAlarm;
                                if (!templates.TryGetValue(isType, out arrAlarm))
                                {
                                    arrAlarm = AlarmTemplate(_info.Title, isType, templateXml);
                                    templates.Add(isType, arrAlarm);
                                }

                                if (!SendMessenger(_info, receiverID.ToNumber<int>(), arrAlarm))
                                {
                                    allSucceeded = false;
                                    LogAlarmFailure(_info, receiverID, null);
                                }
                            }
                            catch (Exception ex)
                            {
                                allSucceeded = false;
                                LogAlarmFailure(_info, receiverID, ex);
                            }
                        }
                    }
                }

                return allSucceeded;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AlarmRequest (" + _info.ListID.ToString() + ") : " + ex);
                return false;
            }
        }

        private void LogAlarmFailure(BE.ListInfo _info, string receiverID, Exception exception)
        {
            try
            {
                string message = "AlarmRequest failed. CompanyID=" + _info.CompanyID.ToString()
                    + ", ListID=" + _info.ListID.ToString() + ", UserID=" + receiverID;

                if (exception == null)
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(message);
                else
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(message, exception);
            }
            catch (Exception)
            {
                // 로그 기록 실패도 나머지 수신자의 알람 처리를 중단하지 않도록 합니다.
            }
        }

        #endregion

        #region 알람 내용 템플릿

        /// <summary>
        /// 알람 처리마다 XML 파일을 한 번 읽고 같은 처리 내에서 재사용합니다.
        /// </summary>
        private XmlDocument LoadAlarmTemplate()
        {
            XmlDocument templateXml = new XmlDocument();
            string xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Education/App_Data/Ko.xml");
            templateXml.Load(xmlPath);
            return templateXml;
        }

        /// <summary>
        /// 이미 읽은 XML에서 알람 유형별 제목과 본문을 생성합니다.
        /// </summary>
        /// <param name="_title"></param>
        /// <param name="_isType"></param>
        /// <param name="templateXml"></param>
        /// <returns></returns>
        private string[] AlarmTemplate(string _title, string _isType, XmlDocument templateXml)
        {
            try
            {
                XmlNode tmpXmlNode = templateXml.DocumentElement;

                string[] arrRtnval = new string[2];
                arrRtnval[0] = tmpXmlNode.SelectSingleNode("/ALARM/MESSENGER/" + _isType).InnerText + _title.ToReverseXSS();
                arrRtnval[1] = (tmpXmlNode.SelectSingleNode("/ALARM/MESSENGER/ContentHTML" + _isType) != null) ? System.Web.HttpUtility.HtmlDecode(tmpXmlNode.SelectSingleNode("/ALARM/MESSENGER/ContentHTML" + _isType).InnerText) : "";
                arrRtnval[1] = arrRtnval[1].Replace("$SUBJECT$", _title.ToReverseXSS());        

                return arrRtnval;   

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Eductaion.AlarmTemplate : " + ex);
                throw;
            }
        }

        #endregion 
    }
}
