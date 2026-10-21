<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Frameset//EN" "http://www.w3.org/TR/xhtml1.DTD/xhtml1-frameset.dtd"> 
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript">
        var SelectDate = "<%=SelDate%>";
        function MovePage(url) {
            document.location.href = url;
        }
        function ChildCommunication(arg) {


        }
    </script>
</head>

<frameset cols="240,*" border="0">
    <frame id="frmLeft" name="frmLeft" frameborder="0" src="/DeskPlusEIP/Pim/Menu/PimMenu.aspx?p=<%=SelDate%>"></frame>    
    <frame id="frmContent" name="frmContent" frameborder="0" src="/DeskPlusEIP/Pim/Schedule/ScheduleMain.aspx?p=<%=SelDate%>" class="frmContent"></frame>
</frameset><noframes></noframes>

</html>
