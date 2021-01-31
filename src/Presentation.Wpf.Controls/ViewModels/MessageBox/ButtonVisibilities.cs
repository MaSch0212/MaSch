using MaSch.Core.Attributes;
using MaSch.Core.Observable;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Windows;

namespace MaSch.Presentation.Wpf.ViewModels.MessageBox
{
    [ObservablePropertyDefinition]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Property definition interface should be first in file.")]
    internal interface IButtonVisibilities_Props
    {
        Visibility Ok { get; set; }
        Visibility Cancel { get; set; }
        Visibility Yes { get; set; }
        Visibility No { get; set; }
    }

    public partial class ButtonVisibilities : ObservableObject, IButtonVisibilities_Props
    {
        public ButtonVisibilities(MessageBoxButton msgButton)
        {
            _ok = Visibility.Collapsed;
            _cancel = Visibility.Collapsed;
            _yes = Visibility.Collapsed;
            _no = Visibility.Collapsed;

            switch (msgButton)
            {
                case MessageBoxButton.OK:
                    Ok = Visibility.Visible;
                    break;
                case MessageBoxButton.OKCancel:
                    Ok = Visibility.Visible;
                    Cancel = Visibility.Visible;
                    break;
                case MessageBoxButton.YesNo:
                    Yes = Visibility.Visible;
                    No = Visibility.Visible;
                    break;
                case MessageBoxButton.YesNoCancel:
                    Yes = Visibility.Visible;
                    No = Visibility.Visible;
                    Cancel = Visibility.Visible;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(msgButton), msgButton, null);
            }
        }

        public MessageBoxButton? GetMessageBoxButton()
        {
            Visibility[] visibilities = { Ok, Cancel, Yes, No };
            StringBuilder sb = new StringBuilder(4);
            foreach (var vis in visibilities)
                sb.Append(vis == Visibility.Visible ? "1" : "0");
            string s = sb.ToString();
            if (s == "1000")
                return MessageBoxButton.OK;
            if (s == "1100")
                return MessageBoxButton.OKCancel;
            if (s == "0011")
                return MessageBoxButton.YesNo;
            if (s == "0111")
                return MessageBoxButton.YesNoCancel;
            return null;
        }
    }
}
