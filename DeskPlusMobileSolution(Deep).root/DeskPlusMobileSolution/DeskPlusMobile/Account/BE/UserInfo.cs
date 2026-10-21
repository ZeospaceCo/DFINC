using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace Zeospace.DeskPlusMobile.Web.Account.BE
{
    /// <summary>
    /// 사용자 정보 Entity
    /// </summary>
    /// 
    [Serializable]
    public class UserInfo
    {
        #region 필드
        /// <summary>
        /// 사용자 ID
        /// </summary>
        private int userID;

        /// <summary>
        /// 사용자 인터페이스 Code
        /// </summary>
        private string userCode;

        /// <summary>
        /// 접속 ID
        /// </summary>
        private string logInID;

        /// <summary>
        /// 가입업체 내부코드
        /// </summary>
        private int companyID;

        /// <summary>
        /// 사용자 이름
        /// </summary>
        private string name;

        /// <summary>
        /// 비밀번호
        /// </summary>
        private string password;

        /// <summary>
        /// 사번
        /// </summary>
        private string employeeNumber;

        /// <summary>
        /// 주민등록 번호
        /// </summary>
        private string registrationNumber;

        /// <summary>
        /// 성별
        /// </summary>
        private string sex;

        /// <summary>
        /// 양/음력구분
        /// </summary>
        private string lunar;

        /// <summary>
        /// 생년월일
        /// </summary>
        private string birthDay;

        /// <summary>
        /// 개인소개
        /// </summary>
        private string description;

        /// <summary>
        /// 집 우편번호
        /// </summary>
        private string homeZipCode;

        /// <summary>
        /// 집 주소1
        /// </summary>
        private string homeAddress1;

        /// <summary>
        /// 집 주소2
        /// </summary>
        private string homeAddress2;

        /// <summary>
        /// 본적 우편번호
        /// </summary>
        private string birthZipCode;

        /// <summary>
        /// 본적 집주소1
        /// </summary>
        private string birthAddress1;

        /// <summary>
        /// 본적 집주소2
        /// </summary>
        private string birthAddress2;

        /// <summary>
        /// 핸드폰번호
        /// </summary>
        private string mobilePhone;

        /// <summary>
        /// 집 전화번호
        /// </summary>
        private string homePhone;

        /// <summary>
        /// 회사 전화
        /// </summary>
        private string officePhone;

        /// <summary>
        /// 회사 내선번호
        /// </summary>
        private string extensionOfficePhone;

        /// <summary>
        /// 팩스 번호
        /// </summary>
        private string fax;

        /// <summary>
        /// IP 번호
        /// </summary>
        private string ipPhone;

        /// <summary>
        /// 입사 일자
        /// </summary>
        private string joinDate;

        /// <summary>
        /// 퇴사 일자
        /// </summary>
        private string retireDate;

        /// <summary>
        /// 인장이미지 바이너리
        /// </summary>
        private string sealImage;

        /// <summary>
        /// 사인이미지 바이너리
        /// </summary>
        private string photoImage;

        /// <summary>
        /// 개인디스크 할당공간
        /// </summary>
        private int privateQuota;


        private int mailQuota;

        /// <summary>
        /// 전자결재 지문인식 여부
        /// </summary>
        private string securityApproval;

        /// <summary>
        /// 로그인시 지문인식 여부
        /// </summary>
        private string securityLogIn;


        /// <summary>
        /// 사용중
        /// </summary>
        private string activeUser;


        /// <summary>
        /// 메일 아이디
        /// </summary>
        private string mailID;

        /// <summary>
        /// 메일 도메인
        /// </summary>
        private string mailDomain;

        /// <summary>
        /// 메일 패스워드
        /// </summary>
        private string mailPassword;


        /// <summary>
        /// 최초 입력자 ID
        /// </summary>
        private int writeUserID;

        /// <summary>
        /// 결혼기념일
        /// </summary>
        private DateTime weddingAnniversary;

        /// <summary>
        /// 수정자아이디
        /// </summary>
        private int modifyUserID;


        private ArrayList userDeptList;
        #endregion

        #region 생성자


        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfo"/> class.
        /// </summary>
        public UserInfo() { }


        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfo"/> class.
        /// </summary>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_logInID">The _log in ID.</param>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_name">The _name.</param>
        /// <param name="_password">The _password.</param>
        /// <param name="_employeeNumber">The _employee number.</param>
        /// <param name="_registrationNumber">The _registration number.</param>
        /// <param name="_sex">The _sex.</param>
        /// <param name="_lunar">The _lunar.</param>
        /// <param name="_birthDay">The _birth day.</param>
        /// <param name="_description">The _description.</param>
        /// <param name="_homeZipCode">The _home zip code.</param>
        /// <param name="_homeAddress1">The _home address1.</param>
        /// <param name="_homeAddress2">The _home address2.</param>
        /// <param name="_birthZipCode">The _birth zip code.</param>
        /// <param name="_birthAddress1">The _birth address1.</param>
        /// <param name="_birthAddress2">The _birth address2.</param>
        /// <param name="_mobilePhone">The _mobile phone.</param>
        /// <param name="_homePhone">The _home phone.</param>
        /// <param name="_officePhone">The _office phone.</param>
        /// <param name="_extensionOfficePhone">The _extension office phone.</param>
        /// <param name="_fax">The _fax.</param>
        /// <param name="_ipPhone">The _ip phone.</param>
        /// <param name="_joinDate">The _join date.</param>
        /// <param name="_retireDate">The _retire date.</param>
        /// <param name="_sealImage">The _seal image.</param>
        /// <param name="_photoImage">The _photo image.</param>
        /// <param name="_privateQuota">The _private quota.</param>
        /// <param name="_securityApproval">The security print approval.</param>
        /// <param name="_securityLogIn">The security print log in.</param>
        /// <param name="_mailID">The _mail ID.</param>
        /// <param name="_mailDomain">The _mail domain.</param>
        /// <param name="_mailPassword">The _mail password.</param>
        /// <param name="_activeUser">The _active user.</param>
        /// <param name="_writeUserID">The _write user ID.</param>
        /// <param name="_userCode">The _write Interpace Code.</param>/// 
        public UserInfo(int _userID, string _logInID, int _companyID, string _name, string _password, 
            string _employeeNumber, string _registrationNumber,
            string _sex, string _lunar, string _birthDay, string _description, string _homeZipCode, string _homeAddress1,
            string _homeAddress2, string _birthZipCode, string _birthAddress1, string _birthAddress2,
            string _mobilePhone, string _homePhone, string _officePhone, string _extensionOfficePhone,
            string _fax, string _ipPhone, string _joinDate, string _retireDate, string _sealImage,
            string _photoImage, int _privateQuota, string _securityApproval, string _securityLogIn, string _mailID,
            string _mailDomain, string _mailPassword, string _activeUser, int _writeUserID, string _userCode)
        {
            this.userID = _userID;
            this.logInID = _logInID;
            this.companyID = _companyID;
            this.name = _name;
            this.password = _password;
            this.employeeNumber = _employeeNumber;
            this.registrationNumber = _registrationNumber;
            this.sex = _sex;
            this.lunar = _lunar;
            this.birthDay = _birthDay;
            this.description = _description;
            this.homeZipCode = _homeZipCode;
            this.homeAddress1 = _homeAddress1;
            this.homeAddress2 = _homeAddress2;
            this.birthZipCode = _birthZipCode;
            this.birthAddress1 = _birthAddress1;
            this.birthAddress2 = _birthAddress2;
            this.mobilePhone = _mobilePhone;
            this.homePhone = _homePhone;
            this.officePhone = _officePhone;
            this.extensionOfficePhone = _extensionOfficePhone;
            this.fax = _fax;
            this.ipPhone = _ipPhone;
            this.joinDate = _joinDate;
            this.retireDate = _retireDate;
            this.sealImage = _sealImage;
            this.photoImage = _photoImage;
            this.privateQuota = _privateQuota;
            this.securityApproval = _securityApproval;
            this.securityLogIn = _securityLogIn;
            this.mailID = _mailID;
            this.mailDomain = _mailDomain;
            this.mailPassword = _mailPassword;
            this.activeUser = _activeUser;
            this.writeUserID = _writeUserID;
            this.userCode = _userCode;
        }

        #endregion

        #region 프로퍼티

        /// <summary>
        /// 사용자 ID 내부코드
        /// </summary>
        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        /// <summary>
        /// 사용자 인터페이스 코드
        /// </summary>
        public string UserCode
        {
            get { return userCode; }
            set { userCode = value; }
        }

        /// <summary>
        /// 접속 ID 
        /// </summary>
        public string LogInID
        {
            get { return logInID; }
            set { logInID = value; }
        }

        /// <summary>
        /// 가입업체 내부코드
        /// </summary>
        public int CompanyID
        {
            get { return companyID; }
            set { companyID = value; }
        }

        /// <summary>
        /// 사용자 한글명
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }     

        /// <summary>
        /// 패스워드
        /// </summary>
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        /// <summary>
        /// 사번
        /// </summary>
        public string EmployeeNumber
        {
            get { return employeeNumber; }
            set { employeeNumber = value; }
        }

        /// <summary>
        /// 주민등록번호
        /// </summary>
        public string RegistrationNumber
        {
            get { return registrationNumber; }
            set { registrationNumber = value; }
        }

        /// <summary>
        /// 성별
        /// </summary>
        public string Sex
        {
            get { return sex; }
            set { sex = value; }
        }

        /// <summary>
        /// 양/음력 구분
        /// </summary>
        public string Lunar
        {
            get { return lunar; }
            set { lunar = value; }
        }

        /// <summary>
        /// 생년월일
        /// </summary>
        public string BirthDay
        {
            get { return birthDay; }
            set { birthDay = value; }
        }

        /// <summary>
        /// 개인소개
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// 집 우편번호
        /// </summary>
        public string HomeZipCode
        {
            get { return homeZipCode; }
            set { homeZipCode = value; }
        }

        /// <summary>
        /// 집 주소1
        /// </summary>
        public string HomeAddress1
        {
            get { return homeAddress1; }
            set { homeAddress1 = value; }
        }


        /// <summary>
        /// 집 주소2
        /// </summary>
        public string HomeAddress2
        {
            get { return homeAddress2; }
            set { homeAddress2 = value; }
        }

        /// <summary>
        /// 호적 우편번호
        /// </summary>
        public string BirthZipCode
        {
            get { return birthZipCode; }
            set { birthZipCode = value; }
        }

        /// <summary>
        /// 호적 주소1
        /// </summary>
        public string BirthAddress1
        {
            get { return birthAddress1; }
            set { birthAddress1 = value; }
        }

        /// <summary>
        /// 호적 주소2
        /// </summary>
        public string BirthAddress2
        {
            get { return birthAddress2; }
            set { birthAddress2 = value; }
        }

        /// <summary>
        /// 핸드폰 번호
        /// </summary>
        public string MobilePhone
        {
            get { return mobilePhone; }
            set { mobilePhone = value; }
        }

        /// <summary>
        /// 집전화번호
        /// </summary>
        public string HomePhone
        {
            get { return homePhone; }
            set { homePhone = value; }
        }


        /// <summary>
        /// 회사 전화번호
        /// </summary>
        public string OfficePhone
        {
            get { return officePhone; }
            set { officePhone = value; }
        }

        /// <summary>
        /// 회사 내선번호
        /// </summary>
        public string ExtensionOfficePhone
        {
            get { return extensionOfficePhone; }
            set { extensionOfficePhone = value; }
        }

        /// <summary>
        /// 팩스번호
        /// </summary>
        public string Fax
        {
            get { return fax; }
            set { fax = value; }
        }

        /// <summary>
        /// IP폰 번호
        /// </summary>
        public string IpPhone
        {
            get { return ipPhone; }
            set { ipPhone = value; }
        }

        /// <summary>
        /// 입사일자
        /// </summary>
        public string JoinDate
        {
            get { return joinDate; }
            set { joinDate = value; }
        }

        /// <summary>
        /// 퇴사일자
        /// </summary>
        public string RetireDate
        {
            get { return retireDate; }
            set { retireDate = value; }
        }

        /// <summary>
        /// 인장이미지 파일 위치 
        /// </summary>
        public string SealImage
        {
            get { return sealImage; }
            set { sealImage = value; }
        }

        /// <summary>
        /// 개인이미지 파일 위치
        /// </summary>
        public string PhotoImage
        {
            get { return photoImage; }
            set { photoImage = value; }
        }

        /// <summary>
        /// 개인디스크 할당공간
        /// </summary>
        public int PrivateQuota
        {
            get { return privateQuota; }
            set { privateQuota = value; }
        }


        /// <summary>
        /// Gets or sets the mail quota.
        /// </summary>
        /// <value>The mail quota.</value>
        public int MailQuota
        {
            get { return mailQuota; }
            set { mailQuota = value; }
        }

        /// <summary>
        /// 전자결재 지문인식 여부
        /// </summary>
        public string SecurityApproval
        {
            get { return securityApproval; }
            set { securityApproval = value; }
        }

        /// <summary>
        /// 로그인시 지문인식 여부
        /// </summary>
        public string SecurityLogIn
        {
            get { return securityLogIn; }
            set { securityLogIn = value; }
        }


        /// <summary>
        /// 기본 메일 아이디
        /// </summary>
        public string MailID
        {
            get { return mailID; }
            set { mailID = value; }
        }

        /// <summary>
        /// 기본 메일 도메인
        /// </summary>
        public string MailDomain
        {
            get { return mailDomain; }
            set { mailDomain = value; }
        }

        /// <summary>
        /// 기본 메일 패스워드
        /// </summary>
        public string MailPassword
        {
            get { return mailPassword; }
            set { mailPassword = value; }
        }

        /// <summary>
        /// 사용중
        /// </summary>
        public string ActiveUser
        {
            get { return activeUser; }
            set { activeUser = value; }
        }

        /// <summary>
        /// 최초 입력자 ID
        /// </summary>
        public int WriteUserID
        {
            get { return writeUserID; }
            set { writeUserID = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ModifyUserID
        {
            get { return modifyUserID; }
            set { modifyUserID = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime WeddingAnniversary
        {
            get { return weddingAnniversary; }
            set { weddingAnniversary = value; }
        }

        /// <summary>
        /// Gets or sets the user dept list.
        /// </summary>
        /// <value>The user dept list.</value>
        public ArrayList UserDeptList
        {
            get { return userDeptList; }
            set { userDeptList = value; }
        }

        #endregion

    }
}