<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tree.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Admin.Menu.Tree" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_EIP.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_EIP.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var UserID = "<%=CurrentUserInfo.UserID %>";
        var MenuID = "<%=MenuID %>";
        var IconYN = "<%=IconYN %>";

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";

        var R_ProcessingApply = "<%=GetResource("ProcessingApply") %>";
        var R_ProcessingErrorApply = "<%=GetResource("ProcessingErrorApply") %>";
        var R_ConfirmDeleteMenu = "<%=GetResource("ConfirmDeleteMenu") %>";
        var R_ExistSubMenu = "<%=GetResource("ExistSubMenu") %>";
        var R_NotConfirmUserAccessAuth = "<%=GetResource("NotConfirmUserAccessAuth") %>";
        var R_ErrorProcessing = "<%=GetResource("ErrorProcessing") %>";
        var R_MenuSetting1 = "<%=GetResource("MenuSetting1") %>";
        var R_MenuSetting2 = "<%=GetResource("MenuSetting2") %>";
    </script>
    <script type="text/javascript" src="../../Js/Admin/Menu/Tree.js"></script>
    <script type="text/javascript">
        //JsCall("../../Js/Admin/Menu/Tree.js");
    </script>
</head>
<body>
<form id="form1" runat="server">
<div id="layout_left" class="left-menu admin">
    <ul class="left-menu-container">
    <li>
    <div id="layout_treetop"><span class="treetop_title"><%=GetResource("MenuManagement")%></span> </div>

    <div id="layout_treeboxAcc" >
        <div class="accordion" >
            <div class="menu" style="display:none"><%=GetResource("MenuManagement")%></div>
            <div class="menusub">
                <zeoMenuTree:UCMenuTreeScript ID="UCMenuTreeScript1" runat="server" UseIcon="true" SetCallbackComplete="onCallbackComplete" SetNodeSelect="MenuNodeSelect_onNodeSelect" />
            </div>
        </div>
    </div>

    <div id="layout_treebuttom"></div>
    <div id="layout_treebutton">  
        <a id="cbtnApply" style="cursor: pointer" class="btn btn-sm btn-secondary"><%=GetResource("Apply")%></a>
        <a id="cbtnAdd" style="cursor: pointer" class="btn btn-sm"><%=GetResource("Add")%></a>  
        <a id="cbtnDel" style="cursor: pointer" class="btn btn-sm btn-danger"><%=GetResource("Del")%></a>
        <a id="cbtnSetting" style="cursor: pointer" class="btn btn-sm"><%=GetResource("MenuSetting")%></a>
    </div>
    </li>
    </ul>
</div>
</form>
</body>
</html>
