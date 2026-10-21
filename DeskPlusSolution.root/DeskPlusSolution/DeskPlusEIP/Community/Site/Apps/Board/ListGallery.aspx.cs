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
using Zeospace.DeskPlus.Community.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Community.Web.Site.Apps.Board
{
    public partial class ListGallery : Base.BasePageCommunity
    {
        #region 변수설정
        public int CurrentPage = 0;
        public int CommunityID = 0;
        public int MenuID = 0;
        public string TotalCount = String.Empty;  //총카운트
        public string RecordPerPage = String.Empty; //리스트 개수
        public string Order = String.Empty; //sort 
        public string SearchField = string.Empty; //검색키워드
        public string SearchValue = string.Empty; //검색어
        public string SearchTerm = string.Empty; //검색기간구분자
        public string SearchTermValue = string.Empty; //검색기간 시작,종료값   
        public string MenuName = string.Empty; // 메뉴 이름
        public int Grade = 0;
        public string ListType = String.Empty;
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
            this.ResourceName = "Zeospace.DeskPlus.Community.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Community.Web.Site.Apps.Board.ListGallery).Assembly;	//어셈블리 선언
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
        /// 작성일자 : 2012-06-11
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            if (CommunityID > 0 && MenuID > 0)
            {
                GetMenuName();
            }
            else
            {
                MenuName = GetResource("NoTitle");
            }

            #region 권한 체크

            if (Grade != 2)
            {
                DAL.MenuAuth dal = new DAL.MenuAuth();
                DataTableReader dtr = dal.GetMenuAuth(CurrentUserInfo.CompanyID, CommunityID, MenuID);

                int WriteAuth = 9;
                int ReadAuth = 9;

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            WriteAuth = dtr["WriteAuth"].ToString().ToNumber<int>();
                            ReadAuth = dtr["ReadAuth"].ToString().ToNumber<int>();
                        }
                    }
                }

                if (Grade <= ReadAuth)
                {
                    if (Grade <= WriteAuth)
                    {
                        cbtnWrite.Visible = true;
                    }
                    else
                    {
                        cbtnWrite.Visible = false;
                    }
                }
                else
                {
                    Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
                    return;
                }
            }

            #endregion 

            ddlSearchTerm.SelectedValue = SearchTerm;
            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;

            GetListCount();

            #region PageSize 설정하기

            RecordPerPage = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.COMMUNITY, "ListCount", "10");//페이지리스트수

            DAL.Setting dalSet = new DAL.Setting();
            List<BE.SettingInfo> al = dalSet.GetList(CurrentUserInfo.CompanyID, CommunityID, MenuID, CurrentUserInfo.UserID, "PL");

            foreach (BE.SettingInfo userInfo in al)
            {
                RecordPerPage = userInfo.PageSize.ToString();
            }

            ddlListCount.SelectedValue = RecordPerPage;

            #endregion
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-11
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnWrite.Text = resMng.GetString("Write");
                cbtnSearch.Text = resMng.GetString("Search");

                ddlSearchField.Items.Clear();

                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Title"), "Title"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Registrant"), "ManyName"));
  
                ddlSearchTerm.Items[0].Text = this.resMng.GetString("ListType0"); //전체
                ddlSearchTerm.Items[1].Text = this.resMng.GetString("Week");    //1주
                ddlSearchTerm.Items[2].Text = this.resMng.GetString("Month");   //1달
                ddlSearchTerm.Items[3].Text = this.resMng.GetString("Period");  //기간

            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-11
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            CommunityID = GetDecryptParam("communityID", "0").ToNumber<int>();
            MenuID = GetDecryptParam("menuID", "0").ToNumber<int>();
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            SearchTerm = GetDecryptParam("searchTerm", "");
            SearchTermValue = GetDecryptParam("searchTermValue", "");
            Order = GetDecryptParam("order", "");
            //RecordPerPage = GetDecryptParam("recordPerPage", "10");
            Grade = GetDecryptParam("grade", "10").ToNumber<int>();
            ListType = GetDecryptParam("listType", "");
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-11
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

            DAL.Boa dal = new DAL.Boa();
            TotalCount = dal.GetListCount(CurrentUserInfo.CompanyID, CommunityID, MenuID, startDate, endDate, SearchField, SearchValue).ToString();
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-11
        /// 작성목적 : 메뉴 이름 가져오기(공지사항, 게시판, 자료실..)
        /// </summary>
        private void GetMenuName()
        {
            DAL.Menu dal = new DAL.Menu();
            DataSet ds = dal.GetMenuName(CurrentUserInfo.CompanyID, CommunityID, MenuID);

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    MenuName = GetLangName(dr["NameBase"].ToString(),
                        dr["NameKo"].ToString(),
                        dr["NameEn"].ToString(),
                        dr["NameJa"].ToString(),
                        dr["NameFr"].ToString(),
                        dr["NameZhcn"].ToString(),
                        dr["NameZhtw"].ToString(), PreLang).ToReverseXSS();
                }
            }
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}