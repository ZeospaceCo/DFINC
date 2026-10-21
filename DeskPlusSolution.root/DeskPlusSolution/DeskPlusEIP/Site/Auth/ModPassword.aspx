<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModPassword.aspx.cs" Inherits="Zeospace.DeskPlus.Base.Web.Site.Auth.ModPassword" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
	<meta name="robots" content="noindex">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Design/C1/Css/DeskPlusEIPBase.css"/>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>  
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.password.js"></script>
    <script type="text/javascript" >
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/        
        var R_InputNowPWd = "<%=GetResource("InputNowPWd")%>";
        var R_InputNewPWd= "<%=GetResource("InputNewPWd")%>";
        var R_InputNewPWd1= "<%=GetResource("InputNewPWd1")%>";
        var R_InputComparePwd= "<%=GetResource("InputComparePwd")%>";
        var R_PwdRegex = "<%=GetResource("PwdRegex")%>";
        
        var PwdLength = "<%=PwdLength%>";
        var PwdLower = "<%=PwdLower%>";
        var PwdUpper = "<%=PwdUpper%>";
        var PwdNumeric = "<%=PwdNumeric%>";
        var PwdSpecial = "<%=PwdSpecial%>";        
        var PwdFirst = "<%=PwdFirst%>";

    </script>
    <script type="text/javascript" src="../../Js/Site/Auth/ModPassword.js"></script>
    <script type="text/javascript">
        //JsCall("../../Js/Site/Auth/ModPassword.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
<div class="layer_popup">
    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("ModPassword")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnClose" runat="server" /></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3" style="min-height:330px;height:330px;">
<!--        <%=GetResource("ModPasswordTitle1")%><br />
        <%=GetResource("ModPasswordTitle2")%><br /><br />
        <%=GetResource("ModPasswordTitle3")%><br /><br />
-->
<span style="font-weight:bold;">정보보안 정책에 의하여 변경 전 패스워드와 동일하게 입력이 불가 합니다.</span><br />
<span style="color:#666666">(패스워드 변경 시 전체 전산시스템 패스워드가 동일하게 변경됩니다.)</span><br />
현재 비밀번호를 입력한 후 새로 사용할 비밀번호를 입력하세요.<br />
아이디와 같은 비밀번호나 주민등록번호, 생일, 학번, 전화번호 등 개인정보와 관련된 숫자, 연속된 숫자, 동일 반복된 숫자, 초기 패스워드 등<br />
다른 사람이 쉽게 알아 낼 수 있는 비밀번호는 사용하지 마세요.<br />
<span style="font-weight:bold;color:green;">* 쉬운 비밀번호나 자주 접속하는 사이트의 비밀번호가 같으면 도용되기 쉬우므로 주기적으로 바꿔쓰는 것이 좋습니다. </span><br /><br />
<span style="font-weight:bold;color:Navy;">* 8~13 글자, 영문(대문자, 소문자), 숫자, 특수문자 2자리(!@%^*()-_=) 포함하여 입력하세요.
		<!-- <%=GetResource("PwdLimit")%> : <%=GetResource("PwdLength")%>(<%=PwdLength%>),<%=GetResource("PwdLower")%>(<%=PwdLower%>),<%=GetResource("PwdUpper")%>(<%=PwdUpper%>),<%=GetResource("PwdNumeric")%>(<%=PwdNumeric%>),<%=GetResource("PwdSpecial")%>(<%=PwdSpecial%>)--></span>
        <table class="table_typeB" border="0" style="width:98%;" > 
		<caption><%=GetResource("ModPassword")%></caption>
		<colgroup>
            <col width="120px" />
            <col />
		</colgroup>
		<tbody>
        <tr>
            <th><%=GetResource("PwdNow")%></th>
            <td><cc3:ClientSmartTextBox ID="ctbPassword" InputMode="None" MaxLength="250" runat="server" TextMode="Password" Required="true" CssStyle="width:98% !important;IME-MODE:disabled;"></cc3:ClientSmartTextBox></td>
        </tr>
        <tr>
            <th><%=GetResource("PwdNew")%></th>
            <td><cc3:ClientSmartTextBox ID="ctbNewPassword" InputMode="None" MaxLength="250" runat="server"  TextMode="Password" Required="true" CssStyle="width:98% !important;IME-MODE:disabled;"></cc3:ClientSmartTextBox></td>
        </tr>
        <tr>
            <th><%=GetResource("PwdNewConfirm")%></th>
            <td><cc3:ClientSmartTextBox ID="ctbNewPassword1" InputMode="None" MaxLength="250" runat="server"  TextMode="Password" Required="true" CssStyle="width:98% !important;IME-MODE:disabled;"></cc3:ClientSmartTextBox></td>
        </tr>
        </tbody>
        </table>
        </div>
    </div>
    
</div>
    <div style="display:none;">
    <asp:LinkButton ID="lbtnSubmit" OnClick="lbtnSubmit_Click" runat="server"></asp:LinkButton>
    </div>
    </form>
</body>
</html>
