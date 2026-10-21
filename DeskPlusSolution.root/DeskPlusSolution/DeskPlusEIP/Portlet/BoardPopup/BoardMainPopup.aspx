<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BoardMainPopup.aspx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.BoardPopup.BoardMainPopup" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>
        <%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <style type="text/css">        
        body {    
            margin: 0 !important;
            padding: 0 !important;
            border: 4px #cccccc solid;
        }
    </style>
    <script type="text/javascript">
        var popid = '<%=Request.QueryString["popid"]%>';
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Portlet/Js/BoardPopup/BoardMainPopup.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <table border="0" style="width: 100%; height: 100%" cellspacing="5" cellpadding="0">            
            <tr>
                <td valign="top">
                    <iframe src="<%=Url %>" id="ifrm" name="ifrm" width="98%" height="<%=PopupHeight%>px" frameborder="0" marginheight="0" marginwidth="0"></iframe>                
                </td>
            </tr>
            <tr>
                <td align="right" bgcolor="#4682b4" height="20px">
                    <a href="javascript:ViewChecked()" style="color: #FFFFFF; font-size: small"><%=GetResource("ForWeek")%></a> &nbsp;&nbsp;&nbsp;&nbsp; <a href="javascript:saveCookie()" style="color: #FFFFFF; font-size: small">
                    <%=GetResource("NotOpen")%></a>&nbsp;&nbsp; 
                    <a href="javascript:PopupClose()" style="color: #FFFFFF; font-size: small"><%=GetResource("Close")%></a>&nbsp;&nbsp;
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
