<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default11.aspx.cs" Inherits="Zeospace.DeskPlus.Base.Web.Site.Auth.Default11" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
	<meta name="robots" content="noindex">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DStyle/bootstrap.min.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DStyle/style.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />

    <style type="text/css">
        
    .login{
	    background: url('<%=BGLoginImage%>') no-repeat center center fixed;       -webkit-background-size: cover;-moz-background-size: cover;-o-background-size: cover;background-size: cover;
	    display: -ms-flexbox;  display: flex;  -ms-flex-align: center;  align-items: center;  padding-top: 40px; padding-bottom: 40px;  background-color: #000;
    }

    </style>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery-1.12.4.min.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.placeholder.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>

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
<body class="login">  
    
    <!-- content -->
    <div class="loginimgoverbox"></div>
    <div class="login-logo-area">
	  <div class="logo"><img src="<%=LoginImage%>"></div>
      <div class="text-white-50">PHONE : 02-2108-3070 FAX : 02-2108-3080 <br /></div>
    </div>
    <div class="position-fixed smartplusv">
      <img src="<%=VLoginImage%>" />
    </div>
    <div class="container">

	    <div class="login-login-box ">
        <form id="form1" runat="server" class="form-signin">
          <asp:TextBox ID="tbLoginID" TabIndex="1" class="form-control" runat="server"></asp:TextBox>
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

          <div style="display:none">
            <asp:LinkButton ID="lbtnSubmit" OnClick="lbtnSubmit_Click" runat="server"></asp:LinkButton>
          </div>   

          </form>
        </div>

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
    <form  name="formPwd" method="post">
        <input type="hidden" name="companyID" id="companyID" />
        <input type="hidden" name="loginID" id="loginID" />
    </form>
</body>
</html>
