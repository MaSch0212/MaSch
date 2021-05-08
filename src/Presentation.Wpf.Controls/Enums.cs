#pragma warning disable SA1649 // File name should match first type name

namespace MaSch.Presentation.Wpf
{
    /// <summary>
    /// Specifies to wich side something is anchored.
    /// </summary>
    public enum AnchorStyle
    {
        /// <summary>
        /// Not anchored.
        /// </summary>
        None = 0,

        /// <summary>
        /// Anchored to the left.
        /// </summary>
        Left = 1,

        /// <summary>
        /// Anchored to the top.
        /// </summary>
        Top = 2,

        /// <summary>
        /// Anchored to the right.
        /// </summary>
        Right = 3,

        /// <summary>
        /// Anchored to the bottom.
        /// </summary>
        Bottom = 4,
    }

    /// <summary>
    /// Specifies the type of status to display.
    /// </summary>
    public enum StatusType
    {
        /// <summary>
        /// Displays no messsage.
        /// </summary>
        None,

        /// <summary>
        /// Displays loading messsage.
        /// </summary>
        Loading,

        /// <summary>
        /// Displays an informational messsage.
        /// </summary>
        Information,

        /// <summary>
        /// Displays a successful message.
        /// </summary>
        Success,

        /// <summary>
        /// Displays a warning message.
        /// </summary>
        Warning,

        /// <summary>
        /// Displays an error message.
        /// </summary>
        Error,
    }
}
