<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminDefault.aspx.cs" Inherits="Zeospace.DeskPlus.ETC.Web.Attendance.AdminDefault" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Frameset//EN" "http://www.w3.org/TR/xhtml1.DTD/xhtml1-frameset.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head><meta charset="UTF-8">
<link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
<link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_ETC.css" />
<link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_ETC.css"/>
<title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
</head>
<frameset cols="240,*" border="0">
    <frame name="frmLeft" id="frmLeft" frameborder="0" src="AdminTree.aspx"></frame>
    <frame name="frmContent" id="frmContent" frameborder="0" src="AdminRole.aspx"></frame>
</frameset>
</html>
