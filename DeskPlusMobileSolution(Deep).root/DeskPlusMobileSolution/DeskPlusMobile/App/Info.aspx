<%@ Page language="c#" AutoEventWireup="true" Inherits="System.Web.UI.Page" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="ComponentArt.Web.UI" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<script language="C#" runat="server">
    public string regID = "";
    public string Android_id = "";
    public string alram = "";
    public string Str_MyPhoneNumber = "";

	private void Page_Load(object sender, System.EventArgs e)
	{
        if (!IsPostBack)
        {
            regID = Request["regid"];
            Android_id = Request["Android_id"];
            alram = Request["alram"];
            Str_MyPhoneNumber = Request["Str_MyPhoneNumber"];
        }
	}    
	
    
    
</script>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
<title></title>
</head>
<body>    
</body>
</html>
