using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace MaSch.Presentation.Wpf.Converter
{
    public class PathTrimmingConverter : IMultiValueConverter
    {
        /// <inheritdoc />
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var path = (string)values[0];
            var textBlock = (TextBlock)values[1];

            var segments = new Uri(path).Segments.Select(x => Uri.UnescapeDataString(x.TrimEnd('/'))).Where(x => !string.IsNullOrEmpty(x)).ToList();
            var leftSegments = segments.Take(segments.Count / 2).ToList();
            var rightSegments = segments.Skip(segments.Count / 2).ToList();

            double dpi = (PresentationSource.FromVisual(textBlock)?.CompositionTarget?.TransformToDevice.M11 ?? 1D) * 96D;
            bool widthOk;
            do
            {
                var formatted = new FormattedText(
                    string.Join(Path.DirectorySeparatorChar.ToString(), leftSegments.Concat(new[] { "..." }).Concat(rightSegments).ToArray()),
                    CultureInfo.CurrentCulture,
                    textBlock.FlowDirection,
                    textBlock.FontFamily.GetTypefaces().First(),
                    textBlock.FontSize,
                    textBlock.Foreground, 
                    new NumberSubstitution(), 
                    TextFormattingMode.Display, dpi
                );

                widthOk = formatted.Width < textBlock.ActualWidth;

                if (!widthOk)
                {
                    if (leftSegments.Count <= rightSegments.Count)
                        rightSegments.RemoveAt(0);
                    else
                        leftSegments.RemoveAt(leftSegments.Count - 1);

                    if (leftSegments.Count + rightSegments.Count == 0)
                        return "..." + Path.DirectorySeparatorChar + segments.Last();
                }

            } while (!widthOk);

            IEnumerable<string> s;
            if (leftSegments.Count + rightSegments.Count == segments.Count)
                s = leftSegments.Concat(rightSegments);
            else
                s = leftSegments.Concat(new[] {"..."}).Concat(rightSegments);

            return string.Join(Path.DirectorySeparatorChar.ToString(), s);
        }

        /// <inheritdoc />
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
