<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Admin.Auth.Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Frameset//EN" "http://www.w3.org/TR/xhtml1.DTD/xhtml1-frameset.dtd"> 
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta charset="UTF-8">
<title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
<link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
<link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_EIP.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_EIP.css" />
</head>
<frameset cols="240,*" border="0">
    <frame name="frmLeft" id="frmLeft" frameborder="0" src="Tree.aspx"></frame>
    <frame name="frmRight" id="frmRight" frameborder="0" src="Add.aspx"></frame>
</frameset>
</html>
