using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.EIP.Web.Portal
{
    public partial class AddMenu : Base.BasePageEIP
    {
        #region 변수설정
        public int CategoryID;//카테고리 아이디
        public string Modules;//모듈명
        public string IsAdmin;//관리자 여부
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Portal.AddMenu).Assembly;	//어셈블리 선언
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

            UCMenuTreeScript1.CompanyID = CurrentUserInfo.CompanyID;            
            UCMenuTreeScript1.UseIcon = true;           

            IsAdmin = (CurrentUserInfo.IsInRole("admin")) ? "1" : "0";
        }

        #endregion

        #region Method (함수)

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            Modules = GetDecryptParam("modules", "");
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                cbtnClose.Text = this.resMng.GetString("Closed");
                cbtnConfirm.Text = this.resMng.GetString("Confirm");
            }
        }

        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion

        #region Property (프로퍼티)
        #endregion
    }
}