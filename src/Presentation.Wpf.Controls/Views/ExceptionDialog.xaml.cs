using System;
using System.Media;
using System.Windows;
using System.Windows.Threading;

namespace MaSch.Presentation.Wpf.Views
{
    /// <summary>
    /// Dialog that can be used to show an unhandled exception to the user.
    /// </summary>
    public partial class ExceptionDialog
    {
        /// <summary>
        /// Dependency property. Gets or sets the exception to show to the user.
        /// </summary>
        public static readonly DependencyProperty ExceptionToDisplayProperty =
            DependencyProperty.Register("ExceptionToDisplay", typeof(Exception), typeof(ExceptionDialog), new PropertyMetadata(new Exception("Test")));

        /// <summary>
        /// Dependency property. Gets or sets an extra message that is shown to the user.
        /// </summary>
        public static readonly DependencyProperty ExtraMessageProperty =
            DependencyProperty.Register("ExtraMessage", typeof(string), typeof(ExceptionDialog), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the exception to show to the user.
        /// </summary>
        public Exception ExceptionToDisplay
        {
            get { return (Exception)GetValue(ExceptionToDisplayProperty); }
            set { SetValue(ExceptionToDisplayProperty, value); }
        }

        /// <summary>
        /// Gets or sets an extra message that is shown to the user.
        /// </summary>
        public string? ExtraMessage
        {
            get { return (string?)GetValue(ExtraMessageProperty); }
            set { SetValue(ExtraMessageProperty, value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionDialog"/> class.
        /// </summary>
        public ExceptionDialog()
        {
            InitializeComponent();
            SystemSounds.Hand.Play();
        }

        private void ToClipboardButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(ExceptionToDisplay.ToString());
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles an exception by showing it to a user.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="DispatcherUnhandledExceptionEventArgs"/> instance containing the event data.</param>
        public static void HandleException(object sender, DispatcherUnhandledExceptionEventArgs eventArgs)
        {
            eventArgs.Handled = HandleException(eventArgs.Exception);
        }

        /// <summary>
        /// Handles an exception by showing it to a user.
        /// </summary>
        /// <param name="ex">The exception to handle.</param>
        /// <returns><c>true</c> if the exception has been handled; otherwise, <c>false</c>.</returns>
        public static bool HandleException(Exception ex) => HandleException(null, ex);

        /// <summary>
        /// Handles an exception by showing it to a user.
        /// </summary>
        /// <param name="extraMessage">The extra message to show to the user.</param>
        /// <param name="ex">The exception to handle.</param>
        /// <returns><c>true</c> if the exception has been handled; otherwise, <c>false</c>.</returns>
        public static bool HandleException(string? extraMessage, Exception ex)
        {
            try
            {
                var wdw = new ExceptionDialog { ExceptionToDisplay = ex, ExtraMessage = extraMessage };
                wdw.ShowDialog();
            }
            catch
            {
                try
                {
                    Wpf.MessageBox.Show(ex.ToString());
                }
                catch
                {
                    try
                    {
                        System.Windows.MessageBox.Show(ex.ToString());
                    }
                    catch
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
