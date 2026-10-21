using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper
{
    /// <summary>
    /// 작성자   : 심재운
    /// 작성일자 : 2008-08-08
    /// 작성목적 : UtilHelper
    /// </summary>
    public static class UtilHelper
    {
        /// <summary>
        /// 작성자   : 심재운
        /// 작성목적 : 문자열을 int 형으로 tryparse , 잘못된 값이면 0 반환
        /// 작성일자 : 2009-09-28
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int ToNumber<T>(this string input) where T : struct
        {
            input = input.Replace(",", "");
            Type[] paramTypes = new Type[] { typeof(string), typeof(T).MakeByRefType() };
            MethodInfo method = typeof(T).GetMethod("TryParse", paramTypes);
            Object[] parameters = new Object[] { input, null };
            if (method != null)
            {
                bool val = (bool)method.Invoke(typeof(T), parameters);
                if (val == true)
                    return (int)parameters[1];
            }
            return 0;
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성목적 : null 을 빈값으로 처리
        /// 작성일자 : 2009-09-28
        /// </summary>
        /// <param name="_inputString">The _input string.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToNull(this string _inputString)
        {
            if (string.IsNullOrEmpty(_inputString) || _inputString.Length == 0) return "";
            return _inputString;
        }

        /// <summary>
        /// 심재운
        /// 용도 : 날짜형 null 처리
        /// </summary>
        /// <param name="_inputString">The _input string.</param>
        /// <returns></returns>
        public static object ToNullDT(this DateTime _inputString)
        {
            if (_inputString == System.DateTime.MinValue ||
                _inputString == System.DateTime.MaxValue || _inputString == null)
            {
                return System.DBNull.Value;
            }

            return _inputString;
        }

        /// <summary>
        /// 심재운
        /// 용도 : 숫자형을 DB 의 null 처리
        /// </summary>
        /// <param name="_inputString">The _input string.</param>
        /// <returns></returns>
        public static object ToNullInt(this int _inputString)
        {
            if (_inputString == 0)
            {
                return System.DBNull.Value;
            }

            return _inputString;
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성목적 : 문자열을 double 형으로 tryparse , 잘못된 값이면 0 반환
        /// 작성일자 : 2009-09-28
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double ToDouble<T>(this string input) where T : struct
        {
            input = input.Replace(",", "");
            Type[] paramTypes = new Type[] { typeof(string), typeof(T).MakeByRefType() };
            MethodInfo method = typeof(T).GetMethod("TryParse", paramTypes);
            Object[] parameters = new Object[] { input, null };
            if (method != null)
            {
                bool val = (bool)method.Invoke(typeof(T), parameters);
                if (val == true)
                    return (double)parameters[1];
            }
            return 0;
        }

        /// <summary>
        /// Toes the inject.
        /// </summary>
        /// <param name="_inputString">The _input string.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToInject(this string _inputString)
        {
            string[] blackList = { "=", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "|", "\\", "`", "~", "--", "-", "{", "}", "\"", "'", ";", "select ", "update ", "delete ", "drop ", "union ", "and ", "or ", "1=1", " sp_", " xp_", "@variable ", "@@variable ", "exec ", "sysobject ", "declare " };

            foreach (string _str in blackList)
            {
                _inputString = _inputString.Replace(_str.ToUpper(), "");
                _inputString = _inputString.Replace(_str.ToLower(), "");
            }

            return _inputString;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 다국날짜형 특수문자 제거 (숫자 8자리 반환)
        /// </summary>
        /// <param name="_inputString">The _input string.</param>
        /// <returns></returns>
        public static string ToDateCharRemove(this string _inputString)
        {
            _inputString = Regex.Replace(_inputString, "-", "", RegexOptions.IgnoreCase);
            _inputString = Regex.Replace(_inputString, "/", "", RegexOptions.IgnoreCase);
            _inputString = Regex.Replace(_inputString, ".", "", RegexOptions.IgnoreCase);

            return _inputString;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 일반 INPUT 입력값에 적용된 사항임.
        /// cross site scripting : 모든 내용 xss 처리함(공격자들이 악용하는 위험한 스크립트들을 아예 제거)
        /// (script)alert(document.cookie)(/script)
        /// </summary>
        /// <param name="_inputString"></param>
        /// <returns></returns>
        public static string ToXSS(this string _inputString)
        {
            _inputString = _inputString.ToNull();

            //return Microsoft.Security.Application.Encoder.HtmlEncode(Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(_inputString));
            //HttpContext context = HttpContext.Current;
            //return context.Server.HtmlEncode(Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(_inputString)); //한글 '으' 넣으면 문제 발생

            // & 와 # 은 html 특수문자와 겹치므로 지울 경우 문제생김
            string removeChar = Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "XSSRemoveChar", @"");

            if (!String.IsNullOrEmpty(removeChar))
            {
                ///string removeChar = @"/,\,;,:,-,+,(,)";
                foreach (string str in removeChar.Split(','))
                {
                    _inputString = _inputString.Replace(str, "");
                }
            }

            //html 에 특수문자 보여주기
            //_inputString = _inputString.Replace("&", "&#38;");
            _inputString = _inputString.Replace("&", "&amp;");
            //_inputString = _inputString.Replace(@"""", "&#34;");
            _inputString = _inputString.Replace(@"""", "&quot;");

            _inputString = _inputString.Replace("#", "&#35;");

            _inputString = _inputString.Replace("'", "&#39;");
            _inputString = _inputString.Replace("(", "&#40;");
            _inputString = _inputString.Replace(")", "&#41;");

            _inputString = _inputString.Replace("<", "&lt;");
            _inputString = _inputString.Replace(">", "&gt;");

            //_inputString = _inputString.Replace("<", "[");
            //_inputString = _inputString.Replace(">", "]");

            return _inputString;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// cross site scripting 차단 코드를 다시 원복
        /// 수정시 특수문자 원복할 경우 사용 ByFreeman 20120514
        /// </summary>
        /// <param name="_inputString">The _input string.</param>
        /// <returns></returns>
        public static string ToReverseXSS(this string _inputString)
        {
            _inputString = _inputString.ToNull();

            //특수문자 추가 replace byfreeman 20120514
            //_inputString = _inputString.Replace("&#38;", "&");
            _inputString = _inputString.Replace("&amp;", "&");
            //_inputString = _inputString.Replace("&#34;", @"""");
            _inputString = _inputString.Replace("&quot;", @"""");
            _inputString = _inputString.Replace("&#35;", "#");

            _inputString = _inputString.Replace("&#39;", "'");
            _inputString = _inputString.Replace("&#40;", "(");
            _inputString = _inputString.Replace("&#41;", ")");

            _inputString = _inputString.Replace("&lt;", "<");
            _inputString = _inputString.Replace("&gt;", ">");

            return _inputString;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 에디터 본문 내용 XSS 스크립트 제거
        /// </summary>
        /// <param name="_inputString"></param>
        /// <returns></returns>
        public static string ToEditorXSS(this string _inputString)
        {
            //_inputString = Regex.Replace(_inputString, "<iframe", "<xiframe", RegexOptions.IgnoreCase);
            //_inputString = Regex.Replace(_inputString, "</iframe", "</xiframe", RegexOptions.IgnoreCase);
            _inputString = Regex.Replace(_inputString, "<script", "<div style='display:none;'><xscript", RegexOptions.IgnoreCase);
            _inputString = Regex.Replace(_inputString, "</script>", "</xscript></div>", RegexOptions.IgnoreCase);
            _inputString = Regex.Replace(_inputString, "<xscript", "<div style='display:none;'><xscript", RegexOptions.IgnoreCase);
            _inputString = Regex.Replace(_inputString, "</xscript>", "</xscript></div>", RegexOptions.IgnoreCase);
            _inputString = Regex.Replace(_inputString, "<form", "<xform", RegexOptions.IgnoreCase);
            _inputString = Regex.Replace(_inputString, "</form>", "</xform>", RegexOptions.IgnoreCase);

            return _inputString;

            //Microsoft.Security.Application.Encoder.JavaScriptEncode();
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-08-06
        /// 작성목적 : 특정 금칙어 제거하기 : RemoveBlackList("\'\"", value)
        /// 공격 예시: "admin' AND USER_NAME='관리자";
        /// 방어 결과: "admin AND USER_NAME=관리자";
        /// 질의문 결과: "SELECT * FROM USERS WHERE USER_ID='admin AND USER_NAME=관리자'"
        /// </summary>
        /// <param name="_pattern">The _pattern.</param>
        /// <param name="_inputString">The _input string.</param>
        /// <returns></returns>
        public static string RemoveBlackList(string _pattern, string _inputString)
        {
            bool flag = false;
            StringBuilder sb = new StringBuilder(_inputString.Length);

            for (int i = 0; i < _inputString.Length; i++)
            {
                for (int j = 0; j < _pattern.Length; j++)
                {
                    // 조건에 해당되는 문자임을 통지하고 검사 과정을 파기합니다.
                    if (_inputString[i].Equals(_pattern[j]))
                    {
                        flag = true;
                        break;
                    }
                }

                // 조건에 해당하는 문자임을 확인하고 검사 문자를 생략합니다.
                if (flag)
                {
                    flag = false;
                    continue;
                }

                // 검사 조건에 해당하지 않는 문자이므로 추가합니다.
                sb.Append(_inputString[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 파리미터값으로 키의 값 가져오기
        /// 작성자     : 2014.05.28 - 심재운(수정 : 파라미터에 & 값이 있을경우 GetParmforParameter 오류 2014.05.28)
        /// 설명 : get 방식 url 에서 파라미터는 & 와 = 조합, 값에 &= 사용시 문제점 수정  (2015.01.27) shimpark
        ///        jquery.common.js 에서 String.prototype.securityText 작업변경
        /// </summary>
        /// <param name="_querystring">The _querystring.</param>
        /// <param name="_parmName">Name of the _parm.</param>
        /// <param name="_errorReturn">The _error return.</param>
        /// <returns></returns>
        /// <remarks></remarks>
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

                        /* jquery.common.js String.prototype.securityText 함수에서 가공하여 원글 제공
                            var vals = this.replace(/&/g, "＆");
                            vals = vals.replace(/=/gi, "＝");
                         */
                        paramvalue[1] = paramvalue[1].Replace("＆", "&").Replace("＝", "="); //  (2015.01.27) shimpark

                        dic.Add(paramvalue[0], paramvalue[1]);
                    }
                }

                if (dic.ContainsKey(_parmName)) response = dic[_parmName];

                return (!string.IsNullOrEmpty(response)) ? response : _errorReturn;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("_querystring : " + _querystring, AppSettingType.EIP);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("_parmName : " + _parmName, AppSettingType.EIP);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return "";
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
        /// 작성자   : 심재운
        /// 작성일자 : 2008-08-07
        /// 작성목적 : DeskPlusEIP 의 특정 테이블 존재 여부
        /// </summary>
        /// <param name="_dataBaseType">Type of the _data base.</param>
        /// <param name="_tableName">Name of the _table.</param>
        /// <returns></returns>
        public static bool ExistDataBaseTable(DataBaseType _dataBaseType, string _tableName)
        {
            if (string.IsNullOrEmpty(_tableName)) return false;

            string tableQry = "";
            bool result = false;
            object obj = null;
            switch (_dataBaseType)
            {
                case DataBaseType.DALORACLE:
                    //tableQry = "SELECT count(*) FROM sys.tables where name = '{0}'";
                    //tableQry = string.Format(tableQry, _tableName);
                    //obj = OracleHelper.ExecuteScalar(OracleHelper.CONN_SOLUTION, System.Data.CommandType.Text, tableQry, null);
                    //obj = (obj != null) ? Convert.ToInt32(obj) : 0;
                    //result = (Convert.ToInt32(obj) == 0) ? false : true;
                    break;

                case DataBaseType.DALSQL:
                    tableQry = "SELECT count(*) FROM sys.tables where name = '{0}'";
                    tableQry = string.Format(tableQry, _tableName);
                    obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, System.Data.CommandType.Text, tableQry, null);
                    obj = (obj != null) ? Convert.ToInt32(obj) : 0;
                    result = (Convert.ToInt32(obj) == 0) ? false : true;
                    break;

                case DataBaseType.DALSQL2005:
                    tableQry = "SELECT count(*) FROM sys.tables where name = '{0}'";
                    tableQry = string.Format(tableQry, _tableName);
                    obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, System.Data.CommandType.Text, tableQry, null);
                    obj = (obj != null) ? Convert.ToInt32(obj) : 0;
                    result = (Convert.ToInt32(obj) == 0) ? false : true;
                    break;
            }

            return result;
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : db 의 1차 언어 또는 2차 언어 여부 가져오기 (1-&gt;1차언어, 2-&gt;2차언어)
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-05-23
        /// </summary>
        /// <param name="_preLang">The _pre lang.</param>
        /// <returns></returns>
        public static int GetDBPreLang(string _preLang)
        {
            string firstLang = Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FirstLanguage", "ko,zh-cn,zh-tw,ja");
            string secondLang = Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "SecondLanguage", "en,en-GB");

            int returnValue = 0;
            foreach (string _langStr in firstLang.Split(','))
            {
                if (UtilHelper.Compare(_preLang, _langStr))
                {
                    returnValue = 1;
                    break;
                }
            }

            if (returnValue == 0)
            {
                foreach (string _langStr in secondLang.Split(','))
                {
                    if (UtilHelper.Compare(_preLang, _langStr))
                    {
                        returnValue = 2;
                        break;
                    }
                }
            }

            return returnValue;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일 : 2011-02-23
        /// 작성목적 : sql 검색 할때 와일드 카드 문자 처리
        /// </summary>
        /// <param name="_inputString"></param>
        /// <returns></returns>
        public static string ReplaceString(string _inputString)
        {
            return _inputString.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]"); ;
        }

        /// <summary>
        /// 다국어 Globalization 객체 반환
        /// </summary>
        /// <param name="_preLangReal"></param>
        /// <returns></returns>
        public static System.Globalization.CultureInfo GetGlobal(string _preLangReal)
        {
            _preLangReal = _preLangReal.ToLower();

            System.Globalization.CultureInfo cf = null;

            if (_preLangReal == "en-gb")
            {
                cf = new System.Globalization.CultureInfo("en-GB");
            }
            else if (_preLangReal == "en")
            {
                cf = new System.Globalization.CultureInfo("en-US");
            }
            else if (_preLangReal == "ko")
            {
                cf = new System.Globalization.CultureInfo("ko-KR");
            }
            else if (_preLangReal == "ja")
            {
                cf = new System.Globalization.CultureInfo("ja-JP");
            }
            else if (_preLangReal == "zh-cn")
            {
                cf = new System.Globalization.CultureInfo("zh-CN");
            }
            else if (_preLangReal == "fr")
            {
                cf = new System.Globalization.CultureInfo("fr-FR");
            }
            return cf;
        }

        #region XmlLoadData

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : XmlData 폴더의 특정경로 XML 가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-12
        /// </summary>
        /// <param name="_xmlPath">The _XML path.</param>
        /// <returns></returns>
        public static List<XmlDataInfo> XmlLoadData(string _xmlPath)
        {
            XmlTextReader reader = null;
            List<XmlDataInfo> al = new List<XmlDataInfo>();

            try
            {
                reader = new XmlTextReader(_xmlPath);

                while (reader.Read())
                {
                    if (reader.HasAttributes)
                    {
                        if (reader.GetAttribute("Value") != "" && reader.GetAttribute("Value") != null)
                        {
                            XmlDataInfo be = new XmlDataInfo();
                            be.KeyText = reader.GetAttribute("Text");
                            be.KeyValue = reader.GetAttribute("Value");
                            be.KeyID = reader.GetAttribute("ID");

                            al.Add(be);
                        }
                    }
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return al;
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-08-07
        /// 작성목적 : keyValue 로 해당 Text값 가져오기
        /// </summary>
        /// <param name="_xmlPath">The _XML path.</param>
        /// <param name="_keyValue">The _key value.</param>
        /// <returns></returns>
        public static string XmlLoadData(string _xmlPath, string _keyValue)
        {
            XmlTextReader reader = null;
            string keyText = String.Empty;

            try
            {
                reader = new XmlTextReader(_xmlPath);

                while (reader.Read())
                {
                    if (reader.HasAttributes)
                    {
                        if (reader.GetAttribute("Value") != "" && reader.GetAttribute("Value") != null)
                        {
                            if (Compare(_keyValue, reader.GetAttribute("Value")))
                            {
                                keyText = reader.GetAttribute("Text");
                                break;
                            }
                        }
                    }
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return keyText;
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-09-05
        /// 작성목적 : 문자형 reverse 하기
        /// </summary>
        /// <param name="_str">The _STR.</param>
        /// <returns></returns>
        public static string ReverseChar(string _str)
        {
            int len = _str.Length;
            char[] arr = new char[len];

            for (int i = 0; i < len; i++)
            {
                arr[i] = _str[len - 1 - i];
            }

            return new string(arr);
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-09-24
        /// 작성목적 : 구분자값으로 REVERSE 하기
        /// </summary>
        /// <param name="toReverse">To reverse.</param>
        /// <param name="_splitChar">The _split char.</param>
        /// <returns></returns>
        public static string[] ReverseString(string toReverse, char _splitChar)
        {
            char[] splitChar = { _splitChar };
            string[] charArray = toReverse.Split(splitChar);
            Array.Reverse(charArray);
            return charArray;
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-09-26
        /// 작성목적 : 캐쉬 값 가져오기
        /// </summary>
        /// <param name="_cacheName">Name of the _cache.</param>
        /// <returns></returns>
        public static object GetCacheValue(string _cacheName)
        {
            object obj = null;

            if (System.Web.HttpContext.Current.Cache[_cacheName] != null)
            {
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                obj = (Object)context.Cache[_cacheName];
            }

            return obj;
        }

        #endregion XmlLoadData

        #region file

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2011-01-20
        /// 작성목적 : 폴더하위 복사
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="sourceDirectory">원본폴더</param>
        /// <param name="targetDirectory">생성폴더</param>
        public static bool FolderCopy(string _sourceDirectory, string _targetDirectory)
        {
            DirectoryInfo diSource = new DirectoryInfo(_sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(_targetDirectory);

            //edm 복사할 대상자 폴더 존재여부
            if (diSource.Exists)
            {
                CopyAll(diSource, diTarget);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2011-01-20
        /// 작성목적 : 폴더 하위 파일 복사 모듈
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void CopyAll(DirectoryInfo _source, DirectoryInfo _target)
        {
            // 폴더 존재 여부, 없으면 생성
            if (Directory.Exists(_target.FullName) == false)
            {
                Directory.CreateDirectory(_target.FullName);
            }

            // 생성한 폴더에 파일을 복사하기
            foreach (FileInfo fi in _source.GetFiles())
            {
                if (!System.IO.File.Exists(Path.Combine(_target.ToString(), fi.Name)))
                {
                    //Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                    fi.CopyTo(Path.Combine(_target.ToString(), fi.Name), true);
                }
            }

            // 재귀함수로 하위폴더의 파일들을 생성하기
            foreach (DirectoryInfo diSourceSubDir in _source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = _target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        /// <summary>
        /// 폴더 삭제
        /// </summary>
        /// <param name="_folderPath">/DeskPlusEIP/FileData/1/Document/18/201102/6/1/</param>
        /// <returns></returns>
        public static bool DelFolder(string _folderPath)
        {
            try
            {
                HttpContext context = HttpContext.Current;
                _folderPath = context.Server.MapPath(_folderPath);

                //@"c:\MyDir"
                DirectoryInfo di = new DirectoryInfo(_folderPath);
                if (di.Exists)
                {
                    di.Delete(true);
                }
                return true;
            }
            catch (Exception ex)
            {
                Exceptions.AppException.WriteException("DelFolder", ex);
                return false;
            }
        }

        #endregion file

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2011-02-01
        /// 작성목적 : PEC.CULTURE 을 CULTURE 로 변환하기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_lang">The _lang.</param>
        /// <returns></returns>
        public static string CommonLanguage(string _lang)
        {
            string lang = _lang;

            //switch (_lang.ToUpper())
            //{
            //    case "KO-KR":
            //        response = "ko";
            //        break;

            //    case "FR-FR" :
            //        response = "fr";
            //        break;

            //    case "EN-US" :
            //        response = "en";
            //        break;

            //    case "JA-JP" :
            //        response = "ja";
            //        break;
            //    default :
            //        response = "ko";
            //        break;
            //}

            //ByFreeman 다국어 정책 20130102
            if (lang == "zh" || lang == "zh-cn" || lang == "zh-hans")  //zh-Hans(중국어간체)
            {
                lang = "zh-cn";
            }
            else if (lang == "ja" || lang == "ja-jp") //일본어
            {
                lang = "ja";
            }
            else if (lang == "ko" || lang == "ko-kr") //한국어
            {
                lang = "ko";
            }
            else //나머지는 영어
            {
                lang = "en";
            }

            return lang;
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

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 멀티 언어 문자열 ,로 구분하여 반환
        /// </summary>
        /// <param name="_list">The _list.</param>
        /// <returns></returns>
        public static string GetMuiltiLang(params object[] _list)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < _list.Length; i++)
            {
                sb.Append(_list[i].ToString().ToNull());
                if (i != (_list.Length - 1)) sb.Append(",");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 메서드명   : CutCharLen
        /// 작성목적   : 한글(2byte),숫자(1byte),영어(1byte) 에 맞게 잘라내기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-01-31
        /// </summary>
        /// <param name="_value">글내용</param>
        /// <param name="limit">제한 글자수</param>
        /// <returns></returns>
        public static String CutCharLen(String _value, int limit)
        {
            int strlen = 0;
            char c;
            StringBuilder sb = new StringBuilder();

            for (int j = 0; j < _value.Length; j++)
            {
                c = _value[j];

                //한글 코드의 시작과 끝 값이다
                if (c < 0xac00 || 0xd7a3 < c)
                {
                    strlen++;
                }
                else
                {
                    strlen += 2;  //한글이다..
                }

                if (strlen > limit)
                {
                    break;
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 메서드명   : CharLen
        /// 작성목적   : 한글(2byte),숫자(1byte),영어(1byte) 글자수
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-01-31
        /// </summary>
        /// <param name="_value">글내용</param>
        /// <param name="limit">제한 글자수</param>
        /// <returns></returns>
        public static int CharLen(String _value)
        {
            int strlen = 0;
            char c;
            StringBuilder sb = new StringBuilder();

            for (int j = 0; j < _value.Length; j++)
            {
                c = _value[j];

                //한글 코드의 시작과 끝 값이다
                if (c < 0xac00 || 0xd7a3 < c)
                {
                    strlen++;
                }
                else
                {
                    strlen += 2;  //한글이다..
                }
            }

            return strlen;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 섬네일 사이즈 가로,세로 알아내기
        /// (원본 사진 손상하지 않게 사이즈 조절)
        /// </summary>
        /// <param name="_original">The _original.</param>
        /// <param name="_maxPixels">The _max pixels.</param>
        /// <returns></returns>
        public static Size GetThumbnailSize(Image _original, int _maxPixels)
        {
            int originalWidth = _original.Width;
            int originalHeight = _original.Height;

            double factor;
            if (originalWidth > originalHeight)
            {
                factor = (double)_maxPixels / originalWidth;
            }
            else
            {
                factor = (double)_maxPixels / originalHeight;
            }

            return new Size((int)(originalWidth * factor), (int)(originalHeight * factor));
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 아이피 정보를 문자열로 반환
        /// </summary>
        /// <param name="_remoteAddrStr">The _remote addr STR.</param>
        /// <returns></returns>
        public static string GetIPAddress(string[] _remoteAddrStr)
        {
            string iPAddress1 = String.Empty;
            string iPAddress2 = String.Empty;
            string iPAddress3 = String.Empty;
            string iPAddress4 = String.Empty;
            string remoteAddr = String.Empty;

            try
            {
                iPAddress1 = Utility.SetFrontDigit(3, _remoteAddrStr[0].ToString(), '0');
                iPAddress2 = Utility.SetFrontDigit(3, _remoteAddrStr[1].ToString(), '0');
                iPAddress3 = Utility.SetFrontDigit(3, _remoteAddrStr[2].ToString(), '0');
                iPAddress4 = Utility.SetFrontDigit(3, _remoteAddrStr[3].ToString(), '0');
                remoteAddr = iPAddress1 + iPAddress2 + iPAddress3 + iPAddress4;
            }
            catch
            {
                remoteAddr = "000000000000";
            }

            return remoteAddr;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 섬네일 이미지 가로,세로 비율값 얻어내기
        /// </summary>
        /// <param name="_sourcePath">The _source path.</param>
        /// <param name="_width">The _width.</param>
        /// <param name="_height">The _height.</param>
        /// <returns></returns>
        public static string GetThumnailSize(string _sourcePath, int _width, int _height)
        {
            Bitmap sourceImage = new Bitmap(_sourcePath);
            ImageFormat sourceFormat = sourceImage.RawFormat;

            decimal ratio = 0;
            int newWidth = 0;
            int newHeight = 0;

            if (sourceImage.Width < _width && sourceImage.Height < _height)
            {
                newWidth = sourceImage.Width;
                newHeight = sourceImage.Height;
            }
            else
            {
                if (sourceImage.Width > sourceImage.Height)
                {
                    ratio = (decimal)_width / sourceImage.Width;
                    newWidth = _width;
                    newHeight = (int)(sourceImage.Height * ratio);
                }
                else
                {
                    ratio = (decimal)_height / sourceImage.Height;
                    newHeight = _height;
                    newWidth = (int)(sourceImage.Width * ratio);
                }
            }

            sourceImage.Dispose();

            return newWidth + "," + newHeight;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// SMS 문자 길이만큼 잘라준다.
        /// 1자리 기준 : 숫자, 영문, 공백
        /// 2자리 기준 : 한글(일본,중국), 한영키로 특수문자 뽑는 ★ 이런거~
        /// </summary>
        /// <param name="_content">The _content.</param>
        /// <param name="_maxLang">The _max language.</param>
        /// <returns></returns>
        public static List<string> GetSMSContent(string _content, int _maxLang)
        {
            int i = 0;
            string tmpMsg = "";
            int lasti = _content.Length;
            List<string> msgList = new List<string>();
            foreach (char c in _content)
            {
                tmpMsg += c.ToString();

                if (Encoding.Default.GetBytes(tmpMsg).Length >= _maxLang) //79로 가자~
                {
                    msgList.Add(tmpMsg);
                    tmpMsg = "";
                }

                i += 1;

                if (i == lasti)
                {
                    msgList.Add(tmpMsg);
                    tmpMsg = "";
                }
            }

            return msgList;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public enum DataBaseType
    {
        /// <summary>
        /// SQL 2000
        /// </summary>
        DALSQL,

        /// <summary>
        /// SQL 2005
        /// </summary>
        DALSQL2005,

        /// <summary>
        /// ORACLE
        /// </summary>
        DALORACLE
    }

    /// <summary>
    /// XmlDataInfo
    /// </summary>
    public class XmlDataInfo
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public string KeyID { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public string KeyText { get; set; }

        /// <summary>
        /// Gets or sets the key value.
        /// </summary>
        /// <value>
        /// The key value.
        /// </value>
        public string KeyValue { get; set; }
    }
}