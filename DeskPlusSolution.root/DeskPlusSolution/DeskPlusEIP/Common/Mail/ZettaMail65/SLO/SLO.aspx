<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SLO.aspx.cs" Inherits="Zeospace.DeskPlus.Common.Web.Mail.ZettaMail65.SLO.SLO" %>

<!DOCTYPE html>

<html>
<head id="Head1" runat="server">
    <title></title>
	 <script>

	     function init() {

	         var writeMail = "<%= WriteMail() %>";

	         mailSLO.location = writeMail;

	     }
			
    </script>
</head>
<body onload="init()">
	<iframe src="about:blank" id="mailSLO" width="1024px" height="1024px" name="mailSLO" />
</body>
</html>
