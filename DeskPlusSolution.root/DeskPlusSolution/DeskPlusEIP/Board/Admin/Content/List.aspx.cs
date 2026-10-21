using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Board.BE;

namespace Zeospace.DeskPlus.Board.Web.Admin.Content
{
    public partial class List : Base.BasePageBoardAdmin
    {
        #region 변수설정

        public string TotalCount = string.Empty; //총카운트
        public string RecordPerPage = string.Empty; //게시판 리스트 개수
        public string OptionUseDept = "0";//부서별게시판(1)
        public string OptionUseView = "0"; //조회용(1)
        public string OptionListType = "0";
        public string CategoryTitle = ""; //카테고리이름
        private string MyAuthCatID = "";

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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Admin.Content.List).Assembly;	//어셈블리 선언

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
        /// 작성자 : 심재운
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            //통합게시판 회사아이디
            BoaComID = BLL.Category.GetCompanyCategoryInfo(CatID);

            if (SearchTerm == "")
            {
                ddlSearchTerm.SelectedValue = "AL";
                SearchTerm = "AL";
            }
            else
            {
                ddlSearchTerm.SelectedValue = SearchTerm;
            }

            ddlSearchGroup.SelectedValue = SearchGroup;
            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;

            RecordPerPage = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.BOARD, "ListCount", "10");//페이지리스트수

            //카테고리값이 0 이면 처리 하지 않는다.
            if (CatID <= 0)
            {
                CategoryTitle = GetResource("NoTitle");
                cbtnAdd.Visible = false;
            }
            else
            {
                #region PageSize 설정하기

                DAL.CategoryUser dal = new DAL.CategoryUser();
                List<BE.CategoryUserInfo> al = dal.GetList(BoaComID,
                    CatID,
                    CurrentUserInfo.UserID,
                    "BL");

                foreach (BE.CategoryUserInfo userInfo in al)
                {
                    RecordPerPage = userInfo.PageSize.ToString();
                }

                #endregion PageSize 설정하기

                GetCategory();
                GetListCount();
                CategoryUser();
            }

            ddlListCount.SelectedValue = RecordPerPage;
        }

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #region (함수)

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 카테고리 옵션 적용
        /// 작성자     : 심재운
        /// 최초작성일 : 2011-05-09
        /// </summary>
        private void GetCategory()
        {
            DAL.Category dal = new DAL.Category();
            DataSet ds = dal.GetCategory(BoaComID, CatID);

            BLL.Category biz = new BLL.Category();
            //CategoryTitle = biz.GetPathName(ds, PreLang); //sitemap 경로 반환

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    OptionUseView = dr["OptionUseView"].ToString();
                    //관리자는 부서별 게시판을 전체 보기 위함이므로 "0" 처리한다.
                    //OptionUseDept = dr["OptionUseDept"].ToString();
                    OptionListType = dr["OptionListType"].ToString();

                    CategoryTitle = this.GetLangName(
                                        dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                                        dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                                        dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(),
                                        dr["NameZhtw"].ToString().ToNull(), PreLang);
                }
            }

            #region 내가 볼 수 있는 하위 카테고리

            NameValueCollection nvc = new NameValueCollection();
            bool isChild = (SearchGroup == "2") ? true : false;

            MyAuthCatID = CatID.ToString();

            if (CatID > 0)
            {
                //보기용 및 하위분류 검색시, 하위카테고리까지 가져옴
                if (OptionUseView == "1" || isChild) MyAuthCatID = biz.GetChildCategory(BoaComID, CatID);
                if (OptionUseView == "1") cbtnAdd.Visible = false;
            }
            else
            {
                MyAuthCatID = "-1"; //0 값이 넘어오면 리스트 안나오게 함.
            }

            #endregion 내가 볼 수 있는 하위 카테고리
        }

        /// <summary>
        /// 작성자 : 심재운
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
        /// 작성자  : 심재운
        /// 언어 설정하기
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                cbtnAdd.Text = this.resMng.GetString("List_Register");
                cbtnSearch.Text = this.resMng.GetString("Search");
                cbtnDel.Text = this.resMng.GetString("Del");
                cbtnMove.Text = this.resMng.GetString("ListMove");

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
        /// 작성자  : 심재운
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            Order = (Order.Length == 0) ? "OptionTop desc, GroupID DESC, ListOrder ASC, WriteDate desc" : Order;
        }

        /// <summary>
        /// 작성자   : shimpark
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

            DAL.Lists list = new DAL.Lists();
            TotalCount = list.GetPageListCount(BoaComID, MyAuthCatID,
                startDate, endDate,
                SearchField, SearchValue,
                OptionUseDept, CurrentUserInfo.DeptID).ToString();
        }

        #endregion (함수)

        #region Property (프로퍼티)

        #endregion Property (프로퍼티)
    }
}