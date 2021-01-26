using MaSch.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace MaSch.Presentation.Wpf.Converter
{
    public class ColorCombineConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var colors = values?.OfType<Color>().ToArray();
            if (colors.IsNullOrEmpty())
                return Colors.Transparent;

            return colors.Aggregate(MixColors);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Combined colors cannot be converted back!");
        }

        public static Color MixColors(Color backgroundColor, Color foregroundColor)
        {
            return Color.FromArgb(
                getMixedAlpha(backgroundColor.A, foregroundColor.A),
                getMixedColor(backgroundColor.R, backgroundColor.A, foregroundColor.R, foregroundColor.A),
                getMixedColor(backgroundColor.G, backgroundColor.A, foregroundColor.G, foregroundColor.A),
                getMixedColor(backgroundColor.B, backgroundColor.A, foregroundColor.B, foregroundColor.A));

            byte getMixedColor(byte backColor, byte backAlpha, byte foreColor, byte foreAlpha)
            {
                var p1 = foreAlpha / 255F;
                var p2 = backAlpha / 255F;
                var c1 = foreColor;
                var c2 = backColor;
                return (byte)Math.Min(255, Math.Floor((p1 * c1 + p2 * c2 - p1 * p2 * c2) / (p1 + p2 - p1 * p2)));
            }

            byte getMixedAlpha(byte backAlpha, byte foreAlpha)
            {
                var p1 = foreAlpha / 255F;
                var p2 = backAlpha / 255F;
                return (byte)Math.Min(255, Math.Round((p1 + p2 - p1 * p2) * 255, 0));
            }
        }
    }
}
