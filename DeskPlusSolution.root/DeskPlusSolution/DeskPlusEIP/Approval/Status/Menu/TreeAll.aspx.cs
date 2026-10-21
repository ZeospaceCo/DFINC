using System;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.Web.Status.Menu
{
    public partial class TreeAll : Base.BasePageApproval
    {
        public string AccActive = "0";
        public string FolderStatusID = "";
        public int ProxyUserID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Status.Menu.TreeAll).Assembly;

            if (!IsPostBack)
            {
                LoadParam();

                UCFolderStatusTreeScript1.FolderStatusID = FolderStatusID;
            }
        }

        private void LoadParam()
        {
            AccActive = GetDecryptParam("accActive", "0");
            FolderStatusID = GetDecryptParam("folderStatusID", "");
            ProxyUserID = GetDecryptParam("proxyUserID", "0").ToNumber<int>();
        }
    }
}
