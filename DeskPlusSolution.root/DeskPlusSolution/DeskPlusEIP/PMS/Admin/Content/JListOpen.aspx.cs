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
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text;
using System.Collections.Generic;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.PMS.Web.Admin.Content
{
    public partial class JListOpen : Base.BasePagePMSAdmin
    {
        #region Declaration (컨트롤 선언)        
        public int pageCount;

        public string UseDeptID;
        public int PageNo;
        public int FolderID;
        public int Status;

        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Admin.Content.JListOpen).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();
            }
        }

        #endregion


        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadList();
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
                cbtnConfirm.Text = resMng.GetString("Project_ProjectInfo_Approval");
                cbtnSearch.Text = resMng.GetString("Search");

                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("List_ProjectName"), "ManySubject"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("ProjectList_Header2"), "ManyName"));

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
            UseDeptID = GetDecryptParam("useDeptID", "");
            FolderID = GetDecryptParam("folderID", "0").ToNumber<int>();
            Status = GetDecryptParam("status", "1").ToNumber<int>();
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

            RecordPerPage = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.BOARD, "ListCount", "10").ToNumber<int>();//페이지리스트수

            ddlListCount.SelectedValue = RecordPerPage.ToString();
        }

        #region 리스트 템플릿
        #endregion

        #endregion

        #region Property (프로퍼티)
        #endregion


        #region Event Handler (이벤트 핸들러)
        #endregion
    }
}
