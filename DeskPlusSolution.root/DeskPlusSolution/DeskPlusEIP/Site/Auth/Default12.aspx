<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default12.aspx.cs" Inherits="Zeospace.DeskPlus.Base.Web.Site.Auth.Default12" %>

<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
	<meta name="robots" content="noindex">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Login.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <style type="text/css">
        .placeholder { color: #aaa; }
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
<body>
    <form id="form1" runat="server">
    <!-- header -->
    <div id="hd">
        <span style="background:url(<%=LoginImage%>) no-repeat 10% !important;"></span>
    </div>
    <!-- //header -->
    <hr />
    <!-- content -->
    <div id="cH">
        <p class="ch_tit1">
            Member's Login</p>
        <p class="ch_tit2">
            <%=GetResource("LoginMsg_01")%>
        </p>
    </div>
    <div id="ct">
        <div class="sc1">
            <fieldset>
                <legend class="hc"><%=GetResource("Login")%></legend>
                <div class="ia">
                    <ul class="ip">
                        <li>
                            <label for="id" class="hc">
                                <%=GetResource("ID")%></label><span class="it" id="idb"><asp:TextBox ID="tbLoginID" TabIndex="1" CssClass="il"
                                    runat="server"></asp:TextBox></span></li>
                        <li>
                            <label for="pw" class="hc">
                                <%=GetResource("Password")%></label><span class="it" id="pwb"><asp:TextBox ID="tbPassword" TabIndex="2" CssClass="pl"
                                    runat="server" TextMode="Password"></asp:TextBox></span></li>
                    </ul>
                    <p class="ac">
                        <!--a onclick="loginProcess()">로그인</a-->
                        <input type="submit" value="<%=GetResource("Login")%>" id="Submit1" />                        
                    </p>                    
                </div>
                <div class="ot">
                    <p class="sv">
                        <asp:CheckBox ID="chkRemember" TabIndex="3" runat="server" Visible="true"></asp:CheckBox>
                        <label><%=GetResource("SaveID")%></label>
                        <li>
                            <label for="company" class="hc"><%=GetResource("Company")%></label>
                            <asp:DropDownList ID="ddlCompany" runat="server">
                            </asp:DropDownList>
                            <asp:CheckBox ID="chkCompany" TabIndex="6" runat="server" Visible="true"></asp:CheckBox>
                            <span style="font-size:0.7em;color:#74749C;"><%=GetResource("SaveCompany")%></span><div class="ac" style="font-size:0.7em;color:#74749C;cursor:pointer;display:none" id="searchPassword" >[패스워드 찾기]</div>
                        </li>
                    </p>
                   
                </div>
               
            </fieldset>
        </div>
    </div>
    <hr />
    <!-- copyright -->
    <address id="cr">
        &copy; <a href="http://www.zeospace.com/" target="_blank">Zeospace Corp.</a></address>
    <!-- //copyright -->
    <div style="display:none">
    <asp:LinkButton ID="lbtnSubmit" OnClick="lbtnSubmit_Click" runat="server"></asp:LinkButton>
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
