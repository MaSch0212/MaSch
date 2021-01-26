using System;
using System.Windows;
using MaSch.Presentation.Wpf.ViewModels;

namespace MaSch.Presentation.Wpf
{
    public static class MessageBox
    {
        public static MessageBoxResult Show(string messageBoxText, string caption = null, MessageBoxButton button = MessageBoxButton.OK, 
            MessageBoxImage icon = MessageBoxImage.None, MessageBoxResult defaultResult = MessageBoxResult.None, MessageBoxOptions options = MessageBoxOptions.None)
        {
            return Show(null, messageBoxText, caption, button, icon, defaultResult, options);
        }

        internal static AlertResult ShowAlert(string messageBoxText, string caption = null, AlertButton button = AlertButton.Ok,
            AlertImage icon = AlertImage.None, AlertResult defaultResult = AlertResult.None, AlertOptions options = AlertOptions.None)
        {
            var msgButton = Convert(button);
            var msgIcon = Convert(icon);
            var msgDefaultResult = Convert(defaultResult);
            var msgOptions = Convert(options);

            var result = Show(null, messageBoxText, caption, msgButton, msgIcon, msgDefaultResult, msgOptions);

            return Convert(result);
        }

        public static MessageBoxResult Show(Window owner, string messageBoxText, string caption = null, MessageBoxButton button = MessageBoxButton.OK, 
            MessageBoxImage icon = MessageBoxImage.None, MessageBoxResult defaultResult = MessageBoxResult.None, MessageBoxOptions options = MessageBoxOptions.None)
        {
            return Application.Current.Dispatcher.Invoke(() =>
            {
                var vm = new MessageBoxViewModel
                {
                    Caption = caption,
                    MessageBoxText = messageBoxText,
                    MessageBoxButtons = button,
                    MessageBoxImage = icon,
                    DefaultResult = defaultResult
                };
                var dialog = new Views.MessageBox {DataContext = vm};
                dialog.Owner = owner ?? (ReferenceEquals(Application.Current?.MainWindow, dialog) ? null : Application.Current?.MainWindow);
                dialog.ShowDialog();
                return dialog.MessageBoxResult;
            });
        }


        private static MessageBoxButton Convert(AlertButton button)
        {
            switch (button)
            {
                case AlertButton.Ok:
                    return MessageBoxButton.OK;
                case AlertButton.OkCancel:
                    return MessageBoxButton.OKCancel;
                case AlertButton.YesNoCancel:
                    return MessageBoxButton.YesNoCancel;
                case AlertButton.YesNo:
                    return MessageBoxButton.YesNo;
                default:
                    throw new ArgumentOutOfRangeException(nameof(button), button, null);
            }
        }

        private static MessageBoxImage Convert(AlertImage icon)
        {
            switch (icon)
            {
                case AlertImage.None:
                    return MessageBoxImage.None;
                case AlertImage.Hand:
                    return MessageBoxImage.Hand;
                case AlertImage.Question:
                    return MessageBoxImage.Question;
                case AlertImage.Exclamation:
                    return MessageBoxImage.Exclamation;
                case AlertImage.Asterisk:
                    return MessageBoxImage.Asterisk;
                default:
                    throw new ArgumentOutOfRangeException(nameof(icon), icon, null);
            }
        }

        private static MessageBoxOptions Convert(AlertOptions options)
        {
            switch (options)
            {
                case AlertOptions.None:
                    return MessageBoxOptions.None;
                case AlertOptions.DefaultDesktopOnly:
                    return MessageBoxOptions.DefaultDesktopOnly;
                case AlertOptions.RightAlign:
                    return MessageBoxOptions.RightAlign;
                case AlertOptions.RtlReading:
                    return MessageBoxOptions.RtlReading;
                case AlertOptions.ServiceNotification:
                    return MessageBoxOptions.ServiceNotification;
                default:
                    throw new ArgumentOutOfRangeException(nameof(options), options, null);
            }
        }

        private static MessageBoxResult Convert(AlertResult result)
        {
            switch (result)
            {
                case AlertResult.None:
                    return MessageBoxResult.None;
                case AlertResult.Ok:
                    return MessageBoxResult.OK;
                case AlertResult.Cancel:
                    return MessageBoxResult.Cancel;
                case AlertResult.Yes:
                    return MessageBoxResult.Yes;
                case AlertResult.No:
                    return MessageBoxResult.No;
                default:
                    throw new ArgumentOutOfRangeException(nameof(result), result, null);
            }
        }

        private static AlertResult Convert(MessageBoxResult result)
        {
            switch (result)
            {
                case MessageBoxResult.None:
                    return AlertResult.None;
                case MessageBoxResult.OK:
                    return AlertResult.Ok;
                case MessageBoxResult.Cancel:
                    return AlertResult.Cancel;
                case MessageBoxResult.Yes:
                    return AlertResult.Yes;
                case MessageBoxResult.No:
                    return AlertResult.No;
                default:
                    throw new ArgumentOutOfRangeException(nameof(result), result, null);
            }
        }
    }
}
