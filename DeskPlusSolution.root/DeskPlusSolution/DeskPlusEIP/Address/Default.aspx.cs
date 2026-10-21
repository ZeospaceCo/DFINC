using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Address.Web
{
    public partial class Default : Base.BasePageAddress
    {
        #region 변수설정
        public string ContentURL = "";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            GetContentURL();
        }

        private void GetContentURL()
        {
            string listPage = "./Content/List.aspx";
            string folderType = "GG";
            string realFolderType = "GG";

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("categoryID", "0");
            paraDic.Add("types", folderType);
            paraDic.Add("rTypes", realFolderType);

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            ContentURL = listPage + "?p=" + encParams;
        }
    }
}