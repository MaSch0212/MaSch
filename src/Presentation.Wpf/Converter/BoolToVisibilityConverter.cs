using System;
using System.Windows;
using System.Windows.Data;

namespace MaSch.Presentation.Wpf.Converter
{
    /// <summary>
    /// A <see cref="IValueConverter"/> that converts boolean to <see cref="Visibility"/>.
    /// <c>true</c> is converted to <see cref="Visibility.Visible"/> and <c>false</c> to <see cref="Visibility.Hidden"/> or <see cref="Visibility.Collapsed"/>.
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets a value indicating whether <c>false</c> should be converted to <see cref="Visibility.Collapsed"/> instead of <see cref="Visibility.Hidden"/>.
        /// </summary>
        public bool UseCollapse { get; set; } = true;

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var b = value as bool?;
            if (!b.HasValue)
                b = true;
            if (parameter != null && parameter.ToString().Equals("negate", StringComparison.InvariantCultureIgnoreCase))
                b = !b.Value;
            return b.Value ? Visibility.Visible : (UseCollapse ? Visibility.Collapsed : Visibility.Hidden);
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool n = parameter != null && parameter.ToString().Equals("negate", StringComparison.InvariantCultureIgnoreCase);
            return value as Visibility? == Visibility.Visible ? !n : n;
        }
    }
}
