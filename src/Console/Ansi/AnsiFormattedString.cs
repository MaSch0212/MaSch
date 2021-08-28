using MaSch.Core;
using MaSch.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaSch.Console.Ansi
{
    /// <summary>
    /// Represents a mutable string of characters with ANSI styling. This class cannot be inherited.
    /// </summary>
    public sealed class AnsiFormattedString
    {
        private readonly StringBuilder _builder;
        private readonly LinkedList<StyleRange> _styles = new();
        private int _nextStyleId;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnsiFormattedString"/> class.
        /// </summary>
        public AnsiFormattedString()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnsiFormattedString"/> class.
        /// </summary>
        /// <param name="value">The initial unstyled value.</param>
        public AnsiFormattedString(string? value)
        {
            _builder = new StringBuilder(value);
        }

        /// <summary>
        /// Gets or sets the maximum number of characters that can be contained in the memory allocated by the current instance.
        /// </summary>
        public int Capacity
        {
            get => _builder.Capacity;
            set => _builder.Capacity = value;
        }

        /// <summary>
        /// Gets or sets the length of the current System.Text.StringBuilder object.
        /// </summary>
        public int Length
        {
            get => _builder.Length;
            set
            {
                var preLen = _builder.Length;
                _builder.Length = value;
                if (preLen > value)
                    OnRemove(value, preLen - value);
            }
        }

        /// <summary>
        /// Gets or sets the character at the specified character position in this instance.
        /// </summary>
        /// <param name="index">The position of the character.</param>
        /// <returns>The Unicode character at position <paramref name="index"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside the bounds of this instance while setting a character.</exception>
        /// <exception cref="IndexOutOfRangeException"><paramref name="index"/> is outside the bounds of this instance while getting a character.</exception>
        public char this[int index]
        {
            get => _builder[index];
            set => _builder[index] = value;
        }

        /// <summary>
        /// Appends an ANSI style to this instance. This style is used for every text that is added to the end of this instance.
        /// </summary>
        /// <param name="styleAction">The style builder delegate.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString AppendStyle(Action<AnsiStyle>? styleAction)
        {
            if (styleAction is null)
                return this;
            return ApplyStyleImpl(Length, int.MaxValue - Length, styleAction);
        }

        /// <summary>
        /// Applies an ANSI style to the specified part of this instance.
        /// </summary>
        /// <param name="startIndex">The start index for the style.</param>
        /// <param name="length">The length of text to apply the style to.</param>
        /// <param name="styleAction">The style builder delegate.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString ApplyStyle(int startIndex, int length, Action<AnsiStyle>? styleAction)
        {
            if (styleAction is null)
                return this;
            ValidateRange(startIndex, length);
            return ApplyStyleImpl(startIndex, length, styleAction);
        }

        /// <summary>
        /// Appends the string representation of a specified <see cref="IFormattable"/> to this instance.
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Append(IFormattable? value, IFormatProvider? formatProvider)
            => Append(value, formatProvider, null);

        /// <summary>
        /// Appends the string representation of a specified <see cref="IFormattable"/> with a specified style to this instance.
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
        /// <param name="styleAction">The style builder delegate.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Append(IFormattable? value, IFormatProvider? formatProvider, Action<AnsiStyle>? styleAction)
            => CallInsert(Length, value?.ToString(null, formatProvider), (b, i, x) => b.Append(x), styleAction);

        /// <summary>
        /// Appends a copy of the specified string to this instance.
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Append(string? value)
            => Append(value, null);

        /// <summary>
        /// Appends a styled copy of the specified string to this instance.
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <param name="styleAction">The style builder delegate.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Append(string? value, Action<AnsiStyle>? styleAction)
            => CallInsert(Length, value, (b, i, x) => b.Append(x), styleAction);

        /// <summary>
        /// Appends a copy of a specified substring to this instance.
        /// </summary>
        /// <param name="value">The string that contains the substring to append.</param>
        /// <param name="startIndex">The starting position of the substring within <paramref name="value"/>.</param>
        /// <param name="count">The number of characters in <paramref name="value"/> to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Append(string? value, int startIndex, int count)
            => Append(value, startIndex, count, null);

        /// <summary>
        /// Appends a styled copy of a specified substring to this instance.
        /// </summary>
        /// <param name="value">The string that contains the substring to append.</param>
        /// <param name="startIndex">The starting position of the substring within <paramref name="value"/>.</param>
        /// <param name="count">The number of characters in <paramref name="value"/> to append.</param>
        /// <param name="styleAction">The style builder delegate.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Append(string? value, int startIndex, int count, Action<AnsiStyle>? styleAction)
            => CallInsert(Length, value, startIndex, count, (b, i, x, y, z) => b.Append(x, y, z), styleAction);

        /// <summary>
        /// Appends a copy of a substring within a specified string builder to this instance.
        /// </summary>
        /// <param name="value">The string builder that contains the substring to append.</param>
        /// <param name="startIndex">The starting position of the substring within <paramref name="value"/>.</param>
        /// <param name="count">The number of characters in <paramref name="value"/> to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Append(StringBuilder? value, int startIndex, int count)
            => Append(value, startIndex, count, null);

        /// <summary>
        /// Appends a styled copy of a substring within a specified string builder to this instance.
        /// </summary>
        /// <param name="value">The string builder that contains the substring to append.</param>
        /// <param name="startIndex">The starting position of the substring within <paramref name="value"/>.</param>
        /// <param name="count">The number of characters in <paramref name="value"/> to append.</param>
        /// <param name="styleAction">The style builder delegate.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Append(StringBuilder? value, int startIndex, int count, Action<AnsiStyle>? styleAction)
#if NETFRAMEWORK
            => CallInsert(Length, value?.ToString(), startIndex, count, (b, i, x, y, z) => b.Append(x, y, z), styleAction);
#else
            => CallInsert(Length, value, startIndex, count, (b, i, x, y, z) => b.Append(x, y, z), styleAction);
#endif

        /// <summary>
        /// Appends the string representation of a specified string builder to this instance.
        /// </summary>
        /// <param name="value">The string builder to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Append(StringBuilder? value)
            => Append(value, null);

        /// <summary>
        /// Appends the string representation of a specified string builder with a specified style to this instance.
        /// </summary>
        /// <param name="value">The string builder to append.</param>
        /// <param name="styleAction">The style builder delegate.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Append(StringBuilder? value, Action<AnsiStyle>? styleAction)
            => CallInsert(Length, value, (b, i, x) => b.Append(x), styleAction);

        /// <summary>
        /// Appends the string representation of a specified object to this instance.
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Append(object? value)
            => Append(value, null);

        /// <summary>
        /// Appends the string representation of a specified object with a specified style to this instance.
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <param name="styleAction">The style builder delegate.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Append(object? value, Action<AnsiStyle>? styleAction)
            => CallInsert(Length, value, (b, i, x) => b.Append(x), styleAction);

        /// <summary>
        /// Appends the string representation of a specified subarray of Unicode characters to this instance.
        /// </summary>
        /// <param name="value">A character array.</param>
        /// <param name="startIndex">The starting position in value.</param>
        /// <param name="count">The number of characters to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Append(char[]? value, int startIndex, int count)
            => Append(value, startIndex, count, null);

        /// <summary>
        /// Appends the string representation of a specified subarray of Unicode characters with a specified style to this instance.
        /// </summary>
        /// <param name="value">A character array.</param>
        /// <param name="startIndex">The starting position in value.</param>
        /// <param name="count">The number of characters to append.</param>
        /// <param name="styleAction">The style builder delegate.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Append(char[]? value, int startIndex, int count, Action<AnsiStyle>? styleAction)
            => CallInsert(Length, value, startIndex, count, (b, i, x, y, z) => b.Append(x, y, z), styleAction);

        /// <summary>
        /// Appends the string representation of the Unicode characters in a specified array to this instance.
        /// </summary>
        /// <param name="value">The array of characters to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Append(char[]? value)
            => Append(value, null);

        /// <summary>
        /// Appends the string representation of the Unicode characters in a specified array with a specified style to this instance.
        /// </summary>
        /// <param name="value">The array of characters to append.</param>
        /// <param name="styleAction">The style builder delegate.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Append(char[]? value, Action<AnsiStyle>? styleAction)
            => CallInsert(Length, value, (b, i, x) => b.Append(x), styleAction);

        /// <summary>
        /// Appends a specified number of copies of the string representation of a Unicode character to this instance.
        /// </summary>
        /// <param name="value">The UTF-16-encoded code unit to append.</param>
        /// <param name="repeatCount">The number of times to append value.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Append(char value, int repeatCount)
            => Append(value, repeatCount, null);

        /// <summary>
        /// Appends a specified number of copies of the string representation of a Unicode character with a specified style to this instance.
        /// </summary>
        /// <param name="value">The UTF-16-encoded code unit to append.</param>
        /// <param name="repeatCount">The number of times to append value.</param>
        /// <param name="styleAction">The style builder delegate.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Append(char value, int repeatCount, Action<AnsiStyle>? styleAction)
            => CallInsert(Length, value, repeatCount, (b, i, x, y) => b.Append(x, y), styleAction);

        /// <summary>
        /// Appends the string representation of a specified System.Char object to this instance.
        /// </summary>
        /// <param name="value">The UTF-16-encoded code unit to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Append(char value)
            => Append(value, null);

        /// <summary>
        /// Appends the string representation of a specified System.Char object with a specified style to this instance.
        /// </summary>
        /// <param name="value">The UTF-16-encoded code unit to append.</param>
        /// <param name="styleAction">The style builder delegate.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Append(char value, Action<AnsiStyle>? styleAction)
            => CallInsert(Length, value, (b, i, x) => b.Append(x), styleAction);

        /// <summary>
        /// Appends the string returned by processing a composite format string, which contains
        /// zero or more format items, to this instance. Each format item is replaced by
        /// the string representation of a corresponding argument in a parameter array.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to format.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString AppendFormat(string format, params object?[] args)
            => AppendFormat((Action<AnsiStyle>?)null, format, args);

        /// <summary>
        /// Appends the string returned by processing a composite format string, which contains
        /// zero or more format items, with a specified style to this instance. Each format item is replaced by
        /// the string representation of a corresponding argument in a parameter array.
        /// </summary>
        /// <param name="styleAction">The style builder delegate.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to format.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString AppendFormat(Action<AnsiStyle>? styleAction, string format, params object?[] args)
            => CallInsert(Length, format, args, (b, i, x, y) => b.AppendFormat(x, y), styleAction);

        /// <summary>
        /// Appends the string returned by processing a composite format string, which contains
        /// zero or more format items, to this instance. Each format item is replaced by
        /// the string representation of a corresponding argument in a parameter array using
        /// a specified format provider.
        /// </summary>
        /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to format.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString AppendFormat(IFormatProvider? formatProvider, string format, params object?[] args)
            => AppendFormat(formatProvider, null, format, args);

        /// <summary>
        /// Appends the string returned by processing a composite format string, which contains
        /// zero or more format items, with a specified style to this instance. Each format item is replaced by
        /// the string representation of a corresponding argument in a parameter array using
        /// a specified format provider.
        /// </summary>
        /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
        /// <param name="styleAction">The style builder delegate.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to format.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString AppendFormat(IFormatProvider? formatProvider, Action<AnsiStyle>? styleAction, string format, params object?[] args)
            => CallInsert(Length, formatProvider, format, args, (b, i, x, y, z) => b.AppendFormat(x, y, z), styleAction);

        /// <summary>
        /// Concatenates and appends the members of a collection, using the specified separator between each member.
        /// </summary>
        /// <typeparam name="T">The type of the members of values.</typeparam>
        /// <param name="separator">The string to use as a separator. separator is included in the concatenated and appended strings only if values has more than one element.</param>
        /// <param name="values">A collection that contains the objects to concatenate and append to the current instance of the string builder.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString AppendJoin<T>(string? separator, IEnumerable<T> values)
            => AppendJoin(null, separator, values);

        /// <summary>
        /// Concatenates and appends the members of a collection, using the specified separator between each member with a specified style.
        /// </summary>
        /// <typeparam name="T">The type of the members of values.</typeparam>
        /// <param name="styleAction">The style builder delegate.</param>
        /// <param name="separator">The string to use as a separator. separator is included in the concatenated and appended strings only if values has more than one element.</param>
        /// <param name="values">A collection that contains the objects to concatenate and append to the current instance of the string builder.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString AppendJoin<T>(Action<AnsiStyle>? styleAction, string? separator, IEnumerable<T> values)
#if NETFRAMEWORK
            => CallInsert(Length, string.Join(separator, values), (b, i, x) => b.Append(x), styleAction);
#else
            => CallInsert(Length, separator, values, (b, i, x, y) => b.AppendJoin(x, y), styleAction);
#endif

        /// <summary>
        /// Concatenates the string representations of the elements in the provided array
        /// of objects, using the specified separator between each member, then appends the
        /// result to the current instance of the string builder.
        /// </summary>
        /// <param name="separator">The string to use as a separator. separator is included in the concatenated and appended strings only if values has more than one element.</param>
        /// <param name="values">A collection that contains the objects to concatenate and append to the current instance of the string builder.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString AppendJoin(string? separator, params object?[] values)
            => AppendJoin(null, separator, values);

        /// <summary>
        /// Concatenates the string representations of the elements in the provided array
        /// of objects, using the specified separator between each member, then appends the
        /// result with a specified style to the current instance of the string builder.
        /// </summary>
        /// <param name="styleAction">The style builder delegate.</param>
        /// <param name="separator">The string to use as a separator. separator is included in the concatenated and appended strings only if values has more than one element.</param>
        /// <param name="values">A collection that contains the objects to concatenate and append to the current instance of the string builder.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString AppendJoin(Action<AnsiStyle>? styleAction, string? separator, params object?[] values)
#if NETFRAMEWORK
            => CallInsert(Length, string.Join(separator, values), (b, i, x) => b.Append(x), styleAction);
#else
            => CallInsert(Length, separator, values, (b, i, x, y) => b.AppendJoin(x, y), styleAction);
#endif

        /// <summary>
        /// Appends a copy of the specified string followed by the default line terminator to this instance.
        /// </summary>
        /// <param name="value">The string to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString AppendLine(string? value)
            => AppendLine(value, null);

        /// <summary>
        /// Appends a copy of the specified string followed by the default line terminator with a specified style to this instance.
        /// </summary>
        /// <param name="value">The string to append.</param>
        /// <param name="styleAction">The style builder delegate.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString AppendLine(string? value, Action<AnsiStyle>? styleAction)
            => CallInsert(Length, value, (b, i, x) => b.AppendLine(x), styleAction);

        /// <summary>
        /// Appends the default line terminator to this instance.
        /// </summary>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString AppendLine()
        {
            _builder.AppendLine();
            return this;
        }

        /// <summary>
        /// Inserts the string representation of an object into this instance at the specified character position.
        /// </summary>
        /// <param name="index">The position in this instance where insertion begins.</param>
        /// <param name="value">The object to insert, or null.</param>
        /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Insert(int index, IFormattable? value, IFormatProvider formatProvider)
            => Insert(index, value, formatProvider, null);

        /// <summary>
        /// Inserts the string representation of an object with a specified style into this instance at the specified character position.
        /// </summary>
        /// <param name="index">The position in this instance where insertion begins.</param>
        /// <param name="value">The object to insert, or null.</param>
        /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
        /// <param name="styleAction">The style builder delegate.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Insert(int index, IFormattable? value, IFormatProvider formatProvider, Action<AnsiStyle>? styleAction)
            => CallInsert(index, value?.ToString(null, formatProvider), (b, i, x) => b.Insert(i, x), styleAction);

        /// <summary>
        /// Inserts a string into this instance at the specified character position.
        /// </summary>
        /// <param name="index">The position in this instance where insertion begins.</param>
        /// <param name="value">The string to insert.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Insert(int index, string? value)
            => Insert(index, value, null);

        /// <summary>
        /// Inserts a string with a specified style into this instance at the specified character position.
        /// </summary>
        /// <param name="index">The position in this instance where insertion begins.</param>
        /// <param name="value">The string to insert.</param>
        /// <param name="styleAction">The style builder delegate.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Insert(int index, string? value, Action<AnsiStyle>? styleAction)
            => CallInsert(index, value, (b, i, x) => b.Insert(i, x), styleAction);

        /// <summary>
        /// Inserts one or more copies of a specified string into this instance at the specified character position.
        /// </summary>
        /// <param name="index">The position in this instance where insertion begins.</param>
        /// <param name="value">The string to insert.</param>
        /// <param name="count">The number of times to insert value.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Insert(int index, string? value, int count)
            => Insert(index, value, count, null);

        /// <summary>
        /// Inserts one or more copies of a specified string with a specified style into this instance at the specified character position.
        /// </summary>
        /// <param name="index">The position in this instance where insertion begins.</param>
        /// <param name="value">The string to insert.</param>
        /// <param name="count">The number of times to insert value.</param>
        /// <param name="styleAction">The style builder delegate.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Insert(int index, string? value, int count, Action<AnsiStyle>? styleAction)
            => CallInsert(index, value, count, (b, i, x, y) => b.Insert(i, x, y), styleAction);

        /// <summary>
        /// Inserts the string representation of an object into this instance at the specified character position.
        /// </summary>
        /// <param name="index">The position in this instance where insertion begins.</param>
        /// <param name="value">The object to insert, or null.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Insert(int index, object? value)
            => Insert(index, value, null);

        /// <summary>
        /// Inserts the string representation of an object with a specified style into this instance at the specified character position.
        /// </summary>
        /// <param name="index">The position in this instance where insertion begins.</param>
        /// <param name="value">The object to insert, or null.</param>
        /// <param name="styleAction">The style builder delegate.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Insert(int index, object? value, Action<AnsiStyle>? styleAction)
            => CallInsert(index, value, (b, i, x) => b.Insert(i, x), styleAction);

        /// <summary>
        /// Inserts the string representation of a specified array of Unicode characters into this instance at the specified character position.
        /// </summary>
        /// <param name="index">The position in this instance where insertion begins.</param>
        /// <param name="value">The character array to insert.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Insert(int index, char[]? value)
            => Insert(index, value, null);

        /// <summary>
        /// Inserts the string representation of a specified array of Unicode characters with a specified style into this instance at the specified character position.
        /// </summary>
        /// <param name="index">The position in this instance where insertion begins.</param>
        /// <param name="value">The character array to insert.</param>
        /// <param name="styleAction">The style builder delegate.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Insert(int index, char[]? value, Action<AnsiStyle>? styleAction)
            => CallInsert(index, value, (b, i, x) => b.Insert(i, x), styleAction);

        /// <summary>
        /// Inserts the string representation of a specified subarray of Unicode characters into this instance at the specified character position.
        /// </summary>
        /// <param name="index">The position in this instance where insertion begins.</param>
        /// <param name="value">A character array.</param>
        /// <param name="startIndex">The starting index within value.</param>
        /// <param name="charCount">The number of characters to insert.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Insert(int index, char[]? value, int startIndex, int charCount)
            => Insert(index, value, startIndex, charCount, null);

        /// <summary>
        /// Inserts the string representation of a specified subarray of Unicode characters with a specified style into this instance at the specified character position.
        /// </summary>
        /// <param name="index">The position in this instance where insertion begins.</param>
        /// <param name="value">A character array.</param>
        /// <param name="startIndex">The starting index within value.</param>
        /// <param name="charCount">The number of characters to insert.</param>
        /// <param name="styleAction">The style builder delegate.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Insert(int index, char[]? value, int startIndex, int charCount, Action<AnsiStyle>? styleAction)
            => CallInsert(index, value, startIndex, charCount, (b, i, x, y, z) => b.Insert(i, x, y, z), styleAction);

        /// <summary>
        /// Inserts the string representation of a specified Unicode character into this instance at the specified character position.
        /// </summary>
        /// <param name="index">The position in this instance where insertion begins.</param>
        /// <param name="value">The value to insert.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Insert(int index, char value)
            => Insert(index, value, null);

        /// <summary>
        /// Inserts the string representation of a specified Unicode character with a specified style into this instance at the specified character position.
        /// </summary>
        /// <param name="index">The position in this instance where insertion begins.</param>
        /// <param name="value">The value to insert.</param>
        /// <param name="styleAction">The style builder delegate.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Insert(int index, char value, Action<AnsiStyle>? styleAction)
            => CallInsert(index, value, (b, i, x) => b.Insert(i, x), styleAction);

        /// <summary>
        /// Removes the specified range of characters from this instance.
        /// </summary>
        /// <param name="startIndex">The zero-based position in this instance where removal begins.</param>
        /// <param name="length">The number of characters to remove.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Remove(int startIndex, int length)
        {
            _builder.Remove(startIndex, length);
            OnRemove(startIndex, length);
            return this;
        }

        /// <summary>
        /// Replaces all occurrences of a specified character in this instance with another specified character.
        /// </summary>
        /// <param name="oldChar">The character to replace.</param>
        /// <param name="newChar">The character that replaces oldChar.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Replace(char oldChar, char newChar)
        {
            _builder.Replace(oldChar, newChar);
            return this;
        }

        /// <summary>
        /// Replaces, within a substring of this instance, all occurrences of a specified character with another specified character.
        /// </summary>
        /// <param name="oldChar">The character to replace.</param>
        /// <param name="newChar">The character that replaces oldChar.</param>
        /// <param name="startIndex">The position in this instance where the substring begins.</param>
        /// <param name="count">The length of the substring.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Replace(char oldChar, char newChar, int startIndex, int count)
        {
            _builder.Replace(oldChar, newChar, startIndex, count);
            return this;
        }

        /// <summary>
        /// Replaces all occurrences of a specified character in this instance with another specified character and sets a specified style.
        /// </summary>
        /// <param name="oldChar">The character to replace.</param>
        /// <param name="newChar">The character that replaces oldChar.</param>
        /// <param name="styleAction">The style builder delegate.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Replace(char oldChar, char newChar, Action<AnsiStyle>? styleAction)
            => Replace(oldChar, newChar, 0, Length, styleAction);

        /// <summary>
        /// Replaces, within a substring of this instance, all occurrences of a specified character with another specified character and sets a specified style.
        /// </summary>
        /// <param name="oldChar">The character to replace.</param>
        /// <param name="newChar">The character that replaces oldChar.</param>
        /// <param name="startIndex">The position in this instance where the substring begins.</param>
        /// <param name="count">The length of the substring.</param>
        /// <param name="styleAction">The style builder delegate.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Replace(char oldChar, char newChar, int startIndex, int count, Action<AnsiStyle>? styleAction)
        {
            for (int i = 0; i < count; i++)
            {
                var idx = i + startIndex;
                if (_builder[idx] == oldChar)
                {
                    _builder[idx] = newChar;
                    if (styleAction is not null)
                        ApplyStyleImpl(idx, 1, styleAction);
                }
            }

            return this;
        }

        /// <summary>
        /// Replaces all occurrences of a specified string in this instance with another specified string.
        /// </summary>
        /// <param name="oldValue">The string to replace.</param>
        /// <param name="newValue">The string that replaces oldValue, or null.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Replace(string oldValue, string? newValue)
            => Replace(oldValue, newValue, 0, Length, null);

        /// <summary>
        /// Replaces, within a substring of this instance, all occurrences of a specified string with another specified string.
        /// </summary>
        /// <param name="oldValue">The string to replace.</param>
        /// <param name="newValue">The string that replaces oldValue, or null.</param>
        /// <param name="startIndex">The position in this instance where the substring begins.</param>
        /// <param name="count">The length of the substring.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Replace(string oldValue, string? newValue, int startIndex, int count)
            => Replace(oldValue, newValue, startIndex, count, null);

        /// <summary>
        /// Replaces all occurrences of a specified string in this instance with another specified string and sets a specified style.
        /// </summary>
        /// <param name="oldValue">The string to replace.</param>
        /// <param name="newValue">The string that replaces oldValue, or null.</param>
        /// <param name="styleAction">The style builder delegate.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Replace(string oldValue, string? newValue, Action<AnsiStyle>? styleAction)
            => Replace(oldValue, newValue, 0, Length, styleAction);

        /// <summary>
        /// Replaces, within a substring of this instance, all occurrences of a specified string with another specified string and sets a specified style.
        /// </summary>
        /// <param name="oldValue">The string to replace.</param>
        /// <param name="newValue">The string that replaces oldValue, or null.</param>
        /// <param name="startIndex">The position in this instance where the substring begins.</param>
        /// <param name="count">The length of the substring.</param>
        /// <param name="styleAction">The style builder delegate.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public AnsiFormattedString Replace(string oldValue, string? newValue, int startIndex, int count, Action<AnsiStyle>? styleAction)
        {
            var foundValues = new LinkedList<int>();

            var wordIdx = -1;
            for (int i = 0; i < count; i++)
            {
                var idx = i + startIndex;
                if (_builder[idx] == oldValue[wordIdx + 1])
                    wordIdx++;
                else
                    wordIdx = -1;

                if (wordIdx >= oldValue.Length)
                {
                    foundValues.AddFirst(idx - oldValue.Length);
                    wordIdx = -1;
                }
            }

            foreach (var i in foundValues)
            {
                Remove(i, oldValue.Length);
                if (!string.IsNullOrEmpty(newValue))
                    Insert(i, newValue, styleAction);
            }

            return this;
        }

        /// <summary>
        /// Removes all characters and styles from the current <see cref="AnsiFormattedString"/> instance.
        /// </summary>
        public void Clear()
        {
            _builder.Clear();
            _styles.Clear();
        }

        /// <summary>
        /// Converts the value of this instance to a <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="includeFormatting">Determines wether to include ANSI control sequences int he resulting <see cref="string"/>.</param>
        /// <returns>A <see cref="StringBuilder"/> whose value is the same as this instance.</returns>
        public StringBuilder ToStringBuilder(bool includeFormatting)
        {
            if (!includeFormatting)
                return new StringBuilder().Append(_builder);

            static AnsiColor GetCurrentColor(IEnumerable<StyleRange> activeStyles, Func<StyleRange?, AnsiColor?> func)
                => func(activeStyles.LastOrDefault(x => func(x) is not null)) ?? AnsiColor.Default;

            static void UpdatePreStyles(IEnumerable<StyleRange> activeStyles, ref AnsiTextStyle? preStyles, AnsiStyle s)
            {
                if (!preStyles.HasValue && (s.AddedStyles != AnsiTextStyle.None || s.RemovedStyles != AnsiTextStyle.None))
                    preStyles = activeStyles.Aggregate(AnsiTextStyle.None, CombineStyles);
            }

            var pendingStyles = _styles.GroupBy(x => x.Start).ToDictionary(x => x.Key, x => x.ToArray());
            var activeStyles = new LinkedList<StyleRange>();
            var result = new StringBuilder(AnsiEscapeUtility.GetResetStyle());
            for (int i = 0; i < _builder.Length; i++)
            {
                AnsiTextStyle? preStyles = null;
                bool hasForegroundColorChanged = false;
                bool hasBackgroundColorChanged = false;

                // Remove old styles
                var currentNode = activeStyles.First;
                while (currentNode != null)
                {
                    var nextNode = currentNode.Next;
                    if (currentNode.Value.Start + currentNode.Value.Length <= i)
                    {
                        UpdatePreStyles(activeStyles, ref preStyles, currentNode.Value.Style);
                        if (currentNode.Value.Style.ForegroundColor is not null)
                            hasForegroundColorChanged = true;
                        if (currentNode.Value.Style.BackgroundColor is not null)
                            hasBackgroundColorChanged = true;
                        activeStyles.Remove(currentNode);
                    }

                    currentNode = nextNode;
                }

                // Add new styles
                if (pendingStyles.TryGetValue(i, out var newStyles))
                {
                    foreach (var s in newStyles)
                    {
                        UpdatePreStyles(activeStyles, ref preStyles, s.Style);
                        if (s.Style.ForegroundColor is not null)
                            hasForegroundColorChanged = true;
                        if (s.Style.BackgroundColor is not null)
                            hasBackgroundColorChanged = true;

                        var current = activeStyles.First;
                        while (current is not null)
                        {
                            if (current.Value.Id > s.Id)
                                break;
                            current = current.Next;
                        }

                        if (current is not null)
                            activeStyles.AddBefore(current, s);
                        else
                            activeStyles.AddLast(s);
                    }
                }

                if (preStyles.HasValue)
                {
                    var postStyles = activeStyles.Aggregate(AnsiTextStyle.None, CombineStyles);
                    var stylesToAdd = (preStyles.Value ^ AnsiTextStyle.All) & postStyles;
                    var stylesToRemove = (postStyles ^ AnsiTextStyle.All) & preStyles.Value;
                    if (stylesToRemove != AnsiTextStyle.None)
                        result.Append(AnsiEscapeUtility.GetRemoveStyle(stylesToRemove));
                    if (stylesToAdd != AnsiTextStyle.None)
                        result.Append(AnsiEscapeUtility.GetAddStyle(stylesToAdd));
                }

                if (hasForegroundColorChanged)
                    result.Append(GetCurrentColor(activeStyles, x => x?.Style.ForegroundColor).ForegroundSequence);
                if (hasBackgroundColorChanged)
                    result.Append(GetCurrentColor(activeStyles, x => x?.Style.BackgroundColor).BackgroundSequence);

                result.Append(_builder[i]);
            }

            return result.Append(AnsiEscapeUtility.GetResetStyle());
        }

        /// <summary>
        /// Converts the value of this instance to a <see cref="string"/>.
        /// </summary>
        /// <param name="includeFormatting">Determines wether to include ANSI control sequences int he resulting <see cref="string"/>.</param>
        /// <returns>A <see cref="string"/> whose value is the same as this instance.</returns>
        public string ToString(bool includeFormatting)
        {
            return includeFormatting ? ToStringBuilder(true).ToString() : _builder.ToString();
        }

        /// <summary>
        /// Converts the value of this instance to a <see cref="string"/>.
        /// </summary>
        /// <returns>A <see cref="string"/> whose value is the same as this instance.</returns>
        public override string ToString()
            => ToString(true);

        private static AnsiTextStyle CombineStyles(AnsiTextStyle current, StyleRange style)
        {
            return (current ^ (style.Style.RemovedStyles & current)) | style.Style.AddedStyles;
        }

        private void OnRemove(int startIndex, int length)
        {
            var currentNode = _styles.First;
            while (currentNode is not null)
            {
                var nextNode = currentNode.Next;
                var s = currentNode.Value;

                if (s.Start >= startIndex && s.Start + s.Length <= startIndex + length)
                {
                    _styles.Remove(currentNode);
                }
                else
                {
                    if (s.Start >= startIndex)
                    {
                        if (s.Start + s.Length == int.MaxValue)
                        {
                            s.Start -= Math.Min(length, s.Start - startIndex);
                            s.Length = int.MaxValue - s.Start;
                        }
                        else
                        {
                            s.Length -= Math.Max(0, startIndex + length - s.Start);
                            s.Start -= Math.Min(length, s.Start - startIndex);
                        }
                    }
                    else if (s.Start + s.Length > startIndex && s.Start + s.Length != int.MaxValue)
                    {
                        s.Length -= Math.Min(length, s.Start + s.Length - startIndex);
                    }
                }

                currentNode = nextNode;
            }
        }

        private AnsiFormattedString ApplyStyleImpl(int startIndex, int length, Action<AnsiStyle> styleAction)
        {
            var style = new AnsiStyle();
            styleAction(style);
            _styles.AddLast(new StyleRange(_nextStyleId++, startIndex, length, style));
            return this;
        }

        private AnsiFormattedString CallInsert<T>(int index, T value, Action<StringBuilder, int, T> func, Action<AnsiStyle>? styleAction)
        {
            using (new StyleScope(this, index, styleAction))
                func(_builder, index, value);
            return this;
        }

        private AnsiFormattedString CallInsert<T1, T2>(int index, T1 value1, T2 value2, Action<StringBuilder, int, T1, T2> func, Action<AnsiStyle>? styleAction)
        {
            using (new StyleScope(this, index, styleAction))
                func(_builder, index, value1, value2);
            return this;
        }

        private AnsiFormattedString CallInsert<T1, T2, T3>(int index, T1 value1, T2 value2, T3 value3, Action<StringBuilder, int, T1, T2, T3> func, Action<AnsiStyle>? styleAction)
        {
            using (new StyleScope(this, index, styleAction))
                func(_builder, index, value1, value2, value3);
            return this;
        }

        private void ValidateRange(int startIndex, int length)
        {
            Guard.NotOutOfRange(startIndex, nameof(startIndex), 0, Length - 1);
            Guard.NotOutOfRange(length, nameof(length), 0, Length - startIndex);
        }

        private class StyleRange
        {
            public StyleRange(int id, int start, int length, AnsiStyle style)
            {
                Id = id;
                Start = start;
                Length = length;
                Style = style;
            }

            public int Id { get; set; }
            public int Start { get; set; }
            public int Length { get; set; }
            public AnsiStyle Style { get; }
        }

        private sealed class StyleScope : IDisposable
        {
            private readonly AnsiFormattedString _formattedString;
            private readonly int _index;
            private readonly int _preLen;
            private readonly Action<AnsiStyle>? _styleAction;

            public StyleScope(AnsiFormattedString formattedString, int index, Action<AnsiStyle>? styleAction)
            {
                _formattedString = formattedString;
                _index = index;
                _preLen = _formattedString.Length;
                _styleAction = styleAction;
            }

            public void Dispose()
            {
                var addLen = _formattedString.Length - _preLen;

                foreach (var s in _formattedString._styles)
                {
                    if (s.Start >= _index && !(s.Start == _index && s.Start + s.Length == int.MaxValue))
                    {
                        s.Start += addLen;
                    }
                    else if (s.Start + s.Length > _index)
                    {
                        var maxLen = int.MaxValue - s.Start;
                        unchecked
                        {
                            s.Length = Math.Max(Math.Min(s.Length + addLen, maxLen), maxLen);
                        }
                    }
                }

                if (addLen > 0 && _styleAction is not null)
                    _formattedString.ApplyStyleImpl(_index, addLen, _styleAction);
            }
        }
    }
}
