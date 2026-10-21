<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeospace.DeskPlus.Address.Web.Admin.Content.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Frameset//EN" "http://www.w3.org/TR/xhtml1.DTD/xhtml1-frameset.dtd"> 
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>    
	<script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>	
    <script type="text/javascript">
        var mainLayout;
        var westWidth = 220;
        var catID = "<%=CatID %>";
        var categoryID = "<%=CategoryID %>";

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";

        jQuery(document).ready(function () {            
            CallEncrypt("catID=" + catID + "&categoryID=" + categoryID);
            $("#frmFolderLeft")[0].src = "./Tree.aspx?p=" + GetEncryptValue;
            $("#frmFolderContent")[0].src = "./ListBlog.aspx?p=" + GetEncryptValue;
        }); 
    </script>
    
</head>
<frameset cols="240,*" border="0">
    <frame name="frmFolderLeft" id="frmFolderLeft" frameborder="0" src="about:blank"></frame>
    <frame name="frmFolderContent" id="frmFolderContent" frameborder="0" src="about:blank"></frame>
</frameset>
<noframes></noframes>
</html>

