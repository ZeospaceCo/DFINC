using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using System.Data;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Board.Web.Content
{
    public partial class GalleryListInterested : Board.Web.Base.BasePageBoard
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public string TotalCount = string.Empty; //총카운트
        public string RecordPerPage = string.Empty; //게시판 리스트 개수
        public int InterestedUserID = 0;
        #endregion

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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Content.GalleryListInterested).Assembly;	//어셈블리 선언
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
        /// 작성목적   : Loads the data.
        /// 작성자     : 이지원
        /// 최초작성일 : 2017-03-27         
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            if (SearchTerm == "")
            {
                ddlSearchTerm.SelectedValue = "AL";
                SearchTerm = "AL";
            }
            else
            {
                ddlSearchTerm.SelectedValue = SearchTerm;
            }

            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;

            RecordPerPage = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.BOARD, "ListCount", "10");//페이지리스트수

            #region PageSize 설정하기

            DAL.CategoryUser dal = new DAL.CategoryUser();
            List<BE.CategoryUserInfo> al = dal.GetList(CurrentUserInfo.CompanyID,
                0,
                CurrentUserInfo.UserID,
                "IL");

            foreach (BE.CategoryUserInfo userInfo in al)
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
        /// 작성목적   : 언어 설정하기
        /// 작성자     : 이지원
        /// 최초작성일 : 2017-03-27         
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                cbtnInterested.Text = this.resMng.GetString("InterestedPersonAdd");
                cbtnSearch.Text = this.resMng.GetString("Search");

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
        /// 작성목적   : Loads the param.
        /// 작성자     : 이지원
        /// 최초작성일 : 2017-03-27   
        /// </summary>
        private void LoadParam()
        {
            InterestedUserID = GetDecryptParam("interestedUserID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 작성목적   : 해당 카테고리 총 카운트
        /// 작성자     : 이지원
        /// 최초작성일 : 2017-03-27   
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

            DAL.InterestedUser list = new DAL.InterestedUser();
            TotalCount = list.GetPageCount(CurrentUserInfo.CompanyID, CurrentUserInfo.DeptID, CurrentUserInfo.UserID, InterestedUserID, startDate, endDate,
                SearchField, SearchValue).ToString();
        }
        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}