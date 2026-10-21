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
    public partial class GetDept : Base.BasePageApproval
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public int DeptID;
        public string DeptNm;
        public string Callback;
        public string IsCheckBox = "1";     // 1,0
        public string IsActive = "1";  // 1,0
        public string IsSub = "1";
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Common.Popup.GetDept).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region Method (함수)
        private void LoadParam()
        {
            DeptID = GetDecryptParam("deptID", "0").ToNumber<int>();
            DeptNm = GetDecryptParam("deptNm", "");
            Callback = GetDecryptParam("callback", "");
            IsCheckBox = GetDecryptParam("isCheckBox", "1");
            IsActive = GetDecryptParam("isactive", "1");
            IsSub = GetDecryptParam("issub", "1");
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnSubmit.Text = this.GetResource("Confirm");
                cbtnCancel.Text = this.GetResource("Close");
            }
        }

        private void LoadData()
        {
            LoadParam();

            UCDeptTreeScript1.CompanyID = CurrentUserInfo.CompanyID;
            UCDeptTreeScript1.DeptID = DeptID;
            UCDeptTreeScript1.UseCheckBox = (IsCheckBox.Equals("1")) ? true : false;
            UCDeptTreeScript1.UseActiveDept = (IsActive.Equals("1")) ? true : false;
        }
        #endregion

        #region Event Handler (이벤트 핸들러)

        #endregion

        #region Property (프로퍼티)

        #endregion
    }
}