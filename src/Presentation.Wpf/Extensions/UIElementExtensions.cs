using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MaSch.Presentation.Wpf.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="UIElement"/> class.
/// </summary>
public static class UIElementExtensions
{
    /// <summary>
    /// Gets the parent of this <see cref="DependencyObject"/>.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns>The parent of the specified <see cref="DependencyObject"/>.</returns>
    public static IEnumerable<DependencyObject> Parents(this DependencyObject element)
    {
        DependencyObject current = element;
        do
        {
            current = VisualTreeHelper.GetParent(current);
            if (current != null)
                yield return current;
        }
        while (current != null);
    }

    /// <summary>
    /// Renders the control into a bitmap.
    /// </summary>
    /// <param name="control">The control to render.</param>
    /// <returns>A bitmap showing a screenshot of the specified control.</returns>
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
