<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tree.aspx.cs" Inherits="Zeospace.DeskPlus.Account.Web.Admin.User.Tree" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Account.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Account.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>
    <script type="text/javascript">
        var DeptID = "";

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
    </script>    
    <script type="text/javascript">
        JsCall("../../Js/Admin/User/Tree.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_left" class="left-menu admin">
        <ul class="left-menu-container"> 
            <li>
                <div id="layout_treetop">
                    <span class="treetop_title"><%=GetResource("Admin_UserList_UserManager")%></span>
                </div>        
        
                <div id="layout_treeboxAcc">
                    <div class="accordion" >
                        <div class="menu" style="display:none"><%=GetResource("Admin_UserManager_Literal2")%></div>
                        <div class="menusub">
                            <zeoDeptTree:UCDeptTreeScript ID="UCDeptTreeScript1" runat="server" UseIcon="true" UseCheckBox="false" UseActiveDept="false" />
                       </div>
                    </div>
                </div>
                <div id="layout_treebuttom"></div>
            </li>
        </ul>
    </div>
    <div style="display: none">        
        <asp:TextBox ID="tbHdnValue" runat="server" Width="0"></asp:TextBox>
    </div>
    </form>
</body>
</html>
