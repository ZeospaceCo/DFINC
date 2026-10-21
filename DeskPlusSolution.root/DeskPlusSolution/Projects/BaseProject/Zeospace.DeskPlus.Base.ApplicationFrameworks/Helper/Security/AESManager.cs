using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security
{
    public static class AESManager
    {
        private static string password = Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "AESPassword", "2012deskplus40");
        private static byte[] salt = Encoding.ASCII.GetBytes(Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "AESSalt", "ee1a9zs9p9co"));

        public static string Encrypt(string _plainText)
        {
            if (string.IsNullOrEmpty(_plainText))
                throw new ArgumentNullException("plainText");

            string outStr = null;                       // Encrypted string to return
            RijndaelManaged aesAlg = null;              // RijndaelManaged object used to encrypt the data.
            try
            {
                // key와 IV 값을 난수값으로 생성
                var key = new Rfc2898DeriveBytes(password, salt);
                // Create a RijndaelManaged object
                // with the specified key and IV.
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);
                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                // Create the streams used for encryption.
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(_plainText);
                        }
                    }
                    outStr = Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }
            // Return the encrypted bytes from the memory stream.
            return outStr;
        }

        public static string Decrypt(string _cipherText)
        {
            if (string.IsNullOrEmpty(_cipherText))
                throw new ArgumentNullException("cipherText");

            // Declare the RijndaelManaged object
            // used to decrypt the data.
            RijndaelManaged aesAlg = null;
            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;
            try
            {
                // generate the key from the shared secret and the salt
                var key = new Rfc2898DeriveBytes(password, salt);
                // Create a RijndaelManaged object
                // with the specified key and IV.
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);
                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                // Create the streams used for decryption.
                byte[] bytes = Convert.FromBase64String(_cipherText);
                using (var msDecrypt = new MemoryStream(bytes))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }
            return plaintext;
        }
    }
}