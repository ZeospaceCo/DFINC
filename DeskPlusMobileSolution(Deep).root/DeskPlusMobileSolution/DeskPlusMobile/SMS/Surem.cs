using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.Data;
using System.Data.SqlClient;

namespace Zeospace.DeskPlusMobile.Web.SMS
{
    public class Surem
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

                    SMS.SuremInfo suremInfo = new SuremInfo();
                    suremInfo.InDate = System.DateTime.Now.ToString("yyyyMMdd");
                    suremInfo.InTime = System.DateTime.Now.ToString("hhmmss");
                    suremInfo.Member = 0;

                    Account.BLL.User bll = new Zeospace.DeskPlusMobile.Web.Account.BLL.User();
                    Account.BE.UserInfo be = new Zeospace.DeskPlusMobile.Web.Account.BE.UserInfo();
                    be = bll.GetUser(companyID, userID);

                    suremInfo.SendId = be.LogInID; //로긴아이디가져오기
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
                    #endregion

                    #region SMS DB 처리(발송)
                    SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@InDate", SqlDbType.Char,8),
                                                            new SqlParameter("@InTime", SqlDbType.Char,6),
                                                            new SqlParameter("@Member", SqlDbType.Int),
                                                            new SqlParameter("@SendId", SqlDbType.VarChar,16),
                                                            new SqlParameter("@SendName", SqlDbType.VarChar,32),
                                                            new SqlParameter("@Rphone1", SqlDbType.Char,4),
                                                            new SqlParameter("@Rphone2", SqlDbType.Char,4),
                                                            new SqlParameter("@Rphone3", SqlDbType.Char,4),
                                                            new SqlParameter("@RecvName", SqlDbType.VarChar,32),
                                                            new SqlParameter("@Sphone1", SqlDbType.Char,4),
                                                            new SqlParameter("@Sphone2", SqlDbType.Char,4),
                                                            new SqlParameter("@Sphone3", SqlDbType.Char,4),
                                                            new SqlParameter("@Msg", SqlDbType.VarChar,120),
                                                            new SqlParameter("@Url", SqlDbType.VarChar,80),
                                                            new SqlParameter("@Rdate", SqlDbType.Char,8),
                                                            new SqlParameter("@Rtime", SqlDbType.Char,6),
                                                            new SqlParameter("@Result", SqlDbType.Char,1),
                                                            new SqlParameter("@Kind", SqlDbType.Char,1),
                                                            new SqlParameter("@ErrCode", SqlDbType.Int),
                                                            new SqlParameter("@RecvTime", SqlDbType.Char,16)
												          };

                    parms[0].Value = suremInfo.InDate;
                    parms[1].Value = suremInfo.InTime;
                    parms[2].Value = suremInfo.Member;
                    parms[3].Value = suremInfo.SendId;
                    parms[4].Value = suremInfo.SendName;
                    parms[5].Value = suremInfo.Rphone1;
                    parms[6].Value = suremInfo.Rphone2;
                    parms[7].Value = suremInfo.Rphone3;
                    parms[8].Value = suremInfo.RecvName;
                    parms[9].Value = suremInfo.Sphone1;
                    parms[10].Value = suremInfo.Sphone2;
                    parms[11].Value = suremInfo.Sphone3;
                    parms[12].Value = suremInfo.Msg;
                    parms[13].Value = suremInfo.Url;
                    parms[14].Value = suremInfo.Rdate;
                    parms[15].Value = suremInfo.Rtime;
                    parms[16].Value = suremInfo.Result;
                    parms[17].Value = suremInfo.Kind;
                    parms[18].Value = suremInfo.ErrCode;
                    parms[19].Value = suremInfo.RecvTime;

                    Helper.AppException.WriteAssert(AppSettingType.SQLApproval, suremInfo.InDate + "," + suremInfo.InTime + "," + suremInfo.Member + "," + suremInfo.SendId + "," + suremInfo.SendName + "," + suremInfo.Rphone1 + "," + suremInfo.Rphone2 + "," + suremInfo.Rphone3 + "," + suremInfo.RecvName + "," + suremInfo.Sphone1 + "," + suremInfo.Sphone2 + "," + suremInfo.Sphone3 + "," + suremInfo.Msg + "," + suremInfo.Url + "," + suremInfo.Rdate + "," + suremInfo.Rtime + "," + suremInfo.Result + "," + suremInfo.Kind + "," + suremInfo.ErrCode + "," + suremInfo.RecvTime);

                    //모든 회사 사용자에게 SMS 발송
                    if (_smsInfo.UseSendAllUsers)
                    {
                        SQLHelper.ExecuteNonQuery(SQLHelper.CONN_ALARM, CommandType.StoredProcedure, "SQL_SMSData_InsertByAllUsers", parms);
                    }
                    else
                    {
                        SQLHelper.ExecuteNonQuery(SQLHelper.CONN_ALARM, CommandType.StoredProcedure, "SQL_SMSData_Insert", parms);
                    }

                    #endregion

                    rtn = true;
                }

                return rtn;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlusMobile.Web.Helper.AppException.SqlExceptionLog(ex);
                AppException.WriteException("SMS.Surem.Transfer() : " + errMsg);
                throw;
            }
        }
    }

    public class UserSMSDAL
    {
        public string GetSmsCode(string _loginID)
        {
            if (_loginID.Equals("")) return "";

            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@LoginID", SqlDbType.NVarChar, 50) };

            parms[0].Value = _loginID;

            string rtnval = "";
            using (SqlDataReader sdr = SQLHelper.ExecuteReader(SQLHelper.CONN_ALARM, CommandType.StoredProcedure, "SQL_SMSDataEtc_SelectByLoginID", parms))
            {
                if (sdr.Read())
                {
                    rtnval = sdr.GetString(0);
                }
            }

            return rtnval;
        }
    }
}