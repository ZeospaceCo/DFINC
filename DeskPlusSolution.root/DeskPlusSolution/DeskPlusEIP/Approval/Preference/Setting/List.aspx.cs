using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Account.DAL;
using Zeospace.DeskPlus.Account.BE;

namespace Zeospace.DeskPlus.Approval.Web.Preference.Setting
{
    public partial class List : Base.BasePageApproval
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Preference.Setting.List).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData(int _companyID, int _userID)
        {
            // 승인 시, 비밀번호 사용유무 설정하기 (N/Y)
            Zeospace.DeskPlus.Account.DAL.UserEnvironment dal = new UserEnvironment();
            List<UserEnvironmentInfo> list = dal.GetUserEnv(_companyID, _userID);
            if (list != null)
            {
                foreach (UserEnvironmentInfo info in list)
                {
                    rblUsePwd.SelectedValue = (info.IsApprovePwd.Equals("Y")) ? "Y" : "N";
                    rblApprovalStatusMenuType.SelectedValue = ("NEW".Equals(info.ApprovalStatusMenuType)) ? "NEW" : "OLD";
                }
            }
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
                cbtConfirm.Text = resMng.GetString("Confirm");
            }

            rblUsePwd.Items.Clear();
            rblUsePwd.Items.Add(new ListItem(resMng.GetString("IsActiveTitN"), "N"));
            rblUsePwd.Items.Add(new ListItem(resMng.GetString("IsActiveTitY"), "Y"));
            rblUsePwd.SelectedValue = "N";

            rblApprovalStatusMenuType.Items.Clear();
            rblApprovalStatusMenuType.Items.Add(new ListItem("상황조회(구)", "OLD"));
            rblApprovalStatusMenuType.Items.Add(new ListItem("전자결재(신)", "NEW"));
            rblApprovalStatusMenuType.SelectedValue = "OLD";
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            this.BaseParam();
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)

        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 대결자 추가
        /// 작성자     : 이문호
        /// 최초작성일 : 2012-05-11
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lbtnAdd_Click(object sender, EventArgs e)
        {
            LoadParam();

            

            //string users = tbxHiddenValue.Text.TrimEnd(',');
            //string depts = tbxHiddenDeptValue.Text.TrimEnd(',');
            //try
            //{
            //    if (users.Length > 0)
            //    {
            //        DAL.Proxy DAL = new Zeospace.DeskPlus.Approval.DAL.Proxy();

            //        BE.ProxyInfo be = new ProxyInfo();

            //        be.CompanyID = CurrentUserInfo.CompanyID;
            //        be.UserID = CurrentUserInfo.UserID;
            //        be.DeptID = CurrentUserInfo.DeptID;
            //        be.IsProxy = IsProxy;


            //        if (users.IndexOf(',') != -1)
            //        {

            //            string[] arrUser = users.Split(',');
            //            string[] arrDept = depts.Split(',');

            //            for (int i = 0; i < users.Split(',').Length; i++)
            //            {
            //                be.ProxyUserID = Convert.ToInt32(arrUser[i]);
            //                be.ProxyDeptID = Convert.ToInt32(arrDept[i]);
            //                DAL.Add(be);
            //            }


            //        }
            //        else
            //        {
            //            be.ProxyUserID = Convert.ToInt32(users);
            //            be.ProxyDeptID = Convert.ToInt32(depts);
            //            DAL.Add(be);
            //        }

            //        TotalItemCount = DAL.GetPageListCount(be.CompanyID, be.UserID, be.DeptID).ToString();
            //    }

            //    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", GetResource("MSG_SUCCESS"), "");

            //    // 자바스크립트 주소 이동하기
            //    string scripts = @"<script type=\""text/javascript\"">location = '" + Request.Url.ToString() + "'</script>";
            //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "goPage", scripts);
            //}
            //catch (Exception ex)
            //{
            //    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("MSG_FAIL"), "");
            //    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnAdd_Click() : ", ex);
            //}
        }
        #endregion
    }
}
