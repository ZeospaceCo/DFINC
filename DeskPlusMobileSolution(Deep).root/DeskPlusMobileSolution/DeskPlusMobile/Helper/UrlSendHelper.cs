using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Zeospace.DeskPlusMobile.Web.Helper
{
    public class UrlSendHelper
    {

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

                if (_params != null)
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

                    req.ContentLength = SomeBytes.Length;
                    req.ContentType = "application/x-www-form-urlencoded";
                    Stream newStream = req.GetRequestStream();
                    newStream.Write(SomeBytes, 0, SomeBytes.Length);
                    newStream.Close();
                }
                else
                {
                    req.ContentLength = 0;
                }

                result = req.GetResponse();
                Stream ReceiveStream = result.GetResponseStream();
                //Encoding encode = System.Text.Encoding.Default;
                Encoding encode = System.Text.Encoding.UTF8;
                StreamReader sr = new StreamReader(ReceiveStream, encode);

                rtnStr = sr.ReadToEnd();

            }
            catch (Exception ex)
            {
                //Zeospace.DeskPlusMobile.Web..Exceptions.AppException.WriteException(_url + " 에 대한 UsePost [" + ex.Message + "]");
                Helper.AppException.WriteException(_url + " 에 대한 UsePost [" + ex.Message + "]");
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
        /// <summary>
        /// Uses the post.
        /// </summary>
        /// <param name="_url">The _url.</param>
        /// <param name="_params">The _params.</param>
        /// <returns></returns>
        public static string UsePostDev(string _url, string _params)
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

                if (_params != null)
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

                    req.ContentLength = SomeBytes.Length;
                    req.ContentType = "application/x-www-form-urlencoded";
                    Stream newStream = req.GetRequestStream();
                    newStream.Write(SomeBytes, 0, SomeBytes.Length);
                    newStream.Close();
                }
                else
                {
                    req.ContentLength = 0;
                }

                result = req.GetResponse();
                Stream ReceiveStream = result.GetResponseStream();
                //Encoding encode = System.Text.Encoding.Default;
                Encoding encode = System.Text.Encoding.UTF8;
                StreamReader sr = new StreamReader(ReceiveStream, encode);

                rtnStr = sr.ReadToEnd();

                // 네임스페이스 제거 추가 // seo
                rtnStr = RemoveAllNamespaces(rtnStr);

                // 케리지리턴제거
                rtnStr = rtnStr.Replace("\r\n", "");

            }
            catch (Exception ex)
            {
                //Zeospace.DeskPlusMobile.Web..Exceptions.AppException.WriteException(_url + " 에 대한 UsePost [" + ex.Message + "]");
                Helper.AppException.WriteException(_url + " 에 대한 UsePost [" + ex.Message + "]");
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

        public static string HttpPost(string URI, string Parameters)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URI);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            string postData = Parameters;
            byte[] bytes = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = bytes.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);

            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);

            string result = reader.ReadToEnd();
            stream.Dispose();
            reader.Dispose();

            return result.Replace("\r\n","");

        }


        // Xml Namgespace 제거 함수 추가
        public static string RemoveAllNamespaces(string xmlDocument)
        {
            XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xmlDocument));

            return xmlDocumentWithoutNs.ToString();
        }

        //Core recursion function
        private static XElement RemoveAllNamespaces(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName);
                xElement.Value = xmlDocument.Value;

                foreach (XAttribute attribute in xmlDocument.Attributes())
                    xElement.Add(attribute);

                return xElement;
            }
            return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
        }
    }
}