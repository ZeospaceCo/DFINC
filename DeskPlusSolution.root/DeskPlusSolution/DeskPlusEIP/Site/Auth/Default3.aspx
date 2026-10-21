<%@ Page Language="C#" CodeBehind="Default3.aspx.cs" AutoEventWireup="true" Inherits="Zeospace.DeskPlus.Base.Web.Site.Auth.Default3" %>

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
body { margin:0px; padding:0px; }
input { vertical-align:middle; }
p { -webkit-margin-before: 10px !important; -webkit-margin-after: 10px !important; font-size: 12px; }

body { background:url('/DeskPlusEIP/Design/C/Img/LoginNew/3/3_01back.png') repeat-x; }
#warp { width: 1170px; position: relative; margin: 0 auto; }
#header { width: 1170px; height: 100px; position: relative; margin: 0 auto; }
#main-content { width: 1170px; height: 650px; position: relative; margin: 0 auto; }
#footer { width: 1170px; height: 120px; position: relative; margin: 0 auto; }

#login_warp {
	border: 1px solid #e1e1e1; background-color:#f9f9f9;
	width:292px; height: 173px; position:absolute; font-size:12px; 
	left:14%; top:45%; font-family: '맑은 고딕', serif;
}
#fom_warp { position:relative; width: 270px; margin: 0 auto; text-align: right; }
#fom_1 { width:250px; padding-top:10px; }
#fom_1 div {margin-bottom:8px; font-family: '맑은 고딕';}
.save { text-align: center; margin-top: 5px;}
#fom_2 { width:250px; margin: 0 auto; text-align: left; padding: 7px 0 0 20px; margin-left: 100px; margin-top: 15px; }

.fom {
	width:180px; height:20px; background:url(/DeskPlusEIP/Design/C/Img/LoginNew/3/fom.jpg) no-repeat cover; border:none;
	font: 12px '맑은 고딕'; color:#363636; padding:3px 5px 0px 5px; border: 1px solid #e1e1e1;
}
#fom_line { width:262px; height:1px; border-top: 1px solid #e1e1e1;}
#ComSel { width:193px; color:#393939; border: 1px solid #e1e1e1;}
#ddlCompany { width:193px; color:#393939; border: 1px solid #e1e1e1;font-size:11px; height:22px;}
#FindIDPW { text-decoration: none; color: #393939;}
.login_bt{
	width:59px; height:25px; position:absolute; margin-left:60px; bottom:-4.5%;
	background:url(/DeskPlusEIP/Design/C/Img/LoginNew/3/3_02bt.png) no-repeat center center; cursor: pointer; border-radius: 3px;
}

.show { position: absolute; }
#top_text { width:380px; height:60px; background: url(<%=LoginImage%>) no-repeat; left:5%; top: 40%; border:0px solid red;}
#mid_top_text { width:436px; height:132px; background:url(/DeskPlusEIP/Design/C/Img/LoginNew/3/3_07text.png) no-repeat; left: 11.5%; top: 2%; }
#mid_login_text { width:140px; height:15px; background:url(/DeskPlusEIP/Design/C/Img/LoginNew/3/3_04login_tit.png) no-repeat; left: 14%; top: 42%; }
#mid_right_img { width:518px; height:492px; background:url(/DeskPlusEIP/Design/C/Img/LoginNew/3/3_06object.png) no-repeat; left: 47%; top: 5%; }
#foot_text { width:820px; font-size: 12px; left: 15%; color: #fff; margin-top: 5px; font-family: '맑은 고딕', serif; }
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
		<div id="top_text" class="show"></div>
	</div>
    <!-- //header -->
    <hr />
    <!-- content -->
	<div id="main-content">
		<div id="mid_top_text" class="show"></div>		
		<div id="mid_login_text" class="show"></div>
		<div id="login_warp">
			<div id="fom_warp">
			<div id="fom_1">
				<div><%=GetResource("ID")%> <asp:TextBox ID="tbLoginID" TabIndex="1" CssClass="fom" runat="server"></asp:TextBox></div>
				<div><%=GetResource("Password")%> <asp:TextBox ID="tbPassword" TabIndex="2" CssClass="fom" runat="server" TextMode="Password"></asp:TextBox></div>
				<div><%=GetResource("Company1")%>
                    <asp:DropDownList ID="ddlCompany" runat="server">
                    </asp:DropDownList>
				</div>
				<div class="save">
					<asp:CheckBox ID="chkCompany" TabIndex="6" runat="server" Visible="true"></asp:CheckBox><span id="com_font"><%=GetResource("SaveCompany")%></span>
					<asp:CheckBox ID="chkRemember" TabIndex="3" runat="server" Visible="true"></asp:CheckBox><span id="id_font"><%=GetResource("SaveID")%></span>
				</div>
			</div>
			<div id="fom_line"></div>
			<div id="fom_2">
				<a onclick="" id="Submit1" href="#" class="login_bt"></a>
			</div>
			</div>
		</div>
		<div id="mid_right_img" class="show footer_img"></div>
	</div>
    <hr />
    <!-- copyright -->
	<div id="footer">
		<div id="foot_text" class="show">
			<div>
				<p style="font-family: '맑은 고딕';">서울시 구로구 디지털로33길 28 우림e-biz센터 1차 507호 제오스페이스 TEL:02-2108-3070  FAX:02-2108-3080</p>
				<p style="font-family: '맑은 고딕';">대표번호:02-2108-3070</p>
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
