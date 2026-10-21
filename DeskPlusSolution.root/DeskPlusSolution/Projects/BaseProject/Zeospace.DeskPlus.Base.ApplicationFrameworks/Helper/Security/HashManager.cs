using System;
using System.Security.Cryptography;
using System.Text;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security
{
    /// <summary>
    /// C : MD5를 사용하여 암호화, Decode 불가
    /// </summary>
    public class HashManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordProtector"/> class.
        /// </summary>
        public HashManager()
        {
        }

        /// <summary>
        /// 패스워드 Hash
        /// </summary>
        /// <param name="_plainText">The _plain text.</param>
        /// <returns></returns>
        public static string GetHash(string _plainText)
        {
            string hashType = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "EncPasswordType", "SHA2");

            if (hashType == "SHA5")
            {
                return GetSHA512Hash(_plainText);
            }
            if (hashType == "SHA2")
            {
                return GetSHA256Hash(_plainText);
            }
            if (hashType == "SHA1")
            {
                return GetSHA1Hash(_plainText);
            }
            else
            {
                return GetMd5Hash(_plainText);
            }
        }

        /// <summary>
        /// MD5 Hash 만들기
        /// </summary>
        /// <param name="_input"></param>
        /// <returns></returns>
        public static string GetMd5Hash(string _plainText)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(_plainText));

            StringBuilder sBuilder = new StringBuilder();

            // 16 진수 만들기
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        /// <summary>
        /// SHA-1 Hash 함수
        /// SHA -> Base64Encording
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetSHA1Hash(string _plainText)
        {
            SHA1 sha1Hasher = SHA1.Create();
            byte[] data = sha1Hasher.ComputeHash(Encoding.UTF8.GetBytes(_plainText));

            StringBuilder sBuilder = new StringBuilder();

            // 16 진수 만들기
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
            //using (HashAlgorithm sha = new SHA1Managed())
            //{
            //    byte[] encryptedBytes = sha.TransformFinalBlock(data, 0, data.Length);

            //    return Convert.ToBase64String(sha.Hash);
            //}
        }

        /// <summary>
        /// SHA-2 Hash 함수
        /// SHA -> Base64Encording
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetSHA256Hash(string _plainText)
        {
            SHA256 sha256Hasher = SHA256.Create();
            byte[] data = sha256Hasher.ComputeHash(Encoding.UTF8.GetBytes(_plainText));

            StringBuilder sBuilder = new StringBuilder();

            // 16 진수 만들기
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();

            //byte[] data = Encoding.UTF8.GetBytes(_plainText);
            //using (HashAlgorithm sha = new SHA256Managed())
            //{
            //    byte[] encryptedBytes = sha.TransformFinalBlock(data, 0, data.Length);

            //    return Convert.ToBase64String(sha.Hash);
            //}
        }

        /// <summary>
        /// SHA-2 Hash 함수
        /// SHA -> Base64Encording
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetSHA512Hash(string _plainText)
        {
            SHA512 sha512Hasher = SHA512.Create();
            byte[] data = sha512Hasher.ComputeHash(Encoding.UTF8.GetBytes(_plainText));

            StringBuilder sBuilder = new StringBuilder();

            // 16 진수 만들기
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();

            //byte[] data = Encoding.UTF8.GetBytes(_plainText);
            //using (HashAlgorithm sha = new SHA256Managed())
            //{
            //    byte[] encryptedBytes = sha.TransformFinalBlock(data, 0, data.Length);

            //    return Convert.ToBase64String(sha.Hash);
            //}
        }

        /// <summary>
        /// MD5 Hash 함수 비교
        /// </summary>
        /// <param name="_x"></param>
        /// <param name="_y"></param>
        /// <returns></returns>
        public bool CompareMD5Hash(string _x, string _y)
        {
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(_x, _y))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}