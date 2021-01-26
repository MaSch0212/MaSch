using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using MaSch.Common.Observable;
using MaSch.Presentation.Services;
using MaSch.Presentation.Wpf.Services;

namespace MaSch.Presentation.Wpf.Views.SplitView
{
    public abstract class SplitViewContentViewModel : ObservableObject
    {
        public event EventHandler<Tuple<string, MessageType>> NewMessage;

        private readonly string _productName;
        private bool _isLoading;
        private string _loadingText;
        private bool _isOpen;

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                SetProperty(ref _isLoading, value);
                if (!value)
                    LoadingText = string.Empty;
            }
        }
        public string LoadingText
        {
            get => _loadingText;
            set => SetProperty(ref _loadingText, value);
        }
        public bool IsOpen
        {
            get => _isOpen;
            set => SetProperty(ref _isOpen, value);
        }

        protected bool IsInDesignMode => DesignerProperties.GetIsInDesignMode(new DependencyObject());

        protected IMessageBoxService MessageBox { get; set; }

        protected SplitViewContentViewModel()
        {
            var assemblyPath = Assembly.GetEntryAssembly().Location;
            _productName = string.IsNullOrEmpty(assemblyPath) ? "Unknown" : FileVersionInfo.GetVersionInfo(assemblyPath).ProductName;
            MessageBox = new MessageBoxService();
        }

        public virtual async Task OnOpen(CancelEventArgs e) { await Task.Yield(); }
        public virtual async Task OnClose(CancelEventArgs e) { await Task.Yield(); }

        protected void RaiseOnMessage(string message, MessageType type)
        {
            NewMessage?.Invoke(this, new Tuple<string, MessageType>(message, type));
        }

        protected void ExecuteLoadingAction(Action action, string succeededMessage, string failedMessage)
            => ExecuteLoadingAction(() => { action(); return true; }, succeededMessage, failedMessage);
        protected void ExecuteLoadingAction(Func<bool> action, string succeededMessage, string failedMessage)
        {
            try
            {
                if (action() && !string.IsNullOrEmpty(succeededMessage))
                    RaiseOnMessage(succeededMessage, MessageType.Success);
            }
            catch
            {
                if (!string.IsNullOrEmpty(failedMessage))
                    RaiseOnMessage(failedMessage, MessageType.Failure);
                throw;
            }
        }

        protected void ExecuteLoadingAction(bool hasChanges, string noChangesText, Action action, string succeededMessage, string failedMessage)
            => ExecuteLoadingAction(hasChanges, null, AlertImage.Warning, AlertResult.Yes, noChangesText, () => { action(); return true; }, succeededMessage, failedMessage);
        protected void ExecuteLoadingAction(bool hasChanges, string askText, string noChangesText, Action action, string succeededMessage, string failedMessage)
            => ExecuteLoadingAction(hasChanges, askText, AlertImage.Warning, AlertResult.Yes, noChangesText, () => { action(); return true; }, succeededMessage, failedMessage);
        protected void ExecuteLoadingAction(bool hasChanges, string askText, AlertImage msgBoxImage, AlertResult resultForExec, string noChangesText,
            Action action, string succeededMessage, string failedMessage)
            => ExecuteLoadingAction(hasChanges, askText, msgBoxImage, resultForExec, noChangesText, () => { action(); return true; }, succeededMessage, failedMessage);
        protected void ExecuteLoadingAction(bool hasChanges, string noChangesText, Func<bool> action, string succeededMessage, string failedMessage)
            => ExecuteLoadingAction(hasChanges, null, AlertImage.Warning, AlertResult.Yes, noChangesText, action, succeededMessage, failedMessage);
        protected void ExecuteLoadingAction(bool hasChanges, string askText, string noChangesText, Func<bool> action, string succeededMessage, string failedMessage)
            => ExecuteLoadingAction(hasChanges, askText, AlertImage.Warning, AlertResult.Yes, noChangesText, action, succeededMessage, failedMessage);
        protected void ExecuteLoadingAction(bool hasChanges, string askText, AlertImage msgBoxImage, AlertResult resultForExec, string noChangesText,
            Func<bool> action, string succeededMessage, string failedMessage)
        {
            if (hasChanges)
            {
                bool exec = string.IsNullOrEmpty(askText);
                if (!exec)
                {
                    var result = MessageBox.Show(askText, _productName, AlertButton.YesNo, msgBoxImage);
                    exec = result == resultForExec;
                }
                if(exec)
                {
                    ExecuteLoadingAction(action, succeededMessage, failedMessage);
                }
            }
            else
                RaiseOnMessage(noChangesText, MessageType.Information);
        }

        protected async Task ExecuteLoadingAction(string loadingText, Func<Task> action, string succeededMessage, string failedMessage)
            => await ExecuteLoadingAction(loadingText, async () => { await action(); return true; }, succeededMessage, failedMessage);
        protected async Task ExecuteLoadingAction(string loadingText, Func<Task<bool>> action, string succeededMessage, string failedMessage)
        {
            LoadingText = loadingText;
            IsLoading = true;
            try
            {
                if (await action() && !string.IsNullOrEmpty(succeededMessage))
                    RaiseOnMessage(succeededMessage, MessageType.Success);
            }
            catch
            {
                if(!string.IsNullOrEmpty(failedMessage))
                    RaiseOnMessage(failedMessage, MessageType.Failure);
                throw;
            }
            finally { IsLoading = false; }
        }

        protected async Task ExecuteLoadingAction(bool hasChanges, string noChangesText, string loadingText, Func<Task> action, string succeededMessage, string failedMessage)
            => await ExecuteLoadingAction(hasChanges, null, AlertImage.Warning, AlertResult.Yes, noChangesText, loadingText, async () => { await action(); return true; }, succeededMessage, failedMessage);
        protected async Task ExecuteLoadingAction(bool hasChanges, string askText, string noChangesText, string loadingText, Func<Task> action, string succeededMessage, string failedMessage)
            => await ExecuteLoadingAction(hasChanges, askText, AlertImage.Warning, AlertResult.Yes, noChangesText, loadingText, async () => { await action(); return true; }, succeededMessage, failedMessage);
        protected async Task ExecuteLoadingAction(bool hasChanges, string askText, AlertImage msgBoxImage, AlertResult resultForExec, string noChangesText,
            string loadingText, Func<Task> action, string succeededMessage, string failedMessage)
            => await ExecuteLoadingAction(hasChanges, askText, msgBoxImage, resultForExec, noChangesText, loadingText, async () => { await action(); return true; }, succeededMessage, failedMessage);
        protected async Task ExecuteLoadingAction(bool hasChanges, string noChangesText, string loadingText, Func<Task<bool>> action, string succeededMessage, string failedMessage)
            => await ExecuteLoadingAction(hasChanges, null, AlertImage.Warning, AlertResult.Yes, noChangesText, loadingText, action, succeededMessage, failedMessage);
        protected async Task ExecuteLoadingAction(bool hasChanges, string askText, string noChangesText, string loadingText, Func<Task<bool>> action, string succeededMessage, string failedMessage)
            => await ExecuteLoadingAction(hasChanges, askText, AlertImage.Warning, AlertResult.Yes, noChangesText, loadingText, action, succeededMessage, failedMessage);
        protected async Task ExecuteLoadingAction(bool hasChanges, string askText, AlertImage msgBoxImage, AlertResult resultForExec, string noChangesText,
            string loadingText, Func<Task<bool>> action, string succeededMessage, string failedMessage)
        {
            if (hasChanges)
            {
                bool exec = string.IsNullOrEmpty(askText);
                if (!exec)
                {
                    var result = MessageBox.Show(askText, _productName, AlertButton.YesNo, msgBoxImage);
                    exec = result == resultForExec;
                }
                if (exec)
                {
                    await ExecuteLoadingAction(loadingText, action, succeededMessage, failedMessage);
                }
            }
            else
                RaiseOnMessage(noChangesText, MessageType.Information);
        }
    }
}
