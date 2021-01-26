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
using MaSch.Common.Attributes;
using MaSch.Common.Observable;
using MaSch.Presentation.Wpf.Commands;
using ThreadState = System.Threading.ThreadState;

namespace MaSch.Presentation.Wpf.Printing
{
    public class PrintTask : ObservableObject, IDisposable
    {
        public delegate FrameworkElement[] CreateDocumentDelegate(PrintTask task, int page, object data);

        private int _totalPageCount;
        private int _currentDocument;
        private PrintTaskState _state = PrintTaskState.NotStarted;
        private string _name = string.Empty;
        private bool _isLandscape;
        private DateTime _finishedTime;

        public int TotalPageCount
        {
            get => _totalPageCount;
            private set => SetProperty(ref _totalPageCount, value);
        }
        public int CurrentDocument
        {
            get => _currentDocument;
            private set => SetProperty(ref _currentDocument, value);
        }
        public PrintTaskState State
        {
            get => _state;
            internal set { if (value != _state) { SetProperty(ref _state, value); } }
        }
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
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
        public DateTime FinishedTime
        {
            get => _finishedTime;
            private set => SetProperty(ref _finishedTime, value);
        }

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
                    return double.NaN;
            }
        }

        [DependsOn(nameof(State))]
        public bool HasFinished => State == PrintTaskState.Cancelled || State == PrintTaskState.Finished || State == PrintTaskState.IncorrectlyQueued;

        [DependsOn(nameof(State))]
        public bool HasProgress => State == PrintTaskState.Preparing || State == PrintTaskState.Printing;

        public PrintQueue PrintQueue { get; set; }
        public PrintTicket PrintTicket { get; set; }
        public Dictionary<string, object> TempData { get; set; } = new Dictionary<string, object>();

        public IEnumerable<object> PrintData { get; set; }
        public CreateDocumentDelegate CreateDocumentFunction { get; set; }

        [DependsOn(nameof(State))]
        public ICommand CancelPrintCommand { get; set; }

        public PrintTask() : this(new object[0], (t, i, o) => null) { }
        public PrintTask(IEnumerable<object> data, CreateDocumentDelegate createDocument)
        {
            PrintData = data;
            CreateDocumentFunction = createDocument;
            Name = "Print task for application " + Assembly.GetEntryAssembly().GetName().Name;

            CancelPrintCommand = new DelegateCommand(CanCancel, Cancel);
        }
        public PrintTask(PrintQueue printQueue, IEnumerable<object> data, CreateDocumentDelegate createDocument) : this(data, createDocument)
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
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public async Task CancelAndWaitAsync()
        {
            Cancel();
            await Task.Run(() =>
            {
                while (State != PrintTaskState.Cancelled && State != PrintTaskState.Finished) { Thread.Sleep(500); }
            });
        }

        public void Start(int runId)
        {
            RunAsync(runId).ConfigureAwait(false);
        }

        public async Task RunAsync(int runId)
        {
            var pqServerName = PrintQueue.HostingPrintServer.Name;
            var pqName = PrintQueue.Name;
            var jobName = $"{Name} ({runId})";

            State = PrintTaskState.Preparing;
            var th = new Thread(() =>
            {
                using (var pq = new PrintQueue(new PrintServer(pqServerName), pqName))
                {
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
                                Height = document.DocumentPaginator.PageSize.Height
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
                        State = PrintTaskState.Finished;
                }
            });
            th.SetApartmentState(ApartmentState.STA);
            th.Start();

            await Task.Run(() =>
            {
                using (var pq = new PrintQueue(new PrintServer(pqServerName), pqName))
                {
                    PrintSystemJobInfo p = null;
                    while (th.ThreadState == ThreadState.Running || th.ThreadState == ThreadState.WaitSleepJoin || Progress < 100 && State != PrintTaskState.Cancelled ||
                        State == PrintTaskState.Preparing || State == PrintTaskState.WaitingForPrint)
                    {
                        var prevP = p;
                        try { p = pq.GetPrintJobInfoCollection().FirstOrDefault(j => j.Name == jobName);}
                        catch { break; }
                        if (p == null && prevP != null)
                            break;
                        if (p != null && (p.IsPrinting || p.IsCompleted))
                        {
                            if (State != PrintTaskState.Cancelling)
                                State = PrintTaskState.Printing;
                            CurrentDocument = Math.Min(p.NumberOfPagesPrinted + 1, TotalPageCount);
                        }
                        else if (State != PrintTaskState.Preparing && State != PrintTaskState.Cancelling && State != PrintTaskState.Cancelled)
                            State = PrintTaskState.WaitingForPrint;

                        if (State == PrintTaskState.Cancelling)
                        {
                            p?.Cancel();
                        }

                        Task.Delay(100);
                    }
                }
            });

            // TODO: Find way to use Cancellation Token instead of Thread.Abort()
#if NETFX
            th.Abort();
#endif

            State = State == PrintTaskState.Cancelling ? PrintTaskState.Cancelled : PrintTaskState.Finished;
            FinishedTime = DateTime.Now;
        }

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
