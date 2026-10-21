<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FaMenu.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Menu.FaMenu" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_EIP.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_EIP.css" />
</head>
<body>
<form id="form1" runat="server">

<div class="layer_popup">
    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("FaMenu")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" class="btn btn-secondary" /></li>
                <li><cc3:ClientSmartButton ID="cbtnClose" runat="server" /></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">

            <table class="table_typeB">
			<caption></caption>
			<colgroup>
                <col width="80px" />
                <col />
			</colgroup>
            <tbody>
                <tr>
                    <th><%=GetResource("Target")%></th>
                    <td><cc3:ClientSmartTextBox ID="menuName" ReadOnly="true" runat="server" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
                </tr>
            </tbody>
            </table>
            <div class="layout_scroll" style="min-height:285px;height:285px;">
            <zeoUserMenuTree:UCUserMenuTreeScript ID="UCUserMenuTreeScript1" runat="server" UseIcon="true" SetCallbackComplete="onCallbackComplete" SetNodeSelect="MenuNodeSelect_onNodeSelect" />
            </div>

        </div>
    </div>
</div>
<!-- ================================== Hidden Input ===================================== -->
<div style="display: none">
    <input id="hdnMenuID" type="hidden" name="hdnUserID" runat="server">    
    <asp:TextBox ID="tvSelectedID" runat="server" Width="0" Height="0" Style="display: none"></asp:TextBox>
    <asp:TextBox ID="tvSelectedValue" runat="server" Width="0" Height="0" Style="display: none"></asp:TextBox>
</div>
<!-- ==================================================================================== -->
</form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var UserID = "<%=CurrentUserInfo.UserID %>";
 
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";

        var R_SelectFaMenu = "<%=GetResource("SelectFaMenu") %>";
        var R_ProcessingApply = "<%=GetResource("ProcessingApply") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/EIP/Js/Menu/FaMenu.js"></script>
</body>
</html>
