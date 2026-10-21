using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Text;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Pim.Web.Help
{
    public partial class GetCategoryInfo : Base.BasePagePim
    {
        #region 변수설정
        public int CompanyID = 0;
        public int UserID = 0;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Help.GetCategoryInfo).Assembly;	//어셈블리 선언
            #endregion



            if (!IsPostBack)
            {
                #region 이미지 처리

                #endregion

                #region 초기화
                SetLanguage();
                DataLoad();

                CompanyID = CurrentUserInfo.CompanyID;
                UserID = CurrentUserInfo.UserID;
                #endregion
            }
        }

        #endregion

        #region Method (함수)
        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnClose.Text = GetResource("Close");
            }
        }


        private void DataLoad()
        {

        }

        #endregion

        #region Event Handler (이벤트 핸들러)

        #endregion

        #region Property (프로퍼티)

        #endregion
    }
}