<%@ Page Language="C#" CodeBehind="Default9.aspx.cs" AutoEventWireup="true" Inherits="Zeospace.DeskPlus.Base.Web.Site.Auth.Default9" %>

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
body { margin:0px; padding:0px; background:#fff;}
input { vertical-align:middle; }

#warp {
	width: 100%; height:100%; position: relative; margin: 0 auto;
	background:url('/DeskPlusEIP/Design/C/Img/LoginNew/9/12_01back.png') no-repeat center center;
	min-width:1170px;
	-o-background-size:cover;      /* opera */
	-ms-background-size:cover;     /* explorer */
	-moz-background-size:cover;    /* firefox */
	-webkit-background-size:cover; /* safari and chrome */
	background-size:100%;         /* css3 */
}
#header { width: 1170px; height: 60px; position: relative; margin: 0 auto; }
#main-content { width: 1170px; height: 700px; position: relative; margin: 0 auto; }
#footer { width:1170px; height:100px; position: relative; margin: 0 auto;}
#warp_footer { width:100%; background-color: rgba( 0, 123, 125, 0.8 ); }
	
#tit_login { font-size: 15px; font-weight:bold; color: #272727; }
#login_warp {
	width:360px; position:absolute; color:#a6a6a6; font-size:12px; left: 60%; top: 20%; font-family: '맑은 고딕', serif;
}

#fom_1 {width:260px; height:80px; padding-top:11px;}
#fom_1 div {margin-bottom:8px;}
#fom_2 select, a { font-size:12px; }

.list_teb {width: 360px; position:relative; margin-top:8px; top:30px;}

.fomid 
{
	width:245px; height:30px; 
	background-image:url(/DeskPlusEIP/Design/C/Img/LoginNew/9/12_img_08.png); 
	font:14px/28px '맑은 고딕', serif; 
	color:#a2a8ba; 
	padding-left:30px; 
	border: 1px solid #bacbcb; 
	border-radius: 3px;
	font-weight:bold;
}

.fompw {
    width:245px; height:30px; 
	background-image:url(/DeskPlusEIP/Design/C/Img/LoginNew/9/12_img_07.png); 
	font:14px/28px '맑은 고딕', serif; 
	color:#a2a8ba; 
	padding-left:30px; 
	border: 1px solid #bacbcb; 
	border-radius: 3px;
	font-weight:bold;
}


::-webkit-input-placeholder { color: #6a6a6a; } /*webkit-browsers*/
:-moz-placeholder { color: #6a6a6a; }			/*firefox 4 to 8*/
::-moz-placeholder { color: #6a6a6a; }			/*firefox 19+*/
::-ms-input-placeholder { color: #6a6a6a; }		/*internet explorer 10+*/

.login_bt{
	width:71px; height:71px; position:absolute; right:0px; top:28px; background:url(/DeskPlusEIP/Design/C/Img/LoginNew/9/12_05login_bt.png) no-repeat center center;
	cursor: pointer; border-radius: 3px;
}
.sel{ position:absolute; }

#ddlCompany { background: rgba(256,256,256,0.8); color: #6a6a6a; overflow-y: scroll; width: 120px; border: 1px solid #d6d7d9; }
#ComSave { position:absolute; left:80px; }
#la_Com { cursor:pointer; position:absolute; left:82px; top:109px; width:14px; height:14px; background:rgba(256,256,256,1); border:1px solid #d6d7d9; }
#com_font{ position:absolute; top: 3px; color: #6a6a6a; left:142px; top:109px; }
#IDSave { position:absolute; left:150px; }
#la_ID { cursor:pointer; position:absolute; left:152px; top:109px; width:14px; height:14px; background:rgba(256,256,256,1); border:1px solid #d6d7d9; }
#id_font{ position:absolute; top: 3px; color: #6a6a6a; left:212px; top:109px;}
#FindIDPW { position:absolute; left:243px; top:109px; text-decoration: none; color: #285255; }

.show { position: absolute; }
#top_logo { width:380px; height:60px; background: url(<%=LoginImage%>) no-repeat; left: 70%; top: 10%; }
#mid_img { width:556px; height:247px; background: url(/DeskPlusEIP/Design/C/Img/LoginNew/9/12_09object.png) no-repeat; left: 10%; top: -5%; }
#foot_text { font-size: 12px; left: 0%; color: #fff; margin-top: 20px; font-family: '맑은 고딕', serif;border:0px solid red; width:100%;text-align:right;}

/*check box + label control*/
#fom_2 label:after {
	-ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=0)";
	filter: alpha(opacity=0);
	opacity: 0;
	content: '';
	position: absolute;
	width: 8px;
	height: 4px;
	background: transparent;
	top: 2px;
	left: 2px;
	border: 3px solid #333;
	border-top: none;
	border-right: none;

	-webkit-transform: rotate(-45deg);
	-moz-transform: rotate(-45deg);
	-o-transform: rotate(-45deg);
	-ms-transform: rotate(-45deg);
	transform: rotate(-45deg);
}
#fom_2 label:hover::after {
	-ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=30)";
	filter: alpha(opacity=30);
	opacity: 0.3;
}
#fom_2 input[type="checkbox"]:checked + label:after {
	-ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=100)";
	filter: alpha(opacity=100);
	opacity: 1;
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
	<div id="header">
		<div id="top_logo" class="show"></div>
	</div>
    <!-- //header -->
    <!-- content -->
	<div id="main-content">
		<div id="mid_img" class="show"></div>
		<div id="login_warp">
			<div id="tit_login"><span>Members Login</span></div>
			<div id="fom_1">
				<div><asp:TextBox ID="tbLoginID" TabIndex="1" CssClass="fomid" runat="server"></asp:TextBox></div>
				<div><asp:TextBox ID="tbPassword" TabIndex="2" CssClass="fompw" runat="server" TextMode="Password"></asp:TextBox></div>
			</div>
			<div id="fom_2">
				<span class="sel">
                    <asp:DropDownList ID="ddlCompany" runat="server">
                    </asp:DropDownList>
				</span>
				<span id="com_font">
                <asp:CheckBox ID="chkCompany" TabIndex="6" runat="server" Visible="true"></asp:CheckBox>
				<%=GetResource("SaveCompany")%></span>
				<span id="id_font">
                <asp:CheckBox ID="chkRemember" TabIndex="3" runat="server" Visible="true"></asp:CheckBox>
				<%=GetResource("SaveID")%></span>
				<a onclick="" id="Submit1" href="#" class="login_bt"></a>
			</div>
		</div>
		<div id="mid_right_img" class="show m_bottom_img"></div>
	</div>
    <hr />
    <!-- copyright -->
	<div id="warp_footer">
	<div id="footer">
		<div id="foot_text" class="show">
			<div style="padding-top:10px;color:#000;">
                <span>서울시 구로구 디지털로33길 28 우림e-biz센터 1차 507호 제오스페이스</span>
                <span style="padding-left:10px;">TEL : 02-2108-3070</span>
                <span style="padding-left:10px;">FAX : 02-2108-3080</span>
                <span style="padding-left:10px;">대표번호 : 02-2108-3070</span>
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
