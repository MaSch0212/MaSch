using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MaSch.Presentation.Wpf.Converter
{
    /// <summary>
    /// A <see cref="IValueConverter"/> that check if a string is empty.
    /// </summary>
    /// <seealso cref="IValueConverter" />
    public class StringEmptyConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets a value indicating whether whitespaces are allowed. If set to <c>true</c> the string needs to be empty so this <see cref="StringEmptyConverter"/> returns <c>true</c>.
        /// </summary>
        public bool AllowWhitespace { get; set; } = true;

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return true;
            if (value is string s)
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
