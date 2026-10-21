using System;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Report.Web.Content
{
    public partial class AdminList : Base.BasePageReport
    {
        #region 변수설정

        public string TotalCount = string.Empty; //총카운트
        public string RecordPerPage = string.Empty; //게시판 리스트 개수
        public int CurrentPage;
        private string YearMonth = string.Empty;

        #endregion 변수설정

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Report.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Report.Web.Content.AdminList).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성목적   : 초기화 데이터 설정
        /// 작성자     :
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;

            //ddlYear.SelectedValue = Years.ToString();
            //ddlMonth.SelectedValue = Months.ToString();

            YearMonth = Years.ToString() + Months.ToString();
            //RecordPerPage = "10";
            ddlListCount.SelectedValue = RecordPerPage;
            GetListCount();
        }

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #region (함수)

        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     :
        /// </summary>
        private void LoadParam()
        {
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            RecordPerPage = GetDecryptParam("recordPerPage", "10");
        }

        /// <summary>
        /// 작성목적   : 다국어 설정
        /// 작성자     :
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnSearch.Text = resMng.GetString("Search");

                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(resMng.GetString("Writer"), "ManyName"));
                ddlSearchField.Items.Add(new ListItem(resMng.GetString("ThisWork"), "NowJob"));
                ddlSearchField.Items.Add(new ListItem(resMng.GetString("NextWork"), "NextJob"));
                ddlSearchField.Items.Add(new ListItem(resMng.GetString("IssueWork"), "Issue"));

                //ddlWeek.Items.Clear();
                //ddlWeek.Items.Add(new ListItem(resMng.GetString("All"), "0"));
                //ddlWeek.Items.Add(new ListItem("1" + resMng.GetString("Week"), "1"));
                //ddlWeek.Items.Add(new ListItem("2" + resMng.GetString("Week"), "2"));
                //ddlWeek.Items.Add(new ListItem("3" + resMng.GetString("Week"), "3"));
                //ddlWeek.Items.Add(new ListItem("4" + resMng.GetString("Week"), "4"));
                //ddlWeek.Items.Add(new ListItem("5" + resMng.GetString("Week"), "5"));

                ddlFolder.Items.Clear();
                ddlFolder.Items.Add(new ListItem(resMng.GetString("Dept"), "Dept"));
                ddlFolder.Items.Add(new ListItem(resMng.GetString("User"), "User"));

                cbtnBeforeDept.Text = resMng.GetString("BeforeDept");
                cbtnBeforeDeptGarbage.Text = resMng.GetString("BeforeDeptGarbage");
                cbtnAfterDept.Text = resMng.GetString("AfterDept");

                cbtnBeforeUser.Text = resMng.GetString("BeforeUser");
                cbtnAfterUser.Text = resMng.GetString("AfterUser");

                cbtnTransfer.Text = resMng.GetString("Transfer");
                cbtnTransferAll.Text = resMng.GetString("TransferAll");
                cbtnCopy.Text = resMng.GetString("Copy");
                cbtnCopyAll.Text = resMng.GetString("CopyAll");

                cbtnDel.Text = resMng.GetString("Del");
            }
        }

        /// <summary>
        /// 작성자   : shimpark
        /// 해당 카테고리 총 카운트
        /// </summary>
        private void GetListCount()
        {
            DAL.List list = new DAL.List();
            TotalCount = list.GetPageListCount(CurrentUserInfo.CompanyID, "", "", 0, SearchField, SearchValue, 0).ToString();
        }

        #endregion (함수)
    }
}