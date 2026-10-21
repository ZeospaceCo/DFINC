using System;
using System.Text;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.Services;
using System.Collections.Specialized;
using Zeospace.DeskPlusMobile.Web.Helper.WebComponent;
using System.Threading;
using System.Globalization;
using System.Reflection;

namespace Zeospace.DeskPlusMobile.Web.Helper
{
    public class MobileBaseControl : System.Web.UI.UserControl
    {
        /// <summary>
        /// InsParms
        /// </summary>
        /// <value>The ins parms.</value>
        public string InsParms { get; set; }
        private int userID = -1;
        private int companyID = -1;
        private Helper.UserInfoHelper userInfo = null;
        private string decryString = string.Empty;//복고화 값
        /// <summary>
        /// 다국어 리소스 변수
        /// </summary>
        protected System.Resources.ResourceManager resMng = null;
        private string preLang = "";
        #region Construct (생성자)
        /// <summary>
        /// 생성자 생성
        /// </summary>
        public MobileBaseControl()
        {
            this.Load += new System.EventHandler(this.MobileBaseControl_Load);
            this.Error += new System.EventHandler(this.MobileBaseControl_Error);
        }
        #endregion

        #region Page Process (페이지 처리)

        /// <summary>
        /// 다국어 처리
        /// </summary>
        /// <remarks></remarks>
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
        /// M : 현재 Client의 Culture 정보 가져오기 메소드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MobileBaseControl_Load(object sender, System.EventArgs e)
        {
            string loginID = HttpContext.Current.User.Identity.Name;
            if (String.IsNullOrEmpty(loginID)) FormsAuthentication.RedirectToLoginPage();
            bool isAuth = true;

            /*
            InsParms = GetParmEnc("p", "");
            if (InsParms != "" && !String.IsNullOrEmpty(InsParms) && InsParms != "!NULL")
            {
                Helper.Security.TripleDESManager tds = new Helper.Security.TripleDESManager();
                InsParms = tds.Decrypt(InsParms.Trim(), System.Text.Encoding.UTF8);
                InsParms = InsParms.TrimEnd('\0');
            }          
             */

            //1. iisrset 이나 컴포넌트를 올릴 경우, 세션은 삭제되지만 인증쿠키는 삭제되지 않는다.
            //2. 세션삭제는 되었지만 인증쿠키가 삭제되지 않았을 경우, 무한로직을 돌게 됩니다.
            //3. 따라서 세션이 없을 경우, 인증쿠키를 삭제해서 로긴페이지로 자동 이동하게 됩니다.
            string useSession = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "CONFIG_USESESSION");
            //세션기술
            if (useSession.ToUpper() == "TRUE")
            {
                if (Session["DeskPlusMobile_" + loginID] == null)
                {
                    if (Request.Cookies["DeskPlusMobile_" + loginID] == null || Request.Cookies["DeskPlusMobile_" + loginID].Value == "")
                    {
                        isAuth = false;
                    }
                }
            }
            else
            {
                //쿠키기술
                if (Context.Request.Cookies["DeskPlusMobile_" + loginID] != null)
                {
                    if (Context.Request.Cookies["DeskPlusMobile_" + loginID].Value == "" || Context.Request.Cookies["DeskPlusMobile_" + loginID].Value == null)
                    {
                        isAuth = false;
                    }

                }
                else if (Context.Request.Cookies["DeskPlusMobile_" + loginID] == null)
                {
                    isAuth = false;
                }
            }

            if (!isAuth)
            {
                //FormsAuthentication.SignOut();
                //FormsAuthentication.RedirectToLoginPage();
                Response.Redirect("/DeskPlusMobile/Portal/LogOut.aspx");
            }
            else
            {
                userInfo = new UserInfoHelper(loginID);
                userID = userInfo.UserID; //사용자 ID
                companyID = userInfo.CompanyID;

                //클아이언트 환경 가져오기 - 다국어에서 사용
                if (userInfo.PreferredLanguage != null) PreLang = userInfo.PreferredLanguage;

                if (String.IsNullOrEmpty(PreLang))
                {
                    if (Request.UserLanguages != null)
                    {
                        PreLang = Request.UserLanguages[0];
                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(PreLang);
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo(PreLang);

                    }
                    else
                    {
                        PreLang = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "DefaultLanguage");
                    }
                }
                else
                {
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(PreLang);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(PreLang);
                }

            }

            AssignPreLang();
            SetDecryptString();
        }

        /// <summary>
        /// M : 에러 정보쓰기 메소드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MobileBaseControl_Error(object sender, System.EventArgs e)
        {
            StringBuilder errorMessage = new StringBuilder();
            Exception currentError = Server.GetLastError();

            errorMessage.Append("▶▶-------------MobileBaseControl--------------");
            errorMessage.Append("<br>\n▶▶Time : " + DateTime.Now.ToString());
            errorMessage.Append("<br>\n▶▶UserID : " + HttpContext.Current.User.Identity.Name);
            errorMessage.Append("<br>\n▶▶Server Name : " + Server.MachineName);
            errorMessage.Append("<br>\n▶▶Client Address : " + Request.UserHostAddress);
            errorMessage.Append("<br>\n▶▶Client Name : " + Request.UserHostName);
            errorMessage.Append("<br>\n▶▶Url : " + Request.Url.ToString());
            errorMessage.Append("<br>\n\n▶▶Error Message : \n" + currentError.Message.ToString());
            errorMessage.Append("<br>\n\n▶▶Stack Trace : \n" + currentError.ToString());

            if (!(currentError is Helper.AppException))
            {
                Helper.AppException.WriteException("▶▶MobileBaseControl.cs[MobileBaseControl_Error] : Error " + errorMessage.ToString(), currentError);
            }

            Server.ClearError();
            Response.Redirect("/DeskPlusMobile/Html/ErrorMessage/ErrorDefault.aspx");
        }
        #endregion

        #region Method
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

        /// <summary>
        /// 작성자 : shimpark
        /// 파리미터 암호화값 전역변수에 할당하기
        /// </summary>
        private void SetDecryptString()
        {
            string param = GetParm("p", "!NULL");
            if (!string.IsNullOrEmpty(param) && param != "!NULL")
            {
                param = Server.UrlDecode(param);//디코딩
                decryString = Utility.DecryptString(param);
                decryString = Server.UrlDecode(decryString);//디코딩
            }
        }


        /// <summary>
        /// 작성자   : shimpark
        /// 작성목적 : code behind 단에서 복고화 하여 파라미터값 가져오기
        /// </summary>
        /// <param name="_keyName">Name of the _key.</param>
        /// <param name="_errorReturn">The _error return.</param>
        /// <returns></returns>
        protected string GetDecryptParam(string _keyName, string _errorReturn)
        {
            if (!string.IsNullOrEmpty(decryString))
            {
                return GetParmforParameter(decryString, _keyName, _errorReturn);
            }

            return _errorReturn;

        }

        /// <summary>
        /// 작성목적 : 파리미터값으로 키의 값 가져오기
        /// </summary>
        /// <param name="_querystring">The _querystring.</param>
        /// <param name="_parmName">Name of the _parm.</param>
        /// <param name="_errorReturn">The _error return.</param>
        /// <returns></returns>
        protected string GetParmforParameter(string _querystring, string _parmName, string _errorReturn)
        {
            if (string.IsNullOrEmpty(_querystring) || _querystring == "!NULL") return _errorReturn;
            string response = String.Empty;

            _querystring = _querystring.Replace("&amp;", "~@~dp~@~");  //ByFreeman 20140516 URL 파라미터에 &값이 있을경우 오류

            _querystring = _querystring.ToReverseXSS();

            string[] pairs = _querystring.Split('&');
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string tmpPair = String.Empty;
            foreach (string pair in pairs)
            {
                if (String.IsNullOrEmpty(pair) || pair.ToNull() == "") continue;

                tmpPair = pair.Replace("~@~dp~@~", "&");  //ByFreeman 20140516 URL 파라미터에 &값이 있을경우 오류

                string[] paramvalue = tmpPair.Split('=');

                if (dic.ContainsKey(paramvalue[0]) == false && paramvalue[0].ToNull() != "")
                {
                    if (paramvalue.Length > 2) paramvalue[1] = tmpPair.Substring(tmpPair.IndexOf('=') + 1).Trim();    //특수문자 이후 사라지는 문제 ByFreeman 20131031

                    paramvalue[1] = paramvalue[1].Replace("＆", "&").Replace("＝", "="); 
                    dic.Add(paramvalue[0], paramvalue[1]);
                }
            }

            if (dic.ContainsKey(_parmName)) response = dic[_parmName];

            return (!string.IsNullOrEmpty(response)) ? response : _errorReturn;

            /*
            if (_querystring.Length == 0) return _errorReturn;

            string response = String.Empty;

            NameValueCollection qscoll = HttpUtility.ParseQueryString("?" + _querystring);
            foreach (String s in qscoll.AllKeys)
            {
                if (s == _parmName)
                {
                    response = qscoll[s];
                    break;
                }
            }

            return (!string.IsNullOrEmpty(response)) ? response : _errorReturn;
            */
        }



        /// <summary>
        /// 작성자 : 심재운
        /// 다국어언어 간추려서 하나의 언어 반환하기
        /// </summary>
        /// <param name="_name">The _name.</param>
        /// <param name="_ko">The _ko.</param>
        /// <param name="_en">The _en.</param>
        /// <param name="_ja">The _ja.</param>
        /// <param name="_fr">The _FR.</param>
        /// <param name="_zhcn">The _ZHCN.</param>
        /// <param name="_zhtw">The _ZHTW.</param>
        /// <param name="_preLang">The _pre lang.</param>
        /// <returns></returns>
        protected string GetLangName(string _name, string _ko, string _en, string _ja, string _fr,
            string _zhcn, string _zhtw, string _preLang)
        {
            return StringHelper.GetLangName(_name, _ko, _en, _ja, _fr, _zhcn, _zhtw, _preLang);
        }

        public string UserID
        {
            get { return userID.ToString(); }
        }

        public string CompanyID
        {
            get { return companyID.ToString(); }
        }


        /// <summary>
        /// 파일 가상디렉터리 경로
        /// </summary>
        /// <value>The file path.</value>
        public string FilePath
        {
            get { return Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "CONFIG_FILEPATH"); }
        }


        /// <summary>
        /// Gets the user info.
        /// </summary>
        /// <value>The user info.</value>
        public Helper.UserInfoHelper UserInfo
        {
            get { return userInfo; }
        }

        #region 다국어
        /// <summary>
        /// 다국어 리소스 인스턴스 메소드
        /// </summary>
        /// <remarks></remarks>
        protected void GetResourceManager()
        {
            resMng = new System.Resources.ResourceManager(ResourceName, AssemblyName);
        }

        /// <summary>
        /// 다국어 리소스 값 반환 메소드
        /// </summary>
        /// <param name="_resName">Name of the _res.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        protected string GetResource(string _resName)
        {
            return new System.Resources.ResourceManager(ResourceName, AssemblyName).GetString(_resName);
        }

        /// <summary>
        /// Gets the resource.
        /// </summary>
        /// <param name="_resName">Name of the _res.</param>
        /// <param name="_ResourceName">Name of the _ resource.</param>
        /// <param name="_AssemblyName">Name of the _ assembly.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        protected string GetResource(string _resName, string _ResourceName, Assembly _AssemblyName)
        {
            return new System.Resources.ResourceManager(_ResourceName, _AssemblyName).GetString(_resName);
        }

        #endregion
        #endregion

        /// <summary>
        /// 작성자   : 이현수
        /// 작성일자 : 2010-06-23
        /// 작성목적 : 사용자 환경 정보 가져오기
        /// </summary>
        /// <param name="_data"></param>
        /// <returns></returns>
        public string UserEnv
        {
            get
            {
                string userEnv = String.Empty; ;
                if (Context.Session["DeskPlusMobileUserEnv"] != null)
                {
                    userEnv = Context.Session["DeskPlusMobileUserEnv"].ToString();
                }
                else
                {
                    if (Context.Request.Cookies["DeskPlusMobileUserEnv"] == null)
                    {
                        userEnv = "0|0|0|0|";
                    }
                    else
                    {
                        userEnv = Context.Request.Cookies["DeskPlusMobileUserEnv"].Value;
                    }
                }

                return userEnv;
            }
        }

        /// <summary>
        /// 작성자   : 이현수
        /// 작성일자 : 2010-06-23
        /// 작성목적 : 메일 사용자 ID 가져오기
        /// </summary>
        /// <param name="_data"></param>
        /// <returns></returns>
        public string WebMailID
        {
            get
            {
                if (Session["WebMailID"] == null)
                {
                    if (Context.Request.Cookies["WebMailID"] == null)
                    {
                        //FormsAuthentication.SignOut();
                        //FormsAuthentication.RedirectToLoginPage();
                        Response.Redirect("/DeskPlusMobile/Portal/LogOut.aspx");

                        return "!NULL";
                    }
                    else
                    {
                        return Context.Request.Cookies["WebMailID"].Value;
                    }
                }
                else
                {
                    return Session["WebMailID"].ToString();
                }
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2010-07-09
        /// 작성목적 : 메일 도메인 ID 가져오기
        /// </summary>
        /// <param name="_data"></param>
        /// <returns></returns>
        public string WebDomainID
        {
            get
            {
                if (Session["WebDomainID"] == null)
                {
                    if (Context.Request.Cookies["WebDomainID"] == null)
                    {
                        //FormsAuthentication.SignOut();
                        //FormsAuthentication.RedirectToLoginPage();
                        Response.Redirect("/DeskPlusMobile/Portal/LogOut.aspx");

                        return "!NULL";
                    }
                    else
                    {
                        return Context.Request.Cookies["WebDomainID"].Value;
                    }
                }
                else
                {
                    return Session["WebDomainID"].ToString();
                }
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2013-02-09
        /// 작성목적 : 모바일 UUID 정보 
        /// </summary>
        public string UUID
        {
            get
            {
                if (Session["UUID"] == null)
                {
                    if (Context.Request.Cookies["UUID"] == null)
                    {
                        return "";
                    }
                    else
                    {
                        return Context.Request.Cookies["UUID"].Value;
                    }
                }
                else
                {
                    return Session["UUID"].ToString();
                }
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2013-09-25
        /// 작성목적 : 모바일 MobileTye 정보
        /// </summary>
        public string MobileType
        {
            get
            {
                if (Session["MobileType"] == null)
                {
                    if (Context.Request.Cookies["MobileType"] == null)
                    {
                        return "";
                    }
                    else
                    {
                        return Context.Request.Cookies["MobileType"].Value;
                    }
                }
                else
                {
                    return Session["MobileType"].ToString();
                }
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2015-1120
        /// 작성목적 : 모바일 RegID 정보
        /// </summary>
        public string RegID
        {
            get
            {
                if (Session["RegID"] == null)
                {
                    if (Context.Request.Cookies["RegID"] == null)
                    {
                        return "";
                    }
                    else
                    {
                        return Context.Request.Cookies["RegID"].Value;
                    }
                }
                else
                {
                    return Session["RegID"].ToString();
                }
            }
        }

        /// <summary>
        /// 다국어
        /// </summary>
        /// <value>The pre lang.</value>
        /// <remarks></remarks>
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
        /// <value>The pre lang real.</value>
        /// <remarks></remarks>
        public string PreLangReal { get; set; }

        /// <summary>
        /// 리소스명 프로퍼티
        /// </summary>
        /// <value>The name of the resource.</value>
        /// <remarks></remarks>
        public string ResourceName { get; set; }

        /// <summary>
        /// 어셈블리 프로퍼티
        /// </summary>
        /// <value>The name of the assembly.</value>
        /// <remarks></remarks>
        public Assembly AssemblyName { get; set; }
    }
}
