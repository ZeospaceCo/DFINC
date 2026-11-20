using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
namespace Zeospace.DeskPlus.Portlet.Web.Package
{
    public partial class Education : Zeospace.DeskPlus.Portlet.Web.Base.BaseControlPortlet
    {
        #region 변수설정
        public int EducationTotalCount;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Package.Education).Assembly;
            #endregion

            if (!IsPostBack)
            {
                #region 이미지 처리
                #endregion

                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region Method (함수)

        /// <summary>
        /// Sets the language.
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        public void LoadData()
        {
            GetListCount();
        }

        /// <summary>
        /// 카운트 가져오기
        /// </summary>
        private void GetListCount()
        {
            Zeospace.DeskPlus.Portlet.DAL.Education biz = new DAL.Education();
            EducationTotalCount = biz.GetPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
        }

        #endregion
    }
}