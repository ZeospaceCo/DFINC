using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Messenger.BE;

namespace Zeospace.DeskPlusMobile.Web.Messenger.Data
{
    public class Alarm
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public string GetLoginIDPwd(int _companyID, int _userID)
        {
            try
            {
                string receiverPwd = "", receiverLoginID = "";

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAccount, "SEL_QRY_USER");
                proc = string.Format(proc, _companyID, _userID);

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, proc, "GetLoginIDPwd", null);

                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        receiverLoginID = dr["LogInID"].ToString();
                        receiverPwd = dr["Password"].ToString();
                    }
                }

                return receiverLoginID + "^" + receiverPwd;
            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("GetLoginIDPwd", ex);
                return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_alarmInfo"></param>
        /// <returns></returns>
        public bool Add(AlarmInfo _alarmInfo)
        {
            SqlParameter[] parms = new SqlParameter[] {
                                               new SqlParameter("@CompanyID", SqlDbType.Int),
											   new SqlParameter("@SenderID", SqlDbType.Int),
											   new SqlParameter("@SenderName", SqlDbType.NVarChar, 50),
											   new SqlParameter("@SenderMail", SqlDbType.NVarChar, 50),
											   new SqlParameter("@SenderPhone", SqlDbType.VarChar, 15),  
											   new SqlParameter("@ReceiverID", SqlDbType.Int),  
											   new SqlParameter("@ReceiverName", SqlDbType.NVarChar, 50),
                                               new SqlParameter("@ReceiverMail", SqlDbType.NVarChar, 50),
                                               new SqlParameter("@ReceiverPhone", SqlDbType.VarChar, 15),
                                               new SqlParameter("@Status", SqlDbType.Char, 1),
                                               new SqlParameter("@SendDate", SqlDbType.DateTime),
                                               new SqlParameter("@Subject", SqlDbType.NVarChar, 70),
                                               new SqlParameter("@Content", SqlDbType.NVarChar, 1000),
                                               new SqlParameter("@TargetUrl", SqlDbType.NVarChar, 500),
                                               new SqlParameter("@TargetMobileUrl", SqlDbType.NVarChar, 500),
                                               new SqlParameter("@StartDate", SqlDbType.DateTime),
                                               new SqlParameter("@EndDate", SqlDbType.DateTime),
                                               new SqlParameter("@AlarmType", SqlDbType.TinyInt),
                                               new SqlParameter("@KindSubject", SqlDbType.NVarChar, 60),
                                               new SqlParameter("@ViewOption", SqlDbType.NVarChar, 2),
                                               new SqlParameter("@Store", SqlDbType.TinyInt),
                                               new SqlParameter("@UserState", SqlDbType.TinyInt),
                                               new SqlParameter("@Random", SqlDbType.VarChar,10),  
                                               new SqlParameter("@MsgSendID", SqlDbType.Int),
                                               new SqlParameter("@AlarmUrl", SqlDbType.NVarChar,500),
                                               new SqlParameter("@ScheduleID", SqlDbType.Int),
                                               new SqlParameter("@DisplayID", SqlDbType.Int)

                };

            parms[0].Value = _alarmInfo.CompanyID;
            parms[1].Value = _alarmInfo.SenderID;
            parms[2].Value = _alarmInfo.SenderName;
            parms[3].Value = _alarmInfo.SenderMail;
            parms[4].Value = _alarmInfo.SenderPhone;
            parms[5].Value = _alarmInfo.ReceiverID;
            parms[6].Value = _alarmInfo.ReceiverName;
            parms[7].Value = _alarmInfo.ReceiverMail;
            parms[8].Value = _alarmInfo.ReceiverPhone;
            parms[9].Value = _alarmInfo.Status;
            parms[10].Value = _alarmInfo.SendDate;
            parms[11].Value = _alarmInfo.Subject;
            parms[12].Value = _alarmInfo.Content;
            parms[13].Value = _alarmInfo.TargetUrl;
            parms[14].Value = _alarmInfo.TargetMobileUrl.ToNull();
            parms[15].Value = _alarmInfo.StartDate;
            parms[16].Value = _alarmInfo.EndDate;
            parms[17].Value = _alarmInfo.AlarmType;
            parms[18].Value = _alarmInfo.KindSubject;
            parms[19].Value = _alarmInfo.ViewOption;
            parms[20].Value = _alarmInfo.Store;
            parms[21].Value = _alarmInfo.UserState;
            parms[22].Value = _alarmInfo.RandomStr;
            parms[23].Value = _alarmInfo.MsgSendID;
            parms[24].Value = _alarmInfo.AlarmUrl;
            parms[25].Value = _alarmInfo.ScheduleID;
            parms[26].Value = _alarmInfo.DisplayID;

            try
            {
                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_ALARM, CommandType.StoredProcedure, "SQL_DP_ALARM_Common_Insert", parms);
                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Add(AlarmInfo): " + errMsg);
                return false;
            }
        }

    }
}