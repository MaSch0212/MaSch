using System.Windows;
using System.Windows.Data;

namespace MaSch.Presentation.Wpf.Converter;

/// <summary>
/// A <see cref="IValueConverter"/> that converts numbers to <see cref="GridLength"/> and back.
/// </summary>
/// <seealso cref="IValueConverter" />
public class GridLengthConverter : IValueConverter
{
    /// <inheritdoc />
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        double val = value.ConvertTo<double>();
        GridLength gridLength = new(val);

        return gridLength;
    }

    /// <inheritdoc />
    public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        GridLength val = (GridLength)value;

        return val.Value.ConvertTo(targetType);
    }
}
