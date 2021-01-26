namespace MaSch.Presentation.Wpf.Printing
{
    public enum PrintTaskState
    {
        NotStarted,
        Queued,
        IncorrectlyQueued,
        Preparing,
        WaitingForPrint,
        Printing,
        Cancelling,
        Cancelled,
        Finished
    }
}
