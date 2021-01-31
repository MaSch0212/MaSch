using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using MaSch.Core.Attributes;
using MaSch.Core.Observable;
using MaSch.Presentation.Wpf.Commands;
using ThreadState = System.Threading.ThreadState;

namespace MaSch.Presentation.Wpf.Printing
{
    /// <summary>
    /// Represents a print task.
    /// </summary>
    /// <seealso cref="MaSch.Core.Observable.ObservableObject" />
    /// <seealso cref="System.IDisposable" />
    public sealed class PrintTask : ObservableObject, IDisposable
    {
        /// <summary>
        /// Delegate to create elements to print.
        /// </summary>
        /// <param name="task">The print task.</param>
        /// <param name="page">The page of the document.</param>
        /// <param name="data">The data attached to the page.</param>
        /// <returns>The elements to print.</returns>
        public delegate FrameworkElement[] CreateDocumentDelegate(PrintTask task, int page, object data);

        private int _totalPageCount;
        private int _currentDocument;
        private PrintTaskState _state = PrintTaskState.NotStarted;
        private string _name = string.Empty;
        private bool _isLandscape;
        private DateTime _finishedTime;

        /// <summary>
        /// Gets the total page count.
        /// </summary>
        public int TotalPageCount
        {
            get => _totalPageCount;
            private set => SetProperty(ref _totalPageCount, value);
        }

        /// <summary>
        /// Gets the index of the current document.
        /// </summary>
        public int CurrentDocument
        {
            get => _currentDocument;
            private set => SetProperty(ref _currentDocument, value);
        }

        /// <summary>
        /// Gets the state of this <see cref="PrintTask"/>.
        /// </summary>
        public PrintTaskState State
        {
            get => _state;
            internal set
            {
                if (value != _state)
                {
                    SetProperty(ref _state, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of this <see cref="PrintTask"/>.
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the printed document is landscape.
        /// </summary>
        /// <exception cref="InvalidOperationException">The landscape mode can only be changed if the task has not been started or queued yet.</exception>
        public bool IsLandscape
        {
            get => _isLandscape;
            set
            {
                if (State != PrintTaskState.NotStarted)
                    throw new InvalidOperationException("The landscape mode can only be changed if the task has not been started or queued yet.");
                _isLandscape = value;
            }
        }

        /// <summary>
        /// Gets the time, this <see cref="PrintTask"/> finished.
        /// </summary>
        public DateTime FinishedTime
        {
            get => _finishedTime;
            private set => SetProperty(ref _finishedTime, value);
        }

        /// <summary>
        /// Gets the progress of this <see cref="PrintTask"/>.
        /// </summary>
        [DependsOn(nameof(State), nameof(TotalPageCount), nameof(CurrentDocument))]
        public double Progress
        {
            get
            {
                if ((State == PrintTaskState.Preparing || State == PrintTaskState.Printing) && TotalPageCount > 0)
                {
                    return ((double)CurrentDocument / TotalPageCount) * 100;
                }
                else
                {
                    return double.NaN;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="PrintTask"/> has finished.
        /// </summary>
        [DependsOn(nameof(State))]
        public bool HasFinished => State == PrintTaskState.Cancelled || State == PrintTaskState.Finished || State == PrintTaskState.IncorrectlyQueued;

        /// <summary>
        /// Gets a value indicating whether this <see cref="PrintTask"/> has progress.
        /// </summary>
        [DependsOn(nameof(State))]
        public bool HasProgress => State == PrintTaskState.Preparing || State == PrintTaskState.Printing;

        /// <summary>
        /// Gets or sets the print queue.
        /// </summary>
        public PrintQueue PrintQueue { get; set; }

        /// <summary>
        /// Gets or sets the print ticket.
        /// </summary>
        public PrintTicket PrintTicket { get; set; }

        /// <summary>
        /// Gets or sets temporary data for this <see cref="PrintTask"/>.
        /// </summary>
        public Dictionary<string, object> TempData { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the print data.
        /// </summary>
        public IEnumerable<object> PrintData { get; set; }

        /// <summary>
        /// Gets or sets the create document function.
        /// </summary>
        public CreateDocumentDelegate CreateDocumentFunction { get; set; }

        /// <summary>
        /// Gets or sets the cancel print command.
        /// </summary>
        [DependsOn(nameof(State))]
        public ICommand CancelPrintCommand { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintTask"/> class.
        /// </summary>
        public PrintTask()
            : this(new object[0], (t, i, o) => null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintTask"/> class.
        /// </summary>
        /// <param name="data">The print data.</param>
        /// <param name="createDocument">The create document function.</param>
        public PrintTask(IEnumerable<object> data, CreateDocumentDelegate createDocument)
        {
            PrintData = data;
            CreateDocumentFunction = createDocument;
            Name = "Print task for application " + Assembly.GetEntryAssembly().GetName().Name;

            CancelPrintCommand = new DelegateCommand(CanCancel, Cancel);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintTask"/> class.
        /// </summary>
        /// <param name="printQueue">The print queue.</param>
        /// <param name="data">The print data.</param>
        /// <param name="createDocument">The create document function.</param>
        public PrintTask(PrintQueue printQueue, IEnumerable<object> data, CreateDocumentDelegate createDocument)
            : this(data, createDocument)
        {
            PrintQueue = printQueue;
        }

        private bool CanCancel()
        {
            return State == PrintTaskState.NotStarted ||
                State == PrintTaskState.Queued ||
                State == PrintTaskState.Preparing ||
                State == PrintTaskState.WaitingForPrint ||
                State == PrintTaskState.Printing;
        }

        /// <summary>
        /// Cancels this <see cref="PrintTask"/>.
        /// </summary>
        public void Cancel()
        {
            switch (State)
            {
                case PrintTaskState.NotStarted:
                case PrintTaskState.Queued:
                    State = PrintTaskState.Cancelled;
                    break;
                case PrintTaskState.Preparing:
                case PrintTaskState.WaitingForPrint:
                case PrintTaskState.Printing:
                    State = PrintTaskState.Cancelling;
                    break;
                case PrintTaskState.IncorrectlyQueued:
                case PrintTaskState.Cancelling:
                case PrintTaskState.Cancelled:
                case PrintTaskState.Finished:
                    break;
            }
        }

        /// <summary>
        /// Cancels this <see cref="PrintTask"/> and waits until it is canceled.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task CancelAndWaitAsync()
        {
            Cancel();
            await Task.Run(() =>
            {
                while (State != PrintTaskState.Cancelled && State != PrintTaskState.Finished)
                {
                    Thread.Sleep(500);
                }
            });
        }

        /// <summary>
        /// Starts this <see cref="PrintTask"/>.
        /// </summary>
        /// <param name="runId">The run identifier.</param>
        public void Start(int runId)
        {
            RunAsync(runId).ConfigureAwait(false);
        }

        /// <summary>
        /// Runs this <see cref="PrintTask"/>.
        /// </summary>
        /// <param name="runId">The run identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task RunAsync(int runId)
        {
            var pqServerName = PrintQueue.HostingPrintServer.Name;
            var pqName = PrintQueue.Name;
            var jobName = $"{Name} ({runId})";

            State = PrintTaskState.Preparing;
            var th = new Thread(() =>
            {
                using var pq = new PrintQueue(new PrintServer(pqServerName), pqName);
                var document = new FixedDocument();
                document.DocumentPaginator.PageSize = IsLandscape ? new Size(1142, 807) : new Size(807, 1142);

                CurrentDocument = 0;
                TotalPageCount = PrintData.Count();
                var documentsToPrint = TotalPageCount;
                foreach (var context in PrintData)
                {
                    CurrentDocument++;
                    if (State == PrintTaskState.Cancelling)
                    {
                        State = PrintTaskState.Cancelled;
                        return;
                    }

                    var controls = CreateDocumentFunction.Invoke(this, CurrentDocument, context);
                    if (controls?.All(x => x == null) ?? true)
                    {
                        documentsToPrint--;
                        continue;
                    }

                    foreach (var control in controls)
                    {
                        var page = new FixedPage
                        {
                            Width = document.DocumentPaginator.PageSize.Width,
                            Height = document.DocumentPaginator.PageSize.Height,
                        };

                        page.Children.Add(control);
                        var pageContent = new PageContent { Child = page };
                        document.Pages.Add(pageContent);
                    }
                }

                TotalPageCount = documentsToPrint;
                if (TotalPageCount > 0)
                {
                    CurrentDocument = 0;
                    State = PrintTaskState.WaitingForPrint;

                    pq.CurrentJobSettings.Description = jobName;
                    var xpsWriter = PrintQueue.CreateXpsDocumentWriter(pq);
                    xpsWriter.WritingPrintTicketRequired += (s, e) =>
                    {
                        var ticket = PrintTicket;
                        ticket.PageOrientation = IsLandscape ? PageOrientation.Landscape : PageOrientation.Portrait;
                        e.CurrentPrintTicket = ticket;
                    };
                    xpsWriter.Write(document);
                }
                else
                {
                    State = PrintTaskState.Finished;
                }
            });

            th.SetApartmentState(ApartmentState.STA);
            th.Start();

            await Task.Run(() =>
            {
                using var pq = new PrintQueue(new PrintServer(pqServerName), pqName);
                PrintSystemJobInfo p = null;
                while (th.ThreadState == ThreadState.Running || th.ThreadState == ThreadState.WaitSleepJoin || (Progress < 100 && State != PrintTaskState.Cancelled) ||
                    State == PrintTaskState.Preparing || State == PrintTaskState.WaitingForPrint)
                {
                    var prevP = p;
                    try
                    {
                        p = pq.GetPrintJobInfoCollection().FirstOrDefault(j => j.Name == jobName);
                    }
                    catch
                    {
                        break;
                    }

                    if (p == null && prevP != null)
                        break;

                    if (p != null && (p.IsPrinting || p.IsCompleted))
                    {
                        if (State != PrintTaskState.Cancelling)
                            State = PrintTaskState.Printing;
                        CurrentDocument = Math.Min(p.NumberOfPagesPrinted + 1, TotalPageCount);
                    }
                    else if (State != PrintTaskState.Preparing && State != PrintTaskState.Cancelling && State != PrintTaskState.Cancelled)
                    {
                        State = PrintTaskState.WaitingForPrint;
                    }

                    if (State == PrintTaskState.Cancelling)
                    {
                        p?.Cancel();
                    }

                    Task.Delay(100);
                }
            });

            // TODO: Find way to use Cancellation Token instead of Thread.Abort()
#if NETFRAMEWORK
            th.Abort();
#endif

            State = State == PrintTaskState.Cancelling ? PrintTaskState.Cancelled : PrintTaskState.Finished;
            FinishedTime = DateTime.Now;
        }

        /// <summary>
        /// Chooses the printer.
        /// </summary>
        /// <param name="dialog">The dialog.</param>
        /// <returns><c>true</c> if a printer was chosen by the user; otherwise, <c>false</c>.</returns>
        public bool? ChoosePrinter(PrintDialog dialog = null)
        {
            if (dialog == null)
                dialog = new PrintDialog();
            var result = dialog.ShowDialog();
            if (result == true)
            {
                PrintTicket = dialog.PrintTicket;
                PrintQueue = dialog.PrintQueue;
            }

            return result;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            PrintQueue.Dispose();
            CancelPrintCommand = new DelegateCommand(() => false, () => { });
            CreateDocumentFunction = null;
            CurrentDocument = -1;
            PrintData = null;
            PrintTicket = null;
            TempData = null;
        }
    }
}
