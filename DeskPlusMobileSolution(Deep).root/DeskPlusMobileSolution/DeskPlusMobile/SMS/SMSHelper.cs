using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.SMS
{
    public class SMSHelper
    {

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