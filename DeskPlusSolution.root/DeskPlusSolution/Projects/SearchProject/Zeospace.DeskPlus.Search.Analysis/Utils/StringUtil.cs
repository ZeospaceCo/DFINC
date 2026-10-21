using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Zeospace.DeskPlus.Search.Analysis.Utils
{

    /// <summary>
    /// <p>Operations on <seealso cref="java.lang.String"/> that are
    /// <code>null</code> safe.</p>
    /// 
    /// <ul>
    ///  <li><b>IsEmpty/IsBlank</b>
    ///      - checks if a String contains text</li>
    ///  <li><b>Trim/Strip</b>
    ///      - removes leading and trailing whitespace</li>
    ///  <li><b>Equals</b>
    ///      - compares two strings null-safe</li>
    ///  <li><b>startsWith</b>
    ///      - check if a String starts with a prefix null-safe</li>
    ///  <li><b>endsWith</b>
    ///      - check if a String ends with a suffix null-safe</li>
    ///  <li><b>IndexOf/LastIndexOf/Contains</b>
    ///      - null-safe index-of checks
    ///  <li><b>IndexOfAny/LastIndexOfAny/IndexOfAnyBut/LastIndexOfAnyBut</b>
    ///      - index-of any of a set of Strings</li>
    ///  <li><b>ContainsOnly/ContainsNone/ContainsAny</b>
    ///      - does String contains only/none/any of these characters</li>
    ///  <li><b>Substring/Left/Right/Mid</b>
    ///      - null-safe substring extractions</li>
    ///  <li><b>SubstringBefore/SubstringAfter/SubstringBetween</b>
    ///      - substring extraction relative to other strings</li>
    ///  <li><b>Split/Join</b>
    ///      - splits a String into an array of substrings and vice versa</li>
    ///  <li><b>Remove/Delete</b>
    ///      - removes part of a String</li>
    ///  <li><b>Replace/Overlay</b>
    ///      - Searches a String and replaces one String with another</li>
    ///  <li><b>Chomp/Chop</b>
    ///      - removes the last part of a String</li>
    ///  <li><b>LeftPad/RightPad/Center/Repeat</b>
    ///      - pads a String</li>
    ///  <li><b>UpperCase/LowerCase/SwapCase/Capitalize/UnCapitalize</b>
    ///      - changes the case of a String</li>
    ///  <li><b>CountMatches</b>
    ///      - counts the number of occurrences of one String in another</li>
    ///  <li><b>IsAlpha/IsNumeric/IsWhitespace/IsAsciiPrintable</b>
    ///      - checks the characters in a String</li>
    ///  <li><b>DefaultString</b>
    ///      - protects against a null input String</li>
    ///  <li><b>Reverse/ReverseDelimited</b>
    ///      - reverses a String</li>
    ///  <li><b>Abbreviate</b>
    ///      - Abbreviates a string using ellipsis</li>
    ///  <li><b>Difference</b>
    ///      - compares Strings and reports on their differences</li>
    ///  <li><b>LevensteinDistance</b>
    ///      - the number of changes needed to change one String into another</li>
    /// </ul>
    /// 
    /// <p>The <code>StringUtil</code> class defines certain words related to
    /// String handling.</p>
    /// 
    /// <ul>
    ///  <li>null - <code>null</code></li>
    ///  <li>empty - a zero-length string (<code>""</code>)</li>
    ///  <li>space - the space character (<code>' '</code>, char 32)</li>
    ///  <li>whitespace - the characters defined by <seealso cref="Character#isWhitespace(char)"/></li>
    ///  <li>trim - the characters &lt;= 32 as in <seealso cref="String#Trim()"/></li>
    /// </ul>
    /// 
    /// <p><code>StringUtil</code> handles <code>null</code> input Strings quietly.
    /// That is to say that a <code>null</code> input will return <code>null</code>.
    /// Where a <code>Boolean</code> or <code>int</code> is being returned
    /// details vary by method.</p>
    /// 
    /// <p>A side effect of the <code>null</code> handling is that a
    /// <code>NullPointerException</code> should be considered a bug in
    /// <code>StringUtil</code> (except for deprecated methods).</p>
    /// 
    /// <p>Methods in this class give sample code to explain their operation.
    /// The symbol <code>*</code> is used to indicate any input including <code>null</code>.</p>
    /// </summary>
    /// <seealso cref= java.lang.String
    /// @author <a href="http://jakarta.apache.org/turbine/">Apache Jakarta Turbine</a>
    /// @author <a href="mailto:jon@latchkey.com">Jon S. Stevens</a>
    /// @author Daniel L. Rall
    /// @author <a href="mailto:gcoladonato@yahoo.com">Greg Coladonato</a>
    /// @author <a href="mailto:ed@apache.org">Ed Korthof</a>
    /// @author <a href="mailto:rand_mcneely@yahoo.com">Rand McNeely</a>
    /// @author Stephen Colebourne
    /// @author <a href="mailto:fredrik@westermarck.com">Fredrik Westermarck</a>
    /// @author Holger Krauth
    /// @author <a href="mailto:alex@purpletech.com">Alexander Day Chaffee</a>
    /// @author <a href="mailto:hps@intermeta.de">Henning P. Schmiedehausen</a>
    /// @author Arun Mammen Thomas
    /// @author Gary Gregory
    /// @author Phil Steitz
    /// @author Al Chou
    /// @author Michael Davey
    /// @author Reuben Sivan
    /// @author Chris Hyzer
    /// @author Scott Johnson
    /// @since 1.0
    /// @version $Id: StringUtil.java,v 1.1 2010/05/25 01:40:12 smlee0818 Exp $ </seealso>
    public class StringUtil
    {
        // Performance testing notes (JDK 1.4, Jul03, scolebourne)
        // Whitespace:
        // Character.isWhitespace() is faster than WHITESPACE.IndexOf()
        // where WHITESPACE is a string of all whitespace characters
        //
        // Character access:
        // String.CharAt(n) versus ToCharArray(), then array[n]
        // String.CharAt(n) is about 15% worse for a 10K string
        // They are about equal for a length 50 string
        // String.CharAt(n) is about 4 times better for a length 3 string
        // String.CharAt(n) is best bet overall
        //
        // Append:
        // String.concat about twice as fast as StringBuilder.Append
        // (not sure who tested this)

        /// <summary>
        /// The default buffer size to use.
        /// </summary>
        private const int DEFAULT_BUFFER_SIZE = 1024 * 4;

        /// <summary>
        /// An empty immutable <code>String</code> array.
        /// </summary>
        public static readonly string[] EMPTY_STRING_ARRAY = new string[0];

        /// <summary>
        /// The empty String <code>""</code>.
        /// @since 2.0
        /// </summary>
        public const string EMPTY = "";

        /// <summary>
        /// Represents a failed index search.
        /// @since 2.1
        /// </summary>
        public const int INDEX_NOT_FOUND = -1;

        /// <summary>
        /// <p>The maximum size to which the padding constant(s) can expand.</p>
        /// </summary>
        private const int PAD_LIMIT = 8192;

        /// <summary>
        /// <p><code>StringUtil</code> instances should NOT be constructed in
        /// standard programming. Instead, the class should be used as
        /// <code>StringUtil.Trim(" foo ");</code>.</p>
        /// 
        /// <p>This constructor is public to permit tools that require a JavaBean
        /// instance to operate.</p>
        /// </summary>
        public StringUtil()
            : base()
        {
        }

        // Empty checks
        //-----------------------------------------------------------------------
        /// <summary>
        /// <p>Checks if a String is empty ("") or null.</p>
        /// 
        /// <pre>
        /// StringUtil.IsEmpty(null)      = true
        /// StringUtil.IsEmpty("")        = true
        /// StringUtil.IsEmpty(" ")       = false
        /// StringUtil.IsEmpty("bob")     = false
        /// StringUtil.IsEmpty("  bob  ") = false
        /// </pre>
        /// 
        /// <p>NOTE: This method changed in Lang version 2.0.
        /// It no longer trims the String.
        /// That functionality is available in IsBlank().</p>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <returns> <code>true</code> if the String is empty or null </returns>
        public static bool IsEmpty(string str)
        {
            return str == null || str.Length == 0;
        }

        /// <summary>
        /// <p>Checks if a String is not empty ("") and not null.</p>
        /// 
        /// <pre>
        /// StringUtil.isNotEmpty(null)      = false
        /// StringUtil.isNotEmpty("")        = false
        /// StringUtil.isNotEmpty(" ")       = true
        /// StringUtil.isNotEmpty("bob")     = true
        /// StringUtil.isNotEmpty("  bob  ") = true
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <returns> <code>true</code> if the String is not empty and not null </returns>
        public static bool IsNotEmpty(string str)
        {
            return !StringUtil.IsEmpty(str);
        }

        /// <summary>
        /// <p>Checks if a String is whitespace, empty ("") or null.</p>
        /// 
        /// <pre>
        /// StringUtil.IsBlank(null)      = true
        /// StringUtil.IsBlank("")        = true
        /// StringUtil.IsBlank(" ")       = true
        /// StringUtil.IsBlank("bob")     = false
        /// StringUtil.IsBlank("  bob  ") = false
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <returns> <code>true</code> if the String is null, empty or whitespace
        /// @since 2.0 </returns>
        public static bool IsBlank(string str)
        {
            int strLen;
            if (str == null || (strLen = str.Length) == 0)
            {
                return true;
            }
            for (int i = 0; i < strLen; i++)
            {
                if ((char.IsWhiteSpace(str[i]) == false))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// <p>Checks if a String is not empty (""), not null and not whitespace only.</p>
        /// 
        /// <pre>
        /// StringUtil.isNotBlank(null)      = false
        /// StringUtil.isNotBlank("")        = false
        /// StringUtil.isNotBlank(" ")       = false
        /// StringUtil.isNotBlank("bob")     = true
        /// StringUtil.isNotBlank("  bob  ") = true
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <returns> <code>true</code> if the String is
        ///  not empty and not null and not whitespace
        /// @since 2.0 </returns>
        public static bool IsNotBlank(string str)
        {
            return !StringUtil.IsBlank(str);
        }

        // Trim
        //-----------------------------------------------------------------------
        /// <summary>
        /// <p>Removes control characters (char &lt;= 32) from both
        /// ends of this String, handling <code>null</code> by returning
        /// an empty String ("").</p>
        /// 
        /// <pre>
        /// StringUtil.clean(null)          = ""
        /// StringUtil.clean("")            = ""
        /// StringUtil.clean("abc")         = "abc"
        /// StringUtil.clean("    abc    ") = "abc"
        /// StringUtil.clean("     ")       = ""
        /// </pre>
        /// </summary>
        /// <seealso cref= java.lang.String#Trim() </seealso>
        /// <param name="str">  the String to clean, may be null </param>
        /// <returns> the trimmed text, never <code>null</code> </returns>
        /// @deprecated Use the clearer named <seealso cref="#trimToEmpty(String)"/>.
        ///             Method will be removed in Commons Lang 3.0. 
        public static string Clean(string str)
        {
            return str == null ? EMPTY : str.Trim();
        }

        /// <summary>
        /// <p>Removes control characters (char &lt;= 32) from both
        /// ends of this String, handling <code>null</code> by returning
        /// <code>null</code>.</p>
        /// 
        /// <p>The String is trimmed using <seealso cref="String#Trim()"/>.
        /// Trim removes start and end characters &lt;= 32.
        /// To strip whitespace use <seealso cref="#Strip(String)"/>.</p>
        /// 
        /// <p>To trim your choice of characters, use the
        /// <seealso cref="#Strip(String, String)"/> methods.</p>
        /// 
        /// <pre>
        /// StringUtil.Trim(null)          = null
        /// StringUtil.Trim("")            = ""
        /// StringUtil.Trim("     ")       = ""
        /// StringUtil.Trim("abc")         = "abc"
        /// StringUtil.Trim("    abc    ") = "abc"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to be trimmed, may be null </param>
        /// <returns> the trimmed string, <code>null</code> if null String input </returns>
        public static string Trim(string str)
        {
            return str == null ? null : str.Trim();
        }

        /// <summary>
        /// <p>Removes control characters (char &lt;= 32) from both
        /// ends of this String returning <code>null</code> if the String is
        /// empty ("") after the trim or if it is <code>null</code>.
        /// 
        /// <p>The String is trimmed using <seealso cref="String#Trim()"/>.
        /// Trim removes start and end characters &lt;= 32.
        /// To strip whitespace use <seealso cref="#stripToNull(String)"/>.</p>
        /// 
        /// <pre>
        /// StringUtil.trimToNull(null)          = null
        /// StringUtil.trimToNull("")            = null
        /// StringUtil.trimToNull("     ")       = null
        /// StringUtil.trimToNull("abc")         = "abc"
        /// StringUtil.trimToNull("    abc    ") = "abc"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to be trimmed, may be null </param>
        /// <returns> the trimmed String,
        ///  <code>null</code> if only chars &lt;= 32, empty or null String input
        /// @since 2.0 </returns>
        public static string TrimToNull(string str)
        {
            string ts = Trim(str);
            return IsEmpty(ts) ? null : ts;
        }

        /// <summary>
        /// <p>Removes control characters (char &lt;= 32) from both
        /// ends of this String returning an empty String ("") if the String
        /// is empty ("") after the trim or if it is <code>null</code>.
        /// 
        /// <p>The String is trimmed using <seealso cref="String#Trim()"/>.
        /// Trim removes start and end characters &lt;= 32.
        /// To strip whitespace use <seealso cref="#stripToEmpty(String)"/>.</p>
        /// 
        /// <pre>
        /// StringUtil.trimToEmpty(null)          = ""
        /// StringUtil.trimToEmpty("")            = ""
        /// StringUtil.trimToEmpty("     ")       = ""
        /// StringUtil.trimToEmpty("abc")         = "abc"
        /// StringUtil.trimToEmpty("    abc    ") = "abc"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to be trimmed, may be null </param>
        /// <returns> the trimmed String, or an empty String if <code>null</code> input
        /// @since 2.0 </returns>
        public static string TrimToEmpty(string str)
        {
            return str == null ? EMPTY : str.Trim();
        }

        // Stripping
        //-----------------------------------------------------------------------
        /// <summary>
        /// <p>Strips whitespace from the start and end of a String.</p>
        /// 
        /// <p>This is similar to <seealso cref="#Trim(String)"/> but removes whitespace.
        /// Whitespace is defined by <seealso cref="Character#isWhitespace(char)"/>.</p>
        /// 
        /// <p>A <code>null</code> input String returns <code>null</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.Strip(null)     = null
        /// StringUtil.Strip("")       = ""
        /// StringUtil.Strip("   ")    = ""
        /// StringUtil.Strip("abc")    = "abc"
        /// StringUtil.Strip("  abc")  = "abc"
        /// StringUtil.Strip("abc  ")  = "abc"
        /// StringUtil.Strip(" abc ")  = "abc"
        /// StringUtil.Strip(" ab c ") = "ab c"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to remove whitespace from, may be null </param>
        /// <returns> the stripped String, <code>null</code> if null String input </returns>
        public static string Strip(string str)
        {
            return Strip(str, null);
        }

        /// <summary>
        /// <p>Strips whitespace from the start and end of a String  returning
        /// <code>null</code> if the String is empty ("") after the strip.</p>
        /// 
        /// <p>This is similar to <seealso cref="#trimToNull(String)"/> but removes whitespace.
        /// Whitespace is defined by <seealso cref="Character#isWhitespace(char)"/>.</p>
        /// 
        /// <pre>
        /// StringUtil.stripToNull(null)     = null
        /// StringUtil.stripToNull("")       = null
        /// StringUtil.stripToNull("   ")    = null
        /// StringUtil.stripToNull("abc")    = "abc"
        /// StringUtil.stripToNull("  abc")  = "abc"
        /// StringUtil.stripToNull("abc  ")  = "abc"
        /// StringUtil.stripToNull(" abc ")  = "abc"
        /// StringUtil.stripToNull(" ab c ") = "ab c"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to be stripped, may be null </param>
        /// <returns> the stripped String,
        ///  <code>null</code> if whitespace, empty or null String input
        /// @since 2.0 </returns>
        public static string StripToNull(string str)
        {
            if (str == null)
            {
                return null;
            }
            str = Strip(str, null);
            return str.Length == 0 ? null : str;
        }

        /// <summary>
        /// <p>Strips whitespace from the start and end of a String  returning
        /// an empty String if <code>null</code> input.</p>
        /// 
        /// <p>This is similar to <seealso cref="#trimToEmpty(String)"/> but removes whitespace.
        /// Whitespace is defined by <seealso cref="Character#isWhitespace(char)"/>.</p>
        /// 
        /// <pre>
        /// StringUtil.stripToEmpty(null)     = ""
        /// StringUtil.stripToEmpty("")       = ""
        /// StringUtil.stripToEmpty("   ")    = ""
        /// StringUtil.stripToEmpty("abc")    = "abc"
        /// StringUtil.stripToEmpty("  abc")  = "abc"
        /// StringUtil.stripToEmpty("abc  ")  = "abc"
        /// StringUtil.stripToEmpty(" abc ")  = "abc"
        /// StringUtil.stripToEmpty(" ab c ") = "ab c"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to be stripped, may be null </param>
        /// <returns> the trimmed String, or an empty String if <code>null</code> input
        /// @since 2.0 </returns>
        public static string StripToEmpty(string str)
        {
            return str == null ? EMPTY : Strip(str, null);
        }

        /// <summary>
        /// <p>Strips any of a set of characters from the start and end of a String.
        /// This is similar to <seealso cref="String#Trim()"/> but allows the characters
        /// to be stripped to be controlled.</p>
        /// 
        /// <p>A <code>null</code> input String returns <code>null</code>.
        /// An empty string ("") input returns the empty string.</p>
        /// 
        /// <p>If the stripChars String is <code>null</code>, whitespace is
        /// stripped as defined by <seealso cref="Character#isWhitespace(char)"/>.
        /// Alternatively use <seealso cref="#Strip(String)"/>.</p>
        /// 
        /// <pre>
        /// StringUtil.Strip(null, *)          = null
        /// StringUtil.Strip("", *)            = ""
        /// StringUtil.Strip("abc", null)      = "abc"
        /// StringUtil.Strip("  abc", null)    = "abc"
        /// StringUtil.Strip("abc  ", null)    = "abc"
        /// StringUtil.Strip(" abc ", null)    = "abc"
        /// StringUtil.Strip("  abcyx", "xyz") = "  abc"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to remove characters from, may be null </param>
        /// <param name="stripChars">  the characters to remove, null treated as whitespace </param>
        /// <returns> the stripped String, <code>null</code> if null String input </returns>
        public static string Strip(string str, string stripChars)
        {
            if (IsEmpty(str))
            {
                return str;
            }
            str = StripStart(str, stripChars);
            return StripEnd(str, stripChars);
        }

        /// <summary>
        /// <p>Strips any of a set of characters from the start of a String.</p>
        /// 
        /// <p>A <code>null</code> input String returns <code>null</code>.
        /// An empty string ("") input returns the empty string.</p>
        /// 
        /// <p>If the stripChars String is <code>null</code>, whitespace is
        /// stripped as defined by <seealso cref="Character#isWhitespace(char)"/>.</p>
        /// 
        /// <pre>
        /// StringUtil.StripStart(null, *)          = null
        /// StringUtil.StripStart("", *)            = ""
        /// StringUtil.StripStart("abc", "")        = "abc"
        /// StringUtil.StripStart("abc", null)      = "abc"
        /// StringUtil.StripStart("  abc", null)    = "abc"
        /// StringUtil.StripStart("abc  ", null)    = "abc  "
        /// StringUtil.StripStart(" abc ", null)    = "abc "
        /// StringUtil.StripStart("yxabc  ", "xyz") = "abc  "
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to remove characters from, may be null </param>
        /// <param name="stripChars">  the characters to remove, null treated as whitespace </param>
        /// <returns> the stripped String, <code>null</code> if null String input </returns>
        public static string StripStart(string str, string stripChars)
        {
            int strLen;
            if (str == null || (strLen = str.Length) == 0)
            {
                return str;
            }
            int start = 0;
            if (stripChars == null)
            {
                while ((start != strLen) && char.IsWhiteSpace(str[start]))
                {
                    start++;
                }
            }
            else if (stripChars.Length == 0)
            {
                return str;
            }
            else
            {
                while ((start != strLen) && (stripChars.IndexOf(str[start]) != -1))
                {
                    start++;
                }
            }
            return str.Substring(start);
        }

        /// <summary>
        /// <p>Strips any of a set of characters from the end of a String.</p>
        /// 
        /// <p>A <code>null</code> input String returns <code>null</code>.
        /// An empty string ("") input returns the empty string.</p>
        /// 
        /// <p>If the stripChars String is <code>null</code>, whitespace is
        /// stripped as defined by <seealso cref="Character#isWhitespace(char)"/>.</p>
        /// 
        /// <pre>
        /// StringUtil.StripEnd(null, *)          = null
        /// StringUtil.StripEnd("", *)            = ""
        /// StringUtil.StripEnd("abc", "")        = "abc"
        /// StringUtil.StripEnd("abc", null)      = "abc"
        /// StringUtil.StripEnd("  abc", null)    = "  abc"
        /// StringUtil.StripEnd("abc  ", null)    = "abc"
        /// StringUtil.StripEnd(" abc ", null)    = " abc"
        /// StringUtil.StripEnd("  abcyx", "xyz") = "  abc"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to remove characters from, may be null </param>
        /// <param name="stripChars">  the characters to remove, null treated as whitespace </param>
        /// <returns> the stripped String, <code>null</code> if null String input </returns>
        public static string StripEnd(string str, string stripChars)
        {
            int end;
            if (str == null || (end = str.Length) == 0)
            {
                return str;
            }

            if (stripChars == null)
            {
                while ((end != 0) && char.IsWhiteSpace(str[end - 1]))
                {
                    end--;
                }
            }
            else if (stripChars.Length == 0)
            {
                return str;
            }
            else
            {
                while ((end != 0) && (stripChars.IndexOf(str[end - 1]) != -1))
                {
                    end--;
                }
            }
            return str.Substring(0, end);
        }

        // StripAll
        //-----------------------------------------------------------------------
        /// <summary>
        /// <p>Strips whitespace from the start and end of every String in an array.
        /// Whitespace is defined by <seealso cref="Character#isWhitespace(char)"/>.</p>
        /// 
        /// <p>A new array is returned each time, except for length zero.
        /// A <code>null</code> array will return <code>null</code>.
        /// An empty array will return itself.
        /// A <code>null</code> array entry will be ignored.</p>
        /// 
        /// <pre>
        /// StringUtil.StripAll(null)             = null
        /// StringUtil.StripAll([])               = []
        /// StringUtil.StripAll(["abc", "  abc"]) = ["abc", "abc"]
        /// StringUtil.StripAll(["abc  ", null])  = ["abc", null]
        /// </pre>
        /// </summary>
        /// <param name="strs">  the array to remove whitespace from, may be null </param>
        /// <returns> the stripped Strings, <code>null</code> if null array input </returns>
        public static string[] StripAll(string[] strs)
        {
            return StripAll(strs, null);
        }

        /// <summary>
        /// <p>Strips any of a set of characters from the start and end of every
        /// String in an array.</p>
        /// Whitespace is defined by <seealso cref="Character#isWhitespace(char)"/>.</p>
        /// 
        /// <p>A new array is returned each time, except for length zero.
        /// A <code>null</code> array will return <code>null</code>.
        /// An empty array will return itself.
        /// A <code>null</code> array entry will be ignored.
        /// A <code>null</code> stripChars will strip whitespace as defined by
        /// <seealso cref="Character#isWhitespace(char)"/>.</p>
        /// 
        /// <pre>
        /// StringUtil.StripAll(null, *)                = null
        /// StringUtil.StripAll([], *)                  = []
        /// StringUtil.StripAll(["abc", "  abc"], null) = ["abc", "abc"]
        /// StringUtil.StripAll(["abc  ", null], null)  = ["abc", null]
        /// StringUtil.StripAll(["abc  ", null], "yz")  = ["abc  ", null]
        /// StringUtil.StripAll(["yabcz", null], "yz")  = ["abc", null]
        /// </pre>
        /// </summary>
        /// <param name="strs">  the array to remove characters from, may be null </param>
        /// <param name="stripChars">  the characters to remove, null treated as whitespace </param>
        /// <returns> the stripped Strings, <code>null</code> if null array input </returns>
        public static string[] StripAll(string[] strs, string stripChars)
        {
            int strsLen;
            if (strs == null || (strsLen = strs.Length) == 0)
            {
                return strs;
            }
            string[] newArr = new string[strsLen];
            for (int i = 0; i < strsLen; i++)
            {
                newArr[i] = Strip(strs[i], stripChars);
            }
            return newArr;
        }

        // Equals
        //-----------------------------------------------------------------------
        /// <summary>
        /// <p>Compares two Strings, returning <code>true</code> if they are equal.</p>
        /// 
        /// <p><code>null</code>s are handled without exceptions. Two <code>null</code>
        /// references are considered to be equal. The comparison is case sensitive.</p>
        /// 
        /// <pre>
        /// StringUtil.Equals(null, null)   = true
        /// StringUtil.Equals(null, "abc")  = false
        /// StringUtil.Equals("abc", null)  = false
        /// StringUtil.Equals("abc", "abc") = true
        /// StringUtil.Equals("abc", "ABC") = false
        /// </pre>
        /// </summary>
        /// <seealso cref= java.lang.String#equals(Object) </seealso>
        /// <param name="str1">  the first String, may be null </param>
        /// <param name="str2">  the second String, may be null </param>
        /// <returns> <code>true</code> if the Strings are equal, case sensitive, or
        ///  both <code>null</code> </returns>
        public static bool Equals(string str1, string str2)
        {
            return str1 == null ? str2 == null : str1.Equals(str2);
        }

        /// <summary>
        /// <p>Compares two Strings, returning <code>true</code> if they are equal ignoring
        /// the case.</p>
        /// 
        /// <p><code>null</code>s are handled without exceptions. Two <code>null</code>
        /// references are considered equal. Comparison is case insensitive.</p>
        /// 
        /// <pre>
        /// StringUtil.EqualsIgnoreCase(null, null)   = true
        /// StringUtil.EqualsIgnoreCase(null, "abc")  = false
        /// StringUtil.EqualsIgnoreCase("abc", null)  = false
        /// StringUtil.EqualsIgnoreCase("abc", "abc") = true
        /// StringUtil.EqualsIgnoreCase("abc", "ABC") = true
        /// </pre>
        /// </summary>
        /// <seealso cref= java.lang.String#EqualsIgnoreCase(String) </seealso>
        /// <param name="str1">  the first String, may be null </param>
        /// <param name="str2">  the second String, may be null </param>
        /// <returns> <code>true</code> if the Strings are equal, case insensitive, or
        ///  both <code>null</code> </returns>
        public static bool EqualsIgnoreCase(string str1, string str2)
        {
            return str1 == null ? str2 == null : str1.ToUpper() == str2.ToUpper();
        }

        // IndexOf
        //-----------------------------------------------------------------------
        /// <summary>
        /// <p>Finds the first index within a String, handling <code>null</code>.
        /// This method uses <seealso cref="String#IndexOf(int)"/>.</p>
        /// 
        /// <p>A <code>null</code> or empty ("") String will return <code>-1</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.IndexOf(null, *)         = -1
        /// StringUtil.IndexOf("", *)           = -1
        /// StringUtil.IndexOf("aabaabaa", 'a') = 0
        /// StringUtil.IndexOf("aabaabaa", 'b') = 2
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="searchChar">  the character to find </param>
        /// <returns> the first index of the search character,
        ///  -1 if no match or <code>null</code> string input
        /// @since 2.0 </returns>
        public static int IndexOf(string str, char searchChar)
        {
            if (IsEmpty(str))
            {
                return -1;
            }
            return str.IndexOf(searchChar);
        }

        /// <summary>
        /// <p>Finds the first index within a String from a start position,
        /// handling <code>null</code>.
        /// This method uses <seealso cref="String#IndexOf(int, int)"/>.</p>
        /// 
        /// <p>A <code>null</code> or empty ("") String will return <code>-1</code>.
        /// A negative start position is treated as zero.
        /// A start position greater than the string length returns <code>-1</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.IndexOf(null, *, *)          = -1
        /// StringUtil.IndexOf("", *, *)            = -1
        /// StringUtil.IndexOf("aabaabaa", 'b', 0)  = 2
        /// StringUtil.IndexOf("aabaabaa", 'b', 3)  = 5
        /// StringUtil.IndexOf("aabaabaa", 'b', 9)  = -1
        /// StringUtil.IndexOf("aabaabaa", 'b', -1) = 2
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="searchChar">  the character to find </param>
        /// <param name="startPos">  the start position, negative treated as zero </param>
        /// <returns> the first index of the search character,
        ///  -1 if no match or <code>null</code> string input
        /// @since 2.0 </returns>
        public static int IndexOf(string str, char searchChar, int startPos)
        {
            if (IsEmpty(str))
            {
                return -1;
            }
            return str.IndexOf(searchChar, startPos);
        }

        /// <summary>
        /// <p>Finds the first index within a String, handling <code>null</code>.
        /// This method uses <seealso cref="String#IndexOf(String)"/>.</p>
        /// 
        /// <p>A <code>null</code> String will return <code>-1</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.IndexOf(null, *)          = -1
        /// StringUtil.IndexOf(*, null)          = -1
        /// StringUtil.IndexOf("", "")           = 0
        /// StringUtil.IndexOf("aabaabaa", "a")  = 0
        /// StringUtil.IndexOf("aabaabaa", "b")  = 2
        /// StringUtil.IndexOf("aabaabaa", "ab") = 1
        /// StringUtil.IndexOf("aabaabaa", "")   = 0
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="searchStr">  the String to find, may be null </param>
        /// <returns> the first index of the search String,
        ///  -1 if no match or <code>null</code> string input
        /// @since 2.0 </returns>
        public static int IndexOf(string str, string searchStr)
        {
            if (str == null || searchStr == null)
            {
                return -1;
            }
            return str.IndexOf(searchStr);
        }

        /// <summary>
        /// <p>Finds the n-th index within a String, handling <code>null</code>.
        /// This method uses <seealso cref="String#IndexOf(String)"/>.</p>
        /// 
        /// <p>A <code>null</code> String will return <code>-1</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.ordinalIndexOf(null, *, *)          = -1
        /// StringUtil.ordinalIndexOf(*, null, *)          = -1
        /// StringUtil.ordinalIndexOf("", "", *)           = 0
        /// StringUtil.ordinalIndexOf("aabaabaa", "a", 1)  = 0
        /// StringUtil.ordinalIndexOf("aabaabaa", "a", 2)  = 1
        /// StringUtil.ordinalIndexOf("aabaabaa", "b", 1)  = 2
        /// StringUtil.ordinalIndexOf("aabaabaa", "b", 2)  = 5
        /// StringUtil.ordinalIndexOf("aabaabaa", "ab", 1) = 1
        /// StringUtil.ordinalIndexOf("aabaabaa", "ab", 2) = 4
        /// StringUtil.ordinalIndexOf("aabaabaa", "", 1)   = 0
        /// StringUtil.ordinalIndexOf("aabaabaa", "", 2)   = 0
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="searchStr">  the String to find, may be null </param>
        /// <param name="ordinal">  the n-th <code>searchStr</code> to find </param>
        /// <returns> the n-th index of the search String,
        ///  <code>-1</code> (<code>INDEX_NOT_FOUND</code>) if no match or <code>null</code> string input
        /// @since 2.1 </returns>
        public static int OrdinalIndexOf(string str, string searchStr, int ordinal)
        {
            if (str == null || searchStr == null || ordinal <= 0)
            {
                return INDEX_NOT_FOUND;
            }
            if (searchStr.Length == 0)
            {
                return 0;
            }
            int found = 0;
            int index = INDEX_NOT_FOUND;
            do
            {
                index = str.IndexOf(searchStr, index + 1);
                if (index < 0)
                {
                    return index;
                }
                found++;
            } while (found < ordinal);
            return index;
        }

        /// <summary>
        /// <p>Finds the first index within a String, handling <code>null</code>.
        /// This method uses <seealso cref="String#IndexOf(String, int)"/>.</p>
        /// 
        /// <p>A <code>null</code> String will return <code>-1</code>.
        /// A negative start position is treated as zero.
        /// An empty ("") search String always matches.
        /// A start position greater than the string length only matches
        /// an empty search String.</p>
        /// 
        /// <pre>
        /// StringUtil.IndexOf(null, *, *)          = -1
        /// StringUtil.IndexOf(*, null, *)          = -1
        /// StringUtil.IndexOf("", "", 0)           = 0
        /// StringUtil.IndexOf("aabaabaa", "a", 0)  = 0
        /// StringUtil.IndexOf("aabaabaa", "b", 0)  = 2
        /// StringUtil.IndexOf("aabaabaa", "ab", 0) = 1
        /// StringUtil.IndexOf("aabaabaa", "b", 3)  = 5
        /// StringUtil.IndexOf("aabaabaa", "b", 9)  = -1
        /// StringUtil.IndexOf("aabaabaa", "b", -1) = 2
        /// StringUtil.IndexOf("aabaabaa", "", 2)   = 2
        /// StringUtil.IndexOf("abc", "", 9)        = 3
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="searchStr">  the String to find, may be null </param>
        /// <param name="startPos">  the start position, negative treated as zero </param>
        /// <returns> the first index of the search String,
        ///  -1 if no match or <code>null</code> string input
        /// @since 2.0 </returns>
        public static int IndexOf(string str, string searchStr, int startPos)
        {
            if (str == null || searchStr == null)
            {
                return -1;
            }
            // JDK1.2/JDK1.3 have a bug, when startPos > str.Length for "", hence
            if (searchStr.Length == 0 && startPos >= str.Length)
            {
                return str.Length;
            }
            return str.IndexOf(searchStr, startPos);
        }

        // LastIndexOf
        //-----------------------------------------------------------------------
        /// <summary>
        /// <p>Finds the last index within a String, handling <code>null</code>.
        /// This method uses <seealso cref="String#LastIndexOf(int)"/>.</p>
        /// 
        /// <p>A <code>null</code> or empty ("") String will return <code>-1</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.LastIndexOf(null, *)         = -1
        /// StringUtil.LastIndexOf("", *)           = -1
        /// StringUtil.LastIndexOf("aabaabaa", 'a') = 7
        /// StringUtil.LastIndexOf("aabaabaa", 'b') = 5
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="searchChar">  the character to find </param>
        /// <returns> the last index of the search character,
        ///  -1 if no match or <code>null</code> string input
        /// @since 2.0 </returns>
        public static int LastIndexOf(string str, char searchChar)
        {
            if (IsEmpty(str))
            {
                return -1;
            }
            return str.LastIndexOf(searchChar);
        }

        /// <summary>
        /// <p>Finds the last index within a String from a start position,
        /// handling <code>null</code>.
        /// This method uses <seealso cref="String#LastIndexOf(int, int)"/>.</p>
        /// 
        /// <p>A <code>null</code> or empty ("") String will return <code>-1</code>.
        /// A negative start position returns <code>-1</code>.
        /// A start position greater than the string length searches the whole string.</p>
        /// 
        /// <pre>
        /// StringUtil.LastIndexOf(null, *, *)          = -1
        /// StringUtil.LastIndexOf("", *,  *)           = -1
        /// StringUtil.LastIndexOf("aabaabaa", 'b', 8)  = 5
        /// StringUtil.LastIndexOf("aabaabaa", 'b', 4)  = 2
        /// StringUtil.LastIndexOf("aabaabaa", 'b', 0)  = -1
        /// StringUtil.LastIndexOf("aabaabaa", 'b', 9)  = 5
        /// StringUtil.LastIndexOf("aabaabaa", 'b', -1) = -1
        /// StringUtil.LastIndexOf("aabaabaa", 'a', 0)  = 0
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="searchChar">  the character to find </param>
        /// <param name="startPos">  the start position </param>
        /// <returns> the last index of the search character,
        ///  -1 if no match or <code>null</code> string input
        /// @since 2.0 </returns>
        public static int LastIndexOf(string str, char searchChar, int startPos)
        {
            if (IsEmpty(str))
            {
                return -1;
            }
            return str.LastIndexOf(searchChar, startPos);
        }

        /// <summary>
        /// <p>Finds the last index within a String, handling <code>null</code>.
        /// This method uses <seealso cref="String#LastIndexOf(String)"/>.</p>
        /// 
        /// <p>A <code>null</code> String will return <code>-1</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.LastIndexOf(null, *)          = -1
        /// StringUtil.LastIndexOf(*, null)          = -1
        /// StringUtil.LastIndexOf("", "")           = 0
        /// StringUtil.LastIndexOf("aabaabaa", "a")  = 0
        /// StringUtil.LastIndexOf("aabaabaa", "b")  = 2
        /// StringUtil.LastIndexOf("aabaabaa", "ab") = 1
        /// StringUtil.LastIndexOf("aabaabaa", "")   = 8
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="searchStr">  the String to find, may be null </param>
        /// <returns> the last index of the search String,
        ///  -1 if no match or <code>null</code> string input
        /// @since 2.0 </returns>
        public static int LastIndexOf(string str, string searchStr)
        {
            if (str == null || searchStr == null)
            {
                return -1;
            }
            return str.LastIndexOf(searchStr);
        }

        /// <summary>
        /// <p>Finds the first index within a String, handling <code>null</code>.
        /// This method uses <seealso cref="String#LastIndexOf(String, int)"/>.</p>
        /// 
        /// <p>A <code>null</code> String will return <code>-1</code>.
        /// A negative start position returns <code>-1</code>.
        /// An empty ("") search String always matches unless the start position is negative.
        /// A start position greater than the string length searches the whole string.</p>
        /// 
        /// <pre>
        /// StringUtil.LastIndexOf(null, *, *)          = -1
        /// StringUtil.LastIndexOf(*, null, *)          = -1
        /// StringUtil.LastIndexOf("aabaabaa", "a", 8)  = 7
        /// StringUtil.LastIndexOf("aabaabaa", "b", 8)  = 5
        /// StringUtil.LastIndexOf("aabaabaa", "ab", 8) = 4
        /// StringUtil.LastIndexOf("aabaabaa", "b", 9)  = 5
        /// StringUtil.LastIndexOf("aabaabaa", "b", -1) = -1
        /// StringUtil.LastIndexOf("aabaabaa", "a", 0)  = 0
        /// StringUtil.LastIndexOf("aabaabaa", "b", 0)  = -1
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="searchStr">  the String to find, may be null </param>
        /// <param name="startPos">  the start position, negative treated as zero </param>
        /// <returns> the first index of the search String,
        ///  -1 if no match or <code>null</code> string input
        /// @since 2.0 </returns>
        public static int LastIndexOf(string str, string searchStr, int startPos)
        {
            if (str == null || searchStr == null)
            {
                return -1;
            }
            return str.LastIndexOf(searchStr, startPos);
        }

        // Contains
        //-----------------------------------------------------------------------
        /// <summary>
        /// <p>Checks if String contains a search character, handling <code>null</code>.
        /// This method uses <seealso cref="String#IndexOf(int)"/>.</p>
        /// 
        /// <p>A <code>null</code> or empty ("") String will return <code>false</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.Contains(null, *)    = false
        /// StringUtil.Contains("", *)      = false
        /// StringUtil.Contains("abc", 'a') = true
        /// StringUtil.Contains("abc", 'z') = false
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="searchChar">  the character to find </param>
        /// <returns> true if the String contains the search character,
        ///  false if not or <code>null</code> string input
        /// @since 2.0 </returns>
        public static bool Contains(string str, char searchChar)
        {
            if (IsEmpty(str))
            {
                return false;
            }
            return str.IndexOf(searchChar) >= 0;
        }

        /// <summary>
        /// <p>Checks if String contains a search String, handling <code>null</code>.
        /// This method uses <seealso cref="String#IndexOf(String)"/>.</p>
        /// 
        /// <p>A <code>null</code> String will return <code>false</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.Contains(null, *)     = false
        /// StringUtil.Contains(*, null)     = false
        /// StringUtil.Contains("", "")      = true
        /// StringUtil.Contains("abc", "")   = true
        /// StringUtil.Contains("abc", "a")  = true
        /// StringUtil.Contains("abc", "z")  = false
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="searchStr">  the String to find, may be null </param>
        /// <returns> true if the String contains the search String,
        ///  false if not or <code>null</code> string input
        /// @since 2.0 </returns>
        public static bool Contains(string str, string searchStr)
        {
            if (str == null || searchStr == null)
            {
                return false;
            }
            return str.IndexOf(searchStr) >= 0;
        }

        /// <summary>
        /// <p>Checks if String contains a search String irrespective of case,
        /// handling <code>null</code>. This method uses
        /// <seealso cref="#Contains(String, String)"/>.</p>
        /// 
        /// <p>A <code>null</code> String will return <code>false</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.Contains(null, *) = false
        /// StringUtil.Contains(*, null) = false
        /// StringUtil.Contains("", "") = true
        /// StringUtil.Contains("abc", "") = true
        /// StringUtil.Contains("abc", "a") = true
        /// StringUtil.Contains("abc", "z") = false
        /// StringUtil.Contains("abc", "A") = true
        /// StringUtil.Contains("abc", "Z") = false
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="searchStr">  the String to find, may be null </param>
        /// <returns> true if the String contains the search String irrespective of
        /// case or false if not or <code>null</code> string input </returns>
        public static bool ContainsIgnoreCase(string str, string searchStr)
        {
            if (str == null || searchStr == null)
            {
                return false;
            }
            return Contains(str.ToUpper(), searchStr.ToUpper());
        }

        // IndexOfAny chars
        //-----------------------------------------------------------------------
        /// <summary>
        /// <p>Search a String to find the first index of any
        /// character in the given set of characters.</p>
        /// 
        /// <p>A <code>null</code> String will return <code>-1</code>.
        /// A <code>null</code> or zero length search array will return <code>-1</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.IndexOfAny(null, *)                = -1
        /// StringUtil.IndexOfAny("", *)                  = -1
        /// StringUtil.IndexOfAny(*, null)                = -1
        /// StringUtil.IndexOfAny(*, [])                  = -1
        /// StringUtil.IndexOfAny("zzabyycdxx",['z','a']) = 0
        /// StringUtil.IndexOfAny("zzabyycdxx",['b','y']) = 3
        /// StringUtil.IndexOfAny("aba", ['z'])           = -1
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="searchChars">  the chars to search for, may be null </param>
        /// <returns> the index of any of the chars, -1 if no match or null input
        /// @since 2.0 </returns>
        public static int IndexOfAny(string str, char[] searchChars)
        {
            if (IsEmpty(str) || IsArrayEmpty(searchChars))
            {
                return -1;
            }
            for (int i = 0; i < str.Length; i++)
            {
                char ch = str[i];
                for (int j = 0; j < searchChars.Length; j++)
                {
                    if (searchChars[j] == ch)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// <p>Search a String to find the first index of any
        /// character in the given set of characters.</p>
        /// 
        /// <p>A <code>null</code> String will return <code>-1</code>.
        /// A <code>null</code> search string will return <code>-1</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.IndexOfAny(null, *)            = -1
        /// StringUtil.IndexOfAny("", *)              = -1
        /// StringUtil.IndexOfAny(*, null)            = -1
        /// StringUtil.IndexOfAny(*, "")              = -1
        /// StringUtil.IndexOfAny("zzabyycdxx", "za") = 0
        /// StringUtil.IndexOfAny("zzabyycdxx", "by") = 3
        /// StringUtil.IndexOfAny("aba","z")          = -1
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="searchChars">  the chars to search for, may be null </param>
        /// <returns> the index of any of the chars, -1 if no match or null input
        /// @since 2.0 </returns>
        public static int IndexOfAny(string str, string searchChars)
        {
            if (IsEmpty(str) || IsEmpty(searchChars))
            {
                return -1;
            }
            return IndexOfAny(str, searchChars.ToCharArray());
        }

        // ContainsAny
        //-----------------------------------------------------------------------
        /// <summary>
        /// <p>Checks if the String contains any character in the given
        /// set of characters.</p>
        /// 
        /// <p>A <code>null</code> String will return <code>false</code>.
        /// A <code>null</code> or zero length search array will return <code>false</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.ContainsAny(null, *)                = false
        /// StringUtil.ContainsAny("", *)                  = false
        /// StringUtil.ContainsAny(*, null)                = false
        /// StringUtil.ContainsAny(*, [])                  = false
        /// StringUtil.ContainsAny("zzabyycdxx",['z','a']) = true
        /// StringUtil.ContainsAny("zzabyycdxx",['b','y']) = true
        /// StringUtil.ContainsAny("aba", ['z'])           = false
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="searchChars">  the chars to search for, may be null </param>
        /// <returns> the <code>true</code> if any of the chars are found,
        /// <code>false</code> if no match or null input
        /// @since 2.4 </returns>
        public static bool ContainsAny(string str, char[] searchChars)
        {
            if (str == null || str.Length == 0 || searchChars == null || searchChars.Length == 0)
            {
                return false;
            }
            for (int i = 0; i < str.Length; i++)
            {
                char ch = str[i];
                for (int j = 0; j < searchChars.Length; j++)
                {
                    if (searchChars[j] == ch)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// <p>
        /// Checks if the String contains any character in the given set of characters.
        /// </p>
        /// 
        /// <p>
        /// A <code>null</code> String will return <code>false</code>. A <code>null</code> search string will return
        /// <code>false</code>.
        /// </p>
        /// 
        /// <pre>
        /// StringUtil.ContainsAny(null, *)            = false
        /// StringUtil.ContainsAny("", *)              = false
        /// StringUtil.ContainsAny(*, null)            = false
        /// StringUtil.ContainsAny(*, "")              = false
        /// StringUtil.ContainsAny("zzabyycdxx", "za") = true
        /// StringUtil.ContainsAny("zzabyycdxx", "by") = true
        /// StringUtil.ContainsAny("aba","z")          = false
        /// </pre>
        /// </summary>
        /// <param name="str">
        ///            the String to check, may be null </param>
        /// <param name="searchChars">
        ///            the chars to search for, may be null </param>
        /// <returns> the <code>true</code> if any of the chars are found, <code>false</code> if no match or null input
        /// @since 2.4 </returns>
        public static bool ContainsAny(string str, string searchChars)
        {
            if (searchChars == null)
            {
                return false;
            }
            return ContainsAny(str, searchChars.ToCharArray());
        }

        // IndexOfAnyBut chars
        //-----------------------------------------------------------------------
        /// <summary>
        /// <p>Search a String to find the first index of any
        /// character not in the given set of characters.</p>
        /// 
        /// <p>A <code>null</code> String will return <code>-1</code>.
        /// A <code>null</code> or zero length search array will return <code>-1</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.IndexOfAnyBut(null, *)           = -1
        /// StringUtil.IndexOfAnyBut("", *)             = -1
        /// StringUtil.IndexOfAnyBut(*, null)           = -1
        /// StringUtil.IndexOfAnyBut(*, [])             = -1
        /// StringUtil.IndexOfAnyBut("zzabyycdxx",'za') = 3
        /// StringUtil.IndexOfAnyBut("zzabyycdxx", '')  = 0
        /// StringUtil.IndexOfAnyBut("aba", 'ab')       = -1
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="searchChars">  the chars to search for, may be null </param>
        /// <returns> the index of any of the chars, -1 if no match or null input
        /// @since 2.0 </returns>
        public static int IndexOfAnyBut(string str, char[] searchChars)
        {
            if (IsEmpty(str) || IsArrayEmpty(searchChars))
            {
                return -1;
            }
        outer:
            for (int i = 0; i < str.Length; i++)
            {
                char ch = str[i];
                for (int j = 0; j < searchChars.Length; j++)
                {
                    if (searchChars[j] == ch)
                    {
                        goto outer;
                    }
                }
                return i;
            }
            return -1;
        }

        /// <summary>
        /// <p>Search a String to find the first index of any
        /// character not in the given set of characters.</p>
        /// 
        /// <p>A <code>null</code> String will return <code>-1</code>.
        /// A <code>null</code> search string will return <code>-1</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.IndexOfAnyBut(null, *)            = -1
        /// StringUtil.IndexOfAnyBut("", *)              = -1
        /// StringUtil.IndexOfAnyBut(*, null)            = -1
        /// StringUtil.IndexOfAnyBut(*, "")              = -1
        /// StringUtil.IndexOfAnyBut("zzabyycdxx", "za") = 3
        /// StringUtil.IndexOfAnyBut("zzabyycdxx", "")   = 0
        /// StringUtil.IndexOfAnyBut("aba","ab")         = -1
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="searchChars">  the chars to search for, may be null </param>
        /// <returns> the index of any of the chars, -1 if no match or null input
        /// @since 2.0 </returns>
        public static int IndexOfAnyBut(string str, string searchChars)
        {
            if (IsEmpty(str) || IsEmpty(searchChars))
            {
                return -1;
            }
            for (int i = 0; i < str.Length; i++)
            {
                if (searchChars.IndexOf(str[i]) < 0)
                {
                    return i;
                }
            }
            return -1;
        }

        // ContainsOnly
        //-----------------------------------------------------------------------
        /// <summary>
        /// <p>Checks if the String contains only certain characters.</p>
        /// 
        /// <p>A <code>null</code> String will return <code>false</code>.
        /// A <code>null</code> valid character array will return <code>false</code>.
        /// An empty String ("") always returns <code>true</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.ContainsOnly(null, *)       = false
        /// StringUtil.ContainsOnly(*, null)       = false
        /// StringUtil.ContainsOnly("", *)         = true
        /// StringUtil.ContainsOnly("ab", '')      = false
        /// StringUtil.ContainsOnly("abab", 'abc') = true
        /// StringUtil.ContainsOnly("ab1", 'abc')  = false
        /// StringUtil.ContainsOnly("abz", 'abc')  = false
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="valid">  an array of valid chars, may be null </param>
        /// <returns> true if it only contains valid chars and is non-null </returns>
        public static bool ContainsOnly(string str, char[] valid)
        {
            // All these pre-checks are to maintain API with an older version
            if ((valid == null) || (str == null))
            {
                return false;
            }
            if (str.Length == 0)
            {
                return true;
            }
            if (valid.Length == 0)
            {
                return false;
            }
            return IndexOfAnyBut(str, valid) == -1;
        }

        /// <summary>
        /// <p>Checks if the String contains only certain characters.</p>
        /// 
        /// <p>A <code>null</code> String will return <code>false</code>.
        /// A <code>null</code> valid character String will return <code>false</code>.
        /// An empty String ("") always returns <code>true</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.ContainsOnly(null, *)       = false
        /// StringUtil.ContainsOnly(*, null)       = false
        /// StringUtil.ContainsOnly("", *)         = true
        /// StringUtil.ContainsOnly("ab", "")      = false
        /// StringUtil.ContainsOnly("abab", "abc") = true
        /// StringUtil.ContainsOnly("ab1", "abc")  = false
        /// StringUtil.ContainsOnly("abz", "abc")  = false
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="validChars">  a String of valid chars, may be null </param>
        /// <returns> true if it only contains valid chars and is non-null
        /// @since 2.0 </returns>
        public static bool ContainsOnly(string str, string validChars)
        {
            if (str == null || validChars == null)
            {
                return false;
            }
            return ContainsOnly(str, validChars.ToCharArray());
        }

        // ContainsNone
        //-----------------------------------------------------------------------
        /// <summary>
        /// <p>Checks that the String does not contain certain characters.</p>
        /// 
        /// <p>A <code>null</code> String will return <code>true</code>.
        /// A <code>null</code> invalid character array will return <code>true</code>.
        /// An empty String ("") always returns true.</p>
        /// 
        /// <pre>
        /// StringUtil.ContainsNone(null, *)       = true
        /// StringUtil.ContainsNone(*, null)       = true
        /// StringUtil.ContainsNone("", *)         = true
        /// StringUtil.ContainsNone("ab", '')      = true
        /// StringUtil.ContainsNone("abab", 'xyz') = true
        /// StringUtil.ContainsNone("ab1", 'xyz')  = true
        /// StringUtil.ContainsNone("abz", 'xyz')  = false
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="invalidChars">  an array of invalid chars, may be null </param>
        /// <returns> true if it contains none of the invalid chars, or is null
        /// @since 2.0 </returns>
        public static bool ContainsNone(string str, char[] invalidChars)
        {
            if (str == null || invalidChars == null)
            {
                return true;
            }
            int strSize = str.Length;
            int validSize = invalidChars.Length;
            for (int i = 0; i < strSize; i++)
            {
                char ch = str[i];
                for (int j = 0; j < validSize; j++)
                {
                    if (invalidChars[j] == ch)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// <p>Checks that the String does not contain certain characters.</p>
        /// 
        /// <p>A <code>null</code> String will return <code>true</code>.
        /// A <code>null</code> invalid character array will return <code>true</code>.
        /// An empty String ("") always returns true.</p>
        /// 
        /// <pre>
        /// StringUtil.ContainsNone(null, *)       = true
        /// StringUtil.ContainsNone(*, null)       = true
        /// StringUtil.ContainsNone("", *)         = true
        /// StringUtil.ContainsNone("ab", "")      = true
        /// StringUtil.ContainsNone("abab", "xyz") = true
        /// StringUtil.ContainsNone("ab1", "xyz")  = true
        /// StringUtil.ContainsNone("abz", "xyz")  = false
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="invalidChars">  a String of invalid chars, may be null </param>
        /// <returns> true if it contains none of the invalid chars, or is null
        /// @since 2.0 </returns>
        public static bool ContainsNone(string str, string invalidChars)
        {
            if (str == null || invalidChars == null)
            {
                return true;
            }
            return ContainsNone(str, invalidChars.ToCharArray());
        }

        // IndexOfAny strings
        //-----------------------------------------------------------------------
        /// <summary>
        /// <p>Find the first index of any of a set of potential substrings.</p>
        /// 
        /// <p>A <code>null</code> String will return <code>-1</code>.
        /// A <code>null</code> or zero length search array will return <code>-1</code>.
        /// A <code>null</code> search array entry will be ignored, but a search
        /// array containing "" will return <code>0</code> if <code>str</code> is not
        /// null. This method uses <seealso cref="String#IndexOf(String)"/>.</p>
        /// 
        /// <pre>
        /// StringUtil.IndexOfAny(null, *)                     = -1
        /// StringUtil.IndexOfAny(*, null)                     = -1
        /// StringUtil.IndexOfAny(*, [])                       = -1
        /// StringUtil.IndexOfAny("zzabyycdxx", ["ab","cd"])   = 2
        /// StringUtil.IndexOfAny("zzabyycdxx", ["cd","ab"])   = 2
        /// StringUtil.IndexOfAny("zzabyycdxx", ["mn","op"])   = -1
        /// StringUtil.IndexOfAny("zzabyycdxx", ["zab","aby"]) = 1
        /// StringUtil.IndexOfAny("zzabyycdxx", [""])          = 0
        /// StringUtil.IndexOfAny("", [""])                    = 0
        /// StringUtil.IndexOfAny("", ["a"])                   = -1
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="searchStrs">  the Strings to search for, may be null </param>
        /// <returns> the first index of any of the searchStrs in str, -1 if no match </returns>
        public static int IndexOfAny(string str, string[] searchStrs)
        {
            if ((str == null) || (searchStrs == null))
            {
                return -1;
            }
            int sz = searchStrs.Length;

            // String's can't have a MAX_VALUEth index.
            int ret = int.MaxValue;

            int tmp = 0;
            for (int i = 0; i < sz; i++)
            {
                string search = searchStrs[i];
                if (search == null)
                {
                    continue;
                }
                tmp = str.IndexOf(search);
                if (tmp == -1)
                {
                    continue;
                }

                if (tmp < ret)
                {
                    ret = tmp;
                }
            }

            return (ret == int.MaxValue) ? -1 : ret;
        }

        /// <summary>
        /// <p>Find the latest index of any of a set of potential substrings.</p>
        /// 
        /// <p>A <code>null</code> String will return <code>-1</code>.
        /// A <code>null</code> search array will return <code>-1</code>.
        /// A <code>null</code> or zero length search array entry will be ignored,
        /// but a search array containing "" will return the length of <code>str</code>
        /// if <code>str</code> is not null. This method uses <seealso cref="String#IndexOf(String)"/></p>
        /// 
        /// <pre>
        /// StringUtil.lastIndexOfAny(null, *)                   = -1
        /// StringUtil.lastIndexOfAny(*, null)                   = -1
        /// StringUtil.lastIndexOfAny(*, [])                     = -1
        /// StringUtil.lastIndexOfAny(*, [null])                 = -1
        /// StringUtil.lastIndexOfAny("zzabyycdxx", ["ab","cd"]) = 6
        /// StringUtil.lastIndexOfAny("zzabyycdxx", ["cd","ab"]) = 6
        /// StringUtil.lastIndexOfAny("zzabyycdxx", ["mn","op"]) = -1
        /// StringUtil.lastIndexOfAny("zzabyycdxx", ["mn","op"]) = -1
        /// StringUtil.lastIndexOfAny("zzabyycdxx", ["mn",""])   = 10
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="searchStrs">  the Strings to search for, may be null </param>
        /// <returns> the last index of any of the Strings, -1 if no match </returns>
        public static int LastIndexOfAny(string str, string[] searchStrs)
        {
            if ((str == null) || (searchStrs == null))
            {
                return -1;
            }
            int sz = searchStrs.Length;
            int ret = -1;
            int tmp = 0;
            for (int i = 0; i < sz; i++)
            {
                string search = searchStrs[i];
                if (search == null)
                {
                    continue;
                }
                tmp = str.LastIndexOf(search);
                if (tmp > ret)
                {
                    ret = tmp;
                }
            }
            return ret;
        }

        // Substring
        //-----------------------------------------------------------------------
        /// <summary>
        /// <p>Gets a substring from the specified String avoiding exceptions.</p>
        /// 
        /// <p>A negative start position can be used to start <code>n</code>
        /// characters from the end of the String.</p>
        /// 
        /// <p>A <code>null</code> String will return <code>null</code>.
        /// An empty ("") String will return "".</p>
        /// 
        /// <pre>
        /// StringUtil.Substring(null, *)   = null
        /// StringUtil.Substring("", *)     = ""
        /// StringUtil.Substring("abc", 0)  = "abc"
        /// StringUtil.Substring("abc", 2)  = "c"
        /// StringUtil.Substring("abc", 4)  = ""
        /// StringUtil.Substring("abc", -2) = "bc"
        /// StringUtil.Substring("abc", -4) = "abc"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to get the substring from, may be null </param>
        /// <param name="start">  the position to start from, negative means
        ///  count back from the end of the String by this many characters </param>
        /// <returns> substring from start position, <code>null</code> if null String input </returns>
        public static string Substring(string str, int start)
        {
            if (str == null)
            {
                return null;
            }

            // handle negatives, which means last n characters
            if (start < 0)
            {
                start = str.Length + start; // remember start is negative
            }

            if (start < 0)
            {
                start = 0;
            }
            if (start > str.Length)
            {
                return EMPTY;
            }

            return str.Substring(start);
        }

        /// <summary>
        /// <p>Gets a substring from the specified String avoiding exceptions.</p>
        /// 
        /// <p>A negative start position can be used to start/end <code>n</code>
        /// characters from the end of the String.</p>
        /// 
        /// <p>The returned substring starts with the character in the <code>start</code>
        /// position and ends before the <code>end</code> position. All position counting is
        /// zero-based -- i.e., to start at the beginning of the string use
        /// <code>start = 0</code>. Negative start and end positions can be used to
        /// specify offsets relative to the end of the String.</p>
        /// 
        /// <p>If <code>start</code> is not strictly to the left of <code>end</code>, ""
        /// is returned.</p>
        /// 
        /// <pre>
        /// StringUtil.Substring(null, *, *)    = null
        /// StringUtil.Substring("", * ,  *)    = "";
        /// StringUtil.Substring("abc", 0, 2)   = "ab"
        /// StringUtil.Substring("abc", 2, 0)   = ""
        /// StringUtil.Substring("abc", 2, 4)   = "c"
        /// StringUtil.Substring("abc", 4, 6)   = ""
        /// StringUtil.Substring("abc", 2, 2)   = ""
        /// StringUtil.Substring("abc", -2, -1) = "b"
        /// StringUtil.Substring("abc", -4, 2)  = "ab"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to get the substring from, may be null </param>
        /// <param name="start">  the position to start from, negative means
        ///  count back from the end of the String by this many characters </param>
        /// <param name="end">  the position to end at (exclusive), negative means
        ///  count back from the end of the String by this many characters </param>
        /// <returns> substring from start position to end positon,
        ///  <code>null</code> if null String input </returns>
        public static string Substring(string str, int start, int end)
        {
            if (str == null)
            {
                return null;
            }

            // handle negatives
            if (end < 0)
            {
                end = str.Length + end; // remember end is negative
            }
            if (start < 0)
            {
                start = str.Length + start; // remember start is negative
            }

            // check length next
            if (end > str.Length)
            {
                end = str.Length;
            }

            // if start is greater than end, return ""
            if (start > end)
            {
                return EMPTY;
            }

            if (start < 0)
            {
                start = 0;
            }
            if (end < 0)
            {
                end = 0;
            }

            return str.Substring(start, end - start);
        }

        // Left/Right/Mid
        //-----------------------------------------------------------------------
        /// <summary>
        /// <p>Gets the leftmost <code>len</code> characters of a String.</p>
        /// 
        /// <p>If <code>len</code> characters are not available, or the
        /// String is <code>null</code>, the String will be returned without
        /// an exception. An exception is thrown if len is negative.</p>
        /// 
        /// <pre>
        /// StringUtil.left(null, *)    = null
        /// StringUtil.left(*, -ve)     = ""
        /// StringUtil.left("", *)      = ""
        /// StringUtil.left("abc", 0)   = ""
        /// StringUtil.left("abc", 2)   = "ab"
        /// StringUtil.left("abc", 4)   = "abc"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to get the leftmost characters from, may be null </param>
        /// <param name="len">  the length of the required String, must be zero or positive </param>
        /// <returns> the leftmost characters, <code>null</code> if null String input </returns>
        public static string Left(string str, int len)
        {
            if (str == null)
            {
                return null;
            }
            if (len < 0)
            {
                return EMPTY;
            }
            if (str.Length <= len)
            {
                return str;
            }
            return str.Substring(0, len);
        }

        /// <summary>
        /// <p>Gets the rightmost <code>len</code> characters of a String.</p>
        /// 
        /// <p>If <code>len</code> characters are not available, or the String
        /// is <code>null</code>, the String will be returned without an
        /// an exception. An exception is thrown if len is negative.</p>
        /// 
        /// <pre>
        /// StringUtil.right(null, *)    = null
        /// StringUtil.right(*, -ve)     = ""
        /// StringUtil.right("", *)      = ""
        /// StringUtil.right("abc", 0)   = ""
        /// StringUtil.right("abc", 2)   = "bc"
        /// StringUtil.right("abc", 4)   = "abc"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to get the rightmost characters from, may be null </param>
        /// <param name="len">  the length of the required String, must be zero or positive </param>
        /// <returns> the rightmost characters, <code>null</code> if null String input </returns>
        public static string Right(string str, int len)
        {
            if (str == null)
            {
                return null;
            }
            if (len < 0)
            {
                return EMPTY;
            }
            if (str.Length <= len)
            {
                return str;
            }
            return str.Substring(str.Length - len);
        }

        /// <summary>
        /// <p>Gets <code>len</code> characters from the middle of a String.</p>
        /// 
        /// <p>If <code>len</code> characters are not available, the remainder
        /// of the String will be returned without an exception. If the
        /// String is <code>null</code>, <code>null</code> will be returned.
        /// An exception is thrown if len is negative.</p>
        /// 
        /// <pre>
        /// StringUtil.mid(null, *, *)    = null
        /// StringUtil.mid(*, *, -ve)     = ""
        /// StringUtil.mid("", 0, *)      = ""
        /// StringUtil.mid("abc", 0, 2)   = "ab"
        /// StringUtil.mid("abc", 0, 4)   = "abc"
        /// StringUtil.mid("abc", 2, 4)   = "c"
        /// StringUtil.mid("abc", 4, 2)   = ""
        /// StringUtil.mid("abc", -2, 2)  = "ab"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to get the characters from, may be null </param>
        /// <param name="pos">  the position to start from, negative treated as zero </param>
        /// <param name="len">  the length of the required String, must be zero or positive </param>
        /// <returns> the middle characters, <code>null</code> if null String input </returns>
        public static string Mid(string str, int pos, int len)
        {
            if (str == null)
            {
                return null;
            }
            if (len < 0 || pos > str.Length)
            {
                return EMPTY;
            }
            if (pos < 0)
            {
                pos = 0;
            }
            if (str.Length <= (pos + len))
            {
                return str.Substring(pos);
            }
            return str.Substring(pos, len);
        }

        // SubStringAfter/SubStringBefore
        //-----------------------------------------------------------------------
        /// <summary>
        /// <p>Gets the substring before the first occurrence of a separator.
        /// The separator is not returned.</p>
        /// 
        /// <p>A <code>null</code> string input will return <code>null</code>.
        /// An empty ("") string input will return the empty string.
        /// A <code>null</code> separator will return the input string.</p>
        /// 
        /// <pre>
        /// StringUtil.SubstringBefore(null, *)      = null
        /// StringUtil.SubstringBefore("", *)        = ""
        /// StringUtil.SubstringBefore("abc", "a")   = ""
        /// StringUtil.SubstringBefore("abcba", "b") = "a"
        /// StringUtil.SubstringBefore("abc", "c")   = "ab"
        /// StringUtil.SubstringBefore("abc", "d")   = "abc"
        /// StringUtil.SubstringBefore("abc", "")    = ""
        /// StringUtil.SubstringBefore("abc", null)  = "abc"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to get a substring from, may be null </param>
        /// <param name="separator">  the String to search for, may be null </param>
        /// <returns> the substring before the first occurrence of the separator,
        ///  <code>null</code> if null String input
        /// @since 2.0 </returns>
        public static string SubstringBefore(string str, string separator)
        {
            if (IsEmpty(str) || separator == null)
            {
                return str;
            }
            if (separator.Length == 0)
            {
                return EMPTY;
            }
            int pos = str.IndexOf(separator);
            if (pos == -1)
            {
                return str;
            }
            return str.Substring(0, pos);
        }

        /// <summary>
        /// <p>Gets the substring after the first occurrence of a separator.
        /// The separator is not returned.</p>
        /// 
        /// <p>A <code>null</code> string input will return <code>null</code>.
        /// An empty ("") string input will return the empty string.
        /// A <code>null</code> separator will return the empty string if the
        /// input string is not <code>null</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.SubstringAfter(null, *)      = null
        /// StringUtil.SubstringAfter("", *)        = ""
        /// StringUtil.SubstringAfter(*, null)      = ""
        /// StringUtil.SubstringAfter("abc", "a")   = "bc"
        /// StringUtil.SubstringAfter("abcba", "b") = "cba"
        /// StringUtil.SubstringAfter("abc", "c")   = ""
        /// StringUtil.SubstringAfter("abc", "d")   = ""
        /// StringUtil.SubstringAfter("abc", "")    = "abc"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to get a substring from, may be null </param>
        /// <param name="separator">  the String to search for, may be null </param>
        /// <returns> the substring after the first occurrence of the separator,
        ///  <code>null</code> if null String input
        /// @since 2.0 </returns>
        public static string SubstringAfter(string str, string separator)
        {
            if (IsEmpty(str))
            {
                return str;
            }
            if (separator == null)
            {
                return EMPTY;
            }
            int pos = str.IndexOf(separator);
            if (pos == -1)
            {
                return EMPTY;
            }
            return str.Substring(pos + separator.Length);
        }


        /// <summary>
        /// <p>Gets the substring before the last occurrence of a separator.
        /// The separator is not returned.</p>
        /// 
        /// <p>A <code>null</code> string input will return <code>null</code>.
        /// An empty ("") string input will return the empty string.
        /// An empty or <code>null</code> separator will return the input string.</p>
        /// 
        /// <pre>
        /// StringUtil.SubstringBeforeLast(null, *)      = null
        /// StringUtil.SubstringBeforeLast("", *)        = ""
        /// StringUtil.SubstringBeforeLast("abcba", "b") = "abc"
        /// StringUtil.SubstringBeforeLast("abc", "c")   = "ab"
        /// StringUtil.SubstringBeforeLast("a", "a")     = ""
        /// StringUtil.SubstringBeforeLast("a", "z")     = "a"
        /// StringUtil.SubstringBeforeLast("a", null)    = "a"
        /// StringUtil.SubstringBeforeLast("a", "")      = "a"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to get a substring from, may be null </param>
        /// <param name="separator">  the String to search for, may be null </param>
        /// <returns> the substring before the last occurrence of the separator,
        ///  <code>null</code> if null String input
        /// @since 2.0 </returns>
        public static string SubstringBeforeLast(string str, string separator)
        {
            if (IsEmpty(str) || IsEmpty(separator))
            {
                return str;
            }
            int pos = str.LastIndexOf(separator);
            if (pos == -1)
            {
                return str;
            }
            return str.Substring(0, pos);
        }

        /// <summary>
        /// <p>Gets the substring after the last occurrence of a separator.
        /// The separator is not returned.</p>
        /// 
        /// <p>A <code>null</code> string input will return <code>null</code>.
        /// An empty ("") string input will return the empty string.
        /// An empty or <code>null</code> separator will return the empty string if
        /// the input string is not <code>null</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.SubstringAfterLast(null, *)      = null
        /// StringUtil.SubstringAfterLast("", *)        = ""
        /// StringUtil.SubstringAfterLast(*, "")        = ""
        /// StringUtil.SubstringAfterLast(*, null)      = ""
        /// StringUtil.SubstringAfterLast("abc", "a")   = "bc"
        /// StringUtil.SubstringAfterLast("abcba", "b") = "a"
        /// StringUtil.SubstringAfterLast("abc", "c")   = ""
        /// StringUtil.SubstringAfterLast("a", "a")     = ""
        /// StringUtil.SubstringAfterLast("a", "z")     = ""
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to get a substring from, may be null </param>
        /// <param name="separator">  the String to search for, may be null </param>
        /// <returns> the substring after the last occurrence of the separator,
        ///  <code>null</code> if null String input
        /// @since 2.0 </returns>
        public static string SubstringAfterLast(string str, string separator)
        {
            if (IsEmpty(str))
            {
                return str;
            }
            if (IsEmpty(separator))
            {
                return EMPTY;
            }
            int pos = str.LastIndexOf(separator);
            if (pos == -1 || pos == (str.Length - separator.Length))
            {
                return EMPTY;
            }
            return str.Substring(pos + separator.Length);
        }

        // Substring between
        //-----------------------------------------------------------------------
        /// <summary>
        /// <p>Gets the String that is nested in between two instances of the
        /// same String.</p>
        /// 
        /// <p>A <code>null</code> input String returns <code>null</code>.
        /// A <code>null</code> tag returns <code>null</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.SubstringBetween(null, *)            = null
        /// StringUtil.SubstringBetween("", "")             = ""
        /// StringUtil.SubstringBetween("", "tag")          = null
        /// StringUtil.SubstringBetween("tagabctag", null)  = null
        /// StringUtil.SubstringBetween("tagabctag", "")    = ""
        /// StringUtil.SubstringBetween("tagabctag", "tag") = "abc"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String containing the substring, may be null </param>
        /// <param name="tag">  the String before and after the substring, may be null </param>
        /// <returns> the substring, <code>null</code> if no match
        /// @since 2.0 </returns>
        public static string SubstringBetween(string str, string tag)
        {
            return SubstringBetween(str, tag, tag);
        }

        /// <summary>
        /// <p>Gets the String that is nested in between two Strings.
        /// Only the first match is returned.</p>
        /// 
        /// <p>A <code>null</code> input String returns <code>null</code>.
        /// A <code>null</code> open/close returns <code>null</code> (no match).
        /// An empty ("") open and close returns an empty string.</p>
        /// 
        /// <pre>
        /// StringUtil.SubstringBetween("wx[b]yz", "[", "]") = "b"
        /// StringUtil.SubstringBetween(null, *, *)          = null
        /// StringUtil.SubstringBetween(*, null, *)          = null
        /// StringUtil.SubstringBetween(*, *, null)          = null
        /// StringUtil.SubstringBetween("", "", "")          = ""
        /// StringUtil.SubstringBetween("", "", "]")         = null
        /// StringUtil.SubstringBetween("", "[", "]")        = null
        /// StringUtil.SubstringBetween("yabcz", "", "")     = ""
        /// StringUtil.SubstringBetween("yabcz", "y", "z")   = "abc"
        /// StringUtil.SubstringBetween("yabczyabcz", "y", "z")   = "abc"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String containing the substring, may be null </param>
        /// <param name="open">  the String before the substring, may be null </param>
        /// <param name="close">  the String after the substring, may be null </param>
        /// <returns> the substring, <code>null</code> if no match
        /// @since 2.0 </returns>
        public static string SubstringBetween(string str, string open, string close)
        {
            if (str == null || open == null || close == null)
            {
                return null;
            }
            int start = str.IndexOf(open);
            if (start != -1)
            {
                int end = str.IndexOf(close, start + open.Length);
                if (end != -1)
                {
                    return str.Substring(start + open.Length, end - (start + open.Length));
                }
            }
            return null;
        }

        /// <summary>
        /// <p>Searches a String for substrings delimited by a start and end tag,
        /// returning all matching substrings in an array.</p>
        /// 
        /// <p>A <code>null</code> input String returns <code>null</code>.
        /// A <code>null</code> open/close returns <code>null</code> (no match).
        /// An empty ("") open/close returns <code>null</code> (no match).</p>
        /// 
        /// <pre>
        /// StringUtil.SubstringsBetween("[a][b][c]", "[", "]") = ["a","b","c"]
        /// StringUtil.SubstringsBetween(null, *, *)            = null
        /// StringUtil.SubstringsBetween(*, null, *)            = null
        /// StringUtil.SubstringsBetween(*, *, null)            = null
        /// StringUtil.SubstringsBetween("", "[", "]")          = []
        /// </pre>
        /// </summary>
        /// <param name="str">  the String containing the substrings, null returns null, empty returns empty </param>
        /// <param name="open">  the String identifying the start of the substring, empty returns null </param>
        /// <param name="close">  the String identifying the end of the substring, empty returns null </param>
        /// <returns> a String Array of substrings, or <code>null</code> if no match
        /// @since 2.3 </returns>
        public static string[] SubstringsBetween(string str, string open, string close)
        {
            if (str == null || IsEmpty(open) || IsEmpty(close))
            {
                return null;
            }
            int strLen = str.Length;
            if (strLen == 0)
            {
                return EMPTY_STRING_ARRAY;
            }
            int closeLen = close.Length;
            int openLen = open.Length;
            List<string> list = new List<string>();
            int pos = 0;
            while (pos < (strLen - closeLen))
            {
                int start = str.IndexOf(open, pos);
                if (start < 0)
                {
                    break;
                }
                start += openLen;
                int end = str.IndexOf(close, start);
                if (end < 0)
                {
                    break;
                }
                list.Add(str.Substring(start, end - start));
                pos = end + closeLen;
            }
            if (list.Count == 0)
            {
                return null;
            }
            //return (string[])list.ToArray(typeof(string));
            return (string[])list.ToArray();
        }

        // Nested extraction
        //-----------------------------------------------------------------------
        /// <summary>
        /// <p>Gets the String that is nested in between two instances of the
        /// same String.</p>
        /// 
        /// <p>A <code>null</code> input String returns <code>null</code>.
        /// A <code>null</code> tag returns <code>null</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.getNestedString(null, *)            = null
        /// StringUtil.getNestedString("", "")             = ""
        /// StringUtil.getNestedString("", "tag")          = null
        /// StringUtil.getNestedString("tagabctag", null)  = null
        /// StringUtil.getNestedString("tagabctag", "")    = ""
        /// StringUtil.getNestedString("tagabctag", "tag") = "abc"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String containing nested-string, may be null </param>
        /// <param name="tag">  the String before and after nested-string, may be null </param>
        /// <returns> the nested String, <code>null</code> if no match </returns>
        /// @deprecated Use the better named <seealso cref="#SubstringBetween(String, String)"/>.
        ///             Method will be removed in Commons Lang 3.0. 
        public static string GetNestedString(string str, string tag)
        {
            return SubstringBetween(str, tag, tag);
        }

        /// <summary>
        /// <p>Gets the String that is nested in between two Strings.
        /// Only the first match is returned.</p>
        /// 
        /// <p>A <code>null</code> input String returns <code>null</code>.
        /// A <code>null</code> open/close returns <code>null</code> (no match).
        /// An empty ("") open/close returns an empty string.</p>
        /// 
        /// <pre>
        /// StringUtil.getNestedString(null, *, *)          = null
        /// StringUtil.getNestedString("", "", "")          = ""
        /// StringUtil.getNestedString("", "", "tag")       = null
        /// StringUtil.getNestedString("", "tag", "tag")    = null
        /// StringUtil.getNestedString("yabcz", null, null) = null
        /// StringUtil.getNestedString("yabcz", "", "")     = ""
        /// StringUtil.getNestedString("yabcz", "y", "z")   = "abc"
        /// StringUtil.getNestedString("yabczyabcz", "y", "z")   = "abc"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String containing nested-string, may be null </param>
        /// <param name="open">  the String before nested-string, may be null </param>
        /// <param name="close">  the String after nested-string, may be null </param>
        /// <returns> the nested String, <code>null</code> if no match </returns>
        /// @deprecated Use the better named <seealso cref="#SubstringBetween(String, String, String)"/>.
        ///             Method will be removed in Commons Lang 3.0. 
        public static string GetNestedString(string str, string open, string close)
        {
            return SubstringBetween(str, open, close);
        }

        // Splitting
        //-----------------------------------------------------------------------
        /// <summary>
        /// <p>Splits the provided text into an array, using whitespace as the
        /// separator.
        /// Whitespace is defined by <seealso cref="Character#isWhitespace(char)"/>.</p>
        /// 
        /// <p>The separator is not included in the returned String array.
        /// Adjacent separators are treated as one separator.
        /// For more control over the split use the StrTokenizer class.</p>
        /// 
        /// <p>A <code>null</code> input String returns <code>null</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.Split(null)       = null
        /// StringUtil.Split("")         = []
        /// StringUtil.Split("abc def")  = ["abc", "def"]
        /// StringUtil.Split("abc  def") = ["abc", "def"]
        /// StringUtil.Split(" abc ")    = ["abc"]
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to parse, may be null </param>
        /// <returns> an array of parsed Strings, <code>null</code> if null String input </returns>
        public static string[] Split(string str)
        {
            return Split(str, null, -1);
        }

        /// <summary>
        /// <p>Splits the provided text into an array, separator specified.
        /// This is an alternative to using StringTokenizer.</p>
        /// 
        /// <p>The separator is not included in the returned String array.
        /// Adjacent separators are treated as one separator.
        /// For more control over the split use the StrTokenizer class.</p>
        /// 
        /// <p>A <code>null</code> input String returns <code>null</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.Split(null, *)         = null
        /// StringUtil.Split("", *)           = []
        /// StringUtil.Split("a.b.c", '.')    = ["a", "b", "c"]
        /// StringUtil.Split("a..b.c", '.')   = ["a", "b", "c"]
        /// StringUtil.Split("a:b:c", '.')    = ["a:b:c"]
        /// StringUtil.Split("a b c", ' ')    = ["a", "b", "c"]
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to parse, may be null </param>
        /// <param name="separatorChar">  the character used as the delimiter </param>
        /// <returns> an array of parsed Strings, <code>null</code> if null String input
        /// @since 2.0 </returns>
        public static string[] Split(string str, char separatorChar)
        {
            return SplitWorker(str, separatorChar, false);
        }

        /// <summary>
        /// <p>Splits the provided text into an array, separators specified.
        /// This is an alternative to using StringTokenizer.</p>
        /// 
        /// <p>The separator is not included in the returned String array.
        /// Adjacent separators are treated as one separator.
        /// For more control over the split use the StrTokenizer class.</p>
        /// 
        /// <p>A <code>null</code> input String returns <code>null</code>.
        /// A <code>null</code> separatorChars splits on whitespace.</p>
        /// 
        /// <pre>
        /// StringUtil.Split(null, *)         = null
        /// StringUtil.Split("", *)           = []
        /// StringUtil.Split("abc def", null) = ["abc", "def"]
        /// StringUtil.Split("abc def", " ")  = ["abc", "def"]
        /// StringUtil.Split("abc  def", " ") = ["abc", "def"]
        /// StringUtil.Split("ab:cd:ef", ":") = ["ab", "cd", "ef"]
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to parse, may be null </param>
        /// <param name="separatorChars">  the characters used as the delimiters,
        ///  <code>null</code> splits on whitespace </param>
        /// <returns> an array of parsed Strings, <code>null</code> if null String input </returns>
        public static string[] Split(string str, string separatorChars)
        {
            return SplitWorker(str, separatorChars, -1, false);
        }

        /// <summary>
        /// <p>Splits the provided text into an array with a maximum length,
        /// separators specified.</p>
        /// 
        /// <p>The separator is not included in the returned String array.
        /// Adjacent separators are treated as one separator.</p>
        /// 
        /// <p>A <code>null</code> input String returns <code>null</code>.
        /// A <code>null</code> separatorChars splits on whitespace.</p>
        /// 
        /// <p>If more than <code>max</code> delimited substrings are found, the last
        /// returned string includes all characters after the first <code>max - 1</code>
        /// returned strings (including separator characters).</p>
        /// 
        /// <pre>
        /// StringUtil.Split(null, *, *)            = null
        /// StringUtil.Split("", *, *)              = []
        /// StringUtil.Split("ab de fg", null, 0)   = ["ab", "cd", "ef"]
        /// StringUtil.Split("ab   de fg", null, 0) = ["ab", "cd", "ef"]
        /// StringUtil.Split("ab:cd:ef", ":", 0)    = ["ab", "cd", "ef"]
        /// StringUtil.Split("ab:cd:ef", ":", 2)    = ["ab", "cd:ef"]
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to parse, may be null </param>
        /// <param name="separatorChars">  the characters used as the delimiters,
        ///  <code>null</code> splits on whitespace </param>
        /// <param name="max">  the maximum number of elements to include in the
        ///  array. A zero or negative value implies no limit </param>
        /// <returns> an array of parsed Strings, <code>null</code> if null String input </returns>
        public static string[] Split(string str, string separatorChars, int max)
        {
            return SplitWorker(str, separatorChars, max, false);
        }

        /// <summary>
        /// <p>Splits the provided text into an array, separator string specified.</p>
        /// 
        /// <p>The separator(s) will not be included in the returned String array.
        /// Adjacent separators are treated as one separator.</p>
        /// 
        /// <p>A <code>null</code> input String returns <code>null</code>.
        /// A <code>null</code> separator splits on whitespace.</p>
        /// 
        /// <pre>
        /// StringUtil.splitByWholeSeparator(null, *)               = null
        /// StringUtil.splitByWholeSeparator("", *)                 = []
        /// StringUtil.splitByWholeSeparator("ab de fg", null)      = ["ab", "de", "fg"]
        /// StringUtil.splitByWholeSeparator("ab   de fg", null)    = ["ab", "de", "fg"]
        /// StringUtil.splitByWholeSeparator("ab:cd:ef", ":")       = ["ab", "cd", "ef"]
        /// StringUtil.splitByWholeSeparator("ab-!-cd-!-ef", "-!-") = ["ab", "cd", "ef"]
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to parse, may be null </param>
        /// <param name="separator">  String containing the String to be used as a delimiter,
        ///  <code>null</code> splits on whitespace </param>
        /// <returns> an array of parsed Strings, <code>null</code> if null String was input </returns>
        public static string[] SplitByWholeSeparator(string str, string separator)
        {
            return SplitByWholeSeparatorWorker(str, separator, -1, false);
        }

        /// <summary>
        /// <p>Splits the provided text into an array, separator string specified.
        /// Returns a maximum of <code>max</code> substrings.</p>
        /// 
        /// <p>The separator(s) will not be included in the returned String array.
        /// Adjacent separators are treated as one separator.</p>
        /// 
        /// <p>A <code>null</code> input String returns <code>null</code>.
        /// A <code>null</code> separator splits on whitespace.</p>
        /// 
        /// <pre>
        /// StringUtil.splitByWholeSeparator(null, *, *)               = null
        /// StringUtil.splitByWholeSeparator("", *, *)                 = []
        /// StringUtil.splitByWholeSeparator("ab de fg", null, 0)      = ["ab", "de", "fg"]
        /// StringUtil.splitByWholeSeparator("ab   de fg", null, 0)    = ["ab", "de", "fg"]
        /// StringUtil.splitByWholeSeparator("ab:cd:ef", ":", 2)       = ["ab", "cd:ef"]
        /// StringUtil.splitByWholeSeparator("ab-!-cd-!-ef", "-!-", 5) = ["ab", "cd", "ef"]
        /// StringUtil.splitByWholeSeparator("ab-!-cd-!-ef", "-!-", 2) = ["ab", "cd-!-ef"]
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to parse, may be null </param>
        /// <param name="separator">  String containing the String to be used as a delimiter,
        ///  <code>null</code> splits on whitespace </param>
        /// <param name="max">  the maximum number of elements to include in the returned
        ///  array. A zero or negative value implies no limit. </param>
        /// <returns> an array of parsed Strings, <code>null</code> if null String was input </returns>
        public static string[] SplitByWholeSeparator(string str, string separator, int max)
        {
            return SplitByWholeSeparatorWorker(str, separator, max, false);
        }

        /// <summary>
        /// <p>Splits the provided text into an array, separator string specified. </p>
        /// 
        /// <p>The separator is not included in the returned String array.
        /// Adjacent separators are treated as separators for empty tokens.
        /// For more control over the split use the StrTokenizer class.</p>
        /// 
        /// <p>A <code>null</code> input String returns <code>null</code>.
        /// A <code>null</code> separator splits on whitespace.</p>
        /// 
        /// <pre>
        /// StringUtil.splitByWholeSeparatorPreserveAllTokens(null, *)               = null
        /// StringUtil.splitByWholeSeparatorPreserveAllTokens("", *)                 = []
        /// StringUtil.splitByWholeSeparatorPreserveAllTokens("ab de fg", null)      = ["ab", "de", "fg"]
        /// StringUtil.splitByWholeSeparatorPreserveAllTokens("ab   de fg", null)    = ["ab", "", "", "de", "fg"]
        /// StringUtil.splitByWholeSeparatorPreserveAllTokens("ab:cd:ef", ":")       = ["ab", "cd", "ef"]
        /// StringUtil.splitByWholeSeparatorPreserveAllTokens("ab-!-cd-!-ef", "-!-") = ["ab", "cd", "ef"]
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to parse, may be null </param>
        /// <param name="separator">  String containing the String to be used as a delimiter,
        ///  <code>null</code> splits on whitespace </param>
        /// <returns> an array of parsed Strings, <code>null</code> if null String was input
        /// @since 2.4 </returns>
        public static string[] SplitByWholeSeparatorPreserveAllTokens(string str, string separator)
        {
            return SplitByWholeSeparatorWorker(str, separator, -1, true);
        }

        /// <summary>
        /// <p>Splits the provided text into an array, separator string specified.
        /// Returns a maximum of <code>max</code> substrings.</p>
        /// 
        /// <p>The separator is not included in the returned String array.
        /// Adjacent separators are treated as separators for empty tokens.
        /// For more control over the split use the StrTokenizer class.</p>
        /// 
        /// <p>A <code>null</code> input String returns <code>null</code>.
        /// A <code>null</code> separator splits on whitespace.</p>
        /// 
        /// <pre>
        /// StringUtil.splitByWholeSeparatorPreserveAllTokens(null, *, *)               = null
        /// StringUtil.splitByWholeSeparatorPreserveAllTokens("", *, *)                 = []
        /// StringUtil.splitByWholeSeparatorPreserveAllTokens("ab de fg", null, 0)      = ["ab", "de", "fg"]
        /// StringUtil.splitByWholeSeparatorPreserveAllTokens("ab   de fg", null, 0)    = ["ab", "", "", "de", "fg"]
        /// StringUtil.splitByWholeSeparatorPreserveAllTokens("ab:cd:ef", ":", 2)       = ["ab", "cd:ef"]
        /// StringUtil.splitByWholeSeparatorPreserveAllTokens("ab-!-cd-!-ef", "-!-", 5) = ["ab", "cd", "ef"]
        /// StringUtil.splitByWholeSeparatorPreserveAllTokens("ab-!-cd-!-ef", "-!-", 2) = ["ab", "cd-!-ef"]
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to parse, may be null </param>
        /// <param name="separator">  String containing the String to be used as a delimiter,
        ///  <code>null</code> splits on whitespace </param>
        /// <param name="max">  the maximum number of elements to include in the returned
        ///  array. A zero or negative value implies no limit. </param>
        /// <returns> an array of parsed Strings, <code>null</code> if null String was input
        /// @since 2.4 </returns>
        public static string[] SplitByWholeSeparatorPreserveAllTokens(string str, string separator, int max)
        {
            return SplitByWholeSeparatorWorker(str, separator, max, true);
        }

        /// <summary>
        /// Performs the logic for the <code>splitByWholeSeparatorPreserveAllTokens</code> methods.
        /// </summary>
        /// <param name="str">  the String to parse, may be <code>null</code> </param>
        /// <param name="separator">  String containing the String to be used as a delimiter,
        ///  <code>null</code> splits on whitespace </param>
        /// <param name="max">  the maximum number of elements to include in the returned
        ///  array. A zero or negative value implies no limit. </param>
        /// <param name="preserveAllTokens"> if <code>true</code>, adjacent separators are
        /// treated as empty token separators; if <code>false</code>, adjacent
        /// separators are treated as one separator. </param>
        /// <returns> an array of parsed Strings, <code>null</code> if null String input
        /// @since 2.4 </returns>
        private static string[] SplitByWholeSeparatorWorker(string str, string separator, int max, bool preserveAllTokens)
        {
            if (str == null)
            {
                return null;
            }

            int len = str.Length;

            if (len == 0)
            {
                return EMPTY_STRING_ARRAY;
            }

            if ((separator == null) || (EMPTY.Equals(separator)))
            {
                // Split on whitespace.
                return SplitWorker(str, null, max, preserveAllTokens);
            }

            int separatorLength = separator.Length;

            ArrayList substrings = new ArrayList();
            int numberOfSubstrings = 0;
            int beg = 0;
            int end = 0;
            while (end < len)
            {
                end = str.IndexOf(separator, beg);

                if (end > -1)
                {
                    if (end > beg)
                    {
                        numberOfSubstrings += 1;

                        if (numberOfSubstrings == max)
                        {
                            end = len;
                            substrings.Add(str.Substring(beg));
                        }
                        else
                        {
                            // The following is OK, because String.Substring( beg, end ) excludes
                            // the character at the position 'end'.
                            substrings.Add(str.Substring(beg, end - beg));

                            // Set the starting point for the next search.
                            // The following is equivalent to beg = end + (separatorLength - 1) + 1,
                            // which is the right calculation:
                            beg = end + separatorLength;
                        }
                    }
                    else
                    {
                        // We found a consecutive occurrence of the separator, so skip it.
                        if (preserveAllTokens)
                        {
                            numberOfSubstrings += 1;
                            if (numberOfSubstrings == max)
                            {
                                end = len;
                                substrings.Add(str.Substring(beg));
                            }
                            else
                            {
                                substrings.Add(EMPTY);
                            }
                        }
                        beg = end + separatorLength;
                    }
                }
                else
                {
                    // String.Substring( beg ) goes from 'beg' to the end of the String.
                    substrings.Add(str.Substring(beg));
                    end = len;
                }
            }

            return (string[])substrings.ToArray(typeof(string));
        }

        // -----------------------------------------------------------------------
        /// <summary>
        /// <p>Splits the provided text into an array, using whitespace as the
        /// separator, preserving all tokens, including empty tokens created by 
        /// adjacent separators. This is an alternative to using StringTokenizer.
        /// Whitespace is defined by <seealso cref="Character#isWhitespace(char)"/>.</p>
        /// 
        /// <p>The separator is not included in the returned String array.
        /// Adjacent separators are treated as separators for empty tokens.
        /// For more control over the split use the StrTokenizer class.</p>
        /// 
        /// <p>A <code>null</code> input String returns <code>null</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.splitPreserveAllTokens(null)       = null
        /// StringUtil.splitPreserveAllTokens("")         = []
        /// StringUtil.splitPreserveAllTokens("abc def")  = ["abc", "def"]
        /// StringUtil.splitPreserveAllTokens("abc  def") = ["abc", "", "def"]
        /// StringUtil.splitPreserveAllTokens(" abc ")    = ["", "abc", ""]
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to parse, may be <code>null</code> </param>
        /// <returns> an array of parsed Strings, <code>null</code> if null String input
        /// @since 2.1 </returns>
        public static string[] SplitPreserveAllTokens(string str)
        {
            return SplitWorker(str, null, -1, true);
        }

        /// <summary>
        /// <p>Splits the provided text into an array, separator specified,
        /// preserving all tokens, including empty tokens created by adjacent
        /// separators. This is an alternative to using StringTokenizer.</p>
        /// 
        /// <p>The separator is not included in the returned String array.
        /// Adjacent separators are treated as separators for empty tokens.
        /// For more control over the split use the StrTokenizer class.</p>
        /// 
        /// <p>A <code>null</code> input String returns <code>null</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.splitPreserveAllTokens(null, *)         = null
        /// StringUtil.splitPreserveAllTokens("", *)           = []
        /// StringUtil.splitPreserveAllTokens("a.b.c", '.')    = ["a", "b", "c"]
        /// StringUtil.splitPreserveAllTokens("a..b.c", '.')   = ["a", "", "b", "c"]
        /// StringUtil.splitPreserveAllTokens("a:b:c", '.')    = ["a:b:c"]
        /// StringUtil.splitPreserveAllTokens("a\tb\nc", null) = ["a", "b", "c"]
        /// StringUtil.splitPreserveAllTokens("a b c", ' ')    = ["a", "b", "c"]
        /// StringUtil.splitPreserveAllTokens("a b c ", ' ')   = ["a", "b", "c", ""]
        /// StringUtil.splitPreserveAllTokens("a b c  ", ' ')   = ["a", "b", "c", "", ""]
        /// StringUtil.splitPreserveAllTokens(" a b c", ' ')   = ["", a", "b", "c"]
        /// StringUtil.splitPreserveAllTokens("  a b c", ' ')  = ["", "", a", "b", "c"]
        /// StringUtil.splitPreserveAllTokens(" a b c ", ' ')  = ["", a", "b", "c", ""]
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to parse, may be <code>null</code> </param>
        /// <param name="separatorChar">  the character used as the delimiter,
        ///  <code>null</code> splits on whitespace </param>
        /// <returns> an array of parsed Strings, <code>null</code> if null String input
        /// @since 2.1 </returns>
        public static string[] SplitPreserveAllTokens(string str, char separatorChar)
        {
            return SplitWorker(str, separatorChar, true);
        }

        /// <summary>
        /// Performs the logic for the <code>split</code> and 
        /// <code>splitPreserveAllTokens</code> methods that do not return a
        /// maximum array length.
        /// </summary>
        /// <param name="str">  the String to parse, may be <code>null</code> </param>
        /// <param name="separatorChar"> the separate character </param>
        /// <param name="preserveAllTokens"> if <code>true</code>, adjacent separators are
        /// treated as empty token separators; if <code>false</code>, adjacent
        /// separators are treated as one separator. </param>
        /// <returns> an array of parsed Strings, <code>null</code> if null String input </returns>
        private static string[] SplitWorker(string str, char separatorChar, bool preserveAllTokens)
        {
            // Performance tuned for 2.0 (JDK1.4)

            if (str == null)
            {
                return null;
            }
            int len = str.Length;
            if (len == 0)
            {
                return EMPTY_STRING_ARRAY;
            }
            List<string> list = new List<string>();
            int i = 0, start = 0;
            bool match = false;
            bool lastMatch = false;
            while (i < len)
            {
                if (str[i] == separatorChar)
                {
                    if (match || preserveAllTokens)
                    {
                        list.Add(str.Substring(start, i - start));
                        match = false;
                        lastMatch = true;
                    }
                    start = ++i;
                    continue;
                }
                lastMatch = false;
                match = true;
                i++;
            }
            if (match || (preserveAllTokens && lastMatch))
            {
                list.Add(str.Substring(start, i - start));
            }
            return (string[])list.ToArray();
        }

        /// <summary>
        /// <p>Splits the provided text into an array, separators specified, 
        /// preserving all tokens, including empty tokens created by adjacent
        /// separators. This is an alternative to using StringTokenizer.</p>
        /// 
        /// <p>The separator is not included in the returned String array.
        /// Adjacent separators are treated as separators for empty tokens.
        /// For more control over the split use the StrTokenizer class.</p>
        /// 
        /// <p>A <code>null</code> input String returns <code>null</code>.
        /// A <code>null</code> separatorChars splits on whitespace.</p>
        /// 
        /// <pre>
        /// StringUtil.splitPreserveAllTokens(null, *)           = null
        /// StringUtil.splitPreserveAllTokens("", *)             = []
        /// StringUtil.splitPreserveAllTokens("abc def", null)   = ["abc", "def"]
        /// StringUtil.splitPreserveAllTokens("abc def", " ")    = ["abc", "def"]
        /// StringUtil.splitPreserveAllTokens("abc  def", " ")   = ["abc", "", def"]
        /// StringUtil.splitPreserveAllTokens("ab:cd:ef", ":")   = ["ab", "cd", "ef"]
        /// StringUtil.splitPreserveAllTokens("ab:cd:ef:", ":")  = ["ab", "cd", "ef", ""]
        /// StringUtil.splitPreserveAllTokens("ab:cd:ef::", ":") = ["ab", "cd", "ef", "", ""]
        /// StringUtil.splitPreserveAllTokens("ab::cd:ef", ":")  = ["ab", "", cd", "ef"]
        /// StringUtil.splitPreserveAllTokens(":cd:ef", ":")     = ["", cd", "ef"]
        /// StringUtil.splitPreserveAllTokens("::cd:ef", ":")    = ["", "", cd", "ef"]
        /// StringUtil.splitPreserveAllTokens(":cd:ef:", ":")    = ["", cd", "ef", ""]
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to parse, may be <code>null</code> </param>
        /// <param name="separatorChars">  the characters used as the delimiters,
        ///  <code>null</code> splits on whitespace </param>
        /// <returns> an array of parsed Strings, <code>null</code> if null String input
        /// @since 2.1 </returns>
        public static string[] SplitPreserveAllTokens(string str, string separatorChars)
        {
            return SplitWorker(str, separatorChars, -1, true);
        }

        /// <summary>
        /// <p>Splits the provided text into an array with a maximum length,
        /// separators specified, preserving all tokens, including empty tokens 
        /// created by adjacent separators.</p>
        /// 
        /// <p>The separator is not included in the returned String array.
        /// Adjacent separators are treated as separators for empty tokens.
        /// Adjacent separators are treated as one separator.</p>
        /// 
        /// <p>A <code>null</code> input String returns <code>null</code>.
        /// A <code>null</code> separatorChars splits on whitespace.</p>
        /// 
        /// <p>If more than <code>max</code> delimited substrings are found, the last
        /// returned string includes all characters after the first <code>max - 1</code>
        /// returned strings (including separator characters).</p>
        /// 
        /// <pre>
        /// StringUtil.splitPreserveAllTokens(null, *, *)            = null
        /// StringUtil.splitPreserveAllTokens("", *, *)              = []
        /// StringUtil.splitPreserveAllTokens("ab de fg", null, 0)   = ["ab", "cd", "ef"]
        /// StringUtil.splitPreserveAllTokens("ab   de fg", null, 0) = ["ab", "cd", "ef"]
        /// StringUtil.splitPreserveAllTokens("ab:cd:ef", ":", 0)    = ["ab", "cd", "ef"]
        /// StringUtil.splitPreserveAllTokens("ab:cd:ef", ":", 2)    = ["ab", "cd:ef"]
        /// StringUtil.splitPreserveAllTokens("ab   de fg", null, 2) = ["ab", "  de fg"]
        /// StringUtil.splitPreserveAllTokens("ab   de fg", null, 3) = ["ab", "", " de fg"]
        /// StringUtil.splitPreserveAllTokens("ab   de fg", null, 4) = ["ab", "", "", "de fg"]
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to parse, may be <code>null</code> </param>
        /// <param name="separatorChars">  the characters used as the delimiters,
        ///  <code>null</code> splits on whitespace </param>
        /// <param name="max">  the maximum number of elements to include in the
        ///  array. A zero or negative value implies no limit </param>
        /// <returns> an array of parsed Strings, <code>null</code> if null String input
        /// @since 2.1 </returns>
        public static string[] SplitPreserveAllTokens(string str, string separatorChars, int max)
        {
            return SplitWorker(str, separatorChars, max, true);
        }

        /// <summary>
        /// Performs the logic for the <code>split</code> and 
        /// <code>splitPreserveAllTokens</code> methods that return a maximum array 
        /// length.
        /// </summary>
        /// <param name="str">  the String to parse, may be <code>null</code> </param>
        /// <param name="separatorChars"> the separate character </param>
        /// <param name="max">  the maximum number of elements to include in the
        ///  array. A zero or negative value implies no limit. </param>
        /// <param name="preserveAllTokens"> if <code>true</code>, adjacent separators are
        /// treated as empty token separators; if <code>false</code>, adjacent
        /// separators are treated as one separator. </param>
        /// <returns> an array of parsed Strings, <code>null</code> if null String input </returns>
        private static string[] SplitWorker(string str, string separatorChars, int max, bool preserveAllTokens)
        {
            // Performance tuned for 2.0 (JDK1.4)
            // Direct code is quicker than StringTokenizer.
            // Also, StringTokenizer uses isSpace() not isWhitespace()

            if (str == null)
            {
                return null;
            }
            int len = str.Length;
            if (len == 0)
            {
                return EMPTY_STRING_ARRAY;
            }
            List<string> list = new List<string>();
            int sizePlus1 = 1;
            int i = 0, start = 0;
            bool match = false;
            bool lastMatch = false;
            if (separatorChars == null)
            {
                // Null separator means use whitespace
                while (i < len)
                {
                    if (char.IsWhiteSpace(str[i]))
                    {
                        if (match || preserveAllTokens)
                        {
                            lastMatch = true;
                            if (sizePlus1++ == max)
                            {
                                i = len;
                                lastMatch = false;
                            }
                            list.Add(str.Substring(start, i - start));
                            match = false;
                        }
                        start = ++i;
                        continue;
                    }
                    lastMatch = false;
                    match = true;
                    i++;
                }
            }
            else if (separatorChars.Length == 1)
            {
                // Optimise 1 character case
                char sep = separatorChars[0];
                while (i < len)
                {
                    if (str[i] == sep)
                    {
                        if (match || preserveAllTokens)
                        {
                            lastMatch = true;
                            if (sizePlus1++ == max)
                            {
                                i = len;
                                lastMatch = false;
                            }
                            list.Add(str.Substring(start, i - start));
                            match = false;
                        }
                        start = ++i;
                        continue;
                    }
                    lastMatch = false;
                    match = true;
                    i++;
                }
            }
            else
            {
                // standard case
                while (i < len)
                {
                    if (separatorChars.IndexOf(str[i]) >= 0)
                    {
                        if (match || preserveAllTokens)
                        {
                            lastMatch = true;
                            if (sizePlus1++ == max)
                            {
                                i = len;
                                lastMatch = false;
                            }
                            list.Add(str.Substring(start, i - start));
                            match = false;
                        }
                        start = ++i;
                        continue;
                    }
                    lastMatch = false;
                    match = true;
                    i++;
                }
            }
            if (match || (preserveAllTokens && lastMatch))
            {
                list.Add(str.Substring(start, i - start));
            }
            return (string[])list.ToArray();
        }

        /// <summary>
        /// <p>Splits a String by Character type as returned by
        /// <code>java.lang.Character.getType(char)</code>. Groups of contiguous
        /// characters of the same type are returned as complete tokens. 
        /// <pre>
        /// StringUtil.SplitByCharacterType(null)         = null
        /// StringUtil.SplitByCharacterType("")           = []
        /// StringUtil.SplitByCharacterType("ab de fg")   = ["ab", " ", "de", " ", "fg"]
        /// StringUtil.SplitByCharacterType("ab   de fg") = ["ab", "   ", "de", " ", "fg"]
        /// StringUtil.SplitByCharacterType("ab:cd:ef")   = ["ab", ":", "cd", ":", "ef"]
        /// StringUtil.SplitByCharacterType("number5")    = ["number", "5"]
        /// StringUtil.SplitByCharacterType("fooBar")     = ["foo", "B", "ar"]
        /// StringUtil.SplitByCharacterType("foo200Bar")  = ["foo", "200", "B", "ar"]
        /// StringUtil.SplitByCharacterType("ASFRules")   = ["ASFR", "ules"]
        /// </pre> </summary>
        /// <param name="str"> the String to split, may be <code>null</code> </param>
        /// <returns> an array of parsed Strings, <code>null</code> if null String input
        /// @since 2.4 </returns>
        public static string[] SplitByCharacterType(string str)
        {
            return SplitByCharacterType(str, false);
        }

        /// <summary>
        /// <p>Splits a String by Character type as returned by
        /// <code>java.lang.Character.getType(char)</code>. Groups of contiguous
        /// characters of the same type are returned as complete tokens, with the
        /// following exception: the character of type
        /// <code>Character.UPPERCASE_LETTER</code>, if any, immediately
        /// preceding a token of type <code>Character.LOWERCASE_LETTER</code>
        /// will belong to the following token rather than to the preceding, if any,
        /// <code>Character.UPPERCASE_LETTER</code> token. 
        /// <pre>
        /// StringUtil.splitByCharacterTypeCamelCase(null)         = null
        /// StringUtil.splitByCharacterTypeCamelCase("")           = []
        /// StringUtil.splitByCharacterTypeCamelCase("ab de fg")   = ["ab", " ", "de", " ", "fg"]
        /// StringUtil.splitByCharacterTypeCamelCase("ab   de fg") = ["ab", "   ", "de", " ", "fg"]
        /// StringUtil.splitByCharacterTypeCamelCase("ab:cd:ef")   = ["ab", ":", "cd", ":", "ef"]
        /// StringUtil.splitByCharacterTypeCamelCase("number5")    = ["number", "5"]
        /// StringUtil.splitByCharacterTypeCamelCase("fooBar")     = ["foo", "Bar"]
        /// StringUtil.splitByCharacterTypeCamelCase("foo200Bar")  = ["foo", "200", "Bar"]
        /// StringUtil.splitByCharacterTypeCamelCase("ASFRules")   = ["ASF", "Rules"]
        /// </pre> </summary>
        /// <param name="str"> the String to split, may be <code>null</code> </param>
        /// <returns> an array of parsed Strings, <code>null</code> if null String input
        /// @since 2.4 </returns>
        public static string[] SplitByCharacterTypeCamelCase(string str)
        {
            return SplitByCharacterType(str, true);
        }

        /// <summary>
        /// <p>Splits a String by Character type as returned by
        /// <code>java.lang.Character.getType(char)</code>. Groups of contiguous
        /// characters of the same type are returned as complete tokens, with the
        /// following exception: if <code>camelCase</code> is <code>true</code>,
        /// the character of type <code>Character.UPPERCASE_LETTER</code>, if any,
        /// immediately preceding a token of type <code>Character.LOWERCASE_LETTER</code>
        /// will belong to the following token rather than to the preceding, if any,
        /// <code>Character.UPPERCASE_LETTER</code> token. </summary>
        /// <param name="str"> the String to split, may be <code>null</code> </param>
        /// <param name="camelCase"> whether to use so-called "camel-case" for letter types </param>
        /// <returns> an array of parsed Strings, <code>null</code> if null String input
        /// @since 2.4 </returns>
        private static string[] SplitByCharacterType(string str, bool camelCase)
        {
            if (str == null)
            {
                return null;
            }
            if (str.Length == 0)
            {
                return EMPTY_STRING_ARRAY;
            }
            char[] c = str.ToCharArray();
            List<string> list = new List<string>();
            int tokenStart = 0;
            
            
            TypeCode currentType = c[tokenStart].GetTypeCode();
            for (int pos = tokenStart + 1; pos < c.Length; pos++)
            {
                TypeCode type = c[pos].GetTypeCode(); 
                if (type == currentType)
                {
                    continue;
                }
                if (camelCase && char.IsLower(c[pos]) && char.IsUpper(c[tokenStart]))
                {
                    int newTokenStart = pos - 1;
                    if (newTokenStart != tokenStart)
                    {
                        list.Add(new string(c, tokenStart, newTokenStart - tokenStart));
                        tokenStart = newTokenStart;
                    }
                }
                else
                {
                    list.Add(new string(c, tokenStart, pos - tokenStart));
                    tokenStart = pos;
                }
                currentType = type;
            }
            list.Add(new string(c, tokenStart, c.Length - tokenStart));
            return (string[])list.ToArray();
        }

        // Joining
        //-----------------------------------------------------------------------
        /// <summary>
        /// <p>Concatenates elements of an array into a single String.
        /// Null objects or empty strings within the array are represented by
        /// empty strings.</p>
        /// 
        /// <pre>
        /// StringUtil.concatenate(null)            = null
        /// StringUtil.concatenate([])              = ""
        /// StringUtil.concatenate([null])          = ""
        /// StringUtil.concatenate(["a", "b", "c"]) = "abc"
        /// StringUtil.concatenate([null, "", "a"]) = "a"
        /// </pre>
        /// </summary>
        /// <param name="array">  the array of values to concatenate, may be null </param>
        /// <returns> the concatenated String, <code>null</code> if null array input </returns>
        /// @deprecated Use the better named <seealso cref="#Join(Object[])"/> instead.
        ///             Method will be removed in Commons Lang 3.0. 
        public static string Concatenate(object[] array)
        {
            return Join(array, null);
        }

        /// <summary>
        /// <p>Joins the elements of the provided array into a single String
        /// containing the provided list of elements.</p>
        /// 
        /// <p>No separator is added to the joined String.
        /// Null objects or empty strings within the array are represented by
        /// empty strings.</p>
        /// 
        /// <pre>
        /// StringUtil.Join(null)            = null
        /// StringUtil.Join([])              = ""
        /// StringUtil.Join([null])          = ""
        /// StringUtil.Join(["a", "b", "c"]) = "abc"
        /// StringUtil.Join([null, "", "a"]) = "a"
        /// </pre>
        /// </summary>
        /// <param name="array">  the array of values to join together, may be null </param>
        /// <returns> the joined String, <code>null</code> if null array input
        /// @since 2.0 </returns>
        public static string Join(object[] array)
        {
            return Join(array, null);
        }

        /// <summary>
        /// <p>Joins the elements of the provided array into a single String
        /// containing the provided list of elements.</p>
        /// 
        /// <p>No delimiter is added before or after the list.
        /// Null objects or empty strings within the array are represented by
        /// empty strings.</p>
        /// 
        /// <pre>
        /// StringUtil.Join(null, *)               = null
        /// StringUtil.Join([], *)                 = ""
        /// StringUtil.Join([null], *)             = ""
        /// StringUtil.Join(["a", "b", "c"], ';')  = "a;b;c"
        /// StringUtil.Join(["a", "b", "c"], null) = "abc"
        /// StringUtil.Join([null, "", "a"], ';')  = ";;a"
        /// </pre>
        /// </summary>
        /// <param name="array">  the array of values to join together, may be null </param>
        /// <param name="separator">  the separator character to use </param>
        /// <returns> the joined String, <code>null</code> if null array input
        /// @since 2.0 </returns>
        public static string Join(object[] array, char separator)
        {
            if (array == null)
            {
                return null;
            }

            return Join(array, separator, 0, array.Length);
        }

        /// <summary>
        /// <p>Joins the elements of the provided array into a single String
        /// containing the provided list of elements.</p>
        /// 
        /// <p>No delimiter is added before or after the list.
        /// Null objects or empty strings within the array are represented by
        /// empty strings.</p>
        /// 
        /// <pre>
        /// StringUtil.Join(null, *)               = null
        /// StringUtil.Join([], *)                 = ""
        /// StringUtil.Join([null], *)             = ""
        /// StringUtil.Join(["a", "b", "c"], ';')  = "a;b;c"
        /// StringUtil.Join(["a", "b", "c"], null) = "abc"
        /// StringUtil.Join([null, "", "a"], ';')  = ";;a"
        /// </pre>
        /// </summary>
        /// <param name="array">  the array of values to join together, may be null </param>
        /// <param name="separator">  the separator character to use </param>
        /// <param name="startIndex"> the first index to start joining from.  It is
        /// an error to pass in an end index past the end of the array </param>
        /// <param name="endIndex"> the index to stop joining from (exclusive). It is
        /// an error to pass in an end index past the end of the array </param>
        /// <returns> the joined String, <code>null</code> if null array input
        /// @since 2.0 </returns>
        public static string Join(object[] array, char separator, int startIndex, int endIndex)
        {
            if (array == null)
            {
                return null;
            }
            int bufSize = (endIndex - startIndex);
            if (bufSize <= 0)
            {
                return EMPTY;
            }

            bufSize *= ((array[startIndex] == null ? 16 : array[startIndex].ToString().Length) + 1);
            StringBuilder buf = new StringBuilder(bufSize);

            for (int i = startIndex; i < endIndex; i++)
            {
                if (i > startIndex)
                {
                    buf.Append(separator);
                }
                if (array[i] != null)
                {
                    buf.Append(array[i]);
                }
            }
            return buf.ToString();
        }


        /// <summary>
        /// <p>Joins the elements of the provided array into a single String
        /// containing the provided list of elements.</p>
        /// 
        /// <p>No delimiter is added before or after the list.
        /// A <code>null</code> separator is the same as an empty String ("").
        /// Null objects or empty strings within the array are represented by
        /// empty strings.</p>
        /// 
        /// <pre>
        /// StringUtil.Join(null, *)                = null
        /// StringUtil.Join([], *)                  = ""
        /// StringUtil.Join([null], *)              = ""
        /// StringUtil.Join(["a", "b", "c"], "--")  = "a--b--c"
        /// StringUtil.Join(["a", "b", "c"], null)  = "abc"
        /// StringUtil.Join(["a", "b", "c"], "")    = "abc"
        /// StringUtil.Join([null, "", "a"], ',')   = ",,a"
        /// </pre>
        /// </summary>
        /// <param name="array">  the array of values to join together, may be null </param>
        /// <param name="separator">  the separator character to use, null treated as "" </param>
        /// <returns> the joined String, <code>null</code> if null array input </returns>
        public static string Join(object[] array, string separator)
        {
            if (array == null)
            {
                return null;
            }
            return Join(array, separator, 0, array.Length);
        }

        /// <summary>
        /// <p>Joins the elements of the provided array into a single String
        /// containing the provided list of elements.</p>
        /// 
        /// <p>No delimiter is added before or after the list.
        /// A <code>null</code> separator is the same as an empty String ("").
        /// Null objects or empty strings within the array are represented by
        /// empty strings.</p>
        /// 
        /// <pre>
        /// StringUtil.Join(null, *)                = null
        /// StringUtil.Join([], *)                  = ""
        /// StringUtil.Join([null], *)              = ""
        /// StringUtil.Join(["a", "b", "c"], "--")  = "a--b--c"
        /// StringUtil.Join(["a", "b", "c"], null)  = "abc"
        /// StringUtil.Join(["a", "b", "c"], "")    = "abc"
        /// StringUtil.Join([null, "", "a"], ',')   = ",,a"
        /// </pre>
        /// </summary>
        /// <param name="array">  the array of values to join together, may be null </param>
        /// <param name="separator">  the separator character to use, null treated as "" </param>
        /// <param name="startIndex"> the first index to start joining from.  It is
        /// an error to pass in an end index past the end of the array </param>
        /// <param name="endIndex"> the index to stop joining from (exclusive). It is
        /// an error to pass in an end index past the end of the array </param>
        /// <returns> the joined String, <code>null</code> if null array input </returns>
        public static string Join(object[] array, string separator, int startIndex, int endIndex)
        {
            if (array == null)
            {
                return null;
            }
            if (separator == null)
            {
                separator = EMPTY;
            }

            // endIndex - startIndex > 0:   Len = NofStrings *(len(firstString) + len(separator))
            //           (Assuming that all Strings are roughly equally long)
            int bufSize = (endIndex - startIndex);
            if (bufSize <= 0)
            {
                return EMPTY;
            }

            bufSize *= ((array[startIndex] == null ? 16 : array[startIndex].ToString().Length) + separator.Length);

            StringBuilder buf = new StringBuilder(bufSize);

            for (int i = startIndex; i < endIndex; i++)
            {
                if (i > startIndex)
                {
                    buf.Append(separator);
                }
                if (array[i] != null)
                {
                    buf.Append(array[i]);
                }
            }
            return buf.ToString();
        }

        /// <summary>
        /// <p>Joins the elements of the provided <code>Iterator</code> into
        /// a single String containing the provided elements.</p>
        /// 
        /// <p>No delimiter is added before or after the list. Null objects or empty
        /// strings within the iteration are represented by empty strings.</p>
        /// 
        /// <p>See the examples here: <seealso cref="#Join(Object[],char)"/>. </p>
        /// </summary>
        /// <param name="iterator">  the <code>Iterator</code> of values to join together, may be null </param>
        /// <param name="separator">  the separator character to use </param>
        /// <returns> the joined String, <code>null</code> if null iterator input
        /// @since 2.0 </returns>
        public static string Join(IEnumerator iterator, char separator)
        {

            // handle null, zero and one elements before building a buffer
            if (iterator == null)
            {
                return null;
            }
            if (!iterator.MoveNext())
            {
                return EMPTY;
            }
            object first = iterator.Current;
            if (!iterator.MoveNext())
            {
                return StringUtil.ToString(first);
            }

            // two or more elements
            StringBuilder buf = new StringBuilder(256); // Java default is 16, probably too small
            if (first != null)
            {
                buf.Append(first);
            }

            while (iterator.MoveNext())
            {
                buf.Append(separator);
                object obj = iterator.Current;
                if (obj != null)
                {
                    buf.Append(obj);
                }
            }

            return buf.ToString();
        }

        /// <summary>
        /// <p>Joins the elements of the provided <code>Iterator</code> into
        /// a single String containing the provided elements.</p>
        /// 
        /// <p>No delimiter is added before or after the list.
        /// A <code>null</code> separator is the same as an empty String ("").</p>
        /// 
        /// <p>See the examples here: <seealso cref="#Join(Object[],String)"/>. </p>
        /// </summary>
        /// <param name="iterator">  the <code>Iterator</code> of values to join together, may be null </param>
        /// <param name="separator">  the separator character to use, null treated as "" </param>
        /// <returns> the joined String, <code>null</code> if null iterator input </returns>
        public static string Join(IEnumerator iterator, string separator)
        {

            // handle null, zero and one elements before building a buffer
            if (iterator == null)
            {
                return null;
            }
            if (!iterator.MoveNext())
            {
                return EMPTY;
            }
            object first = iterator.Current;
            if (!iterator.MoveNext())
            {
                return StringUtil.ToString(first);
            }

            // two or more elements
            StringBuilder buf = new StringBuilder(256); // Java default is 16, probably too small
            if (first != null)
            {
                buf.Append(first);
            }

            while (iterator.MoveNext())
            {
                if (separator != null)
                {
                    buf.Append(separator);
                }
                object obj = iterator.Current;
                if (obj != null)
                {
                    buf.Append(obj);
                }
            }
            return buf.ToString();
        }

        /// <summary>
        /// <p>Joins the elements of the provided <code>Collection</code> into
        /// a single String containing the provided elements.</p>
        /// 
        /// <p>No delimiter is added before or after the list. Null objects or empty
        /// strings within the iteration are represented by empty strings.</p>
        /// 
        /// <p>See the examples here: <seealso cref="#Join(Object[],char)"/>. </p>
        /// </summary>
        /// <param name="collection">  the <code>Collection</code> of values to join together, may be null </param>
        /// <param name="separator">  the separator character to use </param>
        /// <returns> the joined String, <code>null</code> if null iterator input
        /// @since 2.3 </returns>
        public static string Join(ICollection collection, char separator)
        {
            if (collection == null)
            {
                return null;
            }
            return Join(collection.GetEnumerator(), separator);
        }

        /// <summary>
        /// <p>Joins the elements of the provided <code>Collection</code> into
        /// a single String containing the provided elements.</p>
        /// 
        /// <p>No delimiter is added before or after the list.
        /// A <code>null</code> separator is the same as an empty String ("").</p>
        /// 
        /// <p>See the examples here: <seealso cref="#Join(Object[],String)"/>. </p>
        /// </summary>
        /// <param name="collection">  the <code>Collection</code> of values to join together, may be null </param>
        /// <param name="separator">  the separator character to use, null treated as "" </param>
        /// <returns> the joined String, <code>null</code> if null iterator input
        /// @since 2.3 </returns>
        public static string Join(ICollection collection, string separator)
        {
            if (collection == null)
            {
                return null;
            }
            return Join(collection.GetEnumerator(), separator);
        }

        /// <summary>
        /// <p>Deletes all whitespaces from a String as defined by
        /// <seealso cref="Character#isWhitespace(char)"/>.</p>
        /// 
        /// <pre>
        /// StringUtil.deleteWhitespace(null)         = null
        /// StringUtil.deleteWhitespace("")           = ""
        /// StringUtil.deleteWhitespace("abc")        = "abc"
        /// StringUtil.deleteWhitespace("   ab  c  ") = "abc"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to delete whitespace from, may be null </param>
        /// <returns> the String without whitespaces, <code>null</code> if null String input </returns>
        public static string DeleteWhitespace(string str)
        {
            if (IsEmpty(str))
            {
                return str;
            }
            int sz = str.Length;
            char[] chs = new char[sz];
            int count = 0;
            for (int i = 0; i < sz; i++)
            {
                if (!char.IsWhiteSpace(str[i]))
                {
                    chs[count++] = str[i];
                }
            }
            if (count == sz)
            {
                return str;
            }
            return new string(chs, 0, count);
        }

        // Remove
        //-----------------------------------------------------------------------
        /// <summary>
        /// <p>Removes a substring only if it is at the begining of a source string,
        /// otherwise returns the source string.</p>
        /// 
        /// <p>A <code>null</code> source string will return <code>null</code>.
        /// An empty ("") source string will return the empty string.
        /// A <code>null</code> search string will return the source string.</p>
        /// 
        /// <pre>
        /// StringUtil.removeStart(null, *)      = null
        /// StringUtil.removeStart("", *)        = ""
        /// StringUtil.removeStart(*, null)      = *
        /// StringUtil.removeStart("www.domain.com", "www.")   = "domain.com"
        /// StringUtil.removeStart("domain.com", "www.")       = "domain.com"
        /// StringUtil.removeStart("www.domain.com", "domain") = "www.domain.com"
        /// StringUtil.removeStart("abc", "")    = "abc"
        /// </pre>
        /// </summary>
        /// <param name="str">  the source String to search, may be null </param>
        /// <param name="remove">  the String to search for and remove, may be null </param>
        /// <returns> the substring with the string removed if found,
        ///  <code>null</code> if null String input
        /// @since 2.1 </returns>
        public static string RemoveStart(string str, string remove)
        {
            if (IsEmpty(str) || IsEmpty(remove))
            {
                return str;
            }
            if (str.StartsWith(remove))
            {
                return str.Substring(remove.Length);
            }
            return str;
        }

        /// <summary>
        /// <p>Case insensitive removal of a substring if it is at the begining of a source string,
        /// otherwise returns the source string.</p>
        /// 
        /// <p>A <code>null</code> source string will return <code>null</code>.
        /// An empty ("") source string will return the empty string.
        /// A <code>null</code> search string will return the source string.</p>
        /// 
        /// <pre>
        /// StringUtil.removeStartIgnoreCase(null, *)      = null
        /// StringUtil.removeStartIgnoreCase("", *)        = ""
        /// StringUtil.removeStartIgnoreCase(*, null)      = *
        /// StringUtil.removeStartIgnoreCase("www.domain.com", "www.")   = "domain.com"
        /// StringUtil.removeStartIgnoreCase("www.domain.com", "WWW.")   = "domain.com"
        /// StringUtil.removeStartIgnoreCase("domain.com", "www.")       = "domain.com"
        /// StringUtil.removeStartIgnoreCase("www.domain.com", "domain") = "www.domain.com"
        /// StringUtil.removeStartIgnoreCase("abc", "")    = "abc"
        /// </pre>
        /// </summary>
        /// <param name="str">  the source String to search, may be null </param>
        /// <param name="remove">  the String to search for (case insensitive) and remove, may be null </param>
        /// <returns> the substring with the string removed if found,
        ///  <code>null</code> if null String input
        /// @since 2.4 </returns>
        public static string RemoveStartIgnoreCase(string str, string remove)
        {
            if (IsEmpty(str) || IsEmpty(remove))
            {
                return str;
            }
            if (StartsWithIgnoreCase(str, remove))
            {
                return str.Substring(remove.Length);
            }
            return str;
        }

        /// <summary>
        /// <p>Removes a substring only if it is at the end of a source string,
        /// otherwise returns the source string.</p>
        /// 
        /// <p>A <code>null</code> source string will return <code>null</code>.
        /// An empty ("") source string will return the empty string.
        /// A <code>null</code> search string will return the source string.</p>
        /// 
        /// <pre>
        /// StringUtil.removeEnd(null, *)      = null
        /// StringUtil.removeEnd("", *)        = ""
        /// StringUtil.removeEnd(*, null)      = *
        /// StringUtil.removeEnd("www.domain.com", ".com.")  = "www.domain.com"
        /// StringUtil.removeEnd("www.domain.com", ".com")   = "www.domain"
        /// StringUtil.removeEnd("www.domain.com", "domain") = "www.domain.com"
        /// StringUtil.removeEnd("abc", "")    = "abc"
        /// </pre>
        /// </summary>
        /// <param name="str">  the source String to search, may be null </param>
        /// <param name="remove">  the String to search for and remove, may be null </param>
        /// <returns> the substring with the string removed if found,
        ///  <code>null</code> if null String input
        /// @since 2.1 </returns>
        public static string RemoveEnd(string str, string remove)
        {
            if (IsEmpty(str) || IsEmpty(remove))
            {
                return str;
            }
            if (str.EndsWith(remove))
            {
                return str.Substring(0, str.Length - remove.Length);
            }
            return str;
        }

        /// <summary>
        /// <p>Case insensitive removal of a substring if it is at the end of a source string,
        /// otherwise returns the source string.</p>
        /// 
        /// <p>A <code>null</code> source string will return <code>null</code>.
        /// An empty ("") source string will return the empty string.
        /// A <code>null</code> search string will return the source string.</p>
        /// 
        /// <pre>
        /// StringUtil.removeEnd(null, *)      = null
        /// StringUtil.removeEnd("", *)        = ""
        /// StringUtil.removeEnd(*, null)      = *
        /// StringUtil.removeEnd("www.domain.com", ".com.")  = "www.domain.com."
        /// StringUtil.removeEnd("www.domain.com", ".com")   = "www.domain"
        /// StringUtil.removeEnd("www.domain.com", "domain") = "www.domain.com"
        /// StringUtil.removeEnd("abc", "")    = "abc"
        /// </pre>
        /// </summary>
        /// <param name="str">  the source String to search, may be null </param>
        /// <param name="remove">  the String to search for (case insensitive) and remove, may be null </param>
        /// <returns> the substring with the string removed if found,
        ///  <code>null</code> if null String input
        /// @since 2.4 </returns>
        public static string RemoveEndIgnoreCase(string str, string remove)
        {
            if (IsEmpty(str) || IsEmpty(remove))
            {
                return str;
            }
            if (EndsWithIgnoreCase(str, remove))
            {
                return str.Substring(0, str.Length - remove.Length);
            }
            return str;
        }

        /// <summary>
        /// <p>Removes all occurrences of a substring from within the source string.</p>
        /// 
        /// <p>A <code>null</code> source string will return <code>null</code>.
        /// An empty ("") source string will return the empty string.
        /// A <code>null</code> remove string will return the source string.
        /// An empty ("") remove string will return the source string.</p>
        /// 
        /// <pre>
        /// StringUtil.Remove(null, *)        = null
        /// StringUtil.Remove("", *)          = ""
        /// StringUtil.Remove(*, null)        = *
        /// StringUtil.Remove(*, "")          = *
        /// StringUtil.Remove("queued", "ue") = "qd"
        /// StringUtil.Remove("queued", "zz") = "queued"
        /// </pre>
        /// </summary>
        /// <param name="str">  the source String to search, may be null </param>
        /// <param name="remove">  the String to search for and remove, may be null </param>
        /// <returns> the substring with the string removed if found,
        ///  <code>null</code> if null String input
        /// @since 2.1 </returns>
        public static string Remove(string str, string remove)
        {
            if (IsEmpty(str) || IsEmpty(remove))
            {
                return str;
            }
            return Replace(str, remove, EMPTY, -1);
        }

        /// <summary>
        /// <p>Removes all occurrences of a character from within the source string.</p>
        /// 
        /// <p>A <code>null</code> source string will return <code>null</code>.
        /// An empty ("") source string will return the empty string.</p>
        /// 
        /// <pre>
        /// StringUtil.Remove(null, *)       = null
        /// StringUtil.Remove("", *)         = ""
        /// StringUtil.Remove("queued", 'u') = "qeed"
        /// StringUtil.Remove("queued", 'z') = "queued"
        /// </pre>
        /// </summary>
        /// <param name="str">  the source String to search, may be null </param>
        /// <param name="remove">  the char to search for and remove, may be null </param>
        /// <returns> the substring with the char removed if found,
        ///  <code>null</code> if null String input
        /// @since 2.1 </returns>
        public static string Remove(string str, char remove)
        {
            if (IsEmpty(str) || str.IndexOf(remove) == -1)
            {
                return str;
            }
            char[] chars = str.ToCharArray();
            int pos = 0;
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] != remove)
                {
                    chars[pos++] = chars[i];
                }
            }
            return new string(chars, 0, pos);
        }

        // Replacing
        //-----------------------------------------------------------------------
        /// <summary>
        /// <p>Replaces a String with another String inside a larger String, once.</p>
        /// 
        /// <p>A <code>null</code> reference passed to this method is a no-op.</p>
        /// 
        /// <pre>
        /// StringUtil.replaceOnce(null, *, *)        = null
        /// StringUtil.replaceOnce("", *, *)          = ""
        /// StringUtil.replaceOnce("any", null, *)    = "any"
        /// StringUtil.replaceOnce("any", *, null)    = "any"
        /// StringUtil.replaceOnce("any", "", *)      = "any"
        /// StringUtil.replaceOnce("aba", "a", null)  = "aba"
        /// StringUtil.replaceOnce("aba", "a", "")    = "ba"
        /// StringUtil.replaceOnce("aba", "a", "z")   = "zba"
        /// </pre>
        /// </summary>
        /// <seealso cref= #Replace(String text, String searchString, String replacement, int max) </seealso>
        /// <param name="text">  text to search and replace in, may be null </param>
        /// <param name="searchString">  the String to search for, may be null </param>
        /// <param name="replacement">  the String to replace with, may be null </param>
        /// <returns> the text with any replacements processed,
        ///  <code>null</code> if null String input </returns>
        public static string ReplaceOnce(string text, string searchString, string replacement)
        {
            return Replace(text, searchString, replacement, 1);
        }

        /// <summary>
        /// <p>Replaces all occurrences of a String within another String.</p>
        /// 
        /// <p>A <code>null</code> reference passed to this method is a no-op.</p>
        /// 
        /// <pre>
        /// StringUtil.Replace(null, *, *)        = null
        /// StringUtil.Replace("", *, *)          = ""
        /// StringUtil.Replace("any", null, *)    = "any"
        /// StringUtil.Replace("any", *, null)    = "any"
        /// StringUtil.Replace("any", "", *)      = "any"
        /// StringUtil.Replace("aba", "a", null)  = "aba"
        /// StringUtil.Replace("aba", "a", "")    = "b"
        /// StringUtil.Replace("aba", "a", "z")   = "zbz"
        /// </pre>
        /// </summary>
        /// <seealso cref= #Replace(String text, String searchString, String replacement, int max) </seealso>
        /// <param name="text">  text to search and replace in, may be null </param>
        /// <param name="searchString">  the String to search for, may be null </param>
        /// <param name="replacement">  the String to replace it with, may be null </param>
        /// <returns> the text with any replacements processed,
        ///  <code>null</code> if null String input </returns>
        public static string Replace(string text, string searchString, string replacement)
        {
            return Replace(text, searchString, replacement, -1);
        }

        /// <summary>
        /// <p>Replaces a String with another String inside a larger String,
        /// for the first <code>max</code> values of the search String.</p>
        /// 
        /// <p>A <code>null</code> reference passed to this method is a no-op.</p>
        /// 
        /// <pre>
        /// StringUtil.Replace(null, *, *, *)         = null
        /// StringUtil.Replace("", *, *, *)           = ""
        /// StringUtil.Replace("any", null, *, *)     = "any"
        /// StringUtil.Replace("any", *, null, *)     = "any"
        /// StringUtil.Replace("any", "", *, *)       = "any"
        /// StringUtil.Replace("any", *, *, 0)        = "any"
        /// StringUtil.Replace("abaa", "a", null, -1) = "abaa"
        /// StringUtil.Replace("abaa", "a", "", -1)   = "b"
        /// StringUtil.Replace("abaa", "a", "z", 0)   = "abaa"
        /// StringUtil.Replace("abaa", "a", "z", 1)   = "zbaa"
        /// StringUtil.Replace("abaa", "a", "z", 2)   = "zbza"
        /// StringUtil.Replace("abaa", "a", "z", -1)  = "zbzz"
        /// </pre>
        /// </summary>
        /// <param name="text">  text to search and replace in, may be null </param>
        /// <param name="searchString">  the String to search for, may be null </param>
        /// <param name="replacement">  the String to replace it with, may be null </param>
        /// <param name="max">  maximum number of values to replace, or <code>-1</code> if no maximum </param>
        /// <returns> the text with any replacements processed,
        ///  <code>null</code> if null String input </returns>
        public static string Replace(string text, string searchString, string replacement, int max)
        {
            if (IsEmpty(text) || IsEmpty(searchString) || replacement == null || max == 0)
            {
                return text;
            }
            int start = 0;
            int end = text.IndexOf(searchString, start);
            if (end == -1)
            {
                return text;
            }
            int replLength = searchString.Length;
            int increase = replacement.Length - replLength;
            increase = (increase < 0 ? 0 : increase);
            increase *= (max < 0 ? 16 : (max > 64 ? 64 : max));
            StringBuilder buf = new StringBuilder(text.Length + increase);
            while (end != -1)
            {
                buf.Append(text.Substring(start, end - start)).Append(replacement);
                start = end + replLength;
                if (--max == 0)
                {
                    break;
                }
                end = text.IndexOf(searchString, start);
            }
            buf.Append(text.Substring(start));
            return buf.ToString();
        }

        /// <summary>
        /// <p>
        /// Replaces all occurrences of Strings within another String.
        /// </p>
        /// 
        /// <p>
        /// A <code>null</code> reference passed to this method is a no-op, or if
        /// any "search string" or "string to replace" is null, that replace will be
        /// ignored. This will not repeat. For repeating replaces, call the
        /// overloaded method.
        /// </p>
        /// 
        /// <pre>
        ///  StringUtil.ReplaceEach(null, *, *)        = null
        ///  StringUtil.ReplaceEach("", *, *)          = ""
        ///  StringUtil.ReplaceEach("aba", null, null) = "aba"
        ///  StringUtil.ReplaceEach("aba", new String[0], null) = "aba"
        ///  StringUtil.ReplaceEach("aba", null, new String[0]) = "aba"
        ///  StringUtil.ReplaceEach("aba", new String[]{"a"}, null)  = "aba"
        ///  StringUtil.ReplaceEach("aba", new String[]{"a"}, new String[]{""})  = "b"
        ///  StringUtil.ReplaceEach("aba", new String[]{null}, new String[]{"a"})  = "aba"
        ///  StringUtil.ReplaceEach("abcde", new String[]{"ab", "d"}, new String[]{"w", "t"})  = "wcte"
        ///  (example of how it does not repeat)
        ///  StringUtil.ReplaceEach("abcde", new String[]{"ab", "d"}, new String[]{"d", "t"})  = "dcte"
        /// </pre>
        /// </summary>
        /// <param name="text">
        ///            text to search and replace in, no-op if null </param>
        /// <param name="searchList">
        ///            the Strings to search for, no-op if null </param>
        /// <param name="replacementList">
        ///            the Strings to replace them with, no-op if null </param>
        /// <returns> the text with any replacements processed, <code>null</code> if
        ///         null String input </returns>
        /// <exception cref="IndexOutOfBoundsException">
        ///             if the lengths of the arrays are not the same (null is ok,
        ///             and/or size 0)
        /// @since 2.4 </exception>
        public static string ReplaceEach(string text, string[] searchList, string[] replacementList)
        {
            return ReplaceEach(text, searchList, replacementList, false, 0);
        }

        /// <summary>
        /// <p>
        /// Replaces all occurrences of Strings within another String.
        /// </p>
        /// 
        /// <p>
        /// A <code>null</code> reference passed to this method is a no-op, or if
        /// any "search string" or "string to replace" is null, that replace will be
        /// ignored. This will not repeat. For repeating replaces, call the
        /// overloaded method.
        /// </p>
        /// 
        /// <pre>
        ///  StringUtil.ReplaceEach(null, *, *, *) = null
        ///  StringUtil.ReplaceEach("", *, *, *) = ""
        ///  StringUtil.ReplaceEach("aba", null, null, *) = "aba"
        ///  StringUtil.ReplaceEach("aba", new String[0], null, *) = "aba"
        ///  StringUtil.ReplaceEach("aba", null, new String[0], *) = "aba"
        ///  StringUtil.ReplaceEach("aba", new String[]{"a"}, null, *) = "aba"
        ///  StringUtil.ReplaceEach("aba", new String[]{"a"}, new String[]{""}, *) = "b"
        ///  StringUtil.ReplaceEach("aba", new String[]{null}, new String[]{"a"}, *) = "aba"
        ///  StringUtil.ReplaceEach("abcde", new String[]{"ab", "d"}, new String[]{"w", "t"}, *) = "wcte"
        ///  (example of how it repeats)
        ///  StringUtil.ReplaceEach("abcde", new String[]{"ab", "d"}, new String[]{"d", "t"}, false) = "dcte"
        ///  StringUtil.ReplaceEach("abcde", new String[]{"ab", "d"}, new String[]{"d", "t"}, true) = "tcte"
        ///  StringUtil.ReplaceEach("abcde", new String[]{"ab", "d"}, new String[]{"d", "ab"}, true) = IllegalArgumentException
        ///  StringUtil.ReplaceEach("abcde", new String[]{"ab", "d"}, new String[]{"d", "ab"}, false) = "dcabe"
        /// </pre>
        /// </summary>
        /// <param name="text">
        ///            text to search and replace in, no-op if null </param>
        /// <param name="searchList">
        ///            the Strings to search for, no-op if null </param>
        /// <param name="replacementList">
        ///            the Strings to replace them with, no-op if null </param>
        /// <returns> the text with any replacements processed, <code>null</code> if
        ///         null String input </returns>
        /// <exception cref="IllegalArgumentException">
        ///             if the search is repeating and there is an endless loop due
        ///             to outputs of one being inputs to another </exception>
        /// <exception cref="IndexOutOfBoundsException">
        ///             if the lengths of the arrays are not the same (null is ok,
        ///             and/or size 0)
        /// @since 2.4 </exception>
        public static string ReplaceEachRepeatedly(string text, string[] searchList, string[] replacementList)
        {
            // timeToLive should be 0 if not used or nothing to replace, else it's
            // the length of the replace array
            int timeToLive = searchList == null ? 0 : searchList.Length;
            return ReplaceEach(text, searchList, replacementList, true, timeToLive);
        }

        /// <summary>
        /// <p>
        /// Replaces all occurrences of Strings within another String.
        /// </p>
        /// 
        /// <p>
        /// A <code>null</code> reference passed to this method is a no-op, or if
        /// any "search string" or "string to replace" is null, that replace will be
        /// ignored. 
        /// </p>
        /// 
        /// <pre>
        ///  StringUtil.ReplaceEach(null, *, *, *) = null
        ///  StringUtil.ReplaceEach("", *, *, *) = ""
        ///  StringUtil.ReplaceEach("aba", null, null, *) = "aba"
        ///  StringUtil.ReplaceEach("aba", new String[0], null, *) = "aba"
        ///  StringUtil.ReplaceEach("aba", null, new String[0], *) = "aba"
        ///  StringUtil.ReplaceEach("aba", new String[]{"a"}, null, *) = "aba"
        ///  StringUtil.ReplaceEach("aba", new String[]{"a"}, new String[]{""}, *) = "b"
        ///  StringUtil.ReplaceEach("aba", new String[]{null}, new String[]{"a"}, *) = "aba"
        ///  StringUtil.ReplaceEach("abcde", new String[]{"ab", "d"}, new String[]{"w", "t"}, *) = "wcte"
        ///  (example of how it repeats)
        ///  StringUtil.ReplaceEach("abcde", new String[]{"ab", "d"}, new String[]{"d", "t"}, false) = "dcte"
        ///  StringUtil.ReplaceEach("abcde", new String[]{"ab", "d"}, new String[]{"d", "t"}, true) = "tcte"
        ///  StringUtil.ReplaceEach("abcde", new String[]{"ab", "d"}, new String[]{"d", "ab"}, *) = IllegalArgumentException
        /// </pre>
        /// </summary>
        /// <param name="text">
        ///            text to search and replace in, no-op if null </param>
        /// <param name="searchList">
        ///            the Strings to search for, no-op if null </param>
        /// <param name="replacementList">
        ///            the Strings to replace them with, no-op if null </param>
        /// <param name="repeat"> if true, then replace repeatedly 
        ///       until there are no more possible replacements or timeToLive < 0 </param>
        /// <param name="timeToLive">
        ///            if less than 0 then there is a circular reference and endless
        ///            loop </param>
        /// <returns> the text with any replacements processed, <code>null</code> if
        ///         null String input </returns>
        /// <exception cref="IllegalArgumentException">
        ///             if the search is repeating and there is an endless loop due
        ///             to outputs of one being inputs to another </exception>
        /// <exception cref="IndexOutOfBoundsException">
        ///             if the lengths of the arrays are not the same (null is ok,
        ///             and/or size 0)
        /// @since 2.4 </exception>
        private static string ReplaceEach(string text, string[] searchList, string[] replacementList, bool repeat, int timeToLive)
        {

            // mchyzer Performance note: This creates very few new objects (one major goal)
            // let me know if there are performance requests, we can create a harness to measure

            if (text == null || text.Length == 0 || searchList == null || searchList.Length == 0 || replacementList == null || replacementList.Length == 0)
            {
                return text;
            }

            // if recursing, this shouldnt be less than 0
            if (timeToLive < 0)
            {
                //throw new IllegalStateException("TimeToLive of " + timeToLive + " is less than 0: " + text);
                throw new Exception();
            }

            int searchLength = searchList.Length;
            int replacementLength = replacementList.Length;

            // make sure lengths are ok, these need to be equal
            if (searchLength != replacementLength)
            {
                throw new System.ArgumentException("Search and Replace array lengths don't match: " + searchLength + " vs " + replacementLength);
            }

            // keep track of which still have matches
            bool[] noMoreMatchesForReplIndex = new bool[searchLength];

            // index on index that the match was found
            int textIndex = -1;
            int replaceIndex = -1;
            int tempIndex = -1;

            // index of replace array that will replace the search string found
            // NOTE: logic duplicated below START
            for (int i = 0; i < searchLength; i++)
            {
                if (noMoreMatchesForReplIndex[i] || searchList[i] == null || searchList[i].Length == 0 || replacementList[i] == null)
                {
                    continue;
                }
                tempIndex = text.IndexOf(searchList[i]);

                // see if we need to keep searching for this
                if (tempIndex == -1)
                {
                    noMoreMatchesForReplIndex[i] = true;
                }
                else
                {
                    if (textIndex == -1 || tempIndex < textIndex)
                    {
                        textIndex = tempIndex;
                        replaceIndex = i;
                    }
                }
            }
            // NOTE: logic mostly below END

            // no search strings found, we are done
            if (textIndex == -1)
            {
                return text;
            }

            int start = 0;

            // get a good guess on the size of the result buffer so it doesnt have to double if it goes over a bit
            int increase = 0;

            // count the replacement text elements that are larger than their corresponding text being replaced
            for (int i = 0; i < searchList.Length; i++)
            {
                int greater = replacementList[i].Length - searchList[i].Length;
                if (greater > 0)
                {
                    increase += 3 * greater; // assume 3 matches
                }
            }
            // have upper-bound at 20% increase, then let Java take over
            increase = Math.Min(increase, text.Length / 5);

            StringBuilder buf = new StringBuilder(text.Length + increase);

            while (textIndex != -1)
            {

                for (int i = start; i < textIndex; i++)
                {
                    buf.Append(text[i]);
                }
                buf.Append(replacementList[replaceIndex]);

                start = textIndex + searchList[replaceIndex].Length;

                textIndex = -1;
                replaceIndex = -1;
                tempIndex = -1;
                // find the next earliest match
                // NOTE: logic mostly duplicated above START
                for (int i = 0; i < searchLength; i++)
                {
                    if (noMoreMatchesForReplIndex[i] || searchList[i] == null || searchList[i].Length == 0 || replacementList[i] == null)
                    {
                        continue;
                    }
                    tempIndex = text.IndexOf(searchList[i], start);

                    // see if we need to keep searching for this
                    if (tempIndex == -1)
                    {
                        noMoreMatchesForReplIndex[i] = true;
                    }
                    else
                    {
                        if (textIndex == -1 || tempIndex < textIndex)
                        {
                            textIndex = tempIndex;
                            replaceIndex = i;
                        }
                    }
                }
                // NOTE: logic duplicated above END

            }
            int textLength = text.Length;
            for (int i = start; i < textLength; i++)
            {
                buf.Append(text[i]);
            }
            string result = buf.ToString();
            if (!repeat)
            {
                return result;
            }

            return ReplaceEach(result, searchList, replacementList, repeat, timeToLive - 1);
        }

        // Replace, character based
        //-----------------------------------------------------------------------
        /// <summary>
        /// <p>Replaces all occurrences of a character in a String with another.
        /// This is a null-safe version of <seealso cref="String#Replace(char, char)"/>.</p>
        /// 
        /// <p>A <code>null</code> string input returns <code>null</code>.
        /// An empty ("") string input returns an empty string.</p>
        /// 
        /// <pre>
        /// StringUtil.replaceChars(null, *, *)        = null
        /// StringUtil.replaceChars("", *, *)          = ""
        /// StringUtil.replaceChars("abcba", 'b', 'y') = "aycya"
        /// StringUtil.replaceChars("abcba", 'z', 'y') = "abcba"
        /// </pre>
        /// </summary>
        /// <param name="str">  String to replace characters in, may be null </param>
        /// <param name="searchChar">  the character to search for, may be null </param>
        /// <param name="replaceChar">  the character to replace, may be null </param>
        /// <returns> modified String, <code>null</code> if null string input
        /// @since 2.0 </returns>
        public static string ReplaceChars(string str, char searchChar, char replaceChar)
        {
            if (str == null)
            {
                return null;
            }
            return str.Replace(searchChar, replaceChar);
        }

        /// <summary>
        /// <p>Replaces multiple characters in a String in one go.
        /// This method can also be used to delete characters.</p>
        /// 
        /// <p>For example:<br />
        /// <code>replaceChars(&quot;hello&quot;, &quot;ho&quot;, &quot;jy&quot;) = jelly</code>.</p>
        /// 
        /// <p>A <code>null</code> string input returns <code>null</code>.
        /// An empty ("") string input returns an empty string.
        /// A null or empty set of search characters returns the input string.</p>
        /// 
        /// <p>The length of the search characters should normally equal the length
        /// of the replace characters.
        /// If the search characters is longer, then the extra search characters
        /// are deleted.
        /// If the search characters is shorter, then the extra replace characters
        /// are ignored.</p>
        /// 
        /// <pre>
        /// StringUtil.replaceChars(null, *, *)           = null
        /// StringUtil.replaceChars("", *, *)             = ""
        /// StringUtil.replaceChars("abc", null, *)       = "abc"
        /// StringUtil.replaceChars("abc", "", *)         = "abc"
        /// StringUtil.replaceChars("abc", "b", null)     = "ac"
        /// StringUtil.replaceChars("abc", "b", "")       = "ac"
        /// StringUtil.replaceChars("abcba", "bc", "yz")  = "ayzya"
        /// StringUtil.replaceChars("abcba", "bc", "y")   = "ayya"
        /// StringUtil.replaceChars("abcba", "bc", "yzx") = "ayzya"
        /// </pre>
        /// </summary>
        /// <param name="str">  String to replace characters in, may be null </param>
        /// <param name="searchChars">  a set of characters to search for, may be null </param>
        /// <param name="replaceChars">  a set of characters to replace, may be null </param>
        /// <returns> modified String, <code>null</code> if null string input
        /// @since 2.0 </returns>
        public static string ReplaceChars(string str, string searchChars, string replaceChars)
        {
            if (IsEmpty(str) || IsEmpty(searchChars))
            {
                return str;
            }
            if (replaceChars == null)
            {
                replaceChars = EMPTY;
            }
            bool modified = false;
            int replaceCharsLength = replaceChars.Length;
            int strLength = str.Length;
            StringBuilder buf = new StringBuilder(strLength);
            for (int i = 0; i < strLength; i++)
            {
                char ch = str[i];
                int index = searchChars.IndexOf(ch);
                if (index >= 0)
                {
                    modified = true;
                    if (index < replaceCharsLength)
                    {
                        buf.Append(replaceChars[index]);
                    }
                }
                else
                {
                    buf.Append(ch);
                }
            }
            if (modified)
            {
                return buf.ToString();
            }
            return str;
        }

        // Overlay
        //-----------------------------------------------------------------------
        /// <summary>
        /// <p>Overlays part of a String with another String.</p>
        /// 
        /// <pre>
        /// StringUtil.overlayString(null, *, *, *)           = NullPointerException
        /// StringUtil.overlayString(*, null, *, *)           = NullPointerException
        /// StringUtil.overlayString("", "abc", 0, 0)         = "abc"
        /// StringUtil.overlayString("abcdef", null, 2, 4)    = "abef"
        /// StringUtil.overlayString("abcdef", "", 2, 4)      = "abef"
        /// StringUtil.overlayString("abcdef", "zzzz", 2, 4)  = "abzzzzef"
        /// StringUtil.overlayString("abcdef", "zzzz", 4, 2)  = "abcdzzzzcdef"
        /// StringUtil.overlayString("abcdef", "zzzz", -1, 4) = IndexOutOfBoundsException
        /// StringUtil.overlayString("abcdef", "zzzz", 2, 8)  = IndexOutOfBoundsException
        /// </pre>
        /// </summary>
        /// <param name="text">  the String to do overlaying in, may be null </param>
        /// <param name="overlay">  the String to overlay, may be null </param>
        /// <param name="start">  the position to start overlaying at, must be valid </param>
        /// <param name="end">  the position to stop overlaying before, must be valid </param>
        /// <returns> overlayed String, <code>null</code> if null String input </returns>
        /// <exception cref="NullPointerException"> if text or overlay is null </exception>
        /// <exception cref="IndexOutOfBoundsException"> if either position is invalid </exception>
        /// @deprecated Use better named <seealso cref="#overlay(String, String, int, int)"/> instead.
        ///             Method will be removed in Commons Lang 3.0. 
        public static string OverlayString(string text, string overlay, int start, int end)
        {
            return (new StringBuilder(start + overlay.Length + text.Length - end + 1)).Append(text.Substring(0, start)).Append(overlay).Append(text.Substring(end)).ToString();
        }

        /// <summary>
        /// <p>Overlays part of a String with another String.</p>
        /// 
        /// <p>A <code>null</code> string input returns <code>null</code>.
        /// A negative index is treated as zero.
        /// An index greater than the string length is treated as the string length.
        /// The start index is always the smaller of the two indices.</p>
        /// 
        /// <pre>
        /// StringUtil.overlay(null, *, *, *)            = null
        /// StringUtil.overlay("", "abc", 0, 0)          = "abc"
        /// StringUtil.overlay("abcdef", null, 2, 4)     = "abef"
        /// StringUtil.overlay("abcdef", "", 2, 4)       = "abef"
        /// StringUtil.overlay("abcdef", "", 4, 2)       = "abef"
        /// StringUtil.overlay("abcdef", "zzzz", 2, 4)   = "abzzzzef"
        /// StringUtil.overlay("abcdef", "zzzz", 4, 2)   = "abzzzzef"
        /// StringUtil.overlay("abcdef", "zzzz", -1, 4)  = "zzzzef"
        /// StringUtil.overlay("abcdef", "zzzz", 2, 8)   = "abzzzz"
        /// StringUtil.overlay("abcdef", "zzzz", -2, -3) = "zzzzabcdef"
        /// StringUtil.overlay("abcdef", "zzzz", 8, 10)  = "abcdefzzzz"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to do overlaying in, may be null </param>
        /// <param name="overlay">  the String to overlay, may be null </param>
        /// <param name="start">  the position to start overlaying at </param>
        /// <param name="end">  the position to stop overlaying before </param>
        /// <returns> overlayed String, <code>null</code> if null String input
        /// @since 2.0 </returns>
        public static string Overlay(string str, string overlay, int start, int end)
        {
            if (str == null)
            {
                return null;
            }
            if (overlay == null)
            {
                overlay = EMPTY;
            }
            int len = str.Length;
            if (start < 0)
            {
                start = 0;
            }
            if (start > len)
            {
                start = len;
            }
            if (end < 0)
            {
                end = 0;
            }
            if (end > len)
            {
                end = len;
            }
            if (start > end)
            {
                int temp = start;
                start = end;
                end = temp;
            }
            return (new StringBuilder(len + start - end + overlay.Length + 1)).Append(str.Substring(0, start)).Append(overlay).Append(str.Substring(end)).ToString();
        }


        // Padding
        //-----------------------------------------------------------------------
        /// <summary>
        /// <p>Repeat a String <code>repeat</code> times to form a
        /// new String.</p>
        /// 
        /// <pre>
        /// StringUtil.repeat(null, 2) = null
        /// StringUtil.repeat("", 0)   = ""
        /// StringUtil.repeat("", 2)   = ""
        /// StringUtil.repeat("a", 3)  = "aaa"
        /// StringUtil.repeat("ab", 2) = "abab"
        /// StringUtil.repeat("a", -2) = ""
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to repeat, may be null </param>
        /// <param name="repeat">  number of times to repeat str, negative treated as zero </param>
        /// <returns> a new String consisting of the original String repeated,
        ///  <code>null</code> if null String input </returns>
        public static string Repeat(string str, int repeat)
        {
            // Performance tuned for 2.0 (JDK1.4)

            if (str == null)
            {
                return null;
            }
            if (repeat <= 0)
            {
                return EMPTY;
            }
            int inputLength = str.Length;
            if (repeat == 1 || inputLength == 0)
            {
                return str;
            }
            if (inputLength == 1 && repeat <= PAD_LIMIT)
            {
                return Padding(repeat, str[0]);
            }

            int outputLength = inputLength * repeat;
            switch (inputLength)
            {
                case 1:
                    char ch = str[0];
                    char[] output1 = new char[outputLength];
                    for (int i = repeat - 1; i >= 0; i--)
                    {
                        output1[i] = ch;
                    }
                    return new string(output1);
                case 2:
                    char ch0 = str[0];
                    char ch1 = str[1];
                    char[] output2 = new char[outputLength];
                    for (int i = repeat * 2 - 2; i >= 0; i--, i--)
                    {
                        output2[i] = ch0;
                        output2[i + 1] = ch1;
                    }
                    return new string(output2);
                default:
                    StringBuilder buf = new StringBuilder(outputLength);
                    for (int i = 0; i < repeat; i++)
                    {
                        buf.Append(str);
                    }
                    return buf.ToString();
            }
        }

        /// <summary>
        /// <p>Returns padding using the specified delimiter repeated
        /// to a given length.</p>
        /// 
        /// <pre>
        /// StringUtil.Padding(0, 'e')  = ""
        /// StringUtil.Padding(3, 'e')  = "eee"
        /// StringUtil.Padding(-2, 'e') = IndexOutOfBoundsException
        /// </pre>
        /// 
        /// <p>Note: this method doesn't not support padding with
        /// <a href="http://www.unicode.org/glossary/#supplementary_character">Unicode Supplementary Characters</a>
        /// as they require a pair of <code>char</code>s to be represented.
        /// If you are needing to support full I18N of your applications
        /// consider using <seealso cref="#repeat(String, int)"/> instead. 
        /// </p>
        /// </summary>
        /// <param name="repeat">  number of times to repeat delim </param>
        /// <param name="padChar">  character to repeat </param>
        /// <returns> String with repeated character </returns>
        /// <exception cref="IndexOutOfBoundsException"> if <code>repeat &lt; 0</code> </exception>
        /// <seealso cref= #repeat(String, int) </seealso>
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: private static String Padding(int repeat, char padChar) throws IndexOutOfBoundsException
        private static string Padding(int repeat, char padChar)
        {
            if (repeat < 0)
            {
                throw new System.IndexOutOfRangeException("Cannot pad a negative amount: " + repeat);
            }
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final char[] buf = new char[repeat];
            char[] buf = new char[repeat];
            for (int i = 0; i < buf.Length; i++)
            {
                buf[i] = padChar;
            }
            return new string(buf);
        }

        /// <summary>
        /// <p>Right pad a String with spaces (' ').</p>
        /// 
        /// <p>The String is padded to the size of <code>size</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.RightPad(null, *)   = null
        /// StringUtil.RightPad("", 3)     = "   "
        /// StringUtil.RightPad("bat", 3)  = "bat"
        /// StringUtil.RightPad("bat", 5)  = "bat  "
        /// StringUtil.RightPad("bat", 1)  = "bat"
        /// StringUtil.RightPad("bat", -1) = "bat"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to pad out, may be null </param>
        /// <param name="size">  the size to pad to </param>
        /// <returns> right padded String or original String if no padding is necessary,
        ///  <code>null</code> if null String input </returns>
        public static string RightPad(string str, int size)
        {
            return RightPad(str, size, ' ');
        }

        /// <summary>
        /// <p>Right pad a String with a specified character.</p>
        /// 
        /// <p>The String is padded to the size of <code>size</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.RightPad(null, *, *)     = null
        /// StringUtil.RightPad("", 3, 'z')     = "zzz"
        /// StringUtil.RightPad("bat", 3, 'z')  = "bat"
        /// StringUtil.RightPad("bat", 5, 'z')  = "batzz"
        /// StringUtil.RightPad("bat", 1, 'z')  = "bat"
        /// StringUtil.RightPad("bat", -1, 'z') = "bat"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to pad out, may be null </param>
        /// <param name="size">  the size to pad to </param>
        /// <param name="padChar">  the character to pad with </param>
        /// <returns> right padded String or original String if no padding is necessary,
        ///  <code>null</code> if null String input
        /// @since 2.0 </returns>
        public static string RightPad(string str, int size, char padChar)
        {
            if (str == null)
            {
                return null;
            }
            int pads = size - str.Length;
            if (pads <= 0)
            {
                return str; // returns original String when possible
            }
            if (pads > PAD_LIMIT)
            {
                return RightPad(str, size, Convert.ToString(padChar));
            }
            return str + Padding(pads, padChar);
        }

        /// <summary>
        /// <p>Right pad a String with a specified String.</p>
        /// 
        /// <p>The String is padded to the size of <code>size</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.RightPad(null, *, *)      = null
        /// StringUtil.RightPad("", 3, "z")      = "zzz"
        /// StringUtil.RightPad("bat", 3, "yz")  = "bat"
        /// StringUtil.RightPad("bat", 5, "yz")  = "batyz"
        /// StringUtil.RightPad("bat", 8, "yz")  = "batyzyzy"
        /// StringUtil.RightPad("bat", 1, "yz")  = "bat"
        /// StringUtil.RightPad("bat", -1, "yz") = "bat"
        /// StringUtil.RightPad("bat", 5, null)  = "bat  "
        /// StringUtil.RightPad("bat", 5, "")    = "bat  "
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to pad out, may be null </param>
        /// <param name="size">  the size to pad to </param>
        /// <param name="padStr">  the String to pad with, null or empty treated as single space </param>
        /// <returns> right padded String or original String if no padding is necessary,
        ///  <code>null</code> if null String input </returns>
        public static string RightPad(string str, int size, string padStr)
        {
            if (str == null)
            {
                return null;
            }
            if (IsEmpty(padStr))
            {
                padStr = " ";
            }
            int padLen = padStr.Length;
            int strLen = str.Length;
            int pads = size - strLen;
            if (pads <= 0)
            {
                return str; // returns original String when possible
            }
            if (padLen == 1 && pads <= PAD_LIMIT)
            {
                return RightPad(str, size, padStr[0]);
            }

            if (pads == padLen)
            {
                return str + padStr;
            }
            else if (pads < padLen)
            {
                return str + padStr.Substring(0, pads);
            }
            else
            {
                char[] padding = new char[pads];
                char[] padChars = padStr.ToCharArray();
                for (int i = 0; i < pads; i++)
                {
                    padding[i] = padChars[i % padLen];
                }
                return str + new string(padding);
            }
        }

        /// <summary>
        /// <p>Left pad a String with spaces (' ').</p>
        /// 
        /// <p>The String is padded to the size of <code>size<code>.</p>
        /// 
        /// <pre>
        /// StringUtil.LeftPad(null, *)   = null
        /// StringUtil.LeftPad("", 3)     = "   "
        /// StringUtil.LeftPad("bat", 3)  = "bat"
        /// StringUtil.LeftPad("bat", 5)  = "  bat"
        /// StringUtil.LeftPad("bat", 1)  = "bat"
        /// StringUtil.LeftPad("bat", -1) = "bat"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to pad out, may be null </param>
        /// <param name="size">  the size to pad to </param>
        /// <returns> left padded String or original String if no padding is necessary,
        ///  <code>null</code> if null String input </returns>
        public static string LeftPad(string str, int size)
        {
            return LeftPad(str, size, ' ');
        }

        /// <summary>
        /// <p>Left pad a String with a specified character.</p>
        /// 
        /// <p>Pad to a size of <code>size</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.LeftPad(null, *, *)     = null
        /// StringUtil.LeftPad("", 3, 'z')     = "zzz"
        /// StringUtil.LeftPad("bat", 3, 'z')  = "bat"
        /// StringUtil.LeftPad("bat", 5, 'z')  = "zzbat"
        /// StringUtil.LeftPad("bat", 1, 'z')  = "bat"
        /// StringUtil.LeftPad("bat", -1, 'z') = "bat"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to pad out, may be null </param>
        /// <param name="size">  the size to pad to </param>
        /// <param name="padChar">  the character to pad with </param>
        /// <returns> left padded String or original String if no padding is necessary,
        ///  <code>null</code> if null String input
        /// @since 2.0 </returns>
        public static string LeftPad(string str, int size, char padChar)
        {
            if (str == null)
            {
                return null;
            }
            int pads = size - str.Length;
            if (pads <= 0)
            {
                return str; // returns original String when possible
            }
            if (pads > PAD_LIMIT)
            {
                return LeftPad(str, size, Convert.ToString(padChar));
            }
            return Padding(pads, padChar) + str;
        }

        /// <summary>
        /// <p>Left pad a String with a specified String.</p>
        /// 
        /// <p>Pad to a size of <code>size</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.LeftPad(null, *, *)      = null
        /// StringUtil.LeftPad("", 3, "z")      = "zzz"
        /// StringUtil.LeftPad("bat", 3, "yz")  = "bat"
        /// StringUtil.LeftPad("bat", 5, "yz")  = "yzbat"
        /// StringUtil.LeftPad("bat", 8, "yz")  = "yzyzybat"
        /// StringUtil.LeftPad("bat", 1, "yz")  = "bat"
        /// StringUtil.LeftPad("bat", -1, "yz") = "bat"
        /// StringUtil.LeftPad("bat", 5, null)  = "  bat"
        /// StringUtil.LeftPad("bat", 5, "")    = "  bat"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to pad out, may be null </param>
        /// <param name="size">  the size to pad to </param>
        /// <param name="padStr">  the String to pad with, null or empty treated as single space </param>
        /// <returns> left padded String or original String if no padding is necessary,
        ///  <code>null</code> if null String input </returns>
        public static string LeftPad(string str, int size, string padStr)
        {
            if (str == null)
            {
                return null;
            }
            if (IsEmpty(padStr))
            {
                padStr = " ";
            }
            int padLen = padStr.Length;
            int strLen = str.Length;
            int pads = size - strLen;
            if (pads <= 0)
            {
                return str; // returns original String when possible
            }
            if (padLen == 1 && pads <= PAD_LIMIT)
            {
                return LeftPad(str, size, padStr[0]);
            }

            if (pads == padLen)
            {
                return padStr + str;
            }
            else if (pads < padLen)
            {
                return padStr.Substring(0, pads) + str;
            }
            else
            {
                char[] padding = new char[pads];
                char[] padChars = padStr.ToCharArray();
                for (int i = 0; i < pads; i++)
                {
                    padding[i] = padChars[i % padLen];
                }
                return (new string(padding)) + str;
            }
        }

        /// <summary>
        /// Gets a String's length or <code>0</code> if the String is <code>null</code>.
        /// </summary>
        /// <param name="str">
        ///            a String or <code>null</code> </param>
        /// <returns> String length or <code>0</code> if the String is <code>null</code>.
        /// @since 2.4 </returns>
        public static int Length(string str)
        {
            return str == null ? 0 : str.Length;
        }

        // Centering
        //-----------------------------------------------------------------------
        /// <summary>
        /// <p>Centers a String in a larger String of size <code>size</code>
        /// using the space character (' ').<p>
        /// 
        /// <p>If the size is less than the String length, the String is returned.
        /// A <code>null</code> String returns <code>null</code>.
        /// A negative size is treated as zero.</p>
        /// 
        /// <p>Equivalent to <code>Center(str, size, " ")</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.Center(null, *)   = null
        /// StringUtil.Center("", 4)     = "    "
        /// StringUtil.Center("ab", -1)  = "ab"
        /// StringUtil.Center("ab", 4)   = " ab "
        /// StringUtil.Center("abcd", 2) = "abcd"
        /// StringUtil.Center("a", 4)    = " a  "
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to center, may be null </param>
        /// <param name="size">  the int size of new String, negative treated as zero </param>
        /// <returns> centered String, <code>null</code> if null String input </returns>
        public static string Center(string str, int size)
        {
            return Center(str, size, ' ');
        }

        /// <summary>
        /// <p>Centers a String in a larger String of size <code>size</code>.
        /// Uses a supplied character as the value to pad the String with.</p>
        /// 
        /// <p>If the size is less than the String length, the String is returned.
        /// A <code>null</code> String returns <code>null</code>.
        /// A negative size is treated as zero.</p>
        /// 
        /// <pre>
        /// StringUtil.Center(null, *, *)     = null
        /// StringUtil.Center("", 4, ' ')     = "    "
        /// StringUtil.Center("ab", -1, ' ')  = "ab"
        /// StringUtil.Center("ab", 4, ' ')   = " ab"
        /// StringUtil.Center("abcd", 2, ' ') = "abcd"
        /// StringUtil.Center("a", 4, ' ')    = " a  "
        /// StringUtil.Center("a", 4, 'y')    = "yayy"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to center, may be null </param>
        /// <param name="size">  the int size of new String, negative treated as zero </param>
        /// <param name="padChar">  the character to pad the new String with </param>
        /// <returns> centered String, <code>null</code> if null String input
        /// @since 2.0 </returns>
        public static string Center(string str, int size, char padChar)
        {
            if (str == null || size <= 0)
            {
                return str;
            }
            int strLen = str.Length;
            int pads = size - strLen;
            if (pads <= 0)
            {
                return str;
            }
            str = LeftPad(str, strLen + pads / 2, padChar);
            str = RightPad(str, size, padChar);
            return str;
        }

        /// <summary>
        /// <p>Centers a String in a larger String of size <code>size</code>.
        /// Uses a supplied String as the value to pad the String with.</p>
        /// 
        /// <p>If the size is less than the String length, the String is returned.
        /// A <code>null</code> String returns <code>null</code>.
        /// A negative size is treated as zero.</p>
        /// 
        /// <pre>
        /// StringUtil.Center(null, *, *)     = null
        /// StringUtil.Center("", 4, " ")     = "    "
        /// StringUtil.Center("ab", -1, " ")  = "ab"
        /// StringUtil.Center("ab", 4, " ")   = " ab"
        /// StringUtil.Center("abcd", 2, " ") = "abcd"
        /// StringUtil.Center("a", 4, " ")    = " a  "
        /// StringUtil.Center("a", 4, "yz")   = "yayz"
        /// StringUtil.Center("abc", 7, null) = "  abc  "
        /// StringUtil.Center("abc", 7, "")   = "  abc  "
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to center, may be null </param>
        /// <param name="size">  the int size of new String, negative treated as zero </param>
        /// <param name="padStr">  the String to pad the new String with, must not be null or empty </param>
        /// <returns> centered String, <code>null</code> if null String input </returns>
        /// <exception cref="IllegalArgumentException"> if padStr is <code>null</code> or empty </exception>
        public static string Center(string str, int size, string padStr)
        {
            if (str == null || size <= 0)
            {
                return str;
            }
            if (IsEmpty(padStr))
            {
                padStr = " ";
            }
            int strLen = str.Length;
            int pads = size - strLen;
            if (pads <= 0)
            {
                return str;
            }
            str = LeftPad(str, strLen + pads / 2, padStr);
            str = RightPad(str, size, padStr);
            return str;
        }

        // Case conversion
        //-----------------------------------------------------------------------
        /// <summary>
        /// <p>Converts a String to upper case as per <seealso cref="String#toUpperCase()"/>.</p>
        /// 
        /// <p>A <code>null</code> input String returns <code>null</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.upperCase(null)  = null
        /// StringUtil.upperCase("")    = ""
        /// StringUtil.upperCase("aBc") = "ABC"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to upper case, may be null </param>
        /// <returns> the upper cased String, <code>null</code> if null String input </returns>
        public static string UpperCase(string str)
        {
            if (str == null)
            {
                return null;
            }
            return str.ToUpper();
        }

        /// <summary>
        /// <p>Converts a String to lower case as per <seealso cref="String#toLowerCase()"/>.</p>
        /// 
        /// <p>A <code>null</code> input String returns <code>null</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.lowerCase(null)  = null
        /// StringUtil.lowerCase("")    = ""
        /// StringUtil.lowerCase("aBc") = "abc"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to lower case, may be null </param>
        /// <returns> the lower cased String, <code>null</code> if null String input </returns>
        public static string LowerCase(string str)
        {
            if (str == null)
            {
                return null;
            }
            return str.ToLower();
        }

        /// <summary>
        /// <p>Capitalizes a String changing the first letter to title case as
        /// per <seealso cref="Character#toTitleCase(char)"/>. No other letters are changed.</p>
        /// 
        /// <p>For a word based algorithm, see <seealso cref="WordUtils#Capitalize(String)"/>.
        /// A <code>null</code> input String returns <code>null</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.Capitalize(null)  = null
        /// StringUtil.Capitalize("")    = ""
        /// StringUtil.Capitalize("cat") = "Cat"
        /// StringUtil.Capitalize("cAt") = "CAt"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to Capitalize, may be null </param>
        /// <returns> the Capitalized String, <code>null</code> if null String input </returns>
        /// <seealso cref= WordUtils#Capitalize(String) </seealso>
        /// <seealso cref= #unCapitalize(String)
        /// @since 2.0 </seealso>
        public static string Capitalize(string str)
        {
            int strLen;
            if (str == null || (strLen = str.Length) == 0)
            {
                return str;
            }
            return (new StringBuilder(strLen)).Append(char.ToLower(str[0])).Append(str.Substring(1)).ToString();
        }

        /// <summary>
        /// <p>Capitalizes a String changing the first letter to title case as
        /// per <seealso cref="Character#toTitleCase(char)"/>. No other letters are changed.</p>
        /// </summary>
        /// <param name="str">  the String to Capitalize, may be null </param>
        /// <returns> the Capitalized String, <code>null</code> if null String input </returns>
        /// @deprecated Use the standardly named <seealso cref="#Capitalize(String)"/>.
        ///             Method will be removed in Commons Lang 3.0. 
        public static string Capitalise(string str)
        {
            return Capitalize(str);
        }

        /// <summary>
        /// <p>UnCapitalizes a String changing the first letter to title case as
        /// per <seealso cref="Character#toLowerCase(char)"/>. No other letters are changed.</p>
        /// 
        /// <p>For a word based algorithm, see <seealso cref="WordUtils#unCapitalize(String)"/>.
        /// A <code>null</code> input String returns <code>null</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.unCapitalize(null)  = null
        /// StringUtil.unCapitalize("")    = ""
        /// StringUtil.unCapitalize("Cat") = "cat"
        /// StringUtil.unCapitalize("CAT") = "cAT"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to unCapitalize, may be null </param>
        /// <returns> the unCapitalized String, <code>null</code> if null String input </returns>
        /// <seealso cref= WordUtils#unCapitalize(String) </seealso>
        /// <seealso cref= #Capitalize(String)
        /// @since 2.0 </seealso>
        public static string UnCapitalize(string str)
        {
            int strLen;
            if (str == null || (strLen = str.Length) == 0)
            {
                return str;
            }
            return (new StringBuilder(strLen)).Append(char.ToLower(str[0])).Append(str.Substring(1)).ToString();
        }

        /// <summary>
        /// <p>UnCapitalizes a String changing the first letter to title case as
        /// per <seealso cref="Character#toLowerCase(char)"/>. No other letters are changed.</p>
        /// </summary>
        /// <param name="str">  the String to unCapitalize, may be null </param>
        /// <returns> the unCapitalized String, <code>null</code> if null String input </returns>
        /// @deprecated Use the standardly named <seealso cref="#unCapitalize(String)"/>.
        ///             Method will be removed in Commons Lang 3.0. 
        public static string Uncapitalise(string str)
        {
            return UnCapitalize(str);
        }

        /// <summary>
        /// <p>Swaps the case of a String changing upper and title case to
        /// lower case, and lower case to upper case.</p>
        /// 
        /// <ul>
        ///  <li>Upper case character converts to Lower case</li>
        ///  <li>Title case character converts to Lower case</li>
        ///  <li>Lower case character converts to Upper case</li>
        /// </ul>
        /// 
        /// <p>For a word based algorithm, see <seealso cref="WordUtils#swapCase(String)"/>.
        /// A <code>null</code> input String returns <code>null</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.swapCase(null)                 = null
        /// StringUtil.swapCase("")                   = ""
        /// StringUtil.swapCase("The dog has a BONE") = "tHE DOG HAS A bone"
        /// </pre>
        /// 
        /// <p>NOTE: This method changed in Lang version 2.0.
        /// It no longer performs a word based algorithm.
        /// If you only use ASCII, you will notice no change.
        /// That functionality is available in WordUtils.</p>
        /// </summary>
        /// <param name="str">  the String to swap case, may be null </param>
        /// <returns> the changed String, <code>null</code> if null String input </returns>
        public static string SwapCase(string str)
        {
            int strLen;
            if (str == null || (strLen = str.Length) == 0)
            {
                return str;
            }
            StringBuilder buffer = new StringBuilder(strLen);

            char ch = (char)0;
            for (int i = 0; i < strLen; i++)
            {
                ch = str[i];
                if (char.IsUpper(ch))
                {
                    ch = char.ToLower(ch);
                }
                //else if (char.isTitleCase(ch))
                //{
                //    ch = char.ToLower(ch);
                //}
                else if (char.IsLower(ch))
                {
                    ch = char.ToUpper(ch);
                }
                buffer.Append(ch);
            }
            return buffer.ToString();
        }


        // Count matches
        //-----------------------------------------------------------------------
        /// <summary>
        /// <p>Counts how many times the substring appears in the larger String.</p>
        /// 
        /// <p>A <code>null</code> or empty ("") String input returns <code>0</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.countMatches(null, *)       = 0
        /// StringUtil.countMatches("", *)         = 0
        /// StringUtil.countMatches("abba", null)  = 0
        /// StringUtil.countMatches("abba", "")    = 0
        /// StringUtil.countMatches("abba", "a")   = 2
        /// StringUtil.countMatches("abba", "ab")  = 1
        /// StringUtil.countMatches("abba", "xxx") = 0
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="sub">  the substring to count, may be null </param>
        /// <returns> the number of occurrences, 0 if either String is <code>null</code> </returns>
        public static int CountMatches(string str, string sub)
        {
            if (IsEmpty(str) || IsEmpty(sub))
            {
                return 0;
            }
            int count = 0;
            int idx = 0;
            while ((idx = str.IndexOf(sub, idx)) != -1)
            {
                count++;
                idx += sub.Length;
            }
            return count;
        }

        // Character Tests
        //-----------------------------------------------------------------------
        /// <summary>
        /// <p>Checks if the String contains only unicode letters.</p>
        /// 
        /// <p><code>null</code> will return <code>false</code>.
        /// An empty String ("") will return <code>true</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.isAlpha(null)   = false
        /// StringUtil.isAlpha("")     = true
        /// StringUtil.isAlpha("  ")   = false
        /// StringUtil.isAlpha("abc")  = true
        /// StringUtil.isAlpha("ab2c") = false
        /// StringUtil.isAlpha("ab-c") = false
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <returns> <code>true</code> if only contains letters, and is non-null </returns>
        public static bool IsAlpha(string str)
        {
            if (str == null)
            {
                return false;
            }
            int sz = str.Length;
            for (int i = 0; i < sz; i++)
            {
                if (char.IsLetter(str[i]) == false)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// <p>Checks if the String contains only unicode letters and
        /// space (' ').</p>
        /// 
        /// <p><code>null</code> will return <code>false</code>
        /// An empty String ("") will return <code>true</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.isAlphaSpace(null)   = false
        /// StringUtil.isAlphaSpace("")     = true
        /// StringUtil.isAlphaSpace("  ")   = true
        /// StringUtil.isAlphaSpace("abc")  = true
        /// StringUtil.isAlphaSpace("ab c") = true
        /// StringUtil.isAlphaSpace("ab2c") = false
        /// StringUtil.isAlphaSpace("ab-c") = false
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <returns> <code>true</code> if only contains letters and space,
        ///  and is non-null </returns>
        public static bool IsAlphaSpace(string str)
        {
            if (str == null)
            {
                return false;
            }
            int sz = str.Length;
            for (int i = 0; i < sz; i++)
            {
                if ((char.IsLetter(str[i]) == false) && (str[i] != ' '))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// <p>Checks if the String contains only unicode letters or digits.</p>
        /// 
        /// <p><code>null</code> will return <code>false</code>.
        /// An empty String ("") will return <code>true</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.isAlphanumeric(null)   = false
        /// StringUtil.isAlphanumeric("")     = true
        /// StringUtil.isAlphanumeric("  ")   = false
        /// StringUtil.isAlphanumeric("abc")  = true
        /// StringUtil.isAlphanumeric("ab c") = false
        /// StringUtil.isAlphanumeric("ab2c") = true
        /// StringUtil.isAlphanumeric("ab-c") = false
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <returns> <code>true</code> if only contains letters or digits,
        ///  and is non-null </returns>
        public static bool IsAlphanumeric(string str)
        {
            if (str == null)
            {
                return false;
            }
            int sz = str.Length;
            for (int i = 0; i < sz; i++)
            {
                if (char.IsLetterOrDigit(str[i]) == false)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// <p>Checks if the String contains only unicode letters, digits
        /// or space (<code>' '</code>).</p>
        /// 
        /// <p><code>null</code> will return <code>false</code>.
        /// An empty String ("") will return <code>true</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.isAlphanumeric(null)   = false
        /// StringUtil.isAlphanumeric("")     = true
        /// StringUtil.isAlphanumeric("  ")   = true
        /// StringUtil.isAlphanumeric("abc")  = true
        /// StringUtil.isAlphanumeric("ab c") = true
        /// StringUtil.isAlphanumeric("ab2c") = true
        /// StringUtil.isAlphanumeric("ab-c") = false
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <returns> <code>true</code> if only contains letters, digits or space,
        ///  and is non-null </returns>
        public static bool IsAlphanumericSpace(string str)
        {
            if (str == null)
            {
                return false;
            }
            int sz = str.Length;
            for (int i = 0; i < sz; i++)
            {
                if ((char.IsLetterOrDigit(str[i]) == false) && (str[i] != ' '))
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// <p>Checks if the String contains only unicode digits.
        /// A decimal point is not a unicode digit and returns false.</p>
        /// 
        /// <p><code>null</code> will return <code>false</code>.
        /// An empty String ("") will return <code>true</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.isNumeric(null)   = false
        /// StringUtil.isNumeric("")     = true
        /// StringUtil.isNumeric("  ")   = false
        /// StringUtil.isNumeric("123")  = true
        /// StringUtil.isNumeric("12 3") = false
        /// StringUtil.isNumeric("ab2c") = false
        /// StringUtil.isNumeric("12-3") = false
        /// StringUtil.isNumeric("12.3") = false
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <returns> <code>true</code> if only contains digits, and is non-null </returns>
        public static bool IsNumeric(string str)
        {
            if (str == null)
            {
                return false;
            }
            int sz = str.Length;
            for (int i = 0; i < sz; i++)
            {
                if (char.IsDigit(str[i]) == false)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// <p>Checks if the String contains only unicode digits or space
        /// (<code>' '</code>).
        /// A decimal point is not a unicode digit and returns false.</p>
        /// 
        /// <p><code>null</code> will return <code>false</code>.
        /// An empty String ("") will return <code>true</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.isNumeric(null)   = false
        /// StringUtil.isNumeric("")     = true
        /// StringUtil.isNumeric("  ")   = true
        /// StringUtil.isNumeric("123")  = true
        /// StringUtil.isNumeric("12 3") = true
        /// StringUtil.isNumeric("ab2c") = false
        /// StringUtil.isNumeric("12-3") = false
        /// StringUtil.isNumeric("12.3") = false
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <returns> <code>true</code> if only contains digits or space,
        ///  and is non-null </returns>
        public static bool IsNumericSpace(string str)
        {
            if (str == null)
            {
                return false;
            }
            int sz = str.Length;
            for (int i = 0; i < sz; i++)
            {
                if ((char.IsDigit(str[i]) == false) && (str[i] != ' '))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// <p>Checks if the String contains only whitespace.</p>
        /// 
        /// <p><code>null</code> will return <code>false</code>.
        /// An empty String ("") will return <code>true</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.isWhitespace(null)   = false
        /// StringUtil.isWhitespace("")     = true
        /// StringUtil.isWhitespace("  ")   = true
        /// StringUtil.isWhitespace("abc")  = false
        /// StringUtil.isWhitespace("ab2c") = false
        /// StringUtil.isWhitespace("ab-c") = false
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <returns> <code>true</code> if only contains whitespace, and is non-null
        /// @since 2.0 </returns>
        public static bool IsWhitespace(string str)
        {
            if (str == null)
            {
                return false;
            }
            int sz = str.Length;
            for (int i = 0; i < sz; i++)
            {
                if ((char.IsWhiteSpace(str[i]) == false))
                {
                    return false;
                }
            }
            return true;
        }

        // Defaults
        //-----------------------------------------------------------------------
        /// <summary>
        /// <p>Returns either the passed in String,
        /// or if the String is <code>null</code>, an empty String ("").</p>
        /// 
        /// <pre>
        /// StringUtil.defaultString(null)  = ""
        /// StringUtil.defaultString("")    = ""
        /// StringUtil.defaultString("bat") = "bat"
        /// </pre>
        /// </summary>
        /// <seealso cref= ObjectUtils#toString(Object) </seealso>
        /// <seealso cref= String#valueOf(Object) </seealso>
        /// <param name="str">  the String to check, may be null </param>
        /// <returns> the passed in String, or the empty String if it
        ///  was <code>null</code> </returns>
        public static string DefaultString(string str)
        {
            return str == null ? EMPTY : str;
        }

        /// <summary>
        /// <p>Returns either the passed in String, or if the String is
        /// <code>null</code>, the value of <code>defaultStr</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.defaultString(null, "NULL")  = "NULL"
        /// StringUtil.defaultString("", "NULL")    = ""
        /// StringUtil.defaultString("bat", "NULL") = "bat"
        /// </pre>
        /// </summary>
        /// <seealso cref= ObjectUtils#toString(Object,String) </seealso>
        /// <seealso cref= String#valueOf(Object) </seealso>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="defaultStr">  the default String to return
        ///  if the input is <code>null</code>, may be null </param>
        /// <returns> the passed in String, or the default if it was <code>null</code> </returns>
        public static string DefaultString(string str, string defaultStr)
        {
            return str == null ? defaultStr : str;
        }

        /// <summary>
        /// <p>Returns either the passed in String, or if the String is
        /// empty or <code>null</code>, the value of <code>defaultStr</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.defaultIfEmpty(null, "NULL")  = "NULL"
        /// StringUtil.defaultIfEmpty("", "NULL")    = "NULL"
        /// StringUtil.defaultIfEmpty("bat", "NULL") = "bat"
        /// </pre>
        /// </summary>
        /// <seealso cref= StringUtil#defaultString(String, String) </seealso>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="defaultStr">  the default String to return
        ///  if the input is empty ("") or <code>null</code>, may be null </param>
        /// <returns> the passed in String, or the default </returns>
        public static string DefaultIfEmpty(string str, string defaultStr)
        {
            return StringUtil.IsEmpty(str) ? defaultStr : str;
        }

        // Reversing
        //-----------------------------------------------------------------------
        /// <summary>
        /// <p>Reverses a String as per <seealso cref="StringBuilder#reverse()"/>.</p>
        /// 
        /// <p>A <code>null</code> String returns <code>null</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.reverse(null)  = null
        /// StringUtil.reverse("")    = ""
        /// StringUtil.reverse("bat") = "tab"
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to reverse, may be null </param>
        /// <returns> the reversed String, <code>null</code> if null String input </returns>
        public static string Reverse(string str)
        {
            if (str == null)
            {
                return null;
            }
            //JAVA TO C# CONVERTER TODO TASK: There is no .NET StringBuilder equivalent to the Java 'reverse' method:
            //return (new StringBuilder(str)).reverse().ToString();
            return str;
        }
            



        // Abbreviating
        //-----------------------------------------------------------------------
        /// <summary>
        /// <p>Abbreviates a String using ellipses. This will turn
        /// "Now is the time for all good men" into "Now is the time for..."</p>
        /// 
        /// <p>Specifically:
        /// <ul>
        ///   <li>If <code>str</code> is less than <code>maxWidth</code> characters
        ///       long, return it.</li>
        ///   <li>Else Abbreviate it to <code>(Substring(str, 0, max-3) + "...")</code>.</li>
        ///   <li>If <code>maxWidth</code> is less than <code>4</code>, throw an
        ///       <code>IllegalArgumentException</code>.</li>
        ///   <li>In no case will it return a String of length greater than
        ///       <code>maxWidth</code>.</li>
        /// </ul>
        /// </p>
        /// 
        /// <pre>
        /// StringUtil.Abbreviate(null, *)      = null
        /// StringUtil.Abbreviate("", 4)        = ""
        /// StringUtil.Abbreviate("abcdefg", 6) = "abc..."
        /// StringUtil.Abbreviate("abcdefg", 7) = "abcdefg"
        /// StringUtil.Abbreviate("abcdefg", 8) = "abcdefg"
        /// StringUtil.Abbreviate("abcdefg", 4) = "a..."
        /// StringUtil.Abbreviate("abcdefg", 3) = IllegalArgumentException
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="maxWidth">  maximum length of result String, must be at least 4 </param>
        /// <returns> Abbreviated String, <code>null</code> if null String input </returns>
        /// <exception cref="IllegalArgumentException"> if the width is too small
        /// @since 2.0 </exception>
        public static string Abbreviate(string str, int maxWidth)
        {
            return Abbreviate(str, 0, maxWidth);
        }

        /// <summary>
        /// <p>Abbreviates a String using ellipses. This will turn
        /// "Now is the time for all good men" into "...is the time for..."</p>
        /// 
        /// <p>Works like <code>Abbreviate(String, int)</code>, but allows you to specify
        /// a "left edge" offset.  Note that this left edge is not necessarily going to
        /// be the leftmost character in the result, or the first character following the
        /// ellipses, but it will appear somewhere in the result.
        /// 
        /// <p>In no case will it return a String of length greater than
        /// <code>maxWidth</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.Abbreviate(null, *, *)                = null
        /// StringUtil.Abbreviate("", 0, 4)                  = ""
        /// StringUtil.Abbreviate("abcdefghijklmno", -1, 10) = "abcdefg..."
        /// StringUtil.Abbreviate("abcdefghijklmno", 0, 10)  = "abcdefg..."
        /// StringUtil.Abbreviate("abcdefghijklmno", 1, 10)  = "abcdefg..."
        /// StringUtil.Abbreviate("abcdefghijklmno", 4, 10)  = "abcdefg..."
        /// StringUtil.Abbreviate("abcdefghijklmno", 5, 10)  = "...fghi..."
        /// StringUtil.Abbreviate("abcdefghijklmno", 6, 10)  = "...ghij..."
        /// StringUtil.Abbreviate("abcdefghijklmno", 8, 10)  = "...ijklmno"
        /// StringUtil.Abbreviate("abcdefghijklmno", 10, 10) = "...ijklmno"
        /// StringUtil.Abbreviate("abcdefghijklmno", 12, 10) = "...ijklmno"
        /// StringUtil.Abbreviate("abcdefghij", 0, 3)        = IllegalArgumentException
        /// StringUtil.Abbreviate("abcdefghij", 5, 6)        = IllegalArgumentException
        /// </pre>
        /// </summary>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="offset">  left edge of source String </param>
        /// <param name="maxWidth">  maximum length of result String, must be at least 4 </param>
        /// <returns> Abbreviated String, <code>null</code> if null String input </returns>
        /// <exception cref="IllegalArgumentException"> if the width is too small
        /// @since 2.0 </exception>
        public static string Abbreviate(string str, int offset, int maxWidth)
        {
            if (str == null)
            {
                return null;
            }
            if (maxWidth < 4)
            {
                throw new System.ArgumentException("Minimum abbreviation width is 4");
            }
            if (str.Length <= maxWidth)
            {
                return str;
            }
            if (offset > str.Length)
            {
                offset = str.Length;
            }
            if ((str.Length - offset) < (maxWidth - 3))
            {
                offset = str.Length - (maxWidth - 3);
            }
            if (offset <= 4)
            {
                return str.Substring(0, maxWidth - 3) + "...";
            }
            if (maxWidth < 7)
            {
                throw new System.ArgumentException("Minimum abbreviation width with offset is 7");
            }
            if ((offset + (maxWidth - 3)) < str.Length)
            {
                return "..." + Abbreviate(str.Substring(offset), maxWidth - 3);
            }
            return "..." + str.Substring(str.Length - (maxWidth - 3));
        }

        // Difference
        //-----------------------------------------------------------------------
        /// <summary>
        /// <p>Compares two Strings, and returns the portion where they differ.
        /// (More precisely, return the remainder of the second String,
        /// starting from where it's different from the first.)</p>
        /// 
        /// <p>For example,
        /// <code>difference("i am a machine", "i am a robot") -> "robot"</code>.</p>
        /// 
        /// <pre>
        /// StringUtil.difference(null, null) = null
        /// StringUtil.difference("", "") = ""
        /// StringUtil.difference("", "abc") = "abc"
        /// StringUtil.difference("abc", "") = ""
        /// StringUtil.difference("abc", "abc") = ""
        /// StringUtil.difference("ab", "abxyz") = "xyz"
        /// StringUtil.difference("abcde", "abxyz") = "xyz"
        /// StringUtil.difference("abcde", "xyz") = "xyz"
        /// </pre>
        /// </summary>
        /// <param name="str1">  the first String, may be null </param>
        /// <param name="str2">  the second String, may be null </param>
        /// <returns> the portion of str2 where it differs from str1; returns the
        /// empty String if they are equal
        /// @since 2.0 </returns>
        public static string Difference(string str1, string str2)
        {
            if (str1 == null)
            {
                return str2;
            }
            if (str2 == null)
            {
                return str1;
            }
            int at = IndexOfDifference(str1, str2);
            if (at == -1)
            {
                return EMPTY;
            }
            return str2.Substring(at);
        }

        /// <summary>
        /// <p>Compares two Strings, and returns the index at which the
        /// Strings begin to differ.</p>
        /// 
        /// <p>For example,
        /// <code>IndexOfDifference("i am a machine", "i am a robot") -> 7</code></p>
        /// 
        /// <pre>
        /// StringUtil.IndexOfDifference(null, null) = -1
        /// StringUtil.IndexOfDifference("", "") = -1
        /// StringUtil.IndexOfDifference("", "abc") = 0
        /// StringUtil.IndexOfDifference("abc", "") = 0
        /// StringUtil.IndexOfDifference("abc", "abc") = -1
        /// StringUtil.IndexOfDifference("ab", "abxyz") = 2
        /// StringUtil.IndexOfDifference("abcde", "abxyz") = 2
        /// StringUtil.IndexOfDifference("abcde", "xyz") = 0
        /// </pre>
        /// </summary>
        /// <param name="str1">  the first String, may be null </param>
        /// <param name="str2">  the second String, may be null </param>
        /// <returns> the index where str2 and str1 begin to differ; -1 if they are equal
        /// @since 2.0 </returns>
        public static int IndexOfDifference(string str1, string str2)
        {
            if (str1 == str2)
            {
                return -1;
            }
            if (str1 == null || str2 == null)
            {
                return 0;
            }
            int i;
            for (i = 0; i < str1.Length && i < str2.Length; ++i)
            {
                if (str1[i] != str2[i])
                {
                    break;
                }
            }
            if (i < str2.Length || i < str1.Length)
            {
                return i;
            }
            return -1;
        }

        /// <summary>
        /// <p>Compares all Strings in an array and returns the index at which the
        /// Strings begin to differ.</p>
        /// 
        /// <p>For example,
        /// <code>IndexOfDifference(new String[] {"i am a machine", "i am a robot"}) -> 7</code></p>
        /// 
        /// <pre>
        /// StringUtil.IndexOfDifference(null) = -1
        /// StringUtil.IndexOfDifference(new String[] {}) = -1
        /// StringUtil.IndexOfDifference(new String[] {"abc"}) = -1
        /// StringUtil.IndexOfDifference(new String[] {null, null}) = -1
        /// StringUtil.IndexOfDifference(new String[] {"", ""}) = -1
        /// StringUtil.IndexOfDifference(new String[] {"", null}) = 0
        /// StringUtil.IndexOfDifference(new String[] {"abc", null, null}) = 0
        /// StringUtil.IndexOfDifference(new String[] {null, null, "abc"}) = 0
        /// StringUtil.IndexOfDifference(new String[] {"", "abc"}) = 0
        /// StringUtil.IndexOfDifference(new String[] {"abc", ""}) = 0
        /// StringUtil.IndexOfDifference(new String[] {"abc", "abc"}) = -1
        /// StringUtil.IndexOfDifference(new String[] {"abc", "a"}) = 1
        /// StringUtil.IndexOfDifference(new String[] {"ab", "abxyz"}) = 2
        /// StringUtil.IndexOfDifference(new String[] {"abcde", "abxyz"}) = 2
        /// StringUtil.IndexOfDifference(new String[] {"abcde", "xyz"}) = 0
        /// StringUtil.IndexOfDifference(new String[] {"xyz", "abcde"}) = 0
        /// StringUtil.IndexOfDifference(new String[] {"i am a machine", "i am a robot"}) = 7
        /// </pre>
        /// </summary>
        /// <param name="strs">  array of strings, entries may be null </param>
        /// <returns> the index where the strings begin to differ; -1 if they are all equal
        /// @since 2.4 </returns>
        public static int IndexOfDifference(string[] strs)
        {
            if (strs == null || strs.Length <= 1)
            {
                return -1;
            }
            bool anyStringNull = false;
            bool allStringsNull = true;
            int arrayLen = strs.Length;
            int shortestStrLen = int.MaxValue;
            int longestStrLen = 0;

            // find the min and max string lengths; this avoids checking to make
            // sure we are not exceeding the length of the string each time through
            // the bottom loop.
            for (int i = 0; i < arrayLen; i++)
            {
                if (strs[i] == null)
                {
                    anyStringNull = true;
                    shortestStrLen = 0;
                }
                else
                {
                    allStringsNull = false;
                    shortestStrLen = Math.Min(strs[i].Length, shortestStrLen);
                    longestStrLen = Math.Max(strs[i].Length, longestStrLen);
                }
            }

            // handle lists containing all nulls or all empty strings
            if (allStringsNull || (longestStrLen == 0 && !anyStringNull))
            {
                return -1;
            }

            // handle lists containing some nulls or some empty strings
            if (shortestStrLen == 0)
            {
                return 0;
            }

            // find the position with the first difference across all strings
            int firstDiff = -1;
            for (int stringPos = 0; stringPos < shortestStrLen; stringPos++)
            {
                char comparisonChar = strs[0][stringPos];
                for (int arrayPos = 1; arrayPos < arrayLen; arrayPos++)
                {
                    if (strs[arrayPos][stringPos] != comparisonChar)
                    {
                        firstDiff = stringPos;
                        break;
                    }
                }
                if (firstDiff != -1)
                {
                    break;
                }
            }

            if (firstDiff == -1 && shortestStrLen != longestStrLen)
            {
                // we compared all of the characters up to the length of the
                // shortest string and didn't find a match, but the string lengths
                // vary, so return the length of the shortest string.
                return shortestStrLen;
            }
            return firstDiff;
        }

        /// <summary>
        /// <p>Compares all Strings in an array and returns the initial sequence of 
        /// characters that is common to all of them.</p>
        /// 
        /// <p>For example,
        /// <code>getCommonPrefix(new String[] {"i am a machine", "i am a robot"}) -> "i am a "</code></p>
        /// 
        /// <pre>
        /// StringUtil.getCommonPrefix(null) = ""
        /// StringUtil.getCommonPrefix(new String[] {}) = ""
        /// StringUtil.getCommonPrefix(new String[] {"abc"}) = "abc"
        /// StringUtil.getCommonPrefix(new String[] {null, null}) = ""
        /// StringUtil.getCommonPrefix(new String[] {"", ""}) = ""
        /// StringUtil.getCommonPrefix(new String[] {"", null}) = ""
        /// StringUtil.getCommonPrefix(new String[] {"abc", null, null}) = ""
        /// StringUtil.getCommonPrefix(new String[] {null, null, "abc"}) = ""
        /// StringUtil.getCommonPrefix(new String[] {"", "abc"}) = ""
        /// StringUtil.getCommonPrefix(new String[] {"abc", ""}) = ""
        /// StringUtil.getCommonPrefix(new String[] {"abc", "abc"}) = "abc"
        /// StringUtil.getCommonPrefix(new String[] {"abc", "a"}) = "a"
        /// StringUtil.getCommonPrefix(new String[] {"ab", "abxyz"}) = "ab"
        /// StringUtil.getCommonPrefix(new String[] {"abcde", "abxyz"}) = "ab"
        /// StringUtil.getCommonPrefix(new String[] {"abcde", "xyz"}) = ""
        /// StringUtil.getCommonPrefix(new String[] {"xyz", "abcde"}) = ""
        /// StringUtil.getCommonPrefix(new String[] {"i am a machine", "i am a robot"}) = "i am a "
        /// </pre>
        /// </summary>
        /// <param name="strs">  array of String objects, entries may be null </param>
        /// <returns> the initial sequence of characters that are common to all Strings
        /// in the array; empty String if the array is null, the elements are all null 
        /// or if there is no common prefix. 
        /// @since 2.4 </returns>
        public static string GetCommonPrefix(string[] strs)
        {
            if (strs == null || strs.Length == 0)
            {
                return EMPTY;
            }
            int smallestIndexOfDiff = IndexOfDifference(strs);
            if (smallestIndexOfDiff == -1)
            {
                // all strings were identical
                if (strs[0] == null)
                {
                    return EMPTY;
                }
                return strs[0];
            }
            else if (smallestIndexOfDiff == 0)
            {
                // there were no common initial characters
                return EMPTY;
            }
            else
            {
                // we found a common initial character sequence
                return strs[0].Substring(0, smallestIndexOfDiff);
            }
        }

        // Misc
        //-----------------------------------------------------------------------
        /// <summary>
        /// <p>Find the Levenshtein distance between two Strings.</p>
        /// 
        /// <p>This is the number of changes needed to change one String into
        /// another, where each change is a single character modification (deletion,
        /// insertion or substitution).</p>
        /// 
        /// <p>The previous implementation of the Levenshtein distance algorithm
        /// was from <a href="http://www.merriampark.com/ld.htm">http://www.merriampark.com/ld.htm</a></p>
        /// 
        /// <p>Chas Emerick has written an implementation in Java, which avoids an OutOfMemoryError
        /// which can occur when my Java implementation is used with very large strings.<br>
        /// This implementation of the Levenshtein distance algorithm
        /// is from <a href="http://www.merriampark.com/ldjava.htm">http://www.merriampark.com/ldjava.htm</a></p>
        /// 
        /// <pre>
        /// StringUtil.getLevenshteinDistance(null, *)             = IllegalArgumentException
        /// StringUtil.getLevenshteinDistance(*, null)             = IllegalArgumentException
        /// StringUtil.getLevenshteinDistance("","")               = 0
        /// StringUtil.getLevenshteinDistance("","a")              = 1
        /// StringUtil.getLevenshteinDistance("aaapppp", "")       = 7
        /// StringUtil.getLevenshteinDistance("frog", "fog")       = 1
        /// StringUtil.getLevenshteinDistance("fly", "ant")        = 3
        /// StringUtil.getLevenshteinDistance("elephant", "hippo") = 7
        /// StringUtil.getLevenshteinDistance("hippo", "elephant") = 7
        /// StringUtil.getLevenshteinDistance("hippo", "zzzzzzzz") = 8
        /// StringUtil.getLevenshteinDistance("hello", "hallo")    = 1
        /// </pre>
        /// </summary>
        /// <param name="s">  the first String, must not be null </param>
        /// <param name="t">  the second String, must not be null </param>
        /// <returns> result distance </returns>
        /// <exception cref="IllegalArgumentException"> if either String input <code>null</code> </exception>
        public static int GetLevenshteinDistance(string s, string t)
        {
            if (s == null || t == null)
            {
                throw new System.ArgumentException("Strings must not be null");
            }

            /*
               The difference between this impl. and the previous is that, rather 
               than creating and retaining a matrix of size s.Length()+1 by t.Length()+1, 
               we maintain two single-dimensional arrays of length s.Length()+1.  The first, d,
               is the 'current working' distance array that maintains the newest distance cost
               counts as we iterate through the characters of String s.  Each time we increment
               the index of String t we are comparing, d is copied to p, the second int[].  Doing so
               allows us to retain the previous cost counts as required by the algorithm (taking 
               the minimum of the cost count to the left, up one, and diagonally up and to the left
               of the current cost count being calculated).  (Note that the arrays aren't really 
               copied anymore, just switched...this is clearly much better than cloning an array 
               or doing a System.arrayCopy() each time  through the outer loop.)
	
               Effectively, the difference between the two implementations is this one does not 
               cause an out of memory condition when calculating the LD over two very large strings.
             */

            int n = s.Length; // length of s
            int m = t.Length; // length of t

            if (n == 0)
            {
                return m;
            }
            else if (m == 0)
            {
                return n;
            }

            if (n > m)
            {
                // swap the input strings to consume less memory
                string tmp = s;
                s = t;
                t = tmp;
                n = m;
                m = t.Length;
            }

            int[] p = new int[n + 1]; //'previous' cost array, horizontally
            int[] d = new int[n + 1]; // cost array, horizontally
            int[] _d; //placeholder to assist in swapping p and d

            // indexes into strings s and t
            int i; // iterates through s
            int j; // iterates through t

            char t_j; // jth character of t

            int cost; // cost

            for (i = 0; i <= n; i++)
            {
                p[i] = i;
            }

            for (j = 1; j <= m; j++)
            {
                t_j = t[j - 1];
                d[0] = j;

                for (i = 1; i <= n; i++)
                {
                    cost = s[i - 1] == t_j ? 0 : 1;
                    // minimum of cell to the left+1, to the top+1, diagonally left and up +cost
                    d[i] = Math.Min(Math.Min(d[i - 1] + 1, p[i] + 1), p[i - 1] + cost);
                }

                // copy current distance counts to 'previous row' distance counts
                _d = p;
                p = d;
                d = _d;
            }

            // our last action in the above loop was to switch d and p, so p now 
            // actually has the most recent cost counts
            return p[n];
        }

        /// <summary>
        /// <p>Gets the minimum of three <code>int</code> values.</p>
        /// </summary>
        /// <param name="a">  value 1 </param>
        /// <param name="b">  value 2 </param>
        /// <param name="c">  value 3 </param>
        /// <returns>  the smallest of the values </returns>
        /*
            private static int min(int a, int b, int c) {
                // Method copied from NumberUtils to avoid dependency on subpackage
                if (b < a) {
                    a = b;
                }
                if (c < a) {
                    a = c;
                }
                return a;
            }
        */

        // startsWith
        //-----------------------------------------------------------------------

        /// <summary>
        /// <p>Check if a String starts with a specified prefix.</p>
        /// 
        /// <p><code>null</code>s are handled without exceptions. Two <code>null</code>
        /// references are considered to be equal. The comparison is case sensitive.</p>
        /// 
        /// <pre>
        /// StringUtil.StartsWith(null, null)      = true
        /// StringUtil.StartsWith(null, "abcdef")  = false
        /// StringUtil.StartsWith("abc", null)     = false
        /// StringUtil.StartsWith("abc", "abcdef") = true
        /// StringUtil.StartsWith("abc", "ABCDEF") = false
        /// </pre>
        /// </summary>
        /// <seealso cref= java.lang.String#StartsWith(String) </seealso>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="prefix"> the prefix to find, may be null </param>
        /// <returns> <code>true</code> if the String starts with the prefix, case sensitive, or
        ///  both <code>null</code>
        /// @since 2.4 </returns>
        public static bool StartsWith(string str, string prefix)
        {
            return StartsWith(str, prefix, false);
        }

        /// <summary>
        /// <p>Case insensitive check if a String starts with a specified prefix.</p>
        /// 
        /// <p><code>null</code>s are handled without exceptions. Two <code>null</code>
        /// references are considered to be equal. The comparison is case insensitive.</p>
        /// 
        /// <pre>
        /// StringUtil.StartsWithIgnoreCase(null, null)      = true
        /// StringUtil.StartsWithIgnoreCase(null, "abcdef")  = false
        /// StringUtil.StartsWithIgnoreCase("abc", null)     = false
        /// StringUtil.StartsWithIgnoreCase("abc", "abcdef") = true
        /// StringUtil.StartsWithIgnoreCase("abc", "ABCDEF") = true
        /// </pre>
        /// </summary>
        /// <seealso cref= java.lang.String#StartsWith(String) </seealso>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="prefix"> the prefix to find, may be null </param>
        /// <returns> <code>true</code> if the String starts with the prefix, case insensitive, or
        ///  both <code>null</code>
        /// @since 2.4 </returns>
        public static bool StartsWithIgnoreCase(string str, string prefix)
        {
            return StartsWith(str, prefix, true);
        }

        /// <summary>
        /// <p>Check if a String starts with a specified prefix (optionally case insensitive).</p>
        /// </summary>
        /// <seealso cref= java.lang.String#StartsWith(String) </seealso>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="prefix"> the prefix to find, may be null </param>
        /// <param name="ignoreCase"> inidicates whether the compare should ignore case
        ///  (case insensitive) or not. </param>
        /// <returns> <code>true</code> if the String starts with the prefix or
        ///  both <code>null</code> </returns>
        private static bool StartsWith(string str, string prefix, bool ignoreCase)
        {
            //if (str == null || prefix == null)
            //{
            //    return (str == null && prefix == null);
            //}
            //if (prefix.Length > str.Length)
            //{
            //    return false;
            //}
            //return str.RegionMatches(ignoreCase, 0, prefix, 0, prefix.Length);
            return true;
        }

        // endsWith
        //-----------------------------------------------------------------------

        /// <summary>
        /// <p>Check if a String ends with a specified suffix.</p>
        /// 
        /// <p><code>null</code>s are handled without exceptions. Two <code>null</code>
        /// references are considered to be equal. The comparison is case sensitive.</p>
        /// 
        /// <pre>
        /// StringUtil.EndsWith(null, null)      = true
        /// StringUtil.EndsWith(null, "abcdef")  = false
        /// StringUtil.EndsWith("def", null)     = false
        /// StringUtil.EndsWith("def", "abcdef") = true
        /// StringUtil.EndsWith("def", "ABCDEF") = false
        /// </pre>
        /// </summary>
        /// <seealso cref= java.lang.String#EndsWith(String) </seealso>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="suffix"> the suffix to find, may be null </param>
        /// <returns> <code>true</code> if the String ends with the suffix, case sensitive, or
        ///  both <code>null</code>
        /// @since 2.4 </returns>
        public static bool EndsWith(string str, string suffix)
        {
            return EndsWith(str, suffix, false);
        }

        /// <summary>
        /// <p>Case insensitive check if a String ends with a specified suffix.</p>
        /// 
        /// <p><code>null</code>s are handled without exceptions. Two <code>null</code>
        /// references are considered to be equal. The comparison is case insensitive.</p>
        /// 
        /// <pre>
        /// StringUtil.EndsWithIgnoreCase(null, null)      = true
        /// StringUtil.EndsWithIgnoreCase(null, "abcdef")  = false
        /// StringUtil.EndsWithIgnoreCase("def", null)     = false
        /// StringUtil.EndsWithIgnoreCase("def", "abcdef") = true
        /// StringUtil.EndsWithIgnoreCase("def", "ABCDEF") = false
        /// </pre>
        /// </summary>
        /// <seealso cref= java.lang.String#EndsWith(String) </seealso>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="suffix"> the suffix to find, may be null </param>
        /// <returns> <code>true</code> if the String ends with the suffix, case insensitive, or
        ///  both <code>null</code>
        /// @since 2.4 </returns>
        public static bool EndsWithIgnoreCase(string str, string suffix)
        {
            return EndsWith(str, suffix, true);
        }

        /// <summary>
        /// <p>Check if a String ends with a specified suffix (optionally case insensitive).</p>
        /// </summary>
        /// <seealso cref= java.lang.String#EndsWith(String) </seealso>
        /// <param name="str">  the String to check, may be null </param>
        /// <param name="suffix"> the suffix to find, may be null </param>
        /// <param name="ignoreCase"> inidicates whether the compare should ignore case
        ///  (case insensitive) or not. </param>
        /// <returns> <code>true</code> if the String starts with the prefix or
        ///  both <code>null</code> </returns>
        private static bool EndsWith(string str, string suffix, bool ignoreCase)
        {
            //if (str == null || suffix == null)
            //{
            //    return (str == null && suffix == null);
            //}
            //if (suffix.Length > str.Length)
            //{
            //    return false;
            //}
            //int strOffset = str.Length - suffix.Length;
            //return str.RegionMatches(ignoreCase, strOffset, suffix, 0, suffix.Length);

            return false;
        }

        /// <summary>
        /// <p>Checks if an array of primitive chars is empty or <code>null</code>.</p>
        /// </summary>
        /// <param name="array">  the array to test </param>
        /// <returns> <code>true</code> if the array is empty or <code>null</code>
        /// @since 2.1 </returns>
        public static bool IsArrayEmpty(char[] array)
        {
            if (array == null || array.Length == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// <p>Gets the <code>toString</code> of an <code>Object</code> returning
        /// an empty string ("") if <code>null</code> input.</p>
        /// 
        /// <pre>
        /// ObjectUtils.toString(null)         = ""
        /// ObjectUtils.toString("")           = ""
        /// ObjectUtils.toString("bat")        = "bat"
        /// ObjectUtils.toString(Boolean.TRUE) = "true"
        /// </pre>
        /// </summary>
        /// <seealso cref= StringUtils#defaultString(String) </seealso>
        /// <seealso cref= String#valueOf(Object) </seealso>
        /// <param name="obj">  the Object to <code>toString</code>, may be null </param>
        /// <returns> the passed in Object's toString, or nullStr if <code>null</code> input
        /// @since 2.0 </returns>
        public static string ToString(object obj)
        {
            return obj == null ? "" : obj.ToString();
        }

        /// <summary>
        /// Get the contents of an <code>Stream</code> as a String
        /// using the specified character encoding.
        /// <p>
        /// Character encoding names can be found at
        /// <a href="http://www.iana.org/assignments/character-sets">IANA</a>.
        /// <p>
        /// This method buffers the input internally, so there is no need to use a
        /// <code>BufferedStream</code>.
        /// </summary>
        /// <param name="input">  the <code>Stream</code> to read from </param>
        /// <param name="encoding">  the encoding to use, null means platform default </param>
        /// <returns> the requested String </returns>
        /// <exception cref="NullPointerException"> if the input is null </exception>
        /// <exception cref="IOException"> if an I/O error occurs </exception>
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: public static String toString(java.io.Stream input, String encoding) throws java.io.IOException
        public static string ToString(Stream input, string encoding)
        {
            StringWriter sw = new StringWriter();
            Copy(input, sw, encoding);
            return sw.ToString();
        }


        // copy from Stream
        //-----------------------------------------------------------------------
        /// <summary>
        /// Copy bytes from an <code>Stream</code> to an
        /// <code>Stream</code>.
        /// <p>
        /// This method buffers the input internally, so there is no need to use a
        /// <code>BufferedStream</code>.
        /// <p>
        /// Large streams (over 2GB) will return a bytes copied value of
        /// <code>-1</code> after the copy has completed since the correct
        /// number of bytes cannot be returned as an int. For large streams
        /// use the <code>CopyLarge(Stream, Stream)</code> method.
        /// </summary>
        /// <param name="input">  the <code>Stream</code> to read from </param>
        /// <param name="output">  the <code>Stream</code> to write to </param>
        /// <returns> the number of bytes copied </returns>
        /// <exception cref="NullPointerException"> if the input or output is null </exception>
        /// <exception cref="IOException"> if an I/O error occurs </exception>
        /// <exception cref="ArithmeticException"> if the byte count is too large
        /// @since Commons IO 1.1 </exception>
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: public static int Copy(java.io.Stream input, java.io.Stream output) throws java.io.IOException
        public static int Copy(Stream input, Stream output)
        {
            long count = CopyLarge(input, output);
            if (count > int.MaxValue)
            {
                return -1;
            }
            return (int)count;
        }

        /// <summary>
        /// Copy bytes from a large (over 2GB) <code>Stream</code> to an
        /// <code>Stream</code>.
        /// <p>
        /// This method buffers the input internally, so there is no need to use a
        /// <code>BufferedStream</code>.
        /// </summary>
        /// <param name="input">  the <code>Stream</code> to read from </param>
        /// <param name="output">  the <code>Stream</code> to write to </param>
        /// <returns> the number of bytes copied </returns>
        /// <exception cref="NullPointerException"> if the input or output is null </exception>
        /// <exception cref="IOException"> if an I/O error occurs
        /// @since Commons IO 1.3 </exception>
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: public static long CopyLarge(java.io.Stream input, java.io.Stream output) throws java.io.IOException
        public static long CopyLarge(Stream input, Stream output)
        {
            //sbyte[] buffer = new sbyte[DEFAULT_BUFFER_SIZE];
            //long count = 0;
            //int n = 0;
            //while (-1 != (n = input.Read(buffer)))
            //{
            //    output.Write(buffer, 0, n);
            //    count += n;
            //}
            return 0;
        }

        /// <summary>
        /// Copy bytes from an <code>Stream</code> to chars on a
        /// <code>Writer</code> using the default character encoding of the platform.
        /// <p>
        /// This method buffers the input internally, so there is no need to use a
        /// <code>BufferedStream</code>.
        /// <p>
        /// This method uses <seealso cref="StreamReader"/>.
        /// </summary>
        /// <param name="input">  the <code>Stream</code> to read from </param>
        /// <param name="output">  the <code>Writer</code> to write to </param>
        /// <exception cref="NullPointerException"> if the input or output is null </exception>
        /// <exception cref="IOException"> if an I/O error occurs
        /// @since Commons IO 1.1 </exception>
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: public static void Copy(java.io.Stream input, java.io.Writer output) throws java.io.IOException
        public static void Copy(Stream input, TextWriter output)
        {
            StreamReader @in = new StreamReader(input);
            Copy(@in, output);
        }

        /// <summary>
        /// Copy bytes from an <code>Stream</code> to chars on a
        /// <code>Writer</code> using the specified character encoding.
        /// <p>
        /// This method buffers the input internally, so there is no need to use a
        /// <code>BufferedStream</code>.
        /// <p>
        /// Character encoding names can be found at
        /// <a href="http://www.iana.org/assignments/character-sets">IANA</a>.
        /// <p>
        /// This method uses <seealso cref="StreamReader"/>.
        /// </summary>
        /// <param name="input">  the <code>Stream</code> to read from </param>
        /// <param name="output">  the <code>Writer</code> to write to </param>
        /// <param name="encoding">  the encoding to use, null means platform default </param>
        /// <exception cref="NullPointerException"> if the input or output is null </exception>
        /// <exception cref="IOException"> if an I/O error occurs
        /// @since Commons IO 1.1 </exception>
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: public static void Copy(java.io.Stream input, java.io.Writer output, String encoding) throws java.io.IOException
        public static void Copy(Stream input, TextWriter output, string encoding)
        {
            if (encoding == null)
            {
                Copy(input, output);
            }
            else
            {
                StreamReader @in = new StreamReader(input, System.Text.Encoding.GetEncoding(encoding));
                Copy(@in, output);
            }
        }

        // copy from Reader
        //-----------------------------------------------------------------------
        /// <summary>
        /// Copy chars from a <code>Reader</code> to a <code>Writer</code>.
        /// <p>
        /// This method buffers the input internally, so there is no need to use a
        /// <code>BufferedReader</code>.
        /// <p>
        /// Large streams (over 2GB) will return a chars copied value of
        /// <code>-1</code> after the copy has completed since the correct
        /// number of chars cannot be returned as an int. For large streams
        /// use the <code>CopyLarge(Reader, Writer)</code> method.
        /// </summary>
        /// <param name="input">  the <code>Reader</code> to read from </param>
        /// <param name="output">  the <code>Writer</code> to write to </param>
        /// <returns> the number of characters copied </returns>
        /// <exception cref="NullPointerException"> if the input or output is null </exception>
        /// <exception cref="IOException"> if an I/O error occurs </exception>
        /// <exception cref="ArithmeticException"> if the character count is too large
        /// @since Commons IO 1.1 </exception>
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: public static int Copy(java.io.Reader input, java.io.Writer output) throws java.io.IOException
        public static int Copy(TextReader input, TextWriter output)
        {
            long count = CopyLarge(input, output);
            if (count > int.MaxValue)
            {
                return -1;
            }
            return (int)count;
        }

        /// <summary>
        /// Copy chars from a large (over 2GB) <code>Reader</code> to a <code>Writer</code>.
        /// <p>
        /// This method buffers the input internally, so there is no need to use a
        /// <code>BufferedReader</code>.
        /// </summary>
        /// <param name="input">  the <code>Reader</code> to read from </param>
        /// <param name="output">  the <code>Writer</code> to write to </param>
        /// <returns> the number of characters copied </returns>
        /// <exception cref="NullPointerException"> if the input or output is null </exception>
        /// <exception cref="IOException"> if an I/O error occurs
        /// @since Commons IO 1.3 </exception>
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: public static long CopyLarge(java.io.Reader input, java.io.Writer output) throws java.io.IOException
        public static long CopyLarge(TextReader input, TextWriter output)
        {
            char[] buffer = new char[DEFAULT_BUFFER_SIZE];
            long count = 0;
            int n = 0;
            while (-1 != (n = input.Read(buffer, 0, buffer.Length)))
            {
                output.Write(buffer, 0, n);
                count += n;
            }
            return count;
        }

        /// <summary>
        /// Copy chars from a <code>Reader</code> to bytes on an
        /// <code>Stream</code> using the default character encoding of the
        /// platform, and calling flush.
        /// <p>
        /// This method buffers the input internally, so there is no need to use a
        /// <code>BufferedReader</code>.
        /// <p>
        /// Due to the implementation of StreamWriter, this method performs a
        /// flush.
        /// <p>
        /// This method uses <seealso cref="StreamWriter"/>.
        /// </summary>
        /// <param name="input">  the <code>Reader</code> to read from </param>
        /// <param name="output">  the <code>Stream</code> to write to </param>
        /// <exception cref="NullPointerException"> if the input or output is null </exception>
        /// <exception cref="IOException"> if an I/O error occurs
        /// @since Commons IO 1.1 </exception>
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: public static void Copy(java.io.Reader input, java.io.Stream output) throws java.io.IOException
        public static void Copy(TextReader input, Stream output)
        {
            StreamWriter @out = new StreamWriter(output);
            Copy(input, @out);
            // XXX Unless anyone is planning on rewriting StreamWriter, we
            // have to flush here.
            @out.Flush();
        }

        /// <summary>
        /// Copy chars from a <code>Reader</code> to bytes on an
        /// <code>Stream</code> using the specified character encoding, and
        /// calling flush.
        /// <p>
        /// This method buffers the input internally, so there is no need to use a
        /// <code>BufferedReader</code>.
        /// <p>
        /// Character encoding names can be found at
        /// <a href="http://www.iana.org/assignments/character-sets">IANA</a>.
        /// <p>
        /// Due to the implementation of StreamWriter, this method performs a
        /// flush.
        /// <p>
        /// This method uses <seealso cref="StreamWriter"/>.
        /// </summary>
        /// <param name="input">  the <code>Reader</code> to read from </param>
        /// <param name="output">  the <code>Stream</code> to write to </param>
        /// <param name="encoding">  the encoding to use, null means platform default </param>
        /// <exception cref="NullPointerException"> if the input or output is null </exception>
        /// <exception cref="IOException"> if an I/O error occurs
        /// @since Commons IO 1.1 </exception>
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: public static void Copy(java.io.Reader input, java.io.Stream output, String encoding) throws java.io.IOException
        public static void Copy(TextReader input, Stream output, string encoding)
        {
            if (encoding == null)
            {
                Copy(input, output);
            }
            else
            {
                StreamWriter @out = new StreamWriter(output, System.Text.Encoding.GetEncoding(encoding));
                Copy(input, @out);
                // XXX Unless anyone is planning on rewriting StreamWriter,
                // we have to flush here.
                @out.Flush();
            }
        }





    }
}
