using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;

namespace Zeospace.DeskPlus.Address.Web.Admin.Content
{
    public partial class ListBlog : Base.BasePageAddressAdmin
    {
        #region 변수설정
        public string CategoryID;
        public string Types;
        public int CurrentPage = 0;
        public string TotalCount = string.Empty; //총카운트
        public string RecordPerPage = "6"; //게시판 리스트 개수        
        public string SearchKey = string.Empty; //검색키워드
        public string SearchValue = string.Empty; //검색어
        public string SearchFilter = string.Empty; //ㄱㄴㄷㄹ
        public string Order = string.Empty; //sort 
        public string SiteMap = string.Empty;//사이트맵

        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Address.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Address.Web.Admin.Content.ListBlog).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자  : 심재운
        /// 초기화
        /// </summary>
        private void LoadData()
        {
            #region PageSize 설정하기
            RecordPerPage = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.ADDRESS, "ListCount", "10");//페이지리스트수  
            ctbSearch.Text = SearchValue; //검색어

            DAL.CategoryUser dal = new DAL.CategoryUser();
            BE.CategoryUserInfo info = new BE.CategoryUserInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.CategoryID = CategoryID.ToNumber<int>();
            info.UserID = CurrentUserInfo.UserID;
            info.Types = Types;

            List<BE.CategoryUserInfo> al = dal.GetCategoryUserLists(info);

            foreach (BE.CategoryUserInfo userInfo in al)
            {
                RecordPerPage = userInfo.PageSize.ToString();
            }

            ddlListCount.SelectedValue = RecordPerPage;
            #endregion

            GetListCount();
            GetSiteMap(); //사이트맵

            cbtnSimpleAdd.Visible = false; //간편추가
            cbtnAdd.Visible = false;//추가
            cbtnCopy.Visible = false;//복사
            cbtnMove.Visible = false;//이동
            cbtnDel.Visible = false;//삭제

            if (Types == "GG") //전달받은 주소록
            {
                cbtnCopy.Visible = true;//복사
            }
            else if (Types == "CG" && CategoryID == "0")
            {
                cbtnMove.Visible = true;
                cbtnDel.Visible = true;
                cbtnCopy.Visible = true;//복사
            }
            else if (Types == "CG" && CategoryID != "0")
            {
                cbtnSimpleAdd.Visible = true;
                cbtnAdd.Visible = true;
                cbtnMove.Visible = true;
                cbtnDel.Visible = true;
                cbtnCopy.Visible = true;//복사
            }
            else if (Types == "RG") //전달받은 주소록
            {
                cbtnCopy.Visible = true;//복사
                cbtnMove.Visible = true;
                cbtnDel.Visible = true;
            }            
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            CategoryID = GetDecryptParam("categoryID", "0");
            Types = GetDecryptParam("types", "");
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            SearchKey = GetDecryptParam("searchKey", "");
            SearchValue = GetDecryptParam("searchValue", "");
            SearchFilter = GetDecryptParam("searchFilter", "");
            Order = GetDecryptParam("order", "");
            Order = (Order.Length == 0) ? "Orders ASC" : Order;
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                //CbtnSimpleAdd.Text = resMng.GetString("Cancel");
                cbtnSimpleAdd.Text = resMng.GetString("AddSimple");         //"간편추가";
                cbtnAdd.Text = resMng.GetString("AddDetail");               //"상세추가";
                cbtnCopy.Text = resMng.GetString("Copy");                   //"복사";
                cbtnMove.Text = resMng.GetString("Move");                   //"이동";
                cbtnDel.Text = resMng.GetString("Delete");                  // "삭제";                                               
                cbtnSearch.Text = resMng.GetString("Search");               // "검색" 

                ddlSearchField.Items.Add(new ListItem(resMng.GetString("FirstName"), "ManyName"));
                ddlSearchField.Items.Add(new ListItem(resMng.GetString("Email"), "Mail"));
                ddlSearchField.Items.Add(new ListItem(resMng.GetString("DeptName"), "DeptManyName"));
                ddlSearchField.Items.Add(new ListItem(resMng.GetString("CellPhone"), "MobilePhone"));

                ddlSearchField.SelectedValue = SearchKey; //검색필드
            }
        }

        /// <summary>
        /// 작성자   : shimpark
        /// 해당 카테고리 총 카운트
        /// </summary>
        private void GetListCount()
        {
            string typeMember = "";
            int userID = 0;

            switch (Types)
            {
                case "GG":
                    typeMember = "GM";
                    break;
                case "CG":
                    typeMember = "CM";
                    break;
                case "PG":
                    typeMember = "PM";
                    userID = CurrentUserInfo.UserID;
                    break;
                case "RG":
                    typeMember = "RM";
                    userID = CurrentUserInfo.UserID;
                    break;
                case "TG":
                    typeMember = "TM";
                    break;
                case "DG":
                    typeMember = "DM";
                    break;
                case "FG":
                    typeMember = "FM"; //가짜 (데이터 안나올거임)
                    userID = CurrentUserInfo.UserID;
                    break;
            }

            BLL.List biz = new BLL.List();
            string catSubList = biz.GetSubCatList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CategoryID, Types);


            Address.DAL.Private pBiz = new DAL.Private();
            TotalCount = pBiz.GetPageCount(CurrentUserInfo.CompanyID, catSubList, userID, typeMember, SearchKey, SearchValue, FilterString()).ToString();
        }





        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 필터링 쿼리 구문
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-02-02
        /// </summary>
        /// <returns></returns>
        private string FilterString()
        {
            return BLL.List.FilterString(SearchFilter);
        }



        /// <summary>
        /// 작성자 : shimpark
        /// 용도 : 사이트맵 보이기
        /// </summary>
        private void GetSiteMap()
        {
            BLL.List biz = new BLL.List();

            SiteMap = biz.GetSitemapName(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CategoryID.ToNumber<int>(),
                Types, PreLang);

            if (SiteMap == "")
            {
                switch (Types)
                {
                    case "GG":
                        SiteMap = GetResource("CompanyAddress");
                        break;
                    case "CG":
                        SiteMap = GetResource("CommonAddress");
                        break;
                    case "PG":
                        SiteMap = GetResource("PrivateAddress");
                        break;
                    case "RG":
                        SiteMap = GetResource("ForwardAddress");
                        break;
                    case "TG":
                        SiteMap = GetResource("TitleAddress");
                        break;
                    case "DG":
                        SiteMap = GetResource("TitleAddress");
                        break;
                    case "FG":
                        SiteMap = GetResource("Favorite");
                        break;
                }
            }

        }

        #endregion

        #region (속성)

        #endregion

        #region (이벤트핸들러)

        #endregion
    }
}