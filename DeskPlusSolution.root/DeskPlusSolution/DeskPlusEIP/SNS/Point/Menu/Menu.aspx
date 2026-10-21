<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="Zeospace.DeskPlus.SNS.Web.Point.Menu.Menu" %>
<%@ Register TagPrefix="uc1" TagName="WebHardTree" Src="/DeskPlusEIP/WebHard/Common/WebHardTree.ascx" %>
<%@ Register TagPrefix="uc2" TagName="BoardTreeScript" Src="/DeskPlusEIP/Board/Common/BoardTreeScript.ascx" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_SNS.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript">
        var ListTypeB = "0";
        var ListTypeW = "0";
        var ListTypeA = "0";
        var AccordionActive = 0;       
        var R_NoAuthority = "<%=GetResource("NoAuthority") %>";
    </script>    
    <script type="text/javascript" src="/DeskPlusEIP/SNS/Js/Point/Menu/Menu.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/SNS/Js/Point/Menu/Menu.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
<div id="layout_left" class="left-menu">
    <ul class="left-menu-container">
    <li id="layout_treeboxAcc" style="height:90%;">

        <div class="accordion">
            <div class="menu" id="impo"><a href="#"><%=GetResource("Impo")%></a></div>
            <div class="menusub">
                <ul>                                   
                    <li><h2><a onclick="Menu.GoListPage('Approval')"><%=GetResource("Approval")%></a></h2></li>
                    <li><h2><a onclick="Menu.GoListPage('Board')"><%=GetResource("BoardTitle")%></a></h2></li>                    
                    <li><h2><a onclick="Menu.GoListPage('Task')"><%=GetResource("Task")%></a></h2></li>    
                    <li><h2><a onclick="Menu.GoListPage('WebHard')"><%=GetResource("WebHard")%></a></h2></li>
                    <li><h2><a onclick="Menu.GoListPage('Address')"><%=GetResource("Address")%></a></h2></li>
                    <li><h2><a onclick="Menu.GoListPage('Opinion')"><%=GetResource("Opinion")%></a></h2></li>                 
                </ul>
            </div>
            <div class="menu"><a href="#" onclick="" ><%=GetResource("BoardTitle")%> <%=GetResource("Favorite")%></a></div>
            <div class="menusub" style="display:none;">
                <uc2:BoardTreeScript ID="UCBoardTreeScript1" runat="server" UseIcon="true" 
                UseWriteAuthority="false" UseReadAuthority="true" 
                UseFolder="false" UseMyTemplate="false" UseMyFavorite="true" 
                SetNodeSelect="onBoardNodeSelect" SetCallbackComplete="onCallbackComplete">
                </uc2:BoardTreeScript>
            </div> 
            <div class="menu" id="favorite"><a href="#"><%=GetResource("WebHard")%> <%=GetResource("Favorite")%></a></div>
            <div class="menusub" style="display:none;">
                <uc1:WebHardTree ID="WebHardTree1" runat="server" UseCategory="F"></uc1:WebHardTree>
            </div>
        </div>
    
    </li>

    </ul>
</div>
    </form>
</body>
</html>

