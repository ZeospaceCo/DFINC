using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Board.BE;

namespace Zeospace.DeskPlus.Board.Web.Content
{
    public partial class GalleryList : Board.Web.Base.BasePageBoard
    {
        #region 변수설정

        public string TotalCount = string.Empty; //총카운트
        public string RecordPerPage = string.Empty; //게시판 리스트 개수
        public string OptionUseDept = "0";//부서별게시판(1)
        public string OptionUseView = "0"; //조회용(1)
        public string OptionListType = "0";
        public string CategoryTitle = ""; //카테고리이름
        private string MyAuthCatID = "";//내가 볼 수 있는 카테고리
        public int FavoriteID; //즐겨찾기 아이디
        public string CatTitle; //카테고리명
        public string Kind; //홈구분

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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Content.GalleryList).Assembly;	//어셈블리 선언

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

            #region admin 관리자 체크

            cbtnAdmin.Visible = (AccessAuth) ? true : false;

            #endregion admin 관리자 체크

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
                cbtnFavor.Visible = false;
            }
            else
            {
                bool isAccess = GetMyAuthCategory();

                if (isAccess == false)
                {
                    Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
                    return;
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
            }

            //모아보기에서 접속시 
            if (PGubun == "Y")
            {
                cbtnAdmin.Visible = false;
                cbtnMove.Visible = false;
                cbtnOrder.Visible = false;
                cbtnFavor.Visible = false;
                cbtnPortlet.Visible = false;
            }
        }

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #region (함수)

        /// <summary>
        /// 내가 접근 가능한 카테고리 인지 확인하기
        /// 작성자 : 심재운
        /// </summary>
        private bool GetMyAuthCategory()
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
                    OptionUseDept = dr["OptionUseDept"].ToString();
                    OptionListType = dr["OptionListType"].ToString();
                    CatTitle = this.GetLangName(
                                        dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                                        dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                                        dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(),
                                        dr["NameZhtw"].ToString().ToNull(), PreLang);
                }
            }

            CategoryTitle = CatTitle;

            #region 내가 볼 수 있는 하위 카테고리

            bool isChild = (SearchGroup == "2") ? true : false;//하위검색
            isChild = (OptionUseView == "1") ? true : isChild; //보기용이면 하위검색가능
            NameValueCollection nvc = new NameValueCollection();

            if (CatID > 0)
            {
                //보기용은 하위카테고리까지 가져옴
                if (OptionUseView == "1")
                {
                    nvc = biz.GetMyAuthCategory(BoaComID, CurrentUserInfo.UserID,
                        CurrentUserInfo.DeptID, CatID, OptionUseDept, true, BE.BoardAuth.Read, true, false);

                    foreach (string s in nvc.AllKeys) MyAuthCatID += s + ",";
                }
                else
                {
                    nvc = biz.GetMyAuthCategory(BoaComID, CurrentUserInfo.UserID,
                        CurrentUserInfo.DeptID, CatID, OptionUseDept, isChild, BE.BoardAuth.Read, true, false);

                    foreach (string s in nvc.AllKeys) MyAuthCatID += s + ",";
                }

                MyAuthCatID = MyAuthCatID.TrimEnd(',');
            }
            else
            {
                MyAuthCatID = "-1"; //카테고리 값이 0 넘어오면 -1 로 해서 리스트 안나오게 함
            }

            //권한 없음 페이지로 이동한다.
            if (MyAuthCatID == "-1" || MyAuthCatID.Length == 0)
            {
                return false;
            }
            else
            {
                #region 추가버튼 활성화 여부

                cbtnAdd.Visible = false;
                if (OptionUseView == "0")
                {
                    //부서별게시판은 권한없이 쓰기 가능
                    cbtnAdd.Visible = (OptionUseDept == "1") ? true : false;

                    //2 : 쓰기 , 4 : 쓰기 + 댓글쓰기
                    nvc = biz.GetMyAuthCategory(BoaComID, CurrentUserInfo.UserID,
                        CurrentUserInfo.DeptID, CatID, OptionUseDept, false, BE.BoardAuth.Write, true, false);

                    foreach (string s in nvc.AllKeys) cbtnAdd.Visible = true;
                }

                #endregion 추가버튼 활성화 여부
            }

            return true;

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
                cbtnAdmin.Text = this.resMng.GetString("Administrator");
                cbtnAdd.Text = this.resMng.GetString("List_Register");
                cbtnSearch.Text = this.resMng.GetString("Search");
                cbtnFavor.Text = this.resMng.GetString("FavoriteFolderAdd");
                cbtnOrder.Text = this.resMng.GetString("FavoriteOrder");
                cbtnMove.Text = this.resMng.GetString("FavoriteFolderMove");
                cbtnPortlet.Text = this.resMng.GetString("AddPortlet");
                cbtnInfo.Text = this.resMng.GetString("Authority");
                cbtnInterested.Text = this.resMng.GetString("InterestedPersonAdd");

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
            Kind = GetDecryptParam("kind", "");
            FavoriteID = GetDecryptParam("favoriteID", "0").ToNumber<int>();
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
    }
}