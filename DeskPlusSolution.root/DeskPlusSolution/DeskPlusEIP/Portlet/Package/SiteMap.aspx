<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SiteMap.aspx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.SiteMap" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>        
    <style type="text/css">
    body { font-family: "맑은 고딕",Dotum, "돋움체","맑은 고딕", AppleGothic, 굴림, ; font-size:12px; line-height:150% ; margin:0; padding:0;  color:#303030; -webkit-text-size-adjust:none;word-break:keep-all;letter-spacing:; /*scrollbar:yes;*/overflow:hidden !important;overflow-y:auto !important;overflow-x:none !important;}
    a {color:#303030; text-decoration:none; border:0px;cursor:pointer;}
    a:hover {color:#06F;text-decoration:none; }
	#page-content { padding:20px 0 10px;cursor:default;font-size:12px;line-height:24px; }	
	#page-content .column { float:left;width:235px;margin:0 10px 0 10px;padding:10px 5px 10px 0;display:inline; }	
	#page-content .column div { margin:0 0 23px; }

	#page-content h3 { margin:0 0 12px;padding:0 0 0 10px;height:35px;line-height:33px;font-size:13px;font-weight:600;color:#222;border-bottom:1px solid #d7d7d7; }
	#page-content .column4 h3 { margin:0 0 22px; }
	#page-content h4 { margin:0;padding:0 0 2px 10px;color:#999;font-weight:normal;line-height:22px; }
	#page-content ul { margin:0;padding:0;list-style:none; }
	#page-content li,#page-content span.title { padding:0 0 0 40px;background:transparent url("<%=BaseImgPath%>/icon/sitemap.png") no-repeat 0 -200px;line-height:40px; }
	#page-content li.chevron { margin:0 0 0 10px;padding-left:14px;background-position: 0 -151px;line-height:20px; }
	#page-content li.no-background { background:none;padding:0; }

</style>

</head>
<body>
    <form id="form1" runat="server">        
    <div id="page-content">
        <%=TopMenu %>
    </div>
    
    <br /><br />
    
    </form>
</body>
</html>
