<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EncDecApp.aspx.cs" Inherits="Zeospace.DeskPlus.Common.Web.EncDecApp" %>

<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <style type="text/css">
        .style1
        {
            height: 149px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table border="1">
        <tr>
            <td class="style1">
                <asp:TextBox ID="tbLeftEnc" runat="server" Height="176px" Width="430px"></asp:TextBox>
            </td>
            <td class="style1">
                <asp:Button ID="ctnEnc" runat="server" Text="&gt;&gt; Encryption" 
                    onclick="ctnEnc_Click" />
            </td>
            <td class="style1">
                <asp:TextBox ID="tbRightDec" runat="server" Height="176px" Width="430px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="tbLeftDec" runat="server" Height="176px" Width="430px"></asp:TextBox>
            </td>
            <td>
                <asp:Button ID="btnDec" runat="server" Text="&gt;&gt; Decryption" 
                    onclick="btnDec_Click" />
            </td>
            <td>
                <asp:TextBox ID="tbRightEnc" runat="server" Height="176px" Width="430px"></asp:TextBox>
            </td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
