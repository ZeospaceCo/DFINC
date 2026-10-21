<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sender.aspx.cs" Inherits="Zeospace.DeskPlus.SNS.Web.RealTalk.Sender" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta charset="UTF-8">
    <title></title>
    <script src="../Scripts/jquery-1.6.4.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.signalR-1.2.2.js" type="text/javascript"></script>
    <script src="../../signalr/hubs"></script>
    <script type="text/javascript">

        function message() {

            var messenger = $.connection.messenger;

            messenger.sever.broadCastMessage({Title: 'test', Content : 'aaaa', Duration : 500}, 'sourcing').done(function () {
                console.log ('Invocation of NewContosoChatMessage succeeded');
            }).fail(function (error) {
                console.log('Invocation of NewContosoChatMessage failed. Error: ' + error);
            });
        }

        message();
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
