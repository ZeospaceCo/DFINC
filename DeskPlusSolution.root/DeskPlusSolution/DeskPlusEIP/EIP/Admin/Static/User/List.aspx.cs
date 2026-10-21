using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Helper = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.EIP.Web.Admin.Static.User
{
    public partial class List : Base.BasePageEIPAdmin
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public string Order = string.Empty;       // sort 
        public string SearchTerm = string.Empty;  //검색기간구분자
        public string SearchTermValue = string.Empty;   //검색기간 시작,종료값 
        public string TotalCount = string.Empty; //총카운트
        public string RecordPerPage = string.Empty; //게시판 리스트 개수
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.EIP.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Admin.Static.User.List).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        private void LoadData()
        {
            LoadParam();

            RecordPerPage = "10";//GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.COMMON, "ListCount", "10");//페이지리스트수

            ddlListCount.SelectedValue = RecordPerPage;

            GetListCount();
        }

        /// <summary>
        /// 작성자  : 
        /// 언어 설정하기
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                cbtnExcel.Text = this.resMng.GetString("ExcelDown"); // "엑셀다운로드";
                cbtnSearch.Text = this.resMng.GetString("Search");   // "검색";

                ddlSearchTerm.Items.Clear();
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("SearchFieldTotal"), ""));   // 전체
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("Week"), "week"));           // 1주
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("Month"), "month"));         // 1달
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("Period"), "period"));       // 기간

                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("DeptName"), "ManyDept"));          // 부서명
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("TitleName"), "ManyTitle"));        // 직급명 
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("DutyName"), "ManyDuty"));          // 직책명 
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("UserName"), "ManyName"));          // 이름 
                
            }
        }

        /// <summary>
        /// 작성자  : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : Loads the param.
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        private void LoadParam()
        {
            Order = (Order.Length == 0) ? "WriteDate DESC " : Order;
        }

        /// </summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 :해당 카테고리 총 카운트
        /// 수정일자 :
        /// 수정내용 :
        /// <summary>
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

            Zeospace.DeskPlus.Account.DAL.UserDept_History history = new Account.DAL.UserDept_History();
            TotalCount = history.GetPageListCount(CurrentUserInfo.CompanyID, startDate, endDate, ddlSearchField.SelectedValue, ctbSearch.Text).ToString();
        }
        #endregion
    }
}