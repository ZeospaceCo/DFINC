<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShareUserPop.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Admin.Share.ShareUserPop" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>


<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Pim.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Pim.css" />    
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/AutoComplete.css" />
</head>
<body>
    <form id="form1" runat="server">
    <input type="text" id="dummy" style="display: none;" />
     
                               
                            <zeoTree:UCUserDeptTreeScript ID="UCUserDeptTreeScript2" runat="server" />
     
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js" ></script>    
    <script type="text/javascript">
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
    </script> 
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/Admin/Share/ShareUserPop.js" ></script>    
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Pim/Js/Admin/Share/ShareUserPop.js");
    </script>
</body>
</html>
