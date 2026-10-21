using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.Web.Admin.DocumentEtc
{
    public partial class JList : Base.BasePageApproval
    {
        #region 변수설정

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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Admin.DocumentEtc.JList).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                GetAccessAuth();
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
                "Admin_DocEtc", CurrentUserInfo.UserID, "Admin");

            foreach (BE.FolderPageSizeInfo userInfo in al)
            {
                RecordPerPage = userInfo.PageSize;
            }
            #endregion

            //GetListCount();
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

                cbtnSearch.Text = resMng.GetString("OrganBtnSearch"); // "검색";
                cbtnDel.Text = resMng.GetString("Btn_DisuseDel"); //"폐기(삭제)";

                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(resMng.GetString("Subject"), "Subject"));                //제목
                ddlSearchField.Items.Add(new ListItem(resMng.GetString("ReportUser"), "ManyName"));             //상신자
                ddlSearchField.Items.Add(new ListItem(resMng.GetString("DocNumber"), "DocNumber"));          //문서번호
                ddlSearchField.Items.Add(new ListItem(resMng.GetString("DeptName"), "DeptManyName"));         //부서명
                ddlSearchField.Items.Add(new ListItem(resMng.GetString("ReportYear"), "Number2"));            //기안년도
                ddlSearchField.Items.Add(new ListItem(resMng.GetString("FormName2"), "FormMany"));           //기안양식
                ddlSearchField.Items.Add(new ListItem(resMng.GetString("ContentTEXT"), "ContentTEXT"));        //본문검색
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("SendDeptReportUser"), "SendDeptManyName")); // 발신부서상신자
                ddlSearchField.Items.Add(new ListItem("DocID", "DocumentID")); 

                ddlSearchTerm.Items[0].Text = this.resMng.GetString("ListType0");
                ddlSearchTerm.Items[1].Text = this.resMng.GetString("Week");
                ddlSearchTerm.Items[2].Text = this.resMng.GetString("Month");
                ddlSearchTerm.Items[3].Text = this.resMng.GetString("Period");

            }
        }

        /// <summary>
        /// 작성자 : 이문호
        /// DDL 페이지 사이즈
        /// </summary>
        private void PageSize()
        {
            DAL.FolderPageSize dal = new DAL.FolderPageSize();

            //Process : 결재상황조회 Complete : 문서함
            List<FolderPageSizeInfo> al = dal.GetList(CurrentUserInfo.CompanyID, "Admin_DocEtc", CurrentUserInfo.UserID, "Admin");

            foreach (FolderPageSizeInfo info in al)
            {
                ddlListCount.SelectedValue = info.PageSize.ToString();
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
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 2012-02-06
        /// 작성목적 : 자원관리 권한 접근 여부 가져오기
        /// </summary>
        private void GetAccessAuth()
        {
            bool AccessAuth = false;
            bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");//administrator
            bool isRoleApproval = CurrentUserInfo.IsInRole("approval");//approval admin
            bool isRoleApprovalMng = CurrentUserInfo.IsInRole("approvalMng");//approval Mng

            if (CurrentUserInfo.IsSysAdmin || isRoleAdmin || isRoleApproval || isRoleApprovalMng)
            {
                AccessAuth = true;
            }

            if (!AccessAuth)
            {
                Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");

                return;
            }
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}