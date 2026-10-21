using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Reflection;
using System.Threading;
using System.Globalization;

namespace Zeospace.DeskPlus.Approval.Web.Common.Popup
{
    public partial class OpinionView : System.Web.UI.Page
    {
        #region 변수설정

        protected System.Resources.ResourceManager resMng = null;
        public string ResourceName { get; set; }
        public Assembly AssemblyName { get; set; }
        private string decryString = string.Empty;//복고화 값

        public int CompanyID;
        public int ChildDocID;
        public string UpPathID = string.Empty;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.UserLanguages != null)
            {
                string userLang = Request.UserLanguages[0].ToLower();
                if (userLang.StartsWith("en") || userLang.StartsWith("fr") || userLang.StartsWith("ja") || userLang.StartsWith("ko") || userLang.StartsWith("zh"))
                {
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Request.UserLanguages[0]);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(Request.UserLanguages[0]);
                }
                else
                {
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
                }
            }

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			        //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Common.Popup.OpinionView).Assembly;	    //어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                SetDecryptString();
                LoadData();
            }
        }

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        private void LoadData()
        {
            LoadParam();

            DAL.DocumentProcess dalDocProcess = new DAL.DocumentProcess();

            List<BE.DocumentProcessInfo> alDoc = dalDocProcess.GetDocumentUnion(CompanyID, ChildDocID);

            foreach (var docInfo in alDoc)
            {
                UpPathID = docInfo.UpPathID;
            }
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
            }
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CompanyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            ChildDocID = GetDecryptParam("childDocID", "").ToNumber<int>();
        }

        #endregion

        #region BasePage

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
        /// 작성자   : 
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
            try
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
                        dic.Add(paramvalue[0], paramvalue[1]);
                    }
                }

                if (dic.ContainsKey(_parmName)) response = dic[_parmName];

                return (!string.IsNullOrEmpty(response)) ? response : _errorReturn;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("_querystring : " + _querystring, AppSettingType.EIP);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("_parmName : " + _parmName, AppSettingType.EIP);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return "";
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
        public string CssPath
        {
            get { return AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseCssDirectory", "/DeskPlusEIP/Design"); }
        }

        public string ImgPath
        {
            get { return AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/"); }
        }

        #endregion
    }
}