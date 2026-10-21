<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tree.aspx.cs" Inherits="Zeospace.DeskPlus.Account.Web.Admin.Position.Tree" %>
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
    <script type="text/javascript">
         var CodeID = "<%=CodeID %>";
         var Scope = "<%=Scope %>";
         var PCodeID = "<%=PCodeID %>";

         var ImagePath = "<%=BaseImgPath%>";
         var PreLang = "<%=PreLang%>";
    </script>
    <script type="text/javascript">
        JsCall("../../Js/Admin/Position/Tree.js");
    </script>    
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_left" class="left-menu admin">
        <ul class="left-menu-container"> 
            <li>
                <div id="layout_treetop">
                    <span class="treetop_title"><%=GetResource("CodeManage")%></span>
                </div>
                <div id="layout_treeboxAcc">
                    <div class="accordion" >
                        <div class="menu" style="display:none"><%=GetResource("CodeManage")%></div>
                        <div class="menusub">
                            <zeoCodeTree:UCCodeTreeScript ID="UCCodeTreeScript1" runat="server" UseIcon="true" UseCheckBox="false" UseActiveCode="false" />
                       </div>
                    </div>
                </div>

                <div id="layout_treebuttom"></div>
                <div id="layout_treebutton">
                    <ul>                
                        <li><a id="TreeAdd" class="btn btn-sm"><%=GetResource("Admin_CompanyManager_btnAdd")%></a></li>
                    </ul>
                </div>
            </li>
        </ul>
    </div>
    </form>
</body>
</html>
