using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Approval.DAL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.Web.Preference.Favorite
{
    public partial class List : Base.BasePageApproval
    {
        #region 변수설정
        public string TotalItemCount = String.Empty;  //총카운트
        public int ModifyCount = 0;
        public int ApprovalUserID = 0;
        public int ApprovalUserDeptID = 0;

        public string EventArgs = string.Empty;
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Preference.Favorite.List).Assembly;	//어셈블리 선언
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
            List<BE.FolderPageSizeInfo> al = dalPageSize.GetList(CurrentUserInfo.CompanyID,
                "Favorite", CurrentUserInfo.UserID, "Setting");

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
        /// 작성자 : 이문호
        /// 작성일자 : 2012-05-09
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            BaseParam();
        }


        /// <summary>
        /// 작성자 : 이문호
        /// 작성일자 : 2012-05-09
        /// 작성목적 : 리스트 총 카운트
        /// </summary>
        private void GetListCount()
        {
            DAL.FavoriteApproval list = new DAL.FavoriteApproval();
            TotalItemCount = list.GetPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID).ToString();
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 
        /// </summary>
        private void GetListModifyCount()
        {
            DAL.FavoriteApproval list = new DAL.FavoriteApproval();
            ModifyCount = list.GetModifyPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
        }

        /// <summary>
        /// 작성자 : 이문호
        /// DDL 페이지 사이즈
        /// </summary>
        private void PageSize()
        {
            DAL.FolderPageSize dal = new DAL.FolderPageSize();

            //Process : 결재상황조회 Complete : 문서함
            List<FolderPageSizeInfo> al = dal.GetList(CurrentUserInfo.CompanyID, "Favorite", CurrentUserInfo.UserID, "Setting");

            foreach (FolderPageSizeInfo info in al)
            {
                ddlListCount.SelectedValue = info.PageSize.ToString();
            }
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)

        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 자주지정하는 결재자 추가
        /// 작성자     : 이문호
        /// 최초작성일 : 2012-05-08
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lbtnAdd_Click(object sender, EventArgs e)
        {
            string users = tbxHiddenValue.Text.TrimEnd(',');
            string depts = tbxHdApprovalDeptID.Text.TrimEnd(',');

            try
            {
                if (users.Length > 0)
                {
                    DAL.FavoriteApproval DAL = new Zeospace.DeskPlus.Approval.DAL.FavoriteApproval();

                    BE.FavoriteApprovalInfo be = new FavoriteApprovalInfo();

                    be.CompanyID = CurrentUserInfo.CompanyID;
                    be.UserID = CurrentUserInfo.UserID;
                    be.IsDept = "M";    // 부서(G),사용자(M)
                    be.IsSub = "0";     // 하위부서포함(1),미포함(0)

                    be.ApprovalUserID = ApprovalUserID;
                    be.ApprovalUserDeptID = ApprovalUserDeptID;

                    if (users.IndexOf(',') != -1)
                    {
                        string[] arrUsers = users.Split(',');
                        string[] arrDepts = depts.Split(',');

                        for (int i = 0; i < arrUsers.Length; i++)
                        {
                            be.ApprovalUserID = arrUsers[i].ToString().ToNumber<int>();
                            be.ApprovalUserDeptID = arrDepts[i].ToString().ToNumber<int>();

                            DAL.Add(be);
                        }
                    }
                    else
                    {
                        be.ApprovalUserID = users.ToString().ToNumber<int>();
                        be.ApprovalUserDeptID = depts.ToString().ToNumber<int>();
                        DAL.Add(be);
                    }

                    TotalItemCount = DAL.GetPageListCount(be.CompanyID, be.UserID).ToString();
                }

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", GetResource("MSG_SUCCESS"), "");

                // 자바스크립트 주소 이동하기
                string scripts = @"<script type=\""text/javascript\"">location = '" + Request.Url.ToString() + "'</script>";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "goPage", scripts);  
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("MSG_FAIL"), "");
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnAdd_Click() : ", ex);
            }
        }

        //부서추가
        protected void lbtnAddDept_Click(object sender, EventArgs e)
        {
            if (tbxHdApprovalDeptID.Text.Equals("")) return;        //부서정보는 단일정보로 넘어옴 (체크박스불가)

            try
            {
                int appDeptID = tbxHdApprovalDeptID.Text.Trim().ToNumber<int>();
                string isSub = (EventArgs.Equals("")) ? "0" : EventArgs;

                if (appDeptID > 0)
                {
                    DAL.FavoriteApproval DAL = new Zeospace.DeskPlus.Approval.DAL.FavoriteApproval();

                    BE.FavoriteApprovalInfo be = new FavoriteApprovalInfo();

                    be.CompanyID = CurrentUserInfo.CompanyID;
                    be.UserID = CurrentUserInfo.UserID;
                    be.IsDept = "G";        // 부서(G),사용자(M)
                    be.IsSub = isSub;       // 하위부서포함(1),미포함(0)
                    be.ApprovalUserID = 0;
                    be.ApprovalUserDeptID = appDeptID;

                    DAL.Add(be);

                    TotalItemCount = DAL.GetPageListCount(be.CompanyID, be.UserID).ToString();
                }

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", GetResource("MSG_SUCCESS"), "");

                // 자바스크립트 주소 이동하기
                string scripts = @"<script type=\""text/javascript\"">location = '" + Request.Url.ToString() + "'</script>";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "goPage", scripts);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("MSG_FAIL"), "");
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnAddDept_Click() : ", ex);
            }
        }

        /// <summary>
        /// 들어오는 포스트백 이벤트를 처리하도록 포스트백을 발생시킨 서버 컨트롤에 알립니다.
        /// </summary>
        /// <param name="sourceControl">포스트백을 발생시킨 ASP.NET 서버 컨트롤입니다.이 컨트롤은 <see cref="T:System.Web.UI.IPostBackEventHandler"/> 인터페이스를 구현해야 합니다.</param>
        /// <param name="eventArgument">포스트백 인수입니다.</param>
        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            LoadParam(); //파라미터처리

            EventArgs = eventArgument;
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }
        #endregion
    }
}