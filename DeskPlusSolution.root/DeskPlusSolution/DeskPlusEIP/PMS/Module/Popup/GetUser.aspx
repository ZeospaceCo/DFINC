<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetUser.aspx.cs" Inherits="Zeospace.DeskPlus.PMS.Web.Module.Popup.GetUser" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"><title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_PMS.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_PMS.css" />    
</head>
<body>
    <form id="form1" runat="server">

    <div class="layer_popup">
        <div class="inside">
            <div id="popup_header">
                <h1><asp:Literal ID="ltlReceiveMan" runat="server"></asp:Literal></h1>
                <ul class="btn_right">
                    <li><div id="loading"></div></li>
                    <li><cc3:ClientSmartButton ID="cbtnSubmit" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
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

                <div style="height:360px;"> 
                    <asp:ListBox ID="lbxList" runat="Server" Width="100%" Height="100%" SelectionMode="Multiple"></asp:ListBox>
                </div>
                <asp:literal id="ltlReceiveManSelect" runat="server" Visible="false"></asp:literal>
            </div>                
        </div>
    </div>

        <!-- ================================== Hidden Input ===================================== -->
        <div style="display: none">            
            <asp:TextBox ID="hiddenSelectedID" runat="server" Width="0" Height="0" Style="display: none"></asp:TextBox>
        </div>
        <!-- ==================================================================================== -->
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var lbxList = "<%=lbxList.ClientID %>";
        var tbReceiveMan = "<%=tbReceiveMan.ClientID %>";
        var hiddenSelectedID = "<%=hiddenSelectedID.ClientID %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/PMS/Js/Module/Popup/GetUser.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/PMS/Js/Module/Popup/GetUser.js");
    </script>
</body>
</html>
