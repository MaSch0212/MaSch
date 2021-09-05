namespace MaSch.Console.Controls
{
    /// <summary>
    /// Status for instances of the <see cref="ProgressControl"/> class.
    /// </summary>
    public enum ProgressControlStatus
    {
        /// <summary>
        /// The action has not been started yet.
        /// </summary>
        NotStarted,

        /// <summary>
        /// The action is currently running.
        /// </summary>
        Loading,

        /// <summary>
        /// The action has been completed successfully.
        /// </summary>
        Succeeeded,

        /// <summary>
        /// The action has been partially completed successfully.
        /// </summary>
        PartiallySucceeded,

        /// <summary>
        /// The action failed.
        /// </summary>
        Failed,
    }
}
