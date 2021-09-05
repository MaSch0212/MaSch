using MaSch.Presentation.Wpf.Controls;

namespace MaSch.Presentation.Wpf.Services
{
    /// <summary>
    /// Service that controls a <see cref="StatusMessage"/> control.
    /// </summary>
    public interface IStatusMessageService
    {
        /// <summary>
        /// Occurs when the status changed.
        /// </summary>
        event StatusChangedEvent StatusChanged;

        /// <summary>
        /// Gets the last status.
        /// </summary>
        StatusType? LastStatus { get; }

        /// <summary>
        /// Gets the last text.
        /// </summary>
        string? LastText { get; }

        /// <summary>
        /// Pushes a new status to the control.
        /// </summary>
        /// <param name="status">The new status.</param>
        /// <param name="text">The new text.</param>
        void PushNewStatus(StatusType status, string? text);
    }
}
