<%@ Page language="c#" AutoEventWireup="true" Inherits="Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="ComponentArt.Web.UI" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="Zeospace.DeskPlusMobile.Web.Helper" %>
<%@ Import Namespace="Zeospace.DeskPlusMobile.Web.Helper.WebComponent" %>
<%@ Import Namespace="System.Web.Script.Serialization" %>
<%@ Import Namespace="System.Net" %>
<%@ Import Namespace="System.IO" %>


<script language="C#" runat="server">
    
	private void Page_Load(object sender, System.EventArgs e)
	{
        if (!IsPostBack)
        {
             LoadData();
        }
	}    


     /// <summary>
        /// 
        /// </summary>
        public void LoadData()
        {
            string SERVER_API_KEY = "AAAAXDcc0Ig:APA91bHBgJhwuz21ppok0i7Z9KsNBnrWb3-YuN6EB5qQuMm8Rx7UNu09ratu4H-ztXvvZS7o3bSrpfPrb0csp1QLvdc0qw0jpxV_anmSciYzsLe0-gUtVWJ8kUxAtE2_yLilgbr7i2Th";

            string resultStr = "";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add(string.Format("Authorization: key={0}", SERVER_API_KEY));

            var postData =
            new
            {
                data = new
                {
                    title = "제목",
                    message = "내용",
                    arg1 = "",
                    arg2 = "0",
                    arg3 = "",
                    url = "http://msp.deskplus.co.kr/deskplusmobile/portal/portal.aspx",
                    badge = "22",
                },

                //to = "deviceId"
                to = "cnsEBvg4AjM:APA91bFWjaAO--RsnLQLeECf6Lp1RwskG_A4ePyoD2QNsFPEeCBgmhVgqXoT-KfXHLP0kobImzThrot8Jn8QImcB4tOkypjAB9GXFcDvcdVc6jMwULGQWg8KSA7YTlmmmxFoQBzZlCA2"                
                
            };

            //Linq to json
            //string contentMsg = JsonConvert.SerializeObject(postData);
            var serializer = new JavaScriptSerializer();
            var contentMsg = serializer.Serialize(postData);


            Byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(contentMsg);
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                resultStr = reader.ReadToEnd();
                reader.Close();
                responseStream.Close();
                response.Close();
            }
            catch (Exception e)
            {
                resultStr = "";
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("error", e);
            }

            Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("resultStr : " + resultStr);            

        }
	
    
    
</script>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
<title></title>
</head>
<body>    
</body>
</html>
