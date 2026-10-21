using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helper = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using System.Data;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.MDM.Web.Dept
{
    public partial class List : Base.BasePageMDMAdmin
    {
        #region 변수설정
        public int DeptID = 0;
        public int CurrentPage = 0;
        public string TotalCount = String.Empty;  //총카운트
        public string RecordPerPage = String.Empty; //리스트 개수
        public string Order = String.Empty; //sort 
        public string SearchGubun = string.Empty; //분류
        public string SearchField = string.Empty; //검색키워드
        public string SearchValue = string.Empty; //검색어
        public string SiteMap = String.Empty;//사이트맵
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
            this.ResourceName = "Zeospace.DeskPlus.MDM.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.MDM.Web.Dept.List).Assembly;	//어셈블리 선언
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
        /// 작성일자 : 2013-01-17
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            ddlSearchGubun.SelectedValue = SearchGubun;
            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;
            ddlListCount.SelectedValue = RecordPerPage;

            GetListCount();

            GetSiteMap();  //사이트맵
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-01-17
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnReject.Text = resMng.GetString("AccessRight3");
                cbtnSearch.Text = resMng.GetString("Search");                

                #region 구분자
                ddlSearchGubun.Items.Clear();
                ddlSearchGubun.Items.Add(new ListItem(this.resMng.GetString("InternalSearch"), "Internal"));
                ddlSearchGubun.Items.Add(new ListItem(this.resMng.GetString("WholeSearch"), "Whole"));
                #endregion

                #region 인사검색필드
                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Name"), "ManyName"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("LoginID"), "LoginID"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Title"), "ManyTitleName"));
                #endregion
               
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-01-17
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {            
            DeptID = GetDecryptParam("deptID", "0").ToNumber<int>();
            SearchGubun = (DeptID == 0) ? "Whole" : "Internal";
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            RecordPerPage = GetDecryptParam("recordPerPage", "10");
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            Order = (Order.Length == 0) ? "CompanyID, TitleCodeOrder, DutyCodeOrder, UserDeptOrder desc" : Order;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-01-17
        /// 작성목적 : 모바일 사용자 리스트 총 갯수
        /// </summary>
        private void GetListCount()
        {
            DAL.List dal = new DAL.List();

            //검색은 전체에서 검색한다.
            string deptID = (SearchGubun == "Whole") ? "0" : DeptID.ToString();

            TotalCount = dal.GetPageListCount(CurrentUserInfo.CompanyID, deptID, SearchField, SearchValue, true).ToString();            
        }

        /// <summary>
        /// 사이트맵
        /// </summary>
        private void GetSiteMap()
        {
            if (DeptID > 0)
            {
                Account.DAL.Dept dal = new Account.DAL.Dept();
                List<Account.BE.DeptInfo> al = dal.GetDeptLists(CurrentUserInfo.CompanyID, DeptID, false);
                foreach (Account.BE.DeptInfo info in al)
                {
                    SiteMap = this.GetLangName(info.NameBase, info.NameKo, info.NameEn,
                            info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);
                }
            }

            if (SiteMap == "") SiteMap = resMng.GetString("MobileUserInfo");
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}