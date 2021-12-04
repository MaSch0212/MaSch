using System.Windows.Data;

namespace MaSch.Presentation.Wpf.Converter;

/// <summary>
/// A <see cref="IValueConverter"/> that negates a boolean value.
/// </summary>
/// <seealso cref="IValueConverter" />
[ValueConversion(typeof(bool), typeof(bool))]
public class BoolNegationConverter : IValueConverter
{
    /// <inheritdoc />
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool?)value == false;
    }

    /// <inheritdoc />
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool?)value == false;
    }
}
