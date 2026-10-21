using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.ETC.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Text;

namespace Zeospace.DeskPlus.ETC.Web.Pay
{
    public partial class List : Base.BasePageETC
    {
        #region 변수설정
        public int CurrentPage = 0;        
        public string SearchGroup = string.Empty; //분류
        public string SearchField = string.Empty; //검색키워드
        public string SearchValue = string.Empty; //검색어
        public string SearchTerm = string.Empty; //검색기간구분자
        public string SearchTermValue = string.Empty; //검색기간 시작,종료값 
        public string TargetPage = string.Empty; //리스트 경로
        public string TotalCount = string.Empty; //총카운트
        public string RecordPerPage = string.Empty; // 리스트 개수        
        
        public string FormID = string.Empty; //회계문서 양식ID
        public string IsPay = string.Empty; //지급 여부
        public string Types = string.Empty; //지급 여부        
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
            this.ResourceName = "Zeospace.DeskPlus.ETC.Web.Resource.GlobalString.string";	//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.ETC.Web.Pay.List).Assembly; 	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {                
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : 김형석
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            SetLanguage();

            ddlSearchTerm.SelectedValue = SearchTerm;
            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;

            //지급, 미지급을 체크해서 버튼이미지를 보이게 처리
            if (IsPay == "")
            {
                cbtnPayYes.Visible = true;
                cbtnPayNo.Visible = false;
            }
            else
            {
                cbtnPayYes.Visible = false;
                cbtnPayNo.Visible = true;
            }

            #region PageSize 설정하기

            DAL.FolderUser dalPageSize = new DAL.FolderUser();
            List<BE.FolderUserInfo> al = dalPageSize.GetList(CurrentUserInfo.CompanyID,
                "Pay", CurrentUserInfo.UserID, "");

            foreach (BE.FolderUserInfo userInfo in al)
            {
                RecordPerPage = userInfo.PageSize.ToString();
            }

            #endregion

            ddlListCount.SelectedValue = RecordPerPage;                        

            GetListCount();

            TargetPage = Request.Url.AbsolutePath;
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자  : 김형석
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            SearchTerm = GetDecryptParam("searchTerm", "");
            SearchTermValue = GetDecryptParam("searchTermValue", "");
            TargetPage = GetDecryptParam("targetPage", ""); //리스트 경로
            RecordPerPage = GetDecryptParam("recordPerPage", "5");
            
            FormID = GetDecryptParam("formID", "0");
            IsPay = GetDecryptParam("isPay", "");
            Types = GetDecryptParam("types", "");

            //전자결재 문서보기 페이지에서 넘어왔을 경우
            if (Types == "Y") IsPay = "Y";
        }

        /// <summary>
        /// 작성자  : 김형석
        /// 언어 설정하기
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                ltlHeader.Text = (IsPay == "") ? this.resMng.GetString("IsPayN") : this.resMng.GetString("IsPayY");

                cbtnSearch.Text = this.resMng.GetString("Search");
                cbtnPayYes.Text = this.resMng.GetString("IsPayY");
                cbtnPayNo.Text = this.resMng.GetString("IsPayN");

                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Subject"), "Subject")); //제목  
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("ReportUser"), "ManyName")); //상신자
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("TagDic_NO"), "DocNumber")); //문서번호
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("DeptName"), "DeptManyName")); //"부서명"
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("ReportYear"), "Number2")); //기안년도                
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("ContentTEXT"), "ContentTEXT"));   //"본문검색"

                ddlSearchTerm.Items[0].Text = this.resMng.GetString("ListType0");
                ddlSearchTerm.Items[1].Text = this.resMng.GetString("Week");
                ddlSearchTerm.Items[2].Text = this.resMng.GetString("Month");
                ddlSearchTerm.Items[3].Text = this.resMng.GetString("Period");
            }
        }        

        /// <summary>
        /// 작성자   : 김형석
        /// 해당 카테고리 총 카운트
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

            DAL.PayDocument dal = new DAL.PayDocument();
            TotalCount = dal.GetPageListCount(CurrentUserInfo.CompanyID, FormID, IsPay, startDate, endDate, SearchField, SearchValue).ToString();
        }
        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}