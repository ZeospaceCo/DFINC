using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Account.BE
{
    /// <summary>
    /// 작성자   : famo23
    /// 작성일자 : 2011-07-02
    /// 작성목적 : 	/// SSO을 위한 기본 필드를 정의한다
    /// 기본 필드는 추후 확장을 위하여 LDAP Attribute를 사용하며 DP의 내부코드인 UserID 를 제외하고는 String으로 처리한다
    /// </summary>
    [Serializable]
    public class SSOInfo
    {
        #region 필드
        private int userID;
        private int companyID;
        private string displayName;
        private string employeeNumber;
        private string departmentNumber;
        private string mail;
        private string userPassword;
        private string sn;
        private string givenName;
        private string initials;
        private string title;
        private string telephoneNumber;
        private string facsimileTelephoneNumber;
        private string mobile;
        private string roomNumber;
        private string carLicense;
        private string o;
        private string ou;
        private string employeeType;
        private string preferredLanguage;
        private string labeledURI;
        private string description;
        private string userCode;
        #endregion

        #region 생성자
        /// <summary>
        /// Initializes a new instance of the <see cref="SSOInfo"/> class.
        /// </summary>
        public SSOInfo() { }
        #endregion

        #region 프로퍼티
        /// <summary>
        /// DeskPlus 사용자 내부코드
        /// </summary>
        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        /// <summary>
        /// DeskPlus 사용자 내부코드
        /// </summary>
        public int CompanyID
        {
            get { return companyID; }
            set { companyID = value; }
        }

        /// <summary>
        /// 이름 
        /// </summary>
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        /// <summary>
        /// 사번 - 5
        /// </summary>
        public string EmployeeNumber
        {
            get { return employeeNumber; }
            set { employeeNumber = value; }
        }

        /// <summary>
        /// 부서코드 - 15
        /// </summary>
        public string DepartmentNumber
        {
            get { return departmentNumber; }
            set { departmentNumber = value; }
        }

        /// <summary>
        /// 메일 
        /// </summary>
        public string Mail
        {
            get { return mail; }
            set { mail = value; }
        }

        /// <summary>
        /// 패스워드 - XXX
        /// </summary>
        public string UserPassword
        {
            get { return userPassword; }
            set { userPassword = value; }
        }

        /// <summary>
        /// 성 - 이   (Etc로 사용가)
        /// </summary>
        public string SN
        {
            get { return sn; }
            set { sn = value; }
        }
        /// <summary>
        /// 이름 -  (Etc로 사용가)
        /// </summary>
        public string GivenName
        {
            get { return givenName; }
            set { givenName = value; }
        }
        /// <summary>
        /// 이름 약자 - LHS (Etc로 사용가)
        /// </summary>
        public string Initials
        {
            get { return initials; }
            set { initials = value; }
        }
        /// <summary>
        /// 직책 - 팀장(과장)
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        /// <summary>
        /// 전화번호 -
        /// </summary>
        public string TelephoneNumber
        {
            get { return telephoneNumber; }
            set { telephoneNumber = value; }
        }
        /// <summary>
        /// 팩스번호 - 
        /// </summary>
        public string FacsimileTelephoneNumber
        {
            get { return facsimileTelephoneNumber; }
            set { facsimileTelephoneNumber = value; }
        }
        /// <summary>
        /// 핸드폰 - 
        /// </summary>
        public string Mobile
        {
            get { return mobile; }
            set { mobile = value; }
        }
        /// <summary>
        /// ETC 사용
        /// </summary>
        public string RoomNumber
        {
            get { return roomNumber; }
            set { roomNumber = value; }
        }
        /// <summary>
        /// 주민등록번호 - 
        /// </summary>
        public string CarLicense
        {
            get { return carLicense; }
            set { carLicense = value; }
        }
        /// <summary>
        /// 회사명 - 
        /// </summary>
        public string O
        {
            get { return o; }
            set { o = value; }
        }
        /// <summary>
        /// 부서명 -
        /// </summary>
        public string OU
        {
            get { return ou; }
            set { ou = value; }
        }
        /// <summary>
        /// 사용자 유형 정보로 사용 - ETC
        /// </summary>
        public string EmployeeType
        {
            get { return employeeType; }
            set { employeeType = value; }
        }
        /// <summary>
        /// 사용자 환경 - kr, en
        /// </summary>
        public string PreferredLanguage
        {
            get { return preferredLanguage; }
            set { preferredLanguage = value; }
        }
        /// <summary>
        /// ETC 사용
        /// </summary>
        public string LabeledURI
        {
            get { return labeledURI; }
            set { labeledURI = value; }
        }

        /// <summary>
        /// 설명 - ^^
        /// </summary>
        public string UserCode
        {
            get { return userCode; }
            set { userCode = value; }
        }

        /// <summary>
        /// 설명 - ^^
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        #endregion

    }
}