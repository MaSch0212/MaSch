using Avalonia.Data.Converters;

namespace MaSch.Presentation.Avalonia.Converter;

/// <summary>
/// A <see cref="IValueConverter"/> that negates a boolean value.
/// </summary>
/// <seealso cref="IValueConverter" />
public class BoolNegationConverter : IValueConverter
{
    /// <inheritdoc />
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Equals(value, false);
    }

    /// <inheritdoc />
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Equals(value, false);
    }
}
