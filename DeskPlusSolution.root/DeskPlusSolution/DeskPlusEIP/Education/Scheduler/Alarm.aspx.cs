using System;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace Zeospace.DeskPlus.Education.Web.Scheduler
{
    /// <summary>
    /// 로그인 세션 없이 작업 스케줄러에서 호출하는 재알람 페이지입니다.
    /// 호출키나 접속 IP 제한 없이 외부에서도 GET/POST로 실행할 수 있습니다.
    /// </summary>
    public partial class Alarm : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PrepareResponse();

            int statusCode = ValidateSchedulerRequest(Request.HttpMethod);

            if (statusCode != 200)
            {
                if (statusCode == 405)
                    Response.AddHeader("Allow", "GET, POST");

                WriteResult(statusCode, false, "알람 스케줄 요청을 처리할 수 없습니다.", null);
                return;
            }

            try
            {
                Server.ScriptTimeout = 600;
                // 회사와 기준일은 BLL에서 결정하며 외부 요청값은 사용하지 않습니다.
                BE.ScheduledAlarmResult result = new BLL.Alarm().RunScheduledReminders();
                if (result == null)
                {
                    WriteResult(500, false, "알람 처리 중 오류가 발생했습니다.", null);
                    return;
                }

                bool succeeded = result.FailedCount == 0 && !result.HistoryCleanupFailed;
                string message = result.FailedCount == 0
                    ? "알람 처리가 완료되었습니다."
                    : "알람 일부를 처리하지 못했습니다.";
                if (result.HistoryCleanupFailed)
                    message = result.FailedCount == 0
                        ? "알람 처리는 완료되었으나 이력을 정리하지 못했습니다."
                        : "알람 일부 처리 및 이력 정리를 완료하지 못했습니다.";
                WriteResult(succeeded ? 200 : 500, succeeded,
                    message, result);
            }
            catch (Exception)
            {
                // 상세 DB 오류를 응답 또는 예외 로그에 노출하지 않습니다.
                WriteResult(500, false, "알람 처리 중 오류가 발생했습니다.", null);
            }
        }

        private void PrepareResponse()
        {
            Response.Clear();
            Response.ContentType = "application/json";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Charset = "utf-8";
            Response.TrySkipIisCustomErrors = true;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            Response.AddHeader("X-Content-Type-Options", "nosniff");
        }

        private void WriteResult(int statusCode, bool success, string message, BE.ScheduledAlarmResult result)
        {
            Response.StatusCode = statusCode;
            Response.Write(new JavaScriptSerializer().Serialize(new
            {
                success = success,
                message = message,
                targetCount = result == null ? 0 : result.TargetCount,
                sentCount = result == null ? 0 : result.SentCount,
                skippedCount = result == null ? 0 : result.SkippedCount,
                failedCount = result == null ? 0 : result.FailedCount,
                cleanedHistoryCount = result == null ? 0 : result.CleanedHistoryCount,
                historyCleanupFailed = result != null && result.HistoryCleanupFailed
            }));
            Context.ApplicationInstance.CompleteRequest();
        }

        // 일반 페이지의 마크업을 붙이지 않고 JSON 응답만 반환합니다.
        protected override void Render(HtmlTextWriter writer)
        {
        }

        // 외부 스케줄러에서도 호출할 수 있도록 접속 IP는 제한하지 않습니다.
        // GET/POST 모두 실제 알람 발송 및 이력 정리를 실행합니다.
        internal static int ValidateSchedulerRequest(string method)
        {
            if (!String.Equals(method, "GET", StringComparison.OrdinalIgnoreCase)
                && !String.Equals(method, "POST", StringComparison.OrdinalIgnoreCase))
                return 405;

            return 200;
        }
    }
}
