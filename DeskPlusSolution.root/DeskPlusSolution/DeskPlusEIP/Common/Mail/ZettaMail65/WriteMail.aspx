<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WriteMail.aspx.cs" Inherits="Zeospace.DeskPlus.Common.Web.Mail.ZettaMail65.WriteMail" %>

<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title></title>
	<script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript">

        var From = "<%=CurrentUserInfo.MailID%>@<%=CurrentUserInfo.MailDomain%>";
        var ToMail = "<%=ToMail %>";
        var SendMailUrl = "<%=MailServerIP %>/zmail/login.nvd";

        $(function () {
            $(document).ready(function () {
                var url = SendMailUrl + "?user_account=" + From + "&cmd=write_mail_to&to=" + ToMail;                
                $("#mailSLO")[0].src = url;
            });
        });
    </script> 
</head>
<body>
	<iframe src="about:blank" id="mailSLO" width="1075px" height="675px" frameborder="0" marginheight="0" marginwidth="0" name="mailSLO" />
</body>
</html>
