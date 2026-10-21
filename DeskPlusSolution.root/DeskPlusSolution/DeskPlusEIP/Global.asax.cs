using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Microsoft.AspNet.SignalR;

namespace Zeospace.DeskPlus.Base.Web
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            RouteTable.Routes.MapHubs();

            Zeospace.DeskPlus.Component.Converter.AsposeLicense license = new Component.Converter.AsposeLicense();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            string useSession = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "UseSession", "N");
            if (useSession == "Y")
            {
                if (Session.IsNewSession)
                {
                    if ((Request.Headers["Cookie"] != null) && (Request.Headers["Cookie"].IndexOf(FormsAuthentication.FormsCookieName) >= 0))
                    {
                        FormsAuthentication.SignOut();
                        Context.User = null;
                        Response.Redirect("/DeskPlusEIP/Default.aspx");
                    }
                }
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.LogFile("Global.asax.cs[Application_Error] : " + 
                Server.GetLastError().Message.ToString(), "/DeskPlusEIP/Data/Logs/Exception", "exception");

            Server.ClearError();
        }


        

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}