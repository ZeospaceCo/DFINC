using System;
using System.Web.UI.WebControls;

namespace Zeospace.DeskPlus.Report.Web.Content
{
    public partial class DeptList : Base.BasePageReport
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Report.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Report.Web.Content.DeptList).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        /// <summary>
        /// 작성목적   : 초기화 데이터 설정
        /// 작성자     :
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;

            ddlYear.SelectedValue = Years.ToString();
            ddlMonth.SelectedValue = Months.ToString();
        }

        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     :
        /// </summary>
        private void LoadParam()
        {
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
                ddlSearchField.Items.Add(new ListItem(resMng.GetString("ThisWork"), "NowJob"));
                ddlSearchField.Items.Add(new ListItem(resMng.GetString("NextWork"), "NextJob"));
                ddlSearchField.Items.Add(new ListItem(resMng.GetString("IssueWork"), "Issue"));

                ddlWeek.Items.Clear();
                ddlWeek.Items.Add(new ListItem(resMng.GetString("All"), "0"));
                ddlWeek.Items.Add(new ListItem("1" + resMng.GetString("Week"), "1"));
                ddlWeek.Items.Add(new ListItem("2" + resMng.GetString("Week"), "2"));
                ddlWeek.Items.Add(new ListItem("3" + resMng.GetString("Week"), "3"));
                ddlWeek.Items.Add(new ListItem("4" + resMng.GetString("Week"), "4"));
                ddlWeek.Items.Add(new ListItem("5" + resMng.GetString("Week"), "5"));
            }
        }
    }
}