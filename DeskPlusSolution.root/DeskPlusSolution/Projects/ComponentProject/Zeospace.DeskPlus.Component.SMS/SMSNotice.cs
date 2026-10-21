using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Zeospace.DeskPlus.Component.SMS;
using Zeospace.DeskPlus.Base.ApplicationFrameworks;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Component.SMS
{
    public class SMSNotice : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL, ISMS
    {

        /// <summary>
        /// 메서드명 :
        /// 작성목적 : sms 발송
        /// 작성자   : 심재운
        /// </summary>
        /// <param name="_smsInfo">The _SMS info.</param>
        /// <returns></returns>
        public bool Transfer(Zeospace.DeskPlus.Component.SMS.SMSInfo _smsInfo)
        {
            try
            {
                if (_smsInfo != null)
                {
                    string qry = "SQL_DP_ALARM_SMSNotice_Insert";
                    if (_smsInfo.UseSendAllUsers) qry = "SQL_DP_ALARM_InsertByAllUsers";

                    using (var cn = new SqlConnection(ConnAlarm(_smsInfo.CompanyID)))
                    {
                        var cmd = new SqlCommand(qry, cn);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _smsInfo.CompanyID;
						cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _smsInfo.UserID;						
						cmd.Parameters.Add("@SendPhone", SqlDbType.NVarChar, 50).Value = _smsInfo.SendPhone;
						cmd.Parameters.Add("@ReceivePhone", SqlDbType.VarChar, 50).Value = _smsInfo.ReceivePhone;
						cmd.Parameters.Add("@Status", SqlDbType.Char, 1).Value = _smsInfo.Status;
						cmd.Parameters.Add("@SendDate", SqlDbType.DateTime).Value = _smsInfo.SendDate;
						cmd.Parameters.Add("@Subject", SqlDbType.NVarChar, 70).Value = _smsInfo.Subject;
						cmd.Parameters.Add("@Content", SqlDbType.NVarChar, 255).Value = _smsInfo.Content;
						cmd.Parameters.Add("@TargetUrl", SqlDbType.NVarChar, 100).Value = _smsInfo.TargetUrl;
						cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _smsInfo.StartDate;
						cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = _smsInfo.EndDate;
						cmd.Parameters.Add("@InterfaceID", SqlDbType.NVarChar,50).Value = _smsInfo.InterfaceID;
                        
                        cn.Open();

                        ExecuteNonQuery(cmd);
                    }
                }

                return true;

            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Component.SMS.Transfer(): " + errMsg);
                throw;
            }
        }
    }
    
}
