using System;

namespace Zeospace.DeskPlus.Common.BE
{
    /// <summary>
    /// SMSInfo
    /// </summary>
    [Serializable]
    public class SMSInfo
    {
        /// <summary>
        /// 회사아이디
        /// </summary>
        /// <value>The company ID.</value>
        public int CompanyID { get; set; }

        /// <summary>
        /// 회사명
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 인터페이스
        /// </summary>
        /// <value></value>
        public string InterfaceID { get; set; }

        /// <summary>
        /// 발송자 로긴 아이디
        /// </summary>
        public string LoginID { get; set; }

        /// <summary>
        /// 사용자아이디
        /// </summary>
        /// <value>The user ID.</value>
        public int UserID { get; set; }

        /// <summary>
        /// 부서아이디
        /// </summary>
        public int DeptID { get; set; }

        /// <summary>
        /// 부서명
        /// </summary>
        /// <value>
        /// The name of the dept.
        /// </value>
        public string DeptName { get; set; }

        /// <summary>
        /// 고유아이디
        /// </summary>
        /// <value>The SMS notice ID.</value>
        public int SMSNoticeID { get; set; }

        /// <summary>
        /// 송신전화번호
        /// </summary>
        /// <value>The send phone.</value>
        public string SendPhone { get; set; }

        /// <summary>
        /// 수신번호 full
        /// </summary>
        /// <value>The receive phone.</value>
        public string ReceivePhone { get; set; }

        /// <summary>
        /// 수신번호1(조합용)
        /// </summary>
        /// <value>The receive phone1.</value>
        public string ReceivePhone1 { get; set; }

        /// <summary>
        /// 수신번호2(조합용)
        /// </summary>
        /// <value>The receive phone2.</value>
        public string ReceivePhone2 { get; set; }

        /// <summary>
        /// 수신번호3(조합용)
        /// </summary>
        /// <value>The receive phone3.</value>
        public string ReceivePhone3 { get; set; }

        /// <summary>
        /// 회신번호1(조합용)
        /// </summary>
        /// <value>The reply phone1.</value>
        public string ReplyPhone1 { get; set; }

        /// <summary>
        /// 회신번호2(조합용)
        /// </summary>
        /// <value>The reply phone2.</value>
        public string ReplyPhone2 { get; set; }

        /// <summary>
        /// 회신번호3(조합용)
        /// </summary>
        /// <value>The reply phone3.</value>
        public string ReplyPhone3 { get; set; }

        /// <summary>
        /// 상태(즉시:0,예약:1)
        /// </summary>
        /// <value>The status.</value>
        public string Status { get; set; }

        /// <summary>
        /// 발송일자
        /// </summary>
        /// <value>The send date.</value>
        public DateTime SendDate { get; set; }

        /// <summary>
        /// 발송시간
        /// </summary>
        /// <value>The send time.</value>
        public string SendTime { get; set; }

        /// <summary>
        /// 제목
        /// </summary>
        /// <value>The subject.</value>
        public string Subject { get; set; }

        /// <summary>
        /// 내용
        /// </summary>
        /// <value>The content.</value>
        public string Content { get; set; }

        /// <summary>
        /// url
        /// </summary>
        /// <value>The target URL.</value>
        public string TargetUrl { get; set; }

        /// <summary>
        /// 발송시작일자 (일정관리용)
        /// </summary>
        /// <value>The start date.</value>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 발송종료일자 (일정관리용)
        /// </summary>
        /// <value>The end date.</value>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 작성일자
        /// </summary>
        /// <value>The write date.</value>
        public DateTime WriteDate { get; set; }

        /// <summary>
        /// 템플릿아이디
        /// </summary>
        /// <value>The SMS notice template ID.</value>
        public int SMSNoticeTemplateID { get; set; }

        /// <summary>
        /// 작성자 아이디
        /// </summary>
        /// <value>The write user ID.</value>
        public int WriteUserID { get; set; }

        /// <summary>
        /// 작성자 이름
        /// </summary>
        /// <value>The name of the write user.</value>
        public string WriteUserName { get; set; }

        /// <summary>
        /// 구분자
        /// </summary>
        public string Gubun { get; set; }

        /// <summary>
        /// 수신자이름
        /// </summary>
        public string ReceiverName { get; set; }

        /// <summary>
        /// 모든 사용자에게 sms 발송여부(기본값:개별발송) - true : 전체발송, false : 개별발송
        /// </summary>
        /// <value><c>true</c> if [use send all users]; otherwise, <c>false</c>.</value>
        public bool UseSendAllUsers { get; set; }
    }
}