<%@ Page Language="C#" CodeBehind="Default10.aspx.cs" AutoEventWireup="true" Inherits="Zeospace.DeskPlus.Base.Web.Site.Auth.Default10" %>

<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
	<meta name="robots" content="noindex">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Login.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <style type="text/css">
        .placeholder { color: #aaa; }
@import url(http://fonts.googleapis.com/earlyaccess/nanumgothic.css);
body { margin:0px; padding:0px; background:#fff;font-family: "맑은 고딕"}
input { vertical-align:middle; }
p { -webkit-margin-before: 10px !important; -webkit-margin-after: 10px !important; font-size: 12px; }

#warp {
	width:100%; position:relative; margin:0 auto;
}
#header_content_warp {
	background:url('/DeskPlusEIP/Design/C/Img/LoginNew/10/13_09back.png') no-repeat center center;
	min-width:1170px;
	-webkit-background-size:cover; /* For WebKit*/
    -moz-background-size:cover;    /* Mozilla*/
    -o-background-size:cover;      /* Opera*/
    background-size:cover;         /* Generic*/
}
#header {
	width:100%; background:url('/DeskPlusEIP/Design/C/Img/LoginNew/10/13_11top_or.png') repeat-x;
}
#header_warp { width: 1170px; height: 50px; position: relative; margin: 0 auto; padding:10px 0; }
#main-content { width: 1170px; height: 490px; position: relative; margin: 0 auto; }
#footer { width:100%; height:210px; background:url('/DeskPlusEIP/Design/C/Img/LoginNew/10/13_12back_p_bt.png') repeat-x; }
#footer_warp { width:1170px; height:210px; position:relative; margin:0 auto; }
	
.login_m { display:none; }
#login_warp {
	width:340px;position:absolute; font-size:12px; 
	left:5%; top:9%; font-family: '맑은 고딕', serif;
}
#fom_warp { position:relative; width: 310px; margin: 0 auto; text-align: left; margin-top: 20px;}
#fom_title { padding: 8px 0 0 10px; font-family: '맑은 고딕', serif; color: #fff; font-size: 12px;}
#fom_1 { width:310px; padding: 0px; }
#fom_1 div {margin-bottom:8px; font-family: '맑은 고딕';}
#fom_1 > div > #move_id { margin-right: 22px; color: #000; font-weight:bolder;font-family: "맑은 고딕" }
#fom_1 > div > #move_pw { margin-right: 10px; color: #000; font-weight:bolder;font-family: "맑은 고딕" }
#fom_1 > div > #move_com { margin-right: 30px; color: #000; font-weight:bolder;font-family: "맑은 고딕" }

#fom_2 { padding: 0px; text-align: left; margin-left: 55px;font-family: "맑은 고딕";border:0px solid red; font-size:11px;}

.fom {
	width:160px; height:20px; background:url(/DeskPlusEIP/Design/C/Img/LoginNew/10/fom.jpg) no-repeat cover; border:none;
	font: 12px '맑은 고딕'; color:#363636; padding:3px 5px 0px 5px; border: 1px solid #b1b1b1;
}
#ComSel { width:172px; height:20px;}
#ddlCompany { width:172px; height:20px;}
#FindIDPW { text-decoration: none; color: #393939;}
.login_bt{
	width:85px; height:85px; position:absolute; top: 0%; left: 78%; background:url(/DeskPlusEIP/Design/C/Img/LoginNew/10/13_02login_bt.png) no-repeat center center;
	background-size:cover; cursor: pointer; border-radius: 3px;
}

.show { position: absolute; }
#top_logo {
	width:380px; height:60px; background:url(<%=LoginImage%>) no-repeat; position:absolute;
	top:40px; right:0; bottom:0; left:10px; margin:auto;border:0px solid red;
}
#mid_text { width:353px; height:60px; background:url(/DeskPlusEIP/Design/C/Img/LoginNew/10/13_07text.png) no-repeat; left:62%; top:23%; }
#foot_line { width:7px; height:100px; background:url(/DeskPlusEIP/Design/C/Img/LoginNew/10/13_02line.png) repeat-y; left:35%; top:20%; border:0px solid red;}
#foot_text { width:630px; font-size: 12px; left: 40%; top: 26%; color: #787878; margin-top: 10px; font-family: '맑은 고딕', serif; border:0px solid red;}
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
	<div id="header_content_warp">
	<!-- Header -->
	<div id="header">
		<div id="header_warp">
			<div id="top_logo"></div>
		</div>
	</div>
    <!-- //header -->
    <hr />
    <!-- content -->
	<div id="main-content" style="border:0px solid red;">
		<div id="mid_text" class="show"></div>
	</div>
	</div>
    <hr />
    <!-- copyright -->
	<div id="footer">
		<div id="footer_warp" >
		<div id="login_warp" class="login_pc" style="border:0px solid red;">
			<div id="fom_warp">
				<div id="fom_1">
					<div><span id="move_id"><%=GetResource("ID")%></span><asp:TextBox ID="tbLoginID" TabIndex="1" CssClass="fom" runat="server"></asp:TextBox></div>
					<div><span id="move_pw"><%=GetResource("Password")%></span><asp:TextBox ID="tbPassword" TabIndex="2" CssClass="fom" runat="server" TextMode="Password"></asp:TextBox></div>
					<div><span id="move_com"><%=GetResource("Company1")%></span><asp:DropDownList ID="ddlCompany" runat="server"></asp:DropDownList></div>
					<a onclick="" id="Submit1" href="#" class="login_bt"></a>
				</div>
				<div id="fom_2">
					<asp:CheckBox ID="chkCompany" TabIndex="6" runat="server" Visible="true"></asp:CheckBox>&nbsp;<span id="com_font" style="font-family: '맑은 고딕';"><%=GetResource("SaveCompany")%></span>
                    &nbsp;&nbsp;
					<asp:CheckBox ID="chkRemember" TabIndex="3" runat="server" Visible="true"></asp:CheckBox>&nbsp;<span id="id_font" style="font-family: '맑은 고딕';"><%=GetResource("SaveID")%></span>
				</div>
			</div>
		</div>
		<div id="foot_line" class="show"></div>
		<div id="foot_text" class="show">
			<div>
				<p style="font-family: '맑은 고딕';">서울시 구로구 디지털로33길 28 우림e-biz센터 1차 507호 제오스페이스 TEL:02-2108-3070  FAX:02-2108-3080</p>
				<p style="font-family: '맑은 고딕';">대표번호:02-2108-3070</p>
			</div>
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
