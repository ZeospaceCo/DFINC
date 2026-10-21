using System;
using System.Collections;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth
{
    /// <summary>
    /// 작성자   : Freeman
    /// 작성일자 : 2010-11-15
    /// 작성목적 : 사용자 인증
    /// 수정일자 :
    /// 수정내용 :
    /// </summary>
    [Serializable]
    public class UserInfo
    {
        #region Property

        ///
        //CompanyID
        ///
        public int CompanyID { get; set; }

        ///
        //UserID
        ///
        public int UserID { get; set; }

        ///
        //LogInID
        ///
        public string LogInID { get; set; }

        /// <summary>
        /// 비밀번호(단방향)
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 순수패스워드
        /// </summary>
        public string PasswordPure { get; set; }

        public string Name { get; set; }

        public string DeptName { get; set; }

        public string TitleName { get; set; }

        public string DutyName { get; set; }

        ///
        //EmployeeNumber
        ///
        public string EmployeeNumber { get; set; }

        ///
        //MobilePhone
        ///
        public string MobilePhone { get; set; }

        ///
        //MailID
        ///
        public string MailID { get; set; }

        ///
        //MailDomain
        ///
        public string MailDomain { get; set; }

        ///
        //PrivateQuota
        ///
        public int PrivateQuota { get; set; }

        ///
        //SecurityLogIn
        ///
        public string SecurityLogIn { get; set; }

        ///
        //DeptID
        ///
        public int DeptID { get; set; }

        ///
        //CodeTitleID
        ///
        public int CodeTitleID { get; set; }

        ///
        //CodeDutyID
        ///
        public int CodeDutyID { get; set; }

        /// <summary>
        /// 사용자 부서
        /// </summary>
        /// <value>
        /// The user depts.
        /// </value>
        public string[] UserDepts { get; set; }

        /// <summary>
        /// 역할 코드값들
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public string[] Roles { get; set; }

        /// <summary>
        /// 역할 아이디값
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public string[] RoleIDs { get; set; }

        /// <summary>
        /// sysadmin 인지 확인 여부
        /// </summary>
        public bool IsSysAdmin { get; set; }

        /// <summary>
        /// 역할 리스트
        /// </summary>
        /// <value>
        /// The roles list.
        /// </value>
        public ArrayList RolesList
        {
            get
            {
                string[] roles = Roles;
                ArrayList rolesList = new ArrayList();
                for (int i = 0; i < roles.Length; i++)
                {
                    rolesList.Add(roles[i]);
                }

                return rolesList;
            }
        }

        /// <summary>
        /// 역할 여부
        /// </summary>
        /// <param name="_role">The _role.</param>
        /// <returns>
        ///   <c>true</c> if [is in role] [the specified _role]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsInRole(string _role)
        {
            if (Roles == null) return false;

            bool response = false;
            for (int i = 0; i < Roles.Length; i++)
            {
                //나의 역할 == 메뉴의 역할 비교
                if (Roles[i] == _role)
                {
                    response = true;
                    break;
                }
            }

            return response;
        }

        /// <summary>
        /// Root 부서 ID
        /// </summary>
        /// <value>The root dept ID.</value>
        public int RootDeptID { get; set; }

        /// <summary>
        /// 상위부서아이디값들
        /// </summary>
        /// <value>The root dept ID.</value>
        public string UpDeptIDInfo { get; set; }

        /// <summary>
        /// PreferredLanguage
        /// </summary>
        /// <value>The preferred language.</value>
        public string PreferredLanguage { get; set; }

        /// <summary>
        /// 기본 메뉴
        /// </summary>
        /// <value>The base menu.</value>
        public string BaseMenu { get; set; }

        /// <summary>
        /// 표준시간대
        /// </summary>
        public string BaseTimeZone { get; set; }

        public string BirthAddress1 { get; set; }

        #endregion Property
    }
}