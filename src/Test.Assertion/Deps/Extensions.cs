namespace MaSch.Test.Assertion;

internal static class Extensions
{
#if !NETCOREAPP2_1_OR_GREATER && !NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Returns a value indicating whether a specified string occurs within this string, using the specified comparison rules.
    /// </summary>
    /// <param name="s">The string to check.</param>
    /// <param name="value">The string to seek.</param>
    /// <param name="comparisonType"> One of the enumeration values that specifies the rules to use in the comparison.</param>
    /// <returns>
    ///   <c>true</c> if the value parameter occurs within this string, or if value is the empty string (""); otherwise, <c>false</c>.
    /// </returns>
    public static bool Contains(this string s, string value, StringComparison comparisonType)
    {
        return s.IndexOf(value, comparisonType) >= 0;
    }
#endif
}
