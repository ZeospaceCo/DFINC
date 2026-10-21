using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth
{
    /// <summary>
    /// 작성자   : 심재운
    /// 작성일자 : 2008-08-08
    /// 작성목적 : 환경 정보 설정
    /// </summary>
    public abstract class UserEnvironmentBaseControl : System.Web.UI.UserControl, IUserEnvironment
    {
        #region Global Variable (전역 변수)

        /// <summary>
        /// 다국어 리소스 변수
        /// </summary>
        protected System.Resources.ResourceManager resMng = null;

        internal UserInfo userInfo = null;

        private string preLang = "";
        #endregion Global Variable (전역 변수)

        #region Page Process (페이지 처리)

        /// <summary>
        /// 다국어 처리
        /// </summary>
        private void AssignPreLang()
        {
            if (PreLang.ToUpper().StartsWith("FR"))
            {
                PreLangReal = "fr";
                PreLang = "fr";
            }
            else if (PreLang.ToUpper().Equals("EN-GB"))
            {
                PreLangReal = "en-gb";
                PreLang = "en";
            }
            else if (PreLang.ToUpper().StartsWith("EN"))
            {
                PreLangReal = "en";
                PreLang = "en";
            }
            else if (PreLang.ToUpper().StartsWith("KO"))
            {
                PreLangReal = "ko";
                PreLang = "ko";
            }
            else if (PreLang.ToUpper().StartsWith("KO-KR"))
            {
                PreLangReal = "ko";
                PreLang = "ko";
            }
            else if (PreLang.ToUpper().StartsWith("JA"))
            {
                PreLangReal = "ja";
                PreLang = "ja";
            }
            else if (PreLang.ToUpper().StartsWith("ZH") && PreLang.ToUpper() == "ZH-CN")
            {
                PreLangReal = "zh-cn";
                PreLang = "zh-cn";
            }
            else if (PreLang.ToUpper().StartsWith("ZH") && PreLang.ToUpper() == "ZH-TW")
            {
                PreLangReal = "zh-tw";
                PreLang = "zh-tw";
            }
            else
            {
                PreLangReal = "ko";
                PreLang = "ko";
            }
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-08-08
        /// 작성목적 : M : 현재 Client의 Culture 정보 가져오기 메소드
        /// </summary>
        protected void HaveEnvAuthUser()
        {
            //인증 쿠키값이 존재하지 않을 경우
            if (Request.IsAuthenticated == false)
            {
                FormsAuthentication.SignOut();
                FormsAuthentication.RedirectToLoginPage();
                Response.End();
            }

            string loginID = HttpContext.Current.User.Identity.Name;

            if (String.IsNullOrEmpty(loginID) || loginID == "")
            {
                FormsAuthentication.SignOut();
                FormsAuthentication.RedirectToLoginPage();
                return;
            }

            userInfo = Authenticator.GetInstance().AuthenticatorInfo(loginID);

            if (userInfo == null)
            {
                FormsAuthentication.SignOut();
                FormsAuthentication.RedirectToLoginPage();
                return;
            }

            //클아이언트 환경 가져오기 - 다국어에서 사용
            if (userInfo.PreferredLanguage != null) PreLang = userInfo.PreferredLanguage;

            if (String.IsNullOrEmpty(PreLang))
            {
                if (Request.UserLanguages != null)
                {
                    PreLang = Request.UserLanguages[0];
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Request.UserLanguages[0]);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(Request.UserLanguages[0]);
                }
                else
                {
                    PreLang = Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "DefaultLanguage", "en");
                }
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(PreLang);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(PreLang);
            }

            if (String.IsNullOrEmpty(userInfo.BaseTimeZone))
            {
                BaseTimeZone = TimeZoneInfo.Local.Id;
            }
            else
            {
                BaseTimeZone = userInfo.BaseTimeZone;
            }

            AssignPreLang();
        }

        /// <summary>
        /// 미인증자 환경 정보 설정
        /// </summary>
        protected void HaveEnvNoAuthUser()
        {
            if (Request.UserLanguages != null)
            {
                PreLang = Request.UserLanguages[0];
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(PreLang);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(PreLang);
            }
            else
            {
                PreLang = Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "DefaultLanguage", "en");
            }

            AssignPreLang();
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-08-08
        /// 작성목적 : M : 에러 정보쓰기 메소드
        /// </summary>
        protected void ErrorUserEnvironment()
        {
            StringBuilder errorMessage = new StringBuilder();
            Exception currentError = Server.GetLastError();

            errorMessage.Append("▶▶-------------BasePage--------------");
            errorMessage.Append("<br>\n▶▶Time : " + DateTime.Now.ToString());
            errorMessage.Append("<br>\n▶▶UserID : " + HttpContext.Current.User.Identity.Name);
            errorMessage.Append("<br>\n▶▶Server Name : " + Server.MachineName);
            errorMessage.Append("<br>\n▶▶Client Address : " + Request.UserHostAddress);
            errorMessage.Append("<br>\n▶▶Client Name : " + Request.UserHostName);
            errorMessage.Append("<br>\n▶▶Url : " + Request.Url.ToString());
            errorMessage.Append("<br>\n\n▶▶Error Message : \n" + currentError.Message.ToString());
            errorMessage.Append("<br>\n\n▶▶Stack Trace : \n" + currentError.ToString());

            if (!(currentError is Exceptions.AppException))
            {
                Exceptions.AppException.WriteException("▶▶BasePage.cs[BasePage_Error] : Error " + errorMessage.ToString(), currentError);
            }

            //Response.Write(errorMessage.ToString());
            Server.ClearError();
            Response.Redirect("/DeskPlusEIP/Html/ErrorMessage/ErrorDefault.aspx");
        }

        #endregion Page Process (페이지 처리)

        #region Method (함수)

        #region 다국어

        /// <summary>
        /// 다국어 리소스 인스턴스 메소드
        /// </summary>
        protected void GetResourceManager()
        {
            resMng = new System.Resources.ResourceManager(ResourceName, AssemblyName);
        }

        /// <summary>
        /// 다국어 리소스 값 반환 메소드
        /// </summary>
        /// <param name="_resName"></param>
        /// <returns></returns>
        protected string GetResource(string _resName)
        {
            return new System.Resources.ResourceManager(ResourceName, AssemblyName).GetString(_resName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="_resName"></param>
        /// <param name="_ResourceName"></param>
        /// <param name="_AssemblyName"></param>
        /// <returns></returns>
        protected string GetResource(string _resName, string _ResourceName, Assembly _AssemblyName)
        {
            return new System.Resources.ResourceManager(_ResourceName, _AssemblyName).GetString(_resName);
        }

        #endregion 다국어

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

        /// <summary>
        /// 메서드명 :
        /// 작성목적 : Cookies 파라메터 처리
        /// 작성자   : 심재운
        /// </summary>
        /// <param name="_parmName"></param>
        /// <param name="_errorReturn"></param>
        /// <returns></returns>
        protected string GetCookieParam(string _parmName, string _errorReturn)
        {
            try
            {
                return (string)(Request.Cookies[_parmName].Value.Trim() ?? _errorReturn);
            }
            catch
            {
                return (!String.IsNullOrEmpty(Request.Cookies[_parmName].Value)) ? Convert.ToString(Request.Cookies[_parmName].Value) : Convert.ToString(_errorReturn);
            }
        }

        /// <summary>
        /// 메서드명 :
        /// 작성목적 : VIEWSTATE STRING 형 가져오기
        /// 작성자   : 심재운
        /// </summary>
        /// <param name="_viewStateName"></param>
        /// <param name="_errorReturn"></param>
        /// <returns></returns>
        protected string GetViewStateString(string _viewStateName, string _errorReturn)
        {
            try
            {
                return (string)(ViewState[_viewStateName] ?? _errorReturn);
            }
            catch
            {
                return (ViewState[_viewStateName] != null) ? Convert.ToString(ViewState[_viewStateName]) : Convert.ToString(_errorReturn);
            }
        }

        /// <summary>
        /// 메서드명 :
        /// 작성목적 : VIEWSTATE INT 형 가져오기
        /// 작성자   : 심재운
        /// </summary>
        /// <param name="_viewStateName">Name of the _view state.</param>
        /// <param name="_errorReturn">The _error return.</param>
        /// <returns></returns>
        protected int GetViewStateInt(string _viewStateName, string _errorReturn)
        {
            try
            {
                return (int)(ViewState[_viewStateName] ?? Convert.ToInt32(_errorReturn));
            }
            catch
            {
                return (ViewState[_viewStateName] != null) ? Convert.ToInt32(ViewState[_viewStateName]) : Convert.ToInt32(_errorReturn);
            }
        }

        #endregion 파라미터 처리 (Parm/ViewState)

        #endregion Method (함수)

        #region Property (프로퍼티)

        /// <summary>
        /// 다국어
        /// </summary>
        public string PreLang
        {
            get { return preLang; }
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

                preLang = lang;
            }
        }

        /// <summary>
        /// 다국어(진짜꺼~~~)
        /// </summary>
        public string PreLangReal { get; set; }

        /// <summary>
        /// 페이지 경로
        /// </summary>
        public string PagePath { get; set; }

        /// <summary>
        /// 리소스명 프로퍼티
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 어셈블리 프로퍼티
        /// </summary>
        public Assembly AssemblyName { get; set; }

        /// <summary>
        /// 표준시간대 프로퍼티
        /// </summary>
        /// <value>The name of the timezone.</value>
        /// <remarks></remarks>
        public string BaseTimeZone { get; set; }
        #endregion Property (프로퍼티)
    }
}