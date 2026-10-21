using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;


namespace Zeospace.DeskPlus.Approval.Web.FormFlow.Basic
{
    public partial class GetBaseDoc : Base.BasePageApproval
    {
        #region 변수설정
        public int TotalItemCount;
        public string FolderName;
        public string BaseDocIDs; 

        //미사용 변수 (프로시저 인수값 지정을 위해 초기화)
        public string FormID;
        public string YYYY;
        public string YYYYMM;
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.FormFlow.Basic.GetBaseDoc).Assembly;	//어셈블리 선언
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

            ddlSearchTerm.SelectedValue = SearchTerm;
            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;
            ddlListCount.SelectedValue = this.RecordPerPage.ToString();

            // 페이지설정
            #region PageSize 설정하기

            DAL.FolderPageSize dalPageSize = new DAL.FolderPageSize();
            List<BE.FolderPageSizeInfo> al = dalPageSize.GetList(CurrentUserInfo.CompanyID,
                "BaseDoc", CurrentUserInfo.UserID, "BaseDoc");

            foreach (BE.FolderPageSizeInfo userInfo in al)
            {
                RecordPerPage = userInfo.PageSize;
            }
            #endregion

            PageSize();
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
                cbtnSearch.Text = this.resMng.GetString("OrganBtnSearch"); // 검색
                cbtnSave.Text = this.resMng.GetString("Confirm"); //"저장";

                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Subject"), "Subject"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("ReportUser"), "ManyName"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("DocNumber"), "DocNumber"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("DeptName"), "DeptManyName"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("ReportYear"), "Number2"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("FormName2"), "FormMany"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("ContentTEXT"), "ContentTEXT"));

                ddlSearchTerm.Items[0].Text = this.resMng.GetString("ListType0");
                ddlSearchTerm.Items[1].Text = this.resMng.GetString("Week");
                ddlSearchTerm.Items[2].Text = this.resMng.GetString("Month");
                ddlSearchTerm.Items[3].Text = this.resMng.GetString("Period");

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
            this.Order = "CompleteDate desc";
            this.FolderName = GetDecryptParam("folderName", ""); //문서함 탭명
            this.BaseDocIDs = GetDecryptParam("baseDocIDs", ""); //추가한 문서 documentid들
        }

        /// <summary>
        /// 작성자 : 이문호
        /// DDL 페이지 사이즈
        /// </summary>
        private void PageSize()
        {
            DAL.FolderPageSize dal = new DAL.FolderPageSize();

            //Process : 결재상황조회 Complete : 문서함
            List<FolderPageSizeInfo> al = dal.GetList(CurrentUserInfo.CompanyID, "BaseDoc", CurrentUserInfo.UserID, "BaseDoc");

            foreach (FolderPageSizeInfo info in al)
            {
                ddlListCount.SelectedValue = info.PageSize.ToString();
            }
        }
        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}