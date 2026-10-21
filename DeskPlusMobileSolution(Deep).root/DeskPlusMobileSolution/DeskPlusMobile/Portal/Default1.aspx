<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default1.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Portal.Default1" %>
<!DOCTYPE html>
<html lang="ko">
<head>
<title></title>
<meta charset="uft8" />
<meta name="viewport" content="initial-scale=1.0; maximum-scale=1.0; minimum-scale=1.0; user-scalable=no;" />
<meta name="apple-mobile-web-app-capable" content="no" />
<meta name="apple-mobile-web-app-status-bar-style" content="default" />
<meta name="format-detection" content="telephone=no" />
<link rel="shortcut icon" href="/DeskPlusMobile/Content/ko/Base/Img/icon.png" />
<link rel="apple-touch-icon" href="/DeskPlusMobile/Content/ko/Base/Img/icon.png"/>
<link rel="apple-touch-startup-image" href="/DeskPlusMobile/Content/ko/Base/Img/icon.png" />
<link rel="apple-touch-icon-precomposed" href="/DeskPlusMobile/Content/ko/Base/Img/icon.png" />
<link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/Base/Css/mobile_Default1.css"/>
</head>
<body>
   <form id="form1" runat="server" AUTOCOMPLETE="off" onsubmit="return LoginProcess();">
    <div class="warp">
  <!-- Header -->
	<div id="header">
		<span id="companylog"></span>
	</div>

    <!-- Content -->
	<div id="main-content">
		<div id="mid_top_img" class="posi_ab"></div>
		<div id="mid_bot_img" class="posi_ab"></div>
		
		<div id="login_warp" class="posi_ab">
			<div id="fom_warp">
			<div id="fom_1">
				<div class="text1"><%=GetResource("ID")%>&nbsp;<asp:TextBox ID="tbLoginID" TabIndex="1" CssClass="il" onfocus="this.value='';" runat="server"></asp:TextBox></div>
				<div class="text1"><%=GetResource("Password")%> <asp:TextBox ID="tbPassword" TabIndex="2" CssClass="pl" runat="server" TextMode="Password"></asp:TextBox></div>
				<div class="text1"><%=GetResource("Company")%>&nbsp;<asp:DropDownList ID="ddlCompany" runat="server"></asp:DropDownList></div>
				<div class="save">
                    <asp:CheckBox ID="chkCompany"  name="ComSave" TabIndex="3" runat="server" Visible="true"></asp:CheckBox><span id="com_font"><%=GetResource("SaveCompany")%></span>
					<asp:CheckBox ID="chkRemember" name="IDSave" TabIndex="3" runat="server" Visible="true"></asp:CheckBox><span id="id_font"><%=GetResource("SaveLogin")%></span>
				</div>
			</div>
			<div id="fom_line"></div>
			<div id="fom_2">

                <span id="spAndApp" style="display:none;">&nbsp;<a href="http://msp.deskplus.co.kr/DeskPlusMobile/SpDeskplus.apk" target="_blank" ><button type="button" class="app"><%=GetResource("AppDownload")%></button></a></span>
                <span id="spIOSApp" style="display:none;">&nbsp;<a href="itms-services://?action=download-manifest&url=https://m.zeospace.com/ios/SpDeskplus/SpDeskplus.plist" ><button type="button" class="app"><%=GetResource("AppDownload")%></button></a></span>
				
                <a onclick="" id="submit" href="#" class="login_bt"></a>

			</div>
			</div>
			<br/>
			<div id="txtMessage" style="padding-left:10px;"></div>
		</div>
		
	</div>
	
   
    <!-- Footer -->
    <div id="footer">
		<div id="foot_text" class="posi_ab">
			<div>
				<p>서울시 구로구 다지털로33길 28 우림e-biz센터 1차 507호 TEL:02-2108-3070  FAX:02-2108-3080</p>
				<p>대표상담번호:02-2108-3070</p>
				<p>COPY RIGHT ALL RIGHT RESERVED HANTHEME</p>
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
