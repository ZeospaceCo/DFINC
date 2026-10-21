<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Preview.aspx.cs" Inherits="Zeospace.DeskPlus.Base.Web.Preview" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
        <style type="text/css">
        .style1 {
            width: 100%;
            height: 100%;
        }
        .style2
        {
            height: 100%;
        }
    </style>
    <script type="text/javascript">
        <!--

        var managerID = '<%=Request.QueryString["managerID"]%>';
        function setCookie(name, value, expiredays) {
            var todayDate = new Date();
            var year = todayDate.getFullYear();
            var month = todayDate.getMonth();
            var day = todayDate.getDate() + 1;
            todayDate = new Date(year, month, day);
            document.cookie = name + "=" + escape(value) + "; path=/; expires=" + todayDate.toGMTString() + ";"
        }

        function saveCookie() {

            setCookie(managerID, "yes", 1);
            self.close();

        }

        function movePop() {
            var url = "";
            if (url != "") {
                opener.location.href = url;
            }
            self.close();
        }
        //-->
    </script>


</head>

<body bottommargin="0"  topmargin="0" leftmargin="1" rightmargin="1"  onload="this.focus();">
   <form id="form1" runat="server">
    <div>
        <table class="style1" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td style="height:<%=Height%>px" valign="top">
						<table style="width: 100%; height:100%" cellspacing="5">
							<tr>
								<td valign="top"><%=Contents%></td>
							</tr>
						</table>
				</td>
            </tr>
            <tr>
                <td align="right" bgcolor="#999999" height="30">
                    <a href="javascript:saveCookie()" style="color: #FFFFFF;text-decoration:none;"><%=GetResource("NotOpen")%></a>&nbsp;&nbsp; 
                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
