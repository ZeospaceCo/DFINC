using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeospace.DeskPlus.HR.Web.Admin.Menu
{
    public partial class Tree : Base.BasePageHRAdmin
    {
        #region 변수설정
        public string AccActive = "0"; //accordianActive
        public string MenuID = "";
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
            this.ResourceName = "Zeospace.DeskPlus.HR.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.HR.Web.Admin.Menu.Tree).Assembly;	        //어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
            }
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 2011-10-18
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            AccActive = GetDecryptParam("accActive", "0");
            MenuID = GetDecryptParam("menuID", "");
        }
        #endregion


        #region Property (프로퍼티)

        #endregion
    }
}