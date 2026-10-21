using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.Web.Folder.Impo
{
    public partial class List : Base.BasePageApproval
    {
        #region 변수설정

        public int TotalItemCount;

        public string CabinetID;
        public string FormID;
        public string YYYY;
        public string YYYYMM;
        public string Types;//S:기본폴더; U : 수발자 관리자 추가 폴더
        public string HeaderName;

        public string Gubun;
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Folder.Impo.List).Assembly;	//어셈블리 선언
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
                "Impo_Doc", CurrentUserInfo.UserID, "Complete");

            foreach (BE.FolderPageSizeInfo userInfo in al)
            {
                RecordPerPage = userInfo.PageSize;
            }
            #endregion

            GetListCount();
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
                ltlHeader.Text = this.resMng.GetString("HomeTitle") + " " + this.resMng.GetString("ImpoMark");

                cbtnSearch.Text = this.resMng.GetString("OrganBtnSearch"); // 검색
                cbtnDel.Text = this.resMng.GetString("ImpoDel"); //삭제

                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Subject"), "Subject"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("ReportUser"), "ManyName"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("DocNumber"), "DocNumber"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("DeptName"), "DeptManyName"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("ReportYear"), "Number2"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("FormName2"), "FormMany"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("ContentTEXT"), "ContentTEXT"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("SendDeptReportUser"), "SendDeptManyName")); // 발신부서상신자

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
            this.Order = "WriteDate DESC";

            CabinetID = GetDecryptParam("cabinetID", "");//ID
            Types = GetDecryptParam("types", ""); //S(SYSTEM, U:USER)
            FormID = GetDecryptParam("formID", ""); //양식id
            YYYY = GetDecryptParam("yyyy", ""); //년도
            YYYYMM = GetDecryptParam("yyyymm", ""); //년월 
            HeaderName = GetDecryptParam("headerName", "");//리스트 해더 경로(동적은 없으므로 파리미터 날림)

            Gubun = GetDecryptParam("gubun", "");
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

            DAL.ImpoUser dal = new DAL.ImpoUser();
            TotalItemCount = dal.GetPageCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, startDate, endDate, SearchField, SearchValue);
        }

        /// <summary>
        /// 작성자 : 이문호
        /// DDL 페이지 사이즈
        /// </summary>
        private void PageSize()
        {
            DAL.FolderPageSize dal = new DAL.FolderPageSize();

            //Process : 결재상황조회 Complete : 문서함
            List<FolderPageSizeInfo> al = dal.GetList(CurrentUserInfo.CompanyID, "Impo_Doc", CurrentUserInfo.UserID, "Complete");

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