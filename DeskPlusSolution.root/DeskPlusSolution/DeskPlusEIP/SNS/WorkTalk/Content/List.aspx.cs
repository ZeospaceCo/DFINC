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

namespace Zeospace.DeskPlus.SNS.Web.WorkTalk.Content
{
    public partial class List : Base.BasePageWorkTalk
    {
        #region 변수설정
        public string MenuID = "2";
        public int CurrentPage = 0;
        public string TotalCount = String.Empty;  //총카운트
        public string RecordPerPage = String.Empty; //리스트 개수
        public string Order = String.Empty; //sort         
        public string SearchField = string.Empty; //검색키워드
        public string SearchValue = string.Empty; //검색어
        public string SearchTerm = string.Empty; //검색기간구분자
        public string SearchTermValue = string.Empty; //검색기간 시작,종룓값
        public string TitleHeader = String.Empty;//사이트맵
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
            this.ResourceName = "Zeospace.DeskPlus.SNS.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.SNS.Web.WorkTalk.Content.List).Assembly;	//어셈블리 선언
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
        /// 작성일자 : 2013-05-10
        /// 작성목적 : Data Load
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;
            ddlSearchTerm.SelectedValue = SearchTerm;   

            string Types = String.Empty;

            if (MenuID == "2")
            {
                Types = "R";
                TitleHeader = GetResource("ReceiveTalk");  //수신톡
            }
            else if (MenuID == "3")
            {
                Types = "S";
                TitleHeader = GetResource("SendTalk");  //발신톡
            }
            else if (MenuID == "4")
            {
                Types = "A";
                TitleHeader = GetResource("ApprovalTalk");  //결재톡
            }
            else
            {
                Types = "R";
                TitleHeader = GetResource("ReceiveTalk");  //수신톡
            }

            #region PageSize 설정하기

            RecordPerPage = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.SNS, "ListCount", "10");//페이지리스트수

            DAL.FolderUser dalUser = new DAL.FolderUser();
            List<BE.FolderUserInfo> al = dalUser.GetList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, Types);

            foreach (BE.FolderUserInfo userInfo in al)
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
        /// 작성자 : famo23
        /// 작성일자 : 2013-05-10
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {   
                CbtnSearch.Text = this.resMng.GetString("Search");

                ddlSearchTerm.Items.Clear();
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("ALL"), ""));
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("Week"), "week"));
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("Month"), "month"));
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("Period"), "period"));       

                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Title"), "Title"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Register"), "ManyName"));

            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-05-10
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            MenuID = GetDecryptParam("menuID", "2");
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
        /// 작성일자 : 2013-05-10
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
 
            DAL.WorkTalk dal = new DAL.WorkTalk();

            if (MenuID == "2")
            {
                //수신톡
                TotalCount = dal.GetRecvPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SearchField, SearchValue, startDate, endDate).ToString();
            }
            else if (MenuID == "3")
            {
                //발신톡
                TotalCount = dal.GetSendPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SearchField, SearchValue, startDate, endDate).ToString();
            }
            else if (MenuID == "4")
            {
                //결재톡
                TotalCount = dal.GetAppPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SearchField, SearchValue, startDate, endDate).ToString();
            }
            else
            {
                TotalCount = dal.GetRecvPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SearchField, SearchValue, startDate, endDate).ToString();
            }
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}