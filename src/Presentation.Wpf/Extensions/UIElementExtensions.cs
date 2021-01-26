using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MaSch.Presentation.Wpf.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class UIElementExtensions
    {
        public static IEnumerable<DependencyObject> Parents(this DependencyObject element)
        {
            DependencyObject current = element;
            do
            {
                current = VisualTreeHelper.GetParent(current);
                if (current != null)
                    yield return current;
            } while (current != null);
        }

        public static BitmapSource RenderToBitmap(this UIElement control)
        {
            control.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            control.Arrange(new Rect(control.DesiredSize));
            control.UpdateLayout();
            var bmp = new RenderTargetBitmap((int)Math.Ceiling(control.DesiredSize.Width), (int)Math.Ceiling(control.DesiredSize.Height), 96, 96, PixelFormats.Pbgra32);
            bmp.Render(control);
            return bmp;
        }
    }
}
