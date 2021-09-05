using System;

namespace MaSch.Presentation.Wpf.Services
{
    /// <summary>
    /// Event that is invoked when a status has changed.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="StatusChangedEventArgs"/> instance containing the event data.</param>
    public delegate void StatusChangedEvent(object sender, StatusChangedEventArgs e);

    /// <summary>
    /// Arguments for the <see cref="StatusChangedEvent"/>.
    /// </summary>
    /// <seealso cref="EventArgs" />
    public class StatusChangedEventArgs : EventArgs
    {
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

        /// <summary>
        /// Gets the new status.
        /// </summary>
        public StatusType Status { get; }

        /// <summary>
        /// Gets the new text.
        /// </summary>
        public string? Text { get; }
    }
}
