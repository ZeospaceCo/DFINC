using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.PMS.Web.Project
{
    public partial class Default : Zeospace.DeskPlus.PMS.Web.Base.BasePagePMS
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Project.Default).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                LoadData();
            }
        }

        #endregion

        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 메서드명   : 
		/// 작성목적   : 데이터가져오기
		/// 작성자     : 심재운
		/// 최초작성일 : 2009-01-16
        /// </summary>
        public void LoadData()
        {
            
            string projectID = GetDecryptParam("projectID", "");
            //if (projectID != "")
            //{
            //    menuFrm.Attributes.Add("src", "menu.aspx?" + ProjParam);
            //    contentFrm.Attributes.Add("src", "main.aspx?" + ProjParam);
            //}
        }

        #endregion

        #region Method (함수)

        #endregion

        #region Property (프로퍼티)

        #endregion
        
        #region Event Handler (이벤트 핸들러)

        #endregion
    }
}
