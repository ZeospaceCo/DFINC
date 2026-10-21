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


namespace Zeospace.DeskPlus.PMS.Web.Board
{
    public partial class JList : Base.BasePagePMS
    {
        #region Declaration (컨트롤 선언)
        public int pageCount;
        public int PageNo;
        public string TotalCount = String.Empty;  //총카운트
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Board.JList).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();
            }


            #region 버튼 이미지

            #endregion            

            #region 관련프로젝트 처리
            if (Authority == 1) cbtnWrite.Visible = false;
            #endregion
        }

        #endregion


        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        private void LoadData()
        {
            LoadList();

            ltlMenuName.Text = MenuName;
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
                cbtnWrite.Text = resMng.GetString("Writing");
                cbtnSearch.Text = resMng.GetString("Search");

                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Board_List_Title"), "Title"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Board_List_Name"), "ManyName"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Board_List_Content"), "MobileContent"));


                ddlSearchTerm.Items[0].Text = this.resMng.GetString("Unit_whole");
                ddlSearchTerm.Items[1].Text = this.resMng.GetString("Week");
                ddlSearchTerm.Items[2].Text = this.resMng.GetString("Month");
                ddlSearchTerm.Items[3].Text = this.resMng.GetString("Period");
            }
        }

        /// <summary>
        /// 파라미터 정의
        /// </summary>
        private void LoadParam()
        {
            PageNo = GetDecryptParam("pageNo", "0").ToNumber<int>();
        }

        
        /// <summary>
        /// 게시판 리스트 : Loads the board list.
        /// </summary>
        /// <param name="_categoryID">_category ID.</param>
        private void LoadList()
        {
            ddlSearchTerm.SelectedValue = SearchTerm;
            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;

            //RecordPerPage = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.BOARD, "ListCount", "10").ToNumber<int>();//페이지리스트수

            GetListCount();
        }

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

            DAL.Board dal = new DAL.Board();
            BE.BoardInfo listInfo = new Zeospace.DeskPlus.PMS.BE.BoardInfo();

            listInfo.CompanyID = CurrentUserInfo.CompanyID;
            listInfo.ProjectID = ProjectID;
            listInfo.MenuID = MenuID;
            listInfo.SearchField = SearchField;
            listInfo.SearchValue = SearchValue;
            listInfo.SearchStartDate = startDate;
            listInfo.SearchEndDate = endDate;

            TotalCount = dal.GetPageCount(listInfo).ToString();
        }

        #endregion

        #region Property (프로퍼티)

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
                Zeospace.DeskPlus.PMS.Web.Module.Lib biz = new Module.Lib();
                if (MenuID == 0)
                {
                    return biz.MenuName(CurrentUserInfo.CompanyID, ProjectID, 0, 3, PreLang);
                }
                else
                {
                    return biz.MenuName(CurrentUserInfo.CompanyID, ProjectID, MenuID, PreLang);
                }
            }
        }

        
        #endregion


        #region Event Handler (이벤트 핸들러)
        #endregion
    }
}
