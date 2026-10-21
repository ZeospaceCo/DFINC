using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Portlet.Web.BoardPopup
{
    public partial class BoardPopUp : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BasePage
    {
        public string Popid = string.Empty;
        public string Url = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.BoardPopup.BoardPopUp).Assembly;	//어셈블리 선언

            if (!IsPostBack)
            {

                if (GetParm("popid", "") != "")
                {
                    LoadData();
                }
            }
        }

        private void LoadData()
        {
            string categoryID = string.Empty;
            string listID = string.Empty;

            Popid = GetParm("popid", "");

            if (Popid.Length > 0)
            {
                categoryID = Popid.Split('_')[0];
                listID = Popid.Split('_')[1];
            }

            Dictionary<string, string> paraDic = new Dictionary<string, string>();

            paraDic.Add("categoryID", categoryID);
            paraDic.Add("listID", listID);
            //paraDic.Add("goURL", "/DeskplusEIP/Board/Content/ViewPopup.aspx");

            string encParams = "p=" + GetEncryptParam(Utility.GetParameterDic(paraDic));
            Url = "/DeskPlusEIP/Board/Content/ViewPopup.aspx?" + encParams;
        }
    }
}