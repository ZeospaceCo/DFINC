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

namespace Zeospace.DeskPlus.EIP.Web.Admin.Static.LogIn
{
    public partial class List : Base.BasePageEIP
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public string Order = string.Empty;       
        public string SearchTerm = string.Empty;  
        public string SearchTermValue = string.Empty;   
        public string TotalCount = string.Empty; 
        public string RecordPerPage = string.Empty; 
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.ResourceName = "Zeospace.DeskPlus.EIP.Web.Resource.GlobalString.string";			
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Admin.Static.LogIn.List).Assembly;	

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
            RecordPerPage = "10";
            ddlListCount.SelectedValue = RecordPerPage;

            startDate.Value = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString().PadLeft(2, '0') + "-" + System.DateTime.Now.Day.ToString().PadLeft(2, '0');
            endDate.Value = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString().PadLeft(2, '0') + "-" + System.DateTime.Now.Day.ToString().PadLeft(2, '0');
            SearchTermValue = startDate.Value + "," + endDate.Value;

            GetListCount();
        }

        /// <summary>
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                cbtnExcel.Text = this.resMng.GetString("ExcelDown"); 
                cbtnSearch.Text = this.resMng.GetString("Search");   

                /*
                ddlSearchTerm.Items.Clear();
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("SearchFieldTotal"), ""));   // 전체
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("Week"), "week"));           // 1주
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("Month"), "month"));         // 1달
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("Period"), "period"));       // 기간
                */

                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("UserName"), "ManyName"));          // 이름
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("IPAddress"), "IPAddress"));                // IP
                //ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("StartDate"), "StartTimeYMD"));     // 시작일
                //ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("EndDate"), "EndTimeYMD"));         // 종료일
            }
        }

        /// <summary>
        /// </summary>
        private void LoadParam()
        {
            Order = (Order.Length == 0) ? "AccessLogID DESC " : Order;
        }

        
        /// </summary>        /// <summary>
        private void GetListCount()
        {
            string startDate = DateHelper.GetCultureDate(SearchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
            string endDate = DateHelper.GetCultureDate(SearchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
            //string getDate = "";

            /*
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
            */

            Zeospace.DeskPlus.Common.DAL.AccessLog log = new DeskPlus.Common.DAL.AccessLog();
            TotalCount = log.GetPageListCount(CurrentUserInfo.CompanyID, startDate, endDate, ddlSearchField.SelectedValue, ctbSearch.Text).ToString();
        }
        #endregion

        #region (함수)
        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}