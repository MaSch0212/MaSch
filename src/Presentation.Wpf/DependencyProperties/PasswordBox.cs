using System.Windows;

namespace MaSch.Presentation.Wpf.DependencyProperties
{
    /// <summary>
    /// Provides dependency properties for the <see cref="System.Windows.Controls.PasswordBox"/> control.
    /// </summary>
    public static class PasswordBox
    {
        /// <summary>
        /// Dependency property. Gets or sets the bound password.
        /// </summary>
        public static readonly DependencyProperty BoundPasswordProperty =
            DependencyProperty.RegisterAttached(
                "BoundPassword",
                typeof(string),
                typeof(PasswordBox),
                new PropertyMetadata(string.Empty, OnBoundPasswordChanged));

        /// <summary>
        /// Dependency property. Gets or sets a value indicating whether the password is bound to the <see cref="BoundPasswordProperty"/>.
        /// </summary>
        public static readonly DependencyProperty BindPasswordProperty =
            DependencyProperty.RegisterAttached(
                "BindPassword",
                typeof(bool),
                typeof(PasswordBox),
                new PropertyMetadata(false, OnBindPasswordChanged));

        /// <summary>
        /// Dependency property. Gets or sets a value indicating whether the password can be updated through the <see cref="BoundPasswordProperty"/>.
        /// </summary>
        private static readonly DependencyProperty UpdatingPassword =
            DependencyProperty.RegisterAttached(
                "UpdatingPassword",
                typeof(bool),
                typeof(PasswordBox),
                new PropertyMetadata(false));

        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // only handle this event when the property is attached to a PasswordBox
            // and when the BindPassword attached property has been set to true
            if (d is not System.Windows.Controls.PasswordBox box || !GetBindPassword(d))
            {
                return;
            }

            // avoid recursive updating by ignoring the box's changed event
            box.PasswordChanged -= HandlePasswordChanged;

            var newPassword = (string)e.NewValue;

            if (!GetUpdatingPassword(box))
            {
                box.Password = newPassword;
            }

            box.PasswordChanged += HandlePasswordChanged;
        }

        private static void OnBindPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            // when the BindPassword attached property is set on a PasswordBox,
            // start listening to its PasswordChanged event
            if (dp is not System.Windows.Controls.PasswordBox box)
            {
                return;
            }

            var wasBound = (bool)e.OldValue;
            var needToBind = (bool)e.NewValue;

            if (wasBound)
            {
                box.PasswordChanged -= HandlePasswordChanged;
            }

            if (needToBind)
            {
                box.PasswordChanged += HandlePasswordChanged;
            }
        }

        private static void HandlePasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.PasswordBox box)
            {
                // set a flag to indicate that we're updating the password
                SetUpdatingPassword(box, true);

                // push the new password into the BoundPassword property
                SetBoundPassword(box, box.Password);
                SetUpdatingPassword(box, false);
            }
        }

        /// <summary>
        /// Sets the value of the <see cref="BindPasswordProperty"/>.
        /// </summary>
        /// <param name="dp">The element to set the value to.</param>
        /// <param name="value">The value to set.</param>
        public static void SetBindPassword(DependencyObject dp, bool value)
        {
            dp.SetValue(BindPasswordProperty, value);
        }

        /// <summary>
        /// Gets the value of the <see cref="BindPasswordProperty"/>.
        /// </summary>
        /// <param name="dp">The element to get the value from.</param>
        /// <returns>The value of the <see cref="BindPasswordProperty"/>.</returns>
        public static bool GetBindPassword(DependencyObject dp)
        {
            return (bool)dp.GetValue(BindPasswordProperty);
        }

        /// <summary>
        /// Gets the value of the <see cref="BoundPasswordProperty"/>.
        /// </summary>
        /// <param name="dp">The element to get the value from.</param>
        /// <returns>The value of the <see cref="BoundPasswordProperty"/>.</returns>
        public static string GetBoundPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(BoundPasswordProperty);
        }

        /// <summary>
        /// Sets the value of the <see cref="BoundPasswordProperty"/>.
        /// </summary>
        /// <param name="dp">The element to set the value to.</param>
        /// <param name="value">The value to set.</param>
        public static void SetBoundPassword(DependencyObject dp, string value)
        {
            dp.SetValue(BoundPasswordProperty, value);
        }

        /// <summary>
        /// Gets the value of the <see cref="UpdatingPassword"/>.
        /// </summary>
        /// <param name="dp">The element to get the value from.</param>
        /// <returns>The value of the <see cref="UpdatingPassword"/>.</returns>
        private static bool GetUpdatingPassword(DependencyObject dp)
        {
            return (bool)dp.GetValue(UpdatingPassword);
        }

        /// <summary>
        /// Sets the value of the <see cref="UpdatingPassword"/>.
        /// </summary>
        /// <param name="dp">The element to set the value to.</param>
        /// <param name="value">The value to set.</param>
        private static void SetUpdatingPassword(DependencyObject dp, bool value)
        {
            dp.SetValue(UpdatingPassword, value);
        }
    }
}
