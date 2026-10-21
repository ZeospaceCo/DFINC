using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Board.BE;

namespace Zeospace.DeskPlus.Board.Web.Home
{
    public partial class MoreList : Board.Web.Base.BasePageBoard
    {
        #region 변수설정

        public string TotalCount = string.Empty; //총카운트
        public string RecordPerPage = string.Empty; //게시판 리스트 개수
        public string OptionUseDept = "0";//부서별게시판(1)
        public string OptionUseView = "0"; //조회용(1)
        public string OptionListType = "0";
        public string CategoryTitle = ""; //카테고리이름
        public int FavoriteID; //즐겨찾기 아이디
        public string CatTitle; //카테고리명

        public string HomeCategory = ""; //more 값

        #endregion 변수설정

        #region Page Process (페이지 처리)

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Board.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Content.List).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : 윤석원
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            if (SearchTerm == "")
            {
                ddlSearchTerm.SelectedValue = "1Y";
                SearchTerm = "1Y";
            }
            else
            {
                ddlSearchTerm.SelectedValue = SearchTerm;
            }

            ddlSearchGroup.SelectedValue = SearchGroup;
            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;

            RecordPerPage = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.BOARD, "ListCount", "10");//페이지리스트수

            DAL.CategoryUser dal = new DAL.CategoryUser();
            List<BE.CategoryUserInfo> al = dal.GetList(CurrentUserInfo.CompanyID,0,CurrentUserInfo.UserID,"BL");

            foreach (BE.CategoryUserInfo userInfo in al)
            {
                RecordPerPage = userInfo.PageSize.ToString();
            }


            GetListCount();
            CategoryUser();

            ddlListCount.SelectedValue = RecordPerPage;
            cbtnOrder.Visible = false;
            cbtnMove.Visible = false;

            //즐겨찾기 폴더에서 선택된 카테고리(Types:FC)
            if (string.Compare(Types, "FC", true) == 0)
            {
                cbtnFavor.Visible = false;
                cbtnOrder.Visible = true;
                cbtnMove.Visible = true;
                CategoryTitle = GetResource("FavoriteFolder");
            }

            //즐겨찾기 폴더안의 선택된 카테고리(Types:BC)
            if (string.Compare(Types, "BC", true) == 0 && FavoriteID > 0)
            {
                cbtnFavor.Visible = false;
                cbtnOrder.Visible = false;
                cbtnMove.Visible = true;
            }

            //catid 가 없다는 것은, 즐겨찾기 폴더를 의미함.
            cbtnPortlet.Visible = (CatID > 0) ? true : false;
        }

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #region (함수)

        /// <summary>
        /// 작성자 : 윤석원
        /// 카테고리별 사용자 정보
        /// </summary>
        private void CategoryUser()
        {
            DAL.CategoryUser dal = new DAL.CategoryUser();

            //BL : 게시판 TL : 임시저장 IL : 중요표시
            List<CategoryUserInfo> al = dal.GetList(BoaComID, CatID, CurrentUserInfo.UserID, "BL");

            foreach (CategoryUserInfo info in al)
            {
                ddlListCount.SelectedValue = info.PageSize.ToString();
            }
        }

        /// <summary>
        /// 작성자  : 윤석원
        /// 언어 설정하기
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                cbtnAdmin.Text = this.resMng.GetString("Administrator");
                cbtnAdd.Text = this.resMng.GetString("List_Register");
                cbtnSearch.Text = this.resMng.GetString("Search");
                cbtnFavor.Text = this.resMng.GetString("FavoriteFolderAdd");
                cbtnOrder.Text = this.resMng.GetString("FavoriteOrder");
                cbtnMove.Text = this.resMng.GetString("FavoriteFolderMove");
                cbtnPortlet.Text = this.resMng.GetString("AddPortlet");
                cbtnCancel.Text = this.resMng.GetString("List");

                ddlSearchGroup.Items.Clear();
                ddlSearchGroup.Items.Add(new ListItem(this.resMng.GetString("List_Group"), "1"));
                ddlSearchGroup.Items.Add(new ListItem(this.resMng.GetString("List_SubGroup"), "2"));

                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("SearchTitle"), "Title"));                          //제목
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("SearchWriter"), "ManyName"));                      //게시자
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("SearchContents"), "MobileContent"));               //내용
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("SearchTitleContents"), "TitleContents"));          //제목+내용
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("SearchAttachNames"), "FileName"));                 //첨부파일명
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("SearchShortReplyContents"), "ShortReplyContent")); //댓글내용
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("SearchShortReplyWriter"), "ShortReplyManyName"));  //댓글작성자

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
        /// 작성자  : 윤석원
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            FavoriteID = GetDecryptParam("favoriteID", "0").ToNumber<int>();
            HomeCategory = GetDecryptParam("homeCategory", "RecentWrite");
            Order = (Order.Length == 0) ? "OptionTop desc, GroupID DESC, ListOrder ASC, WriteDate desc" : Order;
        }

        /// <summary>
        /// 작성자   : 윤석원
        /// 해당 카테고리 총 카운트
        /// </summary>
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

            DAL.HomeList list = new DAL.HomeList();

            switch (HomeCategory)
            {
                case "RecentWrite":
                    CategoryTitle = this.GetResource("RecentWrite");
                    TotalCount = list.RecentWriteCount(CurrentUserInfo.CompanyID, startDate, endDate, SearchField, SearchValue, CurrentUserInfo.UserID).ToString();
                    break;

                case "MaxShort":
                    CategoryTitle = this.GetResource("MyShort");
                    TotalCount = list.MaxShortCount(CurrentUserInfo.CompanyID, startDate, endDate, SearchField, SearchValue, CurrentUserInfo.UserID).ToString();
                    break;

                case "MaxRead":
                    CategoryTitle = this.GetResource("MaxRead");
                    TotalCount = list.MaxReadCount(CurrentUserInfo.CompanyID, startDate, endDate, SearchField, SearchValue, CurrentUserInfo.UserID).ToString();
                    break;

                case "MaxRate":
                    CategoryTitle = this.GetResource("MaxRate");
                    TotalCount = list.MaxRateCount(CurrentUserInfo.CompanyID, startDate, endDate, SearchField, SearchValue, CurrentUserInfo.UserID).ToString();
                    break;

                case "MyWrite":
                    CategoryTitle = this.GetResource("MyWrite");
                    TotalCount = list.MyWriteCount(CurrentUserInfo.CompanyID, startDate, endDate, SearchField, SearchValue, CurrentUserInfo.UserID).ToString();
                    break;

                case "MyListByShort":
                    CategoryTitle = this.GetResource("MyListByShort");
                    TotalCount = list.MyListByShortCount(CurrentUserInfo.CompanyID, startDate, endDate, SearchField, SearchValue, CurrentUserInfo.UserID).ToString();
                    break;

                case "MyShort":
                    CategoryTitle = this.GetResource("MyShort");
                    TotalCount = list.MyShortCount(CurrentUserInfo.CompanyID, startDate, endDate, SearchField, SearchValue, CurrentUserInfo.UserID).ToString();
                    break;

                case "MyImpo":
                    CategoryTitle = this.GetResource("MyImpo");
                    TotalCount = list.MyImpoCount(CurrentUserInfo.CompanyID, startDate, endDate, SearchField, SearchValue, CurrentUserInfo.UserID).ToString();
                    break;

                default:
                    break;
            }
        }

        #endregion (함수)
    }
}