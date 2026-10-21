using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.Web.Admin.FormGeneration
{
    public partial class List : Base.BasePageApprovalAdmin
    {
        #region 변수설정
        public int TotalItemCount;      //총카운트
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Admin.FormGeneration.List).Assembly;	//어셈블리 선언
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
                "Admin_Form", CurrentUserInfo.UserID, "Admin");

            foreach (BE.FolderPageSizeInfo userInfo in al)
            {
                RecordPerPage = userInfo.PageSize;
            }
            #endregion

            GetListCount();
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
                //Button
                cbtnActive.Text = this.resMng.GetString("ActiveNInactive");       //"활성/비활성화";
                cbtnAdd.Text = this.resMng.GetString("Add");      //"추가";
                cbtnDel.Text = this.resMng.GetString("DelCompletely");       // "영구삭제";
                cbtnSearch.Text = this.resMng.GetString("OrganBtnSearch"); //"검색";
                cbtnFormSeq.Text = this.resMng.GetString("ChangeOrder3");

                //DropDown Item
                this.ddlSearchField.Items.Clear();
                this.ddlSearchField.Items.Add(new ListItem(resMng.GetString("FormName2"), "FormMany"));
                this.ddlSearchField.Items.Add(new ListItem(resMng.GetString("FolderName"), "ManyName"));

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
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 게시물 총 갯수
        /// </summary>
        private void GetListCount()
        {
            string startDate = "", endDate = "", getDate = "";

            if (SearchTerm.ToUpper().Equals("WEEK") || SearchTerm.ToUpper().Equals("MONTH"))
            {
                getDate = (SearchTerm.ToUpper().Equals("WEEK")) ? DateHelper.GetCultureStartEndDate("week", PreLangReal) : DateHelper.GetCultureStartEndDate("month", PreLangReal);
                startDate = DateHelper.GetCultureDate(getDate.Split(',')[0], "yyyyMMdd", PreLangReal);
                endDate = DateHelper.GetCultureDate(getDate.Split(',')[1], "yyyyMMdd", PreLangReal);
            }
            else if (SearchTerm.ToUpper().Equals("PERIOD"))
            {
                startDate = DateHelper.GetCultureDate(SearchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                endDate = DateHelper.GetCultureDate(SearchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
            }

            DAL.Form fDal = new DAL.Form();
            TotalItemCount = fDal.GetList(CurrentUserInfo.CompanyID, SearchField, SearchValue, startDate, endDate);
        }

        /// <summary>
        /// 작성자 : 이문호
        /// DDL 페이지 사이즈
        /// </summary>
        private void PageSize()
        {
            DAL.FolderPageSize dal = new DAL.FolderPageSize();

            //Process : 결재상황조회 Complete : 문서함
            List<FolderPageSizeInfo> al = dal.GetList(CurrentUserInfo.CompanyID, "Admin_Form", CurrentUserInfo.UserID, "Admin");

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