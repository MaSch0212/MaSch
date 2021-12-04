using System.ComponentModel;

namespace MaSch.Core.Extensions;

/// <summary>
/// Contains extensions for <see cref="Enum"/>.
/// </summary>
public static class EnumerationExtensions
{
    /// <summary>
    /// Determines the value of the <see cref="DescriptionAttribute"/> of the <see cref="Enum"/> value.
    /// </summary>
    /// <param name="enumValue">The <see cref="Enum"/> value.</param>
    /// <returns>Return the Description of the <see cref="Enum"/> value if the attribute <see cref="DescriptionAttribute"/> is set; otherwise <see langword="null"/>.</returns>
    public static string? GetDescription(this Enum enumValue)
    {
        return Guard.NotNull(enumValue, nameof(enumValue))
                       .GetType()
                       .GetField(enumValue.ToString())?
                       .GetCustomAttribute<DescriptionAttribute>()?
                       .Description;
    }
}
