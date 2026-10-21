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
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Account.Web.MyInfo
{
    public partial class SLO : Base.BasePageAccount
    {
        private int CompanyID;
        private int UserID;
        private string LoginID;
        private string SubCall;
 
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, System.EventArgs e)
        {
            #region 다국어 리소스 로드
            this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";					//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.MyInfo.SLO).Assembly;						//어셈블리 선언
            this.GetResourceManager();
            #endregion

            if (!Page.IsPostBack)
            {
                DataLoad();
            }
        }


        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 초기화
        /// </summary>
        private void DataLoad()
        {
            LoadParam();

            if (Validation())
            {
                SignInProcess();

                string scripts = "";
                if (SubCall == "")
                {
                    scripts = @"parent.Menu.ReLoad();";
                }
                else
                {
                    scripts = @"parent.ReLoad();";
                }
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "selfClose", scripts, true);
            }
        }

        #endregion


        #region Method (함수)

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            CompanyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            UserID = GetDecryptParam("userID", "0").ToNumber<int>();
            LoginID = GetDecryptParam("loginID", "");
            SubCall = GetDecryptParam("subcall", "");
        }

        /// <summary>
        /// 작성자 : 심재운
        /// validation 및 referer 체크
        /// </summary>
        /// <returns></returns>
        private bool Validation()
        {
            bool response = true;
            string url = @"top.location='{0}';";

            try
            {
                if (Request.UrlReferrer != null)
                {
                    string referurl = Request.UrlReferrer.ToString();
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("referer " + referurl, AppSettingType.COMMON);

                    if (referurl.ToLower().IndexOf("/deskpluseip/") == -1)
                    {
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "goPage", this.resMng.GetString("WrongAccess"), ""); 
                        response = false;
                    }
                }
                else
                {
                    url = string.Format(url, "/DeskPlusEIP/Site/Auth/Logout.aspx");
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "NOAUTH", this.resMng.GetString("WrongAccess_NoReferer"), url);
                    response = false;
                }
            }
            catch
            {
                url = string.Format(url, "/DeskPlusEIP/Site/Auth/Logout.aspx");
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "NOAUTH", this.resMng.GetString("WrongAccess_Error"), url);
                response = false;
            }

            return response;
        }



        /// <summary>
        /// 작성자 : 심재운
        /// 인증 처리
        /// </summary>
        private void SignInProcess()
        {
            #region 2. 사용자별 세션 만들기

            Account.DAL.UserEnvironment bizEnv = new Account.DAL.UserEnvironment();
            List<UserEnvironmentInfo> alEnv = bizEnv.GetUserEnv(CompanyID, UserID);
            string prelang = "";
            foreach (UserEnvironmentInfo al in alEnv)
            {
                prelang = al.PreferredLanguage;
            }

            //사용자 정보 저장
            SetUserInfo(prelang);


                        
            #endregion
        }



        /// <summary>
        /// 작성자 : 심재운
        /// 인증정보 저장
        /// </summary>
        /// <param name="_preLang">The _pre lang.</param>
        private void SetUserInfo(string _preLang)
        {
            try
            {
                SetCookieMail(_preLang); //메일 쿠키 생성하기

                //그룹웨어 쿠키 생성하기
                Zeospace.DeskPlus.Account.BLL.User userBiz = new Account.BLL.User();
                string userInfoStr = userBiz.GetUserCookieStr(CompanyID, LoginID, "", _preLang);

                if (userInfoStr != "")
                {
                    string cookieName = "DeskPlusEIP" + LoginID;

                    #region 나의 쿠키, 세션, 인증쿠키 초기화

                    #region GW 쿠키 삭제
                    Response.Cookies[cookieName].Value = "";
                    Response.Cookies[cookieName].Path = "/";
                    Response.Cookies[cookieName].Expires = DateTime.Now.AddYears(-1);
                    #endregion

                    #region 메일 쿠키 삭제
                    Response.Cookies["ss_webmail_id"].Value = "";
                    Response.Cookies["ss_webmail_id"].Path = "/";
                    Response.Cookies["ss_webmail_id"].Expires = DateTime.Now.AddYears(-1);
                    #endregion

                    Session.Clear();                    
                    
                    #endregion                    

                    // 인증티켓 발급
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                        1,                              // 버전 번호
                        LoginID,     // 티켓과 관련된 사용자 이름
                        DateTime.Now,                   // 쿠키가 발행된 시간
                        DateTime.Now.AddHours(24),       // 쿠키의 만료 날짜
                        true,                          // 영구 쿠키이면 true
                        userInfoStr                      // 개인정보
                        );                    

                    // 인증 티켓 암호화
                    String cookieStr = FormsAuthentication.Encrypt(ticket);

                    //SESSION 설정
                    string useSession = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "UseSession", "");
                    if (useSession == "Y")
                    {
                        //session 에 값 할당 하기
                        Session["DeskPlusEIP"] = cookieStr;
                    }

                    //COOKIE 기술                    
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

                    //인증 쿠키 생성
                    FormsAuthentication.SetAuthCookie(LoginID, true);
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Default[SetUserInfo] 2 : " + ex);
            }
        }


        /// <summary>
        /// 작성자 : 심재운
        /// mail 쿠키 생성하기
        /// </summary>
        /// <param name="_preLang">The _pre lang.</param>
        private void SetCookieMail(string _preLang)
        {
            #region 제타메일용


            Zeospace.DeskPlus.Account.DAL.User user = new Account.DAL.User();
            List<Zeospace.DeskPlus.Account.BE.UserInfo> userList = user.GetUserByLoginID(CompanyID, LoginID, true);

            string mailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "");
            if (mailProduct.Length == 0) return;

            try //20120419 ByFreeman 제타 호출 오류시 exception 처리
            {

                if (mailProduct.ToUpper() == "ZETTA65")
                {
                    Zeospace.DeskPlus.Common.BLL.MailZettaFactory biz = new DeskPlus.Common.BLL.MailZettaFactory();

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
                        Response.Cookies["ss_webmail_id"]["LoginID"] = LoginID;
                        Response.Cookies["ss_webmail_id"]["UserID"] = userInfo.UserID.ToString();

                        string cookieExpiresHour = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "CookieExpiresHour", "");
                        if (cookieExpiresHour != "") Response.Cookies["ss_webmail_id"].Expires = DateTime.Now.AddHours(Convert.ToInt32(cookieExpiresHour));

                        string cookieDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "CookieDomain", "");
                        if (cookieDomain != "") Response.Cookies["ss_webmail_id"].Domain = cookieDomain;
                    }
                }
                else if (mailProduct.ToUpper().IndexOf("DEEP").Equals(0))
                {
                    Zeospace.DeskPlus.Common.BLL.MailDeepFactory biz = new DeskPlus.Common.BLL.MailDeepFactory();
                    string mailpw = string.Empty;

                    foreach (Zeospace.DeskPlus.Account.BE.UserInfo userInfo in userList)
                    {
                        // 인사연동 시, mailpw 없는 경우가 있음 : GW의 pw를 넘김.
                        mailpw = (userInfo.MailPassword.Equals("")) ? userInfo.Password : userInfo.MailPassword;

                        bool rtnval = biz.AddUser(userInfo.CompanyID, userInfo.MailDomain, userInfo.MailID, mailpw, userInfo.NameBase, "","");
                        if (!rtnval)
                        {
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("MyInfo/SLO - SetCookieMail(DEEP인증Error) : mailID(" + userInfo.MailID + "), domain(" + userInfo.MailDomain + ")");
                        }
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("MyInfo/SLO - SetCookieMail(DEEP인증결과-" + rtnval.ToString() + ") : mailID(" + userInfo.MailID + "), domain(" + userInfo.MailDomain + ")", AppSettingType.MAIL);
                    }
                }

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Default[SetCookieMail]  : " + ex);
            }


            #endregion
        }

        
        
        #endregion
    }
}