using System.Windows.Data;
using System.Windows.Media;

namespace MaSch.Presentation.Wpf.Converter;

/// <summary>
/// A <see cref="IValueConverter"/> that applies an alpha transparency value to a specified color.
/// </summary>
/// <seealso cref="IValueConverter" />
/// <seealso cref="IMultiValueConverter" />
public class ColorTransparencyConverter : IValueConverter, IMultiValueConverter
{
    /// <inheritdoc />
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        Color? color = null;
        if (value is SolidColorBrush solidColorBrush)
            color = Color.FromArgb((byte)(solidColorBrush.Opacity * solidColorBrush.Color.A), solidColorBrush.Color.R, solidColorBrush.Color.G, solidColorBrush.Color.B);
        if (value is Color c)
            color = c;

        if (color.HasValue)
        {
            if (ObjectExtensions.ConvertManager.TryConvert<double>(parameter, CultureInfo.InvariantCulture, out var alpha))
                return Color.FromArgb((byte)(alpha > 0 && alpha <= 1 ? alpha * 255 : alpha), color.Value.R, color.Value.G, color.Value.B);
            else
                return color;
        }

        return value;
    }

    /// <inheritdoc />
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length == 2)
            return Convert(values[0], targetType, values[1], culture);
        throw new InvalidOperationException("This MultiValueConverter needs exactly two bindings. First the color to change, second the new alpha value.");
    }

    /// <inheritdoc />
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException("The transparency of a color cannot be restored!");
    }

    /// <inheritdoc />
    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException("The transparency of a color cannot be restored!");
    }
}
