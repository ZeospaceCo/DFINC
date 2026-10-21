<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Headers.aspx.cs" Inherits="Zeospace.DeskPlus.Board.Web.Admin.Category.Headers" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("Headers") %></h1>
                <ul class="btn_right">                    
                    <cc3:ClientSmartButton ID="cbtnClose" BeginEndTag="li" runat="server"></cc3:ClientSmartButton>
                </ul>
            </div>
            <div class="dotLine"></div>&nbsp;           
            <div id="placeholder" style="overflow-y:auto;min-height:300px;height:300px;"></div>			
        </div>
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript">
        var CatID = "<%=CatID %>";
        var R_ApplyDelete = "<%=GetResource("ApplyDelete") %>";
        var R_Processing = "<%=GetResource("Processing") %>";        
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
    </script>    
    <script type="text/javascript" src="/DeskPlusEIP/Board/Js/Admin/Category/Headers.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Board/Js/Admin/Category/Headers.js");
    </script>
</body>
</html>
