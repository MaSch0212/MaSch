using Microsoft.CodeAnalysis.CSharp;

namespace MaSch.CodeAnalysis.CSharp.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="string"/> class.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Converts this string to a C# string literal.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="quote">True to put (double) quotes around the string literal.</param>
    /// <returns>A string literal with the given value.</returns>
    /// <remarks>Escapes non-printable characters.</remarks>
    public static string ToCSharpLiteral(this string? value, bool quote = true)
    {
        return SymbolDisplay.FormatPrimitive(value, quote, false);
    }

    /// <summary>
    /// Converts the string to <see cref="CaseStyle.PascalCase"/>.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns>The value converted to <see cref="CaseStyle.PascalCase"/>.</returns>
    public static string? ToPascalCase(this string? value)
    {
        return Format(value, static (c, i) => char.ToUpperInvariant(c), static (c, i) => c, null);
    }

    /// <summary>
    /// Converts the string to <see cref="CaseStyle.CamelCase"/>.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns>The value converted to <see cref="CaseStyle.CamelCase"/>.</returns>
    public static string? ToCamelCase(this string? value)
    {
        return Format(value, static (c, i) => i == 0 ? char.ToLowerInvariant(c) : char.ToUpperInvariant(c), static (c, i) => c, null);
    }

    /// <summary>
    /// Converts the string to <see cref="CaseStyle.KebabCase"/>.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns>The value converted to <see cref="CaseStyle.KebabCase"/>.</returns>
    public static string? ToKebabCase(this string? value)
    {
        return Format(value, static (c, i) => char.ToLowerInvariant(c), static (c, i) => c, "-");
    }

    /// <summary>
    /// Converts the string to <see cref="CaseStyle.SnakeCase"/>.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns>The value converted to <see cref="CaseStyle.SnakeCase"/>.</returns>
    public static string? ToSnakeCase(this string? value)
    {
        return Format(value, static (c, i) => char.ToLowerInvariant(c), static (c, i) => c, "_");
    }

    /// <summary>
    /// Converts the string to <see cref="CaseStyle.UpperSnakeCase"/>.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns>The value converted to <see cref="CaseStyle.UpperSnakeCase"/>.</returns>
    public static string? ToUpperSnakeCase(this string? value)
    {
        return Format(value, static (c, i) => char.ToUpperInvariant(c), static (c, i) => char.ToUpperInvariant(c), "_");
    }

    /// <summary>
    /// Convert the string to a given <see cref="CaseStyle"/>.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <param name="caseStyle">The <see cref="CaseStyle"/> to use.</param>
    /// <returns>The value converted to the given <paramref name="caseStyle"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The case style <paramref name="caseStyle"/> is unknown.</exception>
    public static string? ToCase(this string? value, CaseStyle caseStyle)
    {
        return caseStyle switch
        {
            CaseStyle.PascalCase => ToPascalCase(value),
            CaseStyle.CamelCase => ToCamelCase(value),
            CaseStyle.KebabCase => ToKebabCase(value),
            CaseStyle.SnakeCase => ToSnakeCase(value),
            CaseStyle.UpperSnakeCase => ToUpperSnakeCase(value),
            _ => throw new ArgumentOutOfRangeException(nameof(caseStyle), $"The case style {caseStyle} is unknown."),
        };
    }

    private static string? Format(string? value, Func<char, int, char> leadingCharConversion, Func<char, int, char> followingCharConversion, string? separator)
    {
        if (value is null or "")
            return value;

        var result = new StringBuilder(value.Length);

        bool hasLeadingChar = false;
        for (int i = 0; i < value.Length; i++)
        {
            var c = value[i];
            if (result.Length == 0 ? !char.IsLetter(c) : !char.IsLetterOrDigit(c))
            {
                hasLeadingChar = false;
                continue;
            }

            if (char.IsUpper(c))
                hasLeadingChar = false;

            if (hasLeadingChar)
            {
                result.Append(followingCharConversion(c, result.Length));
            }
            else
            {
                if (result.Length > 0 && separator is not null)
                    result.Append(separator);
                result.Append(leadingCharConversion(c, result.Length));
            }

            hasLeadingChar = true;
        }

        return result.ToString();
    }
}
