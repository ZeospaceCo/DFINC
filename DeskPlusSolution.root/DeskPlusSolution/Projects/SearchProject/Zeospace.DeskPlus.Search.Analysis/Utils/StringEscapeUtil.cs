using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Zeospace.DeskPlus.Search.Analysis.Utils
{
    class StringEscapeUtil
    {
        public static string UnescapeJava(string str)
        {
            if (str == null)
            {
                return null;
            }
            try
            {
                //StringWriter writer = new StringWriter(str.Length);
                //UnescapeJava(writer, str);
                //return writer.ToString();
                return str;
            }
            catch (IOException ioe)
            {
                // this should never ever happen while writing to a StringWriter
                throw new UnhandledException(ioe);
            }
        }

        public static void UnescapeJava(TextWriter @out, string str)
        {
            /*
            if (@out == null)
            {
                throw new System.ArgumentException("The Writer must not be null");
            }
            if (str == null)
            {
                return;
            }
            int sz = str.Length;
            StrBuilder unicode = new StrBuilder(4);
            bool hadSlash = false;
            bool inUnicode = false;
            for (int i = 0; i < sz; i++)
            {
                char ch = str[i];
                if (inUnicode)
                {
                    // if in unicode, then we're reading unicode
                    // values in somehow
                    unicode.Append(ch);
                    if (unicode.Length() == 4)
                    {
                        // unicode now contains the four hex digits
                        // which represents our unicode character
                        try
                        {
                            int value = Convert.ToInt32(unicode.ToString(), 16);
                            @out.Write((char)value);
                            unicode.SetLength(0);
                            inUnicode = false;
                            hadSlash = false;
                        }
                        catch (Exception nfe) //NumberFormatException nfe)
                        {
                            throw new UnhandledException("Unable to parse unicode value: " + unicode, nfe);
                        }
                    }
                    continue;
                }
                if (hadSlash)
                {
                    // handle an escaped value
                    hadSlash = false;
                    switch (ch)
                    {
                        case '\\':
                            @out.Write('\\');
                            break;
                        case '\'':
                            @out.Write('\'');
                            break;
                        case '\"':
                            @out.Write('"');
                            break;
                        case 'r':
                            @out.Write('\r');
                            break;
                        case 'f':
                            @out.Write('\f');
                            break;
                        case 't':
                            @out.Write('\t');
                            break;
                        case 'n':
                            @out.Write('\n');
                            break;
                        case 'b':
                            @out.Write('\b');
                            break;
                        case 'u':
                            {
                                // uh-oh, we're in unicode country....
                                inUnicode = true;
                                break;
                            }
                        default:
                            @out.Write(ch);
                            break;
                    }
                    continue;
                }
                else if (ch == '\\')
                {
                    hadSlash = true;
                    continue;
                }
                @out.Write(ch);
            }
            if (hadSlash)
            {
                // then we're in the weird case of a \ at the end of the
                // string, let's output it anyway.
                @out.Write('\\');
            }
             * */
        }
    }
}
