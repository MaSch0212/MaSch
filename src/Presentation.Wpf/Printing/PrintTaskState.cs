namespace MaSch.Presentation.Wpf.Printing;

/// <summary>
/// Specifies the current state of a <see cref="PrintTask"/>.
/// </summary>
public enum PrintTaskState
{
    /// <summary>
    /// The print task was not started yet.
    /// </summary>
    NotStarted,

    /// <summary>
    /// The print task is queued in a <see cref="PrintTaskManager"/>.
    /// </summary>
    Queued,

    /// <summary>
    /// The print task has been incorrectly queued.
    /// </summary>
    IncorrectlyQueued,

    /// <summary>
    /// The print task is being prepared for printing.
    /// </summary>
    Preparing,

    /// <summary>
    /// The print task is waiting for the printer to print.
    /// </summary>
    WaitingForPrint,

    /// <summary>
    /// The print task is printing.
    /// </summary>
    Printing,

    /// <summary>
    /// The print task was cancelled by the user, but wait for the printer to cancel.
    /// </summary>
    Cancelling,

    /// <summary>
    /// The print task was cancelled.
    /// </summary>
    Cancelled,

    /// <summary>
    /// The print task has finished.
    /// </summary>
    Finished,
}
