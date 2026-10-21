<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="Zeospace.DeskPlus.Account.Web.Admin.CSV.User" %>

<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
    <div>
    
        <br />
        1. 부서 추가 : <a href="Data/dept.csv">샘플 (dept.csv)</a>  <br />
        <br />
        <br />
        <asp:FileUpload ID="fuDept" runat="server" />
        <br />
        <br />
        <asp:Button ID="btnDept" runat="server" onclick="btnDept_Click" Text="추가" />
        <br />
        <br />
        <asp:Literal ID="ltDept" runat="server"></asp:Literal>
        <br />
        <br />
        <br />
        2. 직급 추가 : <a href="Data/title.csv">샘플 (title.csv)</a><br />
        <br />
        <br />
        <asp:FileUpload ID="fuTitle" runat="server" />
        <br />
        <br />
        <asp:Button ID="btnTitle" runat="server" onclick="btnTitle_Click" Text="추가" />
        <br />
        <br />
        <asp:Literal ID="ltTitle" runat="server"></asp:Literal>
        <br />
        <br />
        <br />
        3. 직책 추가 : <a href="Data/duty.csv">샘플 (duty.csv)</a><br />
        <br />
        <br />
        <asp:FileUpload ID="fuDuty" runat="server" />
        <br />
        <br />
        <asp:Button ID="btnDuty" runat="server" onclick="btnDuty_Click" Text="추가" />
        <br />
        <br />
        <asp:Literal ID="ltDuty" runat="server"></asp:Literal>
        <br />
        <br />
        <br />
        4. 사용자 추가 : <a href="Data/user.csv">샘플 (user.csv)</a><br />
        <br />
        <br />
        <asp:FileUpload ID="fuUser" runat="server" />
        <br />
        <br />
        <asp:Button ID="btnUser" runat="server" onclick="btnUser_Click" Text="추가" />
        <br />
        <br />
        <asp:Literal ID="ltUser" runat="server"></asp:Literal>
        <br />
        <br />
    
    </div>
    </form>
</body>
</html>
