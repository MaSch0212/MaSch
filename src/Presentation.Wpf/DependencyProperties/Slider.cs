using System.Windows;
using System.Windows.Media;

namespace MaSch.Presentation.Wpf.DependencyProperties
{
    /// <summary>
    /// Provides dependency properties for the <see cref="System.Windows.Controls.Slider"/> control.
    /// </summary>
    public static class Slider
    {
        /// <summary>
        /// Dependency property. Gets or sets the brush of the line that is shown on the left side of the grip.
        /// </summary>
        public static readonly DependencyProperty LowerLineBrushProperty =
            DependencyProperty.RegisterAttached(
                "LowerLineBrush",
                typeof(Brush),
                typeof(Slider),
                new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        /// <summary>
        /// Dependency property. Gets or sets the brush of the line that is shown on the right side of the grip.
        /// </summary>
        public static readonly DependencyProperty GreaterLineBrushProperty =
            DependencyProperty.RegisterAttached(
                "GreaterLineBrush",
                typeof(Brush),
                typeof(Slider),
                new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        /// <summary>
        /// Sets the value of the <see cref="LowerLineBrushProperty"/>.
        /// </summary>
        /// <param name="target">The element to set the value to.</param>
        /// <param name="value">The value to set.</param>
        public static void SetLowerLineBrush(DependencyObject target, Brush value)
        {
            target.SetValue(LowerLineBrushProperty, value);
        }

        /// <summary>
        /// Gets the value of the <see cref="LowerLineBrushProperty"/>.
        /// </summary>
        /// <param name="target">The element to get the value from.</param>
        /// <returns>The value of the <see cref="LowerLineBrushProperty"/>.</returns>
        public static Brush GetLowerLineBrush(DependencyObject target)
        {
            return (Brush)target.GetValue(LowerLineBrushProperty);
        }

        /// <summary>
        /// Sets the value of the <see cref="GreaterLineBrushProperty"/>.
        /// </summary>
        /// <param name="target">The element to set the value to.</param>
        /// <param name="value">The value to set.</param>
        public static void SetGreaterLineBrush(DependencyObject target, Brush value)
        {
            target.SetValue(GreaterLineBrushProperty, value);
        }

        /// <summary>
        /// Gets the value of the <see cref="GreaterLineBrushProperty"/>.
        /// </summary>
        /// <param name="target">The element to get the value from.</param>
        /// <returns>The value of the <see cref="GreaterLineBrushProperty"/>.</returns>
        public static Brush GetGreaterLineBrush(DependencyObject target)
        {
            return (Brush)target.GetValue(GreaterLineBrushProperty);
        }
    }
}
