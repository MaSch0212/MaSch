using MaSch.Core.Extensions;
using System;
using System.Globalization;
using System.Windows.Data;

namespace MaSch.Presentation.Wpf.Converter
{
    /// <summary>
    /// A <see cref="IValueConverter"/> that Converts a number of bytes in a human readable file size string (e.g. 1024 => "1 KB").
    /// </summary>
    /// <seealso cref="IValueConverter" />
    [ValueConversion(typeof(double), typeof(string))]
    public class FileSizeConverter : IValueConverter
    {
        private static readonly string[] Measures = { string.Empty, "K", "M", "G", "T", "P", "E", "Z", "Y" };

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (double.TryParse(value?.ToString(), out double size) && size >= 0)
            {
                int m = 0;
                while (size > 1024)
                {
                    m++;
                    size /= 1024;
                }

                return m == 0 ? $"{size:N0} B" : $"{size:N2} {Measures[m]}B";
            }

            return string.Empty;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = value?.ToString();
            if (s != null)
            {
                var c = s[^2].ToString();
                if (double.TryParse(s.Split(' ')[0], out double d))
                {
                    var i = Measures.IndexOf(c);
                    if (i > 0)
                        d *= Math.Pow(1024, i);
                    return (long)d;
                }
            }

            return 0L;
        }
    }
}
