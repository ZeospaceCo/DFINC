using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace Zeospace.DeskPlus.Common.BLL
{
    public class TeamsMSG
    {
        private const string GraphBaseUrl = "https://graph.microsoft.com/v1.0";
        internal const string ChatScopes = "https://graph.microsoft.com/User.Read https://graph.microsoft.com/User.ReadBasic.All https://graph.microsoft.com/Chat.Create https://graph.microsoft.com/ChatMessage.Send offline_access";
        private const int RequestTimeoutMilliseconds = 30000;
        private static readonly object TokenSync = new object();
        private static string cachedToken;
        private static string cachedTokenKey;
        private static DateTime cachedTokenExpiresUtc;

        internal static bool IsEnabled
        {
            get
            {
                string value = ConfigurationManager.AppSettings["TeamsChatEnabled"]
                    ?? ConfigurationManager.AppSettings["TeamsActivityEnabled"];
                return !string.Equals(value, "false", StringComparison.OrdinalIgnoreCase);
            }
        }

        // 기존 호출부와의 호환성을 유지하며 전송 방식은 1:1 채팅으로 통일합니다.
        public static bool SendTeamsAppAlarm(string userEmail, string title, string content, string targetUrl)
        {
            return SendChatMessage(userEmail, title, content, targetUrl);
        }

        public static bool SendChatMessage(string userEmail, string title, string content, string targetUrl)
        {
            if (!IsEnabled)
                return false;

            TeamsSettings settings = null;
            try
            {
                settings = new TeamsSettings
                {
                    TenantId = GetRequiredSetting("TeamsTenantId"),
                    ClientId = GetRequiredSetting("TeamsClientId"),
                    SenderUserPrincipalName = GetRequiredSetting("TeamsSenderUserPrincipalName"),
                    TokenFilePath = Environment.ExpandEnvironmentVariables(GetRequiredSetting("TeamsChatTokenPath"))
                };
                SendOneToOneChat(userEmail, title, content, targetUrl, settings, Request,
                    () => GetAccessToken(settings, Request, DateTime.UtcNow));
                TeamsChatReauthNotifier.Reset(settings.TokenFilePath);
                return true;
            }
            catch (Exception ex)
            {
                var apiError = ex as MicrosoftApiException;
                if (apiError != null && apiError.StatusCode == 401)
                {
                    lock (TokenSync) { cachedToken = null; }
                }
                var reauth = ex as ReauthenticationRequiredException;
                if (settings != null && reauth != null)
                    TeamsChatReauthNotifier.TryNotify(settings, reauth.ErrorCode);
                // 기존 설정에 남아 있는 앱 비밀키도 로그에서 제거합니다.
                string message = ex.Message;
                string secret = ConfigurationManager.AppSettings["TeamsClientSecret"];
                if (!string.IsNullOrEmpty(secret))
                    message = message.Replace(secret, "[REDACTED]");
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Teams 1:1 채팅 발송 실패: " + message);
                return false;
            }
        }

        internal sealed class ReauthenticationRequiredException : InvalidOperationException
        {
            internal string ErrorCode { get; private set; }
            internal ReauthenticationRequiredException(string errorCode)
                : base("Microsoft 재로그인이 필요합니다. 서버에서 Tools\\Connect-TeamsChat.ps1을 다시 실행하세요. 오류 코드: " + errorCode)
            {
                ErrorCode = errorCode;
            }
        }

        internal sealed class TeamsSettings
        {
            public string TenantId { get; set; }
            public string ClientId { get; set; }
            public string SenderUserPrincipalName { get; set; }
            public string TokenFilePath { get; set; }
        }

        // method, URL, content-type, body, bearer token을 분리하여 외부 발송 없이 검증합니다.
        internal static void SendOneToOneChat(string userEmail, string title, string content, string targetUrl,
            TeamsSettings settings, Func<string, string, string, string, string, string> request, Func<string> acquireToken)
        {
            string recipient = NormalizeRecipient(userEmail);
            string message = CreateMessagePayload(title, content, targetUrl);
            string expectedSender = NormalizeRecipient(settings.SenderUserPrincipalName);
            string accessToken = acquireToken();
            if (string.IsNullOrWhiteSpace(accessToken))
                throw new InvalidOperationException("Microsoft 사용자 로그인 토큰이 없습니다.");

            var me = ReadObject(request("GET", GraphBaseUrl + "/me?$select=id,userPrincipalName", null, null, accessToken));
            if (!string.Equals(RequiredString(me, "userPrincipalName"), expectedSender, StringComparison.OrdinalIgnoreCase))
                throw new ReauthenticationRequiredException("sender_mismatch");
            string senderId = RequiredGuid(me, "id");

            Dictionary<string, object> user;
            Guid recipientId;
            if (Guid.TryParse(recipient, out recipientId))
                user = ReadObject(request("GET", GraphBaseUrl + "/users/" + recipientId.ToString("D") + "?$select=id", null, null, accessToken));
            else
            {
                // DB 메일 주소와 Entra 로그인 UPN이 서로 다른 경우도 mail 속성으로 조회합니다.
                string literal = recipient.Replace("'", "''");
                string filter = "userPrincipalName eq '" + literal + "' or mail eq '" + literal + "'";
                var result = ReadObject(request("GET", GraphBaseUrl + "/users?$filter=" + Uri.EscapeDataString(filter)
                    + "&$select=id&$top=2", null, null, accessToken));
                object values;
                var users = result.TryGetValue("value", out values) ? values as IList : null;
                if (users == null || users.Count != 1)
                    throw new InvalidOperationException("DB 수신자 이메일과 일치하는 Entra 사용자가 정확히 한 명이어야 합니다.");
                user = users[0] as Dictionary<string, object>;
            }
            string receiverId = RequiredGuid(user, "id");
            if (string.Equals(senderId, receiverId, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("발신 계정 자신에게는 1:1 채팅을 생성할 수 없습니다.");

            var serializer = new JavaScriptSerializer();
            string chatBody = serializer.Serialize(new
            {
                chatType = "oneOnOne",
                members = new[] { ChatMember(senderId), ChatMember(receiverId) }
            });
            // 같은 두 사용자의 채팅이 존재하면 Graph가 기존 채팅을 반환합니다.
            string chatId = RequiredString(ReadObject(request("POST", GraphBaseUrl + "/chats", "application/json; charset=utf-8", chatBody, accessToken)), "id");
            // 발송 요청은 자동 재시도하지 않아 응답 유실 시 중복 메시지를 방지합니다.
            request("POST", GraphBaseUrl + "/chats/" + Uri.EscapeDataString(chatId) + "/messages", "application/json; charset=utf-8", message, accessToken);
        }

        private static Dictionary<string, object> ChatMember(string id)
        {
            return new Dictionary<string, object>
            {
                { "@odata.type", "#microsoft.graph.aadUserConversationMember" },
                { "roles", new[] { "owner" } },
                { "user@odata.bind", GraphBaseUrl + "/users('" + id + "')" }
            };
        }

        internal static string GetAccessToken(TeamsSettings settings,
            Func<string, string, string, string, string, string> request, DateTime nowUtc)
        {
            string context = TeamsChatTokenStore.CreateContext(settings.TenantId, settings.ClientId, settings.SenderUserPrincipalName);
            string key = context + "|" + Path.GetFullPath(settings.TokenFilePath);
            lock (TokenSync)
            {
                // 파일 삭제 시 메모리 토큰으로 계속 발송하지 않고 재연결을 안내합니다.
                if (!File.Exists(settings.TokenFilePath))
                {
                    cachedToken = null;
                    throw new ReauthenticationRequiredException("refresh_token_missing");
                }
                if (cachedToken != null && cachedTokenKey == key && nowUtc < cachedTokenExpiresUtc)
                    return cachedToken;
                using (TeamsChatTokenStore.AcquireLock(settings.TokenFilePath))
                {
                    string refreshToken;
                    try { refreshToken = TeamsChatTokenStore.Load(settings.TokenFilePath, context); }
                    catch (FileNotFoundException)
                    {
                        throw new ReauthenticationRequiredException("refresh_token_missing");
                    }
                    catch (DirectoryNotFoundException)
                    {
                        throw new ReauthenticationRequiredException("refresh_token_missing");
                    }
                    catch (CryptographicException)
                    {
                        throw new ReauthenticationRequiredException("refresh_token_invalid");
                    }
                    catch (InvalidOperationException)
                    {
                        throw new ReauthenticationRequiredException("refresh_token_unavailable");
                    }
                    string form = "client_id=" + Uri.EscapeDataString(settings.ClientId)
                        + "&scope=" + Uri.EscapeDataString(ChatScopes)
                        + "&refresh_token=" + Uri.EscapeDataString(refreshToken)
                        + "&grant_type=refresh_token";
                    Dictionary<string, object> response;
                    try
                    {
                        response = ReadObject(request("POST", "https://login.microsoftonline.com/" + Uri.EscapeDataString(settings.TenantId)
                            + "/oauth2/v2.0/token", "application/x-www-form-urlencoded; charset=utf-8", form, null));
                    }
                    catch (MicrosoftApiException ex)
                    {
                        if (ex.ErrorCode == "invalid_grant" || ex.ErrorCode == "interaction_required" || ex.ErrorCode == "consent_required")
                            throw new ReauthenticationRequiredException(ex.ErrorCode);
                        throw;
                    }
                    string token = RequiredString(response, "access_token");
                    ValidateScopes(RequiredString(response, "scope"));
                    if (!string.Equals(RequiredString(response, "token_type"), "Bearer", StringComparison.OrdinalIgnoreCase))
                        throw new InvalidOperationException("Microsoft 응답의 token_type이 Bearer가 아닙니다.");
                    object rotatedToken;
                    if (response.TryGetValue("refresh_token", out rotatedToken) && rotatedToken is string && !string.IsNullOrWhiteSpace((string)rotatedToken))
                        TeamsChatTokenStore.Save(settings.TokenFilePath, context, (string)rotatedToken);
                    object lifetime;
                    int seconds;
                    if (!response.TryGetValue("expires_in", out lifetime) || !int.TryParse(Convert.ToString(lifetime), out seconds) || seconds <= 0)
                        throw new InvalidOperationException("Microsoft 응답의 토큰 만료 시간이 올바르지 않습니다.");
                    cachedToken = token;
                    cachedTokenKey = key;
                    cachedTokenExpiresUtc = nowUtc.AddSeconds(Math.Max(0, Math.Min(seconds, 3600) - 120));
                    return token;
                }
            }
        }

        internal static void ValidateScopes(string value)
        {
            const string prefix = "https://graph.microsoft.com/";
            var scopes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (string item in (value ?? string.Empty).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                scopes.Add(item.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) ? item.Substring(prefix.Length) : item);
            foreach (string required in new[] { "User.Read", "User.ReadBasic.All", "Chat.Create", "ChatMessage.Send" })
                if (!scopes.Contains(required))
                    throw new ReauthenticationRequiredException("missing_scope_" + required);
        }

        private static string CreateMessagePayload(string title, string content, string targetUrl)
        {
            Uri documentUri;
            if (!Uri.TryCreate(targetUrl, UriKind.Absolute, out documentUri)
                || (documentUri.Scheme != Uri.UriSchemeHttps && documentUri.Scheme != Uri.UriSchemeHttp)
                || !string.IsNullOrEmpty(documentUri.UserInfo))
                throw new ArgumentException("Teams 채팅의 문서 주소는 유효한 HTTP/HTTPS 주소여야 합니다.", "targetUrl");
            string subject = ToPlainText(title);
            if (string.IsNullOrWhiteSpace(subject)) subject = "그룹웨어";
            string html = "<b>" + WebUtility.HtmlEncode(subject) + "</b><br/>"
                + WebUtility.HtmlEncode(ToPlainText(content)).Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "<br/>")
                + "<br/><a href=\"" + WebUtility.HtmlEncode(documentUri.AbsoluteUri) + "\">문서 확인</a>";
            return new JavaScriptSerializer().Serialize(new { body = new { contentType = "html", content = html } });
        }

        private static string ToPlainText(string value)
        {
            string text = Regex.Replace(value ?? string.Empty, @"<(?:br\s*/?|/p|/div)\s*>", "\n", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(1));
            text = Regex.Replace(text, "<[^>]*>", " ", RegexOptions.None, TimeSpan.FromSeconds(1));
            return WebUtility.HtmlDecode(text).Trim();
        }

        private static string GetRequiredSetting(string key)
        {
            string value = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(value))
                throw new ConfigurationErrorsException("WebAppSettings.config의 " + key + " 설정이 필요합니다.");
            return value.Trim();
        }

        private static string NormalizeRecipient(string userEmail)
        {
            string recipient = (userEmail ?? string.Empty).Trim();
            Guid userId;
            if (Guid.TryParse(recipient, out userId)) return userId.ToString("D");
            try
            {
                var address = new MailAddress(recipient);
                if (string.Equals(address.Address, recipient, StringComparison.OrdinalIgnoreCase) && recipient.IndexOf('@') > 0)
                    return recipient;
            }
            catch (FormatException) { }
            catch (ArgumentException) { }
            throw new ArgumentException("Teams 수신자에는 사용자의 Entra ID 또는 이메일 주소가 필요합니다.", "userEmail");
        }

        private static Dictionary<string, object> ReadObject(string json)
        {
            var data = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(json);
            if (data == null) throw new InvalidOperationException("Microsoft API 응답이 비어 있습니다.");
            return data;
        }

        private static string RequiredString(Dictionary<string, object> data, string property)
        {
            object value;
            if (data == null || !data.TryGetValue(property, out value) || !(value is string) || string.IsNullOrWhiteSpace((string)value))
                throw new InvalidOperationException("Microsoft 응답에 " + property + " 값이 없습니다.");
            return (string)value;
        }

        private static string RequiredGuid(Dictionary<string, object> data, string property)
        {
            Guid value;
            if (!Guid.TryParse(RequiredString(data, property), out value))
                throw new InvalidOperationException("Microsoft 응답의 사용자 ID가 올바르지 않습니다.");
            return value.ToString("D");
        }

        internal sealed class MicrosoftApiException : InvalidOperationException
        {
            public int StatusCode { get; private set; }
            public string ErrorCode { get; private set; }
            public MicrosoftApiException(int statusCode, string code, string message, string requestId)
                : base("Microsoft API HTTP " + statusCode + ": " + code + ": " + message
                    + (string.IsNullOrEmpty(requestId) ? string.Empty : " (request-id: " + requestId + ")"))
            { StatusCode = statusCode; ErrorCode = code; }
        }

        private static string Request(string method, string url, string contentType, string body, string accessToken)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method;
            if (!string.IsNullOrEmpty(contentType)) request.ContentType = contentType;
            request.Accept = "application/json";
            request.Timeout = RequestTimeoutMilliseconds;
            request.ReadWriteTimeout = RequestTimeoutMilliseconds;
            request.AllowAutoRedirect = false;
            IWebProxy proxy = WebRequest.DefaultWebProxy;
            if (proxy != null) request.Proxy = new RequestProxy(proxy);
            if (!string.IsNullOrEmpty(accessToken)) request.Headers[HttpRequestHeader.Authorization] = "Bearer " + accessToken;
            try
            {
                if (body != null)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(body);
                    request.ContentLength = bytes.Length;
                    using (Stream stream = request.GetRequestStream()) stream.Write(bytes, 0, bytes.Length);
                }
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    int statusCode = (int)response.StatusCode;
                    if (statusCode < 200 || statusCode >= 300)
                        throw new InvalidOperationException("Microsoft API가 예상하지 못한 HTTP 상태를 반환했습니다: " + statusCode);
                    return ReadResponse(response);
                }
            }
            catch (WebException ex)
            {
                using (var response = ex.Response as HttpWebResponse)
                {
                    if (response == null)
                        throw new InvalidOperationException("Microsoft API 통신 실패 (" + ex.Status + "). TLS 인증서, 프록시 및 네트워크 연결을 확인하세요.");
                    string code = "UnknownError";
                    string message = "응답 오류 코드를 확인할 수 없습니다.";
                    try
                    {
                        var data = ReadObject(ReadResponse(response));
                        object error;
                        if (data.TryGetValue("error", out error))
                        {
                            var details = error as Dictionary<string, object>;
                            if (details != null) { code = RequiredString(details, "code"); message = RequiredString(details, "message"); }
                            else { code = Convert.ToString(error); message = RequiredString(data, "error_description"); }
                        }
                    }
                    catch (ArgumentException) { }
                    catch (InvalidOperationException) { }
                    throw new MicrosoftApiException((int)response.StatusCode, code, message,
                        response.Headers["request-id"] ?? response.Headers["x-ms-request-id"]);
                }
            }
        }

        private static string ReadResponse(HttpWebResponse response)
        {
            using (Stream stream = response.GetResponseStream())
            {
                if (stream == null) return string.Empty;
                using (var reader = new StreamReader(stream, Encoding.UTF8)) return reader.ReadToEnd();
            }
        }

        private sealed class RequestProxy : IWebProxy
        {
            private readonly IWebProxy proxy;
            public RequestProxy(IWebProxy proxy)
            {
                this.proxy = proxy;
                Credentials = proxy.Credentials ?? CredentialCache.DefaultNetworkCredentials;
            }
            public ICredentials Credentials { get; set; }
            public Uri GetProxy(Uri destination) { return proxy.GetProxy(destination); }
            public bool IsBypassed(Uri host) { return proxy.IsBypassed(host); }
        }
    }
}
