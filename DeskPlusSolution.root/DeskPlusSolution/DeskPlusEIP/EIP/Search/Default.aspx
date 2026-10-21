<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Search.Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Frameset//EN" "http://www.w3.org/TR/xhtml1.DTD/xhtml1-frameset.dtd"> 
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript">
        var SearchValue = "<%=SearchValue %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";

        var R_InputSearchEngine = "<%=GetResource("InputSearchEngine") %>";
        var R_CheckSearchSection = "<%=GetResource("CheckSearchSection") %>";
        var R_InputOrderWay = "<%=GetResource("InputOrderWay") %>";

        $(document).ready(function () {         
            //CallEncrypt("searchValue=" + SearchValue);
            //$("#frmFolderLeft")[0].src = "./Help.aspx";
            //$("#frmFolderContent")[0].src = "./List.aspx?p=" + GetEncryptValue;
        }); 
    </script>
</head>
<frameset cols="0,*" border="0">
    <frame name="frmFolderLeft" id="frmFolderLeft" frameborder="0" src="./Help.aspx"></frame>
    <frame name="frmFolderContent" id="frmFolderContent" frameborder="0" src="<%=ContentURL%>"></frame>
</frameset>
</html>
