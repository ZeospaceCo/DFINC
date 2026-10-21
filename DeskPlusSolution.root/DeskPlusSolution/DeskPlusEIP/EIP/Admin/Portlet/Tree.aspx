<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tree.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Admin.Portlet.Tree" %>
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
        var CategoryID = "<%=CategoryID %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_ConfirmDelete = "<%=GetResource("ConfirmDelete") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
    </script>
    <script type="text/javascript" src="../../Js/Admin/Portlet/Tree.js"></script>
    <script type="text/javascript">
        //JsCall("../../Js/Admin/Portlet/Tree.js");
    </script>
</head>
<body>
    <div id="layout_left" class="left-menu admin">
        <ul class="left-menu-container">
        <li>
        <div id="layout_treetop">
            <span class="treetop_title"><%=GetResource("PortletManagement")%></span>
        </div>

        <div id="layout_treeboxAcc" >
            <div class="accordion" >
                <div class="menu" style="display:none"><%=GetResource("PortletManagement")%></div>
                <div class="menusub">
                    <zeoPortletTree:UCPortletTreeScript ID="UCUCPortletTreeScript1" runat="server" UseIcon="true" UseActive="false" SetCallbackComplete="onCallbackComplete" SetNodeSelect="PortletNodeSelect_onNodeSelect" />
                </div>
            </div>
        </div>

        <div id="layout_treebuttom"></div>
        <div id="layout_treebutton">
            <a id="cbtnAdd" style="cursor: pointer" class="btn btn-sm"><%=GetResource("Add")%></a>
            <a id="cbtnMod" style="cursor: pointer" class="btn btn-sm"><%=GetResource("Mod")%></a>  
            <a id="cbtnDel" style="cursor: pointer" class="btn btn-sm btn-danger"><%=GetResource("Del")%></a>          
        </div>
        </li>
        </ul>   
    </div>
</body>
</html>
