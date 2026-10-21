using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Reflection;

namespace Zeospace.DeskPlus.Base.Web.Site.Auth
{
    public partial class Default10 : System.Web.UI.Page
    {
        #region Declaration (컨트롤 선언)
        protected System.Resources.ResourceManager resMng = null;
        public string ResourceName { get; set; }
        public Assembly AssemblyName { get; set; }

        public string imagePath = "";
        public string cssPath = "";
        public string LoginImage = "";
        public string MaxCnt = "";
        public SortedDictionary<string, string> PopupContent;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, System.EventArgs e)
        {
            PopupContent = new SortedDictionary<string, string>();

            if (Request.UserLanguages != null)
            {
                PreLangs = Request.UserLanguages[0];
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(PreLangs);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(PreLangs);
            }

            string page = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "DefaultPage", "");
            if (!page.Equals("") && !page.ToLower().Equals("default.aspx"))
            {
                string gopage = "/DeskPlusEIP/Site/Auth/" + page;
                Response.Redirect(gopage, false);
            }
            else
            {
                #region 다국어 리소스 로드/Title 정보
                this.ResourceName = "Zeospace.DeskPlus.Base.Web.Resource.GlobalString.string";			//리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.Base.Web.Site.Auth.Default10).Assembly;	//어셈블리 선언
                #endregion

                GetResourceManager();
                MaxCnt = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.ACCOUNT, "PasswrodFaultCount", "0");

                if (Request.UserLanguages != null)
                {
                    string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");
                    string baseCssDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseCssDirectory", "/DeskPlusEIP/Design/");

                    PreLangs = Request.UserLanguages[0];
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(PreLangs);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(PreLangs);

                    imagePath = baseImageDirectory + PreLangs + "/Base/images";
                    cssPath = baseCssDirectory + PreLangs + "/Base/css";
                }
                else
                {
                    PreLangs = "en";
                    imagePath = "/DeskPlusEIP/Design/" + PreLangs + "/Base/images";
                    cssPath = "/DeskPlusEIP/Design/" + PreLangs + "/Base/css";
                }

                if (!Page.IsPostBack)
                {
                    //다국어 설정
                    GetMultiCompany();

                    //로그인 저장
                    if (Request.Cookies["DPAuth"] != null)
                    {
                        string userStr = Request.Cookies["DPAuth"].Value;

                        try
                        {
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security.TripleDESManager enc =
                                new ApplicationFrameworks.Helper.Security.TripleDESManager();

                            userStr = enc.Decrypt(userStr);

                            string[] userArr = userStr.Split(';');

                            if (userArr.Length > 1)
                            {
                                if (userArr[0].Trim().Length != 0) this.tbLoginID.Text = userArr[0];

                                this.chkRemember.Checked = true;
                            }
                        }
                        catch { }
                    }

                    //회사 저장
                    if (Request.Cookies["DPComanyAuth"] != null)
                    {
                        try
                        {
                            ddlCompany.SelectedValue = Request.Cookies["DPComanyAuth"].Value;
                            this.chkCompany.Checked = true;
                        }
                        catch
                        {
                            this.chkCompany.Checked = false;
                        }
                    }

                    string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "/DeskplusEIP/FileData/");
                    string ciLoginImage = baseFileDirectory + ddlCompany.SelectedValue + "/lgn_logo.png";

                    //회사별 이미지가 있다면?
                    if (System.IO.File.Exists(Server.MapPath(ciLoginImage)))
                    {
                        LoginImage = ciLoginImage;
                    }
                    else
                    {
                        //회사별 이미지가 없다면, 기본 이미지 
                        LoginImage = this.BaseImgPath + "/Login/lgn_logo.png";
                    }
                }

                if (chkRemember.Checked)
                {
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "PageLoadInitFunc", "<script language='javascript'>$(\"" + tbPassword.ClientID + "\").focus();</script>");
                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "PageLoadInitFunc", "<script language='javascript'>$(\"" + tbLoginID.ClientID + "\").focus();</script>");
                }

                // 팝업
                GetPopUp();
            }
        }
        #endregion

        #region Method (함수)

        /// <summary>
        /// 로그인 전 팝업처리
        /// </summary>
        /// <remarks></remarks>
        private void GetPopUp()
        {
            int h, w;
            string ct = string.Empty;
            string mID = string.Empty;

            DataSet ds = GetPopUpManager();

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    h = dr["Height"].ToString().ToNumber<int>();
                    w = dr["Width"].ToString().ToNumber<int>();

                    mID = dr["ManagerID"].ToString();

                    ct = "window.open('/DeskPlusEIP/Preview.aspx?managerID=" + mID + "', 'pop_not_log" + mID + "', 'menubar=no, scrollbars=no, status=no, toolbar=no, resizable=no, width=" + w + ", height=" + h + ", top=0 , left=0 ');";
                    PopupContent.Add(mID, ct);
                }
            }
        }

        private DataSet GetPopUpManager()
        {
            string sqlQuery = string.Empty;
            string nowDate = string.Empty;

            nowDate = DateTime.Now.ToString("yyyy-MM-dd");
            sqlQuery = "SELECT * FROM DP_POP_Manager "
                     + " WHERE StartDate <= '" + nowDate + "'"
                     + " AND EndDate >= '" + nowDate + "' AND LoginYN=0";

            return SQLHelper.ExecuteDataSet(ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString, CommandType.Text, sqlQuery, "GetPopUpManager", null);

        }

        /// <summary>
        /// login
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void lbtnSubmit_Click(object sender, System.EventArgs e)
        {
            int companyID = ddlCompany.SelectedValue.ToNumber<int>();
            string loginID = this.tbLoginID.Text;
            string password = this.tbPassword.Text;
            string targetURL = this.GetParm("targetURL", "");

            targetURL = targetURL.Replace("$", "&");

            string tempLoginID = this.GetParm("uID", "");
            string tempPassword = this.GetParm("uPW", "");

            if (tempLoginID != "") loginID = tempLoginID;
            if (tempPassword != "") password = tempPassword;

            string visitLog = "visitLog : " + Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.VisitorLog(HttpContext.Current);
            visitLog += "\nloginID : " + loginID;
            visitLog += "\npassword : " + password;
            visitLog += "\ntargetURL : " + targetURL;

            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert(visitLog, AppSettingType.BASE);

            if (loginID.Length > 0 && password.Length > 0)
            {
                SignInProcess(companyID, loginID, password);
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 로그인 처리
        /// 작성자     : shimpark
        /// 최초작성일 : 2008-07-24
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_loginID">The _login ID.</param>
        /// <param name="_password">The _password.</param>
        /// <remarks></remarks>
        private void SignInProcess(int _companyID, string _loginID, string _password)
        {

            int userID = 0;

            Account.DAL.User biz = new Account.DAL.User();

            //사용자아이디 가져오기
            userID = biz.SignIn(_companyID, _loginID, _password, false);
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
                /* 1. 쿠키저장로직 체크
                 * 2. 사용자 SPOOL 하위 폴더 삭제
                 * 3. 메일인증
                 * 4. 사용자정보 인증쿠키 설정
                 * 5. 사용자정보 인증쿠키 생성
                 * 6. IP대역 및 외부접속관리 체크
                 * 7. 메뉴접근확인 여부
                 */

                #region 쿠키저장로직 = 아이디저장 + 회사저장

                //아이디저장
                if (chkRemember.Checked)
                {
                    String userArrStr = _loginID + ";" + _password + ";";
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security.TripleDESManager enc =
                        new ApplicationFrameworks.Helper.Security.TripleDESManager();

                    //아이디저장
                    Response.Cookies["DPAuth"].Value = enc.Encrypt(userArrStr);
                    Response.Cookies["DPAuth"].Path = "/";
                    Response.Cookies["DPAuth"].Expires = DateTime.Now.AddYears(2);

                }
                else
                {
                    if (Request.Cookies["DPAuth"] != null)
                    {
                        Response.Cookies["DPAuth"].Expires = DateTime.Now.Subtract(new TimeSpan(1, 0, 0));
                    }
                }

                //회사저장
                if (chkCompany.Checked)	//Cookie 정보 저장
                {
                    //회사저장
                    Response.Cookies["DPComanyAuth"].Value = ddlCompany.SelectedValue;
                    Response.Cookies["DPComanyAuth"].Path = "/";
                    Response.Cookies["DPComanyAuth"].Expires = DateTime.Now.AddYears(2);

                }
                else
                {
                    if (Request.Cookies["DPComanyAuth"] != null)
                    {
                        Response.Cookies["DPComanyAuth"].Expires = DateTime.Now.Subtract(new TimeSpan(1, 0, 0));
                    }
                }

                #endregion

                #region 사용자 SPOOL 하위 폴더 삭제
                try
                {
                    string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                    string spoolFolder = baseFileDirectory + _companyID + "/Users/" + userID.ToString() + "/Spool/";
                    DirectoryInfo di = new DirectoryInfo(Server.MapPath(spoolFolder));

                    if (di.Exists)
                    {
                        string[] folder;
                        folder = Directory.GetDirectories(Server.MapPath(spoolFolder));

                        foreach (string folderName in folder)
                        {
                            Directory.Delete(folderName, true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SPOOL 하위 폴더 삭제 : " + ex);
                }
                #endregion

                Account.DAL.UserEnvironment bizEnv = new Account.DAL.UserEnvironment();
                List<UserEnvironmentInfo> alEnv = bizEnv.GetUserEnv(_companyID, userID);

                foreach (UserEnvironmentInfo al in alEnv)
                {
                    PreLangs = al.PreferredLanguage;
                }

                //메일인증 : Zetta(메일쿠키생성) / Deep(인증URL호출)
                SetCookieMail(_companyID, _loginID, PreLangs);      //메일 쿠키 생성하기

                //사용자 정보 인증쿠키 설정하기
                SetCookieUserInfo(_companyID, _loginID, userID, _password, PreLangs);

                //인증 쿠키 생성
                FormsAuthentication.SetAuthCookie(_loginID, false);

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
                        ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "NOAUTH", resMng.GetString("AccessNoAuthIP"), "");
                        //ApplicationFrameworks.Helper.WebComponent.Utility.WriteScript(Page, "NOAUTH", "alert('" + resMng.GetString("AccessNoAuthIP") + "');window.open('about:blank', '_self').close();");

                        FormsAuthentication.SignOut();      // 인증쿠키삭제로직 (쿠키삭제부분 추가확인필요-Logout.aspx.cs)
                        return;
                    }
                }
                #endregion

                #region 휴일가져오기
                string year;
                year = DateTime.Now.ToString("yyyy");
                Pim.BLL.HolidayR holiday = new Pim.BLL.HolidayR();
                holiday.SetCacheHolidayList(_companyID, year);
                #endregion

                #region 메뉴 접근 확인 여부
                //로긴사용자 메뉴가져오기
                EIP.BLL.Menu bizMenu = new EIP.BLL.Menu();
                List<string> alMenuUser = bizMenu.GetMenuListUser(_companyID, userID, remoteAddr.ToDouble<double>());
                if (alMenuUser == null)
                {
                    // 접근 가능한 포탈 메뉴가 없습니다.
                    ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "NOMENU", resMng.GetString("AccessFail"), "");
                    //ApplicationFrameworks.Helper.WebComponent.Utility.WriteScript(Page, "NOMENU", "alert('" + resMng.GetString("AccessFail") + "');window.open('about:blank', '_self').close();");

                    FormsAuthentication.SignOut();      // 인증쿠키삭제로직 (쿠키삭제부분 추가확인필요-Logout.aspx.cs)
                    return;
                }
                #endregion

                if (Request["ReturnUrl"] != null && Request["ReturnUrl"].EndsWith("/Auth/Default.aspx"))
                {
                    Response.Redirect("/DeskPlusEIP/", false);
                }
                else
                {
                    FormsAuthentication.RedirectFromLoginPage(_loginID, false);
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
        /// <param name="_preLang">The _pre lang.</param>
        private void SetCookieMail(int _companyID, string _loginID, string _preLang)
        {
            Zeospace.DeskPlus.Account.DAL.User user = new Account.DAL.User();
            List<Zeospace.DeskPlus.Account.BE.UserInfo> userList = user.GetUserByLoginID(_companyID, _loginID, true);

            string mailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "");
            if (mailProduct.Length == 0) return;

            try //20120419 ByFreeman 제타 호출 오류시 exception 처리
            {

                if (mailProduct.ToUpper() == "ZETTA65")
                {
                    #region Zetta6.5
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
                    #endregion
                }
                else if (mailProduct.ToUpper() == "ZETTA52")
                {
                    #region Zetta5.2
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
                    #endregion
                }
                else if (mailProduct.ToUpper().IndexOf("DEEP").Equals(0))
                {
                    #region DEEP
                    Zeospace.DeskPlus.Common.BLL.MailDeepFactory biz = new Common.BLL.MailDeepFactory();
                    string mailpw = string.Empty;

                    foreach (Zeospace.DeskPlus.Account.BE.UserInfo userInfo in userList)
                    {
                        // 인사연동 시, mailpw 없는 경우가 있음 : GW의 pw를 넘김.
                        mailpw = (userInfo.MailPassword.Equals("")) ? userInfo.Password : userInfo.MailPassword;

                        bool rtnval = biz.AddUser(userInfo.CompanyID, userInfo.MailDomain, userInfo.MailID, mailpw, userInfo.NameBase, "", "");
                        if (!rtnval)
                        {
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Default - SetCookieMail(DEEP인증Error) : mailID(" + userInfo.MailID + "), domain(" + userInfo.MailDomain + ")");
                        }
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("Default - SetCookieMail(DEEP인증결과-" + rtnval.ToString() + ") : mailID(" + userInfo.MailID + "), domain(" + userInfo.MailDomain + ")", AppSettingType.MAIL);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Default[SetCookieMail]  : " + ex);
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
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_password">The _password.</param>
        /// <param name="_preLang">The _pre lang.</param>
        private void SetCookieUserInfo(int _companyID, string _loginID, int _userID, string _password, string _preLang)
        {
            try
            {
                SetAccessLog(_companyID, _userID); //로그인 자 접속 정보 로그 쌓기

                //그룹웨어 쿠키 생성하기
                Zeospace.DeskPlus.Account.BLL.User userBiz = new Account.BLL.User();
                string userInfoStr = userBiz.GetUserCookieStr(_companyID, _loginID, _password, _preLang);

                if (userInfoStr != "")
                {
                    // 인증티켓 발급
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                        1,                              // 버전 번호
                        _loginID,     // 티켓과 관련된 사용자 이름
                        DateTime.Now,                   // 쿠키가 발행된 시간
                        DateTime.Now.AddHours(24),       // 쿠키의 만료 날짜
                        false,                          // 영구 쿠키이면 true
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
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Default[SetUserInfo] 2 : " + ex);
            }
        }

        /// <summary>
        /// 메서드명 :
        /// 작성목적 : Request 파라미터 STRING 가져오기
        /// 작성자   : 심재운
        /// </summary>
        /// <param name="_parmName">Name of the _parm.</param>
        /// <param name="_errorReturn">The _error return.</param>
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

        /// <summary>
        /// 작성자 : 심재운
        /// 회사 정보 가져오기
        /// </summary>
        private void GetMultiCompany()
        {

            Zeospace.DeskPlus.Common.DAL.Company biz = new Common.DAL.Company();
            List<Zeospace.DeskPlus.Common.BE.CompanyInfo> al = biz.GetCompany(true);
            ddlCompany.Items.Clear();

            string companyName = "";
            foreach (Zeospace.DeskPlus.Common.BE.CompanyInfo info in al)
            {

                companyName = UtilHelper.GetLangName(
                    info.NameBase,
                    info.NameKo, info.NameEn, info.NameJa,
                    info.NameFr, info.NameZhcn,
                    info.NameZhtw, PreLangs).ToReverseXSS();


                ddlCompany.Items.Add(new ListItem(companyName.ToReverseXSS(), info.CompanyID.ToString()));

            }
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

        #region Property (프로퍼티)

        /// <summary>
        /// 다국어
        /// </summary>
        /// <value>
        /// The pre langs.
        /// </value>
        private string PreLangs
        {
            get { return ViewState["PreLangs"].ToString(); }
            set
            {
                string lang = "";

                if (value == "")
                {
                    lang = Request.UserLanguages[0].ToLower();
                }
                else
                {
                    lang = value.ToLower();
                }

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

                ViewState["PreLangs"] = lang;
                //ViewState["PreLangs"] = value; 
            }
        }

        /// <summary>
        /// 패스워드 차단 카운트 경고글
        /// </summary>
        public string PasswordFaultTotalCount
        {
            get
            {
                string maxPwdCount = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.ACCOUNT, "PasswrodFaultCount", "0");
                return maxPwdCount + GetResource("PasswordFaultTotalCount");
            }
        }

        /// <summary>
        /// 공통 CSS
        /// </summary>
        public string BaseCssPath
        {
            get { return AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseCssPath", "/DeskPlusEIP/Design/C/Css"); }
        }

        /// <summary>
        /// 공통 Img
        /// </summary>
        public string BaseImgPath
        {
            get { return AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImgPath", "/DeskPlusEIP/Design/C/Img"); }
        }
        #endregion
    }
}
