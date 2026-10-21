using System;
using System.Collections;
using System.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Zeospace.DeskPlus.Account.BE;
using System.Reflection;

namespace Zeospace.DeskPlus.Base.Web.Site.Auth
{
    public partial class LegacyLogin : System.Web.UI.Page
    {
        protected System.Resources.ResourceManager resMng = null;
        public string ResourceName { get; set; }
        public Assembly AssemblyName { get; set; }
        private const string LoginSessionCookiePrefix = "DeskPlusEIPLoginSession";

        protected void Page_Load(object sender, System.EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Base.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Base.Web.Site.Auth.LegacyLogin).Assembly;	//어셈블리 선언
            #endregion

            GetResourceManager();

            if (!Page.IsPostBack)
            {
                DataLoad();
            }
        }

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        private void DataLoad()
        {
            int companyID = this.GetParm("cID", "0").ToNumber<int>();
            companyID = (companyID.Equals(0)) ? 1 : companyID;

            string userid = this.GetParm("userid", "");         //loginID,employeeNumber
            string password = this.GetParm("pwd", "");          //password = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(password));
            string nickName = this.GetParm("nickname", "");
            string legacyKey = this.GetParm("legacykey", "");
            string paramEtc = this.GetParm("parametc", "");
            string targetURL = this.GetParm("targeturl", "").Replace("$", "&");
            targetURL = (targetURL.Equals("")) ? "/DeskPlusEIP/Approval/FormFlow/Default.aspx" : targetURL;

            string visitLog = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.VisitorLog(HttpContext.Current);
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("visitLog : " + visitLog, AppSettingType.BASE);
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("userid : " + userid, AppSettingType.BASE);
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("password : " + password, AppSettingType.BASE);
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("nickName : " + nickName, AppSettingType.BASE);
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("legacyKey : " + legacyKey, AppSettingType.BASE);
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("paramEtc : " + paramEtc, AppSettingType.BASE);
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("targetURL : " + targetURL, AppSettingType.BASE);

            if (userid.Length > 0 && nickName.Length > 0 && targetURL.Length > 0)
            {
                string rtnval = SignInProcess(companyID, userid, password);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("SignInProcess : " + rtnval, AppSettingType.BASE);

                if (rtnval.Equals(""))
                {
                    //작성페이지로 넘기기. targetURL
                    Dictionary<string, string> paraDic = new Dictionary<string, string>();

                    paraDic.Add("listID", "LEGACY");
                    paraDic.Add("nickname", nickName);
                    paraDic.Add("legacykey", legacyKey);
                    paraDic.Add("parametc", paramEtc);
                    string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));

                    if (targetURL.IndexOf('?') > 0)
                    {
                        targetURL = targetURL.Split('?')[0] + "?p=" + encParams + "&" + targetURL.Split('?')[1];
                    }
                    else
                    {
                        targetURL = targetURL + "?p=" + encParams;
                    }

                    Response.Redirect(targetURL);
                }
                else
                {
                    //ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "NOAUTH", resMng.GetString("CheckIDandPwd"), "");
                    Response.Redirect("/DeskPlusEIP/Site/Auth/Logout.aspx");
                }
            }
        }

        #endregion

        #region Method (함수)


        /// <summary>
        /// 인증 처리 (enc가false면 타이핑pwd다)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_loginID">The _login ID.</param>
        /// <param name="_password">The _password.</param>
        /// <param name="_encPwd">if set to <c>true</c> [_enc PWD].</param>
        private string SignInProcess(int _companyID, string _loginID, string _password)
        {
            /* 
             * 1. 사용자 존재여부 확인. (Key : 아이디 / 사번 / 메일계정 등등)
             * 2. password 비교. (빈값이 아닌경우만)
             */

            int tmpCompanyID = 0, tmpUserID = 0;
            string tmpLoginID = "", tmpPwd = "";
            string errCode = "";

            /*
                errCode = -1 : 사용자는 있으나, 비밀번호 불일치.
             *  errCode = -2 : 사용자가 없음.
             */
            
            Account.DAL.User user = new Account.DAL.User();
            List<Account.BE.UserInfo> al = user.GetUserByLegacy(_companyID, _loginID);

            if (al.Count > 0)
            {
                foreach (Account.BE.UserInfo userInfo in al)
                {
                    tmpCompanyID = userInfo.CompanyID;
                    tmpUserID = userInfo.UserID;
                    tmpLoginID = userInfo.LogInID;
                    tmpPwd = userInfo.Password;
                }
            }
            else
            {
                errCode = "-2";         //사용자가 없음.
            }

            if (!_password.Equals(""))
            {
                string useHash = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "UseHashCheckByLegacyLogin", "N");   //패스워드가 Hash로 넘어오면 Y / PlainText로 넘어오면 N
                string hashOfPassword = (useHash.Equals("Y")) ? _password : ApplicationFrameworks.Helper.Security.HashManager.GetHash(_password);

                StringComparer comparer = StringComparer.OrdinalIgnoreCase;
                if (!comparer.Compare(hashOfPassword, tmpPwd).Equals(0))
                {
                    errCode = "-1";     //사용자는 있으나, 비밀번호 불일치.
                }
            }

            if (errCode.Equals(""))
            {
                //사용자 정보 저장
                SetUserInfo(tmpCompanyID, tmpLoginID, tmpPwd, true);

                //인증 쿠키 생성
                FormsAuthentication.SetAuthCookie(tmpLoginID, true);
                SetLoginSessionCookie(tmpLoginID);
                FormsAuthentication.RedirectFromLoginPage(tmpLoginID, false);
            }
            
            return errCode; // 성공일때는 빈값이 넘어감.
        }

        /// <summary>
        /// 작성자   : shimpark
        /// 작성일자 : 2010-11-15
        /// 작성목적 : 인증 정보 저장
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_loginID">The _login ID.</param>
        /// <param name="_password">The _password.</param>
        /// <param name="_encPwd">if set to <c>true</c> [_enc PWD].</param>
        private void SetUserInfo(int _companyID, string _loginID, string _password, bool _encPwd)
        {
            try
            {
                bool isAlreadyAuthenticatedUser = HasLoginSessionCookie(_loginID);

                //패스워드 만기
                ChangePwd(_companyID, _loginID, _password);

                //메일 쿠키 생성
                SetCookieMail(_companyID, _loginID);

                //사용자아이디 가져오기
                Account.DAL.User biz = new Account.DAL.User();
                int userID = biz.SignIn(_companyID, _loginID, _password, _encPwd);

                #region SPOOL 하위 폴더 삭제

                if (!isAlreadyAuthenticatedUser)
                {
                    string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                    string spoolFolder = baseFileDirectory + _companyID + "/Users/" + userID + "/spool/";
                    DirectoryInfo di = new DirectoryInfo(Server.MapPath(spoolFolder));

                    if (di.Exists)
                    {
                        string[] folder;
                        folder = Directory.GetDirectories(Server.MapPath(spoolFolder));
                        foreach (string folderName in folder) Directory.Delete(folderName, true);
                    }
                }


                #endregion

                Account.DAL.UserEnvironment bizEnv = new Account.DAL.UserEnvironment();
                List<UserEnvironmentInfo> alEnv = bizEnv.GetUserEnv(_companyID, userID);
                foreach (UserEnvironmentInfo al in alEnv) PreLangs = al.PreferredLanguage;

                //그룹웨어 쿠키 생성하기
                Zeospace.DeskPlus.Account.BLL.User userBiz = new Account.BLL.User();
                string userInfoStr = userBiz.GetUserCookieStr(_companyID, _loginID, _password, PreLangs);

                if (userInfoStr != "")
                {
                    // 인증티켓 발급
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                        1,                              // 버전 번호
                        _loginID,     // 티켓과 관련된 사용자 이름
                        DateTime.Now,                   // 쿠키가 발행된 시간
                        DateTime.Now.AddHours(24),       // 쿠키의 만료 날짜
                        true,                          // 영구 쿠키이면 true
                        userInfoStr                      // 개인정보
                        );

                    // 인증 티켓 암호화
                    String cookieStr = FormsAuthentication.Encrypt(ticket);

                    //SESSION 기술
                    string useSession = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "UseSession", "");
                    if (useSession == "Y")
                    {
                        //session 에 값 할당 하기
                        Session["DeskPlusEIP"] = cookieStr;
                    }

                    //COOKIE 기술
                    string cookieName = "DeskPlusEIP" + _loginID;
                    Response.Cookies[cookieName].Value = cookieStr;
                    Response.Cookies[cookieName].Path = "/";

                    string cookieExpiresHour = "0";
                    string cookieDomain = "";

                    try
                    {
                        cookieExpiresHour = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "CookieExpiresHour", "");
                        if (cookieExpiresHour != "")
                        {
                            Response.Cookies[cookieName].Expires = DateTime.Now.AddHours(Convert.ToInt32(cookieExpiresHour));
                        }

                        cookieDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "CookieDomain", "");
                        if (cookieDomain != "")
                        {
                            Response.Cookies[cookieName].Domain = cookieDomain;
                        }
                    }
                    catch (Exception ex)
                    {
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Default[SetUserInfo] 1 : " + ex);
                    }
                }

                SetAccessLog(_companyID, userID);//로그인 자 접속 정보 로그 쌓기
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Default[SetUserInfo] 2 : " + ex);
            }
        }


        /// <summary>
        /// 패스워드 변경기안 찾기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_loginID">The _login ID.</param>
        /// <param name="_password">The _password.</param>
        private void ChangePwd(int _companyID, string _loginID, string _password)
        {
            //패스워드 변경 일자
            int passwordChangeDay = AppSettingHelper.GetAppSetting(AppSettingType.ACCOUNT, "PasswordChangeByDay", "0").ToNumber<int>();

            if (passwordChangeDay > 0)
            {
                //어떤 사용자 인지 정보 가져오기
                Zeospace.DeskPlus.Common.DAL.UCMessenger dal = new Common.DAL.UCMessenger();
                DataSet ds = dal.GetUserPasswordDate(_companyID, _loginID, _password);
                if (ds != null)
                {
                    //현재 날짜
                    DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    TimeSpan gap;

                    //마지막으로 업데이트 한 날짜 
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DateTime dt = ds.Tables[0].Rows[0]["PasswordUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(ds.Tables[0].Rows[0]["PasswordUpdateDate"]);

                        gap = todayDate.Subtract(dt);

                        //xml 의 값 이상 차이 날 경우에는
                        if (gap.Days >= passwordChangeDay)
                        {

                            Dictionary<string, string> paraDic = new Dictionary<string, string>();
                            paraDic.Add("companyID", _companyID.ToString());
                            paraDic.Add("loginID", _loginID);
                            string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                            string url = "/DeskPlusEIP/Site/Auth/ModPassword.aspx?p=" + encParams;
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("패스워드업데이트 : " + url, AppSettingType.MAIL);
                            Response.Redirect(url);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 작성자   : shimpark
        /// 작성일자 : 2010-11-15
        /// 작성목적 : mail 쿠키 생성하기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_loginID">The _login ID.</param>
        private void SetCookieMail(int _companyID, string _loginID)
        {
            #region 제타메일용


            Zeospace.DeskPlus.Account.DAL.User user = new Account.DAL.User();
            List<Zeospace.DeskPlus.Account.BE.UserInfo> userList = user.GetUserByLoginID(_companyID, _loginID, true);

            string mailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "");
            if (mailProduct.Length == 0) return;

            try //20120419 ByFreeman 제타 호출 오류시 exception 처리
            {

                if (mailProduct.ToUpper() == "ZETTA65")
                {
                    Zeospace.DeskPlus.Common.BLL.MailZettaFactory biz = new Common.BLL.MailZettaFactory();

                    foreach (Zeospace.DeskPlus.Account.BE.UserInfo userInfo in userList)
                    {
                        string name = UtilHelper.GetLangName(userInfo.NameBase, userInfo.NameKo, userInfo.NameEn, userInfo.NameJa,
                            userInfo.NameFr, userInfo.NameZhCn, userInfo.NameZhTw, userInfo.PreferredLanguage);

                        string email = userInfo.MailID + "@" + userInfo.MailDomain;
                        string sendParams = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "Zetta65_ModDeptUser", "");

                        string encryptType = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "EncryptType", "");
                        if (encryptType.ToUpper() == "SHA1")
                        {
                            //제타에서 암호화 된 파라메터인지 체크를 하는 API가 추가 되었음 (120420)  1이면 암호화한것 0이면 안한것 
                            //굳이 로그인페이지에서 사용자 추가를 던지는 이유는 인사연동으로 인해 강제로 테이블에 사용자 정보가 Insert 되었을 경우 때문
                            ///멀티 컴퍼니로 인해 제타테이블에 들어가는 계정은 이메일 풀 주소가 된다 . deskplus@zeospace.com
                            biz.AddUser(userInfo.CompanyID, email, userInfo.Password, Server.UrlEncode(name), email, 1); //제타메일6 사용자 추가
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("email : " + email, AppSettingType.MAIL);
                        }

                        //메일의 자동화 완성용으로 파라미터 전송해야 함.
                        sendParams = string.Format(sendParams, email, userInfo.DeptID);

                        //이상하게 두번쏴야 된답니다. 허접 제타.~~
                        //biz.SendPost(sendParams);
                        biz.SendPost(sendParams);
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("sendParams : " + sendParams, AppSettingType.MAIL);
                    }
                }
                else if (mailProduct.ToUpper() == "ZETTA52")
                {

                    foreach (Zeospace.DeskPlus.Account.BE.UserInfo userInfo in userList)
                    {
                        Response.Cookies["ss_webmail_id"].Path = "/";
                        Response.Cookies["ss_webmail_id"]["Mail"] = userInfo.MailID + "@" + userInfo.MailDomain;
                        Response.Cookies["ss_webmail_id"]["LoginID"] = _loginID;
                        Response.Cookies["ss_webmail_id"]["UserID"] = userInfo.UserID.ToString();

                        string cookieExpiresHour = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "CookieExpiresHour", "");
                        if (cookieExpiresHour != "") Response.Cookies["ss_webmail_id"].Expires = DateTime.Now.AddHours(Convert.ToInt32(cookieExpiresHour));

                        string cookieDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "CookieDomain", "");
                        if (cookieDomain != "") Response.Cookies["ss_webmail_id"].Domain = cookieDomain;
                    }
                }
                else if (mailProduct.ToUpper().IndexOf("DEEP").Equals(0))
                {
                    Zeospace.DeskPlus.Common.BLL.MailDeepFactory biz = new Common.BLL.MailDeepFactory();
                    string mailpw = string.Empty;

                    foreach (Zeospace.DeskPlus.Account.BE.UserInfo userInfo in userList)
                    {
                        // 인사연동 시, mailpw 없는 경우가 있음 : GW의 pw를 넘김.
                        mailpw = (userInfo.MailPassword.Equals("")) ? userInfo.Password : userInfo.MailPassword;

                        bool rtnval = biz.AddUser(userInfo.CompanyID, userInfo.MailDomain, userInfo.MailID, mailpw, userInfo.NameBase, "","");
                        if (!rtnval)
                        {
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("LegacyLogin - SetCookieMail(DEEP인증Error) : mailID(" + userInfo.MailID + "), domain(" + userInfo.MailDomain + ")");
                        }
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("LegacyLogin - SetCookieMail(DEEP인증결과-" + rtnval.ToString() + ") : mailID(" + userInfo.MailID + "), domain(" + userInfo.MailDomain + ")", AppSettingType.MAIL);
                    }
                }

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Default[SetCookieMail]  : " + ex);
            }


            #endregion
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 로그인 자 접속 정보 로그 쌓기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        private void SetAccessLog(int _companyID, int _userID)
        {
            if (_companyID > 0 && _userID > 0)
            {
                string[] remoteAddrStr = Request.UserHostAddress.Split('.');
                string remoteAddr = UtilHelper.GetIPAddress(remoteAddrStr);

                Zeospace.DeskPlus.Common.DAL.AccessLog dal = new Common.DAL.AccessLog();
                Zeospace.DeskPlus.Common.BE.AccessLogInfo info = new Common.BE.AccessLogInfo();
                info.CompanyID = _companyID;
                info.UserID = _userID;
                info.IPAddress = remoteAddr;
                dal.AddLogin(info);
            }
        }

        private bool HasLoginSessionCookie(string loginID)
        {
            string loginSessionCookieName = GetLoginSessionCookieName(loginID);
            if (Session != null && Session[loginSessionCookieName] != null)
            {
                return true;
            }

            HttpCookie loginSessionCookie = Request.Cookies[loginSessionCookieName];
            if (loginSessionCookie != null && !string.IsNullOrEmpty(loginSessionCookie.Value))
            {
                return true;
            }

            HttpCookie sessionCookie = Request.Cookies["ASP.NET_SessionId"];
            if (sessionCookie == null || string.IsNullOrEmpty(sessionCookie.Value))
            {
                return false;
            }

            HttpCookie gwAuthCookie = Request.Cookies["DeskPlusEIP" + loginID];
            if (gwAuthCookie != null && !string.IsNullOrEmpty(gwAuthCookie.Value))
            {
                return true;
            }

            HttpCookie formsAuthCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            return formsAuthCookie != null && !string.IsNullOrEmpty(formsAuthCookie.Value);
        }

        private void SetLoginSessionCookie(string loginID)
        {
            string loginSessionCookieName = GetLoginSessionCookieName(loginID);
            if (Session != null)
            {
                Session[loginSessionCookieName] = "Y";
            }

            Response.Cookies[loginSessionCookieName].Value = "Y";
            Response.Cookies[loginSessionCookieName].Path = "/";

            string cookieExpiresHour = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "CookieExpiresHour", "");
            if (cookieExpiresHour != "") Response.Cookies[loginSessionCookieName].Expires = DateTime.Now.AddHours(Convert.ToInt32(cookieExpiresHour));

            string cookieDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "CookieDomain", "");
            if (cookieDomain != "") Response.Cookies[loginSessionCookieName].Domain = cookieDomain;
        }

        private string GetLoginSessionCookieName(string loginID)
        {
            return LoginSessionCookiePrefix + (loginID ?? string.Empty).ToLowerInvariant();
        }

        /// <summary>
        /// 다국어
        /// </summary>
        /// <value>
        /// The pre langs.
        /// </value>
        private string PreLangs
        {
            get { return ViewState["PreLangs"].ToString(); }
            set { ViewState["PreLangs"] = value; }
        }

        /// <summary>
        /// 작성자 : shimpark
        /// 암호화 문자열 반환하기
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        protected string GetEncryptParam(string data)
        {
            //빈값처리 Ajax에서는 빈값을 보낼경우 오류 발생 하므로 INULL 이라는 값으로 null 표시함
            if (String.IsNullOrEmpty(data)) return "!NULL";

            try
            {
                return Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.EncryptString(Server.UrlEncode(data));
            }
            catch
            {
                return "!NULL";
            }
        }

        /// <summary>
        /// 다국어 리소스 인스턴스 메소드
        /// </summary>
        /// <remarks></remarks>
        private void GetResourceManager()
        {
            resMng = new System.Resources.ResourceManager(ResourceName, AssemblyName);
        }

        /// <summary>
        /// 다국어 리소스 값 반환 메소드
        /// </summary>
        /// <param name="_resName">Name of the _res.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GetResource(string _resName)
        {
            return new System.Resources.ResourceManager(ResourceName, AssemblyName).GetString(_resName);
        }


        #endregion

        #region 파라미터 처리 (Parm/ViewState)
        /// <summary>
        /// 메서드명 : 
        /// 작성목적 : Request 파라미터 STRING 가져오기
        /// 작성자   : 심재운
        /// </summary>
        /// <param name="_parmName"></param>
        /// <param name="_errorReturn"></param>
        /// <returns></returns>
        protected string GetParm(string _parmName, string _errorReturn)
        {
            try
            {
                return ((string)(Request.Params[_parmName] ?? _errorReturn) == "") ? _errorReturn : (string)(Request.Params[_parmName] ?? _errorReturn);
            }
            catch
            {
                return (Request.Params[_parmName] != null) ? Convert.ToString(Request.Params[_parmName]) : Convert.ToString(_errorReturn);
            }
        }
        #endregion        
    }
}
