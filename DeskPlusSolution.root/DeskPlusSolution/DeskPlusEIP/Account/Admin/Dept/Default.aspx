<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeospace.DeskPlus.Account.Web.Admin.Dept.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Frameset//EN" "http://www.w3.org/TR/xhtml1.DTD/xhtml1-frameset.dtd"> 
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title> 
</head>
<frameset cols="240,*" border="0">
    <frame name="frmFolderLeft" id="frmLeft" frameborder="0" src="<%=LeftURL%>"></frame>
    <frame name="frmFolderContent" id="frmRight" frameborder="0" src="<%=ContentURL%>"></frame>
</frameset>
<noframes></noframes>
</html>