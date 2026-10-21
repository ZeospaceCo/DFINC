using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Collections.Specialized;

namespace Zeospace.DeskPlus.Pim.Web.ToDo
{
    public partial class ShortReplyList : Base.BasePagePim
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정

        public int ToDoID = 0;
        public int TotalCount = 0; //총카운트
        public string RecordPerPage; //게시판 리스트 개수
        public string Order = string.Empty; //sort
        public string SearchField = string.Empty; //검색키워드
        public string SearchValue = string.Empty; //검색어
        public string SearchTerm = string.Empty; //검색기간구분자
        public string SearchTermValue = string.Empty; //검색기간 시작,종룓값   
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.ToDo.ShortReplyList).Assembly;	//어셈블리 선언
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

        private void LoadData()
        {
            ddlListCount.SelectedValue = RecordPerPage;

            ddlSearchTerm.SelectedValue = SearchTerm;
            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;

            GetListCount();
        }

        #endregion

        #region 함수

        /// <summary>
        /// 다국어설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnClose.Text = resMng.GetString("Close");
                cbtnShortReply.Text = resMng.GetString("View_AddShortReply");
                cbtnSearch.Text = resMng.GetString("SEARCHTEXT");

                ddlSearchTerm.Items[0].Text = this.resMng.GetString("ALL");
                ddlSearchTerm.Items[1].Text = this.resMng.GetString("oneweek");
                ddlSearchTerm.Items[2].Text = this.resMng.GetString("ONEMON");
                ddlSearchTerm.Items[3].Text = this.resMng.GetString("Period");

                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Contents"), "Content"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Name"), "ManyName"));
                
            }
        }

        private void LoadParam()
        {
            Order = (Order.Length == 0) ? "List.CompanyID, List.ReplyID desc " : Order;
            ToDoID = GetDecryptParam("toDoID", "0").ToNumber<int>();
            RecordPerPage = GetDecryptParam("recordPerPage", "5");
            if (RecordPerPage.Length == 0 || RecordPerPage == "0") RecordPerPage = "5";
        }

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

            DAL.ToDoReply biz = new DAL.ToDoReply();
            TotalCount = biz.GetPageListCount(CurrentUserInfo.CompanyID, ToDoID,
                startDate, endDate, SearchField, SearchValue, CurrentUserInfo.UserID);

        }

        #endregion
    }
}