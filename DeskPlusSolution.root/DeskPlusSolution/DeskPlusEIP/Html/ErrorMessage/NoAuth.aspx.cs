using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeospace.DeskPlus.Base.Web.Html.ErrorMessage
{
    public partial class NoAuth : Base.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            this.ResourceName = "Zeospace.DeskPlus.Base.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Base.Web.Html.ErrorMessage.NoAuth).Assembly;		//어셈블리 선언
            #endregion

            #region if (!IsPostBack) - 폼필드 / 컨트롤 초기화(다국어처리)
            if (!IsPostBack)
            {
            }
            #endregion
        }
    }
}