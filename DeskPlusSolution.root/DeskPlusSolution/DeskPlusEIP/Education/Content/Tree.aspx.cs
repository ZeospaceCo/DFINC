using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Education.Web.Content
{
    public partial class Tree : Base.BasePageEducation
    {
        #region 변수설정
        public string p_AccActive = "0";
        public bool isRoleEducation = false;
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
            this.ResourceName = "Zeospace.DeskPlus.Education.Web.Resource.GlobalString.string";
            this.AssemblyName = typeof(Zeospace.DeskPlus.Education.Web.Content.Tree).Assembly;	           
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadParam();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            isRoleEducation = CurrentUserInfo.IsInRole("Education");

            if (AccessAuth)
            {
                MenuDash.Visible = true;
                MenuList.Visible = true;
            }
            else
            {
                MenuDash.Visible = false;
                MenuList.Visible = false;
            }         
        }    
      
        #endregion

        #region (함수)

        /// <summary>
        /// 
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
            }
        }

        /// <summary>
        /// </summary>
        private void LoadParam()
        {
            p_AccActive = GetDecryptParam("accActive", "0");
        }

        #endregion
    }
}