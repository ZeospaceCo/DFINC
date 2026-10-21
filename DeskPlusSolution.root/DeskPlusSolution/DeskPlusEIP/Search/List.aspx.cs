using System;
using System.Text;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Search.Web
{
    public partial class List : Base.BasePageSearch
    {
        #region 변수설정

        public string SearchValue = string.Empty;
        public string MenuList = string.Empty;
        public string MailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "");

        #endregion 변수설정

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Search.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Search.Web.List).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

        #region Method (함수)

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 내용가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2011-04-25
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            SetLanguage();
            LoadMenu();

            ctbSearch.Text = SearchValue;
        }

        /// <summary>
        /// 작성목적   : 파리미터
        /// 작성자     : 심재운
        /// 최초작성일 : 2011-04-25
        /// </summary>
        private void LoadParam()
        {
            ModuleName = GetDecryptParam("moduleName", "").Trim();
            SearchValue = GetDecryptParam("searchValue", "").Trim();
        }

        /// <summary>
        /// 작성목적   : 다국어
        /// 작성자     : 심재운
        /// 최초작성일 : 2011-04-25
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                //cbtnHomePost.Text = resMng.GetString("SearchAddress");

                ddlSearchKey.Items.Clear();
                ddlSearchKey.Items.Add(new ListItem(resMng.GetString("TitleNFileName"), "WHOLE"));
                ddlSearchKey.Items.Add(new ListItem(resMng.GetString("SearchOutputTitle"), "SUBJECTS"));
                ddlSearchKey.Items.Add(new ListItem(resMng.GetString("FileName"), "FILENAME"));

                ddlSort.Items.Clear();
                ddlSort.Items.Add(new ListItem(resMng.GetString("SearchRequestSelect"), ""));
                ddlSort.Items.Add(new ListItem(resMng.GetString("SearchOutputDate"), "WriteDate DESC"));
                ddlSort.Items.Add(new ListItem(resMng.GetString("SearchOutputTitle"), "SUBJECTS DESC"));

                ddlSearchTerm.Items.Clear();
                ddlSearchTerm.Items.Add(new ListItem(resMng.GetString("SearchFieldTotal"), ""));
                ddlSearchTerm.Items.Add(new ListItem(resMng.GetString("LastMonth"), "month"));
                ddlSearchTerm.Items.Add(new ListItem(resMng.GetString("LastWeek"), "week"));
                ddlSearchTerm.Items.Add(new ListItem(resMng.GetString("Period"), "period"));

                cbtnSearch.Text = resMng.GetString("Search");
                cbtnApproval.Text = resMng.GetString("GoToDetail");
                cbtnBoard.Text = resMng.GetString("GoToDetail");
                cbtnEDM.Text = resMng.GetString("GoToDetail");
                cbtnAddress.Text = resMng.GetString("GoToDetail");
                cbtnWebHard.Text = resMng.GetString("GoToDetail");
                //cbtnCoop.Text = resMng.GetString("GoToDetail");
                cbtnTask.Text = resMng.GetString("GoToDetail");
                cbtnPim.Text = resMng.GetString("GoToDetail");
                cbtnRM.Text = resMng.GetString("GoToDetail");
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 메뉴 리스트
        /// </summary>
        private void LoadMenu()
        {
            StringBuilder menuHtml = new StringBuilder();
            //menuHtml.Append(string.Format(@"<li id=""menu_whole""><a href=""#"" class=""tab_on"" onclick=""{0}"">{1}</a></li>", "List.SelMenu('')", "" + resMng.GetString("SearchFieldTotal") + ""));
            menuHtml.Append(string.Format(@"<li id=""menu_whole"" class='selected'><a href=""#"" onclick=""{0}"">{1}</a></li>", "List.SelMenu('')", "" + resMng.GetString("SearchFieldTotal") + ""));
            menuHtml.Append(string.Format(@"<li id=""menu_approval""><a href=""#"" onclick=""{0}"">{1}</a></li>", "List.SelMenu('APPROVAL')", "" + resMng.GetString("SearchFieldApproval") + ""));
            menuHtml.Append(string.Format(@"<li id=""menu_board""><a href=""#"" onclick=""{0}"">{1}</a></li>", "List.SelMenu('BOARD')", "" + resMng.GetString("SearchFieldBoard") + ""));
            //menuHtml.Append(string.Format(@"<li id=""menu_coop""><a href=""#"" onclick=""{0}"">{1}</a></li>", "List.SelMenu('COOP')", "" + resMng.GetString("SearchFieldCoop") + ""));
            menuHtml.Append(string.Format(@"<li id=""menu_task""><a href=""#"" onclick=""{0}"">{1}</a></li>", "List.SelMenu('TASK')", "" + resMng.GetString("SearchFieldTask") + ""));
            menuHtml.Append(string.Format(@"<li id=""menu_edm""><a href=""#"" onclick=""{0}"">{1}</a></li>", "List.SelMenu('EDM')", "" + resMng.GetString("SearchFieldEDM") + ""));
            menuHtml.Append(string.Format(@"<li id=""menu_address""><a href=""#"" onclick=""{0}"">{1}</a></li>", "List.SelMenu('ADDRESS')", "" + resMng.GetString("SearchFieldAddress") + ""));
            menuHtml.Append(string.Format(@"<li id=""menu_webhard""><a href=""#"" onclick=""{0}"">{1}</a></li>", "List.SelMenu('WEBHARD')", "" + resMng.GetString("SearchFieldWebHard") + ""));
            menuHtml.Append(string.Format(@"<li id=""menu_pim""><a href=""#"" onclick=""{0}"">{1}</a></li>", "List.SelMenu('PIM')", "" + resMng.GetString("SearchFieldPim") + ""));
            menuHtml.Append(string.Format(@"<li id=""menu_rm"" class='last'><a href=""#"" onclick=""{0}"">{1}</a></li>", "List.SelMenu('RM')", "" + resMng.GetString("SearchFieldRM") + ""));
            //menuHtml.Append(string.Format(@"<li id=""menu_mail"" class='last'><a href=""#"" onclick=""{0}"">{1}</a></li>", "List.SelMenu('MAIL')", "메일"));
            MenuList = menuHtml.ToString();
        }

        #endregion Method (함수)



        #region Property (프로퍼티)

        /// <summary>
        /// 모듈명 가져오기
        /// </summary>
        /// <value>
        /// The name of the module.
        /// </value>
        public string ModuleName { get; set; }

        #endregion Property (프로퍼티)
    }
}