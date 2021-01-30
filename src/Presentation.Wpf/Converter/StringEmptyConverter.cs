using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MaSch.Presentation.Wpf.Converter
{
    public class StringEmptyConverter : IValueConverter
    {
        public bool AllowWhitespace { get; set; } = true;

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return true;
            if(value is string s)
                return AllowWhitespace ? string.IsNullOrEmpty(s) : string.IsNullOrWhiteSpace(s);
            return DependencyProperty.UnsetValue;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
