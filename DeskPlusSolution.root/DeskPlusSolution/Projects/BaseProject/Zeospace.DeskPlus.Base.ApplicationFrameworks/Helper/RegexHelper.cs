using System.Text.RegularExpressions;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper
{
    internal class RegexHelper
    {
        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2012-03-09
        /// 작성목적 : HTML 태그 제거
        /// </summary>
        /// <param name="_html"></param>
        /// <returns></returns>
        public static string RemoveHTML(string _html)
        {
            Regex regHTML = new Regex("[<][a-z|A-Z|/](.|\n)*?[>]");
            return regHTML.Replace(_html, "");
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2012-03-09
        /// 작성목적 : IP 정규식 검사
        /// </summary>
        /// <param name="_ip"></param>
        /// <returns></returns>
        public static bool CheckIPFormat(string _ip)
        {
            Regex regex = new Regex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");
            return regex.IsMatch(_ip);
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2012-03-09
        /// 작성목적 : *를 포함한 IP 정규식 검사
        /// 예) 10.1.10.*      10.*.10.*
        /// </summary>
        /// <param name="_ip"></param>
        /// <returns></returns>
        public static bool CheckIPFormatWithStar(string _ip)
        {
            Regex regex = new Regex(@"^((\d{1,2}|1\d\d|2[0-4]\d|25[0-5])|\*)\.((\d{1,2}|1\d\d|2[0-4]\d|25[0-5])|\*)\.((\d{1,2}|1\d\d|2[0-4]\d|25[0-5])|\*)\.((\d{1,2}|1\d\d|2[0-4]\d|25[0-5])|\*)$");
            return regex.IsMatch(_ip);
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2012-03-09
        /// 작성목적 : 구분자를 기준으로 split 실행한다
        /// </summary>
        /// <param name="orgStr"></param>
        /// <param name="pDelimit"></param>
        /// <returns></returns>
        public static string[] SplitWithString(string _str, string _delimit)
        {
            return (new Regex(_delimit)).Split(_str.Trim());
        }
    }
}