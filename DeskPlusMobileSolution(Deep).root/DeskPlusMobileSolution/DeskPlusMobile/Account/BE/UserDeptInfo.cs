using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Account.BE
{
    /// <summary>
    /// 조직에 할당된 사용자 정보 (겸직 가능) Entity
    /// </summary>
    public class UserDeptInfo
    {
        #region 필드

        /// <summary>
        /// 사용자내부코드
        /// </summary>
        private int userID;

        /// <summary>
        /// 회사내부코드
        /// </summary>
        private int companyID;

        /// <summary>
        /// 부서내부코드
        /// </summary>
        private int deptID;


        /// <summary>
        /// 부서명
        /// </summary>
        private string deptName;

        /// <summary>
        /// 직급코드
        /// </summary>
        private int codeTitleID;


        /// <summary>
        /// 직급명
        /// </summary>
        private string titleName;

        /// <summary>
        /// 직책코드
        /// </summary>
        private int codeDutyID;

        /// <summary>
        /// 직책명
        /// </summary>
        private string dutyName;

        /// <summary>
        /// 담당업무
        /// </summary>
        private string baseDept;

        /// <summary>
        /// 사용자 부서 순서 (호봉)
        /// </summary>
        private int userDeptOrder;

        /// <summary>
        /// 메일 ID
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
        /// 입력자 내부코드
        /// </summary>
        private int writeUserID;

        private string rootDeptName;
        private string rootDeptEnglishName;
        private string userName;
        private string userEnglishName;
        #endregion

        #region 생성자


        /// <summary>
        /// Initializes a new instance of the <see cref="UserDeptInfo"/> class.
        /// </summary>
        public UserDeptInfo() { }



        /// <summary>
        /// Initializes a new instance of the <see cref="UserDeptInfo"/> class.
        /// </summary>
        /// <param name="_userID"></param>
        /// <param name="_companyID"></param>
        /// <param name="_deptID"></param>
        /// <param name="_deptName"></param>
        /// <param name="_codeTitleID"></param>
        /// <param name="_titleName"></param>
        /// <param name="_codeDutyID"></param>
        /// <param name="_dutyName"></param>
        /// <param name="_baseDept"></param>
        /// <param name="_mailID"></param>
        /// <param name="_mailDomain"></param>
        /// <param name="_mailPassword"></param>
        /// <param name="_writeUserID"></param>
        public UserDeptInfo(int _userID, int _companyID, int _deptID, string _deptName, int _codeTitleID, string _titleName, int _codeDutyID, string _dutyName, string _baseDept, string _mailID,
            string _mailDomain, string _mailPassword, int _writeUserID)
        {
            this.deptID = _deptID;
            this.deptName = _deptName;
            this.codeTitleID = _codeTitleID;
            this.titleName = _titleName;
            this.codeDutyID = _codeDutyID;
            this.dutyName = _dutyName;
            this.baseDept = _baseDept;
            this.mailID = _mailID;
            this.mailDomain = _mailDomain;
            this.mailPassword = _mailPassword;
            this.writeUserID = _writeUserID;
        }

        #endregion

        #region 프로퍼티

        /// <summary>
        /// 사용자 내부코드
        /// </summary>
        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }


        /// <summary>
        /// 회사 내부코드
        /// </summary>
        public int CompanyID
        {
            get { return companyID; }
            set { companyID = value; }
        }


        /// <summary>
        /// 부서내부코드
        /// </summary>
        public int DeptID
        {
            get { return deptID; }
            set { deptID = value; }
        }

        /// <summary>
        /// Gets or sets the name of the dept.
        /// </summary>
        /// <value>The name of the dept.</value>
        public string DeptName
        {
            get { return deptName; }
            set { deptName = value; }
        }

        /// <summary>
        /// 직급코드
        /// </summary>
        public int CodeTitleID
        {
            get { return codeTitleID; }
            set { codeTitleID = value; }
        }

        /// <summary>
        /// 직급명
        /// </summary>
        public string TitleName
        {
            get { return titleName; }
            set { titleName = value; }
        }

        /// <summary>
        /// 직책코드
        /// </summary>
        public int CodeDutyID
        {
            get { return codeDutyID; }
            set { codeDutyID = value; }
        }

        /// <summary>
        /// 직책명
        /// </summary>
        public string DutyName
        {
            get { return dutyName; }
            set { dutyName = value; }
        }

        /// <summary>
        /// 기본 조직
        /// </summary>
        public string BaseDept
        {
            get { return baseDept; }
            set { baseDept = value; }
        }


        /// <summary>
        /// 부서사용자 순서
        /// </summary>
        public int UserDeptOrder
        {
            get { return userDeptOrder; }
            set { userDeptOrder = value; }
        }

        /// <summary>
        /// 메일 ID
        /// </summary>
        public string MailID
        {
            get { return mailID; }
            set { mailID = value; }
        }

        /// <summary>
        /// 메일 도메인
        /// </summary>

        public string MailDomain
        {
            get { return mailDomain; }
            set { mailDomain = value; }
        }

        /// <summary>
        /// 메일 패스워드
        /// </summary>
        public string MailPassword
        {
            get { return mailPassword; }
            set { mailPassword = value; }
        }

        /// <summary>
        /// 최초입력자 ID
        /// </summary>
        public int WriteUserID
        {
            get { return writeUserID; }
            set { writeUserID = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        public string RootDeptName
        {
            get { return rootDeptName; }
            set { rootDeptName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string RootDeptEnglishName
        {
            get { return rootDeptEnglishName; }
            set { rootDeptEnglishName = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserEnglishName
        {
            get { return userEnglishName; }
            set { userEnglishName = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        #endregion
    }
}