<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tree.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Target.Tree" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Pim.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Pim.css" />

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick-ko.js"></script>
    <script type="text/javascript">
        var PreLang = "<%=PreLang%>"; 
        
    </script>

    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/Target/Tree.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_left" class="left-menu"> 
        <ul class="left-menu-container">
            <li id="layout_treeboxAcc">
                <div class="accordion" >
                    <div class="menu left-main-menu ui-state-active"><a href="#" onclick="" ><%=GetResource("MarkManage")%></a></div>
                    <div class="menusub">
                        <zeoDeptTree:UCDeptTreeScript ID="UCDeptTreeScript1" runat="server" UseIcon="true" UseCheckBox="false" UseActiveDept="true" />
                    </div>
                </div>
            </li>
        </ul>
    </div>
    <div style="display: none">
    <asp:TextBox ID="hdSelDept" Style="display: none"   runat="server" Width="0px"></asp:TextBox>
    </div>    
    </form>
</body>
</html>
