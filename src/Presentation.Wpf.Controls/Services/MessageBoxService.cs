﻿using System.ComponentModel.Composition;
using MaSch.Presentation.Services;

namespace MaSch.Presentation.Wpf.Services
{
    /// <summary>
    /// Default implementation of the <see cref="IMessageBoxService"/> interface displaying <see cref="MessageBox"/>es.
    /// </summary>
    /// <seealso cref="MaSch.Presentation.Services.IMessageBoxService" />
    [Export(typeof(IMessageBoxService))]
    public class MessageBoxService : IMessageBoxService
    {
        /// <inheritdoc/>
        public AlertResult Show(string messageBoxText)
            => MessageBox.ShowAlert(messageBoxText);

        /// <inheritdoc/>
        public AlertResult Show(string messageBoxText, string caption)
            => MessageBox.ShowAlert(messageBoxText, caption);

        /// <inheritdoc/>
        public AlertResult Show(string messageBoxText, string caption, AlertButton button)
            => MessageBox.ShowAlert(messageBoxText, caption, button);

        /// <inheritdoc/>
        public AlertResult Show(string messageBoxText, string caption, AlertButton button, AlertImage icon)
            => MessageBox.ShowAlert(messageBoxText, caption, button, icon);

        /// <inheritdoc/>
        public AlertResult Show(string messageBoxText, string caption, AlertButton button, AlertImage icon, AlertResult defaultResult)
            => MessageBox.ShowAlert(messageBoxText, caption, button, icon, defaultResult);

        /// <inheritdoc/>
        public AlertResult Show(string messageBoxText, string caption, AlertButton button, AlertImage icon, AlertResult defaultResult, AlertOptions options)
            => MessageBox.ShowAlert(messageBoxText, caption, button, icon, defaultResult, options);
    }
}
