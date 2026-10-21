using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;


namespace Zeospace.DeskPlus.Account.Web.Admin.AuthEmail
{
    public partial class List : Base.BasePageAccountAdmin
    {
        public int DeptID = 0;
        public string UserCnt = "0";

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Admin.AuthEmail.List).Assembly;	//어셈블리 선언
            #endregion


            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }

        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 내용가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2011-04-25
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            ddlSearchTerm.SelectedValue = SearchTerm;
            ddlSearchField.SelectedValue = SearchField;
            ddlListCount.SelectedValue = RecordPerPage;
            ctbSearch.Text = SearchValue;
            cbtnSearch.Text = this.resMng.GetString("Admin_DeptManager_btnSearch");

            GetListCount();
            //GetUserCount();
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {                
                #region 날짜검색
                ddlSearchTerm.Items.Clear();
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("WholeSearch"), ""));
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("oneweek"), "week"));
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("onemonth"), "month"));
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("Period"), "period"));
                #endregion

                #region 인사검색필드
                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("ReceiverName"), "ReceiveUserName"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("ReceiverEmail"), "ReceiveUserEmail"));
                #endregion
                //cbtnCancel.Text = resMng.GetString("Cancel");
            }
        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            this.GetPagingParam();         
            Order = (Order.Length == 0) ? "CompanyID, TitleCodeOrder, DutyCodeOrder, UserDeptOrder desc" : Order;
        }

        /// <summary>
        /// 작성자   : shimpark
        /// 해당 부서 사용자 카운트 가져오기
        /// </summary>
        private void GetListCount()
        {
            DAL.PasswordHistory biz = new DAL.PasswordHistory();

            //검색은 전체에서 검색한다.
            //string deptID = (SearchValue.Length > 0) ? "0" : DeptID.ToString();            
            string[] dateValue = DateHelper.GetSearchTerm(SearchTerm, SearchTermValue, PreLangReal);

            TotalCount = biz.GetPageCnt(CurrentUserInfo.CompanyID, dateValue[0], dateValue[1], SearchField, SearchValue).ToString();

        }
    }
}