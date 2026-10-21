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
    public partial class ListUser : Base.BasePageApprovalAdmin
    {
        #region 변수설정

        //이관할부서                                    : BasePageApprovalAdmin

        //이관받을부서
        public int tCurrentPage = 0;
        public int tRecordPerPage = 10;
        public string tSearchField = string.Empty;       //검색키워드
        public string tSearchValue = string.Empty;       //검색어
        public string tSearchTerm = string.Empty;        //검색기간구분자
        public string tSearchTermValue = string.Empty;   //검색기간 시작,종룓값        
        public string tOrder = string.Empty;             //정렬

        public string TargetPage = string.Empty;    //리스트 경로

        public int ReqID = 0;           // 변경전정보
        public string ReqName = "";
        public int TargetID = 0;        // 변경후정보
        public string TargetName = "";

        public string SelFolder = "";
        public string SelFolderTarget = "";

        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Admin.Transfer.ListUser).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        private void LoadData()
        {
            LoadParam();
            SetLanguage();

            //이관할부서
            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;
            ddlListCount.SelectedValue = RecordPerPage.ToString();

            //이관받을부서
            ddlSearchFieldTarget.SelectedValue = tSearchField;
            ctbSearchTarget.Text = tSearchValue;
            ddlListCountTarget.SelectedValue = tRecordPerPage.ToString();

            //확인필요
            if (!SelFolder.Equals(""))
            {
                ddlFolder.SelectedValue = SelFolder;
            }
            if (!SelFolderTarget.Equals(""))
            {
                ddlFolderTarget.SelectedValue = SelFolderTarget;
            }

            sendinfo.InnerHtml = ReqName;
            targetinfo.InnerHtml = TargetName;

            TargetPage = Request.Url.AbsolutePath;
        }

        #endregion

        #region (함수)

        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                //Button
                cbtnTransfer.Text = this.resMng.GetString("TransferBtnSubmit");         //선택이관
                cbtnTransferAll.Text = this.resMng.GetString("TransferBtnSubmitAll");   //전체이관
                cbtnCopy.Text = this.resMng.GetString("TransferBtnCopy");               //선택복사
                cbtnCopyAll.Text = this.resMng.GetString("TransferBtnCopyAll");         //전체복사

                cbtnSearch.Text = this.resMng.GetString("OrganBtnSearch");              //검색
                cbtnSearchTarget.Text = this.resMng.GetString("OrganBtnSearch");        //검색

                #region Dropdownlist 바인딩
                //이관할대상
                this.ddlFolder.Items.Clear();
                this.ddlFolder.Items.Add(new ListItem(resMng.GetString("TransFolderPri"), "Private"));   //개인문서함
                this.ddlFolder.Items.Add(new ListItem(resMng.GetString("TransFolderApp"), "Approve"));   //결재문서함
                this.ddlFolder.Items.Add(new ListItem(resMng.GetString("TransFolderAppA"), "Arbitrary"));   //전결문서함
                this.ddlFolder.Items.Add(new ListItem(resMng.GetString("TransFolderCarbon"), "Carbon"));   //참조문서함
                this.ddlFolder.Items.Add(new ListItem(resMng.GetString("TransFolderForward"), "Forward"));   //전달문서함
                this.ddlFolder.Items.Add(new ListItem(resMng.GetString("TransFolderCirR"), "CirReceive"));   //받은회람문서함
                this.ddlFolder.Items.Add(new ListItem(resMng.GetString("TransFolderCirS"), "CirSend"));   //보낸회람문서함

                //이관할부서:검색조건
                this.ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Subject"), "Subject")); //제목  
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("ReportUser"), "ManyName")); //상신자
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("TagDic_NO"), "DocNumber")); //문서번호
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("DeptName"), "DeptManyName")); //"부서명"
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("ReportYear"), "Number2")); //기안년도
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("FormName2"), "FormMany")); //기안양식

                //이관받을부서
                this.ddlFolderTarget.Items.Clear();
                this.ddlFolderTarget.Items.Add(new ListItem(resMng.GetString("TransFolderPri"), "Private"));   //개인문서함
                this.ddlFolderTarget.Items.Add(new ListItem(resMng.GetString("TransFolderApp"), "Approve"));   //결재문서함
                this.ddlFolderTarget.Items.Add(new ListItem(resMng.GetString("TransFolderAppA"), "Arbitrary"));   //전결문서함
                this.ddlFolderTarget.Items.Add(new ListItem(resMng.GetString("TransFolderCarbon"), "Carbon"));   //참조문서함
                this.ddlFolderTarget.Items.Add(new ListItem(resMng.GetString("TransFolderForward"), "Forward"));   //전달문서함
                this.ddlFolderTarget.Items.Add(new ListItem(resMng.GetString("TransFolderCirR"), "CirReceive"));   //받은회람문서함
                this.ddlFolderTarget.Items.Add(new ListItem(resMng.GetString("TransFolderCirS"), "CirSend"));   //보낸회람문서함

                //이관받을부서:검색조건
                this.ddlSearchFieldTarget.Items.Clear();
                ddlSearchFieldTarget.Items.Add(new ListItem(this.resMng.GetString("Subject"), "Subject")); //제목  
                ddlSearchFieldTarget.Items.Add(new ListItem(this.resMng.GetString("ReportUser"), "ManyName")); //상신자
                ddlSearchFieldTarget.Items.Add(new ListItem(this.resMng.GetString("TagDic_NO"), "DocNumber")); //문서번호
                ddlSearchFieldTarget.Items.Add(new ListItem(this.resMng.GetString("DeptName"), "DeptManyName")); //"부서명"
                ddlSearchFieldTarget.Items.Add(new ListItem(this.resMng.GetString("ReportYear"), "Number2")); //기안년도
                ddlSearchFieldTarget.Items.Add(new ListItem(this.resMng.GetString("FormName2"), "FormMany")); //기안양식
                #endregion
            }
        }

        private void LoadParam()
        {
            //이관할부서
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>(); //현페이지 번호
            RecordPerPage = GetDecryptParam("recordPerPage", "0").ToNumber<int>(); //페이지당 리스트 개수
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            Order = GetDecryptParam("order", "");
            if (RecordPerPage.Equals(0))
            {
                RecordPerPage = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.APPROVAL, "ListCount", "10").ToNumber<int>();      //페이지리스트수
            }

            //이관받을부서
            tCurrentPage = GetDecryptParam("tcurrentPage", "1").ToNumber<int>(); //현페이지 번호
            tRecordPerPage = GetDecryptParam("trecordPerPage", "0").ToNumber<int>(); //페이지당 리스트 개수
            tSearchField = GetDecryptParam("tsearchField", "");
            tSearchValue = GetDecryptParam("tsearchValue", "");
            tOrder = GetDecryptParam("torder", "");
            if (tRecordPerPage.Equals(0))
            {
                tRecordPerPage = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.APPROVAL, "ListCount", "10").ToNumber<int>();     //페이지리스트수
            }

            ReqID = GetDecryptParam("reqid", "0").ToNumber<int>();
            ReqName = GetDecryptParam("reqname", "");
            TargetID = GetDecryptParam("targetid", "0").ToNumber<int>();
            TargetName = GetDecryptParam("targetname", "");
            SelFolder = GetDecryptParam("selfolder", "");
            SelFolderTarget = GetDecryptParam("selfoldertarget", "");
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}