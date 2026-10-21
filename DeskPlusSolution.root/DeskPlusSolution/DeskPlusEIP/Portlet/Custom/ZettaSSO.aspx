<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Drawing" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<%@ Import Namespace="System.Web.UI.HtmlControls" %>
<%@ Import Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks" %>
<%@ Import Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper" %>
<%@ Import Namespace="System.Text" %>
<%@ Page Language="C#" EnableViewState="false" Inherits="Zeospace.DeskPlus.Portlet.Web.Base.BasePagePortlet" %>

<script language="C#" runat="server">
    private void Page_Load(object sender, System.EventArgs e)
    {
        if (!IsPostBack)
        {
            #region 사용자정보가져오기
            string mailID = CurrentUserInfo.MailID;
            //string domain = CurrentUserInfo.MailDomain;
            string domain = "zetta.zeospace.com";
            #endregion

            //string serverUrl = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "MailServerIP", "http://zetta.zeospace.com:4040");
			string serverUrl = "http://zetta.zeospace.com:4040";

            string url = "{0}/zmail/login.nvd?mode=ZettaAdmin&cmd=receive_mailbox&user_account={1}";
            url = string.Format(url, serverUrl, mailID + "@" + domain);

            ifrm.Attributes.Add("src", url);
        }
    }        
</script>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
</head>
<body topmargin="0" leftmargin="0" marginwidth="0" marginheight="0">
    <form id="Form1" method="post" runat="server">
        <iframe id="ifrm" name="ifrm" scrolling="auto" frameborder="0" width="100%" height="100%" runat="server"></iframe>
    </form>
</body>
</html>
