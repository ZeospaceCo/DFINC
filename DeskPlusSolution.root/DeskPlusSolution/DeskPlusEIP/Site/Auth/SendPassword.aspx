<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendPassword.aspx.cs" Inherits="Zeospace.DeskPlus.Base.Web.Site.Auth.SendPassword" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
	<meta name="robots" content="noindex">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Design/C1/Css/DeskPlusEIPBase.css"/>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>  
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.password.js"></script>
    <script type="text/javascript" >
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CompanyID = "<%=CompanyID%>";
        var LoginID = "<%=LoginID%>";
        var AuthEmail = "<%=AuthEmail %>";
        var ManagerEmail = "<%=ManagerEmail %>";
        var R_Success = "<%=GetResource("SUCCESS")%>"
        var R_NoAuthEmail = "<%=GetResource("NoAuthEmail")%>" 
        var R_CheckEmail = "<%=GetResource("CheckEmail")%>"             
        var Gubun = "<%= Gubun %>";

        var PreLangs = "<%=PreLangs %>";        

    </script>    
    <script type="text/javascript">
        JsCall("../../Js/Site/Auth/SendPassword.js");
    </script>
</head>
<body oncontextmenu="return false">
    <form id="form1" runat="server">
<div class="layer_popup">
    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("SendTempPassword")%></h1>           
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3" style="min-height:238px;height:238px;">
            <div id="txtSendGuide">
            <%=GetResource("SendEmail1")%><br />
            <%=GetResource("SendEmail2")%><br /><br />
            </div>
            <div id="txtSearchGuide">
                <%=GetResource("SendMsg")%><br /><br /><br />
            </div>
        <span style="font-weight:bold;color:Navy;"></span>
        <table class="table_typeB" border="0" style="width:90%;" > 
		<caption></caption>
		<colgroup>
            <col width="120px" />
            <col />
		</colgroup>
		<tbody>
        <tr id="trSearchLoginID">
            <th><%=GetResource("LoginIDRes")%></th>
            <td>
                <span class="aLeft">
                    <cc3:ClientSmartTextBox ID="ctbCheckLoginID" InputMode="None" MaxLength="250" runat="server" Required="true" CssStyle="width:250px !important;IME-MODE:disabled;" ></cc3:ClientSmartTextBox>&nbsp;
                </span>
                <ul class="btn_smallL" style="padding-left:10px;">
                 <li><cc3:ClientSmartButton ID="cbtnLoginCheck" runat="server" /></li>
                </ul>
            </td>
        </tr>
        <tr id="trSearchEmail">
            <th><%=GetResource("MailRes")%></th>
            <td><div id="divAuthEmail"></div></td>
        </tr>
        <tr id="trEmail">
            <th><%=GetResource("ReceiveEmail")%></th>
            <td><%=AuthEmail %></td>
        </tr>
        </tbody>
        </table>
        </div>
             <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnCheckConfirm" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnClose" runat="server" /></li>
            </ul>
    </div>
</div>       
    <div style="display: none">        
        <asp:TextBox ID="tbEmail" runat="server" Width="0"></asp:TextBox>
    </div>
    </form>
</body>
</html>
