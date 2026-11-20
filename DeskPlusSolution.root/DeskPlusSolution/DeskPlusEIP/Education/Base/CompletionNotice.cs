using System;
using System.Globalization;
using System.Web;
using System.Web.UI;

namespace Zeospace.DeskPlus.Education.Web.Base
{
    internal static class CompletionNotice
    {
        private const string QueryKey = "eduCompleteNotice";

        internal static void Redirect(Page page, string viewPath,
            int companyID, int listID, int actorUserID, int targetUserID)
        {
            string token = Guid.NewGuid().ToString("N");
            string sessionKey = GetSessionKey(page, companyID, listID, actorUserID, targetUserID);
            var query = HttpUtility.ParseQueryString(page.Request.Url.Query);
            query[QueryKey] = token;
            page.Session[sessionKey] = token;

            // 성공 후 GET으로 이동하여 새로고침 시 완료 요청이 다시 전송되지 않게 합니다.
            page.Response.Redirect(viewPath + "?" + query.ToString(), false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        internal static void Show(Page page,
            int companyID, int listID, int actorUserID, int targetUserID)
        {
            if (page.IsPostBack || !String.Equals(page.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase)
                || companyID <= 0 || listID <= 0 || actorUserID <= 0 || targetUserID <= 0) return;

            string token = page.Request.QueryString[QueryKey];
            Guid parsedToken;
            if (!Guid.TryParseExact(token, "N", out parsedToken) || parsedToken == Guid.Empty) return;
            string sessionKey = GetSessionKey(page, companyID, listID, actorUserID, targetUserID);
            if (!String.Equals(page.Session[sessionKey] as string, token, StringComparison.Ordinal)) return;

            // 서버에서 실제 완료 성공 후 발급한 알림만 한 번 표시합니다.
            page.Session.Remove(sessionKey);
            page.ClientScript.RegisterStartupScript(page.GetType(), "EducationCompletionSuccess",
                "alert('완료 처리되었습니다.');", true);
        }

        private static string GetSessionKey(Page page,
            int companyID, int listID, int actorUserID, int targetUserID)
        {
            return String.Format(CultureInfo.InvariantCulture, "Education.CompletionNotice:{0}:{1}:{2}:{3}:{4}",
                companyID, listID, actorUserID, targetUserID,
                page.Request.AppRelativeCurrentExecutionFilePath.ToLowerInvariant());
        }
    }
}
