using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.EIP.BE;

namespace Zeospace.DeskPlus.EIP.Web.Admin.Auth
{
    public partial class Add : Base.BasePageEIPAdmin
    {
        #region 변수설정        
        public int RoleID;
        public string Gubun;
        public string HeaderTitle;
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
            this.ResourceName = "Zeospace.DeskPlus.EIP.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Admin.Auth.Add).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                
                LoadData();
            }
        }

        /// <summary>
        /// 초기화
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            SetLanguage();


            if (RoleID > 0)
            {
                DAL.Role dal = new DAL.Role();
                List<RoleInfo>  al = dal.GetRole(CurrentUserInfo.CompanyID, RoleID, "1");
                foreach (RoleInfo info in al)
                {
                    ctbRoleCode.Text = info.RoleCode.ToReverseXSS();
                    ctbRoleName.Text = info.RoleName.ToReverseXSS();
                    ctbRoleDesc.Text = info.RoleDesc.ToReverseXSS();
                }
            }
        }

        #endregion

        #region Method (함수)

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            RoleID = GetDecryptParam("roleID", "-1").ToNumber<int>();
            Gubun = GetDecryptParam("gubun", "1");
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnAdd.Text = resMng.GetString("Save");
                cbtnMove.Text = resMng.GetString("Orders");

                HeaderTitle = (Gubun == "1") ? resMng.GetString("AuthManage") : resMng.GetString("LevelManage");
            }
        }


        

        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion

        #region Property (프로퍼티)
        #endregion
    }
}