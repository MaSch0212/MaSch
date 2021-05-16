using MaSch.Presentation.Wpf.Controls;
using System;

#pragma warning disable SA1649 // File name should match first type name
#pragma warning disable SA1402 // File may only contain a single type

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

    /// <summary>
    /// Default implementation of the <see cref="IStatusMessageService"/> interface.
    /// </summary>
    /// <seealso cref="MaSch.Presentation.Wpf.Services.IStatusMessageService" />
    public class StatusMessageService : IStatusMessageService
    {
        /// <inheritdoc/>
        public event StatusChangedEvent? StatusChanged;

        /// <inheritdoc/>
        public StatusType? LastStatus { get; private set; }

        /// <inheritdoc/>
        public string? LastText { get; private set; }

        /// <inheritdoc/>
        public void PushNewStatus(StatusType status, string? text)
        {
            StatusChanged?.Invoke(this, new StatusChangedEventArgs(status, text));
            LastStatus = status;
            LastText = text;
        }
    }

    /// <summary>
    /// Event that is invoked when a status has changed.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="StatusChangedEventArgs"/> instance containing the event data.</param>
    public delegate void StatusChangedEvent(object sender, StatusChangedEventArgs e);

    /// <summary>
    /// Arguments for the <see cref="StatusChangedEvent"/>.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class StatusChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the new status.
        /// </summary>
        public StatusType Status { get; }

        /// <summary>
        /// Gets the new text.
        /// </summary>
        public string? Text { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusChangedEventArgs"/> class.
        /// </summary>
        /// <param name="status">The new status.</param>
        /// <param name="text">The new text.</param>
        public StatusChangedEventArgs(StatusType status, string? text)
        {
            Status = status;
            Text = text;
        }
    }
}
