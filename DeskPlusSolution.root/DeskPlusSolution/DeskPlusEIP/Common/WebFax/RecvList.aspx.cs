using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Common.Web.WebFax
{
    public partial class RecvList : Base.BasePageCommon
    {
        #region 변수설정
        public int CurrentPage = 0;
        public string RecordPerPage = "";
        public string SearchTerm = string.Empty; //검색기간구분자
        public string SearchTermValue = string.Empty; //검색기간 시작,종룓값        
        public string SearchKey = "";
        public string SearchValue = "";
        public string Order = "";
        public string FaxID = "";
        public string BranchID = "";
        public string PageUrl = "";
        #endregion

        #region Page Process (페이지 처리)
        #endregion

        #region Method (함수)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Common.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Common.Web.WebFax.RecvList).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 내용가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2011-04-25
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            bool isAdmin = CurrentUserInfo.IsInRole("webfax");//webfax admin
            cbtnAdmin.Visible = (isAdmin) ? true : false;

            ddlSearchTerm.SelectedValue = SearchTerm;
            ddlSearchKey.SelectedValue = SearchKey;
            ddlListCount.SelectedValue = RecordPerPage;
            ctbSearch.Text = SearchValue;
        }


        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            RecordPerPage = GetDecryptParam("recordPerPage", "5");
            SearchTerm = GetDecryptParam("searchTerm", "");
            SearchTermValue = GetDecryptParam("searchTermValue", "");
            SearchKey = GetDecryptParam("searchKey", "");
            SearchValue = GetDecryptParam("searchValue", "");
            Order = GetDecryptParam("order", "");
            BranchID = GetDecryptParam("branchID", "");
            FaxID = GetDecryptParam("faxID", "");
            PageUrl = GetDecryptParam("pageUrl", "");

        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnDel.Text = resMng.GetString("Del");
                cbtnSearch.Text = resMng.GetString("Searh");
                cbtnAdmin.Text = resMng.GetString("FaxAdmin");

                ddlSearchKey.Items.Clear();                
                ddlSearchKey.Items.Add(new ListItem(resMng.GetString("FaxNumber"), "FaxNum"));

                ddlSearchTerm.Items[0].Text = this.resMng.GetString("Whole");
                ddlSearchTerm.Items[1].Text = this.resMng.GetString("Week");
                ddlSearchTerm.Items[2].Text = this.resMng.GetString("Month");
                ddlSearchTerm.Items[3].Text = this.resMng.GetString("Period");
            }
        }


        #endregion

        #region Event Handler (이벤트 핸들러)

        #endregion

        #region Property (프로퍼티)

        #endregion
    }
}