<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Target.Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Frameset//EN" "http://www.w3.org/TR/xhtml1.DTD/xhtml1-frameset.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript">
     function ChildCommunication(arg) {            
           $("#frmContent").attr("src", "List.aspx?p="+arg);
         
        }
        </script>
</head>
<frameset cols="240,*" border="0">
    <frame id="frmLeft" name="frmLeft" frameborder="0" src="Tree.aspx"></frame>
    <frame id="frmContent" name="frmContent" frameborder="0" src="List.aspx" class="frmContent"></frame>
</frameset><noframes></noframes>
</html>
