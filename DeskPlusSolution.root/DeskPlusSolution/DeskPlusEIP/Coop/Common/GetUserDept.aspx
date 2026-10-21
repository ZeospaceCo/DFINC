<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetUserDept.aspx.cs" Inherits="Zeospace.DeskPlus.Coop.Web.Common.GetUserDept" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_PMS.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_PMS.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/AutoComplete.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var PreLang = "<%=PreLang%>";
        var Gubun = "<%=Gubun%>";
    </script>
    <script type="text/javascript">        
        JsCall("/DeskPlusEIP/Coop/Js/Common/GetUserDept.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">

    <div class="layer_popup">
        <div class="top-left"></div>
        <div class="top-right"></div>
        <div class="inside">
            <div id="layout_header">
                <h5><asp:Literal ID="ltlReceiveMan" runat="server"></asp:Literal></h5>
                <ul class="btn_right">
                    <li><div id="loading"></div></li>
                    <li class="btn_blue"><cc3:ClientSmartButton ID="cbtnSubmit" runat="server"></cc3:ClientSmartButton></li>
                    <li><cc3:ClientSmartButton ID="cbtnCancel" runat="server"></cc3:ClientSmartButton></li>
                </ul>            
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3">
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

                <div class="layout_scroll"  style="min-height:350px;height:350px;"> 
                    <zeoTree:UCUserDeptTreeScript ID="DeptUserTreeScript1" runat="server" />
                </div>                
            </div>                
        </div>
        <div class="bottom-left"></div>
        <div class="bottom-right"></div>
    </div>

    <!-- ================================== Hidden Input ===================================== -->
    <div style="display: none">
        <input id="hdnUserID" type="hidden" name="hdnUserID" runat="server">
        <asp:TextBox ID="tvSelectedID" runat="server" Width="0" Height="0" Style="display: none"></asp:TextBox>
        <asp:TextBox ID="tvSelectedValue" runat="server" Width="0" Height="0" Style="display: none"></asp:TextBox>
        <asp:TextBox ID="tvTypes" runat="server" Width="0" Height="0" Style="display: none"></asp:TextBox>
    </div>
    <!-- ==================================================================================== -->
</form>
</body>
</html>
