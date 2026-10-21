<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Message.aspx.cs" Inherits="Zeospace.DeskPlus.SNS.Web.RealTalk.Message" %>

<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title></title>
    <link href="../Scripts/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.6.4.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.signalR-1.2.2.js" type="text/javascript"></script>
    <script src="../../signalr/hubs"></script>
    <script src="MessengerHub/MessengerHub.js" type="text/javascript"></script>

</head>
<body>
<a href="javascript:sendMessage();">메시지 보내기</a>
</body>
</html>
