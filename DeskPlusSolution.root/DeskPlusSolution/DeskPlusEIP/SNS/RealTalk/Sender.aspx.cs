using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Hosting;
using Microsoft.AspNet.SignalR.Client.Hubs;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.SNS.Web.RealTalk
{
    public partial class Sender : Base.BasePageRealTalk
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
            //string serverName = baseHttp + AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
            //var connection = new HubConnection(serverName) + "/DeskPlusEIP/";
            ////var connection = new HubConnection("http://localhost:9616/");
            //MessengerHub.Message message = new MessengerHub.Message();
            //message.Content = "test";
            //message.Duration = 500;
            //message.Title = "test Title";
            //var myHub = connection.CreateHubProxy("messenger");
            //connection.Start().Wait();
            //Object[] myData = { (Object)message, "sourcing" };
            //myHub.Invoke("broadCastMessage", myData);
        }
    }
}