<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AttachMail.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Common.AttachMail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>   
</head>
<body onload ="init();">
    <form id="form1" runat="server">
    <div style="font-size:50px">
         File Download
    </div>
    </form>
    <script type="text/javascript">
        var url = "<%=url %>";

    </script>
    <script language="javascript">

        function init() {
            location.href = url;
            window.history.back();
        }
    </script>
</body>
</html>
