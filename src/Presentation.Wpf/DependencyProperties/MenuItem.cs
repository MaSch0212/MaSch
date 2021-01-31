using System.Windows;

namespace MaSch.Presentation.Wpf.DependencyProperties
{
    /// <summary>
    /// Provides dependency properties for the <see cref="System.Windows.Controls.MenuItem"/> control.
    /// </summary>
    public class MenuItem
    {
        /// <summary>
        /// Dependency property. Gets or sets a value indicating whether the <see cref="System.Windows.Controls.MenuItem.IsChecked"/> property is <c>null</c> to display an indeterminate symbol.
        /// </summary>
        public static readonly DependencyProperty IsCheckedNullProperty =
            DependencyProperty.RegisterAttached(
                "IsCheckedNull",
                typeof(bool),
                typeof(MenuItem),
                new PropertyMetadata(false));

        /// <summary>
        /// Sets the value of the <see cref="IsCheckedNullProperty"/>.
        /// </summary>
        /// <param name="target">The element to set the value to.</param>
        /// <param name="value">The value to set.</param>
        public static void SetIsCheckedNull(DependencyObject target, bool value)
        {
            target.SetValue(IsCheckedNullProperty, value);
        }

        /// <summary>
        /// Gets the value of the <see cref="IsCheckedNullProperty"/>.
        /// </summary>
        /// <param name="target">The element to get the value from.</param>
        /// <returns>The value of the <see cref="IsCheckedNullProperty"/>.</returns>
        public static bool GetIsCheckedNull(DependencyObject target)
        {
            return (bool)target.GetValue(IsCheckedNullProperty);
        }
    }
}
