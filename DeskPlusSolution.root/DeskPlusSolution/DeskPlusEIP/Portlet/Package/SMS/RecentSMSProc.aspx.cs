using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Zeospace.DeskPlus.Component.SMS;
using Zeospace.DeskPlus.Base.ApplicationFrameworks;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Portlet.Web.Package.SMS
{
    public partial class RecentSMSProc : Base.BasePagePortlet
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }

        }

        private void LoadData()
        {           

            string id = Server.UrlDecode(GetParm("id",""));
            //Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("id : " + id, AppSettingType.Site);

            if (id.Length > 0)
            {
                id = id.TrimEnd('#');

                Component.SMS.RecentPhoneDAL bll = new RecentPhoneDAL();
                Component.SMS.SuremRecentPhoneInfo be = new SuremRecentPhoneInfo();

                be.CompanyID = CurrentUserInfo.CompanyID;
                be.UserID = CurrentUserInfo.UserID;

                if (id.IndexOf('#') != -1)
                {
                    string[] idStr = id.Split('#');
                    for (int i = 0; i < idStr.Length; i++)
                    {
                        if (idStr[i].ToString() != "")
                        {
                            be.SuremRecentPhoneID = Convert.ToInt32(idStr[i]);
                            bll.Del(be);
                        }
                    }
                }
                else
                {
                    be.SuremRecentPhoneID = Convert.ToInt32(id);
                    bll.Del(be);
                }
            }
        }
    }
}
