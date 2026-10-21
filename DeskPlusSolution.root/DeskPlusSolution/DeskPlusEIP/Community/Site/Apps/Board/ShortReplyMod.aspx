<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShortReplyMod.aspx.cs" Inherits="Zeospace.DeskPlus.Community.Web.Site.Apps.Board.ShortReplyMod" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Community.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Community.css"/>        
</head>
<body>
    <form id="form1" runat="server">    
    <div class="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("ModShortReply")%></h1>
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="CbtnConfirm" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
                    <cc3:ClientSmartButton ID="CbtnClose" runat="server" BeginEndTag="li" />
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3">
                <div class="layout_scroll" style="min-height:120px;height:120px;">
                <textarea name="textarea" id="textarea"  class="textarea" runat="server" style="width:97%;"></textarea>
                </div>
            </div>
        </div>
    </div>

    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>    
    <script type="text/javascript" >
    /**********************************************************************************************
    전역 변수들
    **********************************************************************************************/
        var ShortReplyID = "<%=ShortReplyID %>";
        var CommunityID = "<%=CommunityID %>";
        var MenuID = "<%=MenuID %>";
        var BoardID = "<%=BoardID %>";
        var R_ReplyInputContent = "<%=GetResource("ReplyInputContent") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Community/Js/Site/Apps/Board/ShortReplyMod.js" ></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Community/Js/Site/Apps/Board/ShortReplyMod.js");
    </script>
</body>
</html>

