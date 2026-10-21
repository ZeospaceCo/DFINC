using System;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Address.Web.Mail
{
    public partial class AddressSearch : System.Web.UI.Page
    {
        #region 변수설정

        //to, cc, bc
        public string Gubun = String.Empty;

        public string MailServerIP;

        //메일 로그인
        public string MailLogin;

        public string MailProduct = String.Empty;

        protected System.Resources.ResourceManager resMng = null;

        public string ResourceName { get; set; }

        public Assembly AssemblyName { get; set; }

        public int CompanyID;
        public int UserID;
        #endregion 변수설정

        #region Page Process (페이지 처리)

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParam();

            if (Request.UserLanguages != null)
            {
                PreLang = Request.UserLanguages[0];
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(PreLang);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(PreLang);
            }

            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Address.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Address.Web.Mail.AddressSearch).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : shimpark
        /// 작성일자 :
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            if (Request.UrlReferrer != null)
            {
                AppException.WriteAssert("AddressSearch referer : " + Request.UrlReferrer.ToString(), AppSettingType.MAIL);
            }

            UCAddressMailTree.PreLang = PreLang;
            UCAddressMailTree.CompanyID = 0;
            UCAddressMailTree.UserID = 0;

            if (MailLogin.Length == 0)
            {
                if (!Page.ClientScript.IsClientScriptBlockRegistered("AddressSearch"))
                {
                    string strScript = @"<script type=""text/javascript"">alert('{0}');self.close();" + "<" + "/script>";
                    strScript = String.Format(strScript, "please send me 'loginID' parameter value.");
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "AddressSearch", strScript);

                    cbtnConfirm.Visible = false;
                    cbtnClose.Visible = false;

                    return;
                }
            }

            MailServerIP = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailServerIP", "");
            MailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "");

            if (MailLogin.Length > 0)
            {
                if (MailLogin.IndexOf('@') != -1)
                {
                    Zeospace.DeskPlus.Account.DAL.UserMailDomain uMail = new Account.DAL.UserMailDomain();
                    var mailList = uMail.GetUserMailDomain(MailLogin.Split('@')[0], MailLogin.Split('@')[1]);

                    foreach (var info in mailList)
                    {
                        CompanyID = info.CompanyID;
                        UserID = info.UserID;

                        UCAddressMailTree.CompanyID = info.CompanyID;
                        UCAddressMailTree.UserID = info.UserID;
                    }
                }
            }

            if (CompanyID == 0 || UserID == 0)
            {
                if (!Page.ClientScript.IsClientScriptBlockRegistered("AddressSearch"))
                {
                    string strScript = @"<script type=""text/javascript"">alert('{0}');self.close();" + "<" + "/script>";
                    strScript = String.Format(strScript, "This user does not exist.");
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "AddressSearch", strScript);

                    cbtnConfirm.Visible = false;
                    cbtnClose.Visible = false;

                    return;
                }
            }

            //if (Request.Cookies["ss_webmail_id"]["Mail"] != null)
            //{
            //    Mail = Request.Cookies["ss_webmail_id"]["Mail"].ToString();
            //    LoginCheck();
            //}
        }

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #region (함수)

        /// <summary>
        /// 작성자 : shimpark
        /// 작성일자 :
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Confirm");     //"확인";
                cbtnClose.Text = resMng.GetString("Close");         //"닫기";
                cbtnSearch.Text = resMng.GetString("Search");       //"검색";
                cbtnAdd.Text = resMng.GetString("AddUser");         //"사용자추가";
                cbtnAddGroup.Text = resMng.GetString("AddGroup");         //"그룹추가";

                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("FirstName"), "ManyName"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("EmailEx"), "Mail"));
            }
        }

        /// <summary>
        /// 작성자 : shimpark
        /// 작성일자 :
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            Gubun = GetParm("type", "to");
            MailLogin = GetParm("loginID", "");
            PreLang = GetParm("locale", "ko");

            switch (PreLang.ToLower())
            {
                case "ko":
                    PreLang = "ko";
                    break;

                case "kr":
                    PreLang = "ko";
                    break;

                case "en":
                    PreLang = "en";
                    break;

                case "jp":
                    PreLang = "ja";
                    break;

                case "cn":
                    PreLang = "zh-cn";
                    break;

                default:
                    PreLang = "ko";
                    break;
            }
        }

        #endregion (함수)

        /// <summary>
        /// 메서드명 :
        /// 작성목적 : Request 파라미터 STRING 가져오기
        /// 작성자   : 심재운
        /// </summary>
        /// <param name="_parmName">Name of the _parm.</param>
        /// <param name="_errorReturn">The _error return.</param>
        /// <returns></returns>
        /// <remarks></remarks>
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
                return Utility.EncryptString(Server.UrlEncode(data));
            }
            catch
            {
                return "!NULL";
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
        private string GetViewStateString(string _viewStateName, string _errorReturn)
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
        private int GetViewStateInt(string _viewStateName, string _errorReturn)
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

        public string PreLang
        {
            get { return ViewState["PreLang"].ToString(); }
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

                ViewState["PreLang"] = lang;
            }
        }
    }
}