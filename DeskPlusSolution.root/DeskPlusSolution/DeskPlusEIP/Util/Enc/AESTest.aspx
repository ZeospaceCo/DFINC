<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AESTest.aspx.cs" Inherits="Zeospace.DeskPlus.Base.Web.Util.Enc.AESTest" %>

<!DOCTYPE html>

<html>
<head runat="server"><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.base64.js"></script>
    <script language="javascript">
        function Base64Enc() {
            var str = $("#tbInput").val();
            var encStr = Base64.encode(str);
            
            $("#tbOut").html(encStr);
        }


        function Base64Dec() {
            var str = $("#tbOut").val();
            var decStr = Base64.decode(unescape(str));
            $("#tbInput").html(decStr);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <table>
    <tr>
        <td><asp:TextBox ID="tbInput" runat="server" Height="391px" TextMode="MultiLine" Width="379px"></asp:TextBox></td>
        <td>
            <asp:Button ID="btnEnc" runat="server" Text="ENC" onclick="btnEnc_Click" /><br /><br />
            <asp:Button ID="btnDec" runat="server" Text="DEC" onclick="btnDec_Click" />
            <br /><br /><br /><br /><br /><br />
            <input type="button" onclick="Base64Enc()" value="JSBase64Enc" /><br /><br />
            <input type="button" onclick="Base64Dec()" value="JSBase64Dec" />
        </td>
        <td><asp:TextBox ID="tbOut" runat="server" Height="391px" TextMode="MultiLine" Width="379px"></asp:TextBox></td>
    </tr>
    <tr>
        <td colspan="2">
        <asp:DropDownList ID="ddlType" runat="server">
            <asp:ListItem Text="AES" Value="AES"></asp:ListItem>
            <asp:ListItem Text="TripleDES" Value="TripleDES"></asp:ListItem>
            <asp:ListItem Text="Base64" Value="Base64"></asp:ListItem>
        </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td colspan="2"><asp:Label ID="lbDebug" runat="server" Text=""></asp:Label></td>
    </tr>
    </table>
    
    
    
    </form>
</body>
</html>
