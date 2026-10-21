<%@ Page Language="C#" CodeBehind="Default8.aspx.cs" AutoEventWireup="true" Inherits="Zeospace.DeskPlus.Base.Web.Site.Auth.Default8" %>

<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
	<meta name="robots" content="noindex">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>

    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <style type="text/css">
        .placeholder { color: #aaa; }
@font-face {font-family: "맑은 고딕", "돋움",Dotum,"굴림",Gulim,AppleGothic,Sans-serif;src:url(/DeskPlusEIP/Design/C/Css/NanumGothic.eot);}
input { vertical-align:middle; }
p { -webkit-margin-before: 10px !important; -webkit-margin-after: 10px !important; font-size: 12px; }

body { background-color: #071532; width:100%; height:100%; margin:0px; padding:0px; }
#warp {
	width:100%; position: relative; margin: 0 auto; padding:0;
	background:url('/DeskPlusEIP/Design/C/Img/LoginNew/8/11_01back.png') center center no-repeat;
	min-width:1170px;
	background-size:100% 100%;
}
#header { width: 1170px; height: 100px; position: relative; margin: 0 auto; }
#main-content { width: 1170px; height: 530px; position: relative; margin: 0 auto; }
#footer { width: 1170px; height: 250px; position: relative; margin: 0 auto; }

#login_warp {
	width:340px; height: 186px; position:absolute; font-size:12px;
	left:8%; top:9%; font-family: '맑은 고딕', serif;
}

#fom_warp { position:relative; width: 310px; margin: 0 auto; text-align: left; margin-top: 20px;}
#fom_title { padding: 8px 0 0 10px; font-family: '맑은 고딕', serif; color: #fff; font-size: 12px;}
#fom_1 { width:310px; padding: 5px 0 10px 0; }
#fom_1 div { margin-bottom:8px; font-family: '맑은 고딕'; }
#fom_1 > div > #move_id { margin-right: 8%; color: #fff; font-weight:bold; }
#fom_1 > div > #move_pw { margin-right: 4.4%; color: #fff; font-weight:bold; }
#fom_1 > div > #move_com { margin-right: 10.6%; color: #fff; font-weight:bold; }

#fom_2 { width:280px; padding: 0px; text-align: left; color:#fff;}

.fom {
	width:160px; height:15px; background:url(/DeskPlusEIP/Design/C/Img/LoginNew/8/fom.jpg) no-repeat cover; border:none;
	font: 12px '맑은 고딕'; color:#363636; padding-left:10px; border: 1px solid #b1b1b1;
}
#ComSel { width:173px; height:20px;}
#ddlCompany { width:173px; height:20px;}
#FindIDPW { text-decoration: none; color: #fff;}
.login_bt{
	width:56px; height:74px; position:absolute; top: 4%; left: 78%;
	background:url(/DeskPlusEIP/Design/C/Img/LoginNew/8/11_05login_bt2.png) no-repeat center center; cursor: pointer; border-radius: 3px;
}

.show { position: absolute; }
#top_logo { width:380px; height:60px; background: url(<%=LoginImage%>) no-repeat; left:10%; top: 80%;border:0px solid red;}
#mid_text_img { width:423px; height:273px; background:url(/DeskPlusEIP/Design/C/Img/LoginNew/8/11_09text_img.png) no-repeat; left:10%; top:30%; }
#mid_right_img { width:518px; height:489px; background:url(/DeskPlusEIP/Design/C/Img/LoginNew/8/11_08object.png) no-repeat; left:47%; top:-10%; }
#foot_line { width:100%; border-bottom: 2px dotted #fff; position: relative; margin: 0 auto;}
#foot_text {
	width:650px; font-size:12px; left: 40%; top: 15%; color: #fff; margin-top: 10px; font-family: '맑은 고딕', serif;
}
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
	<div id="header" style="border:0px solid red;">
		<div id="top_logo" class="show"></div>
	</div>
    <!-- //header -->
    <!-- content -->
	<div id="main-content" style="border:0px solid red;">
		<div id="mid_text_img" class="show"></div>
		<div id="mid_right_img" class="show"></div>
	</div>
    <!-- copyright -->
	<div id="footer">
		<div id="foot_line"></div>
		<div id="login_warp" class="login_pc">
			<div id="fom_warp">
				<div id="fom_1">
					<div><span id="move_id"><%=GetResource("ID")%></span><asp:TextBox ID="tbLoginID" TabIndex="1" CssClass="fom" runat="server"></asp:TextBox></div>
					<div><span id="move_pw"><%=GetResource("Password")%></span><asp:TextBox ID="tbPassword" TabIndex="2" CssClass="fom" runat="server" TextMode="Password"></asp:TextBox></div>
					<div id="move_com"><span id="move_com"><%=GetResource("Company1")%></span>
                        <asp:DropDownList ID="ddlCompany" runat="server">
                        </asp:DropDownList>
					</div>
					<a onclick="" id="Submit1" href="#" class="login_bt"></a>
				</div>
				<div id="fom_2">
					<asp:CheckBox ID="chkCompany" TabIndex="6" runat="server" Visible="true"></asp:CheckBox><span id="com_font"><%=GetResource("SaveCompany")%></span>
					<asp:CheckBox ID="chkRemember" TabIndex="3" runat="server" Visible="true"></asp:CheckBox><span id="id_font"><%=GetResource("SaveID")%></span>
				</div>
			</div>
		</div>
		<div id="foot_text" class="show">
			<div>
				<p>서울시 구로구 디지털로33길 28 우림e-biz센터 1차 507호 제오스페이스 TEL:02-2108-3070  FAX:02-2108-3080</p>
				<p>대표번호:02-2108-3070</p>
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
