using System;

namespace Zeospace.DeskPlus.Account.BE
{
    [Serializable]
    public class UserInfo
    {
        public UserInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfo"/> class.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_companyCode">The _company code.</param>
        /// <param name="_comBase">The _com base.</param>
        /// <param name="_comKo">The _com ko.</param>
        /// <param name="_comEn">The _com en.</param>
        /// <param name="_comJa">The _com ja.</param>
        /// <param name="_comFr">The _com fr.</param>
        /// <param name="_comZhcn">The _com ZHCN.</param>
        /// <param name="_comZhtw">The _com ZHTW.</param>
        /// <param name="_companyQuota">The _company quota.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_logInID">The _log in ID.</param>
        /// <param name="_nameBase">The _name base.</param>
        /// <param name="_nameKo">The _name ko.</param>
        /// <param name="_nameEn">The _name en.</param>
        /// <param name="_nameJa">The _name ja.</param>
        /// <param name="_nameFr">The _name fr.</param>
        /// <param name="_nameZhCn">The _name zh cn.</param>
        /// <param name="_nameZhTw">The _name zh tw.</param>
        /// <param name="_deptBase">The _dept base.</param>
        /// <param name="_deptKo">The _dept ko.</param>
        /// <param name="_deptEn">The _dept en.</param>
        /// <param name="_deptJa">The _dept ja.</param>
        /// <param name="_deptFr">The _dept fr.</param>
        /// <param name="_deptZhCn">The _dept zh cn.</param>
        /// <param name="_deptZhTw">The _dept zh tw.</param>
        /// <param name="_titleBase">The _title base.</param>
        /// <param name="_titleKo">The _title ko.</param>
        /// <param name="_titleEn">The _title en.</param>
        /// <param name="_titleJa">The _title ja.</param>
        /// <param name="_titleFr">The _title fr.</param>
        /// <param name="_titleZhCn">The _title zh cn.</param>
        /// <param name="_titleZhTw">The _title zh tw.</param>
        /// <param name="_dutyBase">The _duty base.</param>
        /// <param name="_dutyKo">The _duty ko.</param>
        /// <param name="_dutyEn">The _duty en.</param>
        /// <param name="_dutyJa">The _duty ja.</param>
        /// <param name="_dutyFr">The _duty fr.</param>
        /// <param name="_dutyZhCn">The _duty zh cn.</param>
        /// <param name="_dutyZhTw">The _duty zh tw.</param>
        /// <param name="_password">The _password.</param>
        /// <param name="_employeeNumber">The _employee number.</param>
        /// <param name="_mobilePhone">The _mobile phone.</param>
        /// <param name="_mail">The _mail.</param>
        /// <param name="_mailID">The _mail ID.</param>
        /// <param name="_mailDomain">The _mail domain.</param>
        /// <param name="_mailPassword">The _mail password.</param>
        /// <param name="_privateQuota">The _private quota.</param>
        /// <param name="_securityLogIn">The _security log in.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <param name="_parentDeptID">The _parent dept ID.</param>
        /// <param name="_codeTitleID">The _code title ID.</param>
        /// <param name="_codeDutyID">The _code duty ID.</param>
        /// <param name="_baseDept">The _base dept.</param>
        /// <param name="_userDeptOrder">The _user dept order.</param>
        /// <param name="_titleCodeOrder">The _title code order.</param>
        /// <param name="_dutyCodeOrder">The _duty code order.</param>
        /// <param name="_preferredLanguage">The _preferred language.</param>
        /// <param name="_designStyle">The _design style.</param>
        /// <param name="_baseMenu">The _base menu.</param>
        /// <param name="_userCode">The _user code.</param>
        /// <param name="_passwordUpdateDate">The _password update date.</param>
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
        /// <param name="_homePhone">The _home phone.</param>
        /// <param name="_officePhone">The _office phone.</param>
        /// <param name="_extensionOfficePhone">The _extension office phone.</param>
        /// <param name="_fax">The _fax.</param>
        /// <param name="_ipPhone">The _ip phone.</param>
        /// <param name="_joinDate">The _join date.</param>
        /// <param name="_retireDate">The _retire date.</param>
        /// <param name="_sealImage">The _seal image.</param>
        /// <param name="_photoImage">The _photo image.</param>
        /// <param name="_mailQuota">The _mail quota.</param>
        /// <param name="_securityApproval">The _security approval.</param>
        /// <param name="_activeUser">The _active user.</param>
        /// <param name="_weddingAnniversary">The _wedding anniversary.</param>
        /// <param name="_writeUserID">The _write user ID.</param>
        /// <param name="_writeDate">The _write date.</param>
        /// <param name="_mobilePortalDesign">The _mobile portal design.</param>
        /// <param name="_mobileActionMenu">The _mobile action menu.</param>
        /// <param name="_mailDesign">The _mail design.</param>
        public UserInfo(
        int _companyID, string _companyCode
            , string _comBase, string _comKo, string _comEn, string _comJa, string _comFr, string _comZhcn, string _comZhtw
            , int _companyQuota, int _userID, string _logInID
            , string _nameBase, string _nameKo, string _nameEn, string _nameJa, string _nameFr, string _nameZhCn, string _nameZhTw
            , string _deptBase, string _deptKo, string _deptEn, string _deptJa, string _deptFr, string _deptZhCn, string _deptZhTw
            , string _titleBase, string _titleKo, string _titleEn, string _titleJa, string _titleFr, string _titleZhCn, string _titleZhTw
            , string _dutyBase, string _dutyKo, string _dutyEn, string _dutyJa, string _dutyFr, string _dutyZhCn, string _dutyZhTw
            , string _password, string _employeeNumber, string _mobilePhone, string _mail, string _mailID
            , string _mailDomain, string _mailPassword, int _privateQuota, string _securityLogIn, int _deptID
            , int _parentDeptID, int _codeTitleID, int _codeDutyID, string _baseDept, int _userDeptOrder
            , int _titleCodeOrder, int _dutyCodeOrder, string _preferredLanguage, string _designStyle, string _baseMenu
            , string _userCode, DateTime _passwordUpdateDate, string _registrationNumber, string _sex, string _lunar
            , string _birthDay, string _description, string _homeZipCode, string _homeAddress1, string _homeAddress2
            , string _birthZipCode, string _birthAddress1, string _birthAddress2, string _homePhone, string _officePhone
            , string _extensionOfficePhone, string _fax, string _ipPhone, string _joinDate, string _retireDate
            , string _sealImage, string _photoImage, int _mailQuota, string _securityApproval, string _activeUser
            , string _weddingAnniversary, int _writeUserID, DateTime _writeDate, string _mobilePortalDesign, string _mobileActionMenu, string _mailDesign
            , string _authEmail
            /*, string _alarmMail, string _alarmMessenger, string _alarmSMS*/
            , string _alarmApproval, string _alarmBoard, string _alarmPims, string _alarmTask, string _alarmWork, string _alarmCommunity, string _alarmRM, string _alarmSNS
            , string _baseTimeZone, string _otp, string _otpYN)
        {
            this.CompanyID = _companyID;
            this.CompanyCode = _companyCode;

            this.ComBase = _comBase;
            this.ComKo = _comKo;
            this.ComEn = _comEn;
            this.ComJa = _comJa;
            this.ComFr = _comFr;
            this.ComZhcn = _comZhcn;
            this.ComZhtw = _comZhtw;
            this.CompanyQuota = _companyQuota;

            this.UserID = _userID;
            this.LogInID = _logInID;

            this.NameBase = _nameBase;
            this.NameKo = _nameKo;
            this.NameEn = _nameEn;
            this.NameJa = _nameJa;
            this.NameFr = _nameFr;
            this.NameZhCn = _nameZhCn;
            this.NameZhTw = _nameZhTw;

            this.DeptBase = _deptBase;
            this.DeptKo = _deptKo;
            this.DeptEn = _deptEn;
            this.DeptJa = _deptJa;
            this.DeptFr = _deptFr;
            this.DeptZhCn = _deptZhCn;
            this.DeptZhTw = _deptZhTw;

            this.TitleBase = _titleBase;
            this.TitleKo = _titleKo;
            this.TitleEn = _titleEn;
            this.TitleJa = _titleJa;
            this.TitleFr = _titleFr;
            this.TitleZhCn = _titleZhCn;
            this.TitleZhTw = _titleZhTw;

            this.DutyBase = _dutyBase;
            this.DutyKo = _dutyKo;
            this.DutyEn = _dutyEn;
            this.DutyJa = _dutyJa;
            this.DutyFr = _dutyFr;
            this.DutyZhCn = _dutyZhCn;
            this.DutyZhTw = _dutyZhTw;

            this.Password = _password;
            this.EmployeeNumber = _employeeNumber;
            this.MobilePhone = _mobilePhone;
            this.Mail = _mail;
            this.MailID = _mailID;
            this.MailDomain = _mailDomain;
            this.MailPassword = _mailPassword;
            this.PrivateQuota = _privateQuota;
            this.SecurityLogIn = _securityLogIn;
            this.DeptID = _deptID;
            this.ParentDeptID = _parentDeptID;
            this.CodeTitleID = _codeTitleID;
            this.CodeDutyID = _codeDutyID;
            this.BaseDept = _baseDept;
            this.UserDeptOrder = _userDeptOrder;
            this.TitleCodeOrder = _titleCodeOrder;
            this.DutyCodeOrder = _dutyCodeOrder;
            this.PreferredLanguage = _preferredLanguage;
            this.DesignStyle = _designStyle;
            this.BaseMenu = _baseMenu;
            this.UserCode = _userCode;
            this.PasswordUpdateDate = _passwordUpdateDate;
            this.RegistrationNumber = _registrationNumber;
            this.Sex = _sex;
            this.Lunar = _lunar;
            this.BirthDay = _birthDay;
            this.Description = _description;
            this.HomeZipCode = _homeZipCode;
            this.HomeAddress1 = _homeAddress1;
            this.HomeAddress2 = _homeAddress2;
            this.BirthZipCode = _birthZipCode;
            this.BirthAddress1 = _birthAddress1;
            this.BirthAddress2 = _birthAddress2;
            this.HomePhone = _homePhone;
            this.OfficePhone = _officePhone;
            this.ExtensionOfficePhone = _extensionOfficePhone;
            this.Fax = _fax;
            this.IpPhone = _ipPhone;
            this.JoinDate = _joinDate;
            this.RetireDate = _retireDate;
            this.SealImage = _sealImage;
            this.PhotoImage = _photoImage;
            this.MailQuota = _mailQuota;
            this.SecurityApproval = _securityApproval;
            this.ActiveUser = _activeUser;
            this.WeddingAnniversary = _weddingAnniversary;
            this.WriteUserID = _writeUserID;
            this.WriteDate = _writeDate;
            this.MobilePortalDesign = _mobilePortalDesign;
            this.MobileActionMenu = _mobileActionMenu;
            this.MailDesign = _mailDesign;
            this.AuthEmail = _authEmail;

            //this.AlarmMail = _alarmMail;
            //this.AlarmMessenger = _alarmMessenger;
            //this.AlarmSMS = _alarmSMS;
            this.AlarmApproval = _alarmApproval;
            this.AlarmBoard = _alarmBoard;
            this.AlarmPims = _alarmPims;
            this.AlarmTask = _alarmTask;
            this.AlarmWork = _alarmWork;
            this.AlarmCommunity = _alarmCommunity;
            this.AlarmRM = _alarmRM;
            this.AlarmSNS = _alarmSNS;
            this.BaseTimeZone = _baseTimeZone;
            this.OTP = _otp;
            this.OTPYN = _otpYN;
        }

        // VW_UserInfo
        /// <summary>
        ///CompanyID
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        ///CompanyCode
        /// </summary>
        public string CompanyCode { get; set; }

        public string ComBase { get; set; }

        /// <summary>
        ///ComKo
        /// </summary>
        public string ComKo { get; set; }

        /// <summary>
        ///ComEn
        /// </summary>
        public string ComEn { get; set; }

        /// <summary>
        ///ComJa
        /// </summary>
        public string ComJa { get; set; }

        /// <summary>
        ///ComFr
        /// </summary>
        public string ComFr { get; set; }

        /// <summary>
        ///ComZhcn
        /// </summary>
        public string ComZhcn { get; set; }

        /// <summary>
        ///ComZhtw
        /// </summary>
        public string ComZhtw { get; set; }

        /// <summary>
        ///CompanyQuota
        /// </summary>
        public int CompanyQuota { get; set; }

        /// <summary>
        ///UserID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        ///LogInID
        /// </summary>
        public string LogInID { get; set; }

        /// <summary>
        ///Name
        /// </summary>
        public string Name { get; set; }

        public string NameBase { get; set; }

        /// <summary>
        /// NameKo
        /// </summary>
        /// <value>
        /// The name ko.
        /// </value>
        public string NameKo { get; set; }

        /// <summary>
        ///NameEn
        /// </summary>
        public string NameEn { get; set; }

        /// <summary>
        ///NameJa
        /// </summary>
        public string NameJa { get; set; }

        /// <summary>
        ///NameFr
        /// </summary>
        public string NameFr { get; set; }

        /// <summary>
        ///NameZhCn
        /// </summary>
        public string NameZhCn { get; set; }

        /// <summary>
        ///NameZhTw
        /// </summary>
        public string NameZhTw { get; set; }

        /// <summary>
        ///DeptName
        /// </summary>
        public string DeptName { get; set; }

        public string DeptBase { get; set; }

        /// <summary>
        ///DeptKo
        /// </summary>
        public string DeptKo { get; set; }

        /// <summary>
        ///DeptEn
        /// </summary>
        public string DeptEn { get; set; }

        /// <summary>
        ///DeptJa
        /// </summary>
        public string DeptJa { get; set; }

        /// <summary>
        ///DeptFr
        /// </summary>
        public string DeptFr { get; set; }

        /// <summary>
        ///DeptZhCn
        /// </summary>
        public string DeptZhCn { get; set; }

        /// <summary>
        ///DeptZhTw
        /// </summary>
        public string DeptZhTw { get; set; }

        public string TitleBase { get; set; }

        /// <summary>
        ///TitleName
        /// </summary>
        public string TitleName { get; set; }

        /// <summary>
        ///TitleKo
        /// </summary>
        public string TitleKo { get; set; }

        /// <summary>
        ///TitleEn
        /// </summary>
        public string TitleEn { get; set; }

        /// <summary>
        ///TitleJa
        /// </summary>
        public string TitleJa { get; set; }

        /// <summary>
        ///TitleFr
        /// </summary>
        public string TitleFr { get; set; }

        /// <summary>
        ///TitleZhCn
        /// </summary>
        public string TitleZhCn { get; set; }

        /// <summary>
        ///TitleZhTw
        /// </summary>
        public string TitleZhTw { get; set; }

        /// <summary>
        ///DutyKo
        /// </summary>
        public string DutyName { get; set; }

        public string DutyBase { get; set; }

        /// <summary>
        ///DutyKo
        /// </summary>
        public string DutyKo { get; set; }

        /// <summary>
        ///DutyEn
        /// </summary>
        public string DutyEn { get; set; }

        /// <summary>
        ///DutyJa
        /// </summary>
        public string DutyJa { get; set; }

        /// <summary>
        ///DutyFr
        /// </summary>
        public string DutyFr { get; set; }

        /// <summary>
        ///DutyZhCn
        /// </summary>
        public string DutyZhCn { get; set; }

        /// <summary>
        ///DutyZhTw
        /// </summary>
        public string DutyZhTw { get; set; }

        /// <summary>
        ///Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///EmployeeNumber
        /// </summary>
        public string EmployeeNumber { get; set; }

        /// <summary>
        ///MobilePhone
        /// </summary>
        public string MobilePhone { get; set; }

        /// <summary>
        ///Mail
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        ///MailID
        /// </summary>
        public string MailID { get; set; }

        /// <summary>
        ///MailDomain
        /// </summary>
        public string MailDomain { get; set; }

        /// <summary>
        ///MailPassword
        /// </summary>
        public string MailPassword { get; set; }

        /// <summary>
        ///PrivateQuota
        /// </summary>
        public int PrivateQuota { get; set; }

        /// <summary>
        ///SecurityLogIn
        /// </summary>
        public string SecurityLogIn { get; set; }

        /// <summary>
        ///DeptID
        /// </summary>
        public int DeptID { get; set; }

        /// <summary>
        ///ParentDeptID
        /// </summary>
        public int ParentDeptID { get; set; }

        /// <summary>
        ///CodeTitleID
        /// </summary>
        public int CodeTitleID { get; set; }

        /// <summary>
        ///CodeDutyID
        /// </summary>
        public int CodeDutyID { get; set; }

        /// <summary>
        ///BaseDept
        /// </summary>
        public string BaseDept { get; set; }

        /// <summary>
        ///UserDeptOrder
        /// </summary>
        public int UserDeptOrder { get; set; }

        /// <summary>
        ///TitleCodeOrder
        /// </summary>
        public int TitleCodeOrder { get; set; }

        /// <summary>
        ///DutyCodeOrder
        /// </summary>
        public int DutyCodeOrder { get; set; }

        /// <summary>
        ///PreferredLanguage
        /// </summary>
        public string PreferredLanguage { get; set; }

        /// <summary>
        ///DesignStyle
        /// </summary>
        public string DesignStyle { get; set; }

        /// <summary>
        ///BaseMenu
        /// </summary>
        public string BaseMenu { get; set; }

        /// <summary>
        ///UserCode
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        ///PasswordUpdateDate
        /// </summary>
        public DateTime PasswordUpdateDate { get; set; }

        /// <summary>
        ///RegistrationNumber
        /// </summary>
        public string RegistrationNumber { get; set; }

        /// <summary>
        ///Sex
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        ///Lunar
        /// </summary>
        public string Lunar { get; set; }

        /// <summary>
        ///BirthDay
        /// </summary>
        public string BirthDay { get; set; }

        /// <summary>
        ///Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///HomeZipCode
        /// </summary>
        public string HomeZipCode { get; set; }

        /// <summary>
        ///HomeAddress1
        /// </summary>
        public string HomeAddress1 { get; set; }

        /// <summary>
        ///HomeAddress2
        /// </summary>
        public string HomeAddress2 { get; set; }

        /// <summary>
        ///BirthZipCode
        /// </summary>
        public string BirthZipCode { get; set; }

        /// <summary>
        ///BirthAddress1
        /// </summary>
        public string BirthAddress1 { get; set; }

        /// <summary>
        ///BirthAddress2
        /// </summary>
        public string BirthAddress2 { get; set; }

        /// <summary>
        ///HomePhone
        /// </summary>
        public string HomePhone { get; set; }

        /// <summary>
        ///OfficePhone
        /// </summary>
        public string OfficePhone { get; set; }

        /// <summary>
        ///ExtensionOfficePhone
        /// </summary>
        public string ExtensionOfficePhone { get; set; }

        /// <summary>
        ///Fax
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        ///IpPhone
        /// </summary>
        public string IpPhone { get; set; }

        /// <summary>
        ///JoinDate
        /// </summary>
        public string JoinDate { get; set; }

        /// <summary>
        ///RetireDate
        /// </summary>
        public string RetireDate { get; set; }

        /// <summary>
        ///SealImage
        /// </summary>
        public string SealImage { get; set; }

        /// <summary>
        ///PhotoImage
        /// </summary>
        public string PhotoImage { get; set; }

        /// <summary>
        ///MailQuota
        /// </summary>
        public int MailQuota { get; set; }

        /// <summary>
        ///SecurityApproval
        /// </summary>
        public string SecurityApproval { get; set; }

        /// <summary>
        ///ActiveUser
        /// </summary>
        public string ActiveUser { get; set; }

        /// <summary>
        ///WeddingAnniversary
        /// </summary>
        public string WeddingAnniversary { get; set; }

        /// <summary>
        ///WriteUserID
        /// </summary>
        public int WriteUserID { get; set; }

        /// <summary>
        ///WriteDate
        /// </summary>
        public DateTime WriteDate { get; set; }

        /// <summary>
        /// MobilePortalDesign
        /// </summary>
        /// <value>
        /// The mobile portal design.
        /// </value>
        public string MobilePortalDesign { get; set; }

        /// <summary>
        ///MobileActionMenu
        /// </summary>
        public string MobileActionMenu { get; set; }

        /// <summary>
        /// 오늘 댓글 카운트
        /// </summary>
        /// <value>
        /// The today count.
        /// </value>
        public int TodayCount { get; set; }

        /// <summary>
        /// 메일스킨정보
        /// </summary>
        /// <value>
        /// The mail design.
        /// </value>
        public string MailDesign { get; set; }

        /// <summary>
        /// 패스워드 분실시 사용할 외부 이메일
        /// </summary>
        public string AuthEmail { get; set; }

        /// <summary>
        /// 알람사용여부(메일) - 1 / 0
        /// </summary>
        public string AlarmMail { get; set; }
        /// <summary>
        /// 알람사용여부(메신저) - 1 / 0
        /// </summary>
        public string AlarmMessenger { get; set; }
        
        /// <summary>
        /// 알람사용여부(Mobile) - 1 / 0
        /// </summary>
        //public string AlarmMobile { get; set; }

        /// <summary>
        /// 알람사용여부(SMS) - 1 / 0
        /// </summary>
        public string AlarmSMS { get; set; }

        /// <summary>
        /// 알람사용여부(전자결재) : 1-Messenger,Mail,Mobile / 0-
        /// </summary>
        public string AlarmApproval { get; set; }
        /// <summary>
        /// 알람사용여부(게시판) : 1-Messenger,Mail,Mobile / 0-
        /// </summary>
        public string AlarmBoard { get; set; }
        /// <summary>
        /// 알람사용여부(일정관리) : 1-Messenger,Mail,Mobile / 0-
        /// </summary>
        public string AlarmPims { get; set; }
        /// <summary>
        /// 알람사용여부(업무협업) : 1-Messenger,Mail,Mobile / 0-
        /// </summary>
        public string AlarmTask { get; set; }
        /// <summary>
        /// 알람사용여부(업무일지) : 1-Messenger,Mail,Mobile / 0-
        /// </summary>
        public string AlarmWork { get; set; }
        /// <summary>
        /// 알람사용여부(커뮤니티) : 1-Messenger,Mail,Mobile / 0-
        /// </summary>
        public string AlarmCommunity { get; set; }
        /// <summary>
        /// 알람사용여부(자원관리) : 1-Messenger,Mail,Mobile / 0-
        /// </summary>
        public string AlarmRM { get; set; }
        /// <summary>
        /// 알람사용여부(SNS) : 1-Messenger,Mail,Mobile / 0-
        /// </summary>
        public string AlarmSNS { get; set; }
        /// <summary>
        /// 표준 시간대
        /// </summary>
        public string BaseTimeZone { get; set; }

        public string OTP { get; set; }

        public string OTPYN { get; set; }
    }
}