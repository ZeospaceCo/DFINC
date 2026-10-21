using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Common.Web.WebFax.Admin
{
    public partial class List : Base.BasePageCommon
    {
        #region 변수설정
        public int CurrentPage = 0;
        public string RecordPerPage = "";
        public string SearchKey = "";
        public string SearchValue = "";
        public string Order = "";
        public string Gubun = "";
        #endregion

        #region Page Process (페이지 처리)
        #endregion

        #region Method (함수)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Common.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Common.Web.WebFax.Admin.List).Assembly;	//어셈블리 선언
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
            if (!isAdmin)
            {
                Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
                return;
            }

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
            SearchKey = GetDecryptParam("searchKey", "");
            SearchValue = GetDecryptParam("searchValue", "");
            Order = GetDecryptParam("order", "");
            Gubun = GetDecryptParam("gubun", ""); 
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnAdd.Text = resMng.GetString("Add");
                cbtnDel.Text = resMng.GetString("Del");
                cbtnSearch.Text = resMng.GetString("Searh");
                cbtnActive.Text = resMng.GetString("FaxIsActive");

                ddlSearchKey.Items.Clear();
                ddlSearchKey.Items.Add(new ListItem(resMng.GetString("FaxGroupName"), "ManyName"));
                ddlSearchKey.Items.Add(new ListItem(resMng.GetString("FaxNumber"), "FaxNumber"));

                ltlTitle.Text = (Gubun == "1") ? resMng.GetString("FaxNumber") : resMng.GetString("FaxWebFaxNumber");

            }
        }

        
        #endregion

        #region Event Handler (이벤트 핸들러)

        #endregion

        #region Property (프로퍼티)

        #endregion
    }
}