using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.EIP.Web.Menu
{
    public partial class FaMenu : Base.BasePageEIP
    {
        #region 변수설정
        public int FolderID;
        public string Types;
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.EIP.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Menu.FaMenu).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        #endregion

        #region (함수)

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Confirm");       //"적용";
                cbtnClose.Text = resMng.GetString("Closed");         //"닫기";
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2018-10-18
        /// 작성목적 : 데이터 가져오기
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            UCUserMenuTreeScript1.CompanyID = CurrentUserInfo.CompanyID;
            UCUserMenuTreeScript1.UseIcon = true;
            UCUserMenuTreeScript1.MenuID = 0;
        }

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
        }

        #endregion

        #region (속성)

        #endregion
    }
}