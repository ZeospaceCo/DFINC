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

namespace Zeospace.DeskPlus.PMS.Web.Admin.Content
{
    public partial class JListLinked : Base.BasePagePMSAdmin
    {
        #region Declaration (컨트롤 선언)
        public int pageCount;

        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Admin.Content.JListLinked).Assembly;	//어셈블리 선언
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
                cbtnReject.Text = resMng.GetString("Reject");
                cbtnConfirm.Text = resMng.GetString("Project_ProjectInfo_Approval");
                cbtnSearch.Text = resMng.GetString("Search");

                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("List_ProjectName"), "ManySubject"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("ProjectList_Header2"), "ManyName"));
            }
        }

        /// <summary>
        /// 파라미터 정의
        /// </summary>
        private void LoadParam()
        {            
            
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

        


        

        #endregion

        #region Property (프로퍼티)

        #endregion

        #region Event Handler (이벤트 핸들러)

        #endregion
    }
}
