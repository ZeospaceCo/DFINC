<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tree.aspx.cs" Inherits="Zeospace.DeskPlus.Account.Web.Search.Tree" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Account.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Account.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/AutoComplete.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%=CompanyID %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Account/Js/Search/Tree.js"></script>
</head>
<body>
    <form id="form1" runat="server">

    <div id="layout_left" class="left-menu">
        <ul class="left-menu-container">
            <li id="layout_treeboxAcc" style="height:90%;">
                <div class="accordion" >
                    <div class="menu left-main-menu menu ui-state-active"><a href="#" onclick=""><%=GetResource("EmployeeSearch")%></a></div>            
                    <div class="menusub">
                        <zeoSearchTree:UCSearchTreeScript ID="DeptUserTreeScript1" runat="server" UseActiveDept="true" UseCheckBoxGroup="false" UseCheckBoxMember="false" UseIcon="true" />
                        <input id="Text1" type="text" style="display:none;" />
                   </div>
                </div>
            </li>
    
            <li id="layout_treebuttom"></li>
        </ul>
    </div>
    </form>
</body>
</html>
