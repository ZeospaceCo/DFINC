<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShortReply.aspx.cs" Inherits="Zeospace.DeskPlus.Opinion.Web.Folder.ShortReply" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Opinion.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Opinion.css"/>
</head>
<body>
    <form id="form1" runat="server">


<div class="layer_popup">
    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("WriteShortReply")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="CbtnConfirm" runat="server"  Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="CbtnClose" runat="server"></cc3:ClientSmartButton></li>
            </ul>            
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">
            <div class="layout_scroll" style="min-height:120px;height:150px;">
                <textarea name="textarea" id="txtCont"  class="textarea" style="width:97%;" runat="server" ></textarea>
            </div>
        </div>                
    </div>
</div><!-- //layout_info --> 


    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>    
    <script type="text/javascript">
        var QuesID = "<%= QuesID %>";
        var ReplyID = "<%= ReplyID %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_Add_ReplyInputContent = "<%=GetResource("Add_ReplyInputContent") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Opinion/Js/Folder/ShortReply.js"></script>     
    <script type="text/javascript">
//        JsCall("/DeskPlusEIP/Opinion/Js/Folder/ShortReply.js");
    </script> 
</body>
</html>
