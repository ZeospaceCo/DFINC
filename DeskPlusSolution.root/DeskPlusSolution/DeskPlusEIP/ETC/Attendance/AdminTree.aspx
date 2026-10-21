<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminTree.aspx.cs" Inherits="Zeospace.DeskPlus.ETC.Web.Attendance.AdminTree" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_ETC.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_ETC.css"/>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/ETC/Js/Attendance/AdminTree.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_left" class="left-menu admin"> 
        <ul class="left-menu-container">
            <li>
            <div id="layout_treetop">
                <span class="treetop_title"><%=GetResource("Attendance")%> <%=GetResource("Administrator")%></span>
            </div>

            <div id="layout_treeboxAcc" style="height:90%;">

                <div class="accordion">
                    <div class="menusub">
                        <ul class="menu_line">
                            <li><h2><a onclick="AdminTree.GoListPage('ADMINROLE')"><%=GetResource("View")%><%=GetResource("AuthorityManagement")%></a></h2></li>
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