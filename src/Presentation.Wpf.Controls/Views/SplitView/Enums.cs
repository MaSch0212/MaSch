#pragma warning disable SA1649 // File name should match first type name

namespace MaSch.Presentation.Wpf.Views.SplitView;

/// <summary>
/// Specifies the type of message in a <see cref="ExtendedSplitViewContent"/>.
/// </summary>
public enum MessageType
{
    /// <summary>
    /// Shows a success message.
    /// </summary>
    Success,

    /// <summary>
    /// Shows a fail message.
    /// </summary>
    Failure,

    /// <summary>
    /// Shows a warning message.
    /// </summary>
    Warning,

    /// <summary>
    /// Shows a informational message.
    /// </summary>
    Information,
}
