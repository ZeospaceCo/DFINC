<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserDefault.aspx.cs" Inherits="Zeospace.DeskPlus.ETC.Web.Attendance.UserDefault" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Frameset//EN" "http://www.w3.org/TR/xhtml1.DTD/xhtml1-frameset.dtd"> 
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title> 
</head>
<frameset cols="230,*" border="0">
    <frame name="frmFolderLeft" id="frmLeft" frameborder="0" src="UserTree.aspx"></frame>
    <frame name="frmFolderContent" id="frmRight" frameborder="0" src="about:blank"></frame>
</frameset>
<noframes></noframes>
</html>
