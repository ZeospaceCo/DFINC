<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sample.aspx.cs" Inherits="Zeospace.DeskPlus.Base.Web.Util.uploadJquery.Sample" %>

<!DOCTYPE html>

<html>
<head runat="server"><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
     <script src="jquery.js" type="text/javascript"></script>
        <script src="jquery.MultiFile.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <asp:FileUpload ID="FileUpload1" runat="server" class="multi" />
        <br />
        <asp:Button ID="btnUpload" runat="server" Text="Upload All"
            onclick="btnUpload_Click" />
    </div>
    </form>
</body>
</html>
