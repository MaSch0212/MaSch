using System;
using System.Windows;
using System.Windows.Data;

namespace MaSch.Presentation.Wpf.Converter
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        public bool UseCollapse { get; set; } = true;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var b = value as bool?;
            if (!b.HasValue)
                b = true;
            if (parameter != null && parameter.ToString().Equals("negate", StringComparison.InvariantCultureIgnoreCase))
                b = !b.Value;
            return b.Value ? Visibility.Visible : (UseCollapse ? Visibility.Collapsed : Visibility.Hidden);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool n = parameter != null && parameter.ToString().Equals("negate", StringComparison.InvariantCultureIgnoreCase);
            return value as Visibility? == Visibility.Visible ? !n : n;
        }
    }
}
