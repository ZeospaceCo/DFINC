<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShortReplyMod.aspx.cs" Inherits="Zeospace.DeskPlus.RM.Web.Content.ShortReplyMod" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8"><title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_RM.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_RM.css"/>    
</head>
<body>
    <form id="form1" runat="server">
    
    <div class="layer_popup">

        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("ModifyShortReply")%></h1>
                (<span class="charsRemaining"></span>) 
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="CbtnConfirm" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
                    <cc3:ClientSmartButton ID="CbtnClose" runat="server" BeginEndTag="li" />
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3">
                <div class="layout_scroll" style="min-height:120px;height:120px;">
                <textarea name="textarea" id="textarea"  class="textarea" runat="server" style="width:97%;" maxlength="200"></textarea>
                </div>
            </div>
        </div>
    </div>

    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>    
    <script type="text/javascript" >
        var ReservationID = "<%=ReservationID %>"; 
        var ParentReservationID = "<%=ParentReservationID%>";
        var ShortReplyID = "<%= ShortReplyID %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_Add_ReplyInputContent = "<%=GetResource("Add_ReplyInputContent") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/RM/Js/Content/ShortReplyMod.js" ></script>
    <script type="text/javascript">
//        JsCall("/DeskPlusEIP/RM/Js/Content/ShortReplyMod.js");
    </script>
</body>
</html>
