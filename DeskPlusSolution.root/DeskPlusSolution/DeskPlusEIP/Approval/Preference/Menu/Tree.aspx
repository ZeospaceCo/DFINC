<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tree.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Preference.Menu.Tree" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript">
        var AccordionActive = <%=AccActive %>;
        var menuID = "<%=MenuID %>";
    </script>
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Approval/Js/Preference/Menu/Tree.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
<div id="layout_left" class="left-menu"> 

    <ul class="left-menu-container">
        <li class="menu left-main-menu ui-state-active"><%=GetResource("PrivateSetting")%></li>
        <li  class="left-sub-menu">
                    <ul class="accordion">
                        <li><a id="FavoriteApp"><%=GetResource("FavoriteApp")%></a></li>
                        <li><a id="ProxyApp"><%=GetResource("ManageProxy")%></a></li>
                        <li><a id="SignApp"><%=GetResource("ManageStamp")%></a></li>
                        <li><a id="UserFlowApp"><%=GetResource("PrivateAppNetMgr")%></a></li>
                        <li><a id="FavoriteForm"><%=GetResource("FavoriteForm")%></a></li>
                        <li><a id="Setting"><%=GetResource("Setting2")%></a></li>
                    </ul>
        </li>
	</ul>
</div>
    </form>
</body>
</html>
