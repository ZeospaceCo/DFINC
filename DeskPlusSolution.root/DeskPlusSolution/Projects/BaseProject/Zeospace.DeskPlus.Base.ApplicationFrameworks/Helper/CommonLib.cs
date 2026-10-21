namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper
{
    /// <summary>
    ///
    /// </summary>
    public class CommonLib
    {
        #region IntToStr

        /// <summary>
        /// 숫자를 지정된 길이의 문자형식으로 반환한다.
        /// </summary>
        /// <param name="_str">문자열</param>
        /// <param name="_len">길이</param>
        /// <returns></returns>
        public string IntToString(string _str, int _len)
        {
            string prefix = "";
            for (int i = 0; i < (_len - _str.Length); i++)
            {
                prefix += "0";
            }

            return prefix + _str;
        }

        /// <summary>
        /// 숫자를 지정된 길이의 문자형식으로 반환한다.
        /// </summary>
        /// <param name="_str">문자열</param>
        /// <param name="_len">길이</param>
        /// <param name="_prefix">반복되는 문자열</param>
        /// <returns></returns>
        public string IntToString(string _str, int _len, string _prefix)
        {
            string prefix = "";
            for (int i = 0; i < (_len - _str.Length); i++)
            {
                prefix += _prefix;
            }

            return prefix + _str;
        }

        #endregion IntToStr

        #region NumtoKoAmt

        /// <summary>
        /// 문자열을 금액표시하여 반환
        /// </summary>
        /// <param name="_data"></param>
        /// <returns></returns>
        public string NumtoKoAmt(string _data)
        {
            int idata = 0;
            int.TryParse(_data, out idata);
            return string.Format("{0:n0}", idata);
        }

        /// <summary>
        /// 숫자형을 금액표시하여 문자열로 반환
        /// </summary>
        /// <param name="_data"></param>
        /// <returns></returns>
        public string NumtoKoAmt(int _data)
        {
            return string.Format("{0:n0}", _data);
        }

        #endregion NumtoKoAmt
    }
}