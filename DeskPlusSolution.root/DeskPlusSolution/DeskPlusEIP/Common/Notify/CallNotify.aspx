<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CallNotify.aspx.cs" Inherits="Zeospace.DeskPlus.Common.Web.Notify.CallNotify" %>

<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link type="text/css" href="/DeskPlusEIP/Html/Js/JQDialog/jqdialog.css" rel="stylesheet"/>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script  type="text/javascript"src="/DeskPlusEIP/Html/Js/JQDialog/jqdialog.js"></script>
    <script type="text/javascript">
        var GoPage = "<%=GoPage %>";
        var Msg = "<%=MSG %>";
        var R_Alert_Yes = "<%=GetResource("Alert_Yes") %>";
        var R_Alert_No = "<%=GetResource("Alert_No") %>";
        var R_Alert_Cancel = "<%=GetResource("Alert_Cancel") %>";
        var R_Alert_Ok = "<%=GetResource("Alert_Ok") %>";
        var R_Alert_MovePage = "<%=GetResource("Alert_MovePage") %>";
        var R_Alert_CallNotifyMessage = "<%=GetResource("CallNotifyMessage") %>";
        var Timer = 2;
        $(function () {
            $(document).ready(function () {

                $.jqDialog.labels.yes = R_Alert_Yes;
                $.jqDialog.labels.no = R_Alert_No;
                $.jqDialog.labels.ok = R_Alert_Ok;
                $.jqDialog.labels.cancel = R_Alert_Cancel;
                $.jqDialog.init();
                //$.jqDialog.alert(Msg + "<br/><br/>" + Timer + R_Alert_MovePage, Timer, function () {

               var NewMessage = "<div style='border 1px solid #f00;'>&nbsp;&nbsp;" + Msg + "...<br><br>";
               NewMessage += "<img src=\"/DeskPlusEIP/Html/Js/JQDialog/c_loader.gif\"><br></p></div>";
   

                $.jqDialog.alert(NewMessage, Timer, function () {
                    location = GoPage;
                });
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
