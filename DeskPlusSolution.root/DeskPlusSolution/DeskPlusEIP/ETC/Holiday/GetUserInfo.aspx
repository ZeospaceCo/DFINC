<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetUserInfo.aspx.cs" Inherits="Zeospace.DeskPlus.ETC.Web.Holiday.GetUserInfo" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_ETC.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_ETC.css"/>    
</head>
<body>
    <form id="form1" runat="server">

    <div class="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1><asp:Literal ID="ltlReceiveMan" runat="server"></asp:Literal></h1>
                <ul class="btn_right">
                    <li><div id="loading"></div></li>
                    <li><cc3:ClientSmartButton ID="cbtnSubmit" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                    <li><cc3:ClientSmartButton ID="cbtnClean" runat="server"></cc3:ClientSmartButton></li>
                    <li><cc3:ClientSmartButton ID="cbtnCancel" runat="server"></cc3:ClientSmartButton></li>
                </ul>            
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3" style="min-height:170px;">
                    <table class="table_typeB">
			        <caption></caption>
			        <colgroup>
                        <col width="80px;" />
                        <col />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th><asp:literal id="lblSelectedReceiptor" runat="server"></asp:literal></th>
                            <td>
                            <cc3:ClientSmartTextBox ID="tbReceiveMan" ReadOnly="true" runat="server" CssStyle="width:95% !important;" TitleAtt=""></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                    </tbody>
                    </table>
                    <br />
                    <div class="layout_scroll" style="min-height:400px;height:400px;overflow:hidden">
                        <zeoTree:UCUserDeptTreeScript ID="DeptUserTreeScript1" runat="server" />
                    </div>
                    <asp:literal id="ltlReceiveManSelect" runat="server" Visible="false"></asp:literal>
            </div>                
        </div>
    </div>

    <!-- ================================== Hidden Input ===================================== -->
    <div style="display: none">
        <input id="hdnUserID" type="hidden" name="hdnUserID" runat="server">
        <input id="hdnDeptID" type="hidden" name="hdnDeptID" runat="server">
        <input id="hdnDeptName" type="hidden" name="hdnDeptName" runat="server">
        <asp:TextBox ID="tvSelectedID" runat="server" Width="0" Height="0" Style="display: none"></asp:TextBox>
        <asp:TextBox ID="tvSelectedValue" runat="server" Width="0" Height="0" Style="display: none"></asp:TextBox>
    </div>
    <!-- ==================================================================================== -->
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var R_MsgTarget = "<%=GetResource("MsgTarget") %>";
        var R_MsgPerson = "<%=GetResource("MsgPerson") %>";
    </script>
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/ETC/Js/Holiday/GetUserInfo.js");
    </script>
</form>
</body>
</html>
