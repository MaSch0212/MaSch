using System.ComponentModel.Composition;
using MaSch.Presentation.Services;

namespace MaSch.Presentation.Wpf.Services
{
    [Export(typeof(IMessageBoxService))]
    public class MessageBoxService : IMessageBoxService
    {
        public AlertResult Show(string messageBoxText)
            => MessageBox.ShowAlert(messageBoxText);

        public AlertResult Show(string messageBoxText, string caption)
            => MessageBox.ShowAlert(messageBoxText, caption);

        public AlertResult Show(string messageBoxText, string caption, AlertButton button)
            => MessageBox.ShowAlert(messageBoxText, caption, button);

        public AlertResult Show(string messageBoxText, string caption, AlertButton button, AlertImage icon)
            => MessageBox.ShowAlert(messageBoxText, caption, button, icon);

        public AlertResult Show(string messageBoxText, string caption, AlertButton button, AlertImage icon, AlertResult defaultResult)
            => MessageBox.ShowAlert(messageBoxText, caption, button, icon, defaultResult);

        public AlertResult Show(string messageBoxText, string caption, AlertButton button, AlertImage icon, AlertResult defaultResult, AlertOptions options)
            => MessageBox.ShowAlert(messageBoxText, caption, button, icon, defaultResult, options);
    }
}
