<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FavoriteOrder.aspx.cs" Inherits="Zeospace.DeskPlus.WebHard.Web.Content.FavoriteOrder" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Webhard.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Webhard.css"/>    
</head>
<body>
<form id="form1" runat="server">

<div class="layer_popup">
    <div class="inside">
        <div class="popup_header">
            <h1><asp:literal ID="ltlTitle" runat="server"></asp:literal></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnClose" runat="server"></cc3:ClientSmartButton></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">
            <div class="layout_scroll" style="min-height:315px;height:315px;">
            <div class="whd_WDD">
                <ul id="ulWidgets">
                    <%=FolderList %>
                </ul>
            </div>
            </div>
        </div>
    </div>
</div>
</form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript">
        var FavoriteID = "<%=FavoriteID %>";
        var R_OrderFolder = "<%=GetResource("OrderFolder") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var R_MoveNoSubFolder = "<%=GetResource("MoveNoSubFolder") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/WebHard/Js/Content/FavoriteOrder.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/WebHard/Js/Content/FavoriteOrder.js");
    </script>
</body>
</html>