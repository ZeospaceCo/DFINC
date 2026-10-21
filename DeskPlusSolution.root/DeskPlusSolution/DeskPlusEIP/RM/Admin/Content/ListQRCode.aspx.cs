using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.RM.Web.Admin.Content
{
    public partial class ListQRCode : Zeospace.DeskPlus.RM.Web.Base.BasePageRMAdmin
    {
        #region 변수설정
        public int CurrentPage = 0;
        public string TotalCount = String.Empty;  //총카운트
        public string RecordPerPage = String.Empty; //리스트 개수
        public string SearchField = String.Empty; //검색키워드
        public string SearchValue = String.Empty; //검색어
        public string SearchTerm = String.Empty;  //기간검색키워드
        public string SearchTermValue = String.Empty;  //기간검색어       
        public string Order = String.Empty; //sort            
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.RM.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.RM.Web.Admin.Content.ListQRCode).Assembly;	//어셈블리 선언
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
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-08
        /// 작성목적 : Data Load
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            
            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;
            ddlSearchTerm.SelectedValue = SearchTerm;

            #region PageSize 설정하기

            RecordPerPage = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.RESOURCEMANAGER, "ListCount", "10");//페이지리스트수

            DAL.CategoryUser dal = new DAL.CategoryUser();
            List<BE.CategoryUserInfo> al = dal.GetList(CurrentUserInfo.CompanyID, 0, CurrentUserInfo.UserID, "Q");

            foreach (BE.CategoryUserInfo userInfo in al)
            {
                RecordPerPage = userInfo.PageSize.ToString();
            }

            ddlListCount.SelectedValue = RecordPerPage;

            #endregion

            GetListCount();
        }


        #endregion

        #region (함수)

        /// <summary>
        /// 언어 설정하기
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                cbtnPrint.Text = this.resMng.GetString("Print");
                cbtnSearch.Text = this.resMng.GetString("Search");

                ddlSearchTerm.Items.Clear();
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("ALL"), ""));
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("Week1"), "week"));
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("Month1"), "month"));
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("Period"), "period")); 

                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("ResourcesTitle"), "Title"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("WriteUserName"), "ManyName"));
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-08
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            SearchTerm = GetDecryptParam("searchTerm", "");
            SearchTermValue = GetDecryptParam("searchTermValue", "");
            Order = GetDecryptParam("order", "");
            Order = (Order.Length == 0) ? "WriteDate desc" : Order;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-08
        /// 작성목적 : 게시물 총 갯수
        /// </summary>
        private void GetListCount()
        {
            string startDate = "";
            string endDate = "";
            string getDate = "";

            if (SearchTerm == "week" || SearchTerm == "month")
            {
                if (SearchTerm == "week")
                    getDate = DateHelper.GetCultureStartEndDate("week", PreLangReal);
                else if (SearchTerm == "month")
                    getDate = DateHelper.GetCultureStartEndDate("month", PreLangReal);

                startDate = DateHelper.GetCultureDate(getDate.Split(',')[0], "yyyyMMdd", PreLangReal);
                endDate = DateHelper.GetCultureDate(getDate.Split(',')[1], "yyyyMMdd", PreLangReal);
            }
            else if (SearchTerm == "period")
            {
                startDate = DateHelper.GetCultureDate(SearchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                endDate = DateHelper.GetCultureDate(SearchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
            }

            DAL.List dal = new DAL.List();
            TotalCount = dal.GetQRCodeListCount(CurrentUserInfo.CompanyID, SearchField, SearchValue, startDate, endDate).ToString();
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}