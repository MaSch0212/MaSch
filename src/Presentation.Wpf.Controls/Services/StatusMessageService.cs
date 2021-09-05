namespace MaSch.Presentation.Wpf.Services
{
    /// <summary>
    /// Default implementation of the <see cref="IStatusMessageService"/> interface.
    /// </summary>
    /// <seealso cref="IStatusMessageService" />
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
}
