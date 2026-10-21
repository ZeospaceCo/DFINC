<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="Zeospace.DeskPlus.Account.Web.Admin.AuthEmail.Menu" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Account.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript">
    </script>    
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Account/Js/Admin/AuthEmail/Menu.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
<div id="layout_left" class="left-menu admin">
    <ul class="left-menu-container">
        <li>
            <div id="layout_treetop">
                <span class="treetop_title"><%=GetResource("PasswordTitle")%></span>
            </div>

            <div id="layout_treeboxAcc" style="height:90%;">
                <div class="accordion">
                    <div class="menusub">
                        <ul class="menu_line">
                            <li><h2><a onclick="Menu.GoListPage('manager')"><%=GetResource("PasswordManager") %></a></h2></li>
                            <li><h2><a onclick="Menu.GoListPage('list')"><%=GetResource("SendList") %></a></h2></li>  
                        </ul>
                    </div>
                </div>    
            </div>

        </li>
    </ul> 
</div>
    </form>
</body>
</html>
