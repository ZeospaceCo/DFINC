using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Text;

namespace Zeospace.DeskPlus.Account.Web.Admin.User
{
    public partial class Move : Base.BasePageAccountAdmin
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정        
        public string UserList;
        #endregion

        #region Page Process (페이지 처리)
        #endregion

        #region Method (함수)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Admin.User.Move).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 내용가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2011-04-25
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            UCDeptTreeScript1.CompanyID = CurrentUserInfo.CompanyID;
            UCDeptTreeScript1.DeptID = 0;
            UCDeptTreeScript1.UseCheckBox = false;
            UCDeptTreeScript1.UseActiveDept = false;
            UCDeptTreeScript1.UseIcon = true;
            
        }


        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {            
            UserList = GetDecryptParam("userList", "");
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("BtnConfirm");        //"확인"; //resMng.GetString("Cancel");
                cbtnClose.Text = resMng.GetString("Admin_UserRegister_Close");//"닫기";
            }
        }

        #endregion

        #region Event Handler (이벤트 핸들러)

        #endregion

        #region Property (프로퍼티)

        #endregion
    }
}