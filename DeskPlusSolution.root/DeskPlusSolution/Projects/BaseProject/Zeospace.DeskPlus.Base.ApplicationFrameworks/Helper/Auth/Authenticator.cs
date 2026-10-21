using System;
using System.Web;
using System.Web.Security;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth
{
    /// <summary>
    /// 작성자   : 심재운
    /// 작성일자 : 2008-08-08
    /// 작성목적 : Authenticator
    /// </summary>
    /// <remarks></remarks>
    public class Authenticator
    {
        //If you're using .NET 4 (or higher), you can use the System.Lazy<T> type to make the laziness really simple. 
        //All you need to do is pass a delegate to the constructor which calls the Singleton constructor 
        //which is done most easily with a lambda expression. 

        /// <summary>
        /// Prevents a default instance of the <see cref="Authenticator"/> class from being created.
        /// </summary>
        private Authenticator() { }

        /// <summary>
        /// The lazy
        /// </summary>
        private static readonly Lazy<Authenticator> lazy =
            new Lazy<Authenticator>(() => new Authenticator());

        ///// <summary>
        ///// 작성자   : 심재운
        ///// 작성일자 : 2008-08-08
        ///// 작성목적 : 인스턴스 초기화
        ///// </summary>
        public static Authenticator GetInstance() { return lazy.Value; }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-08-08
        /// 작성목적 : 로그아웃
        /// </summary>
        /// <param name="_loginID">The _login ID.</param>
        /// <remarks></remarks>
        private void LogOut(string _loginID)
        {
            HttpContext context = HttpContext.Current;
            context.Response.Cookies[_loginID].Value = "";
            context.Response.Cookies[_loginID].Path = "/";
            context.Response.Cookies[_loginID].Expires = DateTime.Now.AddYears(-1);
            context.Session["DeskPlusEIP"] = null;
            context.Session["DeskPlusEIPSkin"] = null;
            context.Session.Clear();
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();

            //context.Response.Redirect("/DeskPlusEIP/Default.aspx");
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-08-08
        /// 인증 쿠키 정보 객체에 담아서 반환하기
        /// </summary>
        /// <param name="_loginID">The _login ID.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public UserInfo AuthenticatorInfo(string _loginID)
        {
            HttpContext context = HttpContext.Current;
            UserInfo userInfo = new UserInfo();

            try
            {
                FormsAuthenticationTicket ticket = null;

                //SESSION 값을 인증쿠키에 할당한다.
                string useSession = Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "UseSession", "N");
                if (useSession == "Y")
                {
                    //SESSION 에서 쿠키정보 가져와서 폼인증 쿠키 생성하기
                    if (context.Session["DeskPlusEIP"] != null) // Session 정보가 있다면
                    {
                        ticket = FormsAuthentication.Decrypt(context.Session["DeskPlusEIP"].ToString());
                    }
                }

                string cookieName = "DeskPlusEIP" + _loginID;

                // 인증쿠키값이 존재하지 않으면, 쿠키값으로 인증쿠키에 할당한다.
                if (ticket == null)
                {
                    if (context.Request.Cookies[cookieName] != null && context.Request.Cookies[cookieName].Value != "")
                    {
                        ticket = FormsAuthentication.Decrypt(context.Request.Cookies[cookieName].Value);
                        if (useSession == "Y") context.Session["DeskPlusEIP"] = context.Request.Cookies[cookieName].Value;
                    }
                }

                if (ticket != null) // Session 정보가 있다면
                {
                    /*
                     * [CompanyID],[UserID],[LogInID],[Name],[EnglishName],
                     * [ManyName],[EmployeeNumber],[MobilePhone],[MailID],[MailDomain],
                     * [PrivateQuota],[SecurityLogIn],[DeptID],[CodeTitleID],[CodeDutyID],
                     * [DeptName],[DeptManyName],[TitleName],[TitleManyName],[DutyName],
                     * [DutyManyName],[PreferredLanguage],[DesignStyle],[BaseMenu],Authrity
                     * UserDepts, Roles, GetRootDeptID, GetUpDeptIDInfo
                     */

                    String[] userInfoStr = ticket.UserData.Split(new char[] { '┐' });
                    if (userInfoStr[1].Trim() == "") LogOut(cookieName); //사용자아이디
                    if (userInfoStr[0].Trim() == "") LogOut(cookieName); //회사아이디

                    userInfo.CompanyID = userInfoStr[0].ToNumber<int>();
                    userInfo.UserID = userInfoStr[1].ToNumber<int>();
                    userInfo.LogInID = userInfoStr[2].ToNull();
                    userInfo.Name = userInfoStr[3].ToNull();
                    userInfo.EmployeeNumber = userInfoStr[4].ToNull();
                    userInfo.MobilePhone = userInfoStr[5].ToNull();

                    userInfo.MailID = userInfoStr[6].ToNull();
                    userInfo.MailDomain = userInfoStr[7].ToNull();
                    userInfo.PrivateQuota = userInfoStr[8].ToNumber<int>();
                    userInfo.SecurityLogIn = userInfoStr[9].ToNull();
                    userInfo.DeptID = userInfoStr[10].ToNumber<int>();

                    userInfo.CodeTitleID = userInfoStr[11].ToNumber<int>();
                    userInfo.CodeDutyID = userInfoStr[12].ToNumber<int>();
                    userInfo.DeptName = userInfoStr[13].ToNull();
                    userInfo.TitleName = userInfoStr[14].ToNull();
                    userInfo.DutyName = userInfoStr[15].ToNull();

                    userInfo.PreferredLanguage = (userInfoStr[16].ToNull() == "") ? "ko" : userInfoStr[16];
                    //userInfo.DesignStyle = userInfoStr[17].ToNull();
                    userInfo.BaseMenu = userInfoStr[18].ToNull();
                    userInfo.UserDepts = (userInfoStr[19].ToNull() != "") ? userInfoStr[19].TrimEnd(',').Split(',') : null;
                    userInfo.Roles = (userInfoStr[20].ToNull() != "") ? userInfoStr[20].TrimEnd(',').Split(',') : null;
                    userInfo.RoleIDs = (userInfoStr[21].ToNull() != "") ? userInfoStr[21].TrimEnd(',').Split(',') : null;

                    userInfo.RootDeptID = userInfoStr[22].ToNumber<int>();
                    userInfo.UpDeptIDInfo = userInfoStr[23].ToNull();
                    userInfo.IsSysAdmin = (userInfoStr[24].ToNull() == "1") ? true : false;
                    userInfo.PasswordPure = userInfoStr[25].ToNull();
                    userInfo.Password = userInfoStr[26].ToNull();
                    userInfo.BaseTimeZone = userInfoStr[27].ToNull();
                    userInfo.BirthAddress1 = userInfoStr[28].ToNull();

                    #region 보류

                    /*
                    //오류가 낳을경우 기본 언어를 en 으로 지정
                    string defaultLang = Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "DefaultLanguage", "en").ToString();

                    string currentLang = "";

                    //기본값이 없을 경우
                    if (userInfo.PreferredLanguage == "")
                    {
                        // 브라우저 및 os 의 다국어 환경을 가져온다.
                        if (context.Request.UserLanguages != null)
                        {
                            //브라우저 및 os 의 환경 첫번째 값을 가져온다.
                            //window 7 경우 ko-kr 식으로 되어 있으므로 이를 짧은 언어로 치환한다.
                            currentLang = UtilHelper.CommonLanguage(context.Request.UserLanguages[0]);
                        }
                        else
                        {
                            //브라우저값을 찾지 못하면, 환경정보의 다국어값을 사용한다.
                            currentLang = defaultLang;
                        }
                    }
                    else
                    {
                        currentLang = userInfo.PreferredLanguage; //db 에 사용자가 선택한 다국어 값이 존재할 경우
                    }

                    //기준 언어(XML 의 1차언어)는 NAME 필드를 사용한다.
                    if (currentLang.ToUpper() == defaultLang.ToUpper())
                    {
                        userInfo.Name = userInfoStr[3].ToNull(); //기본 필드 이름
                        if (userInfo.Name == "") userInfo.Name = userInfoStr[4].ToNull(); //영문이름
                    }
                    else
                    {
                        userInfo.Name = userInfoStr[4].ToNull(); //영문이름
                        if (userInfo.Name == "") userInfo.Name = userInfoStr[3].ToNull(); //기본 이름
                    }
                     */

                    #endregion 보류
                }
                else
                {
                    //SESSION 이나 쿠키값이 존재하지 않으면 로그아웃 페이지로 이동시킨다.
                    return null;
                }

                return userInfo;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AuthenticatorInfo [" + _loginID + "] : ", ex);

                return null;
            }
        }
    }
}