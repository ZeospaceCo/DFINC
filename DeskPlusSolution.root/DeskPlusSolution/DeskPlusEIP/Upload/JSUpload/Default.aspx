<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeospace.DeskPlus.Base.Web.Upload.JSUpload.Default" %>
<%@ OutputCache Location="None" VaryByParam="None" %>
<!DOCTYPE html>

<html>
<head id="Head1" runat="server">
    <meta charset="UTF-8">
    <meta name="csrf-token" content="<%=csrf_token%>">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <script type="text/javascript">
        var csrf_token = $('meta[name="csrf-token"]').attr('content');

        function csrfSafeMethod(method) {
            // these HTTP methods do not require CSRF protection
            return (/^(GET|HEAD|OPTIONS)$/.test(method));
        }

        $.ajaxSetup({
            beforeSend: function (xhr, settings) {
                if (!csrfSafeMethod(settings.type) && !this.crossDomain) {
                    xhr.setRequestHeader("anti-csrf-token", csrf_token);
                }
            }
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:HiddenField ID="forgeryToken" runat="server"/>
</form>
</body>
</html>