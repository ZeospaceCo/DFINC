<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeospace.DeskPlus.Search.Web.Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Frameset//EN" "http://www.w3.org/TR/xhtml1.DTD/xhtml1-frameset.dtd"> 
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        var ModuleName = "<%=ModuleName %>";
        var SearchValue = "<%=SearchValue %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var LoginID = "<%=CurrentUserInfo.LogInID %>";
        var Password = "<%=CurrentUserInfo.Password %>";

        $(document).ready(function () {
            
            ParamArray = {
                'moduleName': ModuleName,
                'searchValue': SearchValue
            };

            CallEncrypt(CallGetParam());

            $("#frmFolderLeft")[0].src = "./Help.aspx";
            $("#frmFolderContent")[0].src = "./List.aspx?p=" + GetEncryptValue;

            ParamArray = {
                'CompanyID': CompanyID,
                'USERID': LoginID,
                'USERPASSWORD': Password
            };

            CallEncrypt(CallGetParam());

            var url = "/DeskPlusEDM/Authentication/LoginOutSide.aspx?p=" + GetEncryptValue;

            $("#frmFolderSLO")[0].src = url;
        }); 
    </script>
</head>
<frameset cols="0,0,*" border="0">
    <frame name="frmFolderSLO" id="frmFolderSLO" frameborder="0" src="about:blank"></frame>
    <frame name="frmFolderLeft" id="frmFolderLeft" frameborder="0" src="about:blank"></frame>
    <frame name="frmFolderContent" id="frmFolderContent" frameborder="0" src="about:blank"></frame>
</frameset>
</html>
