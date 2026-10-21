<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tree.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Disuse.Tree" %>
<%@ Register TagPrefix="uc1" TagName="DisuseTree" Src="DisuseTree.ascx" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title>
        <%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript">

        var AccordionActive = 0;
        var FormID = "0";

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Disuse/Tree.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Disuse/Tree.js");
    </script>
</head>
<body>
    <form id="form2" runat="server">
    <div id="layout_left" class="left-menu">
        <ul class="left-menu-container">
            <li class="menu left-main-menu ui-state-active">
               <%=GetResource("DisuseFolder")%>
            </li>

            <li id="layout_treeboxAcc" style="height: 90%;">
                <div class="accordion">
                    <span></span>
                    <div class="menusub">
                        <uc1:DisuseTree ID="UCDisuseTree" runat="server" UseCategory="W"></uc1:DisuseTree>
                    </div>
                </div>
            </li>
        </ul>
        <div id="layout_treebuttom">
        </div>     
    </div>
    </form>
</body>
</html>
