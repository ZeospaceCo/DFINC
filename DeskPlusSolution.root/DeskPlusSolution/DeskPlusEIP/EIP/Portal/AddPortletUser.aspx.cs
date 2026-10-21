using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.EIP.Web.Portal
{
    public partial class AddPortletUser : Base.BasePageEIP
    {
        #region 변수설정        
        public int TotalCount;        
        public string RecordPerPage = string.Empty;
        public string Order = string.Empty;
        public string SearchKey = string.Empty;
        public string SearchValue = string.Empty;
        public string PhotoName = "";
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Portal.AddPortletUser).Assembly;	//어셈블리 선언
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

            PrivatePortletTree.CompanyID = CurrentUserInfo.CompanyID;
            PrivatePortletTree.Gubun = "PRIVATE"; //개인용
            PrivatePortletTree.UseIcon = true;
            PrivatePortletTree.CategoryID = 0;

            RecordPerPage = "10";//GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.COMMON, "ListCount", "10");//페이지리스트수

            ddlListCount.SelectedValue = RecordPerPage;

            GetListCount();
        }

        #endregion

        #region Method (함수)

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                cbtnClose.Text = this.resMng.GetString("Closed");
                cbtnAddPortlet.Text = this.resMng.GetString("AddRSS");
            }
        }


        /// <summary>
        /// 개인용 포틀릿 리스트 총 카운트
        /// </summary>
        private void GetListCount()
        {
            DAL.Portlet biz = new DAL.Portlet();
            TotalCount = biz.GetUserPageListCount(CurrentUserInfo.CompanyID, 0, false, CurrentUserInfo.UserID, "", "", 0);
        }

        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion

        #region Property (프로퍼티)
        #endregion
    }
}