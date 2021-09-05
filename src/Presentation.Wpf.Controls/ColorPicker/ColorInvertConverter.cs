using System;
using System.Windows.Data;
using System.Windows.Media;

namespace MaSch.Presentation.Wpf.ColorPicker
{
    /// <summary>
    /// A <see cref="IValueConverter"/> that inverts a color.
    /// </summary>
    /// <seealso cref="IValueConverter" />
    internal class ColorInvertConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object? Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return InvertColor(value as Color?);
        }

        /// <inheritdoc/>
        public object? ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return InvertColor(value as Color?);
        }

        private static Color? InvertColor(Color? c)
        {
            return c.HasValue ? (Color?)Color.FromArgb(255, (byte)(255 - c.Value.R), (byte)(255 - c.Value.G), (byte)(255 - c.Value.B)) : null;
        }
    }
}
