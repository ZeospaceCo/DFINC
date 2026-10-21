using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace Zeospace.DeskPlus.EIP.Web.Search
{
    public partial class List : Base.BasePageEIP
    {
        #region 변수설정
        public string SearchValue = string.Empty;
        public string MenuList = string.Empty;
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.EIP.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Search.List).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {                
                
                LoadData();
            }
        }

        #endregion

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
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 메뉴 리스트
        /// </summary>
        private void LoadMenu()
        {
            StringBuilder menuHtml = new StringBuilder();
            menuHtml.Append(string.Format(@"<li id=""menu_whole""><a href=""#"" class=""tab_on"" onclick=""{0}"">{1}</a></li>", "List.SelMenu('')", resMng.GetString("SearchFieldTotal")));
            menuHtml.Append(string.Format(@"<li id=""menu_approval""><a href=""#"" onclick=""{0}"">{1}</a></li>", "List.SelMenu('APPROVAL')", resMng.GetString("SearchFieldApproval")));
            menuHtml.Append(string.Format(@"<li id=""menu_board""><a href=""#"" onclick=""{0}"">{1}</a></li>", "List.SelMenu('BOARD')", resMng.GetString("SearchFieldBoard")));
            menuHtml.Append(string.Format(@"<li id=""menu_rm""><a href=""#"" onclick=""{0}"">{1}</a></li>", "List.SelMenu('RM')", resMng.GetString("SearchFieldRM")));
            menuHtml.Append(string.Format(@"<li id=""menu_report"" class='last'><a href=""#"" onclick=""{0}"">{1}</a></li>", "List.SelMenu('REPORT')", resMng.GetString("SearchFieldREPORT")));
            //menuHtml.Append(string.Format(@"<li id=""menu_mail"" class='last'><a href=""#"" onclick=""{0}"">{1}</a></li>", "List.SelMenu('MAIL')", "메일"));
            MenuList = menuHtml.ToString();
        }
    

        #endregion

        #region Event Handler (이벤트 핸들러)

        

        #endregion

        #region Property (프로퍼티)

        

        #endregion
    }
}