<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HtmlToPdf.aspx.cs" Inherits="Zeospace.DeskPlus.Base.Web.Util.PDF.HtmlToPdf" %>

<!DOCTYPE html>

<html>
<head runat="server"><meta charset="UTF-8">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table>
    <tr>
        <td><asp:TextBox ID="tbInput" runat="server" Height="400px" TextMode="MultiLine" Width="500px" Text="" ></asp:TextBox></td>    
        <td>
            <asp:Button ID="btnHtmlToPdf" runat="server" Text="HtmlToPDF" 
                onclick="btnHtmlToPdf_Click" />
            </br></br>
            <asp:Button ID="btnHtmlToImage" runat="server" Text="HtmlToImage" 
                onclick="btnHtmlToImage_Click" />
   
  
            <asp:HyperLink ID="hlFilePath" runat="server"></asp:HyperLink>
         </td>
    </tr>
    </table>
    </div>
    </form>
</body>
</html>
