using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Account.DAL;
using Zeospace.DeskPlus.Account.BE;

namespace Zeospace.DeskPlus.Common.Web.Mail.Deep
{
    public partial class DeepWrite : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string mailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "");
            if (mailProduct.ToUpper().IndexOf("DEEP").Equals(0))
            {
                Zeospace.DeskPlus.Account.DAL.User biz = new Account.DAL.User();
                List<UserInfo> al = biz.GetUserInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, true, true);

                string MailPassword = "";
                foreach (UserInfo info in al)
                {
                    MailPassword = (info.MailPassword.Equals("")) ? info.Password : info.MailPassword;
                }

                string to = GetDecryptParam("to", "");

                Zeospace.DeskPlus.Common.BLL.MailDeepFactory bll = new BLL.MailDeepFactory();
                if (bll.AddUser(CurrentUserInfo.CompanyID, CurrentUserInfo.MailDomain, CurrentUserInfo.MailID, MailPassword, CurrentUserInfo.Name, "",""))
                {
                    string path = bll.GetMailWriteUrl(CurrentUserInfo.CompanyID, CurrentUserInfo.MailDomain, CurrentUserInfo.MailID, MailPassword, PreLang, "", to, "");
                    Response.Redirect(path);
                }
            }
        }
    }
}