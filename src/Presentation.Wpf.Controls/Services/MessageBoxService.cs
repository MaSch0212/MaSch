using MaSch.Presentation.Services;

namespace MaSch.Presentation.Wpf.Services;

/// <summary>
/// Default implementation of the <see cref="IMessageBoxService"/> interface displaying <see cref="MessageBox"/>es.
/// </summary>
/// <seealso cref="IMessageBoxService" />
public class MessageBoxService : IMessageBoxService
{
    /// <inheritdoc/>
    public AlertResult Show(string messageBoxText)
    {
        return MessageBox.ShowAlert(messageBoxText);
    }

    /// <inheritdoc/>
    public AlertResult Show(string messageBoxText, string? caption)
    {
        return MessageBox.ShowAlert(messageBoxText, caption);
    }

    /// <inheritdoc/>
    public AlertResult Show(string messageBoxText, string? caption, AlertButton button)
    {
        return MessageBox.ShowAlert(messageBoxText, caption, button);
    }

    /// <inheritdoc/>
    public AlertResult Show(string messageBoxText, string? caption, AlertButton button, AlertImage icon)
    {
        return MessageBox.ShowAlert(messageBoxText, caption, button, icon);
    }

    /// <inheritdoc/>
    public AlertResult Show(string messageBoxText, string? caption, AlertButton button, AlertImage icon, AlertResult defaultResult)
    {
        return MessageBox.ShowAlert(messageBoxText, caption, button, icon, defaultResult);
    }

    /// <inheritdoc/>
    public AlertResult Show(string messageBoxText, string? caption, AlertButton button, AlertImage icon, AlertResult defaultResult, AlertOptions options)
    {
        return MessageBox.ShowAlert(messageBoxText, caption, button, icon, defaultResult, options);
    }
}
