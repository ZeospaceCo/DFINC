<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeospace.DeskPlus.ETC.Web.Customize.Default" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Etc.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Etc.css"/>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>

    <script type="text/javascript">

//        $(function () {

//            ParamArray = {
//                //"folderStatusID": FolderStatusID
//            };

//            CallEncrypt(CallGetParam());

//            $("#frmLeft")[0].src = "./Menu/Menu.aspx?p=" + GetEncryptValue;
//        }); 
    </script>
</head>
<frameset cols="230,*" border="0">
    <frame name="frmLeft" id="frmLeft" frameborder="0" src="./Menu/Menu.aspx"></frame>
    <frame name="frmContent" id="frmContent" frameborder="0" src="./User/AddMaster.aspx"></frame>
</frameset>
</html>

