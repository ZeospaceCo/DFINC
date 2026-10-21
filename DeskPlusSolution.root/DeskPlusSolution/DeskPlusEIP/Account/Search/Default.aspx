<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeospace.DeskPlus.Account.Web.Search.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Frameset//EN" "http://www.w3.org/TR/xhtml1.DTD/xhtml1-frameset.dtd"> 

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript">
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
    </script>
</head>
<frameset cols="240,*" border="0">
    <frame name="frmLeft" id="frmLeft" frameborder="0" src="./Tree.aspx"></frame>
    <frame name="frmContent" id="frmContent" frameborder="0" src="./List.aspx"></frame>
</frameset>
</html>
