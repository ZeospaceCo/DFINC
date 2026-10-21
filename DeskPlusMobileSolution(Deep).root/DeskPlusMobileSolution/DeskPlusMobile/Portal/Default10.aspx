<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default10.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Portal.Default10" %>
<!DOCTYPE html>
<html lang="ko">
<head>
<title></title>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta name="viewport" content="width=device-width, initial-scale=1">
<meta name="apple-mobile-web-app-capable" content="no" />
<meta name="apple-mobile-web-app-status-bar-style" content="default" />
<meta name="format-detection" content="telephone=no" />
<link rel="shortcut icon" href="/DeskPlusMobile/Content/ko/Base/Img/icon.png" />
<link rel="apple-touch-icon" href="/DeskPlusMobile/Content/ko/Base/Img/icon.png"/>
<link rel="apple-touch-startup-image" href="/DeskPlusMobile/Content/ko/Base/Img/icon.png" />
<link rel="apple-touch-icon-precomposed" href="/DeskPlusMobile/Content/ko/Base/Img/icon.png" />
<link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/Base/Css/mobile_Default10.css"/>
</head>
<body>
    <form id="form1" runat="server" AUTOCOMPLETE="off" onsubmit="return LoginProcess();">
  <div id="warp">
	<div id="header_content_warp">
	<!-- Header -->
	<div id="header">
		<div id="companylog"></div>
	</div>

    <!-- Content -->
	<div id="main-content">
		<div id="mid_text" class="show"></div>
	</div>
	</div> 

   <!-- Footer -->
	<div id="footer">
		<div id="login_warp" class="login_m">
			<div id="fom_warp">
				<div id="fom_1">
					<div><span id="move_id"><%=GetResource("ID")%></span><asp:TextBox ID="tbLoginID" TabIndex="1" CssClass="il" onfocus="this.value='';" runat="server"></asp:TextBox></div>
					<div><span id="move_pw"><%=GetResource("Password")%></span><asp:TextBox ID="tbPassword" TabIndex="2" CssClass="pl" runat="server" TextMode="Password"></asp:TextBox></div>
					<div><span id="move_com"><%=GetResource("Company")%></span>
                       <asp:DropDownList ID="ddlCompany" name="ComSel" runat="server" ></asp:DropDownList>
					</div>
					<a onclick="" id="submit" href="#" class="login_bt"></a>
				</div>
				<div id="fom_2">
                    <asp:CheckBox ID="chkCompany" name="ComSave" TabIndex="3" runat="server" Visible="true"></asp:CheckBox><span id="com_font"><%=GetResource("SaveCompany")%></span>
					<asp:CheckBox ID="chkRemember" name="IDSave"  TabIndex="3" runat="server" Visible="true"></asp:CheckBox>
                    <span id="id_font"><%=GetResource("SaveLogin")%></span>

                    <div style="display:inline-block;float:right;padding-left:40px;">
                    <span id="spAndApp" style="display:none;">&nbsp;<a href="http://msp.deskplus.co.kr/DeskPlusMobile/SpDeskplus.apk" target="_blank" ><button type="button" class="app"><%=GetResource("AppDownload")%></button></a></span>
                    <span id="spIOSApp" style="display:none; ">&nbsp;<a href="itms-services://?action=download-manifest&url=https://m.zeospace.com/ios/SpDeskplus/SpDeskplus.plist" ><button type="button" class="app"><%=GetResource("AppDownload")%></button></a></span>
					</div>
					<div id="txtMessage" style="padding-top:5px;"></div>
				</div>
			</div>
		</div>
	</div>
</div>
    </form>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Portal/Js/jquery.min.js"></script>
    <script type="text/javascript">
        // 모바일기기로만 접속 가능
        /*
        var UserAgent = navigator.userAgent;
        //alert(UserAgent);
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
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/Portal/Default.js");
    </script> 
</body>
</html>
