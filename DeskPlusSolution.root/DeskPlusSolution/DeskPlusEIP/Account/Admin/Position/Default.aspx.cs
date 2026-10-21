using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Account.Web.Base;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Account.Web.Admin.Position
{
    public partial class Default : BasePageAccountAdmin
    {

        #region 변수설정
        public string LeftURL = "";
        public string ContentURL = "";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            LeftURL = "Tree.aspx";

            //ContentURL
            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("codeID", "1");
            paraDic.Add("scope", "T");
            paraDic.Add("gubun", "mod");

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            ContentURL = "List.aspx?p=" + encParams;
        }


    }
}