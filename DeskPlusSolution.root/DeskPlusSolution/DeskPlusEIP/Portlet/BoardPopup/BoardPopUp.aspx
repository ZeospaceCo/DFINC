<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BoardPopUp.aspx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.BoardPopup.BoardPopUp" %>

<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
        <style type="text/css">
        .style1 {
            width: 100%;
            height: 100%;
        }
        .style2
        {
            height: 700px;
        }
    </style>
    <script type="text/javascript" >
        var popid = '<%=Request.QueryString["popid"]%>';

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Portlet/Js/BoardPopup/BoardPopup.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%" class="style1" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td class="style2" valign="top">
					<table style="width: 100%; height:100%" cellspacing="5">
                        <tr>
                            <td align="right" bgcolor="#999999" height="30">
                                <a href="javascript:ViewChecked()" style="color: #FFFFFF; font-size:small" ><%=GetResource("ForWeek")%></a> &nbsp;&nbsp;&nbsp;&nbsp;
                                <a href="javascript:saveCookie()" style="color: #FFFFFF; font-size:small" ><%=GetResource("NotOpen")%></a>&nbsp;&nbsp;&nbsp;&nbsp;
                                <a href="javascript:neverShowAgain()" style="color: #FFFFFF; font-size:small" >다시 띄우지 않기</a>&nbsp;&nbsp;
                            </td>
                        </tr>
						<tr>
							<td valign="top" >
                                <iframe src="<%=Url %>" width="100%" height="700px" frameborder=0 marginheight=0 marginwidth=0></iframe>
                            </td>
						</tr>
					</table>
				</td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
