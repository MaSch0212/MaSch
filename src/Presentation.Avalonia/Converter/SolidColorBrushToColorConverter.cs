using Avalonia.Data.Converters;
using Avalonia.Media;

namespace MaSch.Presentation.Avalonia.Converter;

/// <summary>
/// A <see cref="IValueConverter"/> that converts a <see cref="SolidColorBrush"/> into a <see cref="Color"/>.
/// </summary>
/// <seealso cref="IValueConverter" />
public class SolidColorBrushToColorConverter : IValueConverter
{
    /// <inheritdoc />
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return (value as SolidColorBrush)?.Color;
    }

    /// <inheritdoc />
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is Color color ? new SolidColorBrush(color) : null;
    }
}
