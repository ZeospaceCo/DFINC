<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TreeAll.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Status.Menu.TreeAll" %>
<%@ Register Src="~/Approval/Common/Status/FolderStatusAllTreeScript.ascx" TagPrefix="zeoAppFolderStatusAllTree" TagName="UCAppFolderStatusAllTreeScript" %>

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
        var AccordionActive = <%=AccActive %>;
        var FolderStatusID  = "<%=FolderStatusID%>";
        var ProxyUserID     = "<%=ProxyUserID %>";

        var SearchTerm      = "<%=SearchTerm %>";
        var SearchTermValue = "<%=SearchTermValue %>";
        var SearchField     = "<%=SearchField %>";
        var SearchValue     = "<%=SearchValue %>";
        var CurrentPage     = "<%=CurrentPage %>";
        var Order           = "<%=Order %>";

        var menuID = "";
        var targetUserID = "";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Status/Menu/TreeAll.js?v=3"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_left" class="left-menu">
        <ul class="left-menu-container">
            <li class="menu left-main-menu ui-state-active">
               전자결재
            </li>

            <li id="layout_treeboxAcc" style="height: 90%;">
                <div class="accordion">
                    <span></span>
                    <div class="menusub">
                        <zeoAppFolderStatusAllTree:UCAppFolderStatusAllTreeScript ID="UCFolderStatusTreeScript1" SetCallbackComplete="onStatusCallbackComplete"
                            SetNodeSelect="onStatusNodeSelect" SetFolderStatusList="" UseCheckBox="false" runat="server" />
                    </div>
                </div>
            </li>
        </ul>
    </div>
    </form>
</body>
</html>
