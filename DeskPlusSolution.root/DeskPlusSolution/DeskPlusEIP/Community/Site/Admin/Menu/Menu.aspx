<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="Zeospace.DeskPlus.Community.Web.Site.Admin.Menu.Menu" %>
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
      <li>
        <div id="layout_treetop">
            <span class="treetop_title"><%=GetResource("AdminMenu")%></span>
        </div>
        <div id="layout_treeboxAcc">
            <div class="accordion">     
                <div class="menu"><a href="#" onclick="" ><%=GetResource("MemberManage")%></a></div>    
                <div class="menusub">
                    <ul style="clear: both;">
                        <li><h2><a id="MAdd"><%=GetResource("JoinManage")%></a></h2></li>
                        <li><h2><a id="MMod"><%=GetResource("SearchUserList")%></a></h2></li>
                        <li><h2><a id="MGood"><%=GetResource("GoodMemberSearch")%></a></h2></li>              
                    </ul>
                </div>

                <div class="menu"><a href="#" onclick="" ><%=GetResource("CommunityManage")%></a></div>
                <div class="menusub">
                    <ul style="clear: both;">
                        <li><h2><a id="GInfo"><%=GetResource("ModCommunityInfo")%></a></h2></li>
                        <li><h2><a id="GDel"><%=GetResource("DelCommunity")%></a></h2></li>
                        <li><h2><a id="GMod"><%=GetResource("ChangeManager")%></a></h2></li>
                        <li><h2><a id="GPoint"><%=GetResource("PointManagement")%></a></h2></li>
                        <li><h2><a id="GGrade"><%=GetResource("GradeNameManage")%></a></h2></li>
                    </ul>
                </div>
            
                <div class="menu"><a href="#" onclick="" ><%=GetResource("MenuSkinManage")%></a></div>
                <div class="menusub">
                    <ul style="clear: both;">
                        <li><h2><a id="FMenu"><%=GetResource("MenuManager")%></a></h2></li>
                        <li><h2><a id="FMain"><%=GetResource("MainDisplay")%></a></h2></li>
                        <li><h2><a id="FPortal"><%=GetResource("MainPageManage")%></a></h2></li>
                        <li><h2><a id="FSkin"><%=GetResource("SkinManager")%></a></h2></li>
                    </ul>
                </div>
            </div>    
        </div>    
        
        <div id="layout_treebuttom"></div>

        <div id="layout_treebutton">
           <a href="#" id="HomeGo" style="cursor: pointer" class="btn btn-sm"><%=GetResource("Home")%></a>
        </div>   
      </li>
    </ul>     
</div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript">  
        var AccordionActive = <%=AccActive %>;
        var CommunityID = "<%=CommunityID %>";
        var AdminCheck = "<%=AdminCheck %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Community/Js/Site/Admin/Menu/Menu.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Community/Js/Site/Admin/Menu/Menu.js");
    </script>
</body>
</html>
