namespace MaSch.Presentation.Services;

/// <summary>
/// Provides methods to show a message box or alert.
/// </summary>
public interface IMessageBoxService
{
    /// <summary>
    /// Displays a message box that has a message and that returns a result.
    /// </summary>
    /// <param name="messageBoxText">A <see cref="string"/> that specifies the text to display.</param>
    /// <returns>A <see cref="AlertResult"/> value that specifies which message box button is clicked by the user.</returns>
    AlertResult Show(string messageBoxText);

    /// <summary>
    /// Displays a message box that has a message and title bar caption; and that returns a result.
    /// </summary>
    /// <param name="messageBoxText">A <see cref="string"/> that specifies the text to display.</param>
    /// <param name="caption">A <see cref="string"/> that specifies the title bar caption to display.</param>
    /// <returns>A <see cref="AlertResult"/> value that specifies which message box button is clicked by the user.</returns>
    AlertResult Show(string messageBoxText, string? caption);

    /// <summary>
    /// Displays a message box that has a message, title bar caption, and button; and that returns a result.
    /// </summary>
    /// <param name="messageBoxText">A <see cref="string"/> that specifies the text to display.</param>
    /// <param name="caption">A <see cref="string"/> that specifies the title bar caption to display.</param>
    /// <param name="button">A <see cref="AlertButton"/> value that specifies which button or buttons to display.</param>
    /// <returns>A <see cref="AlertResult"/> value that specifies which message box button is clicked by the user.</returns>
    AlertResult Show(string messageBoxText, string? caption, AlertButton button);

    /// <summary>
    /// Displays a message box that has a message, title bar caption, button, and icon; and that returns a result.
    /// </summary>
    /// <param name="messageBoxText">A <see cref="string"/> that specifies the text to display.</param>
    /// <param name="caption">A <see cref="string"/> that specifies the title bar caption to display.</param>
    /// <param name="button">A <see cref="AlertButton"/> value that specifies which button or buttons to display.</param>
    /// <param name="icon">A <see cref="AlertImage"/> value that specifies the icon to display.</param>
    /// <returns>A <see cref="AlertResult"/> value that specifies which message box button is clicked by the user.</returns>
    AlertResult Show(string messageBoxText, string? caption, AlertButton button, AlertImage icon);

    /// <summary>
    /// Displays a message box that has a message, title bar caption, button, and icon; and that returns a result.
    /// </summary>
    /// <param name="messageBoxText">A <see cref="string"/> that specifies the text to display.</param>
    /// <param name="caption">A <see cref="string"/> that specifies the title bar caption to display.</param>
    /// <param name="button">A <see cref="AlertButton"/> value that specifies which button or buttons to display.</param>
    /// <param name="icon">A <see cref="AlertImage"/> value that specifies the icon to display.</param>
    /// <param name="defaultResult">A <see cref="AlertResult"/> value that specifies the default result of the message box.</param>
    /// <returns>A <see cref="AlertResult"/> value that specifies which message box button is clicked by the user.</returns>
    AlertResult Show(string messageBoxText, string? caption, AlertButton button, AlertImage icon, AlertResult defaultResult);

    /// <summary>
    /// Displays a message box that has a message, title bar caption, button, and icon; and that returns a result.
    /// </summary>
    /// <param name="messageBoxText">A <see cref="string"/> that specifies the text to display.</param>
    /// <param name="caption">A <see cref="string"/> that specifies the title bar caption to display.</param>
    /// <param name="button">A <see cref="AlertButton"/> value that specifies which button or buttons to display.</param>
    /// <param name="icon">A <see cref="AlertImage"/> value that specifies the icon to display.</param>
    /// <param name="defaultResult">A <see cref="AlertResult"/> value that specifies the default result of the message box.</param>
    /// <param name="options">A <see cref="AlertOptions"/> value object that specifies the options.</param>
    /// <returns>A <see cref="AlertResult"/> value that specifies which message box button is clicked by the user.</returns>
    AlertResult Show(string messageBoxText, string? caption, AlertButton button, AlertImage icon, AlertResult defaultResult, AlertOptions options);
}
