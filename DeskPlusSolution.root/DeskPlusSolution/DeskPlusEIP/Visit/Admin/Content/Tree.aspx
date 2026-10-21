<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tree.aspx.cs" Inherits="Zeospace.DeskPlus.Visit.Web.Admin.Content.Tree" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Visit.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Visit.css"/>
</head>
<body>
    <form id="form1" runat="server">
	    <div id="layout_left" class="left-menu admin">  
            <ul style="height: 90%;" class="left-menu-container">
                <li>
                    <div id="layout_treetop">
                        <span class="treetop_title"><%=GetResource("VisitReservation")%><%=GetResource("Manager")%></span>
                    </div>
                </li>            
                <li>
                    <div id="layout_treeboxAcc"  style="height: 90%;">
                         <div class="accordion">
                            <div class="menu"><a href="#"><%=GetResource("Setting")%></a></div>
                            <div class="menusub">
                                <ul class="menu_line">
                                    <li><h2><a id="Share"><%=GetResource("ShareManager")%></a></h2></li>
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
    <script type="text/javascript" src="/DeskPlusEIP/Visit/Js/Admin/Content/Tree.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Visit/Js/Admin/Content/Tree.js");
    </script>
</body>
</html>
