using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.Web.Admin.Statistics
{
    public partial class ListMonth : Base.BasePageApprovalAdmin
    {
        #region 변수설정
        public int TotalItemCount;      //총카운트
        public int SelYears;
        public string TabMenu = "";    // 양식별(Form), 부서별(Dept), 개인별(User)
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Admin.Statistics.ListMonth).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadData();
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            SetLanguage();

            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;
            ddlListCount.SelectedValue = this.RecordPerPage.ToString();

            GetListCount();
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
                //Button
                cbtnSearch.Text = this.resMng.GetString("OrganBtnSearch"); //"검색";
                cbtnExcel.Text = this.resMng.GetString("StatExcel");
                cbtnGraph.Text = this.resMng.GetString("StatGraph");

                //DropDown Item
                this.ddlSearchField.Items.Clear();
                this.ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("FormName2"), "FormName"));     //FormName2
                this.ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("DeptName"), "DeptName"));     //DeptName
                this.ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("UserName"), "UserName"));    //UserName
            }
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            BaseParam();

            SelYears = GetDecryptParam("SelYears", DateTime.Today.Year.ToString()).ToNumber<int>();
            TabMenu = GetDecryptParam("TabMenu", "Form");
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 게시물 총 갯수
        /// </summary>
        private void GetListCount()
        {
            int years = ddlYear.SelectedValue.ToNumber<int>();
            string sDate = years.ToString() + "-01-01";         // 2012-01-01
            string eDate = (years + 1).ToString() + "-01-01";   // 2013-01-01
            string srhField = ddlSearchField.SelectedValue;
            string srhValue = ctbSearch.Text;

            DAL.Statistics dal = new DAL.Statistics();

            switch (TabMenu.ToUpper()) {
                case "Form":
                    TotalItemCount = dal.GetMonthFormListCnt(CurrentUserInfo.CompanyID, sDate, eDate, SearchField, SearchValue);
                    break;
                case "Dept":
                    TotalItemCount = dal.GetMonthDeptListCnt(CurrentUserInfo.CompanyID, sDate, eDate, SearchField, SearchValue);
                    break;
                case "User":
                    TotalItemCount = dal.GetMonthUserListCnt(CurrentUserInfo.CompanyID, sDate, eDate, SearchField, SearchValue);
                    break;
                default:
                    TotalItemCount = dal.GetMonthFormListCnt(CurrentUserInfo.CompanyID, sDate, eDate, SearchField, SearchValue);
                    break;
            }
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}