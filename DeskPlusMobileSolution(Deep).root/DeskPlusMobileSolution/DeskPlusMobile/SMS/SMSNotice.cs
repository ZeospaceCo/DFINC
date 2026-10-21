using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.SMS
{
    public class SMSNotice
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_smsInfo"></param>
        /// <returns></returns>
        public bool Transfer(Zeospace.DeskPlusMobile.Web.SMS.SMSInfo _smsInfo)
        {
            try
            {
                bool rtn = false;

                if (_smsInfo != null)
                {
                    Helper.AppException.WriteAssert(AppSettingType.SQLApproval, "start : SendUserName:" + _smsInfo.WriteUserName + "/ReceiceName:" + _smsInfo.ReceiverName);

                    SqlParameter[] parms = new SqlParameter[]{
																new SqlParameter("@CompanyID", SqlDbType.Int),
																new SqlParameter("@UserID", SqlDbType.Int),
																new SqlParameter("@SMSNoticeID", SqlDbType.Int),
																new SqlParameter("@SendPhone", SqlDbType.NVarChar, 50),
																new SqlParameter("@ReceivePhone", SqlDbType.VarChar, 50),
																new SqlParameter("@Status", SqlDbType.Char, 1),
																new SqlParameter("@SendDate", SqlDbType.DateTime),
																new SqlParameter("@Subject", SqlDbType.NVarChar, 70),
																new SqlParameter("@Content", SqlDbType.NVarChar, 255),
																new SqlParameter("@TargetUrl", SqlDbType.NVarChar, 100),
																new SqlParameter("@StartDate", SqlDbType.DateTime),
																new SqlParameter("@EndDate", SqlDbType.DateTime),
																new SqlParameter("@InterfaceID", SqlDbType.NVarChar,50)
															};

                    parms[0].Value = _smsInfo.CompanyID;
                    parms[1].Value = _smsInfo.UserID;
                    parms[2].Direction = ParameterDirection.Output;
                    parms[3].Value = _smsInfo.SendPhone;
                    parms[4].Value = _smsInfo.ReceivePhone;
                    parms[5].Value = _smsInfo.Status;
                    parms[6].Value = _smsInfo.SendDate;
                    parms[7].Value = _smsInfo.Subject;
                    parms[8].Value = _smsInfo.Content;
                    parms[9].Value = _smsInfo.TargetUrl;
                    parms[10].Value = _smsInfo.StartDate;
                    parms[11].Value = _smsInfo.EndDate;
                    parms[12].Value = _smsInfo.InterfaceID;


                    //모든 회사 사용자에게 SMS 발송
                    if (_smsInfo.UseSendAllUsers)
                    {
                        SQLHelper.ExecuteNonQuery(SQLHelper.CONN_ALARM, CommandType.StoredProcedure, "SQL_DP_ALARM_InsertByAllUsers", parms);
                    }
                    else
                    {
                        SQLHelper.ExecuteNonQuery(SQLHelper.CONN_ALARM, CommandType.StoredProcedure, "SQL_DP_ALARM_SMSNotice_Insert", parms);
                    }

                    rtn = true;
                }

                return rtn;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlusMobile.Web.Helper.AppException.SqlExceptionLog(ex);
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("SMS.SMSNotice.Transfer() : " + errMsg);
                throw;
            }
        }
    }
}