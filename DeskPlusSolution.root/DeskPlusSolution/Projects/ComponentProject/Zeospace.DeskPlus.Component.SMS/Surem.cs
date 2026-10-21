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
    public class Surem : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL, ISMS
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

                    string receivePhone1 = _smsInfo.ReceivePhone1;
                    string receivePhone2 = _smsInfo.ReceivePhone2;
                    string receivePhone3 = _smsInfo.ReceivePhone3;

                    string sphone1 = _smsInfo.ReplyPhone1;
                    string sphone2 = _smsInfo.ReplyPhone2;
                    string sphone3 = _smsInfo.ReplyPhone3;

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

                    #region 문자 발송자 전화번호                    
                    suremInfo.Sphone1 = sphone1;
                    suremInfo.Sphone2 = sphone2;
                    suremInfo.Sphone3 = sphone3;
                    suremInfo.SendName = (_smsInfo.WriteUserName.ToNull().Length == 0) ? sphone1 + sphone2 + sphone3 : _smsInfo.WriteUserName;
                    #endregion

                    #region 문자 수신자 전화번호
                    suremInfo.Rphone1 = receivePhone1;
                    suremInfo.Rphone2 = receivePhone2;
                    suremInfo.Rphone3 = receivePhone3;
                    suremInfo.RecvName = (_smsInfo.ReceiverName.ToNull().Length == 0) ? receivePhone1 + receivePhone2 + receivePhone3 : _smsInfo.ReceiverName;
                    #endregion

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
                    suremInfo.Kind = "S"; //일반 전송인 경우 S , url전송인 경우 U
                    suremInfo.ErrCode = 0; //error 코드
                    suremInfo.RecvTime = ""; // 응답시간
                    #endregion                    

                    try
                    {
                        string query = "SQL_SMSData_Insert";
                        //모든 회사 사용자에게 SMS 발송
                        if (_smsInfo.UseSendAllUsers) query = "SQL_SMSData_InsertByAllUsers";

                        using (var cn = new SqlConnection(ConnAlarm(1)))
                        {
                            var cmd = new SqlCommand(query, cn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@InDate", SqlDbType.Char, 8).Value = suremInfo.InDate;
                            cmd.Parameters.Add("@InTime", SqlDbType.Char, 6).Value = suremInfo.InTime;
                            cmd.Parameters.Add("@Member", SqlDbType.Int).Value = suremInfo.Member;

                            cmd.Parameters.Add("@SendId", SqlDbType.VarChar, 16).Value = suremInfo.SendId;
                            cmd.Parameters.Add("@SendName", SqlDbType.VarChar, 32).Value = suremInfo.SendName;
                            cmd.Parameters.Add("@Sphone1", SqlDbType.Char, 4).Value = suremInfo.Sphone1;
                            cmd.Parameters.Add("@Sphone2", SqlDbType.Char, 4).Value = suremInfo.Sphone2;
                            cmd.Parameters.Add("@Sphone3", SqlDbType.Char, 4).Value = suremInfo.Sphone3;

                            cmd.Parameters.Add("@Rphone1", SqlDbType.Char, 4).Value = suremInfo.Rphone1;
                            cmd.Parameters.Add("@Rphone2", SqlDbType.Char, 4).Value = suremInfo.Rphone2;
                            cmd.Parameters.Add("@Rphone3", SqlDbType.Char, 4).Value = suremInfo.Rphone3;
                            cmd.Parameters.Add("@RecvName", SqlDbType.VarChar, 32).Value = suremInfo.RecvName;
                            
                            cmd.Parameters.Add("@Msg", SqlDbType.VarChar, 120).Value = suremInfo.Msg;
                            cmd.Parameters.Add("@Url", SqlDbType.VarChar, 80).Value = suremInfo.Url;
                            cmd.Parameters.Add("@Rdate", SqlDbType.Char, 8).Value = suremInfo.Rdate;
                            cmd.Parameters.Add("@Rtime", SqlDbType.Char, 6).Value = suremInfo.Rtime;
                            cmd.Parameters.Add("@Result", SqlDbType.Char, 1).Value = suremInfo.Result;
                            cmd.Parameters.Add("@Kind", SqlDbType.Char, 1).Value = suremInfo.Kind;
                            cmd.Parameters.Add("@ErrCode", SqlDbType.Int).Value = suremInfo.ErrCode;
                            cmd.Parameters.Add("@RecvTime", SqlDbType.Char, 16).Value = suremInfo.RecvTime;

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


    /// <summary>
    /// UserSMSDAL
    /// </summary>
    public class UserSMSDAL : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        public string GetSmsCode(string _loginID)
        {
            if (_loginID.Equals("")) return "";


            try
            {
                string rtnval = "";

                using (var cn = new SqlConnection(ConnAlarm(1)))
                {
                    var cmd = new SqlCommand("SQL_SMSDataEtc_SelectByLoginID", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@LoginID", SqlDbType.NVarChar, 50).Value = _loginID;

                    cn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rtnval = reader.GetString(0);
                        }                            
                    }
                }

                return rtnval;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return null;
            }
        }
    }



    /// <summary>
    /// 메서드명 :
    /// 작성목적 : 최신번호
    /// 작성자   : 심재운
    /// </summary>
    public class RecentPhoneDAL : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        public DataSet GetRecentPhone(int _companyID, int _userID)
        {

            try
            {
                using (var cn = new SqlConnection(ConnAlarm(_companyID)))
                {
                    var cmd = new SqlCommand("SQL_DP_ALARM_SuremRecentPhone_Select", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                    cn.Open();

                    return ExecuteDataSet(cmd,"GetRecentPhone");
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return null;
            }
        }

        /// <summary>
        /// Adds the specified _surem recent phone info.
        /// </summary>
        /// <param name="_suremRecentPhoneInfo">The _surem recent phone info.</param>
        /// <returns></returns>
        public bool Add(SuremRecentPhoneInfo _suremRecentPhoneInfo)
        {

            try
            {
                using (var cn = new SqlConnection(ConnAlarm(1)))
                {
                    var cmd = new SqlCommand("SQL_DP_ALARM_SuremRecentPhone_Insert", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _suremRecentPhoneInfo.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _suremRecentPhoneInfo.UserID;
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = _suremRecentPhoneInfo.Name;
                    cmd.Parameters.Add("@PhoneNumber1", SqlDbType.VarChar, 10).Value = _suremRecentPhoneInfo.PhoneNumber1;
                    cmd.Parameters.Add("@PhoneNumber2", SqlDbType.VarChar, 10).Value = _suremRecentPhoneInfo.PhoneNumber2;
                    cmd.Parameters.Add("@PhoneNumber3", SqlDbType.VarChar, 10).Value = _suremRecentPhoneInfo.PhoneNumber3;

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// Mods the specified _surem recent phone info.
        /// </summary>
        /// <param name="_suremRecentPhoneInfo">The _surem recent phone info.</param>
        /// <returns></returns>
        public bool Mod(SuremRecentPhoneInfo _suremRecentPhoneInfo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnAlarm(1)))
                {
                    var cmd = new SqlCommand("SQL_DP_ALARM_SuremRecentPhone_Update", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _suremRecentPhoneInfo.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _suremRecentPhoneInfo.UserID;
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = _suremRecentPhoneInfo.Name;
                    cmd.Parameters.Add("@SuremRecentPhoneID", SqlDbType.Int).Value = _suremRecentPhoneInfo.SuremRecentPhoneID;
                    cmd.Parameters.Add("@PhoneNumber1", SqlDbType.VarChar, 10).Value = _suremRecentPhoneInfo.PhoneNumber1;
                    cmd.Parameters.Add("@PhoneNumber2", SqlDbType.VarChar, 10).Value = _suremRecentPhoneInfo.PhoneNumber2;
                    cmd.Parameters.Add("@PhoneNumber3", SqlDbType.VarChar, 10).Value = _suremRecentPhoneInfo.PhoneNumber3;

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// Dels the specified _surem recent phone info.
        /// </summary>
        /// <param name="_suremRecentPhoneInfo">The _surem recent phone info.</param>
        /// <returns></returns>
        public bool Del(SuremRecentPhoneInfo _suremRecentPhoneInfo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnAlarm(1)))
                {
                    var cmd = new SqlCommand("SQL_DP_ALARM_SuremRecentPhone_Delete", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _suremRecentPhoneInfo.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _suremRecentPhoneInfo.UserID;
                    cmd.Parameters.Add("@SuremRecentPhoneID", SqlDbType.Int).Value = _suremRecentPhoneInfo.SuremRecentPhoneID;

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }
    }


    /// <summary>
    /// 회신번호
    /// </summary>
    public class ReplyPhoneDAL : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        /// <summary>
        /// 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_suremReplyPhoneID">The _surem reply phone ID.</param>
        /// <returns></returns>
        public DataSet GetReplyPhone(int _companyID, int _userID, int _suremReplyPhoneID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnAlarm(_companyID)))
                {
                    var cmd = new SqlCommand("SQL_DP_ALARM_SuremReplyPhone_Select", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@SuremReplyPhoneID", SqlDbType.Int).Value = _suremReplyPhoneID;
                    

                    cn.Open();

                    return ExecuteDataSet(cmd, "GetReplyPhone");
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return null;
            }
        }

        /// <summary>
        /// 추가
        /// </summary>
        /// <param name="_suremReplyPhoneInfo">The _surem reply phone info.</param>
        /// <returns></returns>
        public int Add(SuremReplyPhoneInfo _suremReplyPhoneInfo)
        {

            try
            {
                using (var cn = new SqlConnection(ConnAlarm(_suremReplyPhoneInfo.CompanyID)))
                {
                    var cmd = new SqlCommand("SQL_DP_ALARM_SuremReplyPhone_Insert", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _suremReplyPhoneInfo.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _suremReplyPhoneInfo.UserID;
                    cmd.Parameters.Add("@PhoneNumber1", SqlDbType.VarChar, 10).Value = _suremReplyPhoneInfo.PhoneNumber1;
                    cmd.Parameters.Add("@PhoneNumber2", SqlDbType.VarChar, 10).Value = _suremReplyPhoneInfo.PhoneNumber2;
                    cmd.Parameters.Add("@PhoneNumber3", SqlDbType.VarChar, 10).Value = _suremReplyPhoneInfo.PhoneNumber3;

                    cn.Open();

                    return ExecuteScalar(cmd).ToString().ToNumber<int>();
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// Mods the specified _surem reply phone info.
        /// </summary>
        /// <param name="_suremReplyPhoneInfo">The _surem reply phone info.</param>
        /// <returns></returns>
        public bool Mod(SuremReplyPhoneInfo _suremReplyPhoneInfo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnAlarm(_suremReplyPhoneInfo.CompanyID)))
                {
                    var cmd = new SqlCommand("SQL_DP_ALARM_SuremReplyPhone_Update", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _suremReplyPhoneInfo.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _suremReplyPhoneInfo.UserID;
                    cmd.Parameters.Add("@PhoneNumber1", SqlDbType.VarChar, 10).Value = _suremReplyPhoneInfo.PhoneNumber1;
                    cmd.Parameters.Add("@PhoneNumber2", SqlDbType.VarChar, 10).Value = _suremReplyPhoneInfo.PhoneNumber2;
                    cmd.Parameters.Add("@PhoneNumber3", SqlDbType.VarChar, 10).Value = _suremReplyPhoneInfo.PhoneNumber3;

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="_suremReplyPhoneInfo">The _surem reply phone info.</param>
        /// <returns></returns>
        public bool Del(SuremReplyPhoneInfo _suremReplyPhoneInfo)
        {

            try
            {
                using (var cn = new SqlConnection(ConnAlarm(_suremReplyPhoneInfo.CompanyID)))
                {
                    var cmd = new SqlCommand("SQL_DP_ALARM_SuremReplyPhone_Delete", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _suremReplyPhoneInfo.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _suremReplyPhoneInfo.UserID;
                    cmd.Parameters.Add("@SuremReplyPhoneID", SqlDbType.VarChar, 10).Value = _suremReplyPhoneInfo.SuremReplyPhoneID;

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }
    }


    /// <summary>
    /// 회신 기본설정
    /// </summary>
    public class ReplyPhoneBaseInfoDAL : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        /// <summary>
        /// Dels the specified _user ID.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public DataSet GetReplyPhoneBase(int _companyID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnAlarm(_companyID)))
                {
                    var cmd = new SqlCommand("SQL_DP_ALARM_SuremReplyPhoneBase_Select", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                    cn.Open();

                    return ExecuteDataSet(cmd, "GetReplyPhoneBase");
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }


        /// <summary>
        /// 추가
        /// </summary>
        /// <param name="_suremReplyPhoneBaseInfo">The _surem reply phone base info.</param>
        /// <returns></returns>
        public bool Add(SuremReplyPhoneBaseInfo _suremReplyPhoneBaseInfo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnAlarm(_suremReplyPhoneBaseInfo.CompanyID)))
                {
                    var cmd = new SqlCommand("SQL_DP_ALARM_SuremReplyPhoneBase_Insert", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _suremReplyPhoneBaseInfo.CompanyID;
                    cmd.Parameters.Add("@SuremReplyPhoneID", SqlDbType.Int).Value = _suremReplyPhoneBaseInfo.SuremReplyPhoneID;
                    cmd.Parameters.Add("@ReplyPhoneNumber", SqlDbType.VarChar, 20).Value = _suremReplyPhoneBaseInfo.ReplyPhoneNumber;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _suremReplyPhoneBaseInfo.UserID;

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }


        /// <summary>
        /// Dels the specified _user ID.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnAlarm(_companyID)))
                {
                    var cmd = new SqlCommand("SQL_DP_ALARM_SuremReplyPhoneBase_DeleteByUserID", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }


        /// <summary>
        /// Dels the specified _surem reply phone base info.
        /// </summary>
        /// <param name="_suremReplyPhoneBaseInfo">The _surem reply phone base info.</param>
        /// <returns></returns>
        public bool Del(SuremReplyPhoneBaseInfo _suremReplyPhoneBaseInfo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnAlarm(_suremReplyPhoneBaseInfo.CompanyID)))
                {
                    var cmd = new SqlCommand("SQL_DP_ALARM_SuremReplyPhoneBase_Delete", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _suremReplyPhoneBaseInfo.CompanyID;
                    cmd.Parameters.Add("@SuremReplyPhoneID", SqlDbType.Int).Value = _suremReplyPhoneBaseInfo.SuremReplyPhoneID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _suremReplyPhoneBaseInfo.UserID;

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }
    }

    /// <summary>
    /// 클래스명   :
    /// 작성목적   : 회사 설정 가져오기
    /// 작성자     : 홍석기
    /// 최초작성일 : 2008-06-25
    /// </summary>
    public class DefaultDeptDAL : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        /// <summary>
        /// 사용자 이름 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userList">The _user list.</param>
        /// <returns></returns>
        public DataSet GetUserList(int _companyID, string _userList)
        {

            try
            {
                using (var cn = new SqlConnection(ConnAlarm(1)))
                {
                    var cmd = new SqlCommand("SQL_DP_ALARM_UserName_Select", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserList", SqlDbType.VarChar, 2000).Value = _userList;

                    cn.Open();

                    return ExecuteDataSet(cmd, "GetUserList");
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }


    }   
}
