using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Address.Web.Admin.Content
{
    public partial class Default : Base.BasePageAddressAdmin
    {
        public int CatID = 0;
        public int CategoryID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            CatID = GetDecryptParam("catID", "0").ToNumber<int>();
            CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
        }
    }
}