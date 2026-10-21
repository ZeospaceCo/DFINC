using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security
{
    public class TripleDES
    {
        private byte[] tDESKey = null;
        private byte[] tDESIV = null;
        private const string sEncryptionKey = "1!a^5$x#"; // 꼭 8자리 값이어야 합니다.!!

        public TripleDES(byte[] tDESKey, byte[] tDESIV)
        {
            this.tDESKey = tDESKey;
            this.tDESIV = tDESIV;
        }

        /// <summary>
        /// Default Encoding을 사용하여 문자열을 암호화후 Base64 스트링으로 변환
        /// </summary>
        /// <param name="s">암호화할 문자열</param>
        /// <returns>암호화된 문자열</returns>
        public string TDESEncryptString(string s)
        {
            try
            {
                byte[] dataToEncrypt = Encoding.Default.GetBytes(s);
                // 암호화된 배열은 항상 8의 배수이며 아래 공식으로 암호화된 byte배열의 길이를 정확하게 계산 가능
                byte[] encryptedData = new byte[(dataToEncrypt.Length / 8 + 1) * 8];

                MemoryStream pmem = new MemoryStream(encryptedData);
                TripleDESCryptoServiceProvider pTDES = new TripleDESCryptoServiceProvider();

                pTDES.Mode = CipherMode.CBC;
                pTDES.Padding = PaddingMode.PKCS7;

                CryptoStream encStream = new CryptoStream(pmem, pTDES.CreateEncryptor(tDESKey, tDESIV), CryptoStreamMode.Write);
                encStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                encStream.Close();
                pmem.Close();
                return System.Convert.ToBase64String(encryptedData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 기본 인코딩을 사용하여 문자열을 복호화 (대상 문자열은 TripleDES 암호화후 Base64 변환된 문자열)
        /// </summary>
        /// <param name="s">복호화할 문자열</param>
        /// <returns>복호화된 문자열</returns>
        public string TDESDecryptString(string s)
        {
            try
            {
                byte[] dataToDecrypt = System.Convert.FromBase64String(s);
                byte[] decryptedData = new byte[256];

                MemoryStream pmem = new MemoryStream(decryptedData);
                TripleDESCryptoServiceProvider pTDES = new TripleDESCryptoServiceProvider();

                pTDES.Mode = CipherMode.CBC;
                pTDES.Padding = PaddingMode.PKCS7;

                CryptoStream decStream = new CryptoStream(pmem, pTDES.CreateDecryptor(tDESKey, tDESIV), CryptoStreamMode.Write);
                decStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);
                decStream.Close();
                Encoding enc = Encoding.Default;
                return enc.GetString(decryptedData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 작성자   : 이현수
        /// 작성목적 : 주어진 인코딩을 사용하여 문자열을 암호화후 Base64 스트링으로 변환
        /// 작성일자 : 2009-08-10
        /// </summary>
        /// <param name="s">암호화할 문자열</param>
        /// <param name="encoding">인코딩방식</param>
        /// <returns>암호화된 문자열</returns>
        public string TDESEncryptString(string s, Encoding encoding)
        {
            try
            {
                byte[] dataToEncrypt = encoding.GetBytes(s);
                // 암호화된 배열은 항상 8의 배수이며 아래 공식으로 암호화된 byte배열의 길이를 정확하게 계산 가능
                byte[] encryptedData = new byte[(dataToEncrypt.Length / 8 + 1) * 8];

                MemoryStream pmem = new MemoryStream(encryptedData);
                TripleDESCryptoServiceProvider pTDES = new TripleDESCryptoServiceProvider();

                pTDES.Mode = CipherMode.CBC;
                pTDES.Padding = PaddingMode.PKCS7;

                CryptoStream encStream = new CryptoStream(pmem, pTDES.CreateEncryptor(tDESKey, tDESIV), CryptoStreamMode.Write);
                encStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                encStream.Close();
                pmem.Close();
                return System.Convert.ToBase64String(encryptedData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 작성자   : 이현수
        /// 작성목적 :주어진 인코딩을 사용하여 문자열을 복호화 (대상 문자열은 TripleDES 암호화후 Base64 변환된 문자열)
        /// 작성일자 : 2009-08-10        ///
        /// </summary>
        /// <param name="s">복호화할 문자열</param>
        /// <param name="encoding">인코딩방식</param>
        /// <returns>복호화된 문자열</returns>
        public string TDESDecryptString(string s, Encoding encoding)
        {
            try
            {
                byte[] dataToDecrypt = System.Convert.FromBase64String(s);
                byte[] decryptedData = new byte[256];

                MemoryStream pmem = new MemoryStream(decryptedData);
                TripleDESCryptoServiceProvider pTDES = new TripleDESCryptoServiceProvider();

                pTDES.Mode = CipherMode.CBC;
                pTDES.Padding = PaddingMode.PKCS7;

                CryptoStream decStream = new CryptoStream(pmem, pTDES.CreateDecryptor(tDESKey, tDESIV), CryptoStreamMode.Write);
                decStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);
                decStream.Close();
                return encoding.GetString(decryptedData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성목적 : 주어진 인코딩을 사용하여 문자열을 암호화후 Base64 스트링으로 변환
        /// 작성일자 : 2009-08-10
        /// </summary>
        /// <param name="stringToEncrypt">The string to encrypt.</param>
        /// <param name="encoding">인코딩방식</param>
        /// <param name="_net20">The _net20.</param>
        /// <returns>암호화된 문자열</returns>
        public string TDESEncryptString(string stringToEncrypt, Encoding encoding, string _net20)
        {
            try
            {
                string sOutcome = "";

                tDESKey = encoding.GetBytes(sEncryptionKey);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = encoding.GetBytes(stringToEncrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(tDESKey, tDESIV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                sOutcome = Convert.ToBase64String(ms.ToArray());
                // space killer
                sOutcome = sOutcome.Replace(" ", "");

                // special character replacements
                sOutcome = sOutcome.Replace("+", "~plus~");

                return sOutcome;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성목적 : 주어진 인코딩을 사용하여 문자열을 복호화 (대상 문자열은 TripleDES 암호화후 Base64 변환된 문자열)
        /// 작성일자 : 2009-08-10
        /// </summary>
        /// <param name="stringToDecrypt">The string to decrypt.</param>
        /// <param name="encoding">인코딩방식</param>
        /// <param name="_net20">The _net20.</param>
        /// <returns>복호화된 문자열</returns>
        public string TDESDecryptString(string stringToDecrypt, Encoding encoding, string _net20)
        {
            try
            {
                byte[] inputByteArray = new byte[stringToDecrypt.Length + 1];
                string sOutcome = "";
                // special characters come back home
                stringToDecrypt = stringToDecrypt.Replace("~plus~", "+");

                tDESKey = encoding.GetBytes(sEncryptionKey);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(stringToDecrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(tDESKey, tDESIV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding1 = encoding;
                sOutcome = encoding1.GetString(ms.ToArray());
                return sOutcome;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}