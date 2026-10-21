using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Text;

namespace Zeospace.DeskPlus.Approval.Web.Admin.DocContact
{
    public partial class ListS : Base.BasePageApprovalAdmin
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public string TotalItemCount = String.Empty;  //총카운트 
        #endregion

        #region Page Process (페이지 처리)

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Admin.DocContact.ListR).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                GetParam();
                SetLanguage();
                LoadData();
            }
        }



        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            ddlListCount.SelectedValue = RecordPerPage.ToString();
            UCDeptTreeScript1.CompanyID = CurrentUserInfo.CompanyID;

            // 페이지설정
            #region PageSize 설정하기

            DAL.FolderPageSize dalPageSize = new DAL.FolderPageSize();
            List<BE.FolderPageSizeInfo> al = dalPageSize.GetList(CurrentUserInfo.CompanyID,
                "Admin_Approver", CurrentUserInfo.UserID, "Admin");

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
        /// 작성목적   : 다국어 설정
        /// 작성자     : 
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnActive.Text = resMng.GetString("AddUser");
                cbtnDel.Text = resMng.GetString("DelUser");
            }
        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void GetParam()
        {
            BaseParam();
        }


        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-06-22
        /// 작성목적 : 리스트 총 카운트
        /// </summary>
        private void GetListCount()
        {
            DAL.Account dal = new DAL.Account();
            TotalItemCount = dal.GetPageListCount(CurrentUserInfo.CompanyID, 0, "S").ToString();
        }


        /// <summary>
        /// 작성자 : 이문호
        /// DDL 페이지 사이즈
        /// </summary>
        private void PageSize()
        {
            DAL.FolderPageSize dal = new DAL.FolderPageSize();

            //Process : 결재상황조회 Complete : 문서함
            List<FolderPageSizeInfo> al = dal.GetList(CurrentUserInfo.CompanyID, "Admin_Approver", CurrentUserInfo.UserID, "Admin");

            foreach (FolderPageSizeInfo info in al)
            {
                ddlListCount.SelectedValue = info.PageSize.ToString();
            }
        }


        #endregion

        #region (속성)

        #endregion

        #region (이벤트핸들러)

        #endregion
    }
}