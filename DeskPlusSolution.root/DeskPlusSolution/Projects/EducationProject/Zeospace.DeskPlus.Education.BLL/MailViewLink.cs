using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;

namespace Zeospace.DeskPlus.Education.BLL
{
    /// <summary>
    /// 메일 전용 읽기 권한의 회사, 교육, 수신자와 만료 시각입니다.
    /// </summary>
    public sealed class MailViewScope
    {
        private readonly int companyID;
        private readonly int listID;
        private readonly int userID;
        private readonly DateTime expiresUtc;

        internal MailViewScope(int companyID, int listID, int userID, DateTime expiresUtc)
        {
            this.companyID = companyID;
            this.listID = listID;
            this.userID = userID;
            this.expiresUtc = expiresUtc;
        }

        public int CompanyID { get { return companyID; } }
        public int ListID { get { return listID; } }
        public int UserID { get { return userID; } }
        public DateTime ExpiresUtc { get { return expiresUtc; } }
    }

    /// <summary>
    /// 설정의 URL 암호화 여부와 관계없이 변조 방지와 암호화를 강제하는 30일 읽기 전용 링크입니다.
    /// </summary>
    public static class MailViewLink
    {
        private const string Purpose = "EducationMailViewV1";
        private const int MaximumTokenLength = 2048;
        private const long ValidityTicks = 30L * TimeSpan.TicksPerDay;
        private const long FutureClockSkewTicks = 5L * TimeSpan.TicksPerMinute;

        public static string Create(int companyID, int listID, int userID)
        {
            return Create(companyID, listID, userID, DateTime.UtcNow);
        }

        public static string Create(int companyID, int listID, int userID, DateTime utcNow)
        {
            if (companyID <= 0) throw new ArgumentOutOfRangeException("companyID");
            if (listID <= 0) throw new ArgumentOutOfRangeException("listID");
            if (userID <= 0) throw new ArgumentOutOfRangeException("userID");
            if (utcNow.Kind != DateTimeKind.Utc)
                throw new ArgumentException("UTC time is required.", "utcNow");
            DateTime expiresUtc = utcNow.AddTicks(ValidityTicks);
            string payload = String.Format(CultureInfo.InvariantCulture, "{0}|{1}|{2}|{3}|{4}|{5}",
                Purpose, companyID, listID, userID, utcNow.Ticks, expiresUtc.Ticks);
            string token = MachineKey.Encode(Encoding.UTF8.GetBytes(payload), MachineKeyProtection.All);
            if (String.IsNullOrEmpty(token) || token.Length > MaximumTokenLength)
                throw new CryptographicException("Mail view link could not be protected.");
            return token;
        }

        public static bool TryRead(string token, out MailViewScope scope)
        {
            return TryRead(token, DateTime.UtcNow, out scope);
        }

        public static bool TryRead(string token, DateTime utcNow, out MailViewScope scope)
        {
            scope = null;
            if (utcNow.Kind != DateTimeKind.Utc || String.IsNullOrEmpty(token)
                || token.Length > MaximumTokenLength || (token.Length & 1) != 0)
                return false;
            foreach (char character in token)
                if (!((character >= '0' && character <= '9') || (character >= 'a' && character <= 'f')
                    || (character >= 'A' && character <= 'F')))
                    return false;

            try
            {
                byte[] decoded = MachineKey.Decode(token, MachineKeyProtection.All);
                if (decoded == null || decoded.Length == 0 || decoded.Length > 512) return false;
                string[] fields = new UTF8Encoding(false, true).GetString(decoded).Split('|');
                if (fields.Length != 6 || !String.Equals(fields[0], Purpose, StringComparison.Ordinal))
                    return false;
                int companyID, listID, userID;
                long issuedTicks, expiresTicks;
                if (!Int32.TryParse(fields[1], NumberStyles.None, CultureInfo.InvariantCulture, out companyID)
                    || !Int32.TryParse(fields[2], NumberStyles.None, CultureInfo.InvariantCulture, out listID)
                    || !Int32.TryParse(fields[3], NumberStyles.None, CultureInfo.InvariantCulture, out userID)
                    || companyID <= 0 || listID <= 0 || userID <= 0
                    || !Int64.TryParse(fields[4], NumberStyles.None, CultureInfo.InvariantCulture, out issuedTicks)
                    || !Int64.TryParse(fields[5], NumberStyles.None, CultureInfo.InvariantCulture, out expiresTicks)
                    || issuedTicks < DateTime.MinValue.Ticks || expiresTicks > DateTime.MaxValue.Ticks
                    || expiresTicks < issuedTicks || expiresTicks - issuedTicks != ValidityTicks)
                    return false;

                // 최대 5분의 서버 시계 차이는 허용하지만 만료 시각 자체에는 유예를 두지 않습니다.
                if ((issuedTicks > utcNow.Ticks && issuedTicks - utcNow.Ticks > FutureClockSkewTicks)
                    || utcNow.Ticks >= expiresTicks)
                    return false;
                scope = new MailViewScope(companyID, listID, userID, new DateTime(expiresTicks, DateTimeKind.Utc));
                return true;
            }
            catch (ArgumentException) { return false; }
            catch (CryptographicException) { return false; }
            catch (HttpException) { return false; }
        }
    }
}
