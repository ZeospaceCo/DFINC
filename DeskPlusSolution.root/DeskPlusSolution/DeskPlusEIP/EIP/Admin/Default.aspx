<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Admin.Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Frameset//EN" "http://www.w3.org/TR/xhtml1.DTD/xhtml1-frameset.dtd"> 
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
</head>
<frameset rows="121,*" frameborder="0" bordercolor="#E9E9E9">
    <frame src="./Menu/AdminMenu.aspx" name="frmTop" style="margin:5 5 5 0" scrolling="auto" ></frame>
    <frame name="frmContent" id="frmContent" frameborder="0" src="./Menu/Add.aspx"></frame>
</frameset>
<body>
</body>
</html>