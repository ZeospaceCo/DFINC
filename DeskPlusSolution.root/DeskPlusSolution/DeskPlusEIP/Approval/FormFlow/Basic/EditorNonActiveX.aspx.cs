using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.Web.FormFlow.Basic
{
    public partial class EditorNonActiveX : Base.BasePageApproval
    {
        #region 변수설정
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.FormFlow.Basic.EditorNonActiveX).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        #endregion

        #region (함수)
        #endregion

        #region (프로퍼티)
        public string TemplateHeader
        {
            get { return Helpers.WebComponent.Utility.GetEditorHtmlHeader(PreLang, ""); }
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