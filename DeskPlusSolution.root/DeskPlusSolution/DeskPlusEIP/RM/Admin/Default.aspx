<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeospace.DeskPlus.RM.Web.Admin.Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Frameset//EN" "http://www.w3.org/TR/xhtml1.DTD/xhtml1-frameset.dtd"> 
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="UTF-8"> 
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_RM.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_RM.css"/>
</head>
<frameset cols="240,*" border="0">
    <frame src="./Folder/Tree.aspx" name="frmLeft" frameborder="0">
    <frame src="./Content/List.aspx" name="frmContent" frameborder="0">
</frameset>
</html>
