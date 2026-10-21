using System;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Collections;

namespace Zeospace.DeskPlusMobile.Web.Helper
{
    /// <summary>
    /// UserInfoHelper에 대한 요약 설명입니다.
    /// </summary>
    public class UserInfoHelper
    {
        #region 변수 선언
        private int userID;					//사용자 ID
        private string loginID;				//사용자 접속명
        private int companyID;				//회사 ID
        private string companyName;			//회사 명
        private string companyCode;			//회사 코드
        private string name;			    //사용자 이름
        private string nameBase;
        private string nameKo;
        private string nameEn;
        private string nameJa;
        private string nameFr;
        private string nameZhCn;
        private string nameZhTw;
        private string password;			//로그인 패스워드
        private string employeeNumber;		//사번
        private string mobilePhone;			//핸드폰 번호
        private int deptID;					//그룹 ID
        private string deptName;			//부서 명
        private string deptBase;			//부서 명
        private string deptKo;
        private string deptEn;
        private string deptJa;
        private string deptFr;
        private string deptZhCn;
        private string deptZhTw;
        private int codeTitleID;			//직급 ID
        private string titleName;			//직급 명
        private string titleBase;
        private string titleKo;
        private string titleEn;
        private string titleJa;
        private string titleFr;
        private string titleZhCn;
        private string titleZhTw;
        private int codeDutyID;				//직책 ID
        private string dutyName;			//직책 명
        private string dutyBase;
        private string dutyKo;
        private string dutyEn;
        private string dutyJa;
        private string dutyFr;
        private string dutyZhCn;
        private string dutyZhTw;
        private string mailID;				//메일 ID
        private string mailDomain;			//메일 도메인
        private string mailPassword;		//메일 패스워드
        private int privateQuota;			//개인 사용량
        private string userDepts;			//겸직 부서 ID
        private int companyQuota;			//회사 사용량
        private string roles;				//권한 접속정보
        private string menuRoles;           //메뉴권한정보
        private string securityLogin;		//보안인증키
        private string preferredLanguage;	//기본 언어
        #endregion

        #region 생성자
        /// <summary>
        /// 생성자
        /// </summary>
        public UserInfoHelper()
        {
            new UserInfoHelper("Guest_Join");
        }

        private void LogOut(string _loginID)
        {
            HttpContext context = HttpContext.Current;
            context.Response.Cookies["DeskPlusMobile_" + _loginID].Value = "";
            context.Response.Cookies["DeskPlusMobile_" + _loginID].Path = "/";
            context.Response.Cookies["DeskPlusMobile_" + _loginID].Expires = DateTime.Now.AddYears(-1);
            context.Session.Clear();
            //FormsAuthentication.SignOut();
            //FormsAuthentication.RedirectToLoginPage();            
            context.Response.Redirect("/DeskPlusMobile/Portal/LogOut.aspx", false);

        }

        /// <summary>
        /// 개인정보를 암호화된 form 인증 ticket 의 Cookie에서 가져오기 
        /// 개인정보 가져오기
        ///    0               1               2                    3                           4              5              6                         7                        8               9     
        /// userID : loginID : companyID : companyName : companyCode : name : password : employeeNumber : mobilePhone : deptID 
        ///         10                11                12                13               14            15		   16                       17                     18                19			
        /// deptName : codeTitleID : TitleName :  codeDutyID : DutyName : mailID : mailDomain : mailPassword : privateQuota : companyQuota : 
        ///	       20                                   21                 22                        23             
        /// userDept(겸직 부서 ID) : rols(Role 정보) :  EnglishName : SecurityLogin
        /// </summary>
        /// <param name="_loginID">인증아이디(userid 가 될수 있고, loginID 가 될수 있다.)</param>
        public UserInfoHelper(string _loginID)
        {
            try
            {

                HttpContext context = HttpContext.Current;
                try
                {

                    FormsAuthenticationTicket ticket = null;

                    string useSession = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "CONFIG_USESESSION");
                    if (useSession.ToUpper() == "TRUE")
                    {
                        if (context.Session["DeskPlusMobile_" + _loginID] != null) // Session 정보가 있다면
                        {
                            ticket = FormsAuthentication.Decrypt(context.Session["DeskPlusMobile_" + _loginID].ToString());
                        }
                        else if (context.Request.Cookies["DeskPlusMobile_" + _loginID] != null && context.Request.Cookies["DeskPlusMobile_" + _loginID].Value != "") // Cookie 정보가 있다면 (하위 호환성 유지를 위하여)
                        {
                            ticket = FormsAuthentication.Decrypt(context.Request.Cookies["DeskPlusMobile_" + _loginID].Value);//20070818 이전 개발된 내용 (하위 호환성 유지를 위하여)
                        }
                    }
                    else if (context.Request.Cookies["DeskPlusMobile_" + _loginID] != null && context.Request.Cookies["DeskPlusMobile_" + _loginID].Value != "") // //20070818 이전 개발된 내용 (하위 호환성 유지를 위하여)
                    {
                        ticket = FormsAuthentication.Decrypt(context.Request.Cookies["DeskPlusMobile_" + _loginID].Value);//20070818 이전 개발된 내용 (하위 호환성 유지를 위하여)
                    }

                    if (ticket != null) // Session 정보가 있다면
                    {
                        String[] userInfo = ticket.UserData.Split(new char[] { '┐' });

                        if (userInfo[0].Trim() == "")
                        {
                            userID = 0;
                            //로그아웃 처리
                            LogOut(_loginID);
                        }
                        else
                        {
                            userID = Convert.ToInt32(userInfo[0]);
                        }

                        loginID = userInfo[1];

                        if (userInfo[2].Trim() == "")
                        {
                            companyID = 0;
                            //로그아웃 처리
                            LogOut(_loginID);
                        }
                        else
                        {
                            companyID = Convert.ToInt32(userInfo[2]);
                        }

                        companyName = userInfo[3];
                        companyCode = userInfo[4];

                        nameBase = userInfo[5];
                        nameKo = userInfo[6];
                        nameEn = userInfo[7];
                        nameJa = userInfo[8];
                        nameFr = userInfo[9];
                        nameZhCn = userInfo[10];
                        nameZhTw = userInfo[11];

                        name = StringHelper.GetLangName(nameBase, nameKo, nameEn, nameJa, nameFr, nameZhCn, nameZhTw, userInfo[44]);

                        password = userInfo[12];
                        employeeNumber = userInfo[13];
                        mobilePhone = userInfo[14];

                        if (userInfo[15].Trim() == "")
                        {
                            deptID = 0;
                        }
                        else
                        {
                            deptID = Convert.ToInt32(userInfo[15]);
                        }

                        deptBase = userInfo[16];
                        deptKo = userInfo[17];
                        deptEn = userInfo[18];
                        deptJa = userInfo[19];
                        deptFr = userInfo[20];
                        deptZhCn = userInfo[21];
                        deptZhTw = userInfo[22];

                        deptName = StringHelper.GetLangName(deptBase, deptKo, deptEn, deptJa, deptFr, deptZhCn, deptZhTw, userInfo[44]);

                        if (userInfo[23].Trim() == "")
                        {
                            codeTitleID = 0;
                        }
                        else
                        {
                            codeTitleID = Convert.ToInt32(userInfo[23]);
                        }

                        titleBase = userInfo[24];
                        titleKo = userInfo[25];
                        titleEn = userInfo[26];
                        titleJa = userInfo[27];
                        titleFr = userInfo[28];
                        titleZhCn = userInfo[29];
                        titleZhTw = userInfo[30];

                        titleName = StringHelper.GetLangName(titleBase, titleKo, titleEn, titleJa, titleFr, titleZhCn, titleZhTw, userInfo[44]);

                        if (userInfo[31].Trim() == "")
                        {
                            codeDutyID = 0;
                        }
                        else
                        {
                            codeDutyID = Convert.ToInt32(userInfo[31]);
                        }

                        dutyBase = userInfo[32];
                        dutyKo = userInfo[33];
                        dutyEn = userInfo[34];
                        dutyJa = userInfo[35];
                        dutyFr = userInfo[36];
                        dutyZhCn = userInfo[37];
                        dutyZhTw = userInfo[38];

                        dutyName = StringHelper.GetLangName(dutyBase, dutyKo, dutyEn, dutyJa, dutyFr, dutyZhCn, dutyZhTw, userInfo[44]);

                        mailID = userInfo[39];
                        mailDomain = userInfo[40];
                        mailPassword = userInfo[41];

                        if (userInfo[42].Trim() == "")
                        {
                            privateQuota = 0;
                        }
                        else
                        {
                            privateQuota = Convert.ToInt32(userInfo[42]);
                        }

                        if (userInfo[43].Trim() == "")
                        {
                            companyQuota = 0;
                        }
                        else
                        {
                            companyQuota = Convert.ToInt32(userInfo[43]);
                        }

                        preferredLanguage = userInfo[44];
                        userDepts = userInfo[45];
                        roles = userInfo[46];
                        menuRoles = userInfo[47];
                        securityLogin = userInfo[48];
                    }
                    else
                    {
                        FormsAuthentication.RedirectToLoginPage();
                    }
                }
                catch (Exception ex)
                {
                    Helper.AppException.WriteException("Helper.UserInfoHelper [" + _loginID + "] : ", ex);

                    loginID = null;
                }
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("Helper.UserInfoHelper [" + _loginID + "] : ", ex);
            }
        }

        #endregion

        #region Property
        /// <summary>
        /// 사용자 내부 코드 ID
        /// </summary>
        public int UserID
        {
            get { return userID; }
        }

        /// <summary>
        /// 사용자 로그인 ID
        /// </summary>
        public string LoginID
        {
            get { return loginID; }
        }

        /// <summary>
        /// 회사 내부 코드 ID
        /// </summary>
        public int CompanyID
        {
            get { return companyID; }
        }

        /// <summary>
        /// 회사 명
        /// </summary>
        public string CompanyName
        {
            get { return companyName; }
        }

        /// <summary>
        /// 회사 명
        /// </summary>
        public string CompanyCode
        {
            get { return companyCode; }
        }

        /// <summary>
        /// 사용자 이름
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// 사용자 이름
        /// </summary>
        public string NameBase
        {
            get { return nameBase; }
        }

        /// <summary>
        /// 사용자 이름
        /// </summary>
        public string NameKo
        {
            get { return nameKo; }
        }

        /// <summary>
        /// 사용자 이름
        /// </summary>
        public string NameEn
        {
            get { return nameEn; }
        }

        /// <summary>
        /// 사용자 이름
        /// </summary>
        public string NameJa
        {
            get { return nameJa; }
        }

        /// <summary>
        /// 사용자 이름
        /// </summary>
        public string NameFr
        {
            get { return nameFr; }
        }

        /// <summary>
        /// 사용자 이름
        /// </summary>
        public string NameZhCn
        {
            get { return nameZhCn; }
        }

        /// <summary>
        /// 사용자 이름
        /// </summary>
        public string NameZhTw
        {
            get { return nameZhTw; }
        }

        /// <summary>
        /// 로그인 패스워드
        /// </summary>
        public string Password
        {
            get { return password; }
        }

        /// <summary>
        /// 사원 번호
        /// </summary>
        public string EmployeeNumber
        {
            get { return employeeNumber; }
        }

        /// <summary>
        /// 핸드폰 번호
        /// </summary>
        public string MobilePhone
        {
            get { return mobilePhone; }
        }

        /// <summary>
        /// 그룹 코드 ID
        /// </summary>
        public int DeptID
        {
            get { return deptID; }
        }

        /// <summary>
        /// 그룹 명
        /// </summary>
        public string DeptName
        {
            get { return deptName; }
        }

        /// <summary>
        /// 그룹 명
        /// </summary>
        public string DeptBase
        {
            get { return deptBase; }
        }

        /// <summary>
        /// 그룹 명
        /// </summary>
        public string DeptKo
        {
            get { return deptKo; }
        }

        /// <summary>
        /// 그룹 명
        /// </summary>
        public string DeptEn
        {
            get { return deptEn; }
        }

        /// <summary>
        /// 그룹 명
        /// </summary>
        public string DeptJa
        {
            get { return deptJa; }
        }

        /// <summary>
        /// 그룹 명
        /// </summary>
        public string DeptFr
        {
            get { return deptFr; }
        }

        /// <summary>
        /// 그룹 명
        /// </summary>
        public string DeptZhCn
        {
            get { return deptZhCn; }
        }

        /// <summary>
        /// 그룹 명
        /// </summary>
        public string DeptZhTw
        {
            get { return deptZhTw; }
        }

        /// <summary>
        /// 직급 코드 ID
        /// </summary>
        public int CodeTitleID
        {
            get { return codeTitleID; }
        }

        /// <summary>
        /// 직급명
        /// </summary>
        public string TitleName
        {
            get { return titleName; }
        }

        /// <summary>
        /// 직급명
        /// </summary>
        public string TitleBase
        {
            get { return titleBase; }
        }

        /// <summary>
        /// 직급명
        /// </summary>
        public string TitleKo
        {
            get { return titleKo; }
        }

        /// <summary>
        /// 직급명
        /// </summary>
        public string TitleEn
        {
            get { return titleEn; }
        }

        /// <summary>
        /// 직급명
        /// </summary>
        public string TitleJa
        {
            get { return titleJa; }
        }

        /// <summary>
        /// 직급명
        /// </summary>
        public string TitleFr
        {
            get { return titleFr; }
        }

        /// <summary>
        /// 직급명
        /// </summary>
        public string TitleZhCn
        {
            get { return titleZhCn; }
        }

        /// <summary>
        /// 직급명
        /// </summary>
        public string TitleZhTw
        {
            get { return titleZhTw; }
        }

        /// <summary>
        /// 직책 코드 ID
        /// </summary>
        public int CodeDutyID
        {
            get { return codeDutyID; }
        }

        /// <summary>
        /// 직책명
        /// </summary>
        public string DutyName
        {
            get { return dutyName; }
        }

        /// <summary>
        /// 직책명
        /// </summary>
        public string DutyBase
        {
            get { return dutyBase; }
        }

        /// <summary>
        /// 직책명
        /// </summary>
        public string DutyKo
        {
            get { return dutyKo; }
        }

        /// <summary>
        /// 직책명
        /// </summary>
        public string DutyEn
        {
            get { return dutyEn; }
        }

        /// <summary>
        /// 직책명
        /// </summary>
        public string DutyJa
        {
            get { return dutyJa; }
        }

        /// <summary>
        /// 직책명
        /// </summary>
        public string DutyFr
        {
            get { return dutyFr; }
        }

        /// <summary>
        /// 직책명
        /// </summary>
        public string DutyZhCn
        {
            get { return dutyZhCn; }
        }

        /// <summary>
        /// 직책명
        /// </summary>
        public string DutyZhTw
        {
            get { return dutyZhTw; }
        }


        /// <summary>
        /// 메일 ID
        /// </summary>
        public string MailID
        {
            get { return mailID; }
        }

        /// <summary>
        /// 메일 도메인
        /// </summary>
        public string MailDomain
        {
            get { return mailDomain; }
        }

        /// <summary>
        /// 메일 패스워드
        /// </summary>
        public string MailPassword
        {
            get { return mailPassword; }
        }

        /// <summary>
        /// 개인 사용량
        /// </summary>
        public int PrivateQuota
        {
            get { return privateQuota; }
        }

        /// <summary>
        /// 회사 사용량
        /// </summary>
        public int CompanyQuota
        {
            get { return companyQuota; }
        }


        /// <summary>
        /// 보안인증키값
        /// </summary>
        /// <value>The security login.</value>
        public string SecurityLogin
        {
            get { return securityLogin; }
        }

        /// <summary>
        /// 사용자 부서
        /// </summary>
        public string[] UserDepts
        {
            get
            {
                if (userDepts != null)
                {

                    return userDepts.Split('|');
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 역할
        /// </summary>
        public string[] Roles
        {
            get
            {
                if (roles != null && roles != "")
                {
                    return roles.Split('|');
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 메뉴권한
        /// </summary>
        public string[] MenuRoles
        {
            get
            {
                if (menuRoles != null && menuRoles != "")
                {
                    return menuRoles.Split('|');
                }
                else
                {
                    return null;
                }
            }
        }


        /// <summary>
        /// 역할 리스트
        /// </summary>
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
        /// PreferredLanguage
        /// </summary>
        public string PreferredLanguage
        {
            get { return preferredLanguage; }
        }
        #endregion

        #region Method
        /// <summary>
        /// 역할 여부 : string
        /// </summary>
        /// <param name="_role"></param>
        /// <returns></returns>
        public bool IsInRole(string _role)
        {
            if (roles != null)
            {
                string[] roleList = roles.Split('|');//나의 역할값들
                bool isContains = false;
                for (int i = 0; i < roleList.Length; i++)
                {
                    //나의 역할 == 메뉴의 역할 비교
                    if (roleList[i] == _role)
                    {
                        isContains = true;
                    }
                }

                return isContains;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 역할 여부 : arraylist
        /// </summary>
        /// <param name="_role"></param>
        /// <returns></returns>
        public bool IsInRole(ArrayList _role)
        {

            ArrayList roleList = RolesList;
            if (roleList == null) return false;

            for (int i = 0; i < _role.Count; i++)
            {
                if (roleList.Contains(_role[i].ToString())) return true;
            }

            return false;
        }



        #endregion
    }
}
