<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShortReply.aspx.cs" Inherits="Zeospace.DeskPlus.Report.Web.Content.ShortReply" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>
        <%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("ShortRegister")%></h1>
                (<span class="charsRemaining"></span>)
                <ul class="btn_right">
                    <li>
                        <cc3:ClientSmartButton ID="CbtnConfirm" runat="server"  class="btn btn-secondary" />
                    </li>
                    <li>
                        <cc3:ClientSmartButton ID="CbtnClose" runat="server" />
                    </li>
                </ul>
            </div>
            <div class="dotLine">
            </div>
            <div class="popup_layout_style3">
                <div class="layout_scroll" style="min-height: 120px; height: 130px;">
                    <textarea name="textarea" id="textarea" class="textarea" style="width: 97%;" maxlength="200"></textarea>
                </div>
            </div>
        </div>
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript">
        var ListID = "<%= ListID %>";
        var Gubun = "<%= Gubun %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_Add_ReplyInputContent = "<%=GetResource("Add_ReplyInputContent") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Report/Js/Content/ShortReply.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Report/Js/Content/ShortReply.js");
    </script>
</body>
</html>