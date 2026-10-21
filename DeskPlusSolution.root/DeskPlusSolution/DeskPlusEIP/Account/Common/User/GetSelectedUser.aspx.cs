using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Account.Web.Common.User
{
    public partial class GetSelectedUser : Base.BasePageAccount
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public int CompanyID;
        public string ListID = String.Empty;
        public string CallBackPage = string.Empty;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Common.User.GetSelectedUser).Assembly;	//어셈블리 선언
            #endregion


            if (!IsPostBack)
            {
                #region 리소스 처리
                this.GetResourceManager();
                if (!this.resMng.Equals(null))
                {
                    CbtnSubmit.Text = resMng.GetString("Admin_UserManager_cbsUserConfirm");      // 확인
                    CbtnClose.Text = resMng.GetString("Admin_UserRegister_Close");         // 닫기
                    CbtnAdd.Text = resMng.GetString("Admin_CompanyManager_btnAdd");             // 추가
                    CbtnAddUser.Text = resMng.GetString("UserInfoManage");//"사용자추가";
                    CbtnAddDept.Text = resMng.GetString("ManagementDept");//"부서추가";
                }
                #endregion

                #region 초기화
                LoadData();
                #endregion
            }
        }

        /// <summary>
        /// 작성자 : shine
        /// Gets the param.
        /// </summary>
        private void GetParam()
        {
            CompanyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            ListID = GetDecryptParam("listID", "");
            CallBackPage = GetDecryptParam("callBackPage", ""); 
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        private void LoadData()
        {
            GetParam();

            DeptUserTreeScript1.CompanyID = CompanyID;
            DeptUserTreeScript1.DeptID = 0;
            DeptUserTreeScript1.UseActiveDept = true;
            DeptUserTreeScript1.UseCheckBoxMember = true;
            DeptUserTreeScript1.UseIcon = true;
            DeptUserTreeScript1.UseCheckBoxGroup = false;            
            DeptUserTreeScript1.UseMyRootDept = false;
            DeptUserTreeScript1.UseVirtualTree = false;
            DeptUserTreeScript1.UseRootCheckBox = false;
        }
        #endregion

        #region (함수)

        
        #endregion

        #region (프로퍼티)

        #endregion


        #region (이벤트핸들러)

        #endregion
    }
}