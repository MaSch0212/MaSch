using Avalonia.Data.Converters;

namespace MaSch.Presentation.Avalonia.Converter;

/// <summary>
/// A <see cref="IValueConverter"/> that transforms a string.
/// </summary>
/// <seealso cref="IValueConverter" />
public class StringTransformationConverter : IValueConverter
{
    /// <summary>
    /// Gets or sets a value indicating whether the string should be converted to upper case characters.
    /// </summary>
    public bool ToUpper { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the string should be converted to lower case characters.
    /// </summary>
    public bool ToLower { get; set; }

    /// <inheritdoc />
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
            return null;
        if (ToUpper)
            return value.ToString()?.ToUpper();
        if (ToLower)
            return value.ToString()?.ToLower();
        return value.ToString();
    }

    /// <inheritdoc />
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
