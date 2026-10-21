<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminMenu.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Admin.Menu.AdminMenu" %>
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
    <div id="layout_menu">
        <div class="UTIL">
    	    <div class="menu_logo"><a href="/DeskPlusEIP/" target="_parent"><img src="<%=AMenuImage%>" title="SMART PLUS" alt="SmartPlus Logo" /></a></div>
            <ul class="menu_util">
                <li><a id="SignOutLink" href="/DeskPlusEIP/Site/Auth/LogOut.aspx" class="btn"><%=GetResource("LogOut") %></a></li>
            </ul>
        </div><!-- //UTIL --> 
    
        <div class="GNB">
            <ul class="menu_gnb">
                <div id="phMenu"></div>
            </ul>
        </div><!-- //GNB --> 

        <div class="SUB">
            <ul class="menu_sub">
            <div id="phSubMenu"></div>
            </ul>
        </div><!-- //SUB --> 
    </div><!-- //layout_menu -->  
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/EIP/Js/Admin/Menu/TopMenu.js"></script>
</body>
</html>
