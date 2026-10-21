<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tree.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Admin.Code.Tree" %>

<!DOCTYPE html>
<html>
<head>
    <style>
        html, body
        {
            height: 98%;
        }
    </style>
    <meta charset="UTF-8">
    <title></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_EIP.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_EIP.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var SYSCodeID = "<%=SYSCodeID %>";
        var CodeID = "<%=CodeID %>";
        var ParentCodeID = "";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_ConfirmDeleteSubCode = "<%=GetResource("ConfirmDeleteSubCode") %>";
        var R_ErrorProcessing = "<%=GetResource("ErrorProcessing") %>";
    </script>
    <script type="text/javascript" src="../../Js/Admin/Code/Tree.js"></script>
    <script type="text/javascript">
        //JsCall("../../Js/Admin/Code/Tree.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_left" class="left-menu admin">
    <ul class="left-menu-container">
    <li>
        <div id="layout_treetop">
            <span class="treetop_title"><%=GetResource("SysCodeManage")%></span>
        </div>
        <div id="layout_treeboxAcc" style="height:85%;" >
            <div class="accordion" >
                <div class="menu" style="display:none"><%=GetResource("SysCodeManage")%></div>
                <div class="menusub">
                    <zeoSysCodeTree:UCSysCodeTreeScript ID="UCSysCodeTreeScript1" runat="server" UseIcon="true" />
                </div>
            </div>
        </div>
        <div id="layout_treebuttom"></div>
        <div id="layout_treebutton">
            <a href="#" id="DelCatID" class="btn btn-sm btn-danger"><span><%=GetResource("Del")%></span></a>
        </div>
        <div style="display: none">
            <asp:TextBox ID="tbHdnValue" runat="server" Width="0"></asp:TextBox>
        </div>
    </li>
    </ul>
    </div>
    </form>
</body>
</html>
