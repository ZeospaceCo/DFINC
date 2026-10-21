<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HASHTest.aspx.cs" Inherits="Zeospace.DeskPlus.Base.Web.Util.Enc.HASHTest" %>

<!DOCTYPE html>

<html>
<head runat="server"><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
</head>
<body>
    <form id="form1" runat="server">
    <table>
    <tr>
        <td><asp:TextBox ID="tbInput" runat="server" Height="391px" TextMode="MultiLine" Width="379px"></asp:TextBox></td>
        <td>
            <asp:Button ID="btnSHA1" runat="server" Text="SHA1" onclick="btnSHA1_Click" /><br /><br />
            <asp:Button ID="btnSHA2" runat="server" Text="SHA2" onclick="btnSHA2_Click" /><br /><br />
            <asp:Button ID="btnSHA5" runat="server" Text="SHA5" onclick="btnSHA5_Click" /><br /><br />
            <asp:Button ID="btnMD5" runat="server" Text="MD5" onclick="btnSMD5_Click" />
        </td>
        <td><asp:TextBox ID="tbOut" runat="server" Height="391px" TextMode="MultiLine" Width="379px"></asp:TextBox></td>
    </tr>
    <tr>
        <td colspan="2"><asp:Label ID="lbDebug" runat="server" Text=""></asp:Label></td>
    </tr>
    </table>
    </form>
</body>
</html>
