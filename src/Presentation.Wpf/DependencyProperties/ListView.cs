using System.Windows;

namespace MaSch.Presentation.Wpf.DependencyProperties
{
    /// <summary>
    /// Provides dependency properties for the <see cref="System.Windows.Controls.ListView"/> control.
    /// </summary>
    public static class ListView
    {
        /// <summary>
        /// Dependency property. Gets or sets a value indicating whether auto resizing is enabled.
        /// </summary>
        public static readonly DependencyProperty AllowResizingProperty =
            DependencyProperty.RegisterAttached(
                "AllowResizing",
                typeof(bool),
                typeof(ListView),
                new PropertyMetadata(true));

        /// <summary>
        /// Sets the value of the <see cref="AllowResizingProperty"/>.
        /// </summary>
        /// <param name="target">The element to set the value to.</param>
        /// <param name="value">The value to set.</param>
        public static void SetAllowResizing(DependencyObject target, bool value)
        {
            target.SetValue(AllowResizingProperty, value);
        }

        /// <summary>
        /// Gets the value of the <see cref="AllowResizingProperty"/>.
        /// </summary>
        /// <param name="target">The element to get the value from.</param>
        /// <returns>The value of the <see cref="AllowResizingProperty"/>.</returns>
        public static bool GetAllowResizing(DependencyObject target)
        {
            return (bool)target.GetValue(AllowResizingProperty);
        }
    }
}
