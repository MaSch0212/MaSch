using Avalonia.Data.Converters;
using Avalonia.Media;
using MaSch.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MaSch.Presentation.Avalonia.Converter
{
    /// <summary>
    /// A <see cref="IMultiValueConverter"/> that combines multiple colors together by layering them over each other.
    /// </summary>
    /// <seealso cref="IMultiValueConverter" />
    public class ColorCombineConverter : IMultiValueConverter
    {
        /// <summary>
        /// Mixes the two specified colors.
        /// </summary>
        /// <param name="backgroundColor">Color of the background.</param>
        /// <param name="foregroundColor">Color of the foreground.</param>
        /// <returns>A new <see cref="Color"/> object that represents the mix between <paramref name="backgroundColor"/> and <paramref name="foregroundColor"/>.</returns>
        public static Color MixColors(Color backgroundColor, Color foregroundColor)
        {
            return Color.FromArgb(
                GetMixedAlpha(backgroundColor.A, foregroundColor.A),
                GetMixedColor(backgroundColor.R, backgroundColor.A, foregroundColor.R, foregroundColor.A),
                GetMixedColor(backgroundColor.G, backgroundColor.A, foregroundColor.G, foregroundColor.A),
                GetMixedColor(backgroundColor.B, backgroundColor.A, foregroundColor.B, foregroundColor.A));

            byte GetMixedColor(byte backColor, byte backAlpha, byte foreColor, byte foreAlpha)
            {
                var p1 = foreAlpha / 255F;
                var p2 = backAlpha / 255F;
                var c1 = foreColor;
                var c2 = backColor;
                return (byte)Math.Min(255, Math.Floor(((p1 * c1) + (p2 * c2) - (p1 * p2 * c2)) / (p1 + p2 - (p1 * p2))));
            }

            byte GetMixedAlpha(byte backAlpha, byte foreAlpha)
            {
                var p1 = foreAlpha / 255F;
                var p2 = backAlpha / 255F;
                return (byte)Math.Min(255, Math.Round((p1 + p2 - (p1 * p2)) * 255, 0));
            }
        }

        /// <inheritdoc />
        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            var colors = values?.OfType<Color>().ToArray();
            if (colors.IsNullOrEmpty())
                return Colors.Transparent;

            return colors.Aggregate(MixColors);
        }
    }
}
