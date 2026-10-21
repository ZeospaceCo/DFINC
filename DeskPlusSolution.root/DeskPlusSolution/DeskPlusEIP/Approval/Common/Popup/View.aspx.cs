using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Threading;
using System.Globalization;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;


namespace Zeospace.DeskPlus.Approval.Web.Common.Popup
{
    public partial class View : System.Web.UI.Page
    {
        #region 변수설정
        protected System.Resources.ResourceManager resMng = null;
        public string ResourceName { get; set; }
        public Assembly AssemblyName { get; set; }
        private string decryString = string.Empty;//복고화 값

        public int CompanyID;
        public int DocumentID;
        public int ChildDocID = 0;
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.UserLanguages != null)
            {
                PreLang = Request.UserLanguages[0];
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(PreLang);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(PreLang);
            }

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Common.Popup.View).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                SetDecryptString();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            // 문서정보 가져오기
            GetDocInfo(CompanyID, DocumentID);
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnPrint.Text = resMng.GetString("Print"); //"인쇄";
                cbtnOpinionInfo.Text = resMng.GetString("Opinion"); //결재의견
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
            DocumentID = GetDecryptParam("documentID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 문서정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        public void GetDocInfo(int _companyID, int _documentID)
        {
            if (_companyID.Equals(0) || _documentID.Equals(0)) return;

            #region 문서내용

            DAL.DocumentProcess dpDal = new DAL.DocumentProcess();
            List<BE.DocumentProcessInfo> list = dpDal.GetDocumentUnion(_companyID, _documentID);
            foreach (BE.DocumentProcessInfo info in list)
            {
                ChildDocID = info.LastDocID;
                htaFormHTML.Value = info.ContentHTML;
            }

            #endregion

            #region 근거문서/첨부파일 건수
            if (!htaFormHTML.Value.Equals(""))
            {
                DAL.AttachFile dal = new DAL.AttachFile();
                headerAttachCnt.InnerText = dal.GetAttachFileCntByPath(_companyID, ChildDocID).ToString();

                DAL.BaseDocument dalBase = new DAL.BaseDocument();
                headerBaseDocCnt.InnerText = dalBase.GetBaseDocCntByPath(_companyID, ChildDocID).ToString();
            }
            else
            {
                headerAttachCnt.InnerText = "0";
                headerBaseDocCnt.InnerText = "0";
            }
            #endregion
        }

        #endregion

        #region (프로퍼티)
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
        #endregion

        #region (이벤트핸들러)
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
            get { return AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseCssDirectory", "/DeskPlusEIP/Design/"); }
        }

        public string ImgPath
        {
            get { return AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/"); }
        }

        #endregion
    }
}