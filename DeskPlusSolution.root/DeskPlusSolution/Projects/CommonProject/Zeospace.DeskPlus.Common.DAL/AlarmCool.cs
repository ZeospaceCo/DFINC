using System;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Common.DAL
{
    public class AlarmCool : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region 생성자

        public AlarmCool()
        {
        }

        #endregion 생성자

        #region ADD()

        /// <summary>
        /// 알람 발송
        /// </summary>
        /// <returns></returns>
        public int Send()
        {
            try
            {
                int companyID = 1;
                string senderID = String.Empty;
                string receiveID = String.Empty;
                string subject = String.Empty;
                string content = String.Empty;
                string targetURL = String.Empty;
                string messageData = AppSettingHelper.GetAppSetting(AppSettingType.MESSENGER, "MessageData", "");
                uint messageKind = Convert.ToUInt32(AppSettingHelper.GetAppSetting(AppSettingType.MESSENGER, "MessageKind", ""));

                Interop.WebLink.SendData sendData = new Interop.WebLink.SendData();
                sendData.ServerIP = AppSettingHelper.GetAppSetting(AppSettingType.MESSENGER, "ServerIP", "");
                sendData.ServerPort = Convert.ToInt32(AppSettingHelper.GetAppSetting(AppSettingType.MESSENGER, "ServerPort", ""));

                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ALARM, "QRY_ALARM_Common_SelBySendDate");

                DataSet ds = SQLHelper.ExecuteDataSet(ConnAlarm(companyID), CommandType.Text, query, "dsAlarm", null);
                AlarmCommon bizComm = new AlarmCommon();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    companyID = Convert.ToInt32(dr["CompanyID"]);
                    senderID = dr["SenderID"].ToString();
                    receiveID = dr["ReceiverID"].ToString();

                    senderID = bizComm.GetLoginID(companyID, Convert.ToInt32(senderID));
                    receiveID = bizComm.GetLoginID(companyID, Convert.ToInt32(receiveID));

                    subject = dr["Subject"].ToString();
                    content = dr["Content"].ToString();

                    subject = subject.Replace("<", "&lt;").Replace(">", "&gt;");
                    content = content.Replace("<", "&lt;").Replace(">", "&gt;");

                    targetURL = dr["TargetURL"].ToString();

                    messageData = messageData.Replace("{", "<");
                    messageData = messageData.Replace("}", ">");
                    messageData = messageData.Replace("[SUBJECT]", subject); //제목
                    messageData = messageData.Replace("[CONTENT]", content); // 호출 페이지
                    messageData = messageData.Replace("[TARGETURL]", targetURL); // 호출 페이지
                    messageData = messageData.Replace("[SENDERID]", senderID); // 호출 페이지
                    messageData = messageData.Replace("[RECEIVERID]", receiveID); // 호출 페이지

                    object result = sendData.SendToCOOL(messageKind, 1, 0, 0, messageData);

                    AppException.WriteAssert("Cool Messenger ReturnValue : " + result.ToString() + " AlarmData : " + messageData, AppSettingType.MESSENGER);

                    if (!bizComm.Del(companyID, Convert.ToInt32(dr["SenderID"]), Convert.ToInt32(dr["AlarmID"])))
                    {
                        AppException.WriteAssert("Cool Messenger [Alarm Data Not Deleted] : SenderID " + Convert.ToInt32(dr["SenderID"]) + " AlarmID : " + Convert.ToInt32(dr["AlarmID"]), AppSettingType.MESSENGER);
                    }
                }
                return 1;
            }
            catch (SqlException ex)
            {
                AppException.WriteException("Zeospace.DeskPlus.Messenger.DALBCHS.Alarm[Send]", ex);
                return -1;
            }
        }

        #endregion ADD()
    }
}