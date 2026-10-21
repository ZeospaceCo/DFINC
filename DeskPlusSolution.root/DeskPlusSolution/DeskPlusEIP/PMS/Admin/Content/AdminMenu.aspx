<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminMenu.aspx.cs" Inherits="Zeospace.DeskPlus.PMS.Web.Admin.Content.AdminMenu" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"><title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_PMS.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_PMS.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_left" class="left-menu admin">

        <ul class="left-menu-container">
        <li>
            <div id="layout_treetop">
                <span class="treetop_title"><%=GetResource("Menu_admin1")%></span>
            </div>

            <div id="layout_treeboxAcc" style="height:90%;">
                <div class="accordion">
                    <div class="menu"><a href="#" onclick=""><%=GetResource("Admin_Menu")%></a></div>
                    <div class="menusub">
                        <ul>
                            <li><h2><a href="JListOpen.aspx" target="contentFrm"><%=GetResource("Admin_Menu1")%></a></h2></li>
                            <li><h2><a href="JListClose.aspx" target="contentFrm"><%=GetResource("Admin_Menu2")%></a></h2></li>
                            <li><h2><a href="JListLinked.aspx" target="contentFrm"><%=GetResource("Admin_Menu4")%></a></h2></li>
                        </ul>
                    </div>
                </div>    
            </div>
        
            <div id="layout_treebuttom"></div>
        </li>
        </ul>
    </div>    
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/PMS/Js/Admin/Content/AdminMenu.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/PMS/Js/Admin/Content/AdminMenu.js");
    </script>
</body>
</html>
