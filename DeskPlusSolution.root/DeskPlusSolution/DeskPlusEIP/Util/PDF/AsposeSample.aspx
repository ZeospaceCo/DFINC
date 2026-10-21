<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AsposeSample.aspx.cs" Inherits="Zeospace.DeskPlus.Base.Web.Util.PDF.AsposeSample" %>

<!DOCTYPE html>

<html>
<head runat="server"><meta charset="UTF-8">
    <title></title>
</head>
<body style="font-size:12px;">
    <form id="form1" runat="server">

    <div style="padding-bottom:30px;">
        <table border="1" cellpadding="5" cellspacing="0">
        <tr>
            <td>* orginal : <asp:TextBox ID="tbOrginPath" runat="server" TextMode="SingleLine" Width="500px" Text="" ></asp:TextBox></td>
        </tr>
        <tr>
            <td>* targetpath : <asp:TextBox ID="tbTargetPath" runat="server" TextMode="SingleLine" Width="500px" Text="" ></asp:TextBox></td>
        </tr>
        <tr>
            <td>* targetname : <asp:TextBox ID="tbTargetName" runat="server" TextMode="SingleLine" Width="500px" Text="" ></asp:TextBox></td>
        </tr>
        <tr>
            <td>* 결과 : <asp:Label ID="lbResult" runat="server"></asp:Label></td>
        </tr>
        </table><br /><br />
        <table border="1" cellpadding="5" cellspacing="0">
        <tr>
            <td>
                1. Excel > JPG: 
                <asp:Button ID="btnExcelToJpg" runat="server" Text="btnExcelToJpg" onclick="btnExcelToJpg_Click" /><br />
            </td>
        </tr>
        <tr>
            <td>
                2. PDF > JPG: 
                <asp:Button ID="btnPdfToJpg" runat="server" Text="btnPdfToJpg" onclick="btnPdfToJpg_Click" /><br />
            </td>
        </tr>
        <tr>
            <td>
                3. DOC > Tiff: 
                <asp:Button ID="btnDocToTiff" runat="server" Text="btnDocToTiff" onclick="btnDocToTiff_Click" /><br />
            </td>
        </tr>
        <tr>
            <td>
                4-1. PPT > Tiff: 
                <asp:Button ID="btnPptToTiff" runat="server" Text="btnPPTToTiff" onclick="btnPPTToTiff_Click" /><br />
            </td>
        </tr>
        <tr>
            <td>
                4-2. PPTX > Tiff: 
                <asp:Button ID="btnPptxToTiff" runat="server" Text="btnPptxToTiff" onclick="btnPptxToTiff_Click" /><br />
            </td>
        </tr>
        </table>
    </div>
    ===============================================================================================================
    <div style="padding-top:30px;">
        <table border="1" cellpadding="5" cellspacing="0">
        <tr>
            <td>* orginalfolder : <asp:TextBox ID="tbOriginalFolder" runat="server" TextMode="SingleLine" Width="500px" Text="" ></asp:TextBox></td>
        </tr>
        <tr>
            <td>* targetfile : <asp:TextBox ID="tbTargetFile" runat="server" TextMode="SingleLine" Width="500px" Text="" ></asp:TextBox></td>
        </tr>
        <tr>
            <td>* 결과 : <asp:Label ID="lbResult2" runat="server"></asp:Label></td>
        </tr>
        </table><br /><br />
        <table border="1" cellpadding="5" cellspacing="0">
        <tr>
            <td>
                * 일괄출력(PDF) : Images -> PDF
                <asp:Button ID="btnImgPDF" runat="server" Text="Images -> PDF" onclick="btnImgPDF_Click" /><br />
            </td>
        </tr>
        <tr>
            <td>
                * 일괄출력(PDF)
                <asp:Button ID="btnPrintPDF" runat="server" Text="일괄출력(PDF)" onclick="btnPrintPDF_Click" /><br />
            </td>
        </tr>
        </table>
    </div>
    </form>
</body>
</html>
