using MaSch.Core;
using MaSch.Core.Observable;
using MaSch.Core.Observable.Collections;

namespace MaSch.Presentation.Wpf.Printing;

/// <summary>
/// Manages <see cref="PrintTask"/>s.
/// </summary>
/// <seealso cref="ObservableObject" />
public class PrintTaskManager : ObservableObject
{
    private int _nextJobId = 1;

    private Task? _printerTask;
    private PrintTask? _currentlyPrinting;

    /// <summary>
    /// Initializes a new instance of the <see cref="PrintTaskManager"/> class.
    /// </summary>
    public PrintTaskManager()
    {
        PrintHistory = new ObservableCollection<PrintTask>();
        PrintTaskQueue = new ObservableQueue<PrintTask>();
        PrintTaskQueue.PropertyChanged += PrintTaskQueue_PropertyChanged;
    }

    /// <summary>
    /// Gets the currently executed <see cref="PrintTask"/>.
    /// </summary>
    public PrintTask? CurrentlyPrinting
    {
        get => _currentlyPrinting;
        private set => SetProperty(ref _currentlyPrinting, value);
    }

    /// <summary>
    /// Gets or sets the print history.
    /// </summary>
    public ObservableCollection<PrintTask> PrintHistory { get; set; }

    /// <summary>
    /// Gets the print task queue.
    /// </summary>
    public ObservableQueue<PrintTask> PrintTaskQueue { get; }

    /// <summary>
    /// Queues the new <see cref="PrintTask"/>.
    /// </summary>
    /// <param name="task">The print task.</param>
    /// <exception cref="InvalidOperationException">Only a task in the state NotStarted can be queued.</exception>
    public void QueueNewTask(PrintTask task)
    {
        _ = Guard.NotNull(task, nameof(task));
        if (task.State != PrintTaskState.NotStarted)
            throw new InvalidOperationException("Only a task in the state NotStarted can be queued.");
        task.State = PrintTaskState.Queued;
        PrintTaskQueue.Enqueue(task);
    }

    private void PrintTaskQueue_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (PrintTaskQueue.Count > 0 && (_printerTask == null || _printerTask.Status == TaskStatus.Canceled || _printerTask.Status == TaskStatus.Faulted || _printerTask.Status == TaskStatus.RanToCompletion))
        {
            _printerTask = PrinterTaskWork();
        }
    }

    private async Task PrinterTaskWork()
    {
        while (PrintTaskQueue.Count > 0)
        {
            CurrentlyPrinting = PrintTaskQueue.Dequeue();
            if (CurrentlyPrinting.State == PrintTaskState.Queued)
                await CurrentlyPrinting.RunAsync(_nextJobId++);
            else
                CurrentlyPrinting.State = PrintTaskState.IncorrectlyQueued;
            CurrentlyPrinting.Dispose();
            PrintHistory.Add(CurrentlyPrinting);
        }

        CurrentlyPrinting = null;
    }
}
