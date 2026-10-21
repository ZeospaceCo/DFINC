using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Approval.DAL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.Web.Preference.Flow
{
    public partial class List : Base.BasePageApproval
    {
        #region 변수설정
        public string TotalItemCount = String.Empty;  //총카운트
        //public int ModifyCount = 0;
        //public int ApprovalUserID = 0;
        //public int ApprovalUserDeptID = 0;
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Preference.Flow.List).Assembly;	//어셈블리 선언
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
            ddlListCount.SelectedValue = RecordPerPage.ToString();

            // 페이지설정
            #region PageSize 설정하기

            DAL.FolderPageSize dalPageSize = new DAL.FolderPageSize();
            List<BE.FolderPageSizeInfo> al = dalPageSize.GetList(CurrentUserInfo.CompanyID, "Flow", CurrentUserInfo.UserID, "Setting");

            foreach (BE.FolderPageSizeInfo userInfo in al)
            {
                RecordPerPage = userInfo.PageSize;
            }
            #endregion

            GetListCount();
            GetListModifyCount();
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

            }
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 2012-05-09
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            BaseParam();
        }


        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 2012-05-09
        /// 작성목적 : 리스트 총 카운트
        /// </summary>
        private void GetListCount()
        {
            DAL.FavSeq list = new FavSeq();
            TotalItemCount = list.GetFavSeqListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID).ToString();
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 
        /// </summary>
        private void GetListModifyCount()
        {
            //DAL.FavoriteApproval list = new DAL.FavoriteApproval();
            //ModifyCount = list.GetModifyPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
        }

        /// <summary>
        /// 작성자 : 
        /// DDL 페이지 사이즈
        /// </summary>
        private void PageSize()
        {
            DAL.FolderPageSize dal = new DAL.FolderPageSize();

            //Process : 결재상황조회 Complete : 문서함
            List<FolderPageSizeInfo> al = dal.GetList(CurrentUserInfo.CompanyID, "Flow", CurrentUserInfo.UserID, "Setting");

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