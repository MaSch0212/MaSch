using System;
using System.Globalization;
using System.Windows.Data;

namespace MaSch.Presentation.Wpf.Converter
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class BoolNegationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool?)value == false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool?)value == false;
        }
    }
}
