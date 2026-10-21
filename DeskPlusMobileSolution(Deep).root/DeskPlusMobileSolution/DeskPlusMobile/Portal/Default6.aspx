<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default6.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Portal.Default6" %>
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
<link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/Base/Css/mobile_Default6.css"/>
</head>
<body>
    <form id="form1" runat="server" AUTOCOMPLETE="off" onsubmit="return LoginProcess();">
    <div id="warp">
	<!-- Header -->
	<div id="header">
		<div id="companylog" class="show"></div>
	</div>

    	<!-- Content -->
	<div id="main-content">
		<div id="mid_img" class="show"></div>

		<div id="login_warp">
			<div id="fom_img"></div>
			<div id="fom_warp">
			<div id="fom_1">
				<div><%=GetResource("ID")%> <asp:TextBox ID="tbLoginID" TabIndex="1" CssClass="il" onfocus="this.value='';" runat="server"></asp:TextBox></div>
				<div><%=GetResource("Password")%> <asp:TextBox ID="tbPassword" TabIndex="2" CssClass="pl" runat="server" TextMode="Password"></asp:TextBox></div>
				<div><%=GetResource("Company")%>
                    <asp:DropDownList ID="ddlCompany" runat="server"></asp:DropDownList>
				</div>
				<div class="save">
					<asp:CheckBox ID="chkCompany" TabIndex="3" runat="server" Visible="true"></asp:CheckBox><span id="com_font"><%=GetResource("SaveCompany")%></span>
					<asp:CheckBox ID="chkRemember" TabIndex="3" runat="server" Visible="true"></asp:CheckBox><span id="id_font"><%=GetResource("SaveLogin")%></span>
				</div>
			</div>
			<div id="fom_2" style="border:0px solid red;display:block;">

                <span id="spAndApp" style="display:none;">&nbsp;<a href="http://msp.deskplus.co.kr/DeskPlusMobile/SpDeskplus.apk" target="_blank" ><button type="button" class="app"><%=GetResource("AppDownload")%></button></a></span>
                <span id="spIOSApp" style="display:none;">&nbsp;<a href="itms-services://?action=download-manifest&url=https://m.zeospace.com/ios/SpDeskplus/SpDeskplus.plist" ><button type="button" class="app"><%=GetResource("AppDownload")%></button></a></span>

				<a onclick="" id="submit" href="#" class="login_bt"></a>
			</div>
			</div>
		</div>
		<div id="txtMessage" align="center"></div>
		<div id="mid_text" class="show"></div>
	</div>


	<!-- Footer -->
	<div id="footer">
		<div id="foot_text">
			<div>
				<p>COPY RIGHT ALL RIGHT RESERVED HANTHEME</p>
				<p>서울시 구로구 다지털로33길 28 우림e-biz센터 1차 507호 TEL:02-2108-3070  FAX:02-2108-3080</p>
				<p>대표상담번호:02-2108-3070</p>
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


