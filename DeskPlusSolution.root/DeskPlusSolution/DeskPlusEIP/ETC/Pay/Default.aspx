<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeospace.DeskPlus.ETC.Web.Pay.Default" %>

<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_ETC.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_ETC.css"/>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <title>
        <%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <script type="text/javascript">
        var FolderStatusID = "<%=FolderStatusID %>";        
        var FormID = "<%=FormID %>";

        jQuery(document).ready(function () {

            ParamArray = {
                "folderStatusID": FolderStatusID,                
                "formID": FormID
            };

            CallEncrypt(CallGetParam());

            $("#frmLeft")[0].src = "./Menu/Tree.aspx?p=" + GetEncryptValue;
        }); 
    </script>
</head>
<frameset cols="240,*" border="0">
    <frame name="frmLeft" id="frmLeft" frameborder="0" src="about:blank"></frame>
    <frame name="frmContent" id="frmContent" frameborder="0" src="about:blank"></frame>
</frameset>
</html>