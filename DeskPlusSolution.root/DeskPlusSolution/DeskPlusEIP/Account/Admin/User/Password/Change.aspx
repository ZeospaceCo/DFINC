<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Change.aspx.cs" Inherits="Zeospace.DeskPlus.Account.Web.Admin.User.Password.Change" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Account.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Account.css" />    
</head>
<body>
    <form id="form1" runat="server">

<div class="layer_popup">
    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("Admin_UserRegister_PasswordChange")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="CbtnConfirm" runat="server" Class="btn btn-secondary" /></li>
                <li><cc3:ClientSmartButton ID="CbtnClose" runat="server"/></li>
            </ul>
        </div>
        <div class="dotLine"></div>        
        <div class="popup_layout_style3">

            <div>* 8~13 글자, 영문(대문자, 소문자), 숫자, 특수문자 2자리(!@%^*()-_=) 포함하여 입력하세요.
            <!--<%=GetResource("PwdLength")%>(<%=PwdLength%>),<%=GetResource("PwdLower")%>(<%=PwdLower%>),<%=GetResource("PwdUpper")%>(<%=PwdUpper%>),<%=GetResource("PwdNumeric")%>(<%=PwdNumeric%>),<%=GetResource("PwdSpecial")%>(<%=PwdSpecial%>)--></div>

            <table class="table_typeB">
			<caption></caption>
			<colgroup>
                <col width="120px"/>
                <col />
            </colgroup>
            <tbody>
                <tr>
                    <th><%=GetResource("Admin_UserManager_ltPassword")%></th>
                    <td><cc3:ClientSmartTextBox runat="server" ID="ctbPwd" Width="100px" TextMode="Password" InputMode="None" CssStyle="width:90%;IME-MODE:disabled;"></cc3:ClientSmartTextBox></td>
                </tr>
                <tr>
                    <th><%=GetResource("Admin_UserManager_ltPasswordConfirm")%></th>
                    <td><cc3:ClientSmartTextBox runat="server" ID="ctbPwdConfirm" Width="100px"  TextMode="Password" InputMode="None" CssStyle="width:90%;IME-MODE:disabled;"></cc3:ClientSmartTextBox></td>
                </tr>
            </tbody>
            </table>
            
        </div>
    </div>
</div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script> 
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>  
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.password.js"></script>         
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CompanyID = "<%=CompanyID %>";
        var UserID = "<%=UserID %>";
        var MailID = "<%=MailID %>";
        var ImagePath = "<%=BaseImgPath%>";
        var MailDomain = "<%=MailDomain %>"
        var PreLang = "<%=PreLang%>";

        var PwdLength = "<%=PwdLength%>";
        var PwdLower = "<%=PwdLower%>";
        var PwdUpper = "<%=PwdUpper%>";
        var PwdNumeric = "<%=PwdNumeric%>";
        var PwdSpecial = "<%=PwdSpecial%>";        

        var R_PwdRegex = "<%=GetResource("PwdRegex")%>";
        var R_InputPassword = "<%=GetResource("InputPassword") %>";
        var R_InputMailPassword = "<%=GetResource("InputMailPassword") %>";
        var R_Admin_DeptManager_MSG_SUCCESS = "<%=GetResource("Admin_DeptManager_MSG_SUCCESS") %>";
        var R_DenyConfirm = "<%=GetResource("DenyConfirm") %>";
        var R_NoMatchPwd = "<%=GetResource("NoMatchPwd") %>";
    </script>
    <script type="text/javascript" src="../../../Js/Admin/User/Password/Change.js"></script>         
    <script type="text/javascript">
        //JsCall("../../../Js/Admin/User/Password/Change.js");
    </script>    
</body>
</html>
