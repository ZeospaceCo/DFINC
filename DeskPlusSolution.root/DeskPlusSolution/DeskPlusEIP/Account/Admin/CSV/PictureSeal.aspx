<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PictureSeal.aspx.cs" Inherits="Zeospace.DeskPlus.Account.Web.Admin.CSV.PictureSeal" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        ** /DeskPlusEIP/FileData/Photo 폴더 경로에 LoginID.jpg 파일형태로 넣어주세요.<br />
        <br />
        <asp:Button ID="btnPhoto" runat="server" OnClick="btnPhoto_Click" Text="사진 업로드" />
        <br />
        <br />
        <br />
        <asp:Literal ID="ltlPhoto" runat="server"></asp:Literal>
        <br />
        <br />
        <br />
        ==========================================================================<br />
        <br />
        <br />
        ** /DeskPlusEIP/FileData/Dojang 폴더 경로에 LoginID.jpg 파일형태로 넣어주세요.<br />
        <br />
        <asp:Button ID="btnDojang" runat="server" OnClick="btnDojang_Click" Text="도장 업로드" />
        <br />
        <br />
        <asp:Literal ID="ltlDojang" runat="server"></asp:Literal>
        <br />
        <br />
        <br />
        ==========================================================================<br />
        <br />
        <br />
        ** 사용자별 기본 폴더 생성하기<br />
        <br />
        <asp:Button ID="btnCreateFolder" runat="server" OnClick="btnCreateFolder_Click" Text="폴더생성하기" />
        <br />
        <br />
        <br />
        <asp:Literal ID="ltlCreateFolder" runat="server"></asp:Literal>
        <br />
    </div>
    </form>
</body>
</html>