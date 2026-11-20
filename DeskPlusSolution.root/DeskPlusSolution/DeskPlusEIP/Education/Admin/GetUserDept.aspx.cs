using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Education.Web.Admin
{
    public partial class GetUserDept : Base.BasePageEducationAdmin
    {
        #region 변수처리
        public string CallBackEvent = "";
        public bool SingleDepartment = false;
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Education.Web.Resource.GlobalString.string";
            this.AssemblyName = typeof(Zeospace.DeskPlus.Education.Web.Admin.GetUserDept).Assembly;	
            #endregion

            if (!IsPostBack)
            {
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
            LoadParam();

            DeptUserTreeScript1.CompanyID = CurrentUserInfo.CompanyID;
            DeptUserTreeScript1.DeptID = CurrentUserInfo.DeptID;
            DeptUserTreeScript1.UseActiveDept = true;
            DeptUserTreeScript1.UseIcon = true;
            DeptUserTreeScript1.UseMyRootDept = false;
            DeptUserTreeScript1.UseVirtualTree = false;
            DeptUserTreeScript1.UseRootCheckBox = false;
            // 검색 팝업에서는 선택한 대상 종류만 체크할 수 있습니다.
            // 등록 화면처럼 대상 종류를 전달하지 않으면 기존 사용자/부서 선택을 유지합니다.
            string targetType = GetDecryptParam("targetType", "");
            DeptUserTreeScript1.UseCheckBoxGroup = targetType != "M";
            DeptUserTreeScript1.UseCheckBoxMember = targetType != "G";
        }


        #endregion

        #region (함수)

        /// <summary>
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnConfirm.Text = "확인";
                CbtnClose.Text = "닫기";
                CbtnAdd.Text = "추가";
            }  
        }

        /// <summary>
        /// </summary>
        private void LoadParam()
        {
            CallBackEvent = GetDecryptParam("callBackEvent", "0");
            SingleDepartment = GetDecryptParam("singleDepartment", "") == "1";
        }

        #endregion
    }
}
