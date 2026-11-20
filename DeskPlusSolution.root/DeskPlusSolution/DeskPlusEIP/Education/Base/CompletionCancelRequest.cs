using System;
using System.Globalization;
using System.Web;

namespace Zeospace.DeskPlus.Education.Web.Base
{
    internal static class CompletionCancelRequest
    {
        internal static void Validate(HttpRequest request)
        {
            if (request == null || !String.Equals(request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase))
                throw new HttpException(405, "POST 요청만 사용할 수 있습니다.");
            string contentType = (request.ContentType ?? "").Split(';')[0].Trim();
            if (!String.Equals(contentType, "application/json", StringComparison.OrdinalIgnoreCase))
                throw new HttpException(415, "JSON 요청만 사용할 수 있습니다.");

            string origin = request.Headers["Origin"];
            Uri source;
            if (!String.IsNullOrEmpty(origin))
            {
                if (!Uri.TryCreate(origin, UriKind.Absolute, out source)
                    || source.AbsolutePath != "/" || source.Query != "" || source.Fragment != ""
                    || !SameOrigin(request.Url, source))
                    throw new HttpException(403, "동일한 사이트에서 요청해 주세요.");
            }
            else
            {
                source = request.UrlReferrer;
                if (!SameOrigin(request.Url, source))
                    throw new HttpException(403, "동일한 사이트에서 요청해 주세요.");
            }
        }

        private static bool SameOrigin(Uri target, Uri source)
        {
            return target != null && source != null && source.IsAbsoluteUri && source.UserInfo == ""
                && (source.Scheme == Uri.UriSchemeHttp || source.Scheme == Uri.UriSchemeHttps)
                && String.Equals(target.Scheme, source.Scheme, StringComparison.OrdinalIgnoreCase)
                && String.Equals(target.Host, source.Host, StringComparison.OrdinalIgnoreCase)
                && target.Port == source.Port;
        }

        internal static int ReadPositiveID(string value, string message)
        {
            int id;
            if (!Int32.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out id) || id <= 0)
                throw new HttpException(400, message);
            return id;
        }
    }
}
