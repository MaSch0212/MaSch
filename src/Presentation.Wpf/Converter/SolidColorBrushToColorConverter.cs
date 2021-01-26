using System;
using System.Windows.Data;
using System.Windows.Media;

namespace MaSch.Presentation.Wpf.Converter
{
    [ValueConversion(typeof(SolidColorBrush), typeof(Color))]
    public class SolidColorBrushToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) => (value as SolidColorBrush)?.Color;

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value is Color color ? new SolidColorBrush(color) : null;
        }
    }
}
