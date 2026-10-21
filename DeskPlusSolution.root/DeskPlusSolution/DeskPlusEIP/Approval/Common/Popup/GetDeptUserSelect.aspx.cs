using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.Web.Common.Popup
{
    public partial class GetDeptUserSelect : Base.BasePageApproval
    {
        public string Callback;
        public string Gubun;
        public string IsCheckBox = "1";   // 1,0

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Common.Popup.GetDeptUser).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();
            }

        }

        /// <summary>
        /// 파라미터 정의
        /// </summary>
        private void LoadParam()
        {
            Gubun = GetDecryptParam("gubun", "1");
            Callback = GetDecryptParam("callback", "");
            IsCheckBox = GetDecryptParam("isCheckBox", "1");
        }


        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            bool group = false;
            bool member = false;

            if (Gubun == "1")
            {
                group = true;
            }
            else if (Gubun == "2")
            {
                member = true;
            }
            else
            {
                group = true;
                member = true;
            }

            DeptUserTreeScript1.CompanyID = CurrentUserInfo.CompanyID;
            DeptUserTreeScript1.DeptID = CurrentUserInfo.DeptID;
            DeptUserTreeScript1.UseActiveDept = true;
            DeptUserTreeScript1.UseIcon = true;
            DeptUserTreeScript1.UseMyRootDept = false;
            DeptUserTreeScript1.UseVirtualTree = false;
            DeptUserTreeScript1.UseRootCheckBox = false;

            if (IsCheckBox.Equals("1"))
            {
                DeptUserTreeScript1.UseCheckBoxGroup = group;
                DeptUserTreeScript1.UseCheckBoxMember = member;
            }
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            if (!this.resMng.Equals(null))
            {
                cbtnSubmit.Text = resMng.GetString("Confirm");
                cbtnCancel.Text = resMng.GetString("Close");

                ltlReceiveMan.Text = resMng.GetString("SelectPerson");
                ltlReceiveManSelect.Text = resMng.GetString("PleaseSelectPerson");
                lblSelectedReceiptor.Text = resMng.GetString("Target");
            }
        }
    }
}