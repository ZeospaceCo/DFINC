using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Xml;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent
{
    /// <summary>
    /// Utility
    /// </summary>
    public class Utility
    {
        /// <summary>
        /// 작성자   : shimpark
        /// 작성일자 : 2008-08-06
        /// 작성목적 : Js 파일 include 컨트롤 반환
        /// </summary>
        /// <param name="_js">The _JS.</param>
        /// <returns></returns>
        public static HtmlGenericControl JsInclude(string _js)
        {
            HtmlGenericControl include = new HtmlGenericControl("script");
            include.Attributes.Add("type", "text/javascript");
            include.Attributes.Add("src", _js);

            return include;
        }

        /// <summary>
        /// 작성자   : shimpark
        /// 작성일자 : 2008-08-06
        /// 작성목적 : 자바스크립트 var 변수 할당하기
        /// </summary>
        /// <param name="_dic">The _dic.</param>
        /// <returns></returns>
        public static HtmlGenericControl ScriptInclude(Dictionary<string, string> _dic)
        {
            HtmlGenericControl include = new HtmlGenericControl("script");
            include.Attributes.Add("type", "text/javascript");
            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, string> kvp in _dic)
            {
                sb.Append("\n");
                sb.Append("var ");
                sb.Append(kvp.Key);
                sb.Append("=\"");
                sb.Append(kvp.Value);
                sb.Append("\";");
            }

            sb.Append("\n");

            include.InnerHtml = sb.ToString();

            return include;
        }

        /// <summary>
        /// 작성자   : shimpark
        /// 작성일자 : 2008-08-06
        /// 작성목적 : 컴포넌트 다국어 처리
        /// </summary>
        /// <param name="_errMessage">The _err message.</param>
        /// <returns></returns>
        public static string GetString(string _errMessage)
        {
            System.Resources.ResourceManager resMng = new System.Resources.ResourceManager(
                "Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Resource.GlobalString.string",
                Assembly.GetExecutingAssembly());
            return resMng.GetString(_errMessage);
        }

        /// <summary>
        /// 작성자   : shimpark
        /// 작성일자 : 2008-08-06
        /// 작성목적 : 메일 형식인 체크
        /// </summary>
        /// <param name="_inputString">The _input string.</param>
        /// <returns></returns>
        public static string IsEmail(string _inputString)
        {
            if (Regex.IsMatch(_inputString, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
            {
                return "";
            }
            else
            {
                return GetString("Validation_IsEmail_Email");
            }
        }

        /// <summary>
        /// 작성자   : shimpark
        /// 작성일자 : 2008-08-06
        /// 작성목적 : 문자열을 char 단위로 자른다.
        /// </summary>
        /// <param name="orgText">The org text.</param>
        /// <param name="max">The max.</param>
        /// <returns></returns>
        public static string CutString(string orgText, int max)
        {
            string result = string.Empty;
            if (orgText != null && orgText != string.Empty)
            {
                int nByte = 0;
                char[] cArr = orgText.ToCharArray();
                for (int i = 0; i < cArr.Length; i++)
                {
                    int keyCode = Convert.ToInt32(cArr[i]);
                    if (keyCode < 0 || keyCode >= 128)
                    {
                        nByte += 2; //한글 2byte
                    }
                    else
                    {
                        nByte++;  //영문은 1바이트
                    }

                    if (nByte <= max)
                    {
                        result += orgText.Substring(i, 1);
                    }
                    else
                    {
                        result = result.Trim();
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 작성자   : shimpark
        /// 작성일자 : 2008-08-06
        /// 작성목적 : 텍스트 길이를 가져온다. 한글은 2Byte 처리
        /// </summary>
        /// <param name="orgText">The org text.</param>
        /// <returns></returns>
        public static int LengthString(string orgText)
        {
            string result = string.Empty;
            int nByte = 0;

            if (orgText != null && orgText != string.Empty)
            {
                char[] cArr = orgText.ToCharArray();
                for (int i = 0; i < cArr.Length; i++)
                {
                    int keyCode = Convert.ToInt32(cArr[i]);
                    if (keyCode < 0 || keyCode >= 128)
                    {
                        nByte += 2; //한글 2byte
                    }
                    else
                    {
                        nByte++;  //영문은 1바이트
                    }
                }
            }

            return nByte;
        }

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
                string UrlEnc = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "EncryptURLParameter", "N");

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
                string UrlEnc = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "EncryptURLParameter", "N");

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
        /// 메서드명   :
        /// 작성목적   : 비교문 : true 동일함; false 는 동일하지 않음
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-05-23
        /// </summary>
        /// <param name="inputA">The input A.</param>
        /// <param name="inputB">The input B.</param>
        /// <returns></returns>
        public static bool Compare(string inputA, string inputB)
        {
            return (string.Compare(inputA, inputB, true, System.Globalization.CultureInfo.CurrentCulture) == 0) ? true : false;
        }

        /// <summary>
        /// 작성자   : 이현수
        /// 작성일자 : 2008-08-27
        /// 작성목적 : 복호화 파라미터 HashTable 담기
        /// </summary>
        /// <param name="_encParam">The _enc param.</param>
        /// <returns></returns>
        public static Hashtable DecryptHashTable(string _encParam)
        {
            TripleDESManager tds = new TripleDESManager();
            string decParam = tds.Decrypt(_encParam, System.Text.Encoding.UTF8).TrimEnd('\0');

            string[] arrDecParam = decParam.Split('&');

            Hashtable htDecParameter = new Hashtable();

            foreach (string tmp in arrDecParam)
            {
                string[] arrTmp = tmp.Split('=');
                if (arrTmp != null && arrTmp.Length > 1)
                {
                    htDecParameter.Add(arrTmp[0], arrTmp[1]);
                }
            }

            return htDecParameter;
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
        /// 작성자 : 심재운
        /// 페이징 사이즈 및 카운트 구하기
        /// </summary>
        /// <param name="_totalCnt">The _total CNT.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageNo">The _page no.</param>
        /// <returns></returns>
        public static string SetPaging(int _totalCnt, int _pageSize, int _pageNo)
        {
            //페이지 총 개수

            int recsInLastPage = _totalCnt % _pageSize;

            //마지막 페이지의 리스트 개수 구하기
            if (recsInLastPage == 0)
                recsInLastPage = _pageSize;

            int sizeOfPage = _pageSize;
            int pageIndex = _pageNo - 1;
            int pageCount = Convert.ToInt32(((_totalCnt - 1) / sizeOfPage) + 1);

            //넘어온 페이지값과 총페이지값이 동일할 경우(마지막 페이지일경우)
            if (pageIndex + 1 == pageCount)
                sizeOfPage = recsInLastPage;//나머지 개수만큼 출력

            //페이지번호*페이지사이즈
            int pageNoPageSize = _pageSize * pageIndex;

            return sizeOfPage.ToString() + "," + pageNoPageSize.ToString();
        }

        #region ShowMessageBox()

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
        /// 작성자   : shimpark
        /// 작성일자 : 2008-08-06
        /// 작성목적 : 메시지 박스를 출력한다.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="key">The key.</param>
        /// <param name="message">The message.</param>
        public static void ShowMessageBoxNoAuth(Page page, string key, string message)
        {
            page.Response.Clear();
            page.Response.Redirect("/DeskPlusEIP/Html/ErrorMessage/NoAuth.aspx", false);
            page.Response.Flush();
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

        #endregion ShowMessageBox()

        #region Redirect()

        /// <summary>
        /// 작성자   : shimpark
        /// 작성일자 : 2008-08-06
        /// 작성목적 : Redirect 으로 이동한다
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="url">The URL.</param>
        public static void Redirect(string encParameter)
        {
            string url = "/DeskPlusEIP/Common/Notify/CallNotify.aspx?p=" + encParameter;
            HttpContext ctx = HttpContext.Current;
            ctx.Response.Clear();
            ctx.Response.Redirect(url, false);
            ctx.Response.Flush();
            //ctx.Response.End();
        }

        /// <summary>
        /// 작성자   : shimpark
        /// 작성일자 : 2010-12-14
        /// 작성목적 : window.location 으로 이동한다
        /// </summary>
        /// <param name="up"></param>
        /// <param name="message"></param>
        /// <param name="url"></param>
        public static void RedirectScriptManager(UpdatePanel up, string message, string url)
        {
            string strScript = "alert('{0}'); window.location.href='{1}';";
            strScript = string.Format(strScript, message, url);
            string guidkey = Guid.NewGuid().ToString();
            ScriptManager.RegisterStartupScript(up, up.GetType(), guidkey, strScript, true);
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2010-12-16
        /// 작성목적 : alert표시
        /// </summary>
        /// <param name="up"></param>
        /// <param name="message"></param>
        /// <param name="url"></param>
        public static void MessageScriptManager(UpdatePanel up, string message)
        {
            string strScript = "alert('{0}');";
            strScript = string.Format(strScript, message);
            string guidkey = Guid.NewGuid().ToString();
            ScriptManager.RegisterStartupScript(up, up.GetType(), guidkey, strScript, true);
        }

        #endregion Redirect()

        #region 에디터

        /// <summary>
        /// 작성자 : 심재운
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

            string url = @"/DeskPlusEIP/App_Data/Config.xml";

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