using System.Windows;

namespace MaSch.Presentation.Wpf.DependencyProperties
{
    /// <summary>
    /// Provides dependency properties for the <see cref="System.Windows.Controls.Expander"/> control.
    /// </summary>
    public static class Expander
    {
        /// <summary>
        /// Dependency property. Gets or sets the padding for the header.
        /// </summary>
        public static readonly DependencyProperty HeaderPaddingProperty =
            DependencyProperty.RegisterAttached(
                "HeaderPadding",
                typeof(Thickness),
                typeof(Expander),
                new PropertyMetadata(new Thickness(0)));

        /// <summary>
        /// Sets the value of the <see cref="HeaderPaddingProperty"/>.
        /// </summary>
        /// <param name="target">The element to set the value to.</param>
        /// <param name="value">The value to set.</param>
        public static void SetHeaderPadding(DependencyObject target, Thickness value)
        {
            target.SetValue(HeaderPaddingProperty, value);
        }

        /// <summary>
        /// Gets the value of the <see cref="HeaderPaddingProperty"/>.
        /// </summary>
        /// <param name="target">The element to get the value from.</param>
        /// <returns>The value of the <see cref="HeaderPaddingProperty"/>.</returns>
        public static Thickness GetHeaderPadding(DependencyObject target)
        {
            return (Thickness)target.GetValue(HeaderPaddingProperty);
        }
    }
}
