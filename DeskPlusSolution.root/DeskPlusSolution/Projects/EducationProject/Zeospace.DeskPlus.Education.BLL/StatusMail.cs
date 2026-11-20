using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail;

namespace Zeospace.DeskPlus.Education.BLL
{
    public enum StatusMailBatchOutcome
    {
        Sent,
        RolledBack,
        Unknown
    }

    /// <summary>
    /// 관리자 요청 메일을 EML로만 접수합니다. 실제 발송은 기존 메일 수집 서비스가 담당합니다.
    /// </summary>
    public class StatusMail
    {
        private const string StagingVirtualPath = "/DeskPlusEIP/Education/App_Data/MailSpool";
        private const string SpoolVirtualPath = "/DeskPlusEIP/EmlSpool";

        /// <summary>
        /// 계정의 메일 아이디와 도메인으로 한 개의 유효한 메일 주소를 구성합니다.
        /// </summary>
        public static bool TryGetAddress(string mailID, string mailDomain, out string address)
        {
            address = String.Empty;
            if (String.IsNullOrWhiteSpace(mailID) || String.IsNullOrWhiteSpace(mailDomain)
                || mailID.IndexOf('@') >= 0 || mailDomain.IndexOf('@') >= 0)
                return false;
            foreach (char character in mailID + mailDomain)
                if (Char.IsControl(character)) return false;

            string candidate = mailID.Trim() + "@" + mailDomain.Trim();
            if (!IsValidAddress(candidate)) return false;
            address = candidate;
            return true;
        }

        /// <summary>
        /// 표시명, 여러 수신자, 헤더 개행을 포함하지 않는 단일 주소만 허용합니다.
        /// </summary>
        public static bool IsValidAddress(string address)
        {
            if (String.IsNullOrEmpty(address) || address != address.Trim()) return false;
            int separator = address.IndexOf('@');
            if (separator <= 0 || separator != address.LastIndexOf('@') || separator == address.Length - 1)
                return false;
            foreach (char character in address)
            {
                if (Char.IsControl(character) || Char.IsWhiteSpace(character)
                    || character == '<' || character == '>' || character == ';' || character == ',')
                    return false;
            }

            try
            {
                MailAddress parsed = new MailAddress(address);
                return parsed.DisplayName.Length == 0
                    && String.Equals(parsed.Address, address, StringComparison.Ordinal);
            }
            catch (FormatException) { return false; }
            catch (ArgumentException) { return false; }
        }

        /// <summary>
        /// 전원의 EML을 먼저 준비하고 완성된 배치만 수집 폴더에 게시합니다.
        /// 게시 중 실패하면 이번 배치의 파일만 회수하며, 수집 여부를 모르면 Unknown을 반환합니다.
        /// </summary>
        public StatusMailBatchOutcome QueueCompletionRequests(BE.ListInfo info, string senderMail,
            IList<KeyValuePair<int, string>> recipients, string progressStatus)
        {
            string batchID = Guid.NewGuid().ToString("N");
            string requestDirectory = String.Empty;
            string phase = "VALIDATION";
            int recipientID = 0;
            List<StagedMail> files = new List<StagedMail>();
            StatusMailBatchOutcome outcome = StatusMailBatchOutcome.RolledBack;
            try
            {
                if (!IsValidInfo(info, progressStatus) || !IsValidAddress(senderMail)
                    || recipients == null || recipients.Count == 0)
                {
                    LogFailure(info, batchID, "REQUEST_VALIDATION_FAILED", 0, null);
                    return StatusMailBatchOutcome.RolledBack;
                }

                // 호출자의 목록 변경에 영향을 받지 않도록 복사한 전원을 먼저 검증합니다.
                List<KeyValuePair<int, string>> targets = new List<KeyValuePair<int, string>>(recipients);
                HashSet<int> seenUsers = new HashSet<int>();
                foreach (KeyValuePair<int, string> target in targets)
                {
                    if (target.Key <= 0 || !seenUsers.Add(target.Key) || !IsValidAddress(target.Value))
                    {
                        LogFailure(info, batchID, "RECIPIENT_VALIDATION_FAILED", target.Key, null);
                        return StatusMailBatchOutcome.RolledBack;
                    }
                }

                phase = "MESSAGE_PREPARATION";
                List<MailDataInfo> messages = new List<MailDataInfo>();
                foreach (KeyValuePair<int, string> target in targets)
                {
                    recipientID = target.Key;
                    string targetUrl = CreateTargetUrl(info.CompanyID, info.ListID, target.Key);
                    MailDataInfo message = CreateMessage(info, senderMail, target.Value, progressStatus, targetUrl);
                    if (message == null) throw new InvalidOperationException("Mail message preparation failed.");
                    messages.Add(message);
                }
                if (HttpContext.Current == null) throw new InvalidOperationException("Web context is unavailable.");

                // 두 경로는 서버 고정값입니다. 클라이언트 입력으로 저장 경로를 받지 않습니다.
                phase = "STAGING_PATH";
                recipientID = 0;
                string stagingDirectory = Path.GetFullPath(HttpContext.Current.Server.MapPath(StagingVirtualPath));
                string spoolDirectory = Path.GetFullPath(HttpContext.Current.Server.MapPath(SpoolVirtualPath));
                if (!String.Equals(Path.GetPathRoot(stagingDirectory), Path.GetPathRoot(spoolDirectory),
                    StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException("Mail staging and publication must use the same volume.");
                string requestVirtualPath = StagingVirtualPath + "/" + batchID;
                string candidateDirectory = Path.GetFullPath(HttpContext.Current.Server.MapPath(requestVirtualPath));
                if (!String.Equals(Path.GetDirectoryName(candidateDirectory), stagingDirectory.TrimEnd('\\', '/'),
                        StringComparison.OrdinalIgnoreCase)
                    || !String.Equals(Path.GetFileName(candidateDirectory), batchID, StringComparison.OrdinalIgnoreCase)
                    || Directory.Exists(candidateDirectory) || File.Exists(candidateDirectory))
                    throw new InvalidOperationException("Mail staging directory is invalid.");

                Directory.CreateDirectory(candidateDirectory);
                requestDirectory = candidateDirectory;
                phase = "EML_PREPARATION";
                HashSet<string> seenFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                SendMail writer = new SendMail();
                for (int index = 0; index < messages.Count; index++)
                {
                    recipientID = targets[index].Key;
                    string savedFile = writer.SaveEml(messages[index], requestVirtualPath, false, String.Empty);
                    if (!IsStagedMailFile(savedFile, requestDirectory) || !File.Exists(savedFile))
                        throw new InvalidOperationException("Mail EML preparation failed.");
                    string fullPath = Path.GetFullPath(savedFile);
                    if (!seenFiles.Add(fullPath) || new FileInfo(fullPath).Length == 0)
                        throw new InvalidOperationException("Mail EML file is invalid.");
                    files.Add(new StagedMail
                    {
                        RecipientID = recipientID,
                        StagedPath = fullPath,
                        PublishedPath = Path.Combine(spoolDirectory, Path.GetFileName(fullPath))
                    });
                }

                // 이 지점까지 한 명이라도 실패하면 수집 폴더에는 파일이 하나도 게시되지 않습니다.
                phase = "PUBLICATION";
                recipientID = 0;
                Directory.CreateDirectory(spoolDirectory);
                foreach (StagedMail file in files)
                {
                    recipientID = file.RecipientID;
                    File.Move(file.StagedPath, file.PublishedPath);
                    file.Published = true;
                }
                // 이동 직후 메일 수집기가 파일을 가져갈 수 있으므로 존재 여부를 다시 검사하지 않습니다.
                outcome = StatusMailBatchOutcome.Sent;
            }
            catch (Exception exception)
            {
                LogFailure(info, batchID, phase, recipientID, exception);
                outcome = RollbackPublishedFiles(info, batchID, files);
            }
            finally
            {
                // 공통 생성기가 실패 시 파일 경로를 반환하지 않아도 이번 요청의 미완성 EML을 정리합니다.
                // 요청별 새 폴더 안의 GUID EML만 삭제하며 발행된 파일은 수정하거나 삭제하지 않습니다.
                if (!String.IsNullOrEmpty(requestDirectory))
                {
                    try
                    {
                        foreach (string stagedFile in Directory.GetFiles(requestDirectory, "*.eml", SearchOption.TopDirectoryOnly))
                            if (IsStagedMailFile(stagedFile, requestDirectory)) File.Delete(stagedFile);
                        Directory.Delete(requestDirectory, false);
                    }
                    catch (Exception exception)
                    {
                        // 빈 임시 폴더 정리 실패로 이미 전원 게시에 성공한 결과를 뒤집지 않습니다.
                        LogFailure(info, batchID, "STAGING_CLEANUP_FAILED", 0, exception);
                    }
                }
            }
            return outcome;
        }

        private sealed class StagedMail
        {
            public int RecipientID;
            public string StagedPath;
            public string PublishedPath;
            public bool Published;
        }

        private static StatusMailBatchOutcome RollbackPublishedFiles(BE.ListInfo info, string batchID,
            IList<StagedMail> files)
        {
            bool unknown = false;
            for (int index = files.Count - 1; index >= 0; index--)
            {
                StagedMail file = files[index];
                // File.Move가 성공한 경로만 회수합니다. 이동 실패 대상의 기존 동명 파일은 건드리지 않습니다.
                if (!file.Published) continue;
                // 업체가 원본을 지우기 전에 이미 읽거나 복사했을 수 있으므로,
                // 한 번이라도 노출된 배치는 파일 회수에 성공해도 미발송을 보장할 수 없습니다.
                unknown = true;
                try
                {
                    // 존재 확인 후 삭제 사이에 수집될 수 있어, 먼저 비공개 폴더로 원자적으로 회수합니다.
                    // 원본이 이미 없거나 회수/삭제에 실패하면 발송 여부를 단정할 수 없습니다.
                    File.Move(file.PublishedPath, file.StagedPath);
                    file.Published = false;
                    File.Delete(file.StagedPath);
                    if (File.Exists(file.StagedPath))
                        throw new IOException("Recovered mail file could not be removed.");
                }
                catch (Exception exception)
                {
                    unknown = true;
                    LogFailure(info, batchID, "ROLLBACK_UNCERTAIN", file.RecipientID, exception);
                }
            }
            return unknown ? StatusMailBatchOutcome.Unknown : StatusMailBatchOutcome.RolledBack;
        }

        private static bool IsValidInfo(BE.ListInfo info, string progressStatus)
        {
            return info != null && info.CompanyID > 0 && info.ListID > 0 && info.WriteUserID > 0
                && (progressStatus == "2" || progressStatus == "4");
        }

        private static string CreateTargetUrl(int companyID, int listID, int userID)
        {
            string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
            string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
            Uri siteUri;
            if (!Uri.TryCreate(baseHttp + fullDomain, UriKind.Absolute, out siteUri)
                || (siteUri.Scheme != Uri.UriSchemeHttp && siteUri.Scheme != Uri.UriSchemeHttps)
                || siteUri.UserInfo.Length > 0 || siteUri.Query.Length > 0 || siteUri.Fragment.Length > 0)
                return String.Empty;

            string encrypted = MailViewLink.Create(companyID, listID, userID);
            if (String.IsNullOrEmpty(encrypted)) return String.Empty;

            // 로그인 암호나 자동 로그인 URL을 메일에 포함하지 않습니다.
            Uri target = new Uri(siteUri, "/DeskPlusEIP/Education/Content/ViewPopup.aspx?p="
                + HttpUtility.UrlEncode(encrypted));
            return target.AbsoluteUri;
        }

        private static MailDataInfo CreateMessage(BE.ListInfo info, string senderMail, string recipientMail,
            string progressStatus, string targetUrl)
        {
            Uri target;
            if (!IsValidInfo(info, progressStatus) || !IsValidAddress(senderMail)
                || !IsValidAddress(recipientMail)
                || !Uri.TryCreate(targetUrl, UriKind.Absolute, out target)
                || (target.Scheme != Uri.UriSchemeHttp && target.Scheme != Uri.UriSchemeHttps)
                || target.UserInfo.Length > 0)
                return null;

            string title = (info.Title ?? String.Empty).ToReverseXSS();
            string prefix = Alarm.GetStatusCompletionSubjectPrefix(info, progressStatus, Int32.MaxValue);
            string senderName = SanitizeSenderName((info.SenderName ?? String.Empty).ToReverseXSS());
            MailDataInfo message = new MailDataInfo();
            message.CharacterSet = "utf-8";
            message.MailMessageID = Guid.NewGuid().ToString("N") + "@"
                + senderMail.Substring(senderMail.LastIndexOf('@') + 1);
            message.From = senderName.Length == 0 ? senderMail : senderName + "<" + senderMail + ">";
            message.To.Add(recipientMail, recipientMail);
            message.Subject = prefix + NormalizeSubject(title);

            StringBuilder body = new StringBuilder();
            body.Append("<!DOCTYPE html><html><head><meta charset=\"utf-8\" /></head><body>");
            body.Append("<p>").Append(HttpUtility.HtmlEncode(prefix.TrimEnd())).Append("</p>");
            body.Append("<p>과정명: ").Append(HttpUtility.HtmlEncode(title)).Append("</p>");
            DateTime startDate, endDate;
            string[] dateFormats = new string[] { "yyyyMMdd", "yyyy-MM-dd" };
            if (DateTime.TryParseExact(info.StartDate, dateFormats, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out startDate)
                && DateTime.TryParseExact(info.EndDate, dateFormats, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out endDate))
            {
                body.Append("<p>기간: ").Append(startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))
                    .Append(" ~ ").Append(endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)).Append("</p>");
            }
            body.Append("<p>해당 과정을 완료해 주시기 바랍니다.</p><p><a href=\"")
                .Append(HttpUtility.HtmlAttributeEncode(target.AbsoluteUri))
                .Append("\" style=\"font-weight:bold !important;\"><b style=\"font-weight:bold !important;\">")
                .Append("교육 보기")
                .Append("</b></a></p></body></html>");
            message.Contents = body.ToString();
            return message;
        }

        private static string SanitizeSenderName(string name)
        {
            StringBuilder sanitized = new StringBuilder();
            foreach (char character in name)
            {
                // 공통 메일 파서에서 주소 구분자로 해석되는 문자도 표시명에서 제외합니다.
                if (!Char.IsControl(character) && character != '<' && character != '>'
                    && character != ',' && character != ';' && character != '"' && character != '\\')
                    sanitized.Append(character);
            }
            return sanitized.ToString().Trim();
        }

        private static string NormalizeSubject(string title)
        {
            StringBuilder normalized = new StringBuilder();
            foreach (char character in title)
                normalized.Append(Char.IsControl(character) ? ' ' : character);
            return normalized.ToString();
        }

        private static bool IsStagedMailFile(string filePath, string stagingDirectory)
        {
            if (String.IsNullOrEmpty(filePath)) return false;
            string fullPath = Path.GetFullPath(filePath);
            Guid fileID;
            return String.Equals(Path.GetDirectoryName(fullPath), stagingDirectory.TrimEnd('\\', '/'),
                    StringComparison.OrdinalIgnoreCase)
                && String.Equals(Path.GetExtension(fullPath), ".eml", StringComparison.OrdinalIgnoreCase)
                && Guid.TryParseExact(Path.GetFileNameWithoutExtension(fullPath), "N", out fileID);
        }

        private static void LogFailure(BE.ListInfo info, string batchID, string phase, int recipientID,
            Exception exception)
        {
            try
            {
                // 메일 주소, 본문, URL, 예외 원문에는 민감정보가 포함될 수 있어 기록하지 않습니다.
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(String.Format(
                    CultureInfo.InvariantCulture,
                    "Education.StatusMail CompanyID={0}, ListID={1}, BatchID={2}, Phase={3}, UserID={4}, ExceptionType={5}",
                    info == null ? 0 : info.CompanyID, info == null ? 0 : info.ListID, batchID, phase,
                    recipientID, exception == null ? "None" : exception.GetType().Name));
            }
            catch (Exception) { }
        }
    }
}
