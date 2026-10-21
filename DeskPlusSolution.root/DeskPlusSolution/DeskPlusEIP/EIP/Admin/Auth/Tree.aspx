<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tree.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Admin.Auth.Tree" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_EIP.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_EIP.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_left" class="left-menu admin"> 
      <ul class="left-menu-container">
      <li>
        <div id="layout_treetop">
            <span class="treetop_title"><%=GetResource("AuthManage")%></span>
        </div>

        <div id="layout_treeboxAcc">
            <div class="accordion">
                <div class="menu" style="display:none"><%=GetResource("AuthManage")%></div>
                <div class="menusub">
                    <zeoRoleTree:UCRoleTreeScript ID="UCRoleTreeScript1" runat="server" UseIcon="true" UseRole="true" UseGrade="false" SetCallbackComplete="onCallbackComplete" SetNodeSelect="RoleNodeSelect_onNodeSelect" />
                </div>
            </div>
        </div>

        <div id="layout_treebuttom"></div>
        <div id="layout_treebutton">
            <a id="cbtnApply" style="cursor: pointer" class="btn btn-sm btn-secondary"><%=GetResource("Apply")%></a>
            <a id="cbtnMod" style="cursor: pointer" class="btn btn-sm"><%=GetResource("Mod")%></a>  
            <a id="cbtnDel" style="cursor: pointer" class="btn btn-sm btn-danger"><%=GetResource("Del")%></a>
        </div>
      </li>
      </ul>
        
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var UserID = "<%=CurrentUserInfo.UserID %>";
        var RoleID = "<%=RoleID %>";
        var Gubun = "<%=Gubun %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_ProcessingApply = "<%=GetResource("ProcessingApply") %>";
        var R_ProcessingErrorApply = "<%=GetResource("ProcessingErrorApply") %>";
        var R_ConfirmDeleteAuth = "<%=GetResource("ConfirmDeleteAuth") %>";
        var R_ErrorProcessing = "<%=GetResource("ErrorProcessing") %>";
    </script>
    <script type="text/javascript" src="../../Js/Admin/Auth/Tree.js"></script>
    <script type="text/javascript">
        //JsCall("../../Js/Admin/Auth/Tree.js");
    </script>
</body>
</html>
