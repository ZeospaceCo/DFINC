using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.Approval
{
    public partial class EditorNonActiveX : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Approval.EditorNonActiveX).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2019-07-16
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {

            }
        }

        #region 프로퍼티
        public string TemplateHeader
        {
            get { return Zeospace.DeskPlusMobile.Web.Helper.WebComponent.Utility.GetEditorHtmlHeader(PreLang, ""); }
        }

        public string TemplateFooter
        {
            get { return "</BODY></HTML>"; }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2019-07-16
        /// 작성목적 : SSL     
        /// </summary>
        public string httpType
        {
            get
            {
                return Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "HTTP_TYPE");
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2019-07-16
        /// 작성목적 : 도메인 정보        
        /// </summary>
        public string MobileDOMAIN
        {
            get
            {
                return Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "MobileDOMAIN");
            }
        }
        #endregion
    }
}