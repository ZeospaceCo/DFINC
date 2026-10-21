using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Account.Web.Common
{
    public partial class UserListByDept : Base.BasePageAccount
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정       

        public int DeptID;        
        
        #endregion

        #region Page Process (페이지 처리)
        #endregion

        #region Method (함수)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Common.UserListByDept).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();
            }
        }

        /// <summary>
        /// 작성목적   : 초기화 데이터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadData()
        {
            ddlSearchGubun.SelectedValue = SearchGubun;
            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;
            ddlListCount.SelectedValue = RecordPerPage;

            GetListCount();            
        }


        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadParam()
        {
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            RecordPerPage = GetDecryptParam("recordPerPage", "5");
            SearchTerm = GetDecryptParam("searchTerm", "");
            SearchTermValue = GetDecryptParam("searchTermValue", "");
            SearchGubun = GetDecryptParam("searchGubun", "");
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            Order = GetDecryptParam("order", "");

            DeptID = GetDecryptParam("deptID", "0").ToNumber<int>();            
            SearchGubun = (DeptID == 0) ? "Whole" : "Internal";
            Order = (Order.Length == 0) ? "TitleCodeOrder asc, DutyCodeOrder asc, UserDeptOrder desc, WriteDate desc" : Order;

            Callback = GetDecryptParam("callback", "");
        }

        /// <summary>
        /// 작성목적   : 다국어 설정
        /// 작성자     : 
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                
                cbtnSearch.Text = this.resMng.GetString("Admin_DeptManager_btnSearch");
                cbtnConfirm.Text = this.resMng.GetString("BtnConfirm");
                cbtnClose.Text = this.resMng.GetString("BtnClose");

                #region 구분자
                ddlSearchGubun.Items.Clear();
                ddlSearchGubun.Items.Add(new ListItem(this.resMng.GetString("InternalSearch"), "Internal"));
                ddlSearchGubun.Items.Add(new ListItem(this.resMng.GetString("WholeSearch"), "Whole"));
                #endregion

                #region 인사검색필드
                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Private_AccountInfo_ltUserName"), "ManyName"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("LoginID"), "LoginID"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Private_AccountInfo_ltTitle"), "ManyTitleName"));
                #endregion



            }
        }

        /// <summary>
        /// 작성자   : shimpark
        /// 해당 부서 사용자 카운트 가져오기
        /// </summary>
        private void GetListCount()
        {
            DAL.User biz = new DAL.User();

            //검색은 전체에서 검색한다.
            //string deptID = (SearchValue.Length > 0) ? "0" : DeptID.ToString();
            string deptID = (SearchGubun == "Whole") ? "0" : DeptID.ToString();
            string[] dateValue = DateHelper.GetSearchTerm(SearchTerm, SearchValue, PreLangReal);

            TotalCount = biz.GetPageListCount(CurrentUserInfo.CompanyID,
                deptID, "0", false, SearchField, SearchValue, dateValue[0], dateValue[1]).ToString();
        }


        

        #endregion

        #region Event Handler (이벤트 핸들러)  

        #endregion

        #region Property (프로퍼티)


        protected int CurrentPage { get; set; } //현페이지
        protected string TotalCount { get; set; } //총카운트
        protected string RecordPerPage { get; set; } //게시판 리스트 개수  
        protected string SearchTerm { get; set; } //검색기간구분자
        protected string SearchTermValue { get; set; } //검색기간 시작,종룓값
        protected string SearchGubun { get; set; } //전체,분류내 검색
        protected string SearchField { get; set; } //검색키워드
        protected string SearchValue { get; set; } //검색어
        protected string Order { get; set; } //sort  
        protected string Callback { get; set; } //Callback  

        #endregion
    }
}