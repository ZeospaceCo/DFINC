<%@ Page Language="C#" CodeBehind="Default.aspx.cs" AutoEventWireup="true" Inherits="Zeospace.DeskPlus.Base.Web.Site.Auth.Default" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <meta name="csrf-token" content="<%=csrf_token%>">
	<meta name="robots" content="noindex">
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=yes" />
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DStyle/bootstrap.min.css"/>



    <script>
        document.write('<link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Design/C/Css/DStyle/style.css?ver=' + Math.random() + '">');
    </script>

   
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />

    <style type="text/css">
     html{ height:100%;}  

	.login13{
	    background: url('<%=BGLoginImage%>') no-repeat center center fixed;       
		-webkit-background-size: cover;-moz-background-size: cover;-o-background-size: cover;background-size: cover;
	    display: -ms-flexbox;  display: flex;  -ms-flex-align: center;  align-items: center;  padding-top: 40px; padding-bottom: 40px;  background-color: #fff !important;
    }


    </style> 

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery-1.12.4.min.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.placeholder.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>

    <script type="text/javascript" src="./v5.login.js"></script>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/Design/bootstrap.min.js"></script>

    <script type="text/javascript">
        var R_PasswordFaultCount = "<%=GetResource("PasswordFault") %>"; // 비밀번호 틀렸습니다.        
        var R_PasswordFaultTotalCount = "<%=PasswordFaultTotalCount %>";
        var R_CheckAuthDept = "<%=GetResource("CheckAuthDept") %>";
        var R_InvalidUserIDPassword = "<%=GetResource("InvalidUserIDPassword")%>";
        var R_CheckIDandPwd = "<%=GetResource("CheckIDandPwd")%>";
        var R_InputLogin = "<%=GetResource("InputLogin")%>";
        var R_InputPwd = "<%=GetResource("InputPwd")%>";
        var R_NoAuthEmail = "<%=GetResource("NoAuthEmail")%>"   
        var R_PasswordOver = "<%=GetResource("PasswordOver")%>"   
        var MaxCnt = "<%=MaxCnt %>";       
        var OTPYN = "";
    </script>   
    <script type="text/javascript">
        JsCall("../../Js/Site/Auth/Default.js");
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
<body class="login login-13">  

    <div id="body-bg" style="background: url('<%=BaseImgPath%>/Login/bg (16).jpg'); opacity:.5 "></div>
	<!-- '해당 경로 폴더 참조 -->
    <!-- content -->
    <div class="position-fixed smartplusv">
    	<font class="text-primary">smartsPlus_</font> <font class="text-white"> zeospace</font>
    </div>
    <div class="position-fixed leftback bg-primary"></div>
    
    <div class="login-container">        	
		<div style="flex:1 1 auto"></div>
        <div class="row container-fluid mx-auto px-0 bg-white" style=" flex:3 1 auto;" >
                <div class="col col-12 col-md-auto login-logo-area position-relative bg-primary p-5" style="">
                  <div class="logo text-center"><img id="loginImg" src="<%=LoginImage%>"></div>
                  <div class="notice">
                  		<ul>
						
                        <li><!--a href="#modalnotice" data-toggle="modal" data-target="#modalnotice"--><a href="#">그룹웨어 이용을 위하여 로그인이 필요합니다.<!-- span class="badge number badge-light text-white">6</span--></a><!-- span>01.05</span--></li>
                        <li><a href="#">비밀번호 5회 오류 시 접속이 차단됩니다.<!--span class="badge number badge-light text-white">!</span></a><!--span>01.05</spani--></li>
                        <li><a href="#">본 시스템에는 대외비급 이상의 중요정보가 저장되어<br>있으므로 회사 정보보호규정을 준수하여야 합니다.</a><!--span>02.15</span--></li>	
						<!--<li><a href="#">로그인 관련문의는 전산팀으로 연락주세요.</a></li>-->
                        </ul>
                  </div>
                </div>
                <div class="col col-12 col-md-auto login-login-box p-4" style=""  >
                	<div class="title text-center text-primary">로그인</div>
                    <form id="form1" runat="server" class="form-signin">
                        <asp:TextBox ID="tbLoginID" TabIndex="1" class="form-control" runat="server" placeholder="아이디를 입력해 주세요."></asp:TextBox>
                        <asp:TextBox ID="tbPassword" TabIndex="2" class="form-control" runat="server" TextMode="Password"></asp:TextBox>  
                                                
                        <div class="input-group mb-3">
                          <div class="input-group-prepend">
                            <label class="input-group-text" for="inputGroupSelect01"><%=GetResource("Company")%></label>
                          </div>
                          <asp:DropDownList ID="ddlCompany" runat="server" class="custom-select"></asp:DropDownList>
                        </div>
                        
                        <div class="checkbox mb-3">
                          <label><asp:CheckBox ID="chkRemember" TabIndex="3" runat="server" Visible="true"></asp:CheckBox><%=GetResource("SaveID")%> </label>
                          <label><asp:CheckBox ID="chkCompany" TabIndex="6" runat="server" Visible="true"></asp:CheckBox><%=GetResource("SaveCompany")%> </label>
                        </div>                        
                        <input type="submit" value="<%=GetResource("Login")%>" id="Submit1" class="btn btn-lg btn-primary btn-block"  />
                        <input type="button" value="OTP등록" id="Otp" class="btn btn-lg btn-primary btn-block"  /> 
                        
                        <div style="display:none">
                          <asp:LinkButton ID="lbtnSubmit" OnClick="lbtnSubmit_Click" runat="server"></asp:LinkButton>
                        </div>   
                        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                        <asp:HiddenField ID="forgeryToken" runat="server"/>
                    </form>
                	<!--div class="etc">이용문의 : 02-2108-3070 </div-->
                </div>
        </div>
		<div style="flex:1 1 auto"></div>
    </div>

    <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->        
        <div id="zeoModalDrag" class="modal-drag"><p></p></div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post"></form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank"></iframe>
    </div>
    <div id="zeoModalBG"></div>
    <!-- ############# end of modal ############## -->
</body>
</html>

