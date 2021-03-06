using System;
using System.Windows.Data;
using System.Windows.Media;

namespace MaSch.Presentation.Wpf.Converter
{
    /// <summary>
    /// A <see cref="IValueConverter"/> that converts a <see cref="SolidColorBrush"/> into a <see cref="Color"/>.
    /// </summary>
    /// <seealso cref="IValueConverter" />
    [ValueConversion(typeof(SolidColorBrush), typeof(Color))]
    public class SolidColorBrushToColorConverter : IValueConverter
    {
        /// <inheritdoc />
        public object? Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) => (value as SolidColorBrush)?.Color;

        /// <inheritdoc />
        public object? ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value is Color color ? new SolidColorBrush(color) : null;
        }
    }
}
