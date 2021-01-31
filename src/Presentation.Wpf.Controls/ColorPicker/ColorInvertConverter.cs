using System;
using System.Windows.Data;
using System.Windows.Media;

namespace MaSch.Presentation.Wpf.ColorPicker
{
    internal class ColorInvertConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            => InvertColor(value as Color?);

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            => InvertColor(value as Color?);

        public Color? InvertColor(Color? c)
            => c.HasValue ? (Color?)Color.FromArgb(255, (byte)(255 - c.Value.R), (byte)(255 - c.Value.G), (byte)(255 - c.Value.B)) : null;
    }
}
