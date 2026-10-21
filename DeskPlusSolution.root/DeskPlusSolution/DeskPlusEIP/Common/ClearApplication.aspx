<%@ Page Language="C#" AutoEventWireup="true" Inherits="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BasePage" %>

<%@ Import Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper" %>
<script language="C#" runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        #region APPLICATION 초기화

        //GMU 동적쿼리 APPLICATION 초기화하기
        AppSettingHelper.ClearApplication(AppSettingType.ACCOUNT);
        AppSettingHelper.ClearApplication(AppSettingType.ADDRESS);
        AppSettingHelper.ClearApplication(AppSettingType.ALARM);
        AppSettingHelper.ClearApplication(AppSettingType.ADMIN);
        AppSettingHelper.ClearApplication(AppSettingType.APPROVAL);
        AppSettingHelper.ClearApplication(AppSettingType.BASE);
        AppSettingHelper.ClearApplication(AppSettingType.BOARD);
        AppSettingHelper.ClearApplication(AppSettingType.COMMON);
        AppSettingHelper.ClearApplication(AppSettingType.COMMUNITY);
        AppSettingHelper.ClearApplication(AppSettingType.EIP);
        AppSettingHelper.ClearApplication(AppSettingType.OPINION);
        AppSettingHelper.ClearApplication(AppSettingType.PIM);
        AppSettingHelper.ClearApplication(AppSettingType.PMS);
        AppSettingHelper.ClearApplication(AppSettingType.POLL);
        AppSettingHelper.ClearApplication(AppSettingType.PORTLET);
        AppSettingHelper.ClearApplication(AppSettingType.RESOURCEMANAGER);
        AppSettingHelper.ClearApplication(AppSettingType.WEBHARD);
        AppSettingHelper.ClearApplication(AppSettingType.REPORT);
        AppSettingHelper.ClearApplication(AppSettingType.COOP);
        AppSettingHelper.ClearApplication(AppSettingType.TASK);
        AppSettingHelper.ClearApplication(AppSettingType.MDM);
        

        //각 모듈별 환경정보 APPLICATION 초기화 하기 (dbo.DP_COM_Env)
        AppSettingHelper.ClearConfigApplication(AppSettingType.ACCOUNT);
        AppSettingHelper.ClearConfigApplication(AppSettingType.ADDRESS);
        AppSettingHelper.ClearConfigApplication(AppSettingType.ALARM);
        AppSettingHelper.ClearConfigApplication(AppSettingType.ADMIN);
        AppSettingHelper.ClearConfigApplication(AppSettingType.APPROVAL);
        AppSettingHelper.ClearConfigApplication(AppSettingType.BASE);
        AppSettingHelper.ClearConfigApplication(AppSettingType.BOARD);
        AppSettingHelper.ClearConfigApplication(AppSettingType.COMMON);
        AppSettingHelper.ClearConfigApplication(AppSettingType.COMMUNITY);
        AppSettingHelper.ClearConfigApplication(AppSettingType.EIP);
        AppSettingHelper.ClearConfigApplication(AppSettingType.OPINION);
        AppSettingHelper.ClearConfigApplication(AppSettingType.PIM);
        AppSettingHelper.ClearConfigApplication(AppSettingType.PMS);
        AppSettingHelper.ClearConfigApplication(AppSettingType.POLL);
        AppSettingHelper.ClearConfigApplication(AppSettingType.PORTLET);
        AppSettingHelper.ClearConfigApplication(AppSettingType.RESOURCEMANAGER);
        AppSettingHelper.ClearConfigApplication(AppSettingType.WEBHARD);
        AppSettingHelper.ClearConfigApplication(AppSettingType.REPORT);
        AppSettingHelper.ClearConfigApplication(AppSettingType.COOP);
        AppSettingHelper.ClearConfigApplication(AppSettingType.TASK);
        AppSettingHelper.ClearConfigApplication(AppSettingType.MDM);  

        //시스템 코드정보 APPLICATION 초기화 하기 (dbo.DP_COM_SYSCode)
        Application.Lock();
        Application.Remove("SysCode" + CurrentUserInfo.CompanyID);
        Application.UnLock();

        #endregion

        Response.Write("Application is clear. time is " + System.DateTime.Now);
        Response.Write("<br><br>");

        #region Cache 초기화

        foreach (DictionaryEntry de in this.Cache)
        {
            Response.Write("session is clear. time is " + de.Key);
            Cache.Remove(de.Key.ToString());
            Response.Write("<br><br>");
        }

        Response.Write("<br><br>");

        #endregion


    }
</script>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title>
        <%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    </div>
    </form>
</body>
</html>
