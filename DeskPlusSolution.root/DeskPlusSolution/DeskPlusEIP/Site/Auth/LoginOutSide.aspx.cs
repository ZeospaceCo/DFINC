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
    public partial class LoginOutSide : System.Web.UI.Page
    {
        protected System.Resources.ResourceManager resMng = null;
        public string ResourceName { get; set; }
        public Assembly AssemblyName { get; set; }
        private const string LoginSessionCookiePrefix = "DeskPlusEIPLoginSession";

        protected void Page_Load(object sender, System.EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Base.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Base.Web.Site.Auth.LoginOutSide).Assembly;	//어셈블리 선언
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
            int companyID = 1;
            string loginID = this.GetParm("USERID", "");
            string password = this.GetParm("USERPASSWORD", "");
            string targetURL = this.GetParm("targetURL", "");
            
            //메신저에서 넘어오는 도메인값(companyid) 
            int domainValue = this.GetParm("DOMAINVALUE", "").ToNumber<int>();


            int tempCompanyID = this.GetParm("cID", "1").ToNumber<int>();
            string tempLoginID = this.GetParm("uID", "");
            string tempPassword = this.GetParm("uPW", "");
            string tempTargetURL = this.GetParm("tURL", "");

            if (tempCompanyID != 1) companyID = tempCompanyID;
            if (tempLoginID != "") loginID = tempLoginID;
            if (tempPassword != "") password = tempPassword;
            if (tempTargetURL != "") targetURL = tempTargetURL;

            targetURL = targetURL.Replace("$", "&");

            string visitLog = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.VisitorLog(HttpContext.Current);
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("visitLog : " + visitLog, AppSettingType.BASE);
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("loginID : " + loginID, AppSettingType.BASE);
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("password : " + password, AppSettingType.BASE);
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("targetURL : " + targetURL, AppSettingType.BASE);

            bool encPwd = false;
            
            //메신저에서 SLO 처리 경우
            if (domainValue != 0 )
            {
                companyID = domainValue;
                //메신저에서는 Base64로 암호화를 해서 파라메터 넘김 복호화 처리
                loginID = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(loginID.Replace(" ", "+")));//shimpark:zeospace.com
                if (loginID.Length > 0) loginID = loginID.Split(':')[0];

                password = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(password.Replace(" ", "+")));
                encPwd = false; //패스워드 암호화 안되어 잇음
                
            }
            else //메일이나 기타 모듈에서 SLO 호출했을 때 사용함.
            {
                encPwd = true; //패스워드 암호화 되어 있음
            }

            if (loginID.Length > 0 && password.Length > 0)
            {
                SignInProcess(companyID, loginID, password, encPwd);

                if (targetURL != "")
                {
                    targetURL = GetApprovalStatusMenuAlarmUrl(companyID, loginID, targetURL);
                    Response.Redirect(targetURL);
                    /*
                   string url = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG,"BaseHttp","http://");
                    url = url + Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG,"FullDomain","localhost");
                    url = url+targetURL;
                    string parm = "";
                    string parmValue = "";
                    if (targetURL.IndexOf("dcparm") > 0)
                    {
                        parm = targetURL.Split('?')[1].ToString();
                        parmValue = parm.Split('=')[1].ToString();
                    }

                    UrlSendHelper.UsePost(url.Split('?')[0].ToString(), "dcparm=" + parmValue);
                   */
                }
            }
        }

        #endregion

        #region Method (함수)

        private string GetApprovalStatusMenuAlarmUrl(int _companyID, string _loginID, string _targetURL)
        {
            try
            {
                Uri targetUri;
                if (!Uri.TryCreate(_targetURL, UriKind.Absolute, out targetUri)) return _targetURL;

                bool isStatusDefault = targetUri.AbsolutePath.EndsWith("/Approval/Status/Default.aspx", StringComparison.OrdinalIgnoreCase);
                bool isFolderDefault = targetUri.AbsolutePath.EndsWith("/Approval/Folder/Default.aspx", StringComparison.OrdinalIgnoreCase);
                bool isStatusDefaultAll = targetUri.AbsolutePath.EndsWith("/Approval/Status/DefaultAll.aspx", StringComparison.OrdinalIgnoreCase);

                int approvalIndex = _targetURL.IndexOf("/Approval/", StringComparison.OrdinalIgnoreCase);
                if (approvalIndex < 0) return _targetURL;

                if (!IsApprovalStatusNewMenu(_companyID, _loginID))
                {
                    if (!isStatusDefaultAll) return _targetURL;

                    return GetApprovalStatusOldMenuAlarmUrl(_targetURL, targetUri, approvalIndex);
                }

                if (!isStatusDefault && !isFolderDefault) return _targetURL;

                string encryptedParam = HttpUtility.ParseQueryString(targetUri.Query)["p"];
                if (string.IsNullOrEmpty(encryptedParam)) return _targetURL;

                string parameterString = HttpUtility.UrlDecode(Encoding.Default.GetString(Convert.FromBase64String(encryptedParam.Replace(" ", "+"))));
                System.Collections.Specialized.NameValueCollection parameters = HttpUtility.ParseQueryString(parameterString);
                string folderStatusID = isStatusDefault
                    ? GetApprovalStatusMenuFolderStatusID(parameters["folderStatusID"])
                    : GetApprovalStatusMenuFolderStatusID(parameters["cabinetID"]);

                if (folderStatusID.Length == 0) return _targetURL;

                return _targetURL.Substring(0, approvalIndex) + "/Approval/Status/DefaultAll.aspx?folderStatusID=" + HttpUtility.UrlEncode(folderStatusID);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("LoginOutSide[GetApprovalStatusMenuAlarmUrl] : " + ex);
                return _targetURL;
            }
        }

        private string GetApprovalStatusOldMenuAlarmUrl(string _targetURL, Uri _targetUri, int _approvalIndex)
        {
            string encryptedParam = HttpUtility.ParseQueryString(_targetUri.Query)["p"];
            if (string.IsNullOrEmpty(encryptedParam)) return _targetURL;

            string parameterString = HttpUtility.UrlDecode(Encoding.Default.GetString(Convert.FromBase64String(encryptedParam.Replace(" ", "+"))));
            System.Collections.Specialized.NameValueCollection parameters = HttpUtility.ParseQueryString(parameterString);
            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            foreach (string key in parameters.AllKeys)
            {
                if (key != null) paraDic[key] = parameters[key];
            }

            string folderStatusID = (parameters["folderStatusID"] ?? "").ToUpper();
            string targetPage = "/Approval/Status/Default.aspx";

            switch (folderStatusID)
            {
                case "RECEIVE":
                case "REJECT":
                case "APPROVE":
                case "TRANSFER":
                    break;
                case "APPROVEREJECTALL":
                    paraDic["folderStatusID"] = "Approve";
                    break;
                case "CARBONCOPYALL":
                    targetPage = "/Approval/Folder/Default.aspx";
                    paraDic.Remove("folderStatusID");
                    paraDic["cabinetID"] = "Carboncopy_Doc";
                    break;
                case "CARBONCOPYCIRRECEIPTALL":
                    targetPage = "/Approval/Folder/Default.aspx";
                    paraDic.Remove("folderStatusID");
                    paraDic["cabinetID"] = "Carboncopy_CirReceipt";
                    break;
                case "CARBONCOPYFORWARDRECEIVEALL":
                    targetPage = "/Approval/Folder/Default.aspx";
                    paraDic.Remove("folderStatusID");
                    paraDic["cabinetID"] = "Carboncopy_Forward";
                    break;
                default:
                    return _targetURL;
            }

            string encryptParam = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.EncryptString(Server.UrlEncode(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic)));
            return _targetURL.Substring(0, _approvalIndex) + targetPage + "?p=" + encryptParam;
        }

        private bool IsApprovalStatusNewMenu(int _companyID, string _loginID)
        {
            Account.DAL.User user = new Account.DAL.User();
            List<Account.BE.UserInfo> users = user.GetUserByLoginID(_companyID, _loginID, true);
            if (users == null || users.Count == 0) return false;

            Account.DAL.UserEnvironment userEnvironment = new Account.DAL.UserEnvironment();
            List<UserEnvironmentInfo> environments = userEnvironment.GetUserEnv(_companyID, users[0].UserID);

            return environments != null && environments.Count > 0 &&
                "NEW".Equals(environments[0].ApprovalStatusMenuType, StringComparison.OrdinalIgnoreCase);
        }

        private string GetApprovalStatusMenuFolderStatusID(string _folderStatusID)
        {
            switch ((_folderStatusID ?? "").ToUpper())
            {
                case "RECEIVE":
                    return "Receive";
                case "REJECT":
                    return "Reject";
                case "APPROVE":
                    return "Approve";
                case "TRANSFER":
                    return "Transfer";
                case "CARBONCOPY_DOC":
                    return "CarboncopyAll";
                case "CARBONCOPY_CIRRECEIPT":
                    return "CarboncopyCirReceiptAll";
                case "CARBONCOPY_FORWARD":
                    return "CarboncopyForwardReceiveAll";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 인증 처리 (enc가false면 타이핑pwd다)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_loginID">The _login ID.</param>
        /// <param name="_password">The _password.</param>
        /// <param name="_encPwd">if set to <c>true</c> [_enc PWD].</param>
        private void SignInProcess(int _companyID, string _loginID, string _password, bool _encPwd)
        {
            int userID = 0;

            #region 메신저체크 - 사용자정보(ID/PWD)체크
            //MD5 패스워드 Hashing 여부 ByFreeman 20080722
            string useHash = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "UseHashCheckByLoginOutSide", "N");
            if (useHash == "Y")
            {
                Account.DAL.User user = new Account.DAL.User();
                List<Account.BE.UserInfo> al = user.GetUserByLoginID(_companyID, _loginID, true);

                if (al.Count > 0)
                {
                    string password = String.Empty;
                    int tempUserID = 0;
                    foreach (Account.BE.UserInfo userInfo in al)
                    {
                        tempUserID = userInfo.UserID;
                        password = userInfo.Password;
                    }
                    
                    // Hash the input.
                    string hashOfPassword = ApplicationFrameworks.Helper.Security.HashManager.GetHash(password);

                    // Create a StringComparer an comare the hashes.
                    StringComparer comparer = StringComparer.OrdinalIgnoreCase;

                    if (0 == comparer.Compare(hashOfPassword, _password))
                    {
                        //사용자아이디 가져오기
                        userID = tempUserID;
                    }
                }
                else
                {
                    ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "NOAUTH", resMng.GetString("CheckIDandPwd"), "");
                }

            }
            else
            {
                //사용자아이디 가져오기
                Account.DAL.User biz = new Account.DAL.User();
                userID = biz.SignIn(_companyID, _loginID, _password, _encPwd);

            }
            #endregion

            if (userID == -1)
            {
                // 접속명 혹은 비밀번호를 확인해 주십시오.
                ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "NOAUTH1", resMng.GetString("CheckIDandPwd"), "");
                return;
            }
            else if (userID == -2)
            {
                // 부서 비활성화로 인증할 수 없습니다. 관리자에게 문의하시기 바랍니다.
                ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "NOAUTH2", resMng.GetString("CheckAuthDept"), "");
                return;
            }
            else
            {
                //사용자 정보 저장
                SetUserInfo(_companyID, _loginID, userID, _password, _encPwd);
            }
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
        private void SetUserInfo(int _companyID, string _loginID, int _userID, string _password, bool _encPwd)
        {
            try
            {
                bool isAlreadyAuthenticatedUser = HasLoginSessionCookie(_loginID);

                //패스워드 만기
                ChangePwd(_companyID, _loginID, _password);

                #region SPOOL 하위 폴더 삭제
                if (!isAlreadyAuthenticatedUser)
                {
                    string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                    string spoolFolder = baseFileDirectory + _companyID + "/Users/" + _userID.ToString() + "/spool/";
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
                List<UserEnvironmentInfo> alEnv = bizEnv.GetUserEnv(_companyID, _userID);
                foreach (UserEnvironmentInfo al in alEnv)
                {
                    PreLangs = al.PreferredLanguage;
                }

                //메일인증 : Zetta(메일쿠키생성) / Deep(인증URL호출)
                SetCookieMail(_companyID, _loginID);            //메일 쿠키 생성하기

                #region 사용자 정보 인증쿠키 설정하기

                SetAccessLog(_companyID, _userID);//로그인 자 접속 정보 로그 쌓기

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
                #endregion

                //인증 쿠키 생성
                FormsAuthentication.SetAuthCookie(_loginID, true);

                #region IP대역 및 외부접속관리 체크
                //By Freeman  20150604 
                /*  로그인 IP 접속 제한 기능 
                 *      1. 코드관리 : 접근허용IP리스트(AllowHostIP) 추가하여 외부연동코드 192.168.1.1-192.168.1.254, 
                 *      2. 권한관리 : 사외접근허용사용자(allowUserIPAccess) 추가하면 IP Access 인증 체크 하지 않는 사용자 기능 사용
                 */

                string[] remoteAddrStr = Request.UserHostAddress.Split('.');
                string remoteAddr = UtilHelper.GetIPAddress(remoteAddrStr);

                string allowUserIPAccess = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "AllowUserIPAccess", "");
                if (allowUserIPAccess == "Y")
                {
                    bool allowLogin = false;

                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.IPList allowIPList = new IPList();
                    DataView dvAlarm = AppSettingHelper.GetSysCode(_companyID, "AllowHostIP");
                    string ip = "";
                    for (int i = 0; i < dvAlarm.Count; i++)
                    {
                        if (dvAlarm[i]["ActiveCode"].ToString() == "1")
                        {
                            ip = dvAlarm[i]["InterfaceCode"].ToString();
                            string[] ipArr = ip.Split('-');

                            if (ipArr.Length == 1)
                            {
                                allowIPList.Add(ipArr[0]);
                            }
                            else
                            {
                                allowIPList.AddRange(ipArr[0], ipArr[1]);
                            }
                        }
                    }

                    if (allowIPList.CheckNumber(Request.UserHostAddress))
                    {
                        allowLogin = true;      // 접속 IP가 사내 IP 대역에 있음. 
                    }
                    else
                    {
                        // IP 상관없이 접속 가능한 ID
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth.UserInfo userInfo = Authenticator.GetInstance().AuthenticatorInfo(_loginID);
                        allowLogin = (userInfo.IsInRole("allowUserIPAccess")) ? true : false;
                    }

                    if (!allowLogin)
                    {
                        // 보안정책에 의해 접근이 제한되었습니다
                        //ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "NOAUTH", resMng.GetString("AccessNoAuthIP"), "");
                        ApplicationFrameworks.Helper.WebComponent.Utility.WriteScript(Page, "NOAUTH", "alert('" + resMng.GetString("AccessNoAuthIP") + "');window.open('about:blank', '_self').close();");

                        FormsAuthentication.SignOut();      // 인증쿠키삭제로직 (쿠키삭제부분 추가확인필요-Logout.aspx.cs)
                        return;
                    }
                }
                #endregion

                #region 메뉴 접근 확인 여부
                //로긴사용자 메뉴가져오기
                EIP.BLL.Menu bizMenu = new EIP.BLL.Menu();
                List<string> alMenuUser = bizMenu.GetMenuListUser(_companyID, _userID, remoteAddr.ToDouble<double>());
                if (alMenuUser == null)
                {
                    // 접근 가능한 포탈 메뉴가 없습니다.
                    //ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "NOMENU", resMng.GetString("AccessFail"), "");
                    ApplicationFrameworks.Helper.WebComponent.Utility.WriteScript(Page, "NOMENU", "alert('" + resMng.GetString("AccessFail") + "');window.open('about:blank', '_self').close();");

                    FormsAuthentication.SignOut();      // 인증쿠키삭제로직 (쿠키삭제부분 추가확인필요-Logout.aspx.cs)
                    return;
                }
                #endregion

                SetLoginSessionCookie(_loginID);

                if (Request["ReturnUrl"] != null && Request["ReturnUrl"].EndsWith("/Auth/Default.aspx"))
                {
                    Response.Redirect("/DeskPlusEIP/", false);
                }
                else
                {
                    FormsAuthentication.RedirectFromLoginPage(_loginID, false);
                }
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
                if(ds != null)
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
                            Response.Redirect(url, true);
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
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("LoginOutSide - SetCookieMail(DEEP인증Error) : mailID(" + userInfo.MailID + "), domain(" + userInfo.MailDomain + ")");
                        }
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("LoginOutSide - SetCookieMail(DEEP인증결과-" + rtnval.ToString() + ") : mailID(" + userInfo.MailID + "), domain(" + userInfo.MailDomain + ")", AppSettingType.MAIL);
                    }
                }

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Default[SetCookieMail]  : " + ex);
            }

            #endregion
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
