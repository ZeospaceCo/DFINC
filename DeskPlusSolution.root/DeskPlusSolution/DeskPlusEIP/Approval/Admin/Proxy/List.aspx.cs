using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Approval.DAL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.Web.Admin.Proxy
{
    public partial class List : Base.BasePageApprovalAdmin
    {
        #region 변수설정
        public string TotalItemCount = String.Empty;  //총카운트
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Admin.Proxy.List).Assembly;	    //어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
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
            ddlSearchTerm.SelectedValue = SearchTerm;
            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;

            ddlListCount.SelectedValue = RecordPerPage.ToString();

            // 페이지설정
            #region PageSize 설정하기

            DAL.FolderPageSize dalPageSize = new DAL.FolderPageSize();
            List<BE.FolderPageSizeInfo> al = dalPageSize.GetList(CurrentUserInfo.CompanyID,
                "Proxy", CurrentUserInfo.UserID, "Admin");

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
                cbtnSearch.Text = this.resMng.GetString("OrganBtnSearch"); // 검색

                #region 날짜검색
                ddlSearchTerm.Items[0].Text = this.resMng.GetString("ListType0");
                ddlSearchTerm.Items[1].Text = this.resMng.GetString("Week");
                ddlSearchTerm.Items[2].Text = this.resMng.GetString("Month");
                ddlSearchTerm.Items[3].Text = this.resMng.GetString("Period");
                #endregion

                #region 검색필드
                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Name"), "ManyName"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("DeptName"), "ManyDeptName"));
                //ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("TitleName2"), "ManyTitleName"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("TargetUser") + " " + this.resMng.GetString("Name"), "ProxyManyName"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("TargetUser") + " " + this.resMng.GetString("DeptName"), "ProxyManyDeptName"));
                #endregion
            }
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            
        }

        /// <summary>
        /// 작성자 : 이문호
        /// 작성일자 : 2012-05-09
        /// 작성목적 : 리스트 총 카운트
        /// </summary>
        private void GetListCount()
        {
            DAL.Proxy list = new DAL.Proxy();
            TotalItemCount = list.GetPageListCountAdmin(CurrentUserInfo.CompanyID, "0", "0", "", "", "", "").ToString();
        }


        /// <summary>
        /// 작성자 : 이문호
        /// DDL 페이지 사이즈
        /// </summary>
        private void PageSize()
        {
            DAL.FolderPageSize dal = new DAL.FolderPageSize();

            //Process : 결재상황조회 Complete : 문서함
            List<FolderPageSizeInfo> al = dal.GetList(CurrentUserInfo.CompanyID, "Proxy", CurrentUserInfo.UserID, "Admin");

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