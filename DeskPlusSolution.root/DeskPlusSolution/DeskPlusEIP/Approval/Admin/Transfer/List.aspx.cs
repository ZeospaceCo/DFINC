using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.Web.Admin.Transfer
{
    public partial class List : Base.BasePageApprovalAdmin
    {
        #region 변수설정
        public string TargetPage = string.Empty; //리스트 경로
        public int BeforeTargetID = 0;     // 변경전정보
        public int AfterTargetID = 0;      // 변경후정보
        public string BeforeTargetName = "";
        public string AfterTargetName = "";
        public string SelFolder = "";
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Admin.Transfer.List).Assembly;	//어셈블리 선언
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

            if (!SelFolder.Equals(""))
            {
                ddlFolder.SelectedValue = SelFolder;
            }
            sendInfo.InnerHtml = BeforeTargetName;
            receiveInfo.InnerHtml = AfterTargetName;

            // 페이지설정
            #region PageSize 설정하기

            DAL.FolderPageSize dalPageSize = new DAL.FolderPageSize();
            List<BE.FolderPageSizeInfo> al = dalPageSize.GetList(CurrentUserInfo.CompanyID,
                "Admin_Transfer", CurrentUserInfo.UserID, "Admin");

            foreach (BE.FolderPageSizeInfo userInfo in al)
            {
                RecordPerPage = userInfo.PageSize;
            }
            #endregion

            PageSize();

            TargetPage = Request.Url.AbsolutePath;
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
                //Button
                cbtnSearch.Text = this.resMng.GetString("OrganBtnSearch"); // 검색

                cbtnBeforeDeptGarbage.Text = this.resMng.GetString("TransferBtnDeptDel"); // 변경전삭제부서
                cbtnBeforeDept.Text = this.resMng.GetString("TransferBtnDeptB");    // "변경전부서";
                cbtnBeforeUser.Text = this.resMng.GetString("TransferBtnUserB");    // "변경전사용자";
                cbtnAfterDept.Text = this.resMng.GetString("TransferBtnDeptA");    // "변경후부서";
                cbtnAfterUser.Text = this.resMng.GetString("TransferBtnUserA");    // "변경후사용자";
                cbtnTransfer.Text = this.resMng.GetString("TransferBtnSubmit");    // "선택이관";
                cbtnTransferAll.Text = this.resMng.GetString("TransferBtnSubmitAll");    // "전체이관";
                cbtnCopy.Text = this.resMng.GetString("TransferBtnCopy");    // "선택복사";
                cbtnCopyAll.Text = this.resMng.GetString("TransferBtnCopyAll");    // "전체복사";

                //문서함
                this.ddlFolder.Items.Clear();
                this.ddlFolder.Items.Add(new ListItem(resMng.GetString("TransFolderDept"), "Dept"));   //부서별문서함
                this.ddlFolder.Items.Add(new ListItem(resMng.GetString("TransFolderPri"), "Private"));   //개인문서함
                this.ddlFolder.Items.Add(new ListItem(resMng.GetString("TransFolderApp"), "Approve"));   //결재문서함
                this.ddlFolder.Items.Add(new ListItem(resMng.GetString("TransFolderAppA"), "Arbitrary"));   //전결문서함
                this.ddlFolder.Items.Add(new ListItem(resMng.GetString("TransFolderCarbon"), "Carbon"));   //참조문서함
                this.ddlFolder.Items.Add(new ListItem(resMng.GetString("TransFolderForward"), "Forward"));   //전달문서함
                this.ddlFolder.Items.Add(new ListItem(resMng.GetString("TransFolderCirR"), "CirReceive"));   //받은회람문서함
                this.ddlFolder.Items.Add(new ListItem(resMng.GetString("TransFolderCirS"), "CirSend"));   //보낸회람문서함

                //DropDown Item  - d
                this.ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Subject"), "Subject")); //제목  
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("ReportUser"), "ManyName")); //상신자
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("TagDic_NO"), "DocNumber")); //문서번호
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("DeptName"), "DeptManyName")); //"부서명"
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("ReportYear"), "Number2")); //기안년도
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("FormName2"), "FormMany")); //기안양식
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("ContentTEXT"), "ContentTEXT"));   //"본문검색"  

                this.ddlSearchTerm.Items.Clear();
                this.ddlSearchTerm.Items.Add(new ListItem(resMng.GetString("ListType0"), ""));
                this.ddlSearchTerm.Items.Add(new ListItem(resMng.GetString("Week"), "week"));
                this.ddlSearchTerm.Items.Add(new ListItem(resMng.GetString("Month"), "month"));
                this.ddlSearchTerm.Items.Add(new ListItem(resMng.GetString("Period"), "period"));

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

            BeforeTargetID = GetDecryptParam("beforeTargetID", "").ToNumber<int>();
            AfterTargetID = GetDecryptParam("afterTargetID", "").ToNumber<int>();
            BeforeTargetName = GetDecryptParam("beforeTargetName", "");
            AfterTargetName = GetDecryptParam("afterTargetName", "");
            SelFolder = GetDecryptParam("selFolder", "");
        }

        /// <summary>
        /// 작성자 : 이문호
        /// DDL 페이지 사이즈
        /// </summary>
        private void PageSize()
        {
            DAL.FolderPageSize dal = new DAL.FolderPageSize();

            //Process : 결재상황조회 Complete : 문서함
            List<FolderPageSizeInfo> al = dal.GetList(CurrentUserInfo.CompanyID, "Admin_Transfer", CurrentUserInfo.UserID, "Admin");

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