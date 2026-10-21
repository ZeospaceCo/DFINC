<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NoAuth.aspx.cs" Inherits="Zeospace.DeskPlus.Base.Web.Html.ErrorMessage.NoAuth" %>

<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>

	<script type="text/javascript" language="javascript">

	    var ImgPath = "<%=BaseImgPath%>";
	    var PreLang = "<%=PreLang%>";


	    $(function () {

	        $(document).ready(function () {

	            //탭추가
	            $('#btnAction').click(function (e) {
	                e.preventDefault();

	                if (top.opener) {
	                    try {
	                        self.close();
	                    }
	                    catch (Error) {
	                        history.back();
	                    }
	                }
	                else {
	                    history.back();
	                }
	            });

	            if (top.opener) {
                
	                try {
	                    $('#imgAction').attr("src", ImgPath + "/Lang/" + PreLang + "/btn_sys.gif");
	                }
	                catch (Error) {
	                    $('#imgAction').attr("src", ImgPath + "/Lang/" + PreLang + "/btn_sys.gif");
	                }
	            }
	            else {
	                $('#imgAction').attr("src", ImgPath + "/Lang/" + PreLang + "/btn_sys.gif");
	            }



	        });
	    });    
	
	</script>    
</head>
<body>
    <form id="form1" runat="server">
        <center>
        <table width="325" border="0" cellspacing="0" cellpadding="0" style="padding-top:100px;">
          <tr>
            <td height="188" align="center" background="<%=BaseImgPath%>/icon/ico_sys_bg.gif">
            <table width="90%" border="0" cellspacing="0" cellpadding="5">
              <tr>
                <td>&nbsp;</td>
              </tr>
              <tr>
                <td align="center" style="font-size:14px; font-weight:bold;"><img src="<%=BaseImgPath%>/Icon/ico_sy_icon.gif" align="absmiddle"/><%=GetResource("ErrorAuth")%></td>
              </tr>
              <tr>
                <td align="center"><%=GetResource("ErrorAuthDescription")%></td>
              </tr>
              <tr>
                <td height="50" align="center">
                    <a id="btnAction">
                        <img id="imgAction" width="49" height="23" border="0" />                    
                    </a>
                </td>
              </tr>
            </table></td>
          </tr>
        </table>    
        </center>
    </form>
</body>
</html>
