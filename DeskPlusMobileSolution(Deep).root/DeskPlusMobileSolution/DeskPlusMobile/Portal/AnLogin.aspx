<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AnLogin.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Portal.AnLogin" %>
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
    var RegID = "";
    var SaveLID = "";
    var SaveCID = "";
    var UUID = "";
    var ApproveMobile = -1;
    var LoginType = 0;
	var AccessRight = 0;
    var RegCnt = 1;

</script>  
<script type="text/javascript">
    JsCall("/DeskPlusMobile/Js/Portal/AnLogin.js");
</script> 
</head>
<body class="login" onload="get_value()">
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
                <input type="submit" value="<%=GetResource("Login")%>" id="submit" class="btn btn-lg btn-primary btn-block" style="margin-bottom:10px;" />
                <div id="txtMessage">&nbsp;</div>
            </form>
        </div>
    </div>    
</body>
</html>