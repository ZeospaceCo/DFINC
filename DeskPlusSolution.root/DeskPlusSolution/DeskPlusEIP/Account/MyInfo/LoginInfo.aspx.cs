using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Account.Web.MyInfo
{
    public partial class LoginInfo : Base.BasePageAccount
    {
        public string LoginTable = "";
        public string SearchTerm = "";
        public string SearchTermValue = "";        
        public int TotalCount = 0;
        public int CurrentPage = 0;
        public int CompanyID = 0;
        public int UserID = 0;
        public string RecordPerPage = "";

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.MyInfo.LoginInfo).Assembly;	//어셈블리 선언
            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region 함수
        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                cbtnSearch.Text = resMng.GetString("Admin_DeptManager_btnSearch");
                cbtnUser.Text = resMng.GetString("Admin_UserManager_tabs0");
                cbtnList.Text = resMng.GetString("Admin_UserRegister_Close");
                ddlSearchTerm.Items.Clear();
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("SearchTerm6M"), "6M"));    //6개월
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("SearchTerm1Y"), "1Y"));    //1년
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("SearchTerm2Y"), "2Y"));    //2년
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("SearchTerm3Y"), "3Y"));    //3년
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("SearchTermAL"), "AL"));    //전체
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("SearchTermPE"), "PE"));    //기간입력
            }
        }

        /// <summary>
        /// 페이지로드
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            GetListCount();
            DAL.User dal = new DAL.User();
            ddlSearchTerm.SelectedValue = SearchTerm;
            ddlListCount.SelectedValue = RecordPerPage;
        }

        /// <summary>
        /// 파라메터
        /// </summary>
        private void LoadParam()
        {            
            CurrentPage =  GetDecryptParam("currentPage", "1").ToNumber<int>();
            SearchTermValue = GetDecryptParam("searchTermValue", "");
            SearchTerm = GetDecryptParam("searchTerm", "6M");
            RecordPerPage = GetDecryptParam("recordPerPage", "15"); 
            CompanyID = CurrentUserInfo.CompanyID;
            UserID = CurrentUserInfo.UserID;            
        }

        private void GetListCount()
        {
            string startDate = "";
            string endDate = "";
            string getDate = "";

            if (string.Compare(SearchTerm, "6M", true) == 0 || string.Compare(SearchTerm, "1Y", true) == 0 || string.Compare(SearchTerm, "2Y", true) == 0 || string.Compare(SearchTerm, "3Y", true) == 0)
            {
                if (string.Compare(SearchTerm, "6M", true) == 0)
                    getDate = DateHelper.GetCultureStartEndDatePeriod("6M", PreLangReal);
                else if (string.Compare(SearchTerm, "1Y", true) == 0)
                    getDate = DateHelper.GetCultureStartEndDatePeriod("1Y", PreLangReal);
                else if (string.Compare(SearchTerm, "2Y", true) == 0)
                    getDate = DateHelper.GetCultureStartEndDatePeriod("2Y", PreLangReal);
                else if (string.Compare(SearchTerm, "3Y", true) == 0)
                    getDate = DateHelper.GetCultureStartEndDatePeriod("3Y", PreLangReal);

                startDate = DateHelper.GetCultureDate(getDate.Split(',')[0], "yyyyMMdd", PreLangReal);
                endDate = DateHelper.GetCultureDate(getDate.Split(',')[1], "yyyyMMdd", PreLangReal);
            }
            else if (string.Compare(SearchTerm, "PE", true) == 0)
            {
                startDate = DateHelper.GetCultureDate(SearchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                endDate = DateHelper.GetCultureDate(SearchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
            }

            DAL.User dal = new DAL.User();
            TotalCount = dal.GetLoginInfoCount(CompanyID, UserID, startDate, endDate);
        }
        #endregion 
    }
}