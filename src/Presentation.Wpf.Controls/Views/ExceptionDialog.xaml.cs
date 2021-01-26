using System;
using System.Media;
using System.Windows;
using System.Windows.Threading;

namespace MaSch.Presentation.Wpf.Views
{
    /// <summary>
    /// Interaction logic for ExceptionDialog.xaml
    /// </summary>
    public partial class ExceptionDialog
    {
        public Exception ExceptionToDisplay
        {
            get { return (Exception)GetValue(ExceptionToDisplayProperty); }
            set { SetValue(ExceptionToDisplayProperty, value); }
        }
        public static readonly DependencyProperty ExceptionToDisplayProperty =
            DependencyProperty.Register("ExceptionToDisplay", typeof(Exception), typeof(ExceptionDialog), new PropertyMetadata(new Exception("Test")));

        public string ExtraMessage
        {
            get { return (string)GetValue(ExtraMessageProperty); }
            set { SetValue(ExtraMessageProperty, value); }
        }
        public static readonly DependencyProperty ExtraMessageProperty =
            DependencyProperty.Register("ExtraMessage", typeof(string), typeof(ExceptionDialog), new PropertyMetadata(null));

        public ExceptionDialog()
        {
            InitializeComponent();
            SystemSounds.Hand.Play();
        }

        public static void HandleException(object sender, DispatcherUnhandledExceptionEventArgs eventArgs)
        {
            eventArgs.Handled = HandleException(eventArgs.Exception);
        }
        public static bool HandleException(Exception ex) => HandleException(null, ex);
        public static bool HandleException(string extraMessage, Exception ex)
        {
            try
            {
                var wdw = new ExceptionDialog { ExceptionToDisplay = ex, ExtraMessage = extraMessage };
                wdw.ShowDialog();
            }
            catch 
            {
                try { Wpf.MessageBox.Show(ex.ToString()); }
                catch
                {
                    try { System.Windows.MessageBox.Show(ex.ToString()); }
                    catch { return false; }
                }
            }
            return true;
        }

        private void ToClipboardButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(ExceptionToDisplay.ToString());
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
