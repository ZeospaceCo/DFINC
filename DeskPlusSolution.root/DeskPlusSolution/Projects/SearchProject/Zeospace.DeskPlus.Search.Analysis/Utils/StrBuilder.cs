using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Zeospace.DeskPlus.Search.Analysis.Utils
{
    //public class StrBuilder : ICloneable
    //{
    //    /// <summary>
    //    /// The extra Capacity for new builders.
    //    /// </summary>
    //    internal const int CAPACITY = 32;

    //    /// <summary>
    //    /// Required for serialization support.
    //    /// </summary>
    //    /// <seealso cref= java.io.Serializable </seealso>
    //    private const long serialVersionUID = 7628716375283629643L;

    //    /// <summary>
    //    /// An empty immutable <code>char</code> array.
    //    /// </summary>
    //    public static readonly char[] EMPTY_CHAR_ARRAY = new char[0];

    //    /// <summary>
    //    /// <p>
    //    /// The <code>line.separator</code> System Property. Line separator (<code>&quot;\n&quot;</code> on UNIX).
    //    /// </p>
    //    /// 
    //    /// <p>
    //    /// Defaults to <code>null</code> if the runtime does not have
    //    /// security access to read this property or the property does not exist.
    //    /// </p>
    //    /// 
    //    /// <p>
    //    /// This value is initialized when the class is loaded. If <seealso cref="System#setProperty(String,String)"/> or
    //    /// <seealso cref="System#setProperties(java.util.Properties)"/> is called after this class is loaded, the value
    //    /// will be out of sync with that System property.
    //    /// </p>
    //    /// 
    //    /// @since Java 1.1
    //    /// </summary>
    //    public static readonly string LINE_SEPARATOR = Utilities.GetSystemProperty("line.separator");

    //    /// <summary>
    //    /// Internal data storage. </summary>
    //    protected internal char[] buffer; // TODO make private?
    //    /// <summary>
    //    /// Current size of the buffer. </summary>
    //    protected internal int size_Renamed; // TODO make private?
    //    /// <summary>
    //    /// The new line. </summary>
    //    private string newLine;
    //    /// <summary>
    //    /// The null text. </summary>
    //    private string nullText;

    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Constructor that creates an empty builder initial Capacity 32 characters.
    //    /// </summary>
    //    public StrBuilder() : this(CAPACITY)
    //    {
    //    }

    //    /// <summary>
    //    /// Constructor that creates an empty builder the specified initial Capacity.
    //    /// </summary>
    //    /// <param name="initialCapacity">  the initial Capacity, zero or less will be converted to 32 </param>
    //    public StrBuilder(int initialCapacity) : base()
    //    {
    //        if (initialCapacity <= 0)
    //        {
    //            initialCapacity = CAPACITY;
    //        }
    //        buffer = new char[initialCapacity];
    //    }

    //    /// <summary>
    //    /// Constructor that creates a builder from the string, allocating
    //    /// 32 extra characters for growth.
    //    /// </summary>
    //    /// <param name="str">  the string to copy, null treated as blank string </param>
    //    public StrBuilder(string str) : base()
    //    {
    //        if (str == null)
    //        {
    //            buffer = new char[CAPACITY];
    //        }
    //        else
    //        {
    //            buffer = new char[str.Length + CAPACITY];
    //            Append(str);
    //        }
    //    }

    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Gets the text to be Appended when a new line is added.
    //    /// </summary>
    //    /// <returns> the new line text, null means use system default </returns>
    //    public string NewLineText
    //    {
    //        get
    //        {
    //            return newLine;
    //        }
    //    }

    //    /// <summary>
    //    /// Sets the text to be Appended when a new line is added.
    //    /// </summary>
    //    /// <param name="newLine">  the new line text, null means use system default </param>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder SetNewLineText(string newLine)
    //    {
    //        this.newLine = newLine;
    //        return this;
    //    }

    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Gets the text to be Appended when null is added.
    //    /// </summary>
    //    /// <returns> the null text, null means no Append </returns>
    //    public string NullText
    //    {
    //        get
    //        {
    //            return nullText;
    //        }
    //    }

    //    /// <summary>
    //    /// Sets the text to be Appended when null is added.
    //    /// </summary>
    //    /// <param name="nullText">  the null text, null means no Append </param>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder SetNullText(string nullText)
    //    {
    //        if (nullText != null && nullText.Length == 0)
    //        {
    //            nullText = null;
    //        }
    //        this.nullText = nullText;
    //        return this;
    //    }

    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Gets the length of the string builder.
    //    /// </summary>
    //    /// <returns> the length </returns>
    //    public int Length()
    //    {
    //        return size_Renamed;
    //    }

    //    /// <summary>
    //    /// Updates the length of the builder by either dropping the last characters
    //    /// or adding filler of unicode zero.
    //    /// </summary>
    //    /// <param name="length">  the length to set to, must be zero or positive </param>
    //    /// <returns> this, to enable chaining </returns>
    //    /// <exception cref="IndexOutOfBoundsException"> if the length is negative </exception>
    //    public StrBuilder SetLength(int length)
    //    {
    //        if (length < 0)
    //        {
    //            //throw new StringIndexOutOfBoundsException(length);
    //        }
    //        if (length < size_Renamed)
    //        {
    //            size_Renamed = length;
    //        }
    //        else if (length > size_Renamed)
    //        {
    //            EnsureCapacity(length);
    //            int oldEnd = size_Renamed;
    //            int newEnd = length;
    //            size_Renamed = length;
    //            for (int i = oldEnd; i < newEnd; i++)
    //            {
    //                buffer[i] = '\0';
    //            }
    //        }
    //        return this;
    //    }

    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Gets the current size of the internal character array buffer.
    //    /// </summary>
    //    /// <returns> the Capacity </returns>
    //    public int Capacity()
    //    {
    //        return buffer.Length;
    //    }

    //    /// <summary>
    //    /// Checks the Capacity and ensures that it is at least the size specified.
    //    /// </summary>
    //    /// <param name="Capacity">  the Capacity to ensure </param>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder EnsureCapacity(int Capacity)
    //    {
    //        if (Capacity > buffer.Length)
    //        {
    //            char[] old = buffer;
    //            buffer = new char[Capacity * 2];
    //            Array.Copy(old, 0, buffer, 0, size_Renamed);
    //        }
    //        return this;
    //    }

    //    /// <summary>
    //    /// Minimizes the Capacity to the actual length of the string.
    //    /// </summary>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder MinimizeCapacity()
    //    {
    //        if (buffer.Length > Length())
    //        {
    //            char[] old = buffer;
    //            buffer = new char[Length()];
    //            Array.Copy(old, 0, buffer, 0, size_Renamed);
    //        }
    //        return this;
    //    }

    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Gets the length of the string builder.
    //    /// <p>
    //    /// This method is the same as <seealso cref="#Length()"/> and is provided to match the
    //    /// API of Collections.
    //    /// </summary>
    //    /// <returns> the length </returns>
    //    public int Count
    //    {
    //        get { return size_Renamed; }
    //    }

    //    /// <summary>
    //    /// Checks is the string builder is empty (convenience Collections API style method).
    //    /// <p>
    //    /// This method is the same as checking <seealso cref="#Length()"/> and is provided to match the
    //    /// API of Collections.
    //    /// </summary>
    //    /// <returns> <code>true</code> if the size is <code>0</code>. </returns>
    //    public bool Empty
    //    {
    //        get
    //        {
    //            return size_Renamed == 0;
    //        }
    //    }

    //    /// <summary>
    //    /// Clears the string builder (convenience Collections API style method).
    //    /// <p>
    //    /// This method does not reduce the size of the internal character buffer.
    //    /// To do that, call <code>Clear()</code> followed by <seealso cref="#MinimizeCapacity()"/>.
    //    /// <p>
    //    /// This method is the same as <seealso cref="#SetLength(int)"/> called with zero
    //    /// and is provided to match the API of Collections.
    //    /// </summary>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder Clear()
    //    {
    //        size_Renamed = 0;
    //        return this;
    //    }

    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Gets the character at the specified index.
    //    /// </summary>
    //    /// <seealso cref= #SetCharAt(int, char) </seealso>
    //    /// <seealso cref= #DeleteCharAt(int) </seealso>
    //    /// <param name="index">  the index to retrieve, must be valid </param>
    //    /// <returns> the character at the index </returns>
    //    /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
    //    public char CharAt(int index)
    //    {
    //        if (index < 0 || index >= Length())
    //        {
    //            //throw new StringIndexOutOfBoundsException(index);
    //        }
    //        return buffer[index];
    //    }

    //    /// <summary>
    //    /// Sets the character at the specified index.
    //    /// </summary>
    //    /// <seealso cref= #CharAt(int) </seealso>
    //    /// <seealso cref= #DeleteCharAt(int) </seealso>
    //    /// <param name="index">  the index to set </param>
    //    /// <param name="ch">  the new character </param>
    //    /// <returns> this, to enable chaining </returns>
    //    /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
    //    public StrBuilder SetCharAt(int index, char ch)
    //    {
    //        if (index < 0 || index >= Length())
    //        {
    //            //throw new StringIndexOutOfBoundsException(index);
    //        }
    //        buffer[index] = ch;
    //        return this;
    //    }

    //    /// <summary>
    //    /// Deletes the character at the specified index.
    //    /// </summary>
    //    /// <seealso cref= #CharAt(int) </seealso>
    //    /// <seealso cref= #SetCharAt(int, char) </seealso>
    //    /// <param name="index">  the index to delete </param>
    //    /// <returns> this, to enable chaining </returns>
    //    /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
    //    public StrBuilder DeleteCharAt(int index)
    //    {
    //        if (index < 0 || index >= size_Renamed)
    //        {
    //            //throw new StringIndexOutOfBoundsException(index);
    //        }
    //        DeleteImpl(index, index + 1, 1);
    //        return this;
    //    }

    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Copies the builder's character array into a new character array.
    //    /// </summary>
    //    /// <returns> a new array that represents the contents of the builder </returns>
    //    public char[] ToCharArray()
    //    {
    //        if (size_Renamed == 0)
    //        {
    //            return EMPTY_CHAR_ARRAY;
    //        }
    //        char[] chars = new char[size_Renamed];
    //        Array.Copy(buffer, 0, chars, 0, size_Renamed);
    //        return chars;
    //    }

    //    /// <summary>
    //    /// Copies part of the builder's character array into a new character array.
    //    /// </summary>
    //    /// <param name="startIndex">  the start index, inclusive, must be valid </param>
    //    /// <param name="endIndex">  the end index, exclusive, must be valid except that
    //    ///  if too large it is treated as end of string </param>
    //    /// <returns> a new array that holds part of the contents of the builder </returns>
    //    /// <exception cref="IndexOutOfBoundsException"> if startIndex is invalid,
    //    ///  or if endIndex is invalid (but endIndex greater than size is valid) </exception>
    //    public char[] ToCharArray(int startIndex, int endIndex)
    //    {
    //        endIndex = ValidateRange(startIndex, endIndex);
    //        int len = endIndex - startIndex;
    //        if (len == 0)
    //        {
    //            return EMPTY_CHAR_ARRAY;
    //        }
    //        char[] chars = new char[len];
    //        Array.Copy(buffer, startIndex, chars, 0, len);
    //        return chars;
    //    }

    //    /// <summary>
    //    /// Copies the character array into the specified array.
    //    /// </summary>
    //    /// <param name="destination">  the destination array, null will cause an array to be created </param>
    //    /// <returns> the input array, unless that was null or too small </returns>
    //    public char[] GetChars(char[] destination)
    //    {
    //        int len = Length();
    //        if (destination == null || destination.Length < len)
    //        {
    //            destination = new char[len];
    //        }
    //        Array.Copy(buffer, 0, destination, 0, len);
    //        return destination;
    //    }

    //    /// <summary>
    //    /// Copies the character array into the specified array.
    //    /// </summary>
    //    /// <param name="startIndex">  first index to copy, inclusive, must be valid </param>
    //    /// <param name="endIndex">  last index, exclusive, must be valid </param>
    //    /// <param name="destination">  the destination array, must not be null or too small </param>
    //    /// <param name="destinationIndex">  the index to start copying in destination </param>
    //    /// <exception cref="NullPointerException"> if the array is null </exception>
    //    /// <exception cref="IndexOutOfBoundsException"> if any index is invalid </exception>
    //    public void GetChars(int startIndex, int endIndex, char[] destination, int destinationIndex)
    //    {
    //        if (startIndex < 0)
    //        {
    //            //throw new StringIndexOutOfBoundsException(startIndex);
    //        }
    //        if (endIndex < 0 || endIndex > Length())
    //        {
    //            //throw new StringIndexOutOfBoundsException(endIndex);
    //        }
    //        if (startIndex > endIndex)
    //        {
    //            //throw new StringIndexOutOfBoundsException("end < start");
    //        }
    //        Array.Copy(buffer, startIndex, destination, destinationIndex, endIndex - startIndex);
    //    }

    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Appends the new line string to this string builder.
    //    /// <p>
    //    /// The new line string can be altered using <seealso cref="#SetNewLineText(String)"/>.
    //    /// This might be used to force the output to always use Unix line endings
    //    /// even when on Windows.
    //    /// </summary>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder AppendNewLine()
    //    {
    //        if (newLine == null)
    //        {
    //            Append(LINE_SEPARATOR);
    //            return this;
    //        }
    //        return Append(newLine);
    //    }

    //    /// <summary>
    //    /// Appends the text representing <code>null</code> to this string builder.
    //    /// </summary>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder AppendNull()
    //    {
    //        if (nullText == null)
    //        {
    //            return this;
    //        }
    //        return Append(nullText);
    //    }

    //    /// <summary>
    //    /// Appends an object to this string builder.
    //    /// Appending null will call <seealso cref="#AppendNull()"/>.
    //    /// </summary>
    //    /// <param name="obj">  the object to Append </param>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder Append(object obj)
    //    {
    //        if (obj == null)
    //        {
    //            return AppendNull();
    //        }
    //        return Append(obj.ToString());
    //    }

    //    /// <summary>
    //    /// Appends a string to this string builder.
    //    /// Appending null will call <seealso cref="#AppendNull()"/>.
    //    /// </summary>
    //    /// <param name="str">  the string to Append </param>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder Append(string str)
    //    {
    //        if (str == null)
    //        {
    //            return AppendNull();
    //        }
    //        int strLen = str.Length;
    //        if (strLen > 0)
    //        {
    //            int len = Length();
    //            EnsureCapacity(len + strLen);
                
    //            str.GetChars(0, strLen, buffer, len);

    //            size_Renamed += strLen;
    //        }
    //        return this;
    //    }

    //    /// <summary>
    //    /// Appends part of a string to this string builder.
    //    /// Appending null will call <seealso cref="#AppendNull()"/>.
    //    /// </summary>
    //    /// <param name="str">  the string to Append </param>
    //    /// <param name="startIndex">  the start index, inclusive, must be valid </param>
    //    /// <param name="length">  the length to Append, must be valid </param>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder Append(string str, int startIndex, int length)
    //    {
    //        if (str == null)
    //        {
    //            return AppendNull();
    //        }
    //        if (startIndex < 0 || startIndex > str.Length)
    //        {
    //            //throw new StringIndexOutOfBoundsException("startIndex must be valid");
    //        }
    //        if (length < 0 || (startIndex + length) > str.Length)
    //        {
    //            //throw new StringIndexOutOfBoundsException("length must be valid");
    //        }
    //        if (length > 0)
    //        {
    //            int len = this.Length(); 
    //            EnsureCapacity(len + length);
    //            str.GetChars(startIndex, startIndex + length, buffer, len);
    //            size_Renamed += length;
    //        }
    //        return this;
    //    }

    //    /// <summary>
    //    /// Appends a string buffer to this string builder.
    //    /// Appending null will call <seealso cref="#AppendNull()"/>.
    //    /// </summary>
    //    /// <param name="str">  the string buffer to Append </param>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder Append(StringBuilder str)
    //    {
    //        if (str == null)
    //        {
    //            return AppendNull();
    //        }
    //        int strLen = str.Length;
    //        if (strLen > 0)
    //        {
    //            int len = Length();
    //            EnsureCapacity(len + strLen);
    //            str.GetChars(0, strLen, buffer, len);
    //            size_Renamed += strLen;
    //        }
    //        return this;
    //    }

    //    /// <summary>
    //    /// Appends part of a string buffer to this string builder.
    //    /// Appending null will call <seealso cref="#AppendNull()"/>.
    //    /// </summary>
    //    /// <param name="str">  the string to Append </param>
    //    /// <param name="startIndex">  the start index, inclusive, must be valid </param>
    //    /// <param name="length">  the length to Append, must be valid </param>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder Append(StringBuilder str, int startIndex, int length)
    //    {
    //        if (str == null)
    //        {
    //            return AppendNull();
    //        }
    //        if (startIndex < 0 || startIndex > str.Length)
    //        {
    //            //throw new StringIndexOutOfBoundsException("startIndex must be valid");
    //        }
    //        if (length < 0 || (startIndex + length) > str.Length)
    //        {
    //            //throw new StringIndexOutOfBoundsException("length must be valid");
    //        }
    //        if (length > 0)
    //        {
    //            int len = this.Length();
    //            EnsureCapacity(len + length);
    //            str.GetChars(startIndex, startIndex + length, buffer, len);
    //            size_Renamed += length;
    //        }
    //        return this;
    //    }

    //    /// <summary>
    //    /// Appends another string builder to this string builder.
    //    /// Appending null will call <seealso cref="#AppendNull()"/>.
    //    /// </summary>
    //    /// <param name="str">  the string builder to Append </param>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder Append(StrBuilder str)
    //    {
    //        if (str == null)
    //        {
    //            return AppendNull();
    //        }
    //        int strLen = str.Length();
    //        if (strLen > 0)
    //        {
    //            int len = Length();
    //            EnsureCapacity(len + strLen);
    //            Array.Copy(str.buffer, 0, buffer, len, strLen);
    //            size_Renamed += strLen;
    //        }
    //        return this;
    //    }

    //    /// <summary>
    //    /// Appends part of a string builder to this string builder.
    //    /// Appending null will call <seealso cref="#AppendNull()"/>.
    //    /// </summary>
    //    /// <param name="str">  the string to Append </param>
    //    /// <param name="startIndex">  the start index, inclusive, must be valid </param>
    //    /// <param name="length">  the length to Append, must be valid </param>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder Append(StrBuilder str, int startIndex, int length)
    //    {
    //        if (str == null)
    //        {
    //            return AppendNull();
    //        }
    //        if (startIndex < 0 || startIndex > str.Length())
    //        {
    //            //throw new StringIndexOutOfBoundsException("startIndex must be valid");
    //        }
    //        if (length < 0 || (startIndex + length) > str.Length())
    //        {
    //            //throw new StringIndexOutOfBoundsException("length must be valid");
    //        }
    //        if (length > 0)
    //        {
    //            int len = this.Length();
    //            EnsureCapacity(len + length);
    //            str.GetChars(startIndex, startIndex + length, buffer, len);
    //            size_Renamed += length;
    //        }
    //        return this;
    //    }

    //    /// <summary>
    //    /// Appends a char array to the string builder.
    //    /// Appending null will call <seealso cref="#AppendNull()"/>.
    //    /// </summary>
    //    /// <param name="chars">  the char array to Append </param>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder Append(char[] chars)
    //    {
    //        if (chars == null)
    //        {
    //            return AppendNull();
    //        }
    //        int strLen = chars.Length;
    //        if (strLen > 0)
    //        {
    //            int len = Length();
    //            EnsureCapacity(len + strLen);
    //            Array.Copy(chars, 0, buffer, len, strLen);
    //            size_Renamed += strLen;
    //        }
    //        return this;
    //    }

    //    /// <summary>
    //    /// Appends a char array to the string builder.
    //    /// Appending null will call <seealso cref="#AppendNull()"/>.
    //    /// </summary>
    //    /// <param name="chars">  the char array to Append </param>
    //    /// <param name="startIndex">  the start index, inclusive, must be valid </param>
    //    /// <param name="length">  the length to Append, must be valid </param>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder Append(char[] chars, int startIndex, int length)
    //    {
    //        if (chars == null)
    //        {
    //            return AppendNull();
    //        }
    //        if (startIndex < 0 || startIndex > chars.Length)
    //        {
    //            //throw new StringIndexOutOfBoundsException("Invalid startIndex: " + length);
    //        }
    //        if (length < 0 || (startIndex + length) > chars.Length)
    //        {
    //            //throw new StringIndexOutOfBoundsException("Invalid length: " + length);
    //        }
    //        if (length > 0)
    //        {
    //            int len = this.Length();
    //            EnsureCapacity(len + length);
    //            Array.Copy(chars, startIndex, buffer, len, length);
    //            size_Renamed += length;
    //        }
    //        return this;
    //    }

    //    /// <summary>
    //    /// Appends a Boolean value to the string builder.
    //    /// </summary>
    //    /// <param name="value">  the value to Append </param>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder Append(bool value)
    //    {
    //        if (value)
    //        {
    //            EnsureCapacity(size_Renamed + 4);
    //            buffer[size_Renamed++] = 't';
    //            buffer[size_Renamed++] = 'r';
    //            buffer[size_Renamed++] = 'u';
    //            buffer[size_Renamed++] = 'e';
    //        }
    //        else
    //        {
    //            EnsureCapacity(size_Renamed + 5);
    //            buffer[size_Renamed++] = 'f';
    //            buffer[size_Renamed++] = 'a';
    //            buffer[size_Renamed++] = 'l';
    //            buffer[size_Renamed++] = 's';
    //            buffer[size_Renamed++] = 'e';
    //        }
    //        return this;
    //    }

    //    /// <summary>
    //    /// Appends a char value to the string builder.
    //    /// </summary>
    //    /// <param name="ch">  the value to Append </param>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder Append(char ch)
    //    {
    //        int len = Length();
    //        EnsureCapacity(len + 1);
    //        buffer[size_Renamed++] = ch;
    //        return this;
    //    }

    //    /// <summary>
    //    /// Appends an int value to the string builder using <code>String.valueOf</code>.
    //    /// </summary>
    //    /// <param name="value">  the value to Append </param>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder Append(int value)
    //    {
    //        return Append(Convert.ToString(value));
    //    }

    //    /// <summary>
    //    /// Appends a long value to the string builder using <code>String.valueOf</code>.
    //    /// </summary>
    //    /// <param name="value">  the value to Append </param>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder Append(long value)
    //    {
    //        return Append(Convert.ToString(value));
    //    }

    //    /// <summary>
    //    /// Appends a float value to the string builder using <code>String.valueOf</code>.
    //    /// </summary>
    //    /// <param name="value">  the value to Append </param>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder Append(float value)
    //    {
    //        return Append(Convert.ToString(value));
    //    }

    //    /// <summary>
    //    /// Appends a double value to the string builder using <code>String.valueOf</code>.
    //    /// </summary>
    //    /// <param name="value">  the value to Append </param>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder Append(double value)
    //    {
    //        return Append(Convert.ToString(value));
    //    }

    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Appends an object followed by a new line to this string builder.
    //    /// Appending null will call <seealso cref="#AppendNull()"/>.
    //    /// </summary>
    //    /// <param name="obj">  the object to Append </param>
    //    /// <returns> this, to enable chaining
    //    /// @since 2.3 </returns>
    //    public StrBuilder Appendln(object obj)
    //    {
    //        return Append(obj).AppendNewLine();
    //    }

    //    /// <summary>
    //    /// Appends a string followed by a new line to this string builder.
    //    /// Appending null will call <seealso cref="#AppendNull()"/>.
    //    /// </summary>
    //    /// <param name="str">  the string to Append </param>
    //    /// <returns> this, to enable chaining
    //    /// @since 2.3 </returns>
    //    public StrBuilder Appendln(string str)
    //    {
    //        return Append(str).AppendNewLine();
    //    }

    //    /// <summary>
    //    /// Appends part of a string followed by a new line to this string builder.
    //    /// Appending null will call <seealso cref="#AppendNull()"/>.
    //    /// </summary>
    //    /// <param name="str">  the string to Append </param>
    //    /// <param name="startIndex">  the start index, inclusive, must be valid </param>
    //    /// <param name="length">  the length to Append, must be valid </param>
    //    /// <returns> this, to enable chaining
    //    /// @since 2.3 </returns>
    //    public StrBuilder Appendln(string str, int startIndex, int length)
    //    {
    //        return Append(str, startIndex, length).AppendNewLine();
    //    }

    //    /// <summary>
    //    /// Appends a string buffer followed by a new line to this string builder.
    //    /// Appending null will call <seealso cref="#AppendNull()"/>.
    //    /// </summary>
    //    /// <param name="str">  the string buffer to Append </param>
    //    /// <returns> this, to enable chaining
    //    /// @since 2.3 </returns>
    //    public StrBuilder Appendln(StringBuilder str)
    //    {
    //        return Append(str).AppendNewLine();
    //    }

    //    /// <summary>
    //    /// Appends part of a string buffer followed by a new line to this string builder.
    //    /// Appending null will call <seealso cref="#AppendNull()"/>.
    //    /// </summary>
    //    /// <param name="str">  the string to Append </param>
    //    /// <param name="startIndex">  the start index, inclusive, must be valid </param>
    //    /// <param name="length">  the length to Append, must be valid </param>
    //    /// <returns> this, to enable chaining
    //    /// @since 2.3 </returns>
    //    public StrBuilder Appendln(StringBuilder str, int startIndex, int length)
    //    {
    //        return Append(str, startIndex, length).AppendNewLine();
    //    }

    //    /// <summary>
    //    /// Appends another string builder followed by a new line to this string builder.
    //    /// Appending null will call <seealso cref="#AppendNull()"/>.
    //    /// </summary>
    //    /// <param name="str">  the string builder to Append </param>
    //    /// <returns> this, to enable chaining
    //    /// @since 2.3 </returns>
    //    public StrBuilder Appendln(StrBuilder str)
    //    {
    //        return Append(str).AppendNewLine();
    //    }

    //    /// <summary>
    //    /// Appends part of a string builder followed by a new line to this string builder.
    //    /// Appending null will call <seealso cref="#AppendNull()"/>.
    //    /// </summary>
    //    /// <param name="str">  the string to Append </param>
    //    /// <param name="startIndex">  the start index, inclusive, must be valid </param>
    //    /// <param name="length">  the length to Append, must be valid </param>
    //    /// <returns> this, to enable chaining
    //    /// @since 2.3 </returns>
    //    public StrBuilder Appendln(StrBuilder str, int startIndex, int length)
    //    {
    //        return Append(str, startIndex, length).AppendNewLine();
    //    }

    //    /// <summary>
    //    /// Appends a char array followed by a new line to the string builder.
    //    /// Appending null will call <seealso cref="#AppendNull()"/>.
    //    /// </summary>
    //    /// <param name="chars">  the char array to Append </param>
    //    /// <returns> this, to enable chaining
    //    /// @since 2.3 </returns>
    //    public StrBuilder Appendln(char[] chars)
    //    {
    //        return Append(chars).AppendNewLine();
    //    }

    //    /// <summary>
    //    /// Appends a char array followed by a new line to the string builder.
    //    /// Appending null will call <seealso cref="#AppendNull()"/>.
    //    /// </summary>
    //    /// <param name="chars">  the char array to Append </param>
    //    /// <param name="startIndex">  the start index, inclusive, must be valid </param>
    //    /// <param name="length">  the length to Append, must be valid </param>
    //    /// <returns> this, to enable chaining
    //    /// @since 2.3 </returns>
    //    public StrBuilder Appendln(char[] chars, int startIndex, int length)
    //    {
    //        return Append(chars, startIndex, length).AppendNewLine();
    //    }

    //    /// <summary>
    //    /// Appends a Boolean value followed by a new line to the string builder.
    //    /// </summary>
    //    /// <param name="value">  the value to Append </param>
    //    /// <returns> this, to enable chaining
    //    /// @since 2.3 </returns>
    //    public StrBuilder Appendln(bool value)
    //    {
    //        return Append(value).AppendNewLine();
    //    }

    //    /// <summary>
    //    /// Appends a char value followed by a new line to the string builder.
    //    /// </summary>
    //    /// <param name="ch">  the value to Append </param>
    //    /// <returns> this, to enable chaining
    //    /// @since 2.3 </returns>
    //    public StrBuilder Appendln(char ch)
    //    {
    //        return Append(ch).AppendNewLine();
    //    }

    //    /// <summary>
    //    /// Appends an int value followed by a new line to the string builder using <code>String.valueOf</code>.
    //    /// </summary>
    //    /// <param name="value">  the value to Append </param>
    //    /// <returns> this, to enable chaining
    //    /// @since 2.3 </returns>
    //    public StrBuilder Appendln(int value)
    //    {
    //        return Append(value).AppendNewLine();
    //    }

    //    /// <summary>
    //    /// Appends a long value followed by a new line to the string builder using <code>String.valueOf</code>.
    //    /// </summary>
    //    /// <param name="value">  the value to Append </param>
    //    /// <returns> this, to enable chaining
    //    /// @since 2.3 </returns>
    //    public StrBuilder Appendln(long value)
    //    {
    //        return Append(value).AppendNewLine();
    //    }

    //    /// <summary>
    //    /// Appends a float value followed by a new line to the string builder using <code>String.valueOf</code>.
    //    /// </summary>
    //    /// <param name="value">  the value to Append </param>
    //    /// <returns> this, to enable chaining
    //    /// @since 2.3 </returns>
    //    public StrBuilder Appendln(float value)
    //    {
    //        return Append(value).AppendNewLine();
    //    }

    //    /// <summary>
    //    /// Appends a double value followed by a new line to the string builder using <code>String.valueOf</code>.
    //    /// </summary>
    //    /// <param name="value">  the value to Append </param>
    //    /// <returns> this, to enable chaining
    //    /// @since 2.3 </returns>
    //    public StrBuilder Appendln(double value)
    //    {
    //        return Append(value).AppendNewLine();
    //    }

    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Appends each item in an array to the builder without any separators.
    //    /// Appending a null array will have no effect.
    //    /// Each object is Appended using <seealso cref="#Append(Object)"/>.
    //    /// </summary>
    //    /// <param name="array">  the array to Append </param>
    //    /// <returns> this, to enable chaining
    //    /// @since 2.3 </returns>
    //    public StrBuilder AppendAll(object[] array)
    //    {
    //        if (array != null && array.Length > 0)
    //        {
    //            for (int i = 0; i < array.Length; i++)
    //            {
    //                Append(array[i]);
    //            }
    //        }
    //        return this;
    //    }

    //    /// <summary>
    //    /// Appends each item in a collection to the builder without any separators.
    //    /// Appending a null collection will have no effect.
    //    /// Each object is Appended using <seealso cref="#Append(Object)"/>.
    //    /// </summary>
    //    /// <param name="coll">  the collection to Append </param>
    //    /// <returns> this, to enable chaining
    //    /// @since 2.3 </returns>
    //    public StrBuilder AppendAll(ICollection<string> coll)
    //    {
    //        if (coll != null && coll.Count > 0)
    //        {
    //            IEnumerator<string> it = coll.GetEnumerator();
    //            while (it.MoveNext())
    //            {
    //                Append(it.Current);
    //            }
    //        }
    //        return this;
    //    }

    //    /// <summary>
    //    /// Appends each item in an iterator to the builder without any separators.
    //    /// Appending a null iterator will have no effect.
    //    /// Each object is Appended using <seealso cref="#Append(Object)"/>.
    //    /// </summary>
    //    /// <param name="it">  the iterator to Append </param>
    //    /// <returns> this, to enable chaining
    //    /// @since 2.3 </returns>
    //    public StrBuilder AppendAll(IEnumerator<string> it)
    //    {
    //        if (it != null)
    //        {
    //            while (it.MoveNext())
    //            {
    //                Append(it.Current);
    //            }
    //        }
    //        return this;
    //    }

    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Appends an array placing separators between each value, but
    //    /// not before the first or after the last.
    //    /// Appending a null array will have no effect.
    //    /// Each object is Appended using <seealso cref="#Append(Object)"/>.
    //    /// </summary>
    //    /// <param name="array">  the array to Append </param>
    //    /// <param name="separator">  the separator to use, null means no separator </param>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder AppendWithSeparators(object[] array, string separator)
    //    {
    //        if (array != null && array.Length > 0)
    //        {
    //            separator = (separator == null ? "" : separator);
    //            Append(array[0]);
    //            for (int i = 1; i < array.Length; i++)
    //            {
    //                Append(separator);
    //                Append(array[i]);
    //            }
    //        }
    //        return this;
    //    }

    //    /// <summary>
    //    /// Appends a collection placing separators between each value, but
    //    /// not before the first or after the last.
    //    /// Appending a null collection will have no effect.
    //    /// Each object is Appended using <seealso cref="#Append(Object)"/>.
    //    /// </summary>
    //    /// <param name="coll">  the collection to Append </param>
    //    /// <param name="separator">  the separator to use, null means no separator </param>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder AppendWithSeparators(ICollection<string> coll, string separator)
    //    {
    //        if (coll != null && coll.Count > 0)
    //        {
    //            separator = (separator == null ? "" : separator);
    //            IEnumerator<string> it = coll.GetEnumerator();
    //            while (it.MoveNext())
    //            {
    //                Append(it.Current);
    //                if (it.MoveNext())
    //                {
    //                    Append(separator);
    //                }
    //            }
    //        }
    //        return this;
    //    }

    //    /// <summary>
    //    /// Appends an iterator placing separators between each value, but
    //    /// not before the first or after the last.
    //    /// Appending a null iterator will have no effect.
    //    /// Each object is Appended using <seealso cref="#Append(Object)"/>.
    //    /// </summary>
    //    /// <param name="it">  the iterator to Append </param>
    //    /// <param name="separator">  the separator to use, null means no separator </param>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder AppendWithSeparators(IEnumerator<string> it, string separator)
    //    {
    //        if (it != null)
    //        {
    //            separator = (separator == null ? "" : separator);
    //            while (it.MoveNext())
    //            {
    //                Append(it.Current);
    //                if (it.MoveNext())
    //                {
    //                    Append(separator);
    //                }
    //            }
    //        }
    //        return this;
    //    }

    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Appends a separator if the builder is currently non-empty.
    //    /// Appending a null separator will have no effect.
    //    /// The separator is Appended using <seealso cref="#Append(String)"/>.
    //    /// <p>
    //    /// This method is useful for adding a separator each time around the
    //    /// loop except the first.
    //    /// <pre>
    //    /// for (Iterator it = list.iterator(); it.MoveNext(); ) {
    //    ///   AppendSeparator(",");
    //    ///   Append(it.Current);
    //    /// }
    //    /// </pre>
    //    /// Note that for this simple example, you should use
    //    /// <seealso cref="#AppendWithSeparators(Collection, String)"/>.
    //    /// </summary>
    //    /// <param name="separator">  the separator to use, null means no separator </param>
    //    /// <returns> this, to enable chaining
    //    /// @since 2.3 </returns>
    //    public StrBuilder AppendSeparator(string separator)
    //    {
    //        return AppendSeparator(separator, null);
    //    }

    //    /// <summary>
    //    /// Appends one of both separators to the StrBuilder.
    //    /// If the builder is currently empty it will Append the defaultIfEmpty-separator
    //    /// Otherwise it will Append the standard-separator
    //    /// 
    //    /// Appending a null separator will have no effect.
    //    /// The separator is Appended using <seealso cref="#Append(String)"/>.
    //    /// <p>
    //    /// This method is for example useful for constructing queries
    //    /// <pre>
    //    /// StrBuilder whereClause = new StrBuilder();
    //    /// if(searchCommand.getPriority() != null) {
    //    ///  whereClause.AppendSeparator(" and", " where");
    //    ///  whereClause.Append(" priority = ?")
    //    /// }
    //    /// if(searchCommand.getComponent() != null) {
    //    ///  whereClause.AppendSeparator(" and", " where");
    //    ///  whereClause.Append(" component = ?")
    //    /// }
    //    /// selectClause.Append(whereClause)
    //    /// </pre>
    //    /// </summary>
    //    /// <param name="standard"> the separator if builder is not empty, null means no separator </param>
    //    /// <param name="defaultIfEmpty"> the separator if builder is empty, null means no separator </param>
    //    /// <returns> this, to enable chaining
    //    /// @since 2.5 </returns>
    //    public StrBuilder AppendSeparator(string standard, string defaultIfEmpty)
    //    {
    //        string str = Empty ? defaultIfEmpty : standard;
    //        if (str != null)
    //        {
    //            Append(str);
    //        }
    //        return this;
    //    }

    //    /// <summary>
    //    /// Appends a separator if the builder is currently non-empty.
    //    /// The separator is Appended using <seealso cref="#Append(char)"/>.
    //    /// <p>
    //    /// This method is useful for adding a separator each time around the
    //    /// loop except the first.
    //    /// <pre>
    //    /// for (Iterator it = list.iterator(); it.MoveNext(); ) {
    //    ///   AppendSeparator(',');
    //    ///   Append(it.Current);
    //    /// }
    //    /// </pre>
    //    /// Note that for this simple example, you should use
    //    /// <seealso cref="#AppendWithSeparators(Collection, String)"/>.
    //    /// </summary>
    //    /// <param name="separator">  the separator to use </param>
    //    /// <returns> this, to enable chaining
    //    /// @since 2.3 </returns>
    //    public StrBuilder AppendSeparator(char separator)
    //    {
    //        if (Count > 0)
    //        {
    //            Append(separator);
    //        }
    //        return this;
    //    }

    //    /// <summary>
    //    /// Append one of both separators to the builder
    //    /// If the builder is currently empty it will Append the defaultIfEmpty-separator
    //    /// Otherwise it will Append the standard-separator
    //    /// 
    //    /// The separator is Appended using <seealso cref="#Append(char)"/>. </summary>
    //    /// <param name="standard"> the separator if builder is not empty </param>
    //    /// <param name="defaultIfEmpty"> the separator if builder is empty </param>
    //    /// <returns> this, to enable chaining
    //    /// @since 2.5 </returns>
    //    public StrBuilder AppendSeparator(char standard, char defaultIfEmpty)
    //    {
    //        if (Count > 0)
    //        {
    //            Append(standard);
    //        }
    //        else
    //        {
    //            Append(defaultIfEmpty);
    //        }
    //        return this;
    //    }
    //    /// <summary>
    //    /// Appends a separator to the builder if the loop index is greater than zero.
    //    /// Appending a null separator will have no effect.
    //    /// The separator is Appended using <seealso cref="#Append(String)"/>.
    //    /// <p>
    //    /// This method is useful for adding a separator each time around the
    //    /// loop except the first.
    //    /// <pre>
    //    /// for (int i = 0; i < list.Count; i++) {
    //    ///   AppendSeparator(",", i);
    //    ///   Append(list.Get(i));
    //    /// }
    //    /// </pre>
    //    /// Note that for this simple example, you should use
    //    /// <seealso cref="#AppendWithSeparators(Collection, String)"/>.
    //    /// </summary>
    //    /// <param name="separator">  the separator to use, null means no separator </param>
    //    /// <param name="loopIndex">  the loop index </param>
    //    /// <returns> this, to enable chaining
    //    /// @since 2.3 </returns>
    //    public StrBuilder AppendSeparator(string separator, int loopIndex)
    //    {
    //        if (separator != null && loopIndex > 0)
    //        {
    //            Append(separator);
    //        }
    //        return this;
    //    }

    //    /// <summary>
    //    /// Appends a separator to the builder if the loop index is greater than zero.
    //    /// The separator is Appended using <seealso cref="#Append(char)"/>.
    //    /// <p>
    //    /// This method is useful for adding a separator each time around the
    //    /// loop except the first.
    //    /// <pre>
    //    /// for (int i = 0; i < list.Count; i++) {
    //    ///   AppendSeparator(",", i);
    //    ///   Append(list.Get(i));
    //    /// }
    //    /// </pre>
    //    /// Note that for this simple example, you should use
    //    /// <seealso cref="#AppendWithSeparators(Collection, String)"/>.
    //    /// </summary>
    //    /// <param name="separator">  the separator to use </param>
    //    /// <param name="loopIndex">  the loop index </param>
    //    /// <returns> this, to enable chaining
    //    /// @since 2.3 </returns>
    //    public StrBuilder AppendSeparator(char separator, int loopIndex)
    //    {
    //        if (loopIndex > 0)
    //        {
    //            Append(separator);
    //        }
    //        return this;
    //    }

    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Appends the pad character to the builder the specified number of times.
    //    /// </summary>
    //    /// <param name="length">  the length to Append, negative means no Append </param>
    //    /// <param name="padChar">  the character to Append </param>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder AppendPadding(int length, char padChar)
    //    {
    //        if (length >= 0)
    //        {
    //            EnsureCapacity(size_Renamed + length);
    //            for (int i = 0; i < length; i++)
    //            {
    //                buffer[size_Renamed++] = padChar;
    //            }
    //        }
    //        return this;
    //    }

    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Appends an object to the builder padding on the left to a fixed width.
    //    /// The <code>toString</code> of the object is used.
    //    /// If the object is larger than the length, the left hand side is lost.
    //    /// If the object is null, the null text value is used.
    //    /// </summary>
    //    /// <param name="obj">  the object to Append, null uses null text </param>
    //    /// <param name="width">  the fixed field width, zero or negative has no effect </param>
    //    /// <param name="padChar">  the pad character to use </param>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder AppendFixedWidthPadLeft(object obj, int width, char padChar)
    //    {
    //        if (width > 0)
    //        {
    //            EnsureCapacity(size_Renamed + width);
    //            string str = (obj == null ? NullText : obj.ToString());
    //            if (str == null)
    //            {
    //                str = "";
    //            }
    //            int strLen = str.Length;
    //            if (strLen >= width)
    //            {
    //                str.GetChars(strLen - width, strLen, buffer, size_Renamed);
    //            }
    //            else
    //            {
    //                int padLen = width - strLen;
    //                for (int i = 0; i < padLen; i++)
    //                {
    //                    buffer[size_Renamed + i] = padChar;
    //                }
    //                str.GetChars(0, strLen, buffer, size_Renamed + padLen);
    //            }
    //            size_Renamed += width;
    //        }
    //        return this;
    //    }

    //    /// <summary>
    //    /// Appends an object to the builder padding on the left to a fixed width.
    //    /// The <code>String.valueOf</code> of the <code>int</code> value is used.
    //    /// If the formatted value is larger than the length, the left hand side is lost.
    //    /// </summary>
    //    /// <param name="value">  the value to Append </param>
    //    /// <param name="width">  the fixed field width, zero or negative has no effect </param>
    //    /// <param name="padChar">  the pad character to use </param>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder AppendFixedWidthPadLeft(int value, int width, char padChar)
    //    {
    //        return AppendFixedWidthPadLeft(Convert.ToString(value), width, padChar);
    //    }

    //    /// <summary>
    //    /// Appends an object to the builder padding on the right to a fixed length.
    //    /// The <code>toString</code> of the object is used.
    //    /// If the object is larger than the length, the right hand side is lost.
    //    /// If the object is null, null text value is used.
    //    /// </summary>
    //    /// <param name="obj">  the object to Append, null uses null text </param>
    //    /// <param name="width">  the fixed field width, zero or negative has no effect </param>
    //    /// <param name="padChar">  the pad character to use </param>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder AppendFixedWidthPadRight(object obj, int width, char padChar)
    //    {
    //        if (width > 0)
    //        {
    //            EnsureCapacity(size_Renamed + width);
    //            string str = (obj == null ? NullText : obj.ToString());
    //            if (str == null)
    //            {
    //                str = "";
    //            }
    //            int strLen = str.Length;
    //            if (strLen >= width)
    //            {
    //                str.GetChars(0, width, buffer, size_Renamed);
    //            }
    //            else
    //            {
    //                int padLen = width - strLen;
    //                str.GetChars(0, strLen, buffer, size_Renamed);
    //                for (int i = 0; i < padLen; i++)
    //                {
    //                    buffer[size_Renamed + strLen + i] = padChar;
    //                }
    //            }
    //            size_Renamed += width;
    //        }
    //        return this;
    //    }

    //    /// <summary>
    //    /// Appends an object to the builder padding on the right to a fixed length.
    //    /// The <code>String.valueOf</code> of the <code>int</code> value is used.
    //    /// If the object is larger than the length, the right hand side is lost.
    //    /// </summary>
    //    /// <param name="value">  the value to Append </param>
    //    /// <param name="width">  the fixed field width, zero or negative has no effect </param>
    //    /// <param name="padChar">  the pad character to use </param>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder AppendFixedWidthPadRight(int value, int width, char padChar)
    //    {
    //        return AppendFixedWidthPadRight(Convert.ToString(value), width, padChar);
    //    }

    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Inserts the string representation of an object into this builder.
    //    /// Inserting null will use the stored null text value.
    //    /// </summary>
    //    /// <param name="index">  the index to add at, must be valid </param>
    //    /// <param name="obj">  the object to insert </param>
    //    /// <returns> this, to enable chaining </returns>
    //    /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
    //    public StrBuilder Insert(int index, object obj)
    //    {
    //        if (obj == null)
    //        {
    //            return Insert(index, nullText);
    //        }
    //        return Insert(index, obj.ToString());
    //    }

    //    /// <summary>
    //    /// Inserts the string into this builder.
    //    /// Inserting null will use the stored null text value.
    //    /// </summary>
    //    /// <param name="index">  the index to add at, must be valid </param>
    //    /// <param name="str">  the string to insert </param>
    //    /// <returns> this, to enable chaining </returns>
    //    /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
    //    public StrBuilder Insert(int index, string str)
    //    {
    //        ValidateIndex(index);
    //        if (str == null)
    //        {
    //            str = nullText;
    //        }
    //        int strLen = (str == null ? 0 : str.Length);
    //        if (strLen > 0)
    //        {
    //            int newSize = size_Renamed + strLen;
    //            EnsureCapacity(newSize);
    //            Array.Copy(buffer, index, buffer, index + strLen, size_Renamed - index);
    //            size_Renamed = newSize;
    //            str.GetChars(0, strLen, buffer, index); // str cannot be null here
    //        }
    //        return this;
    //    }

    //    /// <summary>
    //    /// Inserts the character array into this builder.
    //    /// Inserting null will use the stored null text value.
    //    /// </summary>
    //    /// <param name="index">  the index to add at, must be valid </param>
    //    /// <param name="chars">  the char array to insert </param>
    //    /// <returns> this, to enable chaining </returns>
    //    /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
    //    public StrBuilder Insert(int index, char[] chars)
    //    {
    //        ValidateIndex(index);
    //        if (chars == null)
    //        {
    //            return Insert(index, nullText);
    //        }
    //        int len = chars.Length;
    //        if (len > 0)
    //        {
    //            EnsureCapacity(size_Renamed + len);
    //            Array.Copy(buffer, index, buffer, index + len, size_Renamed - index);
    //            Array.Copy(chars, 0, buffer, index, len);
    //            size_Renamed += len;
    //        }
    //        return this;
    //    }

    //    /// <summary>
    //    /// Inserts part of the character array into this builder.
    //    /// Inserting null will use the stored null text value.
    //    /// </summary>
    //    /// <param name="index">  the index to add at, must be valid </param>
    //    /// <param name="chars">  the char array to insert </param>
    //    /// <param name="offset">  the offset into the character array to start at, must be valid </param>
    //    /// <param name="length">  the length of the character array part to copy, must be positive </param>
    //    /// <returns> this, to enable chaining </returns>
    //    /// <exception cref="IndexOutOfBoundsException"> if any index is invalid </exception>
    //    public StrBuilder Insert(int index, char[] chars, int offset, int length)
    //    {
    //        ValidateIndex(index);
    //        if (chars == null)
    //        {
    //            return Insert(index, nullText);
    //        }
    //        if (offset < 0 || offset > chars.Length)
    //        {
    //            //throw new StringIndexOutOfBoundsException("Invalid offset: " + offset);
    //        }
    //        if (length < 0 || offset + length > chars.Length)
    //        {
    //            //throw new StringIndexOutOfBoundsException("Invalid length: " + length);
    //        }
    //        if (length > 0)
    //        {
    //            EnsureCapacity(size_Renamed + length);
    //            Array.Copy(buffer, index, buffer, index + length, size_Renamed - index);
    //            Array.Copy(chars, offset, buffer, index, length);
    //            size_Renamed += length;
    //        }
    //        return this;
    //    }

    //    /// <summary>
    //    /// Inserts the value into this builder.
    //    /// </summary>
    //    /// <param name="index">  the index to add at, must be valid </param>
    //    /// <param name="value">  the value to insert </param>
    //    /// <returns> this, to enable chaining </returns>
    //    /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
    //    public StrBuilder Insert(int index, bool value)
    //    {
    //        ValidateIndex(index);
    //        if (value)
    //        {
    //            EnsureCapacity(size_Renamed + 4);
    //            Array.Copy(buffer, index, buffer, index + 4, size_Renamed - index);
    //            buffer[index++] = 't';
    //            buffer[index++] = 'r';
    //            buffer[index++] = 'u';
    //            buffer[index] = 'e';
    //            size_Renamed += 4;
    //        }
    //        else
    //        {
    //            EnsureCapacity(size_Renamed + 5);
    //            Array.Copy(buffer, index, buffer, index + 5, size_Renamed - index);
    //            buffer[index++] = 'f';
    //            buffer[index++] = 'a';
    //            buffer[index++] = 'l';
    //            buffer[index++] = 's';
    //            buffer[index] = 'e';
    //            size_Renamed += 5;
    //        }
    //        return this;
    //    }

    //    /// <summary>
    //    /// Inserts the value into this builder.
    //    /// </summary>
    //    /// <param name="index">  the index to add at, must be valid </param>
    //    /// <param name="value">  the value to insert </param>
    //    /// <returns> this, to enable chaining </returns>
    //    /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
    //    public StrBuilder Insert(int index, char value)
    //    {
    //        ValidateIndex(index);
    //        EnsureCapacity(size_Renamed + 1);
    //        Array.Copy(buffer, index, buffer, index + 1, size_Renamed - index);
    //        buffer[index] = value;
    //        size_Renamed++;
    //        return this;
    //    }

    //    /// <summary>
    //    /// Inserts the value into this builder.
    //    /// </summary>
    //    /// <param name="index">  the index to add at, must be valid </param>
    //    /// <param name="value">  the value to insert </param>
    //    /// <returns> this, to enable chaining </returns>
    //    /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
    //    public StrBuilder Insert(int index, int value)
    //    {
    //        return Insert(index, Convert.ToString(value));
    //    }

    //    /// <summary>
    //    /// Inserts the value into this builder.
    //    /// </summary>
    //    /// <param name="index">  the index to add at, must be valid </param>
    //    /// <param name="value">  the value to insert </param>
    //    /// <returns> this, to enable chaining </returns>
    //    /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
    //    public StrBuilder Insert(int index, long value)
    //    {
    //        return Insert(index, Convert.ToString(value));
    //    }

    //    /// <summary>
    //    /// Inserts the value into this builder.
    //    /// </summary>
    //    /// <param name="index">  the index to add at, must be valid </param>
    //    /// <param name="value">  the value to insert </param>
    //    /// <returns> this, to enable chaining </returns>
    //    /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
    //    public StrBuilder Insert(int index, float value)
    //    {
    //        return Insert(index, Convert.ToString(value));
    //    }

    //    /// <summary>
    //    /// Inserts the value into this builder.
    //    /// </summary>
    //    /// <param name="index">  the index to add at, must be valid </param>
    //    /// <param name="value">  the value to insert </param>
    //    /// <returns> this, to enable chaining </returns>
    //    /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
    //    public StrBuilder Insert(int index, double value)
    //    {
    //        return Insert(index, Convert.ToString(value));
    //    }

    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Internal method to delete a range without validation.
    //    /// </summary>
    //    /// <param name="startIndex">  the start index, must be valid </param>
    //    /// <param name="endIndex">  the end index (exclusive), must be valid </param>
    //    /// <param name="len">  the length, must be valid </param>
    //    /// <exception cref="IndexOutOfBoundsException"> if any index is invalid </exception>
    //    private void DeleteImpl(int startIndex, int endIndex, int len)
    //    {
    //        Array.Copy(buffer, endIndex, buffer, startIndex, size_Renamed - endIndex);
    //        size_Renamed -= len;
    //    }

    //    /// <summary>
    //    /// Deletes the characters between the two specified indices.
    //    /// </summary>
    //    /// <param name="startIndex">  the start index, inclusive, must be valid </param>
    //    /// <param name="endIndex">  the end index, exclusive, must be valid except
    //    ///  that if too large it is treated as end of string </param>
    //    /// <returns> this, to enable chaining </returns>
    //    /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
    //    public StrBuilder Delete(int startIndex, int endIndex)
    //    {
    //        endIndex = ValidateRange(startIndex, endIndex);
    //        int len = endIndex - startIndex;
    //        if (len > 0)
    //        {
    //            DeleteImpl(startIndex, endIndex, len);
    //        }
    //        return this;
    //    }

    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Deletes the character wherever it occurs in the builder.
    //    /// </summary>
    //    /// <param name="ch">  the character to delete </param>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder deleteAll(char ch)
    //    {
    //        for (int i = 0; i < size_Renamed; i++)
    //        {
    //            if (buffer[i] == ch)
    //            {
    //                int start = i;
    //                while (++i < size_Renamed)
    //                {
    //                    if (buffer[i] != ch)
    //                    {
    //                        break;
    //                    }
    //                }
    //                int len = i - start;
    //                DeleteImpl(start, i, len);
    //                i -= len;
    //            }
    //        }
    //        return this;
    //    }

    //    /// <summary>
    //    /// Deletes the character wherever it occurs in the builder.
    //    /// </summary>
    //    /// <param name="ch">  the character to delete </param>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder deleteFirst(char ch)
    //    {
    //        for (int i = 0; i < size_Renamed; i++)
    //        {
    //            if (buffer[i] == ch)
    //            {
    //                DeleteImpl(i, i + 1, 1);
    //                break;
    //            }
    //        }
    //        return this;
    //    }

    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Deletes the string wherever it occurs in the builder.
    //    /// </summary>
    //    /// <param name="str">  the string to delete, null causes no action </param>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder deleteAll(string str)
    //    {
    //        int len = (str == null ? 0 : str.Length);
    //        if (len > 0)
    //        {
    //            int index = IndexOf(str, 0);
    //            while (index >= 0)
    //            {
    //                DeleteImpl(index, index + len, len);
    //                index = IndexOf(str, index);
    //            }
    //        }
    //        return this;
    //    }

    //    /// <summary>
    //    /// Deletes the string wherever it occurs in the builder.
    //    /// </summary>
    //    /// <param name="str">  the string to delete, null causes no action </param>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder deleteFirst(string str)
    //    {
    //        int len = (str == null ? 0 : str.Length);
    //        if (len > 0)
    //        {
    //            int index = IndexOf(str, 0);
    //            if (index >= 0)
    //            {
    //                DeleteImpl(index, index + len, len);
    //            }
    //        }
    //        return this;
    //    }



    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Internal method to delete a range without validation.
    //    /// </summary>
    //    /// <param name="startIndex">  the start index, must be valid </param>
    //    /// <param name="endIndex">  the end index (exclusive), must be valid </param>
    //    /// <param name="removeLen">  the length to remove (endIndex - startIndex), must be valid </param>
    //    /// <param name="insertStr">  the string to replace with, null means delete range </param>
    //    /// <param name="insertLen">  the length of the insert string, must be valid </param>
    //    /// <exception cref="IndexOutOfBoundsException"> if any index is invalid </exception>
    //    private void replaceImpl(int startIndex, int endIndex, int removeLen, string insertStr, int insertLen)
    //    {
    //        int newSize = size_Renamed - removeLen + insertLen;
    //        if (insertLen != removeLen)
    //        {
    //            EnsureCapacity(newSize);
    //            Array.Copy(buffer, endIndex, buffer, startIndex + insertLen, size_Renamed - endIndex);
    //            size_Renamed = newSize;
    //        }
    //        if (insertLen > 0)
    //        {
    //            insertStr.GetChars(0, insertLen, buffer, startIndex);
    //        }
    //    }

    //    /// <summary>
    //    /// Replaces a portion of the string builder with another string.
    //    /// The length of the inserted string does not have to match the removed length.
    //    /// </summary>
    //    /// <param name="startIndex">  the start index, inclusive, must be valid </param>
    //    /// <param name="endIndex">  the end index, exclusive, must be valid except
    //    ///  that if too large it is treated as end of string </param>
    //    /// <param name="replaceStr">  the string to replace with, null means delete range </param>
    //    /// <returns> this, to enable chaining </returns>
    //    /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
    //    public StrBuilder Replace(int startIndex, int endIndex, string replaceStr)
    //    {
    //        endIndex = ValidateRange(startIndex, endIndex);
    //        int insertLen = (replaceStr == null ? 0 : replaceStr.Length);
    //        replaceImpl(startIndex, endIndex, endIndex - startIndex, replaceStr, insertLen);
    //        return this;
    //    }

    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Replaces the search character with the replace character
    //    /// throughout the builder.
    //    /// </summary>
    //    /// <param name="search">  the search character </param>
    //    /// <param name="replace">  the replace character </param>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder replaceAll(char search, char replace)
    //    {
    //        if (search != replace)
    //        {
    //            for (int i = 0; i < size_Renamed; i++)
    //            {
    //                if (buffer[i] == search)
    //                {
    //                    buffer[i] = replace;
    //                }
    //            }
    //        }
    //        return this;
    //    }

    //    /// <summary>
    //    /// Replaces the first instance of the search character with the
    //    /// replace character in the builder.
    //    /// </summary>
    //    /// <param name="search">  the search character </param>
    //    /// <param name="replace">  the replace character </param>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder replaceFirst(char search, char replace)
    //    {
    //        if (search != replace)
    //        {
    //            for (int i = 0; i < size_Renamed; i++)
    //            {
    //                if (buffer[i] == search)
    //                {
    //                    buffer[i] = replace;
    //                    break;
    //                }
    //            }
    //        }
    //        return this;
    //    }

    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Replaces the search string with the replace string throughout the builder.
    //    /// </summary>
    //    /// <param name="searchStr">  the search string, null causes no action to occur </param>
    //    /// <param name="replaceStr">  the replace string, null is equivalent to an empty string </param>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder replaceAll(string searchStr, string replaceStr)
    //    {
    //        int searchLen = (searchStr == null ? 0 : searchStr.Length);
    //        if (searchLen > 0)
    //        {
    //            int replaceLen = (replaceStr == null ? 0 : replaceStr.Length);
    //            int index = IndexOf(searchStr, 0);
    //            while (index >= 0)
    //            {
    //                replaceImpl(index, index + searchLen, searchLen, replaceStr, replaceLen);
    //                index = IndexOf(searchStr, index + replaceLen);
    //            }
    //        }
    //        return this;
    //    }

    //    /// <summary>
    //    /// Replaces the first instance of the search string with the replace string.
    //    /// </summary>
    //    /// <param name="searchStr">  the search string, null causes no action to occur </param>
    //    /// <param name="replaceStr">  the replace string, null is equivalent to an empty string </param>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder replaceFirst(string searchStr, string replaceStr)
    //    {
    //        int searchLen = (searchStr == null ? 0 : searchStr.Length);
    //        if (searchLen > 0)
    //        {
    //            int index = IndexOf(searchStr, 0);
    //            if (index >= 0)
    //            {
    //                int replaceLen = (replaceStr == null ? 0 : replaceStr.Length);
    //                replaceImpl(index, index + searchLen, searchLen, replaceStr, replaceLen);
    //            }
    //        }
    //        return this;
    //    }


    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Reverses the string builder placing each character in the opposite index.
    //    /// </summary>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder reverse()
    //    {
    //        if (size_Renamed == 0)
    //        {
    //            return this;
    //        }

    //        int half = size_Renamed / 2;
    //        char[] buf = buffer;
    //        for (int leftIdx = 0, rightIdx = size_Renamed - 1; leftIdx < half; leftIdx++, rightIdx--)
    //        {
    //            char swap = buf[leftIdx];
    //            buf[leftIdx] = buf[rightIdx];
    //            buf[rightIdx] = swap;
    //        }
    //        return this;
    //    }

    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Trims the builder by removing characters less than or equal to a space
    //    /// from the beginning and end.
    //    /// </summary>
    //    /// <returns> this, to enable chaining </returns>
    //    public StrBuilder Trim()
    //    {
    //        if (size_Renamed == 0)
    //        {
    //            return this;
    //        }
    //        int len = size_Renamed;
    //        char[] buf = buffer;
    //        int pos = 0;
    //        while (pos < len && buf[pos] <= ' ')
    //        {
    //            pos++;
    //        }
    //        while (pos < len && buf[len - 1] <= ' ')
    //        {
    //            len--;
    //        }
    //        if (len < size_Renamed)
    //        {
    //            Delete(len, size_Renamed);
    //        }
    //        if (pos > 0)
    //        {
    //            Delete(0, pos);
    //        }
    //        return this;
    //    }

    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Checks whether this builder starts with the specified string.
    //    /// <p>
    //    /// Note that this method handles null input quietly, unlike String.
    //    /// </summary>
    //    /// <param name="str">  the string to search for, null returns false </param>
    //    /// <returns> true if the builder starts with the string </returns>
    //    public bool StartsWith(string str)
    //    {
    //        if (str == null)
    //        {
    //            return false;
    //        }
    //        int len = str.Length;
    //        if (len == 0)
    //        {
    //            return true;
    //        }
    //        if (len > size_Renamed)
    //        {
    //            return false;
    //        }
    //        for (int i = 0; i < len; i++)
    //        {
    //            if (buffer[i] != str[i])
    //            {
    //                return false;
    //            }
    //        }
    //        return true;
    //    }

    //    /// <summary>
    //    /// Checks whether this builder ends with the specified string.
    //    /// <p>
    //    /// Note that this method handles null input quietly, unlike String.
    //    /// </summary>
    //    /// <param name="str">  the string to search for, null returns false </param>
    //    /// <returns> true if the builder ends with the string </returns>
    //    public bool EndsWith(string str)
    //    {
    //        if (str == null)
    //        {
    //            return false;
    //        }
    //        int len = str.Length;
    //        if (len == 0)
    //        {
    //            return true;
    //        }
    //        if (len > size_Renamed)
    //        {
    //            return false;
    //        }
    //        int pos = size_Renamed - len;
    //        for (int i = 0; i < len; i++, pos++)
    //        {
    //            if (buffer[pos] != str[i])
    //            {
    //                return false;
    //            }
    //        }
    //        return true;
    //    }

    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Extracts a portion of this string builder as a string.
    //    /// </summary>
    //    /// <param name="start">  the start index, inclusive, must be valid </param>
    //    /// <returns> the new string </returns>
    //    /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
    //    public string Substring(int start)
    //    {
    //        return Substring(start, size_Renamed);
    //    }

    //    /// <summary>
    //    /// Extracts a portion of this string builder as a string.
    //    /// <p>
    //    /// Note: This method treats an endIndex greater than the length of the
    //    /// builder as equal to the length of the builder, and continues
    //    /// without error, unlike StringBuilder or String.
    //    /// </summary>
    //    /// <param name="startIndex">  the start index, inclusive, must be valid </param>
    //    /// <param name="endIndex">  the end index, exclusive, must be valid except
    //    ///  that if too large it is treated as end of string </param>
    //    /// <returns> the new string </returns>
    //    /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
    //    public string Substring(int startIndex, int endIndex)
    //    {
    //        endIndex = ValidateRange(startIndex, endIndex);
    //        return new string(buffer, startIndex, endIndex - startIndex);
    //    }

    //    /// <summary>
    //    /// Extracts the leftmost characters from the string builder without
    //    /// throwing an exception.
    //    /// <p>
    //    /// This method extracts the left <code>length</code> characters from
    //    /// the builder. If this many characters are not available, the whole
    //    /// builder is returned. Thus the returned string may be shorter than the
    //    /// length requested.
    //    /// </summary>
    //    /// <param name="length">  the number of characters to extract, negative returns empty string </param>
    //    /// <returns> the new string </returns>
    //    public string LeftString(int length)
    //    {
    //        if (length <= 0)
    //        {
    //            return "";
    //        }
    //        else if (length >= size_Renamed)
    //        {
    //            return new string(buffer, 0, size_Renamed);
    //        }
    //        else
    //        {
    //            return new string(buffer, 0, length);
    //        }
    //    }

    //    /// <summary>
    //    /// Extracts the rightmost characters from the string builder without
    //    /// throwing an exception.
    //    /// <p>
    //    /// This method extracts the right <code>length</code> characters from
    //    /// the builder. If this many characters are not available, the whole
    //    /// builder is returned. Thus the returned string may be shorter than the
    //    /// length requested.
    //    /// </summary>
    //    /// <param name="length">  the number of characters to extract, negative returns empty string </param>
    //    /// <returns> the new string </returns>
    //    public string RightString(int length)
    //    {
    //        if (length <= 0)
    //        {
    //            return "";
    //        }
    //        else if (length >= size_Renamed)
    //        {
    //            return new string(buffer, 0, size_Renamed);
    //        }
    //        else
    //        {
    //            return new string(buffer, size_Renamed - length, length);
    //        }
    //    }

    //    /// <summary>
    //    /// Extracts some characters from the middle of the string builder without
    //    /// throwing an exception.
    //    /// <p>
    //    /// This method extracts <code>length</code> characters from the builder
    //    /// at the specified index.
    //    /// If the index is negative it is treated as zero.
    //    /// If the index is greater than the builder size, it is treated as the builder size.
    //    /// If the length is negative, the empty string is returned.
    //    /// If insufficient characters are available in the builder, as much as possible is returned.
    //    /// Thus the returned string may be shorter than the length requested.
    //    /// </summary>
    //    /// <param name="index">  the index to start at, negative means zero </param>
    //    /// <param name="length">  the number of characters to extract, negative returns empty string </param>
    //    /// <returns> the new string </returns>
    //    public string MidString(int index, int length)
    //    {
    //        if (index < 0)
    //        {
    //            index = 0;
    //        }
    //        if (length <= 0 || index >= size_Renamed)
    //        {
    //            return "";
    //        }
    //        if (size_Renamed <= index + length)
    //        {
    //            return new string(buffer, index, size_Renamed - index);
    //        }
    //        else
    //        {
    //            return new string(buffer, index, length);
    //        }
    //    }

    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Checks if the string builder contains the specified char.
    //    /// </summary>
    //    /// <param name="ch">  the character to find </param>
    //    /// <returns> true if the builder contains the character </returns>
    //    public bool Contains(char ch)
    //    {
    //        char[] thisBuf = buffer;
    //        for (int i = 0; i < this.size_Renamed; i++)
    //        {
    //            if (thisBuf[i] == ch)
    //            {
    //                return true;
    //            }
    //        }
    //        return false;
    //    }

    //    /// <summary>
    //    /// Checks if the string builder contains the specified string.
    //    /// </summary>
    //    /// <param name="str">  the string to find </param>
    //    /// <returns> true if the builder contains the string </returns>
    //    public bool Contains(string str)
    //    {
    //        return IndexOf(str, 0) >= 0;
    //    }

    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Searches the string builder to find the first reference to the specified char.
    //    /// </summary>
    //    /// <param name="ch">  the character to find </param>
    //    /// <returns> the first index of the character, or -1 if not found </returns>
    //    public int IndexOf(char ch)
    //    {
    //        return IndexOf(ch, 0);
    //    }

    //    /// <summary>
    //    /// Searches the string builder to find the first reference to the specified char.
    //    /// </summary>
    //    /// <param name="ch">  the character to find </param>
    //    /// <param name="startIndex">  the index to start at, invalid index rounded to edge </param>
    //    /// <returns> the first index of the character, or -1 if not found </returns>
    //    public int IndexOf(char ch, int startIndex)
    //    {
    //        startIndex = (startIndex < 0 ? 0 : startIndex);
    //        if (startIndex >= size_Renamed)
    //        {
    //            return -1;
    //        }
    //        char[] thisBuf = buffer;
    //        for (int i = startIndex; i < size_Renamed; i++)
    //        {
    //            if (thisBuf[i] == ch)
    //            {
    //                return i;
    //            }
    //        }
    //        return -1;
    //    }

    //    /// <summary>
    //    /// Searches the string builder to find the first reference to the specified string.
    //    /// <p>
    //    /// Note that a null input string will return -1, whereas the JDK throws an exception.
    //    /// </summary>
    //    /// <param name="str">  the string to find, null returns -1 </param>
    //    /// <returns> the first index of the string, or -1 if not found </returns>
    //    public int IndexOf(string str)
    //    {
    //        return IndexOf(str, 0);
    //    }

    //    /// <summary>
    //    /// Searches the string builder to find the first reference to the specified
    //    /// string starting searching from the given index.
    //    /// <p>
    //    /// Note that a null input string will return -1, whereas the JDK throws an exception.
    //    /// </summary>
    //    /// <param name="str">  the string to find, null returns -1 </param>
    //    /// <param name="startIndex">  the index to start at, invalid index rounded to edge </param>
    //    /// <returns> the first index of the string, or -1 if not found </returns>
    //    public int IndexOf(string str, int startIndex)
    //    {
    //        startIndex = (startIndex < 0 ? 0 : startIndex);
    //        if (str == null || startIndex >= size_Renamed)
    //        {
    //            return -1;
    //        }
    //        int strLen = str.Length;
    //        if (strLen == 1)
    //        {
    //            return IndexOf(str[0], startIndex);
    //        }
    //        if (strLen == 0)
    //        {
    //            return startIndex;
    //        }
    //        if (strLen > size_Renamed)
    //        {
    //            return -1;
    //        }
    //        char[] thisBuf = buffer;
    //        int len = size_Renamed - strLen + 1;
    //    outer:
    //        for (int i = startIndex; i < len; i++)
    //        {
    //            for (int j = 0; j < strLen; j++)
    //            {
    //                if (str[j] != thisBuf[i + j])
    //                {
    //                    goto outer;
    //                }
    //            }
    //            return i;
    //        }
    //        return -1;
    //    }

    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Searches the string builder to find the last reference to the specified char.
    //    /// </summary>
    //    /// <param name="ch">  the character to find </param>
    //    /// <returns> the last index of the character, or -1 if not found </returns>
    //    public int LastIndexOf(char ch)
    //    {
    //        return LastIndexOf(ch, size_Renamed - 1);
    //    }

    //    /// <summary>
    //    /// Searches the string builder to find the last reference to the specified char.
    //    /// </summary>
    //    /// <param name="ch">  the character to find </param>
    //    /// <param name="startIndex">  the index to start at, invalid index rounded to edge </param>
    //    /// <returns> the last index of the character, or -1 if not found </returns>
    //    public int LastIndexOf(char ch, int startIndex)
    //    {
    //        startIndex = (startIndex >= size_Renamed ? size_Renamed - 1 : startIndex);
    //        if (startIndex < 0)
    //        {
    //            return -1;
    //        }
    //        for (int i = startIndex; i >= 0; i--)
    //        {
    //            if (buffer[i] == ch)
    //            {
    //                return i;
    //            }
    //        }
    //        return -1;
    //    }

    //    /// <summary>
    //    /// Searches the string builder to find the last reference to the specified string.
    //    /// <p>
    //    /// Note that a null input string will return -1, whereas the JDK throws an exception.
    //    /// </summary>
    //    /// <param name="str">  the string to find, null returns -1 </param>
    //    /// <returns> the last index of the string, or -1 if not found </returns>
    //    public int LastIndexOf(string str)
    //    {
    //        return LastIndexOf(str, size_Renamed - 1);
    //    }

    //    /// <summary>
    //    /// Searches the string builder to find the last reference to the specified
    //    /// string starting searching from the given index.
    //    /// <p>
    //    /// Note that a null input string will return -1, whereas the JDK throws an exception.
    //    /// </summary>
    //    /// <param name="str">  the string to find, null returns -1 </param>
    //    /// <param name="startIndex">  the index to start at, invalid index rounded to edge </param>
    //    /// <returns> the last index of the string, or -1 if not found </returns>
    //    public int LastIndexOf(string str, int startIndex)
    //    {
    //        startIndex = (startIndex >= size_Renamed ? size_Renamed - 1 : startIndex);
    //        if (str == null || startIndex < 0)
    //        {
    //            return -1;
    //        }
    //        int strLen = str.Length;
    //        if (strLen > 0 && strLen <= size_Renamed)
    //        {
    //            if (strLen == 1)
    //            {
    //                return LastIndexOf(str[0], startIndex);
    //            }

    //        outer:
    //            for (int i = startIndex - strLen + 1; i >= 0; i--)
    //            {
    //                for (int j = 0; j < strLen; j++)
    //                {
    //                    if (str[j] != buffer[i + j])
    //                    {
    //                        goto outer;
    //                    }
    //                }
    //                return i;
    //            }

    //        }
    //        else if (strLen == 0)
    //        {
    //            return startIndex;
    //        }
    //        return -1;
    //    }


    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Gets the contents of this builder as a Reader.
    //    /// <p>
    //    /// This method allows the contents of the builder to be read
    //    /// using any standard method that expects a Reader.
    //    /// <p>
    //    /// To use, simply create a <code>StrBuilder</code>, populate it with
    //    /// data, call <code>asReader</code>, and then read away.
    //    /// <p>
    //    /// The internal character array is shared between the builder and the reader.
    //    /// This allows you to Append to the builder after creating the reader,
    //    /// and the changes will be picked up.
    //    /// Note however, that no synchronization occurs, so you must perform
    //    /// all operations with the builder and the reader in one thread.
    //    /// <p>
    //    /// The returned reader supports marking, and ignores the flush method.
    //    /// </summary>
    //    /// <returns> a reader that reads from this builder </returns>
    //    public TextReader AsReader()
    //    {
    //        return new StrBuilderReader(this);
    //    }

    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Gets this builder as a Writer that can be written to.
    //    /// <p>
    //    /// This method allows you to populate the contents of the builder
    //    /// using any standard method that takes a Writer.
    //    /// <p>
    //    /// To use, simply create a <code>StrBuilder</code>,
    //    /// call <code>asWriter</code>, and populate away. The data is available
    //    /// at any time using the methods of the <code>StrBuilder</code>.
    //    /// <p>
    //    /// The internal character array is shared between the builder and the writer.
    //    /// This allows you to intermix calls that Append to the builder and
    //    /// write using the writer and the changes will be occur correctly.
    //    /// Note however, that no synchronization occurs, so you must perform
    //    /// all operations with the builder and the writer in one thread.
    //    /// <p>
    //    /// The returned writer ignores the close and flush methods.
    //    /// </summary>
    //    /// <returns> a writer that populates this builder </returns>
    //    public TextWriter AsWriter()
    //    {
    //        return new StrBuilderWriter(this);
    //    }

    //    //-----------------------------------------------------------------------
    //    //    /**
    //    //     * Gets a String version of the string builder by calling the internal
    //    //     * constructor of String by reflection.
    //    //     * <p>
    //    //     * WARNING: You must not use the StrBuilder after calling this method
    //    //     * as the buffer is now shared with the String object. To ensure this,
    //    //     * the internal character array is set to null, so you will get
    //    //     * NullPointerExceptions on all method calls.
    //    //     *
    //    //     * @return the builder as a String
    //    //     */
    //    //    public String toSharedString() {
    //    //        try {
    //    //            Constructor con = String.class.getDeclaredConstructor(
    //    //                new Class[] {int.class, int.class, char[].class});
    //    //            con.setAccessible(true);
    //    //            char[] buffer = buf;
    //    //            buf = null;
    //    //            size = -1;
    //    //            nullText = null;
    //    //            return (String) con.newInstance(
    //    //                new Object[] {new Integer(0), new Integer(size), buffer});
    //    //            
    //    //        } catch (Exception ex) {
    //    //            ex.printStackTrace();
    //    //            throw new UnsupportedOperationException("StrBuilder.toSharedString is unsupported: " + ex.getMessage());
    //    //        }
    //    //    }

    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Checks the contents of this builder against another to see if they
    //    /// contain the same character content ignoring case.
    //    /// </summary>
    //    /// <param name="other">  the object to check, null returns false </param>
    //    /// <returns> true if the builders contain the same characters in the same order </returns>
    //    public bool EqualsIgnoreCase(StrBuilder other)
    //    {
    //        if (this == other)
    //        {
    //            return true;
    //        }
    //        if (this.size_Renamed != other.size_Renamed)
    //        {
    //            return false;
    //        }
    //        char[] thisBuf = this.buffer;
    //        char[] otherBuf = other.buffer;
    //        for (int i = size_Renamed - 1; i >= 0; i--)
    //        {
    //            char c1 = thisBuf[i];
    //            char c2 = otherBuf[i];
    //            if (c1 != c2 && char.ToUpper(c1) != char.ToUpper(c2))
    //            {
    //                return false;
    //            }
    //        }
    //        return true;
    //    }

    //    /// <summary>
    //    /// Checks the contents of this builder against another to see if they
    //    /// contain the same character content.
    //    /// </summary>
    //    /// <param name="other">  the object to check, null returns false </param>
    //    /// <returns> true if the builders contain the same characters in the same order </returns>
    //    public bool Equals(StrBuilder other)
    //    {
    //        if (this == other)
    //        {
    //            return true;
    //        }
    //        if (this.size_Renamed != other.size_Renamed)
    //        {
    //            return false;
    //        }
    //        char[] thisBuf = this.buffer;
    //        char[] otherBuf = other.buffer;
    //        for (int i = size_Renamed - 1; i >= 0; i--)
    //        {
    //            if (thisBuf[i] != otherBuf[i])
    //            {
    //                return false;
    //            }
    //        }
    //        return true;
    //    }

    //    /// <summary>
    //    /// Checks the contents of this builder against another to see if they
    //    /// contain the same character content.
    //    /// </summary>
    //    /// <param name="obj">  the object to check, null returns false </param>
    //    /// <returns> true if the builders contain the same characters in the same order </returns>
    //    public bool Equals(object obj)
    //    {
    //        if (obj is StrBuilder)
    //        {
    //            return Equals((StrBuilder)obj);
    //        }
    //        return false;
    //    }

    //    /// <summary>
    //    /// Gets a suitable hash code for this builder.
    //    /// </summary>
    //    /// <returns> a hash code </returns>
    //    public int GetHashCode()
    //    {
    //        char[] buf = buffer;
    //        int hash = 0;
    //        for (int i = size_Renamed - 1; i >= 0; i--)
    //        {
    //            hash = 31 * hash + buf[i];
    //        }
    //        return hash;
    //    }

    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Gets a String version of the string builder, creating a new instance
    //    /// each time the method is called.
    //    /// <p>
    //    /// Note that unlike StringBuilder, the string version returned is
    //    /// independent of the string builder.
    //    /// </summary>
    //    /// <returns> the builder as a String </returns>
    //    public string ToString()
    //    {
    //        return new string(buffer, 0, size_Renamed);
    //    }

    //    /// <summary>
    //    /// Gets a StringBuilder version of the string builder, creating a
    //    /// new instance each time the method is called.
    //    /// </summary>
    //    /// <returns> the builder as a StringBuilder </returns>
    //    public StringBuilder ToStringBuilder()
    //    {
    //        return (new StringBuilder(size_Renamed)).Append(buffer, 0, size_Renamed);
    //    }

    //    /// <summary>
    //    /// Clone this object.
    //    /// </summary>
    //    /// <returns> a clone of this object </returns>
    //    /// <exception cref="CloneNotSupportedException"> if clone is not supported
    //    /// @since 2.6 </exception>
    //    //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
    //    //ORIGINAL LINE: public Object Clone() throws CloneNotSupportedException
    //    public object Clone()
    //    {
    //        StrBuilder clone = (StrBuilder)this.Clone();
    //        clone.buffer = new char[buffer.Length];
    //        Array.Copy(buffer, 0, clone.buffer, 0, buffer.Length);
    //        return clone;
    //    }

    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Validates parameters defining a range of the builder.
    //    /// </summary>
    //    /// <param name="startIndex">  the start index, inclusive, must be valid </param>
    //    /// <param name="endIndex">  the end index, exclusive, must be valid except
    //    ///  that if too large it is treated as end of string </param>
    //    /// <returns> the new string </returns>
    //    /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
    //    protected internal int ValidateRange(int startIndex, int endIndex)
    //    {
    //        if (startIndex < 0)
    //        {
    //            //throw new StringIndexOutOfBoundsException(startIndex);
    //        }
    //        if (endIndex > size_Renamed)
    //        {
    //            endIndex = size_Renamed;
    //        }
    //        if (startIndex > endIndex)
    //        {
    //            //throw new StringIndexOutOfBoundsException("end < start");
    //        }
    //        return endIndex;
    //    }

    //    /// <summary>
    //    /// Validates parameters defining a single index in the builder.
    //    /// </summary>
    //    /// <param name="index">  the index, must be valid </param>
    //    /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
    //    protected internal void ValidateIndex(int index)
    //    {
    //        if (index < 0 || index > size_Renamed)
    //        {
    //            //throw new StringIndexOutOfBoundsException(index);
    //        }
    //    }


    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Inner class to allow StrBuilder to operate as a writer.
    //    /// </summary>
    //    internal class StrBuilderReader : System.IO.StreamReader
    //    {
    //        private readonly StrBuilder outerInstance;

    //        /// <summary>
    //        /// The current stream position. </summary>
    //        private int pos;
    //        /// <summary>
    //        /// The last mark position. </summary>
    //        private int mark_Renamed;

    //        /// <summary>
    //        /// Default constructor.
    //        /// </summary>
    //        internal StrBuilderReader(StrBuilder outerInstance)
    //            : base()
    //        {
    //            this.outerInstance = outerInstance;
    //        }

    //        /// <summary>
    //        /// {@inheritDoc} </summary>
    //        public void Close()
    //        {
    //            // do nothing
    //        }

    //        /// <summary>
    //        /// {@inheritDoc} </summary>
    //        public int Read()
    //        {
    //            if (Ready() == false)
    //            {
    //                return -1;
    //            }
    //            return outerInstance[pos++];
    //        }

    //        /// <summary>
    //        /// {@inheritDoc} </summary>
    //        public new int Read(char[] b, int off, int len)
    //        {
    //            if (off < 0 || len < 0 || off > b.Length || (off + len) > b.Length || (off + len) < 0)
    //            {
    //                throw new System.IndexOutOfRangeException();
    //            }
    //            if (len == 0)
    //            {
    //                return 0;
    //            }
    //            if (pos >= outerInstance.Count)
    //            {
    //                return -1;
    //            }
    //            if (pos + len > Count)
    //            {
    //                len = outerInstance.Count - pos;
    //            }
    //            outerInstance.GetChars(pos, pos + len, b, off);
    //            pos += len;
    //            return len;
    //        }

    //        /// <summary>
    //        /// {@inheritDoc} </summary>
    //        public long Skip(long n)
    //        {
    //            if (pos + n > outerInstance.Count)
    //            {
    //                n = outerInstance.Count - pos;
    //            }
    //            if (n < 0)
    //            {
    //                return 0;
    //            }
    //            pos += Convert.ToInt32(n);
    //            return n;
    //        }

    //        /// <summary>
    //        /// {@inheritDoc} </summary>
    //        public bool Ready()
    //        {
    //            return pos < outerInstance.Count;
    //        }

    //        /// <summary>
    //        /// {@inheritDoc} </summary>
    //        public bool MarkSupported()
    //        {
    //            return true;
    //        }

    //        /// <summary>
    //        /// {@inheritDoc} </summary>
    //        public void Mark(int readAheadLimit)
    //        {
    //            mark_Renamed = pos;
    //        }

    //        /// <summary>
    //        /// {@inheritDoc} </summary>
    //        public void Reset()
    //        {
    //            pos = mark_Renamed;
    //        }
    //    }

    //    //-----------------------------------------------------------------------
    //    /// <summary>
    //    /// Inner class to allow StrBuilder to operate as a writer.
    //    /// </summary>
    //    internal class StrBuilderWriter : System.IO.StreamWriter
    //    {
    //        private readonly StrBuilder outerInstance;


    //        /// <summary>
    //        /// Default constructor.
    //        /// </summary>
    //        internal StrBuilderWriter(StrBuilder outerInstance)
    //            : base()
    //        {
    //            this.outerInstance = outerInstance;
    //        }

    //        /// <summary>
    //        /// {@inheritDoc} </summary>
    //        public void Close()
    //        {
    //            // do nothing
    //        }

    //        /// <summary>
    //        /// {@inheritDoc} </summary>
    //        public void Flush()
    //        {
    //            // do nothing
    //        }

    //        /// <summary>
    //        /// {@inheritDoc} </summary>
    //        public void Write(int c)
    //        {
    //            outerInstance.Append((char)c);
    //        }

    //        /// <summary>
    //        /// {@inheritDoc} </summary>
    //        public void Write(char[] cbuf)
    //        {
    //            outerInstance.Append(cbuf);
    //        }

    //        /// <summary>
    //        /// {@inheritDoc} </summary>
    //        public void Write(char[] cbuf, int off, int len)
    //        {
    //            outerInstance.Append(cbuf, off, len);
    //        }

    //        /// <summary>
    //        /// {@inheritDoc} </summary>
    //        public void Write(string str)
    //        {
    //            outerInstance.Append(str);
    //        }

    //        /// <summary>
    //        /// {@inheritDoc} </summary>
    //        public void Write(string str, int off, int len)
    //        {
    //            outerInstance.Append(str, off, len);
    //        }
    //    }


    //}
}
