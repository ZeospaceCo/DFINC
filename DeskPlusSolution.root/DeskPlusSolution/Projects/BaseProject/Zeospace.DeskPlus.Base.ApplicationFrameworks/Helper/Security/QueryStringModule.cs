using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security
{
    /// <summary>
    /// 쿼리 암복고화 모듈 - written by shimpark dated 2007-10-04
    /// </summary>
    public class QueryStringModule
    {
        #region IHttpModule Members

        /// <summary>
        /// 	<see cref="T:System.Web.IHttpModule"></see>을 구현하는 모듈에서 사용하는 리소스(메모리 제외)를 삭제합니다.
        /// </summary>
        public void Dispose()
        {
            // Nothing to dispose
        }

        #endregion IHttpModule Members

        private const string PARAMETER_NAME = "";
        private const string ENCRYPTION_KEY = "key";

        /// <summary>
        /// URL 파싱 및 쿼리문자열 없이 가상경로 추출하기
        /// </summary>
        /// <returns>The virtual path of the current URL.</returns>
        private static string GetVirtualPath()
        {
            string path = HttpContext.Current.Request.RawUrl;
            path = path.Substring(0, path.IndexOf("?"));
            path = path.Substring(path.LastIndexOf("/") + 1);
            return path;
        }

        /// <summary>
        /// url 파싱 및 쿼리 문자 반환하기
        /// </summary>
        /// <param name="url">The URL to parse.</param>
        /// <returns>The query string without the question mark.</returns>
        private static string ExtractQuery(string url)
        {
            int index = url.IndexOf("?") + 1;
            return url.Substring(index);
        }

        #region Encryption/decryption

        /// <summary>
        /// encryption 강화하기 위해 사용할 salt 값
        /// </summary>
        private readonly static byte[] SALT = Encoding.ASCII.GetBytes(ENCRYPTION_KEY.Length.ToString());

        /// <summary>
        /// Rijndael algorithm 사용하여 암호화
        /// </summary>
        /// <param name="inputText">The string to encrypt.</param>
        /// <returns>A Base64 encrypted string.</returns>
        public static string Encrypt(string inputText, bool isUrlEncode)
        {
            string rtnVal = "";

            //Rijndael 알고리즘의 관리되는 버전에 액세스합니다
            RijndaelManaged rijndaelCipher = new RijndaelManaged();

            // UTF-16 형식에 대한 인코딩을 가져옵니다
            byte[] plainText = Encoding.Unicode.GetBytes(inputText);

            //PBKDF1 알고리즘의 확장을 사용하여 암호에서 키를 파생시킵니다. (키를 파생시킬 암호, 키를 파생시키는 데 사용할 키 솔트)
            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(ENCRYPTION_KEY, SALT);

            //지정한 Key와 초기화 벡터(IV)를 사용하여 대칭 Rijndael encryptor 개체를 만듭니다.
            using (ICryptoTransform encryptor = rijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16)))
            {
                //메모리에 stream 생성
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    //대상 데이터 스트림, 사용할 변환 및 스트림 모드를 사용하여 CryptoStream 클래스의 새 인스턴스를 초기화합니다
                    //암호화 변형을 수행할 스트림, 스트림에 대해 수행될 암호화 변형,CryptoStreamMode 값 중 암호화 스트림의 액세스를 씁니다.
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        //현재 CryptoStream에 바이트의 시퀀스를 쓰고 이 스트림 내의 현재 위치를 쓰여진 바이트 수만큼 앞으로 이동합니다.
                        cryptoStream.Write(plainText, 0, plainText.Length);

                        //내부 데이터 소스 또는 리포지토리에 버퍼의 현재 상태를 업데이트한 다음 해당 버퍼를 지웁니다.
                        cryptoStream.FlushFinalBlock();

                        //8비트 부호 없는 정수로 구성된 배열의 값을 64자리 기수로 인코딩된 String 표현으로 변환합니다.
                        rtnVal = PARAMETER_NAME + Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
            }

            return (isUrlEncode) ? HttpUtility.UrlEncode(rtnVal, System.Text.Encoding.GetEncoding("utf-8")) : rtnVal;
        }

        private string GetRijndaelManaged(string _inputText)
        {
            string rtnVal = "";
            //Rijndael 알고리즘의 관리되는 버전에 액세스합니다
            RijndaelManaged rijndaelCipher = new RijndaelManaged();

            // UTF-16 형식에 대한 인코딩을 가져옵니다
            byte[] plainText = Encoding.Unicode.GetBytes(_inputText);

            //PBKDF1 알고리즘의 확장을 사용하여 암호에서 키를 파생시킵니다. (키를 파생시킬 암호, 키를 파생시키는 데 사용할 키 솔트)
            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(ENCRYPTION_KEY, SALT);

            //지정한 Key와 초기화 벡터(IV)를 사용하여 대칭 Rijndael encryptor 개체를 만듭니다.
            using (ICryptoTransform encryptor = rijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16)))
            {
                //메모리에 stream 생성
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    //대상 데이터 스트림, 사용할 변환 및 스트림 모드를 사용하여 CryptoStream 클래스의 새 인스턴스를 초기화합니다
                    //암호화 변형을 수행할 스트림, 스트림에 대해 수행될 암호화 변형,CryptoStreamMode 값 중 암호화 스트림의 액세스를 씁니다.
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        //현재 CryptoStream에 바이트의 시퀀스를 쓰고 이 스트림 내의 현재 위치를 쓰여진 바이트 수만큼 앞으로 이동합니다.
                        cryptoStream.Write(plainText, 0, plainText.Length);

                        //내부 데이터 소스 또는 리포지토리에 버퍼의 현재 상태를 업데이트한 다음 해당 버퍼를 지웁니다.
                        cryptoStream.FlushFinalBlock();

                        //8비트 부호 없는 정수로 구성된 배열의 값을 64자리 기수로 인코딩된 String 표현으로 변환합니다.
                        rtnVal = PARAMETER_NAME + Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// Rijndael algorithm 이용하여 복고화 - written by shimpark
        /// </summary>
        /// <param name="inputText">The encrypted string to decrypt.</param>
        /// <returns>A decrypted string.</returns>
        public static string Decrypt(string inputText)
        {
            //inputText = HttpUtility.UrlDecode(inputText, System.Text.Encoding.GetEncoding("euc-kr"));

            //Rijndael 알고리즘의 관리되는 버전에 액세스합니다
            RijndaelManaged rijndaelCipher = new RijndaelManaged();

            //64자리 기수의 이진 데이터를 동등한 8비트 부호 없는 정수 배열로 인코딩하는 방법으로 지정된 String을 변환합니다
            byte[] encryptedData = Convert.FromBase64String(inputText);

            //PBKDF1 알고리즘의 확장을 사용하여 암호에서 키를 파생시킵니다. (키를 파생시킬 암호, 키를 파생시키는 데 사용할 키 솔트)
            PasswordDeriveBytes secretKey = new PasswordDeriveBytes(ENCRYPTION_KEY, SALT);

            //지정한 Key와 초기화 벡터(IV)를 사용하여 대칭 Rijndael decryptor 개체를 만듭니다. (대칭 알고리즘에 사용할 비밀 키, 대칭 알고리즘에 사용될 IV)
            using (ICryptoTransform decryptor = rijndaelCipher.CreateDecryptor(secretKey.GetBytes(32), secretKey.GetBytes(16)))
            {
                //메모리에 stream 생성
                using (MemoryStream memoryStream = new MemoryStream(encryptedData))
                {
                    //대상 데이터 스트림, 사용할 변환 및 스트림 모드를 사용하여 CryptoStream 클래스의 새 인스턴스를 초기화합니다
                    //암호화 변형을 수행할 스트림, 스트림에 대해 수행될 암호화 변형,CryptoStreamMode 값 중 암호화 스트림의 액세스를 읽습니다
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        byte[] plainText = new byte[encryptedData.Length];

                        //현재 CryptoStream에서 바이트의 시퀀스를 읽은 다음 읽은 바이트 수만큼 스트림 내에서 위치를 앞으로 이동합니다.
                        int decryptedCount = cryptoStream.Read(plainText, 0, plainText.Length);

                        // UTF-16 형식에 대한 인코딩을 가져옵니다
                        return Encoding.Unicode.GetString(plainText, 0, decryptedCount);
                    }
                }
            }
        }

        #endregion Encryption/decryption
    }
}