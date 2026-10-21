<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeospace.DeskPlus.Base.Web.Util.jTemplate.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server"><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <script src="../../Html/Js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../Html/Js/jQuery/jtemplates.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CallService() {
            $.ajax({
                type: "POST",
                url: "MyService.asmx/GetCustomer",
                data: "{customerID:" + $("#ddlCustomer").val() + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: Success,
                error: Error
            });
        }

        function Success(data, status) {
            $('#placeholder').setTemplateURL('JTemplates/Template.htm');
            $('#placeholder').processTemplate(data.d);
        }

        function Error(request, status, error) {
            $("#placeholder").html(request.statusText);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:DropDownList ID="ddlCustomer" runat="server">
                <asp:ListItem Value="0">Joe</asp:ListItem>
                <asp:ListItem Value="1">Tom</asp:ListItem>
                <asp:ListItem Value="2">Emily</asp:ListItem>
                <asp:ListItem Value="3">Leroy</asp:ListItem>
                <asp:ListItem Value="4">Saul</asp:ListItem>
            </asp:DropDownList>

            <asp:Button ID="btnGetDetails" Text="Get Details" OnClientClick="CallService(); return false;" runat="server" />

            <div id="placeholder" style="clear: both;"></div>
        </div> 
    </form>
</body>
</html>
