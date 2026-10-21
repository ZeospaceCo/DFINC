<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OpenContentSMS.aspx.cs"
    Inherits="Zeospace.DeskPlus.Portlet.Web.Package.SMS.OpenContentSMS" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <style type="text/css">
    <!--
    @import url("Css/style.css");
    -->
    </style>
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript">
        var SelectInputSendNumber = "입력할 전송번호를 선택해주세요";
        var SelectDeleteSendNumber = "삭제할 전송번호를 선택해주세요";
        var ConfirmDel = "삭제하시겠습니까?";
    </script>
    <script type="text/javascript">        
        JsCall("/DeskPlusEIP/Portlet/Js/Package/SMS/openContent.js");
        JsCall("/DeskPlusEIP/Portlet/Js/Package/SMS/common.js");
    </script>
</head>
<body topmargin="0" leftmargin="0">
    <form id="rf" name="rf" target="rf_iframe" method="post" runat="server">
    <div style="width: 137px; height: 100px; overflow: auto; overflow-x: hidden; overflow-y: auto;"
        id="scroll_03">
        <table cellspacing="0" cellpadding="0" width="160">
            <input type="hidden" value="" name="openContentidx" id="openContentidx">
            <input type="hidden" value="" name="tr_idx" id="tr_idx">
            <input type="hidden" value="" name="content" id="content">
            <%=htmlString %>
        </table>
    </div>
    </form>
</body>
</html>
