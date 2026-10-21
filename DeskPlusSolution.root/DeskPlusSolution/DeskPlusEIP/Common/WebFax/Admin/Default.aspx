<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeospace.DeskPlus.Common.Web.WebFax.Admin.Default" %>

<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript">
        $(function () {
            $(document).ready(function () {

                var url = "Tree.aspx";
                $("#frmLeft")[0].src = url;
            });
        });
    </script>    
</head>
<frameset cols="230,*" border="0">
    <frame name="frmLeft" id="frmLeft" frameborder="0" src="about:blank"></frame>
    <frame name="frmFolderContent" id="frmFolderContent" frameborder="0" src="about:blank"></frame>
</frameset>
</html>
