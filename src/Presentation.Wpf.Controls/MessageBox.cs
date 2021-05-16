using MaSch.Presentation.Wpf.ViewModels;
using System;
using System.Windows;

namespace MaSch.Presentation.Wpf
{
    /// <summary>
    /// Static class to show a styled MessageBox.
    /// </summary>
    public static class MessageBox
    {
        /// <summary>
        /// Shows the specified message box.
        /// </summary>
        /// <param name="messageBoxText">The message box text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="button">The buttons to show.</param>
        /// <param name="icon">The icon to show.</param>
        /// <param name="defaultResult">The default result.</param>
        /// <param name="options">The options.</param>
        /// <returns>The result of the message box.</returns>
        public static MessageBoxResult Show(
            string messageBoxText,
            string? caption = null,
            MessageBoxButton button = MessageBoxButton.OK,
            MessageBoxImage icon = MessageBoxImage.None,
            MessageBoxResult defaultResult = MessageBoxResult.None,
            MessageBoxOptions options = MessageBoxOptions.None)
        {
            return Show(null, messageBoxText, caption, button, icon, defaultResult, options);
        }

        /// <summary>
        /// Shows the specified alert.
        /// </summary>
        /// <param name="messageBoxText">The alert text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="button">The buttons to show.</param>
        /// <param name="icon">The icon to show.</param>
        /// <param name="defaultResult">The default result.</param>
        /// <param name="options">The options.</param>
        /// <returns>The result of the alert.</returns>
        internal static AlertResult ShowAlert(
            string messageBoxText,
            string? caption = null,
            AlertButton button = AlertButton.Ok,
            AlertImage icon = AlertImage.None,
            AlertResult defaultResult = AlertResult.None,
            AlertOptions options = AlertOptions.None)
        {
            var msgButton = Convert(button);
            var msgIcon = Convert(icon);
            var msgDefaultResult = Convert(defaultResult);
            var msgOptions = Convert(options);

            var result = Show(null, messageBoxText, caption, msgButton, msgIcon, msgDefaultResult, msgOptions);

            return Convert(result);
        }

        /// <summary>
        /// Shows the specified message box.
        /// </summary>
        /// <param name="owner">The owner of the message box.</param>
        /// <param name="messageBoxText">The message box text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="button">The buttons to show.</param>
        /// <param name="icon">The icon to show.</param>
        /// <param name="defaultResult">The default result.</param>
        /// <param name="options">The options.</param>
        /// <returns>The result of the message box.</returns>
        public static MessageBoxResult Show(
            Window? owner,
            string messageBoxText,
            string? caption = null,
            MessageBoxButton button = MessageBoxButton.OK,
            MessageBoxImage icon = MessageBoxImage.None,
            MessageBoxResult defaultResult = MessageBoxResult.None,
            MessageBoxOptions options = MessageBoxOptions.None)
        {
            return Application.Current.Dispatcher.Invoke(() =>
            {
                var vm = new MessageBoxViewModel
                {
                    Caption = caption,
                    MessageBoxText = messageBoxText,
                    MessageBoxButtons = button,
                    MessageBoxImage = icon,
                    DefaultResult = defaultResult,
                };
                var dialog = new Views.MessageBox
                {
                    DataContext = vm,
                };
                dialog.Owner = owner ?? (ReferenceEquals(Application.Current?.MainWindow, dialog) ? null : Application.Current?.MainWindow);
                dialog.ShowDialog();
                return dialog.MessageBoxResult;
            });
        }

        private static MessageBoxButton Convert(AlertButton button)
        {
            return button switch
            {
                AlertButton.Ok => MessageBoxButton.OK,
                AlertButton.OkCancel => MessageBoxButton.OKCancel,
                AlertButton.YesNoCancel => MessageBoxButton.YesNoCancel,
                AlertButton.YesNo => MessageBoxButton.YesNo,
                _ => throw new ArgumentOutOfRangeException(nameof(button), button, null),
            };
        }

        private static MessageBoxImage Convert(AlertImage icon)
        {
            return icon switch
            {
                AlertImage.None => MessageBoxImage.None,
                AlertImage.Hand => MessageBoxImage.Hand,
                AlertImage.Question => MessageBoxImage.Question,
                AlertImage.Exclamation => MessageBoxImage.Exclamation,
                AlertImage.Asterisk => MessageBoxImage.Asterisk,
                _ => throw new ArgumentOutOfRangeException(nameof(icon), icon, null),
            };
        }

        private static MessageBoxOptions Convert(AlertOptions options)
        {
            return options switch
            {
                AlertOptions.None => MessageBoxOptions.None,
                AlertOptions.DefaultDesktopOnly => MessageBoxOptions.DefaultDesktopOnly,
                AlertOptions.RightAlign => MessageBoxOptions.RightAlign,
                AlertOptions.RtlReading => MessageBoxOptions.RtlReading,
                AlertOptions.ServiceNotification => MessageBoxOptions.ServiceNotification,
                _ => throw new ArgumentOutOfRangeException(nameof(options), options, null),
            };
        }

        private static MessageBoxResult Convert(AlertResult result)
        {
            return result switch
            {
                AlertResult.None => MessageBoxResult.None,
                AlertResult.Ok => MessageBoxResult.OK,
                AlertResult.Cancel => MessageBoxResult.Cancel,
                AlertResult.Yes => MessageBoxResult.Yes,
                AlertResult.No => MessageBoxResult.No,
                _ => throw new ArgumentOutOfRangeException(nameof(result), result, null),
            };
        }

        private static AlertResult Convert(MessageBoxResult result)
        {
            return result switch
            {
                MessageBoxResult.None => AlertResult.None,
                MessageBoxResult.OK => AlertResult.Ok,
                MessageBoxResult.Cancel => AlertResult.Cancel,
                MessageBoxResult.Yes => AlertResult.Yes,
                MessageBoxResult.No => AlertResult.No,
                _ => throw new ArgumentOutOfRangeException(nameof(result), result, null),
            };
        }
    }
}
