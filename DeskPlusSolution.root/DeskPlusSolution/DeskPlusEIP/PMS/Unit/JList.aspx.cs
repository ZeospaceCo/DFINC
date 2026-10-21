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

namespace Zeospace.DeskPlus.PMS.Web.Unit
{
    public partial class JList : Base.BasePagePMS
    {
        public string Status = string.Empty;
        public int WeightTotal;

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Unit.JList).Assembly;	//어셈블리 선언
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
            if (Status.Length > 0) ddlStatus.SelectedValue = Status;
            if (SearchField.Length > 0) ddlSearchField.SelectedValue = SearchField;
            if (SearchValue.Length > 0) ctbSearch.Text = SearchValue;


            BE.UnitInfo listInfo = new Zeospace.DeskPlus.PMS.BE.UnitInfo();
            DAL.Unit list = new Zeospace.DeskPlus.PMS.DAL.Unit();

            //가중치 합산 가져오기
            WeightTotal = list.GetUnitWeightPercent(CurrentUserInfo.CompanyID, ProjectID);

            ltlMenuName.Text = MenuName;
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
                if (MenuID == 0)
                {
                    return lib.MenuName(CurrentUserInfo.CompanyID, ProjectID, 0, 2, PreLang);
                }
                else
                {
                    return lib.MenuName(CurrentUserInfo.CompanyID, ProjectID, MenuID, PreLang);
                }
            }
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
                cbtnWrite.Text = resMng.GetString("Writing"); //작성하기
                cbtnOrder.Text = resMng.GetString("ChangeOrder");

                ddlStatus.Items.Clear();
                ddlStatus.Items.Add(new ListItem(GetResource("Unit_whole"), "0"));
                ddlStatus.Items.Add(new ListItem(this.resMng.GetString("List_Status1"), "1"));
                ddlStatus.Items.Add(new ListItem(this.resMng.GetString("List_Status2"), "2"));
                ddlStatus.Items.Add(new ListItem(this.resMng.GetString("List_Status3"), "3"));
                ddlStatus.Items.Add(new ListItem(this.resMng.GetString("List_Status4"), "4"));
                ddlStatus.Items.Add(new ListItem(this.resMng.GetString("List_Status5"), "5"));

                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Unit_Register_ltlTitle"), "Subjects"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Unit_Code"), "Code"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Unit_Owner"), "ManyName"));
            }
        }

        /// <summary>
        /// 파라미터 정의
        /// </summary>
        private void LoadParam()
        {
            Status = GetDecryptParam("status", "0");
        }

        #endregion
    }
}