using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

//using System.EnterpriseServices;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper
{
    /// <summary>
    /// UrlSendHelper에 대한 요약 설명입니다.
    /// </summary>
    public class UrlSendHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UrlSendHelper"/> class.
        /// </summary>
        public UrlSendHelper()
        {
        }

        /// <summary>
        /// UrlEncoding Data 가져오기
        /// </summary>
        /// <param name="_targetPath"></param>
        /// <returns></returns>
        public static string GetUrlEncoding(string _targetPath)
        {
            StringBuilder targetURL = new StringBuilder();
            targetURL.Append(HttpUtility.UrlEncode(_targetPath, System.Text.Encoding.GetEncoding("euc-kr")));   // 파라미터 값

            return targetURL.ToString();
        }

        /// <summary>
        /// Makes the param.
        /// </summary>
        /// <param name="_targetPath">The _target path.</param>
        /// <param name="_paramName">Name of the _param.</param>
        /// <param name="_paramValue">The _param value.</param>
        /// <returns></returns>
        public static string MakeParam(string _targetPath, string _paramName, string _paramValue)
        {
            StringBuilder targetURL = new StringBuilder();
            targetURL.Append("http://");
            targetURL.Append(_targetPath);								//  "/cgi-bin/mailgate.cgi"
            targetURL.Append("?" + _paramName + "=");						//   "?parm="
            targetURL.Append(HttpUtility.UrlEncode(_paramValue, System.Text.Encoding.GetEncoding("euc-kr")));   // 파라미터 값

            return targetURL.ToString();
        }

        /// <summary>
        /// Makes the param.
        /// </summary>
        /// <param name="_targetPath">The _target path.</param>
        /// <param name="_paramName">Name of the _param.</param>
        /// <param name="_paramValue">The _param value.</param>
        /// <returns></returns>
        public static string MakeParam(string _targetPath, string[] _paramName, string[] _paramValue)
        {
            StringBuilder targetURL = new StringBuilder();
            targetURL.Append("http://");
            targetURL.Append(_targetPath);

            for (int i = 0; i < _paramName.Length; i++)
            {
                if (i == 0)
                {
                    targetURL.Append("?");
                }
                else
                {
                    targetURL.Append("&");
                }

                targetURL.Append(_paramName[i] + "=" + HttpUtility.UrlEncode(_paramValue[i], System.Text.Encoding.GetEncoding("euc-kr")));
            }

            return targetURL.ToString();
        }

        /// <summary>
        /// Uses the post.
        /// </summary>
        /// <param name="_url">The _url.</param>
        /// <param name="_params">The _params.</param>
        /// <returns></returns>
        public static string UsePost(string _url, string _params)
        {
            string rtnStr = "";

            WebResponse result = null;

            try
            {
                WebRequest req = WebRequest.Create(_url);
                req.Method = "POST";

                StringBuilder UrlEncoded = new StringBuilder();
                Char[] reserved = { '?', '=', '&' };
                byte[] SomeBytes = null;

                if (!String.IsNullOrEmpty(_params))
                {
                    int i = 0, j;

                    while (i < _params.Length)
                    {
                        j = _params.IndexOfAny(reserved, i);
                        if (j == -1)
                        {
                            //UrlEncoded.Append(HttpUtility.UrlEncode(_params.Substring(i, _params.Length-i)));
                            UrlEncoded.Append(_params.Substring(i, _params.Length - i));
                            break;
                        }
                        //UrlEncoded.Append(HttpUtility.UrlEncode(_params.Substring(i, j-i)));
                        UrlEncoded.Append(_params.Substring(i, j - i));
                        UrlEncoded.Append(_params.Substring(j, 1));
                        i = j + 1;
                    }

                    SomeBytes = Encoding.UTF8.GetBytes(UrlEncoded.ToString());

                    req.ContentType = "application/x-www-form-urlencoded";
                    req.ContentLength = SomeBytes.Length;

                    Stream newStream = req.GetRequestStream();
                    newStream.Write(SomeBytes, 0, SomeBytes.Length);
                    newStream.Close();
                }
                else
                {
                    req.ContentLength = 0;
                }

                result = req.GetResponse();

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("result:" + ((HttpWebResponse)result).StatusDescription, AppSettingType.MAIL);

                Stream ReceiveStream = result.GetResponseStream();
                Encoding encode = System.Text.Encoding.UTF8;
                StreamReader sr = new StreamReader(ReceiveStream, encode);

                rtnStr = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(_url + " 에 대한 UsePost [" + ex.Message + "]");
            }
            finally
            {
                if (result != null)
                {
                    result.Close();
                }
            }
            return rtnStr;
        }
    }
}