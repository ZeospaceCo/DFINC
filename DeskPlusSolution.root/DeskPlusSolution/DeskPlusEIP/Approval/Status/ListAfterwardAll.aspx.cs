using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;


namespace Zeospace.DeskPlus.Approval.Web.Status
{
    public partial class ListAfterwardAll : Base.BasePageApproval
    {
        #region 변수설정
        public string FolderStatusID;
        public int TotalItemCount;
        public int TotalAfterwardCount;
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Status.ListAfterwardAll).Assembly;	//어셈블리 선언
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

            #region admin 관리자 체크
            cbtnAdmin.Visible = (AccessAuth) ? true : false;
            #endregion

            ddlSearchTerm.SelectedValue = SearchTerm;
            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;
            ddlListCount.SelectedValue = this.RecordPerPage.ToString();

            // 페이지설정
            #region PageSize 설정하기

            DAL.FolderPageSize dalPageSize = new DAL.FolderPageSize();
            List<BE.FolderPageSizeInfo> al = dalPageSize.GetList(CurrentUserInfo.CompanyID,
                FolderStatusID, CurrentUserInfo.UserID, "Process");

            foreach (BE.FolderPageSizeInfo userInfo in al)
            {
                RecordPerPage = userInfo.PageSize;
            }

            #endregion

            GetListCount();
            PageSize();

            if (FolderStatusID.ToNull().Length > 0)
            {
                DAL.FolderStatusAll dal = new DAL.FolderStatusAll();

                var alList = dal.GetFolderStatusName(CurrentUserInfo.CompanyID, FolderStatusID, true);
                foreach (var listinfo in alList)
                {
                    #region 전체경로

                    string name = this.GetLangName(
                            listinfo.NameBase,
                            listinfo.NameKo,
                            listinfo.NameEn, listinfo.NameJa,
                            listinfo.NameFr, listinfo.NameZhcn,
                            listinfo.NameZhtw, PreLang);

                    ltlHeader.Text = name;

                    #endregion
                }
            }

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
                cbtnAdmin.Text = this.resMng.GetString("Administrator"); // 관리자
                cbtnDefault.Text = this.resMng.GetString("DefaultMenu"); // 초기메뉴로 설정
                cbtnSearch.Text = this.resMng.GetString("OrganBtnSearch"); // 검색

                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Subject"), "Subject")); //제목
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("ReportUser"), "ManyName")); // 상신자
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("ReportYear"), "Number2")); //기안년도
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("FormName2"), "FormMany")); //양식명
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("SendDeptReportUser"), "SendDeptManyName")); // 발신부서상신자

                ddlSearchTerm.Items[0].Text = this.resMng.GetString("ListType0");    // 전체
                ddlSearchTerm.Items[1].Text = this.resMng.GetString("Week");         // 1주
                ddlSearchTerm.Items[2].Text = this.resMng.GetString("Month");        // 1달
                ddlSearchTerm.Items[3].Text = this.resMng.GetString("Period");       // 기간

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
            this.Order = "";
            FolderStatusID = GetDecryptParam("folderStatusID", "");
        }

        /// <summary>
        /// 작성자 :
        /// 작성일자 :
        /// 작성목적 : 게시물 총 갯수
        /// </summary>
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


            DAL.DocumentProcessAll dal = new DAL.DocumentProcessAll();
            TotalItemCount = dal.GetPageCountAfterwardAll(CurrentUserInfo.CompanyID,
                CurrentUserInfo.UserID, startDate, endDate,
                SearchField, SearchValue);

            TotalAfterwardCount = dal.GetPageCountAfterwardAll(CurrentUserInfo.CompanyID,
                CurrentUserInfo.UserID, startDate, endDate,
                SearchField, SearchValue);
        }

        /// <summary>
        /// 작성자 : 이문호
        /// DDL 페이지 사이즈
        /// </summary>
        private void PageSize()
        {
            DAL.FolderPageSize dal = new DAL.FolderPageSize();

            //Process : 결재상황조회 Complete : 문서함
            List<FolderPageSizeInfo> al = dal.GetList(CurrentUserInfo.CompanyID, FolderStatusID, CurrentUserInfo.UserID, "Process");

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
