<%@ Page language="c#" AutoEventWireup="true" Inherits="System.Web.UI.Page" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="ComponentArt.Web.UI" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Web.Script.Serialization" %>
<%@ Import Namespace="System.Net" %>
<%@ Import Namespace="System.IO" %>


<script language="C#" runat="server">
    
	private void Page_Load(object sender, System.EventArgs e)
	{
	}    


        [ComponentArtCallbackMethod]
        public string LoadData(string _regId)
        {
            //string SERVER_API_KEY = "AAAAXDcc0Ig:APA91bHBgJhwuz21ppok0i7Z9KsNBnrWb3-YuN6EB5qQuMm8Rx7UNu09ratu4H-ztXvvZS7o3bSrpfPrb0csp1QLvdc0qw0jpxV_anmSciYzsLe0-gUtVWJ8kUxAtE2_yLilgbr7i2Th";
            string SERVER_API_KEY = "AAAAJkou4Ig:APA91bE63pFdO03DLSdy4NWrMeFHNNyk2Ww4yYWXCBfhPPIvEMTQ4Qfr29vHxFEDxUqHU0XRorweNkFbXwUl_xoRMYiRmQezO7TZK1ibPc10vhGWhV0O29HDoF0djEkZQ7MwriqCZSaN";

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
                    url = "http://m.zeospace.com/deskplusmobile/portal/anlogin.aspx",
                    badge = "22",
                },

                //to = "deviceId"
                to = _regId              
                
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
            }       

            string logFileLocation = "/DeskPlusMobile/Data/Logs/Exception";

            if (logFileLocation.StartsWith("/"))
            {
                HttpContext context = HttpContext.Current;
                logFileLocation = context.Server.MapPath(logFileLocation);
            }

            LogFile(resultStr, logFileLocation, "Mobile");

           return resultStr;
        }
	
        /// <summary>
        /// M : 파일에 로그/에러/경고를 쓴다
        /// </summary>
        /// <param name="_message">메시지</param>
        /// <param name="_fileLocation">로그파일 위치</param>
        /// <param name="_prefix">구분자</param>
        public static void LogFile(string _message, string _fileLocation, string _prefix)
        {
            if (!_fileLocation.EndsWith("\\"))
            {
                _fileLocation += "\\";
            }

            if (!Directory.Exists(_fileLocation)) Directory.CreateDirectory(_fileLocation);

            if (Directory.Exists(_fileLocation))
            {
                string nowYear = DateTime.Now.Year.ToString();
                string nowMon = DateTime.Now.ToString("MM");
                string nowDay = DateTime.Now.ToString("dd");
                string nowHour = DateTime.Now.ToString("HH");

                StreamWriter stream = null;
                try
                {
                    stream = new StreamWriter(_fileLocation + _prefix + nowYear + nowMon + nowDay + nowHour + ".log", true, Encoding.Default);

                    stream.WriteLine("▶▶-------------AppExceptioin--------------");
                    stream.WriteLine("▶▶Time : " + DateTime.Now.ToString("HH") + ":" + DateTime.Now.ToString("mm") + ":" + DateTime.Now.ToString("ss"));
                    stream.WriteLine(_message);
                    stream.WriteLine("▶▶UserID : " + HttpContext.Current.User.Identity.Name);
                }
                catch { }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }
            }
        }

    
</script>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
<title></title>
<script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.js"></script>
<script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.common.js"></script>   
<script language="javascript">

    function LoginProcess() {
        var tmp = $("#tbRegID").val();

        if (tmp != "") {
            var rtn = LoadData(tmp);
            var parsedJson = JSON.parse(rtn);
            if (parsedJson.success == "1") {
                alert("정상발송되었습니다");
            }
            else {
                alert("발송에 실패했습니다. regID 값을 확인 부탁드립니다");
            }
        }
        else {
            alert("RegID 값을 입력하세요");
        }
    }
</script>
</head>
<body>    
     <form id="form1" runat="server">
     RegID : <asp:TextBox ID="tbRegID" TabIndex="1" class="form-control" style="width:1000px" runat="server"></asp:TextBox>
     <input type="submit" value="발송" id="submit" class="btn btn-lg btn-primary btn-block" style="margin-bottom:10px;" onclick="return LoginProcess();" />
	 <Br>
	 <div id="txtMessage">&nbsp;</div>
     </form>
</body>
</html>
