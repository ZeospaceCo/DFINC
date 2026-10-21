<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Admin.Menu.Menu" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Pim.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Pim.css" />
</head>
<body>
    <form id="form1" runat="server">
	    <div id="layout_left" class="left-menu admin"> 
            <ul class="left-menu-container">
                <li>
                    <div id="layout_treetop">
                        <span class="treetop_title"><%=GetResource("Schedule")%><%=GetResource("Manager")%></span>
                    </div>
                </li>    
                <li id="layout_treeboxAcc" style="height: 90%;">
                    <div class="accordion">
                        <div class="menu"><a href="#" ><%=GetResource("Setting") %></a></div>
                        <div class="menusub">
                            <ul class="menu_line">                        
                                <li><h2><a id="ShareUser"><%=GetResource("PIMShareManager")%></a></h2></li>
                                <li><h2><a id="Category"><%=GetResource("Category")%></a></h2></li>
                                <li><h2><a id="Holiday"><%=GetResource("AnnivControl")%></a></h2></li>
                                <li><h2><a id="HolidayR"><%=GetResource("HolidayManage")%></a></h2></li>
                                <li><h2><a id="HolidayD"><%=GetResource("HolidayManageDay")%></a></h2></li>
                            </ul>
                        </div>

                        <!--div class="menu"><%=GetResource("EnviroMent") %></div>
                        <div class="menusub">
                            <a id="configManage"><%=GetResource("EnviroMentSetting") %></a></div>
                        </div-->     
                      </div>                       
                </li>
             </ul>
         </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <!--<script type="text/javascript" src="/DeskPlusEIP/Pim/Js/Admin/Menu/Menu.js"></script>-->
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Pim/Js/Admin/Menu/Menu.js");
    </script>
</body>
</html>
