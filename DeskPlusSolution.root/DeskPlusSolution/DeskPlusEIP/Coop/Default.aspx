<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeospace.DeskPlus.Coop.Web.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Frameset//EN" "http://www.w3.org/TR/xhtml1.DTD/xhtml1-frameset.dtd"> 
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript">       
      
    </script>
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Coop/Js/Default.js");
    </script>
</head>
<frameset cols="230,*" border="0">
    <frame name="frmLeft" id="frmLeft" frameborder="0" src="about:blank"></frame>
    <frame name="frmContent" id="frmContent" frameborder="0" src="about:blank"></frame>
</frameset>
</html>
