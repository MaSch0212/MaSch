using System;
using System.Linq;
using System.Windows.Data;

namespace MaSch.Presentation.Wpf.Converter
{
    /// <inheritdoc />
    /// <summary>
    /// Converter for convertion from byte size to string.
    /// </summary>
    public class ByteSizeToStringConverter : IValueConverter
    {
        private static readonly string[] Suffixes = {
            "Byte", "KB", "MB", "GB", "TB"
        };

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var (bytes, suffix) = Format(System.Convert.ToDouble(value));
            return $"{bytes:0.00} {suffix}";
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var s = System.Convert.ToString(value).Split(' ');
            return double.Parse(s[0]) * Suffixes.ToList().IndexOf(s[1]);
        }

        public static (double value, string suffix) Format(double value)
        {
            int i = 0;
            while (value > 1000 && i < Suffixes.Length)
            {
                value /= 1024D;
                i++;
            }
            return (value, Suffixes[i]);
        }
    }
}
