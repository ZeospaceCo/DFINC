<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Portal.Default" %>
<!DOCTYPE html>
<html lang="ko">
<head>
<title></title>
<meta charset="uft8" />
<meta name="viewport" content="width=device-width, initial-scale=1; maximum-scale=1.0; minimum-scale=1.0; user-scalable=no;" />
<meta name="apple-mobile-web-app-capable" content="no" />
<meta name="apple-mobile-web-app-status-bar-style" content="default" />
<meta name="format-detection" content="telephone=no" />

<link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/ml.css"/>
<link rel="shortcut icon" href="data:image/x-icon;," type="image/x-icon">  

<script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.js"></script>
<script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.common.js"></script>
<script type="text/javascript" src="/DeskPlusMobile/Html/Js/validator.js"></script>
<script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/bootstrap.min.js"></script>

<script type="text/javascript">
    /* 
    //모바일기기로만 접속 가능
    var UserAgent = navigator.userAgent;
    if (UserAgent.match(/iPhone|iPod|iPad|Android|Windows CE|BlackBerry|Symbian|Windows Phone|webOS|Opera Mini|Opera Mobi|POLARIS|IEMobile|lgtelecom|nokia|SonyEricsson/i) || UserAgent.match(/LG|SAMSUNG|Samsung/)) {

    }
    else {
        location.href = "ForMobileOnly.htm";
    }
    */
    var R_InputLoginID = "<%=GetResource("InputLoginID") %>";
    var R_InputPwd = "<%=GetResource("InputPwd") %>";
    var R_AuthFail = "<%=GetResource("AuthFail") %>";
    var R_AuthWait = "<%=GetResource("AuthWait") %>";
    var R_ComfirmAuth = "<%=GetResource("ComfirmAuth") %>";
    var R_AccessRight = "<%=GetResource("AccessRight") %>";
</script>  
<script type="text/javascript" src="/DeskPlusMobile/Js/Portal/Default.js"></script>
</head>
<body class="login">
    <div class="loginimgoverbox"></div>
    <div class="login-logo-area">
      <div class="logo"><a><img src="<%=LoginImage%>" alt="" /></a></div>
    </div>
    <div class="container">
        <div class="login-login-box ">
            <form id="form1" runat="server" autocomplete="off" onsubmit="return LoginProcess();" class="form-signin">
                <asp:TextBox ID="tbLoginID" TabIndex="1" class="form-control" runat="server"></asp:TextBox>
                <asp:TextBox ID="tbPassword" TabIndex="2" class="form-control" runat="server" TextMode="Password"></asp:TextBox>
                <div class="input-group mb-3">
                    <div class="input-group-prepend">
                        <label class="input-group-text" for="inputGroupSelect01"><%=GetResource("Company")%></label>
                    </div>
                    <asp:DropDownList ID="ddlCompany" runat="server" class="custom-select"></asp:DropDownList>
                </div>
                <div class="checkbox mb-3">
                    <label><asp:CheckBox ID="chkRemember" TabIndex="3" runat="server" Visible="true"></asp:CheckBox><%=GetResource("SaveLogin")%></label>
                    <label><asp:CheckBox ID="chkCompany" TabIndex="3" runat="server" Visible="true"></asp:CheckBox><%=GetResource("SaveCompany")%></label>
                </div>
                <input type="submit" value="<%=GetResource("Login")%>" id="submit" class="btn btn-lg btn-primary btn-block" style="margin-bottom:10px;" />
                <div>
                    <span id="spAndApp" class="full-width">
                        <a href="https://play.google.com/store/apps/details?id=com.zeospace.deskplusad" data-ajax="false">
                            <button type="button" class="btn btn-xs btn-outline-light full-width">Android App</button>
                        </a>
                    </span>
                </div>
                <div id="txtMessage">&nbsp;</div>
            </form>
        </div>
    </div>

    <!-- -->

    
</body>
</html>