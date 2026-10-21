<%@ Page language="c#" Codebehind="ErrorDefault.aspx.cs" AutoEventWireup="True" ValidateRequest="false" Inherits="Zeospace.DeskPlus.Base.Web.Html.ErrorMessage.ErrorDefault" %>
<!DOCTYPE html>
<html>
	<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
	<script type="text/javascript" language="javascript">
	    function GoBack() {
	        history.back();
	    }
	</script>    
	</head>
	<body>
	    <form runat="server">
        <center>
        <table width="325" border="0" cellspacing="0" cellpadding="0" style="padding-top:100px;">
          <tr>
            <td height="188" align="center" background="<%=BaseImgPath%>/icon/ico_sys_bg.gif">
            <table width="90%" border="0" cellspacing="0" cellpadding="5">
              <tr>
                <td>&nbsp;</td>
              </tr>
              <tr>
                <td align="center" style="font-size:14px;font-weight:bold;"><img src="<%=BaseImgPath%>/icon/ico_sy_icon.gif" align="absmiddle"/><%=GetResource("ErrorTitle")%></td>
              </tr>
              <tr>
                <td align="center"><%=GetResource("ErrorDescription")%></td>
              </tr>
              <tr>
                <td height="50" align="center"><a href="javascript:GoBack();"><img src="<%=BaseImgPath%>/Lang/<%=PreLang%>/btn_sys.gif" border="0" /></a></td>
              </tr>
            </table></td>
          </tr>
        </table>    
        </center>
		</form>
	</body>
</html>
