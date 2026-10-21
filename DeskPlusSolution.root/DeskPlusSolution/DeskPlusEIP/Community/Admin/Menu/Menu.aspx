<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="Zeospace.DeskPlus.Community.Web.Admin.Menu.Menu" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Community.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Community.css"/>    
</head>
<body>
    <form id="form1" runat="server">
<div id="layout_left" class="left-menu admin">
    <ul class="left-menu-container">
         <li id="layout_treetop"><%=GetResource("CommunityManage")%></li>

         <li id="layout_treeboxAcc" style="height:90%;">
            <div class="accordion">
                <div class="menu"><a href="#" onclick="" ><%=GetResource("CommunityManage")%></a></div>
                <div class="menusub">
                    <ul>
                        <li><h2><a id="aAdd"><%=GetResource("ComConfirm")%></a></h2></li>
                        <li><h2><a id="aClose"><%=GetResource("CommunityDel")%></a></h2></li>
                        <li><h2><a id="aCategory"><%=GetResource("GroupManage")%></a></h2></li>
                    </ul>
                </div>             
            </div>    
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
        var AccordionActive = <%=AccActive %>;
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Community/Js/Admin/Menu/Menu.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Community/Js/Admin/Menu/Menu.js");
    </script>
</body>
</html>
