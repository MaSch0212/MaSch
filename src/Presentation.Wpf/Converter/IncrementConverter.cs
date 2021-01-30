using System;
using System.Globalization;
using System.Windows.Data;

namespace MaSch.Presentation.Wpf.Converter
{
    [ValueConversion(typeof(int), typeof(int))]
    public class IncrementConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!int.TryParse(parameter?.ToString(), out int p))
                p = 1;
            if (int.TryParse(value?.ToString(), out int i))
                return i + p;
            return 0;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!int.TryParse(parameter?.ToString(), out int p))
                p = 1;
            if (int.TryParse(value?.ToString(), out int i))
                return i - p;
            return 0;
        }
    }
}
