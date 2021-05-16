using MaSch.Core.Attributes;
using MaSch.Core.Observable;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Windows;

namespace MaSch.Presentation.Wpf.ViewModels.MessageBox
{
    /// <summary>
    /// Observable properties of the <see cref="ButtonVisibilities"/> class.
    /// </summary>
    [ObservablePropertyDefinition]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Property definition interface should be first in file.")]
    internal interface IButtonVisibilitiesProps
    {
        /// <summary>
        /// Gets or sets the visibility of the OK button.
        /// </summary>
        Visibility Ok { get; set; }

        /// <summary>
        /// Gets or sets the visiblity of the Cancel button.
        /// </summary>
        Visibility Cancel { get; set; }

        /// <summary>
        /// Gets or sets the visibility of the Yes button.
        /// </summary>
        Visibility Yes { get; set; }

        /// <summary>
        /// Gets or sets the visibility of the No button.
        /// </summary>
        Visibility No { get; set; }
    }

    /// <summary>
    /// Button state for a message box.
    /// </summary>
    /// <seealso cref="MaSch.Core.Observable.ObservableObject" />
    /// <seealso cref="MaSch.Presentation.Wpf.ViewModels.MessageBox.IButtonVisibilitiesProps" />
    public partial class ButtonVisibilities : ObservableObject, IButtonVisibilitiesProps
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonVisibilities"/> class.
        /// </summary>
        /// <param name="msgButton">The buttons to show in the message box.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="msgButton"/> is unknown.</exception>
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

        /// <summary>
        /// Gets the message box buttons from the current state.
        /// </summary>
        /// <returns><see cref="MessageBoxButton"/> that represents the current state of this object.</returns>
        public MessageBoxButton? GetMessageBoxButton()
        {
            Visibility[] visibilities = { Ok, Cancel, Yes, No };
            StringBuilder sb = new(4);
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
