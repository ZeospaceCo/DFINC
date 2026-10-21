using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Address.Web.Admin.Folder
{
    public partial class Default : Base.BasePageAddressAdmin
    {
        public int CatID = 0;
        public int CategoryID = 0;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetParam();
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// Gets the param.
        /// </summary>
        private void GetParam()
        {
            CatID = GetDecryptParam("catID", "0").ToNumber<int>();
            CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
        }
    }
}