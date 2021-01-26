using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MaSch.Common;
using MaSch.Common.Observable;
using MaSch.Presentation.Observable.Collections;

namespace MaSch.Presentation.Wpf.Printing
{
    public class PrintTaskManager : ObservableObject
    {
        private int _nextJobId = 1;

        private Task _printerTask;
        private PrintTask _currentlyPrinting;

        public PrintTask CurrentlyPrinting
        {
            get => _currentlyPrinting;
            private set => SetProperty(ref _currentlyPrinting, value);
        }
        public ObservableCollection<PrintTask> PrintHistory { get; set; }
        public ObservableQueue<PrintTask> PrintTaskQueue { get; }

        public PrintTaskManager()
        {
            PrintHistory = new ObservableCollection<PrintTask>();
            PrintTaskQueue = new ObservableQueue<PrintTask>();
            PrintTaskQueue.PropertyChanged += PrintTaskQueue_PropertyChanged;
        }

        public void QueueNewTask(PrintTask task)
        {
            Guard.NotNull(task, nameof(task));
            if (task.State != PrintTaskState.NotStarted)
                throw new InvalidOperationException("Only a task in the state NotStarted can be queued.");
            task.State = PrintTaskState.Queued;
            PrintTaskQueue.Enqueue(task);
        }

        private void PrintTaskQueue_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
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
}
