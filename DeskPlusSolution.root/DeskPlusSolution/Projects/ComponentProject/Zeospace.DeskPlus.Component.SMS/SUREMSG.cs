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
    public class SUREMSG : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL, ISMS
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

                    SMS.SuremInfo suremInfo = new SuremInfo();
                    suremInfo.InDate = System.DateTime.Now.ToString("yyyyMMdd");
                    suremInfo.InTime = System.DateTime.Now.ToString("HHmmss");

                    Account.BLL.User bll = new Zeospace.DeskPlus.Account.BLL.User();

                    #region 대표계정/개인개정 구분 (BySite : Hintw)
                    string representKey = AppSettingHelper.GetAppSetting(AppSettingType.SMS, "RepresentKey", "");
                    if (representKey.Equals(""))
                    {
                        UserSMSDAL uSms = new UserSMSDAL();
                        suremInfo.SendId = uSms.GetSmsCode(_smsInfo.LoginID);
                    }
                    else
                    {
                        suremInfo.SendId = representKey;
                    }
                    #endregion

                    suremInfo.Member = userID;

                    //suremInfo.SendId = be.LogInID; //로긴아이디가져오기
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

                    try
                    {
                        string query = "SQL_SUREMSG_Insert";
                        //모든 회사 사용자에게 SMS 발송
                        if (_smsInfo.UseSendAllUsers) query = "SQL_SUREMSG_InsertByAllUsers";

                        using (var cn = new SqlConnection(ConnAlarm(_smsInfo.CompanyID)))
                        {
                            var cmd = new SqlCommand(query, cn);
                            cmd.CommandType = CommandType.StoredProcedure;

                            //회사 아이디
                            cmd.Parameters.Add("@COMPANYID", SqlDbType.Int).Value = _smsInfo.CompanyID;
                            //송신자 아이디
                            cmd.Parameters.Add("@USERCODE", SqlDbType.VarChar, 16).Value = _smsInfo.LoginID;
                            //SMS 요청시간
                            cmd.Parameters.Add("@INTIME", SqlDbType.Char, 14).Value = suremInfo.InDate + suremInfo.InTime;
                            //예약시간
                            cmd.Parameters.Add("@REQTIME", SqlDbType.Char, 14).Value = suremInfo.Rdate + suremInfo.Rtime;
                            //수신자명
                            cmd.Parameters.Add("@RECVNAME", SqlDbType.VarChar, 32).Value = suremInfo.RecvName;
                            //수신번호
                            cmd.Parameters.Add("@CALLPHONE", SqlDbType.Char, 12).Value = suremInfo.Rphone1 + suremInfo.Rphone2 + suremInfo.Rphone3;
                            //회신자명
                            cmd.Parameters.Add("@SENDNAME", SqlDbType.VarChar, 32).Value = suremInfo.SendName;
                            //회신자번호
                            cmd.Parameters.Add("@REQPHONE", SqlDbType.Char, 12).Value = suremInfo.Sphone1 + suremInfo.Sphone2 + suremInfo.Sphone3;
                            //메세지
                            cmd.Parameters.Add("@MSG", SqlDbType.VarChar, 4000).Value = suremInfo.Msg;
                            //결과 (대기 0 ; 예약 R)
                            cmd.Parameters.Add("@RESULT", SqlDbType.Char, 1).Value = (status.Equals("0")) ? "0" : "R";
                            //구분(SMS : S)
                            cmd.Parameters.Add("@KIND", SqlDbType.Char, 1).Value = "S";

                            cn.Open();

                            ExecuteNonQuery(cmd);

                            return true;
                        }
                    }
                    catch (SqlException ex)
                    {
                        string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                        Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                        return false;
                    }
                }

                return rtn;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Transfer : " + errMsg);
                throw;
            }
        }
    }
}
