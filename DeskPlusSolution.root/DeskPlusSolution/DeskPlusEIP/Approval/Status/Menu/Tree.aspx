<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tree.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Status.Menu.Tree" %>

<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var AccordionActive = <%=AccActive %>;
        var FolderStatusID  = "<%=FolderStatusID%>";
        var SearchTerm      = "<%=SearchTerm %>";       //날자 분류
        var SearchTermValue = "<%=SearchTermValue %>";  //날자값        
        var SearchField     = "<%=SearchField %>";      //검색키워드
        var SearchValue     = "<%=SearchValue %>";      //검색어
        var CurrentPage     = "<%=CurrentPage %>";      //현재페이지
        var Order           = "<%=Order %>";            //정렬
        var ProxyUserID     = "<%=ProxyUserID %>";

        var menuID          = "";
        var targetUserID    = "";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Status/Menu/Tree.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Status/Menu/Tree.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_left" class="left-menu">
        <ul class="left-menu-container">
            <li class="menu left-main-menu ui-state-active">
               <%=GetResource("ApproveStatus")%>
            </li>

            <li id="layout_treeboxAcc" style="height: 90%;">
                <div class="accordion">
                    <span></span>
                    <div class="menusub">                    
                        <zeoAppFolderStatusTree:UCAppFolderStatusTreeScript ID="UCFolderStatusTreeScript1" SetCallbackComplete="onCallbackComplete" SetNodeSelect="onNodeSelect" 
                        SetFolderStatusList = "" UseCheckBox="false" runat="server" />
                    </div>
                </div>    
            </li>
        </ul>
    </div>
    </form>
</body>
</html>
