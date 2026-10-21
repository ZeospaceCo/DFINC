<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tree.aspx.cs" Inherits="Zeospace.DeskPlus.MDM.Web.MenuAuth.Tree" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_MDM.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_MDM.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var UserID = "<%=CurrentUserInfo.UserID %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var MenuID = "";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/MDM/Js/MenuAuth/Tree.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/MDM/Js/MenuAuth/Tree.js");
    </script>
</head>
<body>
<form id="form1" runat="server">
<div id="layout_left">
    <div id="layout_treetop"><span class="treetop_title"><%=GetResource("MenuManagement")%></span> </div>

    <div id="layout_treeboxAcc" style="height:90%;" >
        <div class="accordion" >
            <div class="menusub">
                <zeoMDMTree:UCMDMTreeScript ID="UCMDMTreeScript1" runat="server" UseIcon="true" SetCallbackComplete="onCallbackComplete" SetNodeSelect="MenuNodeSelect_onNodeSelect" />
            </div>
        </div>
    </div>

    <div id="layout_treebuttom"></div> 
</div>
</form>
</body>
</html>
