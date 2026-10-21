using System;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Common.BE;

namespace Zeospace.DeskPlus.Common.DAL
{
    public class SMSSurem : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        /// <summary>
        /// 메서드명 :
        /// 작성목적 : sms 발송
        /// 작성자   : 심재운
        /// </summary>
        /// <param name="_smsInfo">The _SMS info.</param>
        /// <returns></returns>
        public bool Transfer(SMSInfo _smsInfo)
        {
            try
            {
                bool rtn = false;

                if (_smsInfo != null)
                {
                    #region Sms Data 처리

                    int companyID = _smsInfo.CompanyID;
                    int userID = _smsInfo.UserID;
                    string interfaceID = _smsInfo.InterfaceID;
                    int smsNoticeID = _smsInfo.SMSNoticeID;
                    string sendPhone = _smsInfo.SendPhone;
                    string receivePhone = _smsInfo.ReceivePhone;

                    string receivePhone1 = _smsInfo.ReceivePhone1;
                    string receivePhone2 = _smsInfo.ReceivePhone2;
                    string receivePhone3 = _smsInfo.ReceivePhone3;

                    string replyPhone1 = _smsInfo.ReplyPhone1;
                    string replyPhone2 = _smsInfo.ReplyPhone2;
                    string replyPhone3 = _smsInfo.ReplyPhone3;

                    string status = _smsInfo.Status;
                    DateTime sendDate = _smsInfo.SendDate;
                    string sendTime = _smsInfo.SendTime;

                    string subject = _smsInfo.Subject;
                    string content = _smsInfo.Content;
                    string targetUrl = _smsInfo.TargetUrl;
                    DateTime startDate = _smsInfo.StartDate;
                    DateTime endDate = _smsInfo.EndDate;

                    SMSSuremInfo suremInfo = new SMSSuremInfo();
                    suremInfo.InDate = System.DateTime.Now.ToString("yyyyMMdd");
                    suremInfo.InTime = System.DateTime.Now.ToString("hhmmss");
                    suremInfo.Member = userID;

                    suremInfo.SendId = _smsInfo.LoginID; //로긴아이디가져오기
                    suremInfo.SendName = (_smsInfo.WriteUserName == null) ? "" : _smsInfo.WriteUserName;
                    suremInfo.Rphone1 = receivePhone1;
                    suremInfo.Rphone2 = receivePhone2;
                    suremInfo.Rphone3 = receivePhone3;
                    suremInfo.RecvName = (_smsInfo.ReceiverName == null) ? "" : _smsInfo.ReceiverName;
                    suremInfo.Sphone1 = (string.IsNullOrEmpty(replyPhone1)) ? "" : replyPhone1.Trim();
                    suremInfo.Sphone2 = (string.IsNullOrEmpty(replyPhone2)) ? "" : replyPhone2.Trim();
                    suremInfo.Sphone3 = (string.IsNullOrEmpty(replyPhone3)) ? "" : replyPhone3.Trim();
                    suremInfo.Msg = content.Trim();
                    suremInfo.Url = "";

                    //0 : 즉시, 1: 예약
                    if (status.Equals("0"))
                    {
                        suremInfo.Rdate = "00000000";
                        suremInfo.Rtime = "000000";
                    }
                    else
                    {
                        suremInfo.Rdate = sendDate.ToString("yyyyMMdd");
                        suremInfo.Rtime = sendTime.Trim();
                    }
                    suremInfo.Result = "0"; //대기
                    suremInfo.Kind = "S"; //S는 sms 전송
                    suremInfo.ErrCode = 0; //error 코드
                    suremInfo.RecvTime = ""; // 응답시간

                    #endregion Sms Data 처리

                    #region SMS DB 처리(발송)

                    using (SqlConnection cn = new SqlConnection(ConnAlarm(companyID)))
                    {
                        string sp = (_smsInfo.UseSendAllUsers) ? "SQL_SMSData_InsertByAllUsers" : "SQL_SMSData_Insert";
                        SqlCommand cmd = new SqlCommand(sp, cn);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@InDate", SqlDbType.Char, 8).Value = suremInfo.InDate.ToNull().ToXSS();
                        cmd.Parameters.Add("@InTime", SqlDbType.Char, 6).Value = suremInfo.InTime.ToNull().ToXSS();
                        cmd.Parameters.Add("@Member", SqlDbType.Int).Value = suremInfo.Member;
                        cmd.Parameters.Add("@SendId", SqlDbType.VarChar, 16).Value = suremInfo.SendId.ToNull().ToXSS();
                        cmd.Parameters.Add("@SendName", SqlDbType.VarChar, 32).Value = suremInfo.SendName.ToNull().ToXSS();
                        cmd.Parameters.Add("@Rphone1", SqlDbType.Char, 4).Value = suremInfo.Rphone1.ToNull().ToXSS();
                        cmd.Parameters.Add("@Rphone2", SqlDbType.Char, 4).Value = suremInfo.Rphone2.ToNull().ToXSS();
                        cmd.Parameters.Add("@Rphone3", SqlDbType.Char, 4).Value = suremInfo.Rphone3.ToNull().ToXSS();
                        cmd.Parameters.Add("@RecvName", SqlDbType.VarChar, 32).Value = suremInfo.RecvName.ToNull().ToXSS();
                        cmd.Parameters.Add("@Sphone1", SqlDbType.Char, 4).Value = suremInfo.Sphone1.ToNull().ToXSS();
                        cmd.Parameters.Add("@Sphone2", SqlDbType.Char, 4).Value = suremInfo.Sphone2.ToNull().ToXSS();
                        cmd.Parameters.Add("@Sphone3", SqlDbType.Char, 4).Value = suremInfo.Sphone3.ToNull().ToXSS();
                        cmd.Parameters.Add("@Msg", SqlDbType.VarChar, 120).Value = suremInfo.Msg.ToNull().ToXSS();
                        cmd.Parameters.Add("@Url", SqlDbType.VarChar, 80).Value = suremInfo.Url.ToNull().ToXSS();
                        cmd.Parameters.Add("@Rdate", SqlDbType.Char, 8).Value = suremInfo.Rdate.ToNull().ToXSS();
                        cmd.Parameters.Add("@Rtime", SqlDbType.Char, 6).Value = suremInfo.Rtime.ToNull().ToXSS();
                        cmd.Parameters.Add("@Result", SqlDbType.Char, 1).Value = suremInfo.Result.ToNull().ToXSS();
                        cmd.Parameters.Add("@Kind", SqlDbType.Char, 1).Value = suremInfo.Kind.ToNull().ToXSS();
                        cmd.Parameters.Add("@ErrCode", SqlDbType.Int).Value = suremInfo.ErrCode;
                        cmd.Parameters.Add("@RecvTime", SqlDbType.Char, 16).Value = suremInfo.RecvTime.ToNull().ToXSS();

                        cn.Open();

                        ExecuteNonQuery(cmd);
                    }

                    #endregion SMS DB 처리(발송)

                    rtn = true;
                }

                return rtn;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }
    }
}