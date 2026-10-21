<%@ Page Language="C#" CodeBehind="Default7.aspx.cs" AutoEventWireup="true" Inherits="Zeospace.DeskPlus.Base.Web.Site.Auth.Default7" %>

<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
	<meta name="robots" content="noindex">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Login.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <style type="text/css">
        .placeholder { color: #aaa; }
@import url('http://fonts.googleapis.com/earlyaccess/nanumgothic.css');
@media all and (min-height:1170px){
	body{ overflow-x:hidden; }
}
body { margin:0px; padding:0px; }
input { vertical-align:middle; }
p { -webkit-margin-before: 10px !important; -webkit-margin-after: 10px !important; font-size: 12px; }
::-webkit-input-placeholder { color: #fff; } /*webkit-browsers*/
:-moz-placeholder { color: #fff; } /*firefox 4 to 8*/
::-moz-placeholder { color: #fff; } /*firefox 19+*/
::-ms-input-placeholder { color: #fff; } /*internet explorer 10+*/

body { background:url('/DeskPlusEIP/Design/C/Img/LoginNew/7/8_img_18.png') repeat-x; background-color: #9ea1b1; }
#warp { width: 1170px; position: relative; margin: 0 auto; }
#header { width: 1170px; height: 99px; position: relative; margin: 0 auto; }
#main-content { width: 1170px; height: 680px; position: relative; margin: 0 auto; }
#footer { width: 1170px; height: 150px; position: relative; margin: 0 auto; }
	
#tit_login span { font-size: 15px; font-weight:bold; color: #fff; }
#login_warp {
	width:350px; position:absolute; color:#a6a6a6; font-size:12px; left: 50%; top: 50%;
	margin-left: -180px; margin-top: 60px; font-family: '맑은 고딕', serif;
}

#fom_1 { padding-top:11px;}
#fom_1 div {margin-bottom:8px;}
#fom_2 select, label, a { font-size:11px; }

.fomid {
	width:240px; 
	height:25px; 
	font-size:12px; 
	color:#fff; 
	font-weight:bold;
	padding-top:3px;
	padding-left:30px; 
	background:#9ea1b1;
	background-image:url(/DeskPlusEIP/Design/C/Img/LoginNew/7/8_img_06.png);
	border: 1px solid #fff; 
	border-radius: 3px; 
}

.fompw {
	width:240px; 
	height:25px; 
	font-size:12px; 
	color:#fff; 
	font-weight:bold;
	padding-top:3px;
	padding-left:30px; 
	background:#9ea1b1;
	background-image:url(/DeskPlusEIP/Design/C/Img/LoginNew/7/8_img_07.png);
	border: 1px solid #fff; 
	border-radius: 3px; 
}

.login_bt{
	width:71px; height:71px; position:absolute; right:0px; top:28px; background:url(/DeskPlusEIP/Design/C/Img/LoginNew/7/8_img_16.png) no-repeat center center;
	cursor: pointer; border-radius: 3px;

}
.sel{ position:absolute; color:#363636; border:1px solid #fff;}

#ddlCompany { background: #9ea1b1; color: #fff; overflow-y: scroll; width: 120px; border: 1px solid #8f92a0; height:22px;font-weight:bold;}
#ComSave { position:absolute; left:80px; width:15px; }
#com_font{ position:absolute; top: 110px; color: #fff; left:140px; font-size:12px; }
#IDSave { position:absolute; left:150px; width:15px; }
#id_font{ position:absolute; top: 110px; color: #fff; left:210px; font-size:12px; }
#FindIDPW { position:absolute; text-decoration: none; color: #fff; left:250px; top:110px; font-size:12px; }

.show { position: absolute; }
#top_logo { width:380px; height:60px; background: url(<%=LoginImage%>) no-repeat; left: 65%; top: 20%; }
#mid_left_img { width:470px; height:266px; background: url(/DeskPlusEIP/Design/C/Img/LoginNew/7/8_img_11.png) no-repeat; left: 3%; }
#mid_text { width:367px; height:104px; background: url(/DeskPlusEIP/Design/C/Img/LoginNew/7/8_img_15.png) no-repeat; left: 37%; top: 10%; }
#mid_right_img { width:534px; height:266px; background: url(/DeskPlusEIP/Design/C/Img/LoginNew/7/8_img_17.png) no-repeat; left: 60%; }
#under_line { width:90%; height: 1px; border-bottom:1px solid #fff; left: 5%;}
#foot_img { width:192px; height: 14px; background: url(/DeskPlusEIP/Design/C/Img/LoginNew/7/8_img_09.png) no-repeat; left: 5.5%;}
#foot_text { width:820px; font-size: 12px; left: 26%; color: #fff; margin-top: 20px; font-family: '맑은 고딕', serif;}
#footer > #foot_img { margin-top: 30px; }
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
		<div id="top_logo" class="show"></div>
	</div>
    <!-- //header -->
    <hr />
    <!-- content -->
	<div id="main-content">
		<div id="mobile_img">
			<div id="mid_text" class="show"></div>
			<div id="mid_left_img" class="show"></div>
			<div id="mid_right_img" class="show"></div>
		</div>
		<div id="login_warp">
			<div id="tit_login"><span>Hantheme Members Login</span></div>
			<div id="fom_1">
				<div><asp:TextBox ID="tbLoginID" TabIndex="1" CssClass="fomid" runat="server"></asp:TextBox></div>
				<div><asp:TextBox ID="tbPassword" TabIndex="2" CssClass="fompw" runat="server" TextMode="Password"></asp:TextBox></div>
			</div>
			<div id="fom_2">
				<span class="sel">
                    <asp:DropDownList ID="ddlCompany" runat="server"></asp:DropDownList>
				</span>
				<label id="com_font"><asp:CheckBox ID="chkCompany" TabIndex="6" runat="server" Visible="true"></asp:CheckBox>&nbsp;<%=GetResource("SaveCompany")%></label>
				<label id="id_font"><asp:CheckBox ID="chkRemember" TabIndex="3" runat="server" Visible="true"></asp:CheckBox>&nbsp;<%=GetResource("SaveID")%></label>
			</div>
			<a onclick="" id="Submit1" href="#" class="login_bt show"></a>
		</div>
	</div>
    <hr />
    <!-- copyright -->
	<div id="footer">
		<div id="under_line" class="show"></div>
		<div id="foot_img" class="show"></div>
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
