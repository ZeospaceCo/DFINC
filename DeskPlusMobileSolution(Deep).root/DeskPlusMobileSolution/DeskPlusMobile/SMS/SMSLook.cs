using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Zeospace.DeskPlusMobile.Web.SMS
{
    public class SMSLook
    {
        private const string ORACLE_SMSDATA_INSERT = "INSERT INTO ARREO_SMS (CMP_MSG_ID, SND_DTTM, RCV_PHN_ID, CALLBACK, SND_MSG) VALUES (  '{0}', '{1}', '{2}', '{3}', '{4}' );";

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

                    #region Sms Data BackUp 처리
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

                    Zeospace.DeskPlusMobile.Web.SMS.SuremInfo suremInfo = new SuremInfo();
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

                    #region SMS DB BackUp 처리(발송)
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

                    #endregion

                    #region SMS Data 처리 ( for Oracle)
                    LookInfo lookInfo = new LookInfo();

                    lookInfo.CMP_MSG_ID = "GRPW" + _smsInfo.SendDate.ToString("yyMMdd") + GetSMSTotalCount().PadLeft(10, '0');
                    lookInfo.SND_DTTM = DateTime.Now.ToString("yyyyMMddHHmmss");
                    lookInfo.RCV_PHN_ID = _smsInfo.ReceivePhone1 + "-" + _smsInfo.ReceivePhone2 + "-" + _smsInfo.ReceivePhone3;
                    lookInfo.CALLBACK = _smsInfo.ReplyPhone1 + "-" + _smsInfo.ReplyPhone2 + "-" + _smsInfo.ReplyPhone3;
                    lookInfo.SND_MSG = _smsInfo.Content;
                    lookInfo.SMS_TYPE = "20";
                    lookInfo.CONSUMER_NM = (_smsInfo.ReceiverName == null) ? "" : _smsInfo.ReceiverName;
                    lookInfo.SND_DEPT_NM = _smsInfo.DeptName;

                    DataSet ds = GetUserData(_smsInfo.UserID, _smsInfo.CompanyID);

                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            lookInfo.SND_DEPT_CD = (ds.Tables[0].Rows[0]["DeptID"].ToString() == "") ? "GW" : ds.Tables[0].Rows[0]["DeptID"].ToString();
                            lookInfo.SND_PERSON_CD = (ds.Tables[0].Rows[0]["EmployeeNumber"].ToString() == "") ? "GWEmp" : ds.Tables[0].Rows[0]["EmployeeNumber"].ToString();
                        }
                        else
                        {
                            lookInfo.SND_DEPT_CD = "GW";
                            lookInfo.SND_PERSON_CD = "GWEmp";
                        }
                    }
                    else
                    {
                        lookInfo.SND_DEPT_CD = "GW";
                        lookInfo.SND_PERSON_CD = "GWEmp";
                    }

                    lookInfo.SND_PERSON_NM = _smsInfo.WriteUserName;

                    #endregion

                    #region SMS DB 처리 (for Oracle)
                    StringBuilder sqlStr = new StringBuilder();
                    string buildQuery = String.Format(ORACLE_SMSDATA_INSERT, lookInfo.CMP_MSG_ID, lookInfo.SND_DTTM, lookInfo.RCV_PHN_ID, lookInfo.CALLBACK, lookInfo.SND_MSG);
                    #endregion

                    #region DB insert (Backup and Oracle)

                    Helper.AppException.WriteAssert(AppSettingType.SQLApproval, suremInfo.InDate + "," + suremInfo.InTime + "," + suremInfo.Member + "," + suremInfo.SendId + "," + suremInfo.SendName + "," + suremInfo.Rphone1 + "," + suremInfo.Rphone2 + "," + suremInfo.Rphone3 + "," + suremInfo.RecvName + "," + suremInfo.Sphone1 + "," + suremInfo.Sphone2 + "," + suremInfo.Sphone3 + "," + suremInfo.Msg + "," + suremInfo.Url + "," + suremInfo.Rdate + "," + suremInfo.Rtime + "," + suremInfo.Result + "," + suremInfo.Kind + "," + suremInfo.ErrCode + "," + suremInfo.RecvTime);
                    Helper.AppException.WriteAssert(AppSettingType.SQLSMS, lookInfo.CMP_MSG_ID + "," + lookInfo.SND_DTTM + "," + lookInfo.RCV_PHN_ID + "," + lookInfo.CALLBACK + "," + lookInfo.SND_MSG + "," + lookInfo.SMS_TYPE + "," + lookInfo.CONSUMER_NM + "," + lookInfo.SND_DEPT_CD + "," + lookInfo.SND_DEPT_NM + "," + lookInfo.SND_PERSON_CD + "," + lookInfo.SND_PERSON_NM);

                    //모든 회사 사용자에게 SMS 발송
                    if (_smsInfo.UseSendAllUsers)
                    {
                        //룩에선 사용 않함
                        SQLHelper.ExecuteNonQuery(SQLHelper.CONN_ALARM, CommandType.StoredProcedure, "SQL_SMSData_InsertByAllUsers", parms);
                    }
                    else
                    {
                        //sql BackUp
                        SQLHelper.ExecuteNonQuery(SQLHelper.CONN_ALARM, CommandType.StoredProcedure, "SQL_SMSData_Insert", parms);
                        SQLHelper.ExecuteNonQuery(SQLHelper.CONN_ETC3, CommandType.Text, buildQuery, null);
                    }

                    #endregion

                    rtn = true;
                }

                return rtn;
            }
            catch (SqlException ex)
            {
                string errMsg = AppException.SqlExceptionLog(ex);
                AppException.WriteException("SMS.SMSLook.Transfer() : " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetSMSTotalCount()
        {
            try
            {
                return SQLHelper.ExecuteScalar(SQLHelper.CONN_ALARM, CommandType.StoredProcedure, "SQL_DP_SMSData_ForLookOpticsCount_Select", null).ToString();
            }
            catch (SqlException ex)
            {
                string errMsg = AppException.SqlExceptionLog(ex);
                AppException.WriteException("SMS.SMSLook.GetSMSTotalCount() : " + errMsg);
                return "0";
            }
        }


        /// <summary>
        /// 사용자 정보
        /// </summary>
        /// <param name="_userID">유저아이디</param>
        /// <returns></returns>
        public DataSet GetUserData(int _userID, int _companyID)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@UserID", SqlDbType.Int),
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                };

                parms[0].Value = _userID;
                parms[1].Value = _companyID;

                return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "SQL_DP_ACCOUNT_UserInterfaceCode_SelectByUserID", "GetUserData", parms);
            }
            catch (SqlException ex)
            {
                string errMsg = AppException.SqlExceptionLog(ex);
                AppException.WriteException("SMS.SMSLook.GetUserData() : " + errMsg);
                return null;
            }
        }

    }

    /// <summary>
    /// 슈어엠 전송
    /// </summary>
    [Serializable]
    public class LookInfo
    {
        /// <summary>
        /// 메시지ID (CMP_MSG_ID) : “GRPW” + YYMMDD + 일련번호(10자리)
        /// </summary>
        private string cmp_MSG_ID;

        /// <summary>
        /// 전송할시간 (SND_DTTM) : YYYYMMDDhhMMss 형식으로 입력 (ex: 20090118172343)
        /// </summary>
        private string snd_DTTM;

        /// <summary>
        /// 수신번호 (RCV_PHN_ID) : (ex: 010-1234-5678)
        /// </summary>
        private string rcv_PHN_ID;

        /// <summary>
        /// 회신번호 (CALLBACK) : 발신번호 (ex: 010-1234-8876)
        /// </summary>
        private string callBACK;

        /// <summary>
        /// 메시지내용 (SND_MSG)
        /// </summary>
        private string snd_MSG;

        /// <summary>
        /// SMS유형 (SMS_TYPE) : ‘20’
        /// </summary>
        private string sms_TYPE;

        /// <summary>
        /// 수신자성명 (CONSUMER_NM)
        /// </summary>
        private string consumer_NM;

        /// <summary>
        /// 전송부서코드 (SND_DEPT_CD) : 로그인사용자 부서코드
        /// </summary>
        private string snd_DEPT_CD;

        /// <summary>
        /// 전송부서명 (SND_DEPT_NM)
        /// </summary>
        private string snd_DEPT_NM;

        /// <summary>
        /// 전송자사번 (SND_PERSON_CD) : 로그인사용자 사번
        /// </summary>
        private string snd_PERSON_CD;

        /// <summary>
        /// 전송자성명 (SND_PERSON_NM)
        /// </summary>
        private string snd_PERSON_NM;


        /// <summary>
        /// 메시지ID (CMP_MSG_ID) : “GRPW” + YYMMDD + 일련번호(10자리)
        /// </summary>
        public string CMP_MSG_ID
        {
            get { return cmp_MSG_ID; }
            set { cmp_MSG_ID = value; }
        }

        /// <summary>
        /// 전송할시간 (SND_DTTM) : YYYYMMDDhhMMss 형식으로 입력 (ex: 20090118172343)
        /// </summary>
        public string SND_DTTM
        {
            get { return snd_DTTM; }
            set { snd_DTTM = value; }
        }

        /// <summary>
        /// 수신번호 (RCV_PHN_ID) : (ex: 010-1234-5678)
        /// </summary>
        public string RCV_PHN_ID
        {
            get { return rcv_PHN_ID; }
            set { rcv_PHN_ID = value; }
        }

        /// <summary>
        /// 회신번호 (CALLBACK) : 발신번호 (ex: 010-1234-8876)
        /// </summary>
        public string CALLBACK
        {
            get { return callBACK; }
            set { callBACK = value; }
        }

        /// <summary>
        /// 메시지내용 (SND_MSG)
        /// </summary>
        public string SND_MSG
        {
            get { return snd_MSG; }
            set { snd_MSG = value; }
        }

        /// <summary>
        /// SMS유형 (SMS_TYPE) : ‘20’
        /// </summary>
        public string SMS_TYPE
        {
            get { return sms_TYPE; }
            set { sms_TYPE = value; }
        }

        /// <summary>
        /// 수신자성명 (CONSUMER_NM)
        /// </summary>
        public string CONSUMER_NM
        {
            get { return consumer_NM; }
            set { consumer_NM = value; }
        }

        /// <summary>
        /// 전송부서코드 (SND_DEPT_CD) : 로그인사용자 부서코드
        /// </summary>
        public string SND_DEPT_CD
        {
            get { return snd_DEPT_CD; }
            set { snd_DEPT_CD = value; }
        }

        /// <summary>
        /// 전송부서명 (SND_DEPT_NM)
        /// </summary>
        public string SND_DEPT_NM
        {
            get { return snd_DEPT_NM; }
            set { snd_DEPT_NM = value; }
        }

        /// <summary>
        /// 전송자사번 (SND_PERSON_CD) : 로그인사용자 사번
        /// </summary>
        public string SND_PERSON_CD
        {
            get { return snd_PERSON_CD; }
            set { snd_PERSON_CD = value; }
        }

        /// <summary>
        /// 전송자성명 (SND_PERSON_NM)
        /// </summary>
        public string SND_PERSON_NM
        {
            get { return snd_PERSON_NM; }
            set { snd_PERSON_NM = value; }
        }
    }
}