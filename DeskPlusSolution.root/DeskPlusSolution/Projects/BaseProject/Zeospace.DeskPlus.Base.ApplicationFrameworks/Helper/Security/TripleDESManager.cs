using System;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security
{
    public class TripleDESManager
    {
        private byte[] key;
        private byte[] iv;

        //private byte[] mKey = {135,181,15,30,31,157,65,102,217,162,79,181,212,104,246,207,217,21,173,195,51,56,128,130};
        //private byte[] mIV = {64,249,131,251,161,102,215,78};

        public TripleDESManager()
        {
            string tripleKey = Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "TripleDESKey", "");
            string tripleIV = Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "TripleDESIV", "");
            string[] sKey = tripleKey.Split(',');
            string[] sIV = tripleIV.Split(',');
            this.key = GetByteKey(sKey);
            this.iv = GetByteIV(sIV);
        }

        public byte[] Key { get { return key; } }

        public byte[] IV { get { return iv; } }

        public string Encrypt(string s)
        {
            TripleDES enc = new TripleDES(key, iv);
            return enc.TDESEncryptString(s);
        }

        public string Decrypt(string s)
        {
            TripleDES dec = new TripleDES(key, iv);
            return dec.TDESDecryptString(s);
        }

        /// <summary>
        /// Encrypts the specified s.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public string Encrypt(string s, System.Text.Encoding encoding)
        {
            #region written by shimpark

            //s = s.Replace(" ", "+");

            #endregion written by shimpark

            TripleDES enc = new TripleDES(key, iv);
            return enc.TDESEncryptString(s, encoding, "");
        }

        /// <summary>
        /// Decrypts the specified s.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public string Decrypt(string s, System.Text.Encoding encoding)
        {
            #region written by shimpark

            //s = s.Replace(" ", "+");

            #endregion written by shimpark

            TripleDES dec = new TripleDES(key, iv);
            return dec.TDESDecryptString(s, encoding, "");
        }

        private byte[] GetByteKey(string[] sKey)
        {
            byte[] mkey = new byte[sKey.Length];

            for (int i = 0; i < sKey.Length; i++)
            {
                mkey[i] = Convert.ToByte(sKey[i]);
            }

            return mkey;
        }

        private byte[] GetByteIV(string[] sIV)
        {
            byte[] mIV = new byte[sIV.Length];

            for (int i = 0; i < sIV.Length; i++)
            {
                mIV[i] = Convert.ToByte(sIV[i]);
            }

            return mIV;
        }
    }
}