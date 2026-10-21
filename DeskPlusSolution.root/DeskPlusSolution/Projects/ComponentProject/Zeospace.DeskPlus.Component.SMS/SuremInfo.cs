using System;
using System.Collections.Generic;
using System.Text;

namespace Zeospace.DeskPlus.Component.SMS
{

    /// <summary>
    /// 슈어엠 전송
    /// </summary>
    [Serializable]
    public class SuremInfo
    {
        /// <summary>
        /// 일련번호
        /// </summary>
        private int seqno;
        /// <summary>
        /// 발생일자(YYYYMMDD) : 필수
        /// </summary>
        private string inDate;
        /// <summary>
        /// 발생시간(HHMMSS) : 필수
        /// </summary>
        private string inTime;
        /// <summary>
        /// 고객사측구분번호 : 기본값 0
        /// </summary>
        private int member;
        /// <summary>
        /// 송신자아이디 : 고객사용 : 필수(요금징수시)
        /// </summary>
        private string sendId;
        /// <summary>
        /// 송신자이름 : 고객사용
        /// </summary>
        private string sendName;
        /// <summary>
        /// 수신번호1 : 필수
        /// </summary>
        private string rphone1;
        /// <summary>
        /// 수신번호2 : 필수
        /// </summary>
        private string rphone2;
        /// <summary>
        /// 수신번호3 : 필수
        /// </summary>
        private string rphone3;
        /// <summary>
        /// 수신자명 : 고객사용
        /// </summary>
        private string recvName;
        /// <summary>
        /// 회신번호1 : 필수
        /// </summary>
        private string sphone1;
        /// <summary>
        /// 회신번호2 : 필수
        /// </summary>
        private string sphone2;
        /// <summary>
        /// 회신번호3 : 필수
        /// </summary>
        private string sphone3;
        /// <summary>
        /// 메시지 : 필수
        /// </summary>
        private string msg;
        /// <summary>
        /// 호출 URL (CALLBACK URL 전송시)
        /// </summary>
        private string url;
        /// <summary>
        /// 예약일자 : 즉시전송(0),예약전송(YYYYMMDD) : 필수
        /// </summary>
        private string rdate;
        /// <summary>
        /// 예약시간 : 즉시전송(0),예약전송(HHMMSS)-24시기준 : 필수
        /// </summary>
        private string rtime;

        /// <summary>
        /// 결과 - 대기(0), 성공(Y), 실패(N)
        /// </summary>
        private string result;

        /// <summary>
        /// 구분 - SMS 전송(S), URL전송(U)
        /// </summary>
        private string kind;
        /// <summary>
        /// 오류코드 - 슈어엠측에서 정의
        /// </summary>
        private int errCode;
        /// <summary>
        /// 응답시간 - MSSQL 만 지원
        /// </summary>
        private string recvTime;


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
        /// 일련번호
        /// </summary>
        /// <value>The seqno.</value>
        public int Seqno
        {
            get { return seqno; }
            set { seqno = value; }
        }
        /// <summary>
        /// 발생일자(YYYYMMDD) : 필수
        /// </summary>
        /// <value>The in date.</value>
        public string InDate
        {
            get { return inDate; }
            set { inDate = value; }
        }
        /// <summary>
        /// 발생시간(HHMMSS) : 필수
        /// </summary>
        /// <value>The in time.</value>
        public string InTime
        {
            get { return inTime; }
            set { inTime = value; }
        }
        /// <summary>
        /// 고객사측구분번호 : 기본값 0
        /// </summary>
        /// <value>The member.</value>
        public int Member
        {
            get { return member; }
            set { member = value; }
        }
        /// <summary>
        /// 송신자아이디 : 고객사용 : 필수(요금징수시)
        /// </summary>
        /// <value>The send id.</value>
        public string SendId
        {
            get { return sendId; }
            set { sendId = value; }
        }
        /// <summary>
        /// 송신자이름 : 고객사용
        /// </summary>
        /// <value>The name of the send.</value>
        public string SendName
        {
            get { return sendName; }
            set { sendName = value; }
        }
        /// <summary>
        /// 수신번호1 : 필수
        /// </summary>
        /// <value>The rphone1.</value>
        public string Rphone1
        {
            get { return rphone1; }
            set { rphone1 = value; }
        }
        /// <summary>
        /// 수신번호21 : 필수
        /// </summary>
        /// <value>The rphone2.</value>
        public string Rphone2
        {
            get { return rphone2; }
            set { rphone2 = value; }
        }
        /// <summary>
        /// 수신번호3 : 필수
        /// </summary>
        /// <value>The rphone3.</value>
        public string Rphone3
        {
            get { return rphone3; }
            set { rphone3 = value; }
        }
        /// <summary>
        /// 수신자명 : 고객사용
        /// </summary>
        /// <value>The name of the recv.</value>
        public string RecvName
        {
            get { return recvName; }
            set { recvName = value; }
        }
        /// <summary>
        /// 회신번호1 : 필수
        /// </summary>
        /// <value>The sphone1.</value>
        public string Sphone1
        {
            get { return sphone1; }
            set { sphone1 = value; }
        }
        /// <summary>
        /// 회신번호2 : 필수
        /// </summary>
        /// <value>The sphone2.</value>
        public string Sphone2
        {
            get { return sphone2; }
            set { sphone2 = value; }
        }
        /// <summary>
        /// 회신번호3 : 필수
        /// </summary>
        /// <value>The sphone3.</value>
        public string Sphone3
        {
            get { return sphone3; }
            set { sphone3 = value; }
        }
        /// <summary>
        /// 메시지 : 필수
        /// </summary>
        /// <value>The MSG.</value>
        public string Msg
        {
            get { return msg; }
            set { msg = value; }
        }
        /// <summary>
        /// 호출 URL (CALLBACK URL 전송시)
        /// </summary>
        /// <value>The URL.</value>
        public string Url
        {
            get { return url; }
            set { url = value; }
        }
        /// <summary>
        /// 예약일자 : 즉시전송(0),예약전송(YYYYMMDD) : 필수
        /// </summary>
        /// <value>The rdate.</value>
        public string Rdate
        {
            get { return rdate; }
            set { rdate = value; }
        }
        /// <summary>
        /// 예약시간 : 즉시전송(0),예약전송(HHMMSS)-24시기준 : 필수
        /// </summary>
        /// <value>The rtime.</value>
        public string Rtime
        {
            get { return rtime; }
            set { rtime = value; }
        }
        /// <summary>
        /// 결과 - 대기(0), 성공(Y), 실패(N)
        /// </summary>
        /// <value>The result.</value>
        public string Result
        {
            get { return result; }
            set { result = value; }
        }
        /// <summary>
        /// SMS 전송(S), URL전송(U)
        /// </summary>
        /// <value>The kind.</value>
        public string Kind
        {
            get { return kind; }
            set { kind = value; }
        }
        /// <summary>
        /// 오류코드 - 슈어엠측에서 정의
        /// </summary>
        /// <value>The err code.</value>
        public int ErrCode
        {
            get { return errCode; }
            set { errCode = value; }
        }
        /// <summary>
        /// 응답시간 - MSSQL 만 지원
        /// </summary>
        /// <value>The recv time.</value>
        public string RecvTime
        {
            get { return recvTime; }
            set { recvTime = value; }
        }  

    }


    /// <summary>
    /// 최신
    /// </summary>
    [Serializable]
    public class SuremRecentPhoneInfo
    {
        private int suremRecentPhoneID;
        private int companyID;
        private int userID;
        private string name;
        private string phoneNumber1;
        private string phoneNumber2;
        private string phoneNumber3;
        private DateTime writeDate;

        /// <summary>
        /// 이름
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }


        /// <summary>
        /// Gets or sets the surem recent phone ID.
        /// </summary>
        /// <value>The surem recent phone ID.</value>
        public int SuremRecentPhoneID
        {
            get { return suremRecentPhoneID; }
            set { suremRecentPhoneID = value; }
        }
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
        /// 사용자아이디
        /// </summary>
        /// <value>The user ID.</value>
        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }
        /// <summary>
        /// 전화번호1
        /// </summary>
        /// <value>The phone number1.</value>
        public string PhoneNumber1
        {
            get { return phoneNumber1; }
            set { phoneNumber1 = value; }
        }
        /// <summary>
        /// 전화번호2
        /// </summary>
        /// <value>The phone number2.</value>
        public string PhoneNumber2
        {
            get { return phoneNumber2; }
            set { phoneNumber2 = value; }
        }
        /// <summary>
        /// 전화번호3
        /// </summary>
        /// <value>The phone number3.</value>
        public string PhoneNumber3
        {
            get { return phoneNumber3; }
            set { phoneNumber3 = value; }
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


    }


    /// <summary>
    /// 회신
    /// </summary>
    [Serializable]
    public class SuremReplyPhoneInfo
    {
        private int suremReplyPhoneID;
        private int companyID;
        private int userID;
        private string phoneNumber1;
        private string phoneNumber2;
        private string phoneNumber3;
        private DateTime writeDate;

        /// <summary>
        /// Gets or sets the surem reply phone ID.
        /// </summary>
        /// <value>The surem reply phone ID.</value>
        public int SuremReplyPhoneID
        {
            get { return suremReplyPhoneID; }
            set { suremReplyPhoneID = value; }
        }
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
        /// 사용자아이디
        /// </summary>
        /// <value>The user ID.</value>
        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }
        /// <summary>
        /// 전화번호1
        /// </summary>
        /// <value>The phone number1.</value>
        public string PhoneNumber1
        {
            get { return phoneNumber1; }
            set { phoneNumber1 = value; }
        }
        /// <summary>
        /// 전화번호2
        /// </summary>
        /// <value>The phone number2.</value>
        public string PhoneNumber2
        {
            get { return phoneNumber2; }
            set { phoneNumber2 = value; }
        }
        /// <summary>
        /// 전화번호3
        /// </summary>
        /// <value>The phone number3.</value>
        public string PhoneNumber3
        {
            get { return phoneNumber3; }
            set { phoneNumber3 = value; }
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


    }


    /// <summary>
    /// 회신 기본
    /// </summary>
    [Serializable]
    public class SuremReplyPhoneBaseInfo
    {
        private int suremReplyPhoneBaseID;
        private int suremReplyPhoneID;
        private string replyPhoneNumber;
        private int userID;
        private int companyID;

        /// <summary>
        /// Gets or sets the surem reply phone base ID.
        /// </summary>
        /// <value>The surem reply phone base ID.</value>
        public int CompanyID
        {
            get { return companyID; }
            set { companyID = value; }
        }

        /// <summary>
        /// Gets or sets the surem reply phone base ID.
        /// </summary>
        /// <value>The surem reply phone base ID.</value>
        public int SuremReplyPhoneBaseID
        {
            get { return suremReplyPhoneBaseID; }
            set { suremReplyPhoneBaseID = value; }
        }

        /// <summary>
        /// Gets or sets the surem reply phone ID.
        /// </summary>
        /// <value>The surem reply phone ID.</value>
        public int SuremReplyPhoneID
        {
            get { return suremReplyPhoneID; }
            set { suremReplyPhoneID = value; }
        }

        /// <summary>
        /// Gets or sets the reply phone number.
        /// </summary>
        /// <value>The reply phone number.</value>
        public string ReplyPhoneNumber
        {
            get { return replyPhoneNumber; }
            set { replyPhoneNumber = value; }
        }
        /// <summary>
        /// 사용자 아이디
        /// </summary>
        /// <value>The user ID.</value>
        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }
    }
}
