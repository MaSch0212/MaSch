using System;
using System.Globalization;
using System.Windows.Data;

namespace MaSch.Presentation.Wpf.Converter
{
    public class StringTransformationConverter : IValueConverter
    {
        public bool ToUpper { get; set; }
        public bool ToLower { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            if (ToUpper)
                return value.ToString().ToUpper();
            if (ToLower)
                return value.ToString().ToLower();
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
