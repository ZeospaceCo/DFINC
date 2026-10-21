<%@ Page Language="C#" CodeBehind="Default2.aspx.cs" AutoEventWireup="true" Inherits="Zeospace.DeskPlus.Base.Web.Site.Auth.Default2" %>

<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
	<meta name="robots" content="noindex">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Login.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <style type="text/css">
.placeholder { color: #aaa; }

@font-face {font-family: "맑은 고딕", "돋움",Dotum,"굴림",Gulim,AppleGothic,Sans-serif;src:url(/DeskPlusEIP/Design/C/Css/NanumGothic.eot);}
body { margin:0px; padding:0px; background:#fff;}
input { vertical-align:middle; }
p { -webkit-margin-before: 10px !important; -webkit-margin-after: 10px !important; font-size: 12px; }

#warp { width: 1170px; position: relative; margin: 0 auto; }
#header { width: 1170px; height: 100px; position: relative; margin: 0 auto; }
#main-content { width: 1170px; height: 555px; position: relative; margin: 0 auto; }
#footer { width: 1170px; height: 108px; position: relative; margin: 0 auto; }

#login_warp {
	background:url('/DeskPlusEIP/Design/C/Img/LoginNew/2/2_07.png') no-repeat; background-size: cover;
	width:365px; height: 200px; position:absolute; font-size:12px; 
	left:9.5%; top:25%; font-family: '맑은 고딕', serif;
}
#fom_warp { position:relative; width: 310px; margin: 0 auto; text-align: left; margin-top: 20px;}
#fom_title { padding: 5px 0 0 10px; font-family: '맑은 고딕', serif; color: #fff; font-size: 12px;}
#fom_1 { width:310px; padding: 5px 0 10px 0; }
#fom_1 div {margin-bottom:8px; font-family: '맑은 고딕';}
#fom_1 > div > #move_id { margin-right: 22px; color: #16a3db; font-family: '맑은 고딕'}
#fom_1 > div > #move_pw { margin-right: 10px; color: #16a3db; font-family: '맑은 고딕'}
#fom_1 > div > #move_com { margin-right: 29px; color: #16a3db; font-family: '맑은 고딕'}

#fom_2 { width:280px; padding: 0; text-align: left; margin-top: 3px; margin-left: 120px;}

.fom {
	width:160px; height:20px; background:url(/DeskPlusEIP/Design/C/Img/LoginNew/2/fom.jpg) no-repeat cover; border:none;
	font: 12px '맑은 고딕'; color:#363636; padding:3px 5px 0px 5px;border: 1px solid #b1b1b1;
}
#fom_line { width:262px; height:8px; background:url(/DeskPlusEIP/Design/C/Img/LoginNew/2/2_03_line.png) no-repeat; }
#ComSel { width:172px; }
#ddlCompany { width:172px;  font-size:11px; height:22px;}
#FindIDPW { text-decoration: none; color: #393939;}
.login_bt{
	width:65px; height:80px; position:absolute; top: 2%; left: 78%;
	background:url(/DeskPlusEIP/Design/C/Img/LoginNew/2/2_05.png) no-repeat center center; cursor: pointer; border-radius: 3px;
}

.show { position: absolute; }
#mid_text { width:380px; height:60px; background:url(<%=LoginImage%>) no-repeat; left: 9.5%; top: 13%; border:0px solid red;}
#mid_img { width:700px; height:540px; background:url(/DeskPlusEIP/Design/C/Img/LoginNew/2/2_02.png) no-repeat; left: 40%; top: 10%;}
#mid_bottom_text {
	width:400px; height:70px; left: 10%; top: 56%; font-size: 12px; font-family: '맑은 고딕', serif; color: #aeaeae;
}
#foot_line { width:1000px; height:2px; background:url(/DeskPlusEIP/Design/C/Img/LoginNew/2/2_03_line.png) no-repeat; position:relative; margin:0 auto; }
#foot_text { width:820px; font-size: 12px; left: 9.5%; color: #787878; margin-top: 20px; font-family: '맑은 고딕', serif;}
    </style>
     <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.placeholder.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript">
        var R_PasswordFaultCount = "<%=GetResource("PasswordFault") %>"; // 비밀번호 틀렸습니다.        
        var R_PasswordFaultTotalCount = "<%=PasswordFaultTotalCount %>";
        var R_CheckAuthDept = "<%=GetResource("CheckAuthDept") %>";
        var R_InvalidUserIDPassword = "<%=GetResource("InvalidUserIDPassword")%>";
        var R_CheckIDandPwd = "<%=GetResource("CheckIDandPwd")%>";
        var R_InputLogin = "<%=GetResource("InputLogin")%>";
        var R_InputPwd = "<%=GetResource("InputPwd")%>";
        var R_PasswordOver = "<%=GetResource("PasswordOver")%>" 
        var MaxCnt = "<%=MaxCnt %>";                 
    </script>
    <script type="text/javascript" src="../../Js/Site/Auth/Default.js"></script>
    <script type="text/javascript">
        //JsCall("../../Js/Site/Auth/Default.js");
    </script>
    <script language="javascript">
	     <%
               foreach (System.Collections.Generic.KeyValuePair<string, string> content in PopupContent)
                {
                    if (Request.Cookies[content.Key] == null)
                    {
                        Response.Write(content.Value);
                    }
                }
         %>
	</script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="warp">
    <!-- header -->
	<div id="header">
		<div id="top_img" class="show"></div>
	</div>
    <!-- //header -->
    <hr />
    <!-- content -->
	<div id="main-content">
	<div id="container">
		<div id="mid_text" class="show"></div>
		<div id="mid_img" class="show"></div>
		<div id="login_warp">
			<div id="fom_title"><span>서비스 이용을 위해서 로그인을 해 주시기 바랍니다.</span></div>
			<div id="fom_warp">
				<div id="fom_1">
					<div><span id="move_id"><%=GetResource("ID")%></span><asp:TextBox ID="tbLoginID" TabIndex="1" CssClass="fom" runat="server"></asp:TextBox></div>
					<div id="move_pw"><span id="move_pw"><%=GetResource("Password")%></span><asp:TextBox ID="tbPassword" TabIndex="2" CssClass="fom" runat="server" TextMode="Password"></asp:TextBox></div>
					<div id="move-com"><span id="move_com"><%=GetResource("Company1")%></span>
                        <asp:DropDownList ID="ddlCompany" runat="server">
                        </asp:DropDownList>
					</div>
                    <a onclick="" id="Submit1" href="#" class="login_bt" style="margin-top:5px;"></a>
				</div>
				<div id="fom_line"></div>
				<div id="fom_2">
					<asp:CheckBox ID="chkCompany" TabIndex="6" runat="server" Visible="true"></asp:CheckBox><span id="com_font"><%=GetResource("SaveCompany")%></span>
					<asp:CheckBox ID="chkRemember" TabIndex="3" runat="server" Visible="true"></asp:CheckBox><span id="id_font"><%=GetResource("SaveID")%></span>
				</div>
			</div>
		</div>
	</div>
	</div>
    <hr />
    <!-- copyright -->
	<div id="footer">
		<div id="foot_line"></div>
		<div id="foot_text" class="show">			
			<div>
				<p style="font-family: '맑은 고딕';">서울시 구로구 디지털로33길 28 우림e-biz센터 1차 507호 제오스페이스 TEL : 02-2108-3070  FAX : 02-2108-3080</p>
				<p style="font-family: '맑은 고딕';">대표번호 : 02-2108-3070</p>
			</div>
		</div>
	</div>
    <!-- //copyright -->
    <div style="display:none">
    <asp:LinkButton ID="lbtnSubmit" OnClick="lbtnSubmit_Click" runat="server"></asp:LinkButton>
    </div>
    </div>
    </form>
    <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->        
        <div id="zeoModalDrag" class="modal-drag"><p></p></div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post"></form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank"></iframe>
    </div>
    <div id="zeoModalBG"></div>
    <!-- ############# end of modal ############## -->
    <form  name="formPwd" method="post">
        <input type="hidden" name="companyID" id="companyID" />
        <input type="hidden" name="loginID" id="loginID" />
    </form>
   
</body>
</html>
