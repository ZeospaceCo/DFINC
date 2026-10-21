using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeospace.DeskPlus.Approval.Web.Disuse
{
    public partial class Tree : Base.BasePageApprovalAdmin
    {
        #region Page Process (페이지 처리)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Disuse.Tree).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
            }
        }
        #endregion

        #region (함수)
        #endregion

        #region Property (프로퍼티)        
        #endregion
    }
}