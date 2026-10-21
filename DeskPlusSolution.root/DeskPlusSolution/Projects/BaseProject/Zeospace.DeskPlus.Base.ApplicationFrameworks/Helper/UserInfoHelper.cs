using System;
using System.Collections;
using System.Web;
using System.Web.Security;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper
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
        private string name;				//사용자 이름
        private string password;			//로그인 패스워드
        private string employeeNumber;		//사번
        private string mobilePhone;			//휴대폰 번호
        private int deptID;					//그룹 ID
        private string deptName;			//그룹 명
        private int codeTitleID;			//직급 ID
        private string titleName;			//직급 명
        private int codeDutyID;				//직책 ID
        private string dutyName;			//직책 명
        private string mailID;				//메일 ID
        private string mailDomain;			//메일 도메인
        private string mailPassword;		//메일 패스워드
        private int privateQuota;			//개인 사용량
        private string authrity;			//허용된 관리자 권한
        private string userDepts;			//겸직 부서 ID
        private int companyQuota;			//회사 사용량
        private string roles;				//권한 접속정보
        private int rootDeptID;				//Root DeptID
        private string preferredLanguage;	//기본 언어
        private string designStyle;			//기본 스타일
        private string baseMenu;			//기본 메뉴
        private string englishName;			//영어이름
        private string securityLogin;		//보안인증키

        #endregion 변수 선언

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
            string cookieName = "DeskPlusEIP" + _loginID;
            HttpContext context = HttpContext.Current;
            context.Response.Cookies[cookieName].Value = "";
            context.Response.Cookies[cookieName].Path = "/";
            context.Response.Cookies[cookieName].Expires = DateTime.Now.AddYears(-1);
            context.Session.Clear();
            FormsAuthentication.SignOut();
            context.Response.Redirect("/DeskPlusEIP/Default.aspx");
        }

        /// <summary>
        /// 개인정보를 암호화된 form 인증 ticket 의 Cookie에서 가져오기
        ///    0         1         2            3          4             5        6           7               8          9           10           11           12          13          14        15		   16           17         18                19				20			21                        22                    23          24               25							26               27			  28
        /// userID : loginID : companyID : companyName : companyCode : name : password : employeeNumber : mobilePhone : deptID : deptName : codeTitleID : TitleName :  codeDutyID : DutyName : mailID : mailDomain : mailPassword : privateQuota : companyQuota : Authority : userDept(겸직 부서 ID) : rols(Role 정보) : EnglishName : SecurityLogin : rootDeptID(Root DeptID) :  preferredLanguage : designStyle : baseMenu
        /// </summary>
        /// <param name="_loginID">인증아이디(userid 가 될수 있고, loginID 가 될수 있다.)</param>
        public UserInfoHelper(string _loginID)
        {
            HttpContext context = HttpContext.Current;
            try
            {
                string cookieName = "DeskPlusEIP" + _loginID;
                FormsAuthenticationTicket ticket = null;

                string useSession = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "UseSession", "");
                if (useSession == "Y")
                {
                    if (context.Session[cookieName] != null) // Session 정보가 있다면
                    {
                        ticket = FormsAuthentication.Decrypt(context.Session[cookieName].ToString());
                    }
                    else if (context.Request.Cookies[cookieName] != null && context.Request.Cookies[cookieName].Value != "") // Cookie 정보가 있다면
                    {
                        ticket = FormsAuthentication.Decrypt(context.Request.Cookies[cookieName].Value);
                    }
                    else if (context.Request.Cookies[cookieName] != null && context.Request.Cookies[cookieName].Value != "") // Cookie 정보가 있다면 (하위 호환성 유지를 위하여)
                    {
                        ticket = FormsAuthentication.Decrypt(context.Request.Cookies[cookieName].Value);//20070818 이전 개발된 내용 (하위 호환성 유지를 위하여)
                    }
                }
                else if (context.Request.Cookies[cookieName] != null && context.Request.Cookies[cookieName].Value != "") // Cookie 정보가 있다면
                {
                    ticket = FormsAuthentication.Decrypt(context.Request.Cookies[cookieName].Value);
                }
                else if (context.Request.Cookies[cookieName] != null && context.Request.Cookies[cookieName].Value != "") // //20070818 이전 개발된 내용 (하위 호환성 유지를 위하여)
                {
                    ticket = FormsAuthentication.Decrypt(context.Request.Cookies[cookieName].Value);//20070818 이전 개발된 내용 (하위 호환성 유지를 위하여)
                }
                //else   //무한 루프에 빠짐
                //{
                //    //로그아웃 처리
                //    LogOut(_loginID);
                //}

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

                    string defaultLang = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "DefaultLanguage", "").ToString();

                    string currentLang = "";
                    //기본어(언어 == 기본어)
                    if (userInfo[26] == "")
                    {
                        if (context.Request.UserLanguages != null)
                        {
                            string lang = context.Request.UserLanguages[0].ToLower();

                            if (lang == "zh" || lang == "zh-cn" || lang == "zh-hans" || lang == "zh-sg")  //zh-Hans(중국어간체)
                            {
                                lang = "zh-cn";
                            }
                            else if (lang == "zh-tw" || lang == "zh-hant" || lang == "zh-mo")  //번체
                            {
                                lang = "zh-tw";
                            }
                            else if (lang == "ja" || lang == "ja-jp") //일본어
                            {
                                lang = "ja";
                            }
                            else if (lang == "ko" || lang == "ko-kr") //한국어
                            {
                                lang = "ko";
                            }
                            else //나머지는 영어
                            {
                                lang = "en";
                            }

                            currentLang = lang;
                        }
                        else
                        {
                            currentLang = defaultLang;
                        }
                    }
                    else
                    {
                        currentLang = userInfo[26];
                    }

                    if (currentLang.ToUpper() == defaultLang.ToUpper())
                    {
                        name = userInfo[5]; //기본이름
                        if (name == "") name = userInfo[23]; //영문이름
                    }
                    else
                    {
                        name = userInfo[23]; //영문이름
                        if (name == "") name = userInfo[5]; //기본 이름
                    }

                    //name			= userInfo[5];
                    password = userInfo[6];
                    employeeNumber = userInfo[7];
                    mobilePhone = userInfo[8];

                    if (userInfo[9].Trim() == "")
                    {
                        deptID = 0;
                    }
                    else
                    {
                        deptID = Convert.ToInt32(userInfo[9]);
                    }

                    deptName = userInfo[10];

                    if (userInfo[11].Trim() == "")
                    {
                        codeTitleID = 0;
                    }
                    else
                    {
                        codeTitleID = Convert.ToInt32(userInfo[11]);
                    }

                    titleName = userInfo[12];

                    if (userInfo[13].Trim() == "")
                    {
                        codeDutyID = 0;
                    }
                    else
                    {
                        codeDutyID = Convert.ToInt32(userInfo[13]);
                    }

                    dutyName = userInfo[14];
                    mailID = userInfo[15];
                    mailDomain = userInfo[16];
                    mailPassword = userInfo[17];

                    if (userInfo[18].Trim() == "")
                    {
                        privateQuota = 0;
                    }
                    else
                    {
                        privateQuota = Convert.ToInt32(userInfo[18]);
                    }

                    if (userInfo[19].Trim() == "")
                    {
                        companyQuota = 0;
                    }
                    else
                    {
                        companyQuota = Convert.ToInt32(userInfo[19]);
                    }

                    authrity = userInfo[20];
                    userDepts = userInfo[21];
                    roles = userInfo[22];

                    englishName = userInfo[23];
                    securityLogin = userInfo[24];

                    if (userInfo[25].Trim() == "")
                    {
                        rootDeptID = 0;
                    }
                    else
                    {
                        rootDeptID = Convert.ToInt32(userInfo[25]);
                    }

                    preferredLanguage = userInfo[26];
                    designStyle = userInfo[27];
                    baseMenu = userInfo[28];

                }
                else
                {
                    FormsAuthentication.RedirectToLoginPage();
                    //context.Response.Redirect("/DeskPlusEIP/Default.aspx");
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Helper.UserInfoHelper 2 [" + _loginID + "] : ", ex);

                loginID = null;
            }
        }

        #endregion 생성자

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
        /// 휴대폰 번호
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
        /// 직급 코드 ID
        /// </summary>
        public int CodeTitleID
        {
            get { return codeTitleID; }
        }

        /// <summary>
        /// 직급 명
        /// </summary>
        public string TitleName
        {
            get { return titleName; }
        }

        /// <summary>
        /// 직책 코드 ID
        /// </summary>
        public int CodeDutyID
        {
            get { return codeDutyID; }
        }

        /// <summary>
        /// 직책 명
        /// </summary>
        public string DutyName
        {
            get { return dutyName; }
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
        /// 영어이름
        /// </summary>
        /// <value>The name of the english.</value>
        public string EnglishName
        {
            get { return englishName; }
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
        /// 권한
        /// </summary>
        public string[] Authrity
        {
            get
            {
                if (authrity != null)
                {
                    return authrity.Split('|');
                }
                else
                {
                    return null;
                }
            }
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
        /// Root 부서 ID
        /// </summary>
        public int RootDeptID
        {
            get
            {
                return rootDeptID;
            }
        }

        /// <summary>
        /// PreferredLanguage
        /// </summary>
        public string PreferredLanguage
        {
            get { return preferredLanguage; }
        }

        /// <summary>
        /// 다지인 스타일
        /// </summary>
        public string DesignStyle
        {
            get { return designStyle; }
        }

        /// <summary>
        /// 기본 메뉴
        /// </summary>
        public string BaseMenu
        {
            get { return baseMenu; }
        }

        #endregion Property

        #region Method

        /// <summary>
        /// 권한여부
        /// </summary>
        /// <param name="_auth"></param>
        /// <returns></returns>
        public bool IsInAuthrity(string _auth)
        {
            if (authrity != null)
            {
                string[] authrityList = authrity.Split('|');
                bool isContains = false;
                for (int i = 0; i < authrityList.Length; i++)
                {
                    if (authrityList[i] == _auth)
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

        /// <summary>
        /// 사용자의 언어 리소스 상황값
        /// </summary>
        /// <returns></returns>
        public string PreLang()
        {
            string preLang = String.Empty;

            //클아이언트 환경 가져오기 - 다국어에서 사용
            string loginID = HttpContext.Current.User.Identity.Name;
            Helper.UserInfoHelper userInfo = new Helper.UserInfoHelper(loginID);

            if (userInfo.PreferredLanguage != null) preLang = userInfo.PreferredLanguage;

            if (preLang.Trim() == "" || preLang == null)
            {
                preLang = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "DefaultLanguage", "");

                if (preLang.ToUpper().StartsWith("EN")) preLang = "en";
                if (preLang.ToUpper().StartsWith("FR")) preLang = "fr";
                if (preLang.ToUpper().StartsWith("ZH") && preLang.ToUpper() != "ZH-TW") preLang = "zh-cn";

                if (preLang.ToUpper() != "EN" && preLang.ToUpper() != "ZH-CN" && preLang.ToUpper() != "ZH-TW" && preLang.ToUpper() != "KO" && preLang.ToUpper() != "JA" && preLang.ToUpper() != "FR")
                {
                    preLang = "en";
                }
            }

            return preLang;
        }

        #endregion Method
    }
}