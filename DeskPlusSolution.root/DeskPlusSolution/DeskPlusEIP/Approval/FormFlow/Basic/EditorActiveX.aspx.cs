using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.IO;

namespace Zeospace.DeskPlus.Approval.Web.FormFlow.Basic
{
    public partial class EditorActiveX : Base.BasePageApproval
    {
        #region 변수설정
        #endregion 

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.FormFlow.Basic.EditorActiveX).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                ////에디터 본문 이미지 첨부 경로
                //System.Guid guid = System.Guid.NewGuid();
                //string reqTime = FileHelper.GetYearMonth();
                //string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                //UploadPath = baseFileDirectory + CurrentUserInfo.CompanyID + "/Approval/Editor/" + reqTime + "/" + guid.ToString();

                //if (!Directory.Exists(Server.MapPath(UploadPath))) Directory.CreateDirectory(Server.MapPath(UploadPath));
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        #endregion

        #region (함수)
        #endregion

        #region (프로퍼티)
        /// <summary>
        /// 에디터-파일처리경로
        /// </summary>
        public string UploadPath
        {
            get { return GetViewStateString("UploadPath", ""); }
            set { ViewState["UploadPath"] = value; }
        }
        public string UploadPort
        {
            get
            {
                return AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttpPort", "80"); 
            }
        }
        public string BaseHttpVal
        {
            get
            {
                return AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "http://");
            }
        }
        public string BaseHost
        {
            get
            {
                return Request.ServerVariables["SERVER_NAME"].ToString();
            }
        }
        public int CompanyID
        {
            get { return CurrentUserInfo.CompanyID; }
        }
        public int UserID
        {
            get { return CurrentUserInfo.UserID; }
        }

        public string TemplateHeader
        {
            get { return Helpers.WebComponent.Utility.GetEditorHtmlHeader(PreLang, "");}
        }

        public string TemplateFooter
        {
            get { return "</BODY></HTML>"; }
        }
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}