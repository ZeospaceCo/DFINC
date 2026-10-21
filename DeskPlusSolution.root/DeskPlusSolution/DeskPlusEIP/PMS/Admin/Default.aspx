<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeospace.DeskPlus.PMS.Web.Admin.Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Frameset//EN" "http://www.w3.org/TR/xhtml1.DTD/xhtml1-frameset.dtd"> 
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="UTF-8"> 
	<title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
</head>
<frameset cols="240,*" border="0">
	<frame id="menuFrm" name="menuFrm" frameborder="0" src="./Content/AdminMenu.aspx" />
	<frame id="contentFrm" name="contentFrm" frameborder="0" src="./Content/JListOpen.aspx"/>
</frameset>
</html>