using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using LegacyMail = Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.Messenger.BLL
{
    internal static class TeamsChatReauthNotifier
    {
        private static readonly ConcurrentDictionary<string, DateTime> FallbackQueued = new ConcurrentDictionary<string, DateTime>(StringComparer.OrdinalIgnoreCase);
        internal const string StateSuffix = ".reauth-mail.sent";

        internal static void TryNotify(TeamsMSG.TeamsSettings settings, string errorCode)
        {
            string recipients = ConfigurationManager.AppSettings["TeamsReauthNotificationEmail"];
            if (string.IsNullOrWhiteSpace(recipients)) return;
            TryNotify(settings.TokenFilePath, DateTime.UtcNow,
                () => QueueMail(settings.SenderUserPrincipalName, recipients, errorCode), Log);
        }

        // 토큰 파일의 잠금을 공유하여 웹/모바일 및 IIS 재시작 사이에도 중복을 제한합니다.
        internal static void TryNotify(string tokenPath, DateTime nowUtc, Func<bool> queueMail, Action<string> log)
        {
            try
            {
                string path = Path.GetFullPath(tokenPath);
                using (TeamsChatTokenStore.AcquireLock(path))
                {
                    DateTime last;
                    string marker = path + StateSuffix;
                    if (File.Exists(marker)
                        && DateTime.TryParseExact(File.ReadAllText(marker), "O", CultureInfo.InvariantCulture,
                            DateTimeStyles.RoundtripKind, out last)
                        && nowUtc < last.ToUniversalTime().AddHours(24)) return;
                    if (FallbackQueued.TryGetValue(path, out last) && nowUtc < last.AddHours(24)) return;
                    if (!queueMail())
                    {
                        log("Teams 재로그인 안내 메일을 발송 대기열에 저장하지 못했습니다.");
                        return;
                    }
                    try { File.WriteAllText(marker, nowUtc.ToString("O", CultureInfo.InvariantCulture)); }
                    catch (Exception)
                    {
                        // 상태 저장 실패 때만 메모리로 제한합니다. 정상 경로는 공유 파일을 기준으로 합니다.
                        FallbackQueued[path] = nowUtc;
                        throw;
                    }
                }
            }
            catch (Exception)
            {
                // 메일/상태 파일 오류가 기존 업무 알림을 중단하거나 인증 정보를 노출하지 않게 합니다.
                log("Teams 재로그인 안내 메일 처리에 실패했습니다. 메일 서비스와 토큰 디렉터리 권한을 확인하세요.");
            }
        }

        // 정상 Teams 발송 후에만 제한을 초기화합니다. 계정 불일치가 계속되면 제한을 유지합니다.
        internal static void Reset(string tokenPath)
        {
            try
            {
                string path = Path.GetFullPath(tokenPath);
                using (TeamsChatTokenStore.AcquireLock(path))
                {
                    DateTime ignored;
                    FallbackQueued.TryRemove(path, out ignored);
                    if (File.Exists(path + StateSuffix)) File.Delete(path + StateSuffix);
                }
            }
            catch (Exception) { Log("Teams 재로그인 안내 메일의 발송 상태를 초기화하지 못했습니다."); }
        }

        internal static string CreateBody(string sender, string errorCode)
        {
            // Microsoft 오류 원문, 토큰, 업무 문서 주소 등은 메일에 넣지 않습니다.
            return "<p>Teams 메신저 연동의 Microsoft 재로그인이 필요합니다.</p>"
                + "<p>서버: " + WebUtility.HtmlEncode(Environment.MachineName)
                + "<br/>발신 계정: " + WebUtility.HtmlEncode(sender)
                + "<br/>확인 시각(UTC): " + DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture)
                + "<br/>오류 코드: " + WebUtility.HtmlEncode(errorCode) + "</p>"
                + "<p>실제 IIS 서버에서 관리자 Windows PowerShell을 열고 "
                + "<b>Tools\\Connect-TeamsChat.ps1</b>을 실행한 뒤 위 발신 계정으로 로그인하세요. "
                + "모바일 설정을 사용할 때는 -ConfigPath에 해당 WebAppSettings.config를 지정하세요. "
                + "자세한 명령은 Tools\\TeamsChat-Setup.txt를 참고하세요.</p>"
                + "<p>재로그인 전까지 Teams 업무 알림이 전달되지 않을 수 있습니다. "
                + "문제가 계속되면 안내 메일은 24시간에 한 번 발송합니다.</p>";
        }

        private static bool QueueMail(string sender, string recipients, string errorCode)
        {
            var addresses = new MailAddressCollection();
            addresses.Add(recipients.Replace(';', ','));
            var mail = new LegacyMail.MailDataInfo
            {
                From = new MailAddress(sender).Address,
                Subject = "[DeskPlus] Teams 메신저 재로그인 필요",
                Contents = CreateBody(sender, errorCode),
                CharacterSet = "utf-8",
                MailMessageID = Guid.NewGuid().ToString("N") + "@" + new MailAddress(sender).Host
            };
            foreach (MailAddress address in addresses)
                if (!mail.To.ContainsKey(address.Address)) mail.To.Add(address.Address, address.Address);
            string queued = new LegacyMail.SendMail().SaveEml(mail, "/DeskPlusMobile/EmlSpool", false, "", "DeskPlus");
            return !string.IsNullOrEmpty(queued) && File.Exists(queued);
        }

        private static void Log(string message)
        {
            Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException(message);
        }
    }
}
