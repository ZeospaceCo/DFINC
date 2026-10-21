using System;
using System.Globalization;
using System.Reflection;
using System.Threading;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail;
using System.Web.UI;
using System.Text.RegularExpressions;

namespace Zeospace.DeskPlus.Base.Web.Site.Auth
{
    public partial class SendPassword : System.Web.UI.Page
    {
        #region 변수설정

        protected System.Resources.ResourceManager resMng = null;        
        public string ResourceName { get; set; }
        public Assembly AssemblyName { get; set; }
        private string decryString = string.Empty;//복고화 값

        public int CompanyID = 0;
        public string LoginID = "";
        public string AuthEmail = "";
        public string ManagerEmail = "";
        public string Gubun = "";

        #endregion 변수설정

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.UserLanguages != null)
            {
                PreLangs = Request.UserLanguages[0];
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(PreLangs);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(PreLangs);
            }

            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Base.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Base.Web.Site.Auth.SendPassword).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            GetResourceManager();

            if (!IsPostBack)
            {
                SetLanguage();
                SetDecryptString();
                LoadData();
            }

        }

        /// <summary>
        /// 로드시 인증 이메일이 텍스트박스에 Load 되어야 함
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            CompanyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            LoginID = GetDecryptParam("loginID", "");
            
            //로그인 시도한 사용자 2차 메일 아이디 가져오기
            Zeospace.DeskPlus.Account.DAL.User dal = new Account.DAL.User();
            DataTableReader dtr = dal.GetAuthEmail(CompanyID, LoginID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        AuthEmail = dtr["AuthEmail"].ToString();                        
                    }
                }
            }

            Regex regex = new Regex(@"^[0-9a-zA-Z]([-_.]?[0-9a-zA-Z])*@[0-9a-zA-Z]([-_.]?[0-9a-zA-Z])*.[a-zA-Z]{2,3}$"); //메일 정규식

            Match match = regex.Match(AuthEmail);
            if (match.Success) //이메일 형태가 맞을 경우엔
            {
                string mailid = AuthEmail.Split('@')[0].ToString();
                string domain = AuthEmail.Split('@')[1].ToString();

                if (mailid.Length <= 2) //메일ID의 길이가 2자 이상이면
                {
                    mailid = mailid.Remove(mailid.Length - 1, 1) + "*";
                    AuthEmail = mailid + "@" + domain;
                }
                else if (mailid.Length > 2) //메일ID의 길이가 2자 이상이면
                {
                    mailid = mailid.Remove(mailid.Length - 2, 2) + "**";
                    AuthEmail = mailid + "@" + domain;
                }
            }

            //발송 관리자 이메일 가져옴
            Zeospace.DeskPlus.Account.DAL.PasswordManager managerdal = new Account.DAL.PasswordManager();
            DataTableReader dtr2 = managerdal.GetManager(CompanyID);
            if (dtr2 != null)
            {
                if (dtr2.HasRows)
                {
                    while (dtr2.Read())
                    {
                        ManagerEmail = dtr2["mail"].ToString();
                    }
                }
            }
        }


        #region Property (프로퍼티)

        /// <summary>
        /// 다국어
        /// </summary>
        /// <value>
        /// The pre langs.
        /// </value>
        public string PreLangs
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

        #endregion Property (프로퍼티)

        #region Method (함수)

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
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            ViewState["CompanyID"] = GetDecryptParam("companyID", "0").ToNumber<int>();
            ViewState["LoginID"] = GetDecryptParam("loginID", "");

            Gubun = GetDecryptParam("gubun", "");
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                this.cbtnConfirm.Text = resMng.GetString("SendEmail");
                this.cbtnCheckConfirm.Text = resMng.GetString("Send");
                this.cbtnClose.Text = resMng.GetString("Close");
                this.cbtnLoginCheck.Text = resMng.GetString("Search");
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
                return UtilHelper.GetParmforParameter(decryString, _keyName, _errorReturn);
            }

            return _errorReturn;
        }

        #endregion Method (함수)

    }
}