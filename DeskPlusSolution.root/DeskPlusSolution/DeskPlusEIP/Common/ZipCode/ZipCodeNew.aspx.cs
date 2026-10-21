using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeospace.DeskPlus.Common.Web.ZipCode
{
    public partial class ZipCodeNew : Base.BasePageCommon
    {
        public string CallBackPage = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Common.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Common.Web.ZipCode.ZipCodeNew).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                CallBackPage = GetDecryptParam("callBackPage", "");

                this.GetResourceManager();

                if (!this.resMng.Equals(null))
                {
                    cbtnSearch.Text = resMng.GetString("Find");
                    cbtnClose.Text = resMng.GetString("Close");
                }
            }
        }
    }
}