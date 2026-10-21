using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Account.Web.Common
{
    public partial class AccountInfoMessenger : Base.BasePageAccount
    {
        public string url = "";
        /// <summary>
        /// 메신저에서 나의정보를 선택했을 경우
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {
                int userID = GetParm("uID", "0").ToNumber<int>();

                if (userID > 0)
                {
                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("userID", userID.ToString());
                    paraDic.Add("enableClose", "false");

                    string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                    url = "/DeskPlusEIP/Account/Common/AccountInfo.aspx?p=" + encParams;

                    Response.Redirect(url, false);
                }
            }
        }
    }
}