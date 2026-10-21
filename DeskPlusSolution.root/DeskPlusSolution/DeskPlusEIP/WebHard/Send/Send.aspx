<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Send.aspx.cs" Inherits="Zeospace.DeskPlus.WebHard.Web.Send.Send" %>

<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Common.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Common.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Webhard.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Webhard.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">
        <div class="inside">
        <div class="cmn_out" style="min-height:500px;">
        <div class="cmn_notice" style="min-height:400px;width:400px;">

        <div class="tit"><%=GetResource("Notice")%><img src="<%=BaseImgPath%>/Icon/LoadingA.gif" /></div>
        <div >
         <%=GetResource("ProcessingWait")%><br />
         <span class="subtxtA"><%=GetResource("Send")%></span><br />
         <span class="subtxtB"><%=GetResource("ProcessingDelay")%></span>
        </div>
        </div>
        </div>
        </div>
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript">
        var FolderID = "<%=FolderID%>";
        var FileList = "<%=FileList%>";
        var UID = "<%=UID %>";
        var Order = "<%=Order %>"; //정렬       
        var R_CompleteConfirm = "<%=GetResource("CompleteConfirm") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var ImagePath = "<%=BaseImgPath%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/WebHard/Js/Send/Send.js"></script>    
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/WebHard/Js/Send/Send.js");
    </script>
</body>
</html>
