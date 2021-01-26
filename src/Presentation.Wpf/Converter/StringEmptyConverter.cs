using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MaSch.Presentation.Wpf.Converter
{
    public class StringEmptyConverter : IValueConverter
    {
        public bool AllowWhitespace { get; set; } = true;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return true;
            if(value is string s)
                return AllowWhitespace ? string.IsNullOrEmpty(s) : string.IsNullOrWhiteSpace(s);
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
