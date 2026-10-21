<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tree.aspx.cs" Inherits="Zeospace.DeskPlus.Education.Web.Content.Tree" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"> 
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Task.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Task.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Home.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Home.css" />  

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var AccordionActive = <%=p_AccActive %>;
        var isRoleEducation = "<%=isRoleEducation %>";
    </script>
    <!--script type="text/javascript" src="/DeskPlusEIP/Education/Js/Content/Tree.js"></script-->
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Education/Js/Content/Tree.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="layout_left" class="left-menu">
        <ul class="left-menu-container">
            <li id="layout_treeboxAcc">
                <div class="accordion">
                    <div class="menu"><a href="#" onclick="" >교육관리</a></div>
                    <div class="menusub">
                        <ul>   
                            <li><h2><a id="UserList">교육리스트</a></h2></li>  
                            <li id="MenuDash" runat="server"><h2><a id="AdminDashBoard">관리자 - 전체현황</a></h2></li>
                            <li id="MenuList" runat="server"><h2><a id="AdminList">관리자 - 교육현황</a></h2></li> 
                         </ul>                 
                    </div>    
                </div>
            </li>
        </ul>
        </div> 
    </form>
</body>
</html>
