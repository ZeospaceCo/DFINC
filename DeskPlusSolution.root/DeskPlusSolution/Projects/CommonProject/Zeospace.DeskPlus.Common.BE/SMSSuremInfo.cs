using System;

namespace Zeospace.DeskPlus.Common.BE
{
    /// <summary>
    /// 슈어엠 전송
    /// </summary>
    [Serializable]
    public class SMSSuremInfo
    {
        /// <summary>
        /// 부서아이디
        /// </summary>
        public int DeptID { get; set; }

        /// <summary>
        /// 부서명
        /// </summary>
        public string DeptName { get; set; }

        /// <summary>
        /// 회사선택 이름
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 일련번호
        /// </summary>
        /// <value>The seqno.</value>
        public int Seqno { get; set; }

        /// <summary>
        /// 발생일자(YYYYMMDD) : 필수
        /// </summary>
        /// <value>The in date.</value>
        public string InDate { get; set; }

        /// <summary>
        /// 발생시간(HHMMSS) : 필수
        /// </summary>
        /// <value>The in time.</value>
        public string InTime { get; set; }

        /// <summary>
        /// 고객사측구분번호 : 기본값 0
        /// </summary>
        /// <value>The member.</value>
        public int Member { get; set; }

        /// <summary>
        /// 송신자아이디 : 고객사용 : 필수(요금징수시)
        /// </summary>
        /// <value>The send id.</value>
        public string SendId { get; set; }

        /// <summary>
        /// 송신자이름 : 고객사용
        /// </summary>
        /// <value>The name of the send.</value>
        public string SendName { get; set; }

        /// <summary>
        /// 수신번호1 : 필수
        /// </summary>
        /// <value>The rphone1.</value>
        public string Rphone1 { get; set; }

        /// <summary>
        /// 수신번호21 : 필수
        /// </summary>
        /// <value>The rphone2.</value>
        public string Rphone2 { get; set; }

        /// <summary>
        /// 수신번호3 : 필수
        /// </summary>
        /// <value>The rphone3.</value>
        public string Rphone3 { get; set; }

        /// <summary>
        /// 수신자명 : 고객사용
        /// </summary>
        /// <value>The name of the recv.</value>
        public string RecvName { get; set; }

        /// <summary>
        /// 회신번호1 : 필수
        /// </summary>
        /// <value>The sphone1.</value>
        public string Sphone1 { get; set; }

        /// <summary>
        /// 회신번호2 : 필수
        /// </summary>
        /// <value>The sphone2.</value>
        public string Sphone2 { get; set; }

        /// <summary>
        /// 회신번호3 : 필수
        /// </summary>
        /// <value>The sphone3.</value>
        public string Sphone3 { get; set; }

        /// <summary>
        /// 메시지 : 필수
        /// </summary>
        /// <value>The MSG.</value>
        public string Msg { get; set; }

        /// <summary>
        /// 호출 URL (CALLBACK URL 전송시)
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; set; }

        /// <summary>
        /// 예약일자 : 즉시전송(0),예약전송(YYYYMMDD) : 필수
        /// </summary>
        /// <value>The rdate.</value>
        public string Rdate { get; set; }

        /// <summary>
        /// 예약시간 : 즉시전송(0),예약전송(HHMMSS)-24시기준 : 필수
        /// </summary>
        /// <value>The rtime.</value>
        public string Rtime { get; set; }

        /// <summary>
        /// 결과 - 대기(0), 성공(Y), 실패(N)
        /// </summary>
        /// <value>The result.</value>
        public string Result { get; set; }

        /// <summary>
        /// SMS 전송(S), URL전송(U)
        /// </summary>
        /// <value>The kind.</value>
        public string Kind { get; set; }

        /// <summary>
        /// 오류코드 - 슈어엠측에서 정의
        /// </summary>
        /// <value>The err code.</value>
        public int ErrCode { get; set; }

        /// <summary>
        /// 응답시간 - MSSQL 만 지원
        /// </summary>
        /// <value>The recv time.</value>
        public string RecvTime { get; set; }
    }

    /// <summary>
    /// 최신
    /// </summary>
    [Serializable]
    public class SuremRecentPhoneInfo
    {
        /// <summary>
        /// 이름
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the surem recent phone ID.
        /// </summary>
        /// <value>The surem recent phone ID.</value>
        public int SuremRecentPhoneID { get; set; }

        /// <summary>
        /// 회사아이디
        /// </summary>
        /// <value>The company ID.</value>
        public int CompanyID { get; set; }

        /// <summary>
        /// 사용자아이디
        /// </summary>
        /// <value>The user ID.</value>
        public int UserID { get; set; }

        /// <summary>
        /// 전화번호1
        /// </summary>
        /// <value>The phone number1.</value>
        public string PhoneNumber1 { get; set; }

        /// <summary>
        /// 전화번호2
        /// </summary>
        /// <value>The phone number2.</value>
        public string PhoneNumber2 { get; set; }

        /// <summary>
        /// 전화번호3
        /// </summary>
        /// <value>The phone number3.</value>
        public string PhoneNumber3 { get; set; }

        /// <summary>
        /// 작성일자
        /// </summary>
        /// <value>The write date.</value>
        public DateTime WriteDate { get; set; }
    }

    /// <summary>
    /// 회신
    /// </summary>
    [Serializable]
    public class SuremReplyPhoneInfo
    {
        /// <summary>
        /// Gets or sets the surem reply phone ID.
        /// </summary>
        /// <value>The surem reply phone ID.</value>
        public int SuremReplyPhoneID { get; set; }

        /// <summary>
        /// 회사아이디
        /// </summary>
        /// <value>The company ID.</value>
        public int CompanyID { get; set; }

        /// <summary>
        /// 사용자아이디
        /// </summary>
        /// <value>The user ID.</value>
        public int UserID { get; set; }

        /// <summary>
        /// 전화번호1
        /// </summary>
        /// <value>The phone number1.</value>
        public string PhoneNumber1 { get; set; }

        /// <summary>
        /// 전화번호2
        /// </summary>
        /// <value>The phone number2.</value>
        public string PhoneNumber2 { get; set; }

        /// <summary>
        /// 전화번호3
        /// </summary>
        /// <value>The phone number3.</value>
        public string PhoneNumber3 { get; set; }

        /// <summary>
        /// 작성일자
        /// </summary>
        /// <value>The write date.</value>
        public DateTime WriteDate { get; set; }
    }

    /// <summary>
    /// 회신 기본
    /// </summary>
    [Serializable]
    public class SuremReplyPhoneBaseInfo
    {
        /// <summary>
        /// Gets or sets the surem reply phone base ID.
        /// </summary>
        /// <value>The surem reply phone base ID.</value>
        public int SuremReplyPhoneBaseID { get; set; }

        /// <summary>
        /// Gets or sets the surem reply phone ID.
        /// </summary>
        /// <value>The surem reply phone ID.</value>
        public int SuremReplyPhoneID { get; set; }

        /// <summary>
        /// Gets or sets the reply phone number.
        /// </summary>
        /// <value>The reply phone number.</value>
        public string ReplyPhoneNumber { get; set; }

        /// <summary>
        /// 사용자 아이디
        /// </summary>
        /// <value>The user ID.</value>
        public int UserID { get; set; }

        /// <summary>
        /// Gets or sets the company ID.
        /// </summary>
        /// <value>
        /// The company ID.
        /// </value>
        public int CompanyID { get; set; }
    }

    /// <summary>
    /// SMSEnum
    /// </summary>
    public enum SMSEnum
    {
        SMSSurem = 0,
        SMSSuremDll = 1
    };
}