<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InGoogleStep1.aspx.cs" Inherits="Zeospace.DeskPlus.Address.Web.Outlook.InGoogleStep1" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Address.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Address.css"/>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
    <div class="layout_full">
    <div id="layout_header"></div>
    <div class="layout_srh clearfix">
        <div class="float-left"> 
            <ul id="ulTabMenu" class="tab">
            <li class="selected"><a href="#"><%=GetResource("GetGoogleMail")%></a></li>              
            </ul>
        </div>
    </div>
    <!-- //layout_header -->
    <div class="outmail">
        <p class="outmail_descrip"><%=GetResource("InGoogleStep1_Descript1")%><br />
           <%=GetResource("InGoogleStep1_Descript2")%></p>           
        <ul class="outmail_location">
            <li><a href="#" class="over"><%=GetResource("Load")%></a></li>
            <li><a href="#"><%=GetResource("UsePreview")%></a></li>
            <li class="last-child"><a href="#"><%=GetResource("Save")%></a></li>
        </ul>
        <div class="outmail_select">
            <span class="btnWrap">
                <label for="Id"><%=GetResource("ID")%></label><input type="text" id="Id" name="Id" />
                <label for="Pwd"><%=GetResource("PWD") %></label><input type="password" id="Pwd" name="Pwd"/>
            </span>
        </div>
        <div class="btnwrap_w ">
            <span class="btn_right">
                <a href="#" id="register" class="btn btn-secondary"><%=GetResource("GetAddressFile")%></a>
            </span>
        </div>
    </div>
    </div>
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>    
    <script type="text/javascript">
        var AccessToken = "<%=AccessToken %>";
        var WindowObjectReference; //새창용
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_InputGoogleMail = "<%=GetResource("InputGoogleMail") %>";
        var R_InputGooglePW = "<%=GetResource("InputGooglePW")%>";
        var R_AfterProcessingError = "<%=GetResource("AfterProcessingError") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Address/Js/Outlook/InGoogleStep1.js"></script>    
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Address/Js/Outlook/InGoogleStep1.js");
    </script>
</body>
</html>
