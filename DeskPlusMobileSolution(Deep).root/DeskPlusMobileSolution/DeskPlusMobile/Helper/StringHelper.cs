using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Reflection;
using System.Xml;
using System.Linq;
using System.Data;
using System.Web.Script.Serialization;

namespace Zeospace.DeskPlusMobile.Web.Helper
{
    public static class StringHelper
    {

        /// <summary>
        /// Base64 encodes a string.
        /// </summary>
        /// <param name="input">A string</param>
        /// <returns>A base64 encoded string</returns>
        public static string Base64StringEncode(string input)
        {
            byte[] encbuff = System.Text.Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(encbuff);
        }

        /// <summary>
        /// Base64 decodes a string.
        /// </summary>
        /// <param name="input">A base64 encoded string</param>
        /// <returns>A decoded string</returns>
        public static string Base64StringDecode(string input)
        {
            byte[] decbuff = Convert.FromBase64String(input);
            return System.Text.Encoding.UTF8.GetString(decbuff);
        }

        /// <summary>
        /// A case insenstive replace function.
        /// </summary>
        /// <param name="input">The string to examine.</param>
        /// <param name="newValue">The value to replace.</param>
        /// <param name="oldValue">The new value to be inserted</param>
        /// <returns>A string</returns>
        public static string CaseInsenstiveReplace(string input,
           string newValue, string oldValue)
        {
            Regex regEx = new Regex(oldValue,
               RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return regEx.Replace(input, newValue);
        }

        /// <summary>
        /// Replaces the first occurence of a string with the replacement value. The Replace
        /// is case senstive
        /// </summary>
        /// <param name="input">The string to examine</param>
        /// <param name="oldValue">The value to replace</param>
        /// <param name="newValue">the new value to be inserted</param>
        /// <returns>A string</returns>
        public static string ReplaceFirst(string input, string oldValue, string newValue)
        {
            Regex regEx = new Regex(oldValue, RegexOptions.Multiline);
            return regEx.Replace(input, newValue, 1);
        }

        /// <summary>
        /// Replaces the last occurence of a string with the replacement value.
        /// The replace is case senstive.
        /// </summary>
        /// <param name="input">The string to examine</param>
        /// <param name="oldValue">The value to replace</param>
        /// <param name="newValue">the new value to be inserted</param>
        /// <returns>A string</returns>
        public static string ReplaceLast(string input, string oldValue, string newValue)
        {
            int index = input.LastIndexOf(oldValue);
            if (index < 0)
            {
                return input;
            }
            else
            {
                StringBuilder sb = new StringBuilder(input.Length - oldValue.Length + newValue.Length);
                sb.Append(input.Substring(0, index));
                sb.Append(newValue);
                sb.Append(input.Substring(index + oldValue.Length,
                   input.Length - index - oldValue.Length));

                return sb.ToString();
            }
        }

        /// <summary>
        /// Removes all the words passed in the filter words 
        /// parameters. The replace is NOT case
        /// sensitive.
        /// </summary>
        /// <param name="input">The string to search.</param>
        /// <param name="filterWords">The words to 
        /// repace in the input string.</param>
        /// <returns>A string.</returns>
        public static string FilterWords(string input,
           params string[] filterWords)
        {
            return StringHelper.FilterWords(input,
               char.MinValue, filterWords);
        }

        /// <summary>
        /// Removes all the words passed in the filter words 
        /// parameters. The replace is NOT case
        /// sensitive.
        /// </summary>
        /// <param name="input">The string to search.</param>
        /// <param name="mask">A character that is inserted for each 
        /// letter of the replaced word.</param>
        /// <param name="filterWords">The words to 
        // repace in the input string.</param>
        /// <returns>A string.</returns>
        public static string FilterWords(string input, char mask,
           params string[] filterWords)
        {
            string stringMask = mask == char.MinValue ?
               string.Empty : mask.ToString();
            string totalMask = stringMask;

            foreach (string s in filterWords)
            {
                Regex regEx = new Regex(s,
                   RegexOptions.IgnoreCase | RegexOptions.Multiline);

                if (stringMask.Length > 0)
                {
                    for (int i = 1; i < s.Length; i++)
                        totalMask += stringMask;
                }

                input = regEx.Replace(input, totalMask);

                totalMask = stringMask;
            }

            return input;
        }

        /// <summary>
        /// Checks the passed string to see if has any of the 
        /// passed words. Not case-sensitive.
        /// </summary>
        /// <param name="input">The string to check.</param>
        /// <param name="hasWords">The words to check for.</param>
        /// <returns>A collection of the matched words.</returns>
        public static MatchCollection HasWords(string input,
           params string[] hasWords)
        {
            StringBuilder sb = new StringBuilder(hasWords.Length + 50);
            //sb.Append("[");

            foreach (string s in hasWords)
            {
                sb.AppendFormat("({0})|",
                   StringHelper.HtmlSpecialEntitiesEncode(s.Trim()));
            }

            string pattern = sb.ToString();
            pattern = pattern.TrimEnd('|'); // +"]";

            Regex regEx = new Regex(pattern,
               RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return regEx.Matches(input);
        }

        /// <summary>
        /// A wrapper around HttpUtility.HtmlEncode
        /// </summary>
        /// <param name="input">The string to be encoded</param>
        /// <returns>An encoded string</returns>
        public static string HtmlSpecialEntitiesEncode(string input)
        {
            return HttpUtility.HtmlEncode(input);
        }

        /// <summary>
        /// A wrapper around HttpUtility.HtmlDecode
        /// </summary>
        /// <param name="input">The string to be decoded</param>
        /// <returns>The decode string</returns>
        public static string HtmlSpecialEntitiesDecode(string input)
        {
            return HttpUtility.HtmlDecode(input);
        }

        /// <summary>
        /// MD5 encodes the passed string
        /// </summary>
        /// <param name="input">The string to encode.</param>
        /// <returns>An encoded string.</returns>
        public static string MD5String(string input)
        {
            // Create a new instance of the 
            // MD5CryptoServiceProvider object.
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte 
            // array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(
               Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        /// <summary>
        /// Verified a string against the passed MD5 hash.
        /// </summary>
        /// <param name="input">The string to compare.</param>
        /// <param name="hash">The hash to compare against.</param>
        /// <returns>True if the input and the hash 
        /// are the same, false otherwise.</returns>
        public static bool MD5VerifyString(string input, string hash)
        {
            // Hash the input.
            string hashOfInput = StringHelper.MD5String(input);

            // Create a StringComparer an comare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
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
        /// Left pads the passed input using the HTML 
        /// non-breaking string entity (&nbsp;)
        /// for the total number of spaces.
        /// </summary>
        /// <param name="input">The string to pad.</param>
        /// <param name="totalSpaces">The total number 
        /// to pad the string.</param>
        /// <returns>A padded string.</returns>
        public static string PadLeftHtmlSpaces(string input, int totalSpaces)
        {
            string space = "&nbsp;";
            return PadLeft(input, space, totalSpaces * space.Length);
        }

        /// <summary>
        /// Left pads the passed input using the passed pad string
        /// for the total number of spaces.  It will not 
        /// cut-off the pad even if it 
        /// causes the string to exceed the total width.
        /// </summary>
        /// <param name="input">The string to pad.</param>
        /// <param name="pad">The string to uses as padding.</param>
        /// <param name="totalSpaces">The total number to 
        /// pad the string.</param>
        /// <returns>A padded string.</returns>
        public static string PadLeft(string input,
           string pad, int totalWidth)
        {
            return StringHelper.PadLeft(input, pad, totalWidth, false);
        }

        /// <summary>
        /// Left pads the passed input using the passed pad string
        /// for the total number of spaces.  It will 
        /// cut-off the pad so that  
        /// the string does not exceed the total width.
        /// </summary>
        /// <param name="input">The string to pad.</param>
        /// <param name="pad">The string to uses as padding.</param>
        /// <param name="totalSpaces">The total number to 
        /// pad the string.</param>
        /// <returns>A padded string.</returns>
        public static string PadLeft(string input, string pad,
           int totalWidth, bool cutOff)
        {
            if (input.Length >= totalWidth)
                return input;

            int padCount = pad.Length;
            string paddedString = input;

            while (paddedString.Length < totalWidth)
            {
                paddedString += pad;
            }

            // trim the excess.
            if (cutOff)
                paddedString = paddedString.Substring(0, totalWidth);

            return paddedString;
        }

        /// <summary>
        /// Right pads the passed input using the HTML 
        /// non-breaking string entity (&nbsp;)
        /// for the total number of spaces.
        /// </summary>
        /// <param name="input">The string to pad.</param>
        /// <param name="totalSpaces">The total number 
        /// to pad the string.</param>
        /// <returns>A padded string.</returns>
        public static string PadRightHtmlSpaces(string input,
           int totalSpaces)
        {
            string space = "&nbsp;";
            return PadRight(input, space, totalSpaces * space.Length);
        }

        /// <summary>
        /// Right pads the passed input using the passed pad string
        /// for the total number of spaces.  It will not 
        /// cut-off the pad even if it 
        /// causes the string to exceed the total width.
        /// </summary>
        /// <param name="input">The string to pad.</param>
        /// <param name="pad">The string to uses as padding.</param>
        /// <param name="totalSpaces">The total number to 
        /// pad the string.</param>
        /// <returns>A padded string.</returns>
        public static string PadRight(string input,
           string pad, int totalWidth)
        {
            return StringHelper.PadRight(input, pad, totalWidth, false);
        }

        /// <summary>
        /// Right pads the passed input using the passed pad string
        /// for the total number of spaces.  It will cut-off
        /// the pad so that  
        /// the string does not exceed the total width.
        /// </summary>
        /// <param name="input">The string to pad.</param>
        /// <param name="pad">The string to uses as padding.</param>
        /// <param name="totalSpaces">The total number to 
        /// pad the string.</param>
        /// <returns>A padded string.</returns>
        public static string PadRight(string input, string pad,
           int totalWidth, bool cutOff)
        {
            if (input.Length >= totalWidth)
                return input;

            string paddedString = string.Empty;

            while (paddedString.Length < totalWidth - input.Length)
            {
                paddedString += pad;
            }

            // trim the excess.
            if (cutOff)
                paddedString = paddedString.Substring(0,
                   totalWidth - input.Length);

            paddedString += input;

            return paddedString;
        }

        /// <summary>
        /// Removes the new line (\n) and carriage return (\r) symbols.
        /// </summary>
        /// <param name="input">The string to search.</param>
        /// <returns>A string</returns>
        public static string RemoveNewLines(string input)
        {
            return StringHelper.RemoveNewLines(input, false);
        }

        /// <summary>
        /// Removes the new line (\n) and carriage return 
        /// (\r) symbols.
        /// </summary>
        /// <param name="input">The string to search.</param>
        /// <param name="addSpace">If true, adds a space 
        /// (" ") for each newline and carriage
        /// return found.</param>
        /// <returns>A string</returns>
        public static string RemoveNewLines(string input,
           bool addSpace)
        {
            string replace = string.Empty;
            if (addSpace)
                replace = " ";

            string pattern = @"[\r|\n]";
            Regex regEx = new Regex(pattern,
               RegexOptions.IgnoreCase | RegexOptions.Multiline);

            return regEx.Replace(input, replace);
        }

        /// <summary>
        /// Reverse a string.
        /// </summary>
        /// <param name="input">The string to reverse</param>
        /// <returns>A string</returns>
        public static string Reverse(string input)
        {
            if (input.Length <= 1)
                return input;

            char[] c = input.ToCharArray();
            StringBuilder sb = new StringBuilder(c.Length);
            for (int i = c.Length - 1; i > -1; i--)
                sb.Append(c[i]);

            return sb.ToString();
        }

        /// <summary>
        /// Converts a string to sentence case.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>A string</returns>
        public static string SentenceCase(string input)
        {
            if (input.Length < 1)
                return input;

            string sentence = input.ToLower();
            return sentence[0].ToString().ToUpper() +
               sentence.Substring(1);
        }

        /// <summary>
        /// Converts all spaces to HTML non-breaking spaces
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>A string</returns>
        public static string SpaceToNbsp(string input)
        {
            string space = "&nbsp;";
            return input.Replace(" ", space);
        }

        /// <summary>
        /// Removes all HTML tags from the passed string
        /// </summary>
        /// <param name="input">The string whose 
        /// values should be replaced.</param>
        /// <returns>A string.</returns>
        public static string StripTags(string input)
        {
            Regex stripTags = new Regex("<(.|\n)+?>");
            return stripTags.Replace(input, "");
        }

        /// <summary>
        /// Converts a string to title case.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>A string.</returns>
        public static string TitleCase(string input)
        {
            return TitleCase(input, true);
        }

        /// <summary>
        /// Converts a string to title case.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <param name="ignoreShortWords">If true, 
        /// does not capitalize words like
        /// "a", "is", "the", etc.</param>
        /// <returns>A string.</returns>
        public static string TitleCase(string input,
           bool ignoreShortWords)
        {
            List<string> ignoreWords = null;
            if (ignoreShortWords)
            {
                //TODO: Add more ignore words?
                ignoreWords = new List<string>();
                ignoreWords.Add("a");
                ignoreWords.Add("is");
                ignoreWords.Add("was");
                ignoreWords.Add("the");
            }

            string[] tokens = input.Split(' ');
            StringBuilder sb = new StringBuilder(input.Length);
            foreach (string s in tokens)
            {
                if (ignoreShortWords == true
                    && s != tokens[0]
                    && ignoreWords.Contains(s.ToLower()))
                {
                    sb.Append(s + " ");
                }
                else
                {
                    sb.Append(s[0].ToString().ToUpper());
                    sb.Append(s.Substring(1).ToLower());
                    sb.Append(" ");
                }
            }

            return sb.ToString().Trim();
        }

        /// <summary>
        /// Removes multiple spaces between words
        /// </summary>
        /// <param name="input">The string to trim.</param>
        /// <returns>A string.</returns>
        public static string TrimIntraWords(string input)
        {
            Regex regEx = new Regex(@"[\s]+");
            return regEx.Replace(input, " ");
        }

        /// <summary>
        /// Converts new line(\n) and carriage return(\r) symbols to
        /// HTML line breaks.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>A string.</returns>
        public static string NewLineToBreak(string input)
        {
            Regex regEx = new Regex(@"[\n|\r]+");
            return regEx.Replace(input, "<br />");
        }

        /// <summary>
        /// Wraps the passed string at the 
        /// at the next whitespace on or after the 
        /// total charCount has been reached
        /// for that line.  Uses the environment new line
        /// symbol for the break text.
        /// </summary>
        /// <param name="input">The string to wrap.</param>
        /// <param name="charCount">The number of characters 
        /// per line.</param>
        /// <returns>A string.</returns>
        public static string WordWrap(string input, int charCount)
        {
            return StringHelper.WordWrap(input, charCount,
               false, Environment.NewLine);
        }

        /// <summary>
        /// Wraps the passed string at the total 
        /// number of characters (if cuttOff is true)
        /// or at the next whitespace (if cutOff is false).
        /// Uses the environment new line
        /// symbol for the break text.
        /// </summary>
        /// <param name="input">The string to wrap.</param>
        /// <param name="charCount">The number of characters 
        /// per line.</param>
        /// <param name="cutOff">If true, will break in 
        /// the middle of a word.</param>
        /// <returns>A string.</returns>
        public static string WordWrap(string input,
           int charCount, bool cutOff)
        {
            return StringHelper.WordWrap(input, charCount,
               cutOff, Environment.NewLine);
        }

        /// <summary>
        /// Wraps the passed string at the total number 
        /// of characters (if cuttOff is true)
        /// or at the next whitespace (if cutOff is false).
        /// Uses the passed breakText
        /// for lineBreaks.
        /// </summary>
        /// <param name="input">The string to wrap.</param>
        /// <param name="charCount">The number of 
        /// characters per line.</param>
        /// <param name="cutOff">If true, will break in 
        /// the middle of a word.</param>
        /// <param name="breakText">The line break text to use.</param>
        /// <returns>A string.</returns>
        public static string WordWrap(string input, int charCount,
           bool cutOff, string breakText)
        {
            StringBuilder sb = new StringBuilder(input.Length + 100);
            int counter = 0;

            if (cutOff)
            {
                while (counter < input.Length)
                {
                    if (input.Length > counter + charCount)
                    {
                        sb.Append(input.Substring(counter, charCount));
                        sb.Append(breakText);
                    }
                    else
                    {
                        sb.Append(input.Substring(counter));
                    }
                    counter += charCount;
                }
            }
            else
            {
                string[] strings = input.Split(' ');
                for (int i = 0; i < strings.Length; i++)
                {
                    // added one to represent the space.
                    counter += strings[i].Length + 1;
                    if (i != 0 && counter > charCount)
                    {
                        sb.Append(breakText);
                        counter = 0;
                    }

                    sb.Append(strings[i] + ' ');
                }
            }
            // to get rid of the extra space at the end.
            return sb.ToString().TrimEnd();
        }


        /// <summary>
        /// This static function will take an encoded string and
        /// convert certain tags to their original format leaving
        /// tags like <script></script> in their encoded state.
        /// </summary>
        /// <param name="oldHTML">encoded string coming in</param>
        /// <returns>return string with certain tags decoded</returns>
        public static string ConvertEncodedHTMLToRealHTML(string _oldHTML)
        {
            StringBuilder sb = new StringBuilder(_oldHTML);
            // Selectively allow <b> and <i> and ...
            sb.Replace("&lt;b&gt;", "<b>");
            sb.Replace("&lt;/b&gt;", "</b>");
            sb.Replace("&lt;i&gt;", "<i>");
            sb.Replace("&lt;/i&gt;", "</i>");
            sb.Replace("&lt;p&gt;", "<p>");
            sb.Replace("&lt;/p&gt;", "</p>");
            sb.Replace("&lt;u&gt;", "<u>");
            sb.Replace("&lt;/u&gt;", "</u>");
            sb.Replace("&lt;br&gt;", "<br>");
            sb.Replace("&lt;/br&gt;", "</br>");
            sb.Replace("&lt;br/&gt;", "</br>");
            return sb.ToString();

        }

        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 원하는 cipher(자릿수), 해당값, 빈자리수 대체값
        /// ex) 3,"1","0" => 001
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-04-16
        /// </summary>
        /// <param name="_cipher">The _cipher.</param>
        /// <param name="_value">The _value.</param>
        /// <param name="_paddingChar">The _padding char.</param>
        /// <returns></returns>
        public static string SetFrontDigit(int _cipher, string _value, char _paddingChar)
        {
            string getValue = "".PadRight(_cipher - _value.Length, _paddingChar) + _value;
            return getValue;
        }


        /// <summary>
        /// 작성자   : 심재운
        /// 작성목적 : 문자열을 int 형으로 tryparse , 잘못된 값이면 0 반환
        /// 작성일자 : 2009-09-28
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">The input.</param>
        /// <returns></returns>
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
        /// 작성목적 : 문자열을 double 형으로 tryparse , 잘못된 값이면 0 반환
        /// 작성일자 : 2009-09-28
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">The input.</param>
        /// <returns></returns>
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
        /// 작성자   : 심재운
        /// 작성목적 : 제네릭 dictionary 정렬하기
        /// 작성일자 : 2009-10-05
        /// </summary>
        /// <param name="_myDict">The _my dict.</param>
        /// <param name="_sort">asc 또는 desc</param>
        /// <returns></returns>
        public static Dictionary<string, int> DictionarySort(Dictionary<string, int> _myDict, string _sort)
        {
            _sort = (string.IsNullOrEmpty(_sort) || _sort == "") ? "desc" : _sort.ToLower();
            Dictionary<string, int> getMyDict = new Dictionary<string, int>();

            //내림차순일 경우
            if (_sort == "desc")
            {
                var sortedDictDesc = (from entry in _myDict orderby entry.Value descending select entry);
                foreach (KeyValuePair<string, int> data in sortedDictDesc)
                {
                    getMyDict.Add(data.Key, data.Value);
                }
            }
            else if (_sort == "asc") //오름차순
            {
                var sortedDictAsc = (from entry in _myDict orderby entry.Value ascending select entry);
                foreach (KeyValuePair<string, int> data in sortedDictAsc)
                {
                    getMyDict.Add(data.Key, data.Value);
                }
            }

            return getMyDict;
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성목적 : 요소값 합치기
        /// string[] strArray = new string[] { "a", "b", "c" };
        /// string[] strArray2 = new string[] { "d", "e", "f" };
        /// 첫번째 array 에 d, e, f 추가하기
        /// ArrayEx.Append<string>(ref strArray, strArray2);
        /// 하나의 요소인 g 추가하기
        /// ArrayEx.Append<string>(ref strArray, "g");
        /// Console.Write(string.Join(strArray));
        /// 작성일자 : 2009-10-05
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_array">The _array.</param>
        /// <param name="_items">The _items.</param>
        public static void Append<T>(ref T[] _array, params T[] _items)
        {
            int oldLength = _array.Length;

            Array.Resize<T>(ref _array, oldLength + _items.Length);

            for (int i = 0; i < _items.Length; i++)
            {
                _array[oldLength + i] = _items[i];
            }
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성목적 : 특정위치 요소 제거하기
        /// string[] strArray = new string[] { "a", "b", "c" };
        /// element b 요소 제거하기, 1번째임
        /// string Element = RemoveAt<string>(1, ref strArray);
        /// 작성일자 : 2009-10-05
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_index">The _index.</param>
        /// <param name="_list">The _list.</param>
        public static void RemoveAt<T>(int _index, ref T[] _list)
        {
            if (_index < 0 || _list == null || _list.Length == 0) return;

            if (_list.Length > _index + 1)
            {
                for (int i = _index + 1; i < _list.Length; i++)
                {
                    _list[i - 1] = _list[i];
                }
            }

            Array.Resize<T>(ref _list, _list.Length - 1);
        }

        /// <summary>
        /// Toes the SQL inject.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public static string ToSQLInject(this string content)
        {
            string[] blackList = { "--", "-", "{", "}", "\"", "'", ";", "select", "update", "delete", "drop", "union", "and", "or", "1=1", "sp_", "xp_", "@variable", "@@variable", "exec", "sysobject" };

            foreach (string _str in blackList)
            {
                content = content.Replace(_str.ToUpper(), "");
                content = content.Replace(_str.ToLower(), "");
            }

            return content;
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

        #region Json 을 사용하기 위한 함수

        /// <summary>
        /// 작성자   : 심재운
        /// 작성목적 : 객체를 json 형식으로 변환
        /// 작성일자 : 2010-01-15
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static string ToJSON(this object obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = 2147483647;
            return serializer.Serialize(obj);
        }

        /// <summary>
        /// Rowses to dictionary.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        static object RowsToDictionary(this DataTable table)
        {
            var columns = table.Columns.Cast<DataColumn>().ToArray();
            return table.Rows.Cast<DataRow>().Select(r => columns.ToDictionary(c => c.ColumnName, c => r[c]));
        }

        /// <summary>
        /// Toes the dictionary.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        static Dictionary<string, object> ToDictionary(this DataTable table)
        {
            return new Dictionary<string, object>
            {
                { 
                    table.TableName, table.RowsToDictionary() 
                }
            };
        }

        /// <summary>
        /// Toes the dictionary.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        static Dictionary<string, object> ToDictionary(this DataSet data)
        {
            return data.Tables.Cast<DataTable>().ToDictionary(t => t.TableName, t => t.RowsToDictionary());
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성목적 : DataTable 을 json 형식으로 변환
        /// 작성일자 : 2010-01-15
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public static string GetJSONString(DataTable table)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = 2147483647;
            return serializer.Serialize(table.ToDictionary());
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성목적 : DataSet 을 json 형식으로 변환
        /// 작성일자 : 2010-01-15
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string GetJSONString(DataSet data)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(data.ToDictionary());
        }

        #endregion

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


            // & 와 # 은 html 특수문자와 겹치므로 지울 경우 문제생김
            string removeChar = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "XSSRemoveChar");

            if (!String.IsNullOrEmpty(removeChar))
            {
                ///string removeChar = @"/,\,;,:,-,+,(,)";
                foreach (string str in removeChar.Split(','))
                {
                    _inputString = _inputString.Replace(str, "");
                }
            }

            //html 에 특수문자 보여주기
            _inputString = _inputString.Replace("&", "&amp;");
            _inputString = _inputString.Replace(@"""", "&quot;");
            _inputString = _inputString.Replace("#", "&#35;");

            _inputString = _inputString.Replace("'", "&#39;");
            _inputString = _inputString.Replace("(", "&#40;");
            _inputString = _inputString.Replace(")", "&#41;");

            _inputString = _inputString.Replace("<", "&lt;");
            _inputString = _inputString.Replace(">", "&gt;");

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
            _inputString = _inputString.Replace("&amp;", "&");
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
            _inputString = Regex.Replace(_inputString, "<iframe", "<xiframe", RegexOptions.IgnoreCase);
            _inputString = Regex.Replace(_inputString, "</iframe", "</xiframe", RegexOptions.IgnoreCase);
            _inputString = Regex.Replace(_inputString, "<script", "<xscript", RegexOptions.IgnoreCase);
            _inputString = Regex.Replace(_inputString, "</script>", "</xscript>", RegexOptions.IgnoreCase);
            _inputString = Regex.Replace(_inputString, "<form", "<xform", RegexOptions.IgnoreCase);
            _inputString = Regex.Replace(_inputString, "</form>", "</xform>", RegexOptions.IgnoreCase);

            return _inputString;
        }
    }
}
