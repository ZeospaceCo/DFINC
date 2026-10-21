using System;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Component.SMS
{
    /// <summary>
    /// NoticeHelper에 대한 요약 설명입니다.
    /// </summary>
    public class SMSHelper : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {

        /// <summary>
        /// shimpark
        /// SMS notice 추가
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public int AddSMSNotice(SMSInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnAlarm(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("SQL_DP_ALARM_SMSNotice_Insert", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@SendPhone", SqlDbType.NVarChar, 50).Value = _info.SendPhone.ToNull().ToXSS();
                    cmd.Parameters.Add("@ReceivePhone", SqlDbType.VarChar, 50).Value = _info.ReceivePhone.ToNull().ToXSS();
                    cmd.Parameters.Add("@Status", SqlDbType.Char, 1).Value = _info.Status.ToNull().ToXSS();
                    cmd.Parameters.Add("@SendDate", SqlDbType.DateTime).Value = _info.SendDate;
                    cmd.Parameters.Add("@Subject", SqlDbType.NVarChar, 70).Value = _info.Subject.ToNull().ToXSS();
                    cmd.Parameters.Add("@Content", SqlDbType.NVarChar, 255).Value = _info.Content;
                    cmd.Parameters.Add("@TargetUrl", SqlDbType.NVarChar, 100).Value = _info.TargetUrl;
                    cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _info.StartDate;
                    cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = _info.EndDate;
                    cmd.Parameters.Add("@InterfaceID", SqlDbType.NVarChar, 50).Value = _info.InterfaceID;


                    cn.Open();

                    return ExecuteScalar(cmd).ToString().ToNumber<int>();
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddSMSNotice : " + errMsg);
                throw;
            }
        }


        /// <summary>
        /// SMS notice 삭제
        /// </summary>
        /// <param name="_be">The _be.</param>
        /// <returns></returns>
        public bool DelSMSNotice(SMSInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnAlarm(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("SQL_DP_ALARM_SMSNotice_Delete", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@SMSNoticeID", SqlDbType.NVarChar, 50).Value = _info.SMSNoticeID;


                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelSMSNotice : " + errMsg);
                throw;
            }
        }


        /// <summary>
        /// 사용자별 sms notice 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public DataSet GetSMSNotice(int _companyID, int _userID)
        {

            try
            {
                using (var cn = new SqlConnection(ConnAlarm(_companyID)))
                {
                    var cmd = new SqlCommand("SQL_DP_ALARM_SMSNotice_Select", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;


                    cn.Open();

                    return ExecuteDataSet(cmd, "GetSMSNotice");
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetSMSNotice : " + errMsg);
                throw;
            }
        }


        /// <summary>
        /// SMS 템플릿 추가
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public int AddSMSTemplateNotice(SMSInfo _info)
        {

            try
            {
                using (var cn = new SqlConnection(ConnAlarm(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("SQL_DP_ALARM_SMSNoticeTemplate_Insert", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@Content", SqlDbType.NVarChar, 100).Value = _info.Content;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;
                    cmd.Parameters.Add("@WriteUserName", SqlDbType.NVarChar, 50).Value = _info.WriteUserName;


                    cn.Open();

                    return ExecuteScalar(cmd).ToString().ToNumber<int>();
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddSMSTemplateNotice : " + errMsg);
                throw;
            }
        }


        /// <summary>
        /// SMS 템플릿 업데이트
        /// </summary>
        /// <param name="_be"></param>
        /// <returns></returns>
        public bool UpdateSMSTemplateNotice(SMSInfo _info)
        {

            try
            {
                using (var cn = new SqlConnection(ConnAlarm(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("SQL_DP_ALARM_SMSNoticeTemplate_Update", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@SMSNoticeTemplateID", SqlDbType.Int).Value = _info.SMSNoticeTemplateID;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;
                    cmd.Parameters.Add("@Gubun", SqlDbType.Char, 1).Value = _info.Gubun;

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("UpdateSMSTemplateNotice : " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// sms 템플릿 삭제하기
        /// </summary>
        /// <param name="_be"></param>
        /// <returns></returns>
        public bool DelSMSTemplateNotice(SMSInfo _info)
        {

            try
            {
                using (var cn = new SqlConnection(ConnAlarm(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("SQL_DP_ALARM_SMSNoticeTemplate_Delete", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@SMSNoticeTemplateID", SqlDbType.Int).Value = _info.SMSNoticeTemplateID;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("UpdateSMSTemplateNotice : " + errMsg);
                throw;
            }
        }



        /// <summary>
        /// Gets the SMS template notice.
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public DataSet GetSMSTemplateNotice(SMSInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnAlarm(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("SQL_DP_ALARM_SMSNoticeTemplate_SelectByWriteUserID", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;
                    cmd.Parameters.Add("@Gubun", SqlDbType.Char, 1).Value = _info.Gubun.ToNull().ToXSS();


                    cn.Open();

                    return ExecuteDataSet(cmd, "GetSMSTemplateNotice");
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetSMSTemplateNotice : " + errMsg);
                throw;
            }
        }
    }


    /// <summary>
    /// NoticeHelperInfo
    /// </summary>
    [Serializable]
    public class SMSInfo
    {
        private int companyID;
        private string interfaceID;
        private int userID;
        private string loginID;
        private int sMSNoticeID;
        private string sendPhone;
        private string receivePhone;
        private string receivePhone1;
        private string receivePhone2;
        private string receivePhone3;
        private string replyPhone1;
        private string replyPhone2;
        private string replyPhone3;
        private string status;
        private DateTime sendDate;
        private string sendTime;
        private string subject;
        private string content;
        private string targetUrl;
        private DateTime startDate;
        private DateTime endDate;
        private DateTime writeDate;
        private int sMSNoticeTemplateID;
        private int writeUserID;
        private string writeUserName;
        private string gubun;
        private string receiverName;
        private bool useSendAllUsers = false;

        /// <summary>
        /// 전체사용자 메일 발송 여부
        /// </summary>
        /// <value>The use send all users.</value>
        public bool UseSendAllUsers
        {
            get { return useSendAllUsers; }
            set { useSendAllUsers = value; }
        }


        /// <summary>
        /// 수신자이름
        /// </summary>
        public string ReceiverName
        {
            get { return receiverName; }
            set { receiverName = value; }
        }

        /// <summary>
        /// 회사선택 이름
        /// </summary>
        private string companyName;

        /// <summary>
        /// 부서아이디
        /// </summary>
        private int deptID;

        /// <summary>
        /// 부서명
        /// </summary>
        private string deptName;

        /// <summary>
        /// 회사아이디
        /// </summary>
        /// <value>The company ID.</value>
        public int CompanyID
        {
            get { return companyID; }
            set { companyID = value; }
        }

        /// <summary>
        /// 부서아이디
        /// </summary>
        public int DeptID
        {
            get { return deptID; }
            set { deptID = value; }
        }

        /// <summary>
        /// 부서명
        /// </summary>
        public string DeptName
        {
            get { return deptName; }
            set { deptName = value; }
        }


        /// <summary>
        /// 회사선택 이름
        /// </summary>
        public string CompanyName
        {
            get { return companyName; }
            set { companyName = value; }
        }


        /// <summary>
        /// 인터페이스
        /// </summary>
        /// <value></value>
        public string InterfaceID
        {
            get { return interfaceID; }
            set { interfaceID = value; }
        }

        /// <summary>
        /// 사용자아이디
        /// </summary>
        /// <value>The user ID.</value>
        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        /// <summary>
        /// 로그인 아이디
        /// </summary>
        public string LoginID
        {
            get { return loginID; }
            set { loginID = value; }
        }

        /// <summary>
        /// 고유아이디
        /// </summary>
        /// <value>The SMS notice ID.</value>
        public int SMSNoticeID
        {
            get { return sMSNoticeID; }
            set { sMSNoticeID = value; }
        }
        /// <summary>
        /// 송신전화번호
        /// </summary>
        /// <value>The send phone.</value>
        public string SendPhone
        {
            get { return sendPhone; }
            set { sendPhone = value; }
        }
        /// <summary>
        /// 수신번호 full
        /// </summary>
        /// <value>The receive phone.</value>
        public string ReceivePhone
        {
            get { return receivePhone; }
            set { receivePhone = value; }
        }

        /// <summary>
        /// 수신번호1(조합용)
        /// </summary>
        /// <value>The receive phone1.</value>
        public string ReceivePhone1
        {
            get { return receivePhone1; }
            set { receivePhone1 = value; }
        }

        /// <summary>
        /// 수신번호2(조합용)
        /// </summary>
        /// <value>The receive phone2.</value>
        public string ReceivePhone2
        {
            get { return receivePhone2; }
            set { receivePhone2 = value; }
        }

        /// <summary>
        /// 수신번호3(조합용)
        /// </summary>
        /// <value>The receive phone3.</value>
        public string ReceivePhone3
        {
            get { return receivePhone3; }
            set { receivePhone3 = value; }
        }


        /// <summary>
        /// 회신번호1(조합용)
        /// </summary>
        /// <value>The reply phone1.</value>
        public string ReplyPhone1
        {
            get { return replyPhone1; }
            set { replyPhone1 = value; }
        }

        /// <summary>
        /// 회신번호2(조합용)
        /// </summary>
        /// <value>The reply phone2.</value>
        public string ReplyPhone2
        {
            get { return replyPhone2; }
            set { replyPhone2 = value; }
        }

        /// <summary>
        /// 회신번호3(조합용)
        /// </summary>
        /// <value>The reply phone3.</value>
        public string ReplyPhone3
        {
            get { return replyPhone3; }
            set { replyPhone3 = value; }
        }

        /// <summary>
        /// 상태(즉시:0,예약:1)
        /// </summary>
        /// <value>The status.</value>
        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        /// <summary>
        /// 발송일자
        /// </summary>
        /// <value>The send date.</value>
        public DateTime SendDate
        {
            get { return sendDate; }
            set { sendDate = value; }
        }

        /// <summary>
        /// 발송시간
        /// </summary>
        /// <value>The send time.</value>
        public string SendTime
        {
            get { return sendTime; }
            set { sendTime = value; }
        }

        /// <summary>
        /// 제목
        /// </summary>
        /// <value>The subject.</value>
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }
        /// <summary>
        /// 내용
        /// </summary>
        /// <value>The content.</value>
        public string Content
        {
            get { return content; }
            set { content = value; }
        }
        /// <summary>
        /// url
        /// </summary>
        /// <value>The target URL.</value>
        public string TargetUrl
        {
            get { return targetUrl; }
            set { targetUrl = value; }
        }
        /// <summary>
        /// 발송시작일자 (일정관리용)
        /// </summary>
        /// <value>The start date.</value>
        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }
        /// <summary>
        /// 발송종료일자 (일정관리용)
        /// </summary>
        /// <value>The end date.</value>
        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }
        /// <summary>
        /// 작성일자
        /// </summary>
        /// <value>The write date.</value>
        public DateTime WriteDate
        {
            get { return writeDate; }
            set { writeDate = value; }
        }



        /// <summary>
        /// 템플릿아이디
        /// </summary>
        /// <value>The SMS notice template ID.</value>
        public int SMSNoticeTemplateID
        {
            get { return sMSNoticeTemplateID; }
            set { sMSNoticeTemplateID = value; }
        }

        /// <summary>
        /// 작성자 아이디
        /// </summary>
        /// <value>The write user ID.</value>
        public int WriteUserID
        {
            get { return writeUserID; }
            set { writeUserID = value; }
        }

        /// <summary>
        /// 작성자 이름
        /// </summary>
        /// <value>The name of the write user.</value>
        public string WriteUserName
        {
            get { return writeUserName; }
            set { writeUserName = value; }
        }


        /// <summary>
        /// 구분자
        /// </summary>
        public string Gubun
        {
            get { return gubun; }
            set { gubun = value; }
        }
    }
}
