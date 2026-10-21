<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuesSeq.aspx.cs" Inherits="Zeospace.DeskPlus.Opinion.Web.Admin.Content.QuesSeq" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Opinion.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Opinion.css"/>
</head>
<body>
<form id="form1" runat="server">

<div class="layer_popup">
    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("TitF14") %></h1>
            <ul class="btn_right">
                <li ><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnClose" runat="server"></cc3:ClientSmartButton></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">        	    
            <div class="layout_scroll" style="min-height:300px;height:300px;">
                <div class="DD">
                    <ul id="ulWidgets">
                        <%=FolderList %>
                    </ul>
                </div>
            </div>
            <span class="opn_subtxt"><%=GetResource("TitF15") %></span>
        </div>
    </div>
</div>

</form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript">
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang %>";
        var PreLangReal = "<%=PreLangReal %>"; //ja     
        //리소스
        var R_Msg_Success = "<%=GetResource("Msg_Success") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Opinion/Js/Admin/Content/QuesSeq.js"></script>
    <script type="text/javascript">
//        JsCall("/DeskPlusEIP/Opinion/Js/Admin/Content/QuesSeq.js");
    </script>
</body>
</html>
