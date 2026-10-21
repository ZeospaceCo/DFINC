using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Education.Web.Admin
{
    public partial class DashBoard : Base.BasePageEducationAdmin
    {
        #region Page Process (페이지 처리)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Education.Web.Resource.GlobalString.string";			
            this.AssemblyName = typeof(Zeospace.DeskPlus.Education.Web.Admin.DashBoard).Assembly;	
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            ddlTarget.SelectedValue = SearchTarget;
            ddlGubun.SelectedValue = SearchItem;
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
            }
        }

        #endregion
    }
}