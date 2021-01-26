using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;

namespace MaSch.Common.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="string"/>.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Returns the value of the <see cref="string.Contains(string)"/>-Method. If the string is null it returns <see cref="string.IsNullOrEmpty(string)"/> for the value parameter
        /// </summary>
        /// <param name="s1">The string to check on.</param>
        /// <param name="value">The string to seek.</param>
        /// <param name="ignoreCase">Determines if the case should be ignored.</param>
        /// <returns></returns>
        public static bool ContainsEx(this string? s1, string? value, bool ignoreCase = false)
        {
            var s2 = ignoreCase ? s1?.ToUpper() : s1;
            var v2 = ignoreCase ? value?.ToUpper() : value;
            return v2 == null || (s2?.Contains(v2) ?? string.IsNullOrEmpty(value));
        }

        /// <summary>
        /// Removes all the leading occurrences of a specified string from the current string.
        /// </summary>
        /// <param name="s">The string to trim.</param>
        /// <param name="trimString">The Unicode string to remove.</param>
        /// <returns>
        ///     The string that remains after all occurrences of the <paramref name="trimString"/> string are removed from the start of the current string. 
        ///     If the <paramref name="trimString"/> is not found at the start of the current instance, the method returns the current instance unchanged.
        /// </returns>
        [return: NotNullIfNotNull("s")]
        public static string? TrimStart(this string? s, string trimString)
            => TrimStartImpl(s, trimString, false);

        /// <summary>
        /// Removes the first occurrence of a specified string from the current string.
        /// </summary>
        /// <param name="s">The string to trim.</param>
        /// <param name="trimString">The Unicode string to remove.</param>
        /// <returns>
        ///     The string that remains after the first occurrence of the <paramref name="trimString"/> string is removed from the start of the current string. 
        ///     If the <paramref name="trimString"/> is not found at the start of the current instance, the method returns the current instance unchanged.
        /// </returns>
        [return: NotNullIfNotNull("s")]
        public static string? TrimStartOnce(this string? s, string trimString)
            => TrimStartImpl(s, trimString, true);

        private static string? TrimStartImpl(string? s, string trimString, bool onlyOnce)
            => s == null ? s : Regex.Replace(s, $"^({Regex.Escape(trimString)}){(onlyOnce ? "" : "+")}", "");

        /// <summary>
        /// Removes all the trailing occurrences of a specified string from the current string.
        /// </summary>
        /// <param name="s">The string to trim.</param>
        /// <param name="trimString">The Unicode string to remove.</param>
        /// <returns>
        ///     The string that remains after all occurrences of the <paramref name="trimString"/> string are removed from the end of the current string. 
        ///     If the <paramref name="trimString"/> is not found at the end of the current instance, the method returns the current instance unchanged.
        /// </returns>
        [return: NotNullIfNotNull("s")]
        public static string? TrimEnd(string? s, string trimString)
            => TrimEndImpl(s, trimString, false);

        /// <summary>
        /// Removes the first trailing occurrence of a specified string from the current string.
        /// </summary>
        /// <param name="s">The string to trim.</param>
        /// <param name="trimString">The Unicode string to remove.</param>
        /// <returns>
        ///     The string that remains after the first occurrence of the <paramref name="trimString"/> string is removed from the end of the current string. 
        ///     If the <paramref name="trimString"/> is not found at the end of the current instance, the method returns the current instance unchanged.
        /// </returns>
        [return: NotNullIfNotNull("s")]
        public static string? TrimEndOnce(string? s, string trimString)
            => TrimEndImpl(s, trimString, true);

        private static string? TrimEndImpl(string? s, string trimString, bool onlyOnce)
            => s == null ? s : Regex.Replace(s, $"({Regex.Escape(trimString)}){(onlyOnce ? "" : "+")}$", "");

        /// <summary>
        /// Removes all leading and trailing instances of a specified string from the current string.
        /// </summary>
        /// <param name="s">The string to trim.</param>
        /// <param name="trimString">The Unicode string to remove.</param>
        /// <returns>
        ///     The string that remains after all occurrences of the <paramref name="trimString"/> string are removed from the start and end of the current string. 
        ///     If the <paramref name="trimString"/> is not found at the start and end of the current instance, the method returns the current instance unchanged.
        /// </returns>
        [return: NotNullIfNotNull("s")]
        public static string? Trim(string? s, string trimString)
            => TrimImpl(s, trimString, false);

        /// <summary>
        /// Removes the first leading and trailing instances of a specified string from the current string.
        /// </summary>
        /// <param name="s">The string to trim.</param>
        /// <param name="trimString">The Unicode string to remove.</param>
        /// <returns>
        ///     The string that remains after the first occurrence of the <paramref name="trimString"/> string is removed from the start and end of the current string. 
        ///     If the <paramref name="trimString"/> is not found at the start and end of the current instance, the method returns the current instance unchanged.
        /// </returns>
        [return: NotNullIfNotNull("s")]
        public static string? TrimOnce(string? s, string trimString)
            => TrimImpl(s, trimString, true);

        private static string? TrimImpl(string? s, string trimString, bool onlyOnce)
        {
            var ets = Regex.Escape(trimString);
            var p = onlyOnce ? "" : "+";
            return s == null ? s : Regex.Replace(s, $"(^({ets}){p}|({ets}){p}$)", "");
        }

        /// <summary>
        /// Ensures that the current string ends with a specified string.
        /// </summary>
        /// <param name="s">The string to check.</param>
        /// <param name="end">The expected end.</param>
        /// <returns>The string with the correct ending string. If the current string already ended with <paramref name="end"/>, the method returns the current instance unchanged.</returns>
        [return: NotNullIfNotNull("s")]
        public static string? EnsureEndsWith(this string? s, string end) 
            => s?.EndsWith(end) == true ? s : s + end;

        /// <summary>
        /// Returns a new string that center-aligns the characters in this instance by padding them on the left and right with a specified Unicode character, for a specified total length.
        /// </summary>
        /// <param name="s">The string to pad.</param>
        /// <param name="totalWidth">The number of characters in the resulting string, equal to the number of original characters plus any additional padding characters.</param>
        /// <param name="paddingChar">A Unicode padding character.</param>
        /// <returns>
        ///     A new string that is equivalent to this instance, but center-aligned and padded on the left and right with as many <paramref name="paddingChar"/> characters as needed to create a length
        ///     of <paramref name="totalWidth"/>. However, if <paramref name="totalWidth"/> is less than the length of this instance, the method returns a reference to the existing instance. If <paramref name="totalWidth"/> is equal
        ///     to the length of this instance, the method returns a new string that is identical to this instance.
        /// </returns>
        public static string PadBoth(this string s, int totalWidth, char paddingChar)
        {
            int padLeft = (totalWidth - s.Length) / 2 + s.Length;
            return s.PadLeft(padLeft, paddingChar).PadRight(totalWidth, paddingChar);
        }

        public static string Indent(this string s, int indentation)
            => Indent(s, indentation, true);
        public static string Indent(this string s, int indentation, bool indentFirstLine)
            => (indentFirstLine ? new string(' ', indentation) : string.Empty) + s.Replace("\r", "").Replace("\n", $"{Environment.NewLine}{new string(' ', indentation)}");
    }
}
