<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeskPlusEPDefault.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.DeskPlusEPDefault" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Frameset//EN" "http://www.w3.org/TR/xhtml1.DTD/xhtml1-frameset.dtd"> 
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="UTF-8">
	<meta name="viewport" content="width=device-width">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <script type="text/javascript">
		if(('ontouchstart' in window) || (navigator.maxTouchPoints > 0) ||  (navigator.msMaxTouchPoints > 0)) {
			viewport = document.querySelector("meta[name=viewport]");
			viewport.setAttribute('content', 'width=1400');
		}
        <%
            foreach (System.Collections.Generic.KeyValuePair<string, string> content in PopupContent)
            {
                if (Request.Cookies[content.Key] == null)
                {
                    Response.Write(content.Value);
                }
            }
        %>
    </script>
</head>
<frameset rows="100,*" border="0px" framespacing="0px">
    <frame border="0px" frameborder="0" framespacing="0px" marginwidth="0px" marginheight="0px" src="Package/NMenu.aspx?p=<%=GetParm("p","")%>&d=<%=GetParm("dcparm","")%>&t=<%=GetParm("t","")%>" id="frmTop" name="frmTop" style="margin:auto;" scrolling="no" noresize="noresize" title=""></frame>
    <frame border="0px" frameborder="0" framespacing="0px" marginwidth="0px" marginheight="0px" src="<%=MainUrl%>" id="frmMain" name="frmMain" scrolling="auto" noresize="noresize" title=""></frame>
</frameset>
<noframes></noframes>
</html>
