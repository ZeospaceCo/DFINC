using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Zeospace.DeskPlus.PMS.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;


namespace Zeospace.DeskPlus.PMS.Web.Member
{
    public partial class JList : Base.BasePagePMS
    {
        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Member.JList).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();
            }


            #region 버튼 이미지
            //cbtnFindit.Icon = BaseImgPath + "/Board/Button/but_search.gif";
            #endregion


        }

        #endregion


        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        private void LoadData()
        {
            ltlMenuName.Text = MenuName;
            
            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;
        }

        #endregion


        #region Method (함수)

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            if (!this.resMng.Equals(null))
            {
                cbtnSearch.Text = resMng.GetString("Search");               

                ddlStatus.Items.Clear();
                ddlStatus.Items.Add(new ListItem("----", "0"));
                ddlStatus.Items.Add(new ListItem(this.resMng.GetString("Admin_Menu"), "1"));
                ddlStatus.Items.Add(new ListItem(this.resMng.GetString("Participant"), "2"));

                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Board_List_Name"), "ManyName"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Member_TitleName"), "TitleManyName"));
            }
        }

        /// <summary>
        /// 파라미터 정의
        /// </summary>
        private void LoadParam()
        {

        }


        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 메뉴제목
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-22
        /// </summary>
        /// <value>The name of the menu.</value>
        public string MenuName
        {
            get
            {
                Zeospace.DeskPlus.PMS.Web.Module.Lib lib = new Module.Lib();
                return lib.MenuName(CurrentUserInfo.CompanyID, ProjectID, MenuID, PreLang);
            }
        }

        #endregion
    }
}
