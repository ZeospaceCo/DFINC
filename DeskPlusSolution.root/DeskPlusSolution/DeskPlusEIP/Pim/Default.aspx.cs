using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeospace.DeskPlus.Pim.Web
{
    public partial class Default : Base.BasePagePim
    {
        public string SelDate = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SelDate = GetParm("p", "");
                
            }
        }
    }
}