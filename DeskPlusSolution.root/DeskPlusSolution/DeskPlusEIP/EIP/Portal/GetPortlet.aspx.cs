using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.EIP.Web.Portal
{
    public partial class GetPortlet : Base.BasePageEIP
    {
        #region 변수설정
        public int CategoryID;
        public int TotalCount;
        public int TabID;
        public string FirstColumn;
        public string RecordPerPage = string.Empty;
        public string Order = string.Empty;
        public string SearchKey = string.Empty;
        public string SearchValue = string.Empty;
        public string PhotoName = "";
        public string IsAdmin = "";
        public string TabLayout = "";
        public string LayoutCols = "";
        public string Options = "";
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Portal.GetPortlet).Assembly;	//어셈블리 선언
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

            CommonPortletTree.CompanyID = CurrentUserInfo.CompanyID;
            CommonPortletTree.UseIcon = true;
            CommonPortletTree.Gubun = "COMMON"; //공용
            CommonPortletTree.CategoryID = CategoryID;

            PrivatePortletTree.CompanyID = CurrentUserInfo.CompanyID;
            PrivatePortletTree.Gubun = "PRIVATE"; //개인용
            PrivatePortletTree.UseIcon = true;
            PrivatePortletTree.CategoryID = CategoryID;

            RecordPerPage = "10";//GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.COMMON, "ListCount", "10");//페이지리스트수

            ddlListCount.SelectedValue = RecordPerPage;

            GetListCount();

            if (LayoutCols.Length > 0)
            {
                string[] colsArr = LayoutCols.Split(',');

                foreach (string col in colsArr)
                {
                    Options += "<option value='" + col + "'>" + col + "</option>";
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
            CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            TabID = GetDecryptParam("tabID", "0").ToNumber<int>();
            FirstColumn = GetDecryptParam("firstColumn", "");
            IsAdmin = GetDecryptParam("isAdmin", "");
            TabLayout = GetDecryptParam("tabLayout", "5");
            LayoutCols = GetDecryptParam("layoutCols", "col1");
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                CbtnClose.Text = this.resMng.GetString("Closed");
            }
        }


        /// <summary>
        /// 포틀릿 리스트 총 카운트
        /// </summary>
        private void GetListCount()
        {
            DAL.Portlet biz = new DAL.Portlet();

            TotalCount = biz.GetPageCount(CurrentUserInfo.CompanyID, CategoryID, TabID, CurrentUserInfo.UserID,
                string.Join(",",CurrentUserInfo.RoleIDs), SearchKey, SearchValue, true);
        }

        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion

        #region Property (프로퍼티)
        #endregion
    }
}