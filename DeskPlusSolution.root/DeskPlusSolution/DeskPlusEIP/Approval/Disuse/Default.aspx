<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Disuse.Default" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
</head>
<frameset cols="240,*" border="0">
    <frame name="frmLeft" id="frmLeft" frameborder="0" src="Tree.aspx"></frame>
    <frame name="frmContent" id="frmContent" frameborder="0" src="List.aspx"></frame>
</frameset>
</html>


