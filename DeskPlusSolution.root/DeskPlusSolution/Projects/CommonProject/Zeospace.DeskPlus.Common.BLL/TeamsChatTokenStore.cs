using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Zeospace.DeskPlus.Common.BLL
{
    // 서버의 Windows DPAPI와 연결 스크립트에서 설정하는 디렉터리 ACL을 함께 사용합니다.
    // 암호화 파일은 다른 서버로 복사하지 않고 해당 서버에서 다시 로그인해 생성합니다.
    public static class TeamsChatTokenStore
    {
        public static string CreateContext(string tenantId, string clientId, string senderUpn)
        {
            return "DeskPlus.TeamsChat.v1|" + Guid.Parse(tenantId).ToString("D") + "|"
                + Guid.Parse(clientId).ToString("D") + "|" + senderUpn.Trim().ToLowerInvariant();
        }

        public static FileStream AcquireLock(string path)
        {
            string fullPath = ValidatePath(path);
            DateTime deadline = DateTime.UtcNow.AddSeconds(30);
            while (true)
            {
                try { return new FileStream(fullPath + ".lock", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None); }
                catch (IOException)
                {
                    if (DateTime.UtcNow >= deadline)
                        throw new IOException("Teams 토큰 파일이 사용 중입니다. 잠시 후 다시 시도하세요.");
                    Thread.Sleep(100);
                }
            }
        }

        public static string Load(string path, string context)
        {
            string fullPath = ValidatePath(path);
            if (!File.Exists(fullPath))
                throw new InvalidOperationException("Teams 발신 계정 연결이 필요합니다. 서버에서 Tools\\Connect-TeamsChat.ps1을 실행하세요.");
            byte[] clear = ProtectedData.Unprotect(File.ReadAllBytes(fullPath), Encoding.UTF8.GetBytes(context), DataProtectionScope.LocalMachine);
            try
            {
                string token = Encoding.UTF8.GetString(clear);
                if (string.IsNullOrWhiteSpace(token)) throw new InvalidOperationException("Teams 갱신 토큰이 비어 있습니다. 다시 연결하세요.");
                return token;
            }
            finally { Array.Clear(clear, 0, clear.Length); }
        }

        // 호출자는 AcquireLock을 유지해야 합니다. 교체는 같은 디렉터리에서 원자적으로 수행합니다.
        public static void Save(string path, string context, string refreshToken)
        {
            string fullPath = ValidatePath(path);
            if (string.IsNullOrWhiteSpace(refreshToken)) throw new ArgumentException("갱신 토큰이 비어 있습니다.", "refreshToken");
            byte[] clear = Encoding.UTF8.GetBytes(refreshToken);
            byte[] encrypted;
            try { encrypted = ProtectedData.Protect(clear, Encoding.UTF8.GetBytes(context), DataProtectionScope.LocalMachine); }
            finally { Array.Clear(clear, 0, clear.Length); }
            string temporary = fullPath + "." + Guid.NewGuid().ToString("N") + ".tmp";
            try
            {
                using (var file = new FileStream(temporary, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                {
                    file.Write(encrypted, 0, encrypted.Length);
                    file.Flush(true);
                }
                if (File.Exists(fullPath)) File.Replace(temporary, fullPath, null);
                else File.Move(temporary, fullPath);
            }
            finally { if (File.Exists(temporary)) File.Delete(temporary); }
        }

        private static string ValidatePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !Path.IsPathRooted(path))
                throw new ArgumentException("TeamsChatTokenPath에는 절대 경로가 필요합니다.", "path");
            string fullPath = Path.GetFullPath(path);
            if (!Directory.Exists(Path.GetDirectoryName(fullPath)))
                throw new InvalidOperationException("Teams 토큰 디렉터리가 없습니다. 서버에서 Tools\\Connect-TeamsChat.ps1을 실행하세요.");
            return fullPath;
        }
    }
}
