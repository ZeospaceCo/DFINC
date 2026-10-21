<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListMonthExcel.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Admin.Statistics.ListMonthExcel" %>

<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" /> 
</head>
<body>
    <form id="form1" runat="server">
    <div><asp:Literal ID="ltCont" runat="server"></asp:Literal></div>
    </form>
</body>
</html>
