using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.EIP.Web.Admin.Portlet
{
    public partial class AddPortlet : Base.BasePageEIPAdmin
    {
        #region 변수설정
        public int PortletID;
        public int PortletTabID;
        #endregion

        #region Page Process (페이지 처리)

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.EIP.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Admin.Portlet.AddPortlet).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadData();
            }
        }

        /// <summary>
        /// 초기화
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            SetLanguage();

            UCPortletCategoryTreeScript1.CompanyID = CurrentUserInfo.CompanyID;
            UCPortletCategoryTreeScript1.UseIcon = true;

            if (PortletTabID > 0)
            {
                DAL.PortletTab dal = new DAL.PortletTab();
                List<BE.PortletTabInfo> al = dal.GetPortletTab(CurrentUserInfo.CompanyID, PortletID, PortletTabID);

                foreach (BE.PortletTabInfo info in al)
                {
                    ctbTabName.Text = info.NameBase;
                }
            }
        }

        #endregion

        #region Method (함수)

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            PortletID = GetDecryptParam("portletID", "0").ToNumber<int>();
            PortletTabID = GetDecryptParam("portletTabID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Confirm");
                cbtnClose.Text = resMng.GetString("Closed");
            }
        }

        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion

        #region Property (프로퍼티)
        #endregion
    }
}