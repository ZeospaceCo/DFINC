using System;
using System.Collections.Generic;
using Zeospace.DeskPlusMobile.Web.Helper.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Xml;

namespace Zeospace.DeskPlusMobile.Web.Helper.WebComponent
{
    /// <summary>
    /// Utility
    /// </summary>
    public class Utility
    {
        /// <summary>
        /// 작성자   : 이현수
        /// 작성일자 : 2008-08-27
        /// 작성목적 : TripleDES 암호화 (168 byte 가능 한글 84자까지)
        /// </summary>
        /// <param name="_data">The _data.</param>
        /// <returns></returns>
        public static string EncryptString(string _data)
        {
            //빈값처리 Ajax에서는 빈값을 보낼경우 오류 발생 하므로 INULL 이라는 값으로 null 표시함
            if (String.IsNullOrEmpty(_data)) return "!NULL";

            try
            {
                string UrlEnc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "EncryptURLParameter");

                if (UrlEnc == "Y")
                {
                    // 파라미터 암호화에 TripleDES 사용 (보안 강화)
                    TripleDESManager tds = new TripleDESManager();
                    return tds.Encrypt(_data.Trim(), System.Text.Encoding.UTF8);
                }
                else
                {
                    string enc = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(_data));
                    return enc.Replace("+", "~plus~");
                }
            }
            catch
            {
                return "!NULL";
            }
        }

        /// <summary>
        /// 작성자   : 이현수
        /// 작성일자 : 2008-08-27
        /// 작성목적 : TripleDES 복호화
        /// </summary>
        /// <param name="_data">The _data.</param>
        /// <returns></returns>
        public static string DecryptString(string _data)
        {
            //빈값처리 Ajax에서는 빈값을 보낼경우 오류 발생 하므로 INULL 이라는 값으로 null 표시함
            if (String.IsNullOrEmpty(_data)) return "!NULL";

            try
            {
                string UrlEnc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "EncryptURLParameter");

                if (UrlEnc == "Y")
                {
                    // 파라미터 암호화에 TripleDES 사용 (보안 강화)
                    TripleDESManager tds = new TripleDESManager();
                    string data = tds.Decrypt(_data.Trim(), System.Text.Encoding.UTF8);
                    return data.TrimEnd('\0');
                }
                else
                {
                    _data = _data.Replace("~plus~", "+");
                    return System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(_data));
                }
            }
            catch
            {
                return "!NULL";
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// dictionary 를 파라미터 문자열 반환하기
        /// </summary>
        /// <param name="paraDic">The para dic.</param>
        /// <returns></returns>
        public static string GetParameterDic(Dictionary<string, string> paraDic)
        {
            string response = "";
            foreach (KeyValuePair<String, String> entry in paraDic)
            {
                response += entry.Key + "=" + entry.Value + "&";
            }

            return response.TrimEnd('&');
        }

        /// <summary>
        /// 작성목적 : 파리미터값으로 키의 값 가져오기
        /// 심재운
        /// </summary>
        /// <param name="_querystring">The _querystring.</param>
        /// <param name="_parmName">Name of the _parm.</param>
        /// <param name="_errorReturn">The _error return.</param>
        /// <returns></returns>
        public static string GetParmforParameter(string _querystring, string _parmName, string _errorReturn)
        {
            try
            {
                if (string.IsNullOrEmpty(_querystring) || _querystring == "!NULL") return _errorReturn;
                string response = String.Empty;

                _querystring = _querystring.Replace("&amp;", "~@~dp~@~");  //ByFreeman 20140516 URL 파라미터에 &값이 있을경우 오류

                _querystring = _querystring.ToReverseXSS();

                string[] pairs = _querystring.Split('&');
                Dictionary<string, string> dic = new Dictionary<string, string>();
                string tmpPair = String.Empty;
                foreach (string pair in pairs)
                {
                    if (String.IsNullOrEmpty(pair) || pair.ToNull() == "") continue;

                    tmpPair = pair.Replace("~@~dp~@~", "&");  //ByFreeman 20140516 URL 파라미터에 &값이 있을경우 오류

                    string[] paramvalue = tmpPair.Split('=');

                    if (dic.ContainsKey(paramvalue[0]) == false && paramvalue[0].ToNull() != "")
                    {
                        if (paramvalue.Length > 2) paramvalue[1] = tmpPair.Substring(tmpPair.IndexOf('=') + 1).Trim();    //특수문자 이후 사라지는 문제 ByFreeman 20131031
                        dic.Add(paramvalue[0], paramvalue[1]);
                    }
                }

                if (dic.ContainsKey(_parmName)) response = dic[_parmName];

                return (!string.IsNullOrEmpty(response)) ? response : _errorReturn;
            }
            catch (Exception ex)
            {
                AppException.WriteException(ex.Message);
                return "";
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 작성목적 :  원하는 cipher(자릿수), 해당값, 빈자리수 대체값
        /// ex) 3,"1","0"  (결과값 : 001)
        /// </summary>
        /// <param name="_cipher">The _cipher.</param>
        /// <param name="_value">The _value.</param>
        /// <param name="_paddingChar">The _padding char.</param>
        /// <returns></returns>
        public static string SetFrontDigit(int _cipher, string _value, char _paddingChar)
        {
            return "".PadRight(_cipher - _value.Length, _paddingChar) + _value;
        }

        /// <summary>
        /// 작성자   : shimpark
        /// 작성일자 : 2008-08-06
        /// 작성목적 : 메시지 박스를 출력한다.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="key">The key.</param>
        /// <param name="message">The message.</param>
        public static void ShowMessageBox(Page page, string key, string message, string url)
        {
            ClientScriptManager cs = page.ClientScript;
            Type cstype = page.GetType();

            if (!cs.IsStartupScriptRegistered(cstype, key))
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("<script type=text/javascript> alert('{0}')");
                if (url.Length > 0) sb.Append(";" + url);
                sb.Append("</");
                sb.Append("script>");

                cs.RegisterStartupScript(cstype, key, string.Format(sb.ToString(), message, true));
            }
        }

        /// <summary>
        /// 작성자    : shimpark
        /// 작성일자  : 2008-09-20
        /// 작성목적  : 스크립트를 출력한다.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="key">The key.</param>
        /// <param name="script">Thie script string.</param>
        public static void WriteScript(Page page, string key, string script)
        {
            if (!page.ClientScript.IsStartupScriptRegistered(key))
            {
                page.ClientScript.RegisterStartupScript(page.GetType(), key, script, true);
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 다국어언어 간추려서 하나의 언어 반환하기
        /// 특정 언어에 값이 없으면 base 값이 기본값으로 반환
        /// </summary>
        /// <param name="_base">The _base.</param>
        /// <param name="_ko">The _ko.</param>
        /// <param name="_en">The _en.</param>
        /// <param name="_ja">The _ja.</param>
        /// <param name="_fr">The _FR.</param>
        /// <param name="_zhcn">The _ZHCN.</param>
        /// <param name="_zhtw">The _ZHTW.</param>
        /// <param name="_preLang">The _pre lang.</param>
        /// <returns></returns>
        public static string GetLangName(string _base, string _ko, string _en, string _ja, string _fr,
            string _zhcn, string _zhtw, string _preLang)
        {
            string lang = "";

            _base = _base.ToNull();
            _ko = _ko.ToNull();
            _en = _en.ToNull();
            _ja = _ja.ToNull();
            _fr = _fr.ToNull();
            _zhcn = _zhcn.ToNull();
            _zhtw = _zhtw.ToNull();

            switch (_preLang.ToLower())
            {
                case "ko":
                    lang = _ko;
                    break;

                case "en":
                    lang = _en;
                    break;

                case "ja":
                    lang = _ja;
                    break;

                case "fr":
                    lang = _fr;
                    break;

                case "zh-cn":
                    lang = _zhcn;
                    break;

                case "zh-tw":
                    lang = _zhtw;
                    break;
            }

            //특정 언어에 값이 없으면 base 값이 기본값으로 반환
            return (lang.Length > 0) ? lang : _base;
        }


        #region 에디터

        /// <summary>
        /// 에디터 header 부분 기본 스타일값(P , TABLE , DIV) 적용
        /// </summary>
        /// <param name="_preLang">The _pre lang.</param>
        /// <param name="_cssPath">The _CSS path.</param>
        /// <returns></returns>
        public static string GetEditorHtmlHeader(string _preLang, string _cssPath)
        {
            string lang = "";
            switch (_preLang)
            {
                case "ko":
                    lang = "gulim";
                    break;

                case "zh-cn":
                    lang = "SimSun";
                    break;

                default:
                    lang = "Arial";
                    break;
            }

            string url = @"/DeskPlusMobile/App_Data/Base.xml";

            HttpContext context = HttpContext.Current;
            XmlDocument xml = new XmlDocument();
            xml.Load(context.Server.MapPath(url));

            XmlNodeList xnList = xml.SelectNodes("/XmlData/EditorHtmlHeader"); //루트 접근할 노드

            string response = "";

            foreach (XmlNode xn in xnList)
            {
                string htmlCss = xn["Css"].InnerText.Replace("$0$", lang).Replace("$1$", _cssPath); //CSS 포함 HTML
                string html = xn["CssNo"].InnerText.Replace("$0$", lang).Replace("$1$", _cssPath); //CSS 미포함 HTML

                response = html;

                if (_cssPath.Trim().Length > 0) response = htmlCss;
            }

            return response;
        }

        #endregion 에디터

    }
}