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

namespace Zeospace.DeskPlus.Community.Web.Site.Admin.Member
{
    public partial class ListMod : Base.BasePageCommunitySysop
    {
        #region 변수설정
        public int CurrentPage = 0;
        public int CommunityID = 0;
        public string TotalCount = String.Empty;  //총카운트
        public string RecordPerPage = String.Empty; //리스트 개수
        public string Order = String.Empty; //sort 
        public string SearchGroup = string.Empty; //분류
        public string SearchField = string.Empty; //검색키워드
        public string SearchValue = string.Empty; //검색어
        public string SearchTerm = string.Empty; //검색기간구분자
        public string SearchTermValue = string.Empty; //검색기간 시작,종료값      
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Community.Web.Site.Admin.Member.ListMod).Assembly;	//어셈블리 선언
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
        /// 작성자 : shine
        /// 작성일자 : 2012-05-22
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            GetUserGroupMember();

            ddlGrade.SelectedValue = SearchGroup;
            ddlSearchTerm.SelectedValue = SearchTerm;
            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;

            GetListCount();

            #region PageSize 설정하기

            RecordPerPage = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.COMMUNITY, "ListCount", "10");//페이지리스트수

            DAL.Setting dal = new DAL.Setting();
            List<BE.SettingInfo> al = dal.GetList(CurrentUserInfo.CompanyID, CommunityID, 0, CurrentUserInfo.UserID, "SL");

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
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {  
                cbtnBreak.Text = resMng.GetString("Drop");
                cbtnActive.Text = resMng.GetString("JoinActive2");
                cbtnGrade.Text = resMng.GetString("ChangeGrade");
                cbtnSearch.Text = resMng.GetString("Search");

                ddlSearchField.Items.Clear();
                //ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("CommunityName"), "Name"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("UserName"), "ManyName"));

                ddlSearchTerm.Items[0].Text = this.resMng.GetString("ListType0"); //전체
                ddlSearchTerm.Items[1].Text = this.resMng.GetString("Week");    //1주
                ddlSearchTerm.Items[2].Text = this.resMng.GetString("Month");   //1달
                ddlSearchTerm.Items[3].Text = this.resMng.GetString("Period");  //기간

            }
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            CommunityID = GetDecryptParam("communityID", "0").ToNumber<int>();
            SearchGroup = GetDecryptParam("searchGroup", "");
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            SearchTerm = GetDecryptParam("searchTerm", "");
            SearchTermValue = GetDecryptParam("searchTermValue", "");
            Order = GetDecryptParam("order", "");
            //RecordPerPage = GetDecryptParam("recordPerPage", "10");
        }

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-21
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

            DAL.User dal = new DAL.User();
            TotalCount = dal.GetPageCount(CurrentUserInfo.CompanyID, CommunityID, startDate, endDate, SearchField, SearchValue, SearchGroup).ToString();
        }

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-21
        /// 작성목적 : 회원등급 바인딩
        /// </summary>
        private void GetUserGroupMember()
        {
            ddlSearchGroup.Items.Clear();
            ddlSearchGroup.Items.Add(new ListItem(GetResource("WholeMember"), ""));

            ddlGrade.Items.Clear();
            
            string name = "";

            DAL.UserGroup dal = new DAL.UserGroup();
            DataTableReader dtr = dal.GetUserGroup(CurrentUserInfo.CompanyID, CommunityID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        name = GetLangName(dtr["NameBase"].ToString(),
                            dtr["NameKo"].ToString(),
                            dtr["NameEn"].ToString(),
                            dtr["NameJa"].ToString(),
                            dtr["NameFr"].ToString(),
                            dtr["NameZhcn"].ToString(),
                            dtr["NameZhtw"].ToString(), PreLang).ToReverseXSS();

                        
                        // 커뮤니티 지기는 제외함
                        if (dtr["Grade"].ToString() != "2")
                        {
                            ddlGrade.Items.Add(new ListItem(name, dtr["Grade"].ToString()));
                            ddlSearchGroup.Items.Add(new ListItem(name, dtr["Grade"].ToString()));
                        }
                    }
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