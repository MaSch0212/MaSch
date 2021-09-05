namespace MaSch.Presentation.Wpf.Services
{
    /// <summary>
    /// Provides extension methods for the <see cref="IStatusMessageService"/> interface.
    /// </summary>
    public static class StatusMessageServiceExtensions
    {
        /// <summary>
        /// Clears the status for the control.
        /// </summary>
        /// <param name="service">The service.</param>
        public static void Clear(this IStatusMessageService service)
        {
            service.PushNewStatus(StatusType.None, null);
        }

        /// <summary>
        /// Sets the status to <see cref="StatusType.Loading"/> for the control.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="loadingText">The loading text.</param>
        public static void StartLoading(this IStatusMessageService service, string? loadingText)
        {
            service.PushNewStatus(StatusType.Loading, loadingText);
        }

        /// <summary>
        /// Sets the status to <see cref="StatusType.Information"/> for the control.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="text">The text.</param>
        public static void PushInformation(this IStatusMessageService service, string? text)
        {
            service.PushNewStatus(StatusType.Information, text);
        }

        /// <summary>
        /// Sets the status to <see cref="StatusType.Success"/> for the control.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="text">The text.</param>
        public static void PushSuccess(this IStatusMessageService service, string? text)
        {
            service.PushNewStatus(StatusType.Success, text);
        }

        /// <summary>
        /// Sets the status to <see cref="StatusType.Warning"/> for the control.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="text">The text.</param>
        public static void PushWarning(this IStatusMessageService service, string? text)
        {
            service.PushNewStatus(StatusType.Warning, text);
        }

        /// <summary>
        /// Sets the status to <see cref="StatusType.Error"/> for the control.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="text">The text.</param>
        public static void PushError(this IStatusMessageService service, string? text)
        {
            service.PushNewStatus(StatusType.Error, text);
        }
    }
}
