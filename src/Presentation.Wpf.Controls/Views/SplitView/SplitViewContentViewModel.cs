using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using MaSch.Core.Observable;
using MaSch.Presentation.Services;
using MaSch.Presentation.Wpf.Services;

namespace MaSch.Presentation.Wpf.Views.SplitView
{
    /// <summary>
    /// Abstract class that is used for view models of a <see cref="SplitViewContent"/>.
    /// </summary>
    /// <seealso cref="MaSch.Core.Observable.ObservableObject" />
    public abstract class SplitViewContentViewModel : ObservableObject
    {
        /// <summary>
        /// Occurs when a new message has been posted.
        /// </summary>
        public event EventHandler<Tuple<string, MessageType>> NewMessage;

        private readonly string _productName;
        private bool _isLoading;
        private string _loadingText;
        private bool _isOpen;

        /// <summary>
        /// Gets or sets a value indicating whether the view this model is attached to is loading.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the loading text.
        /// </summary>
        public string LoadingText
        {
            get => _loadingText;
            set => SetProperty(ref _loadingText, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the page is currently open.
        /// </summary>
        public bool IsOpen
        {
            get => _isOpen;
            set => SetProperty(ref _isOpen, value);
        }

        /// <summary>
        /// Gets a value indicating whether the view this model is attached to is in design mode.
        /// </summary>
        protected bool IsInDesignMode => DesignerProperties.GetIsInDesignMode(new DependencyObject());

        /// <summary>
        /// Gets or sets the message box service to show message boxes.
        /// </summary>
        protected IMessageBoxService MessageBox { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SplitViewContentViewModel"/> class.
        /// </summary>
        protected SplitViewContentViewModel()
        {
            var assemblyPath = Assembly.GetEntryAssembly().Location;
            _productName = string.IsNullOrEmpty(assemblyPath) ? "Unknown" : FileVersionInfo.GetVersionInfo(assemblyPath).ProductName;
            MessageBox = new MessageBoxService();
        }

        /// <summary>
        /// Executed when the page has been opened.
        /// </summary>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public virtual async Task OnOpen(CancelEventArgs e)
        {
            await Task.Yield();
        }

        /// <summary>
        /// Executed when the page has been closed.
        /// </summary>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public virtual async Task OnClose(CancelEventArgs e)
        {
            await Task.Yield();
        }

        /// <summary>
        /// Raises the <see cref="NewMessage"/> event.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="type">The type.</param>
        protected void RaiseOnMessage(string message, MessageType type)
        {
            NewMessage?.Invoke(this, new Tuple<string, MessageType>(message, type));
        }

        /// <summary>
        /// Executes a loading action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="succeededMessage">The succeeded message.</param>
        /// <param name="failedMessage">The failed message.</param>
        protected void ExecuteLoadingAction(Action action, string succeededMessage, string failedMessage)
            => ExecuteLoadingAction(
                () =>
                {
                    action();
                    return true;
                },
                succeededMessage,
                failedMessage);

        /// <summary>
        /// Executes a loading action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="succeededMessage">The succeeded message.</param>
        /// <param name="failedMessage">The failed message.</param>
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

        /// <summary>
        /// Executes a loading action.
        /// </summary>
        /// <param name="hasChanges">if set to <c>true</c> there were changed and the <paramref name="noChangesText"/> is not displayed.</param>
        /// <param name="noChangesText">The text that is shown if there are not changes.</param>
        /// <param name="action">The action.</param>
        /// <param name="succeededMessage">The succeeded message.</param>
        /// <param name="failedMessage">The failed message.</param>
        protected void ExecuteLoadingAction(bool hasChanges, string noChangesText, Action action, string succeededMessage, string failedMessage)
            => ExecuteLoadingAction(
                hasChanges,
                null,
                AlertImage.Warning,
                AlertResult.Yes,
                noChangesText,
                () =>
                {
                    action();
                    return true;
                },
                succeededMessage,
                failedMessage);

        /// <summary>
        /// Executes a loading action.
        /// </summary>
        /// <param name="hasChanges">if set to <c>true</c> there were changed and the <paramref name="noChangesText"/> is not displayed.</param>
        /// <param name="askText">The text that is displayed in a message box asking the user if he/she wants to proceed with an action.</param>
        /// <param name="noChangesText">The text that is shown if there are not changes.</param>
        /// <param name="action">The action.</param>
        /// <param name="succeededMessage">The succeeded message.</param>
        /// <param name="failedMessage">The failed message.</param>
        protected void ExecuteLoadingAction(bool hasChanges, string askText, string noChangesText, Action action, string succeededMessage, string failedMessage)
            => ExecuteLoadingAction(
                hasChanges,
                askText,
                AlertImage.Warning,
                AlertResult.Yes,
                noChangesText,
                () =>
                {
                    action();
                    return true;
                },
                succeededMessage,
                failedMessage);

        /// <summary>
        /// Executes a loading action.
        /// </summary>
        /// <param name="hasChanges">if set to <c>true</c> there were changed and the <paramref name="noChangesText"/> is not displayed.</param>
        /// <param name="askText">The text that is displayed in a message box asking the user if he/she wants to proceed with an action.</param>
        /// <param name="msgBoxImage">The message box image to use in the question to the user.</param>
        /// <param name="resultForExec">The result that leads to the action beeing executed.</param>
        /// <param name="noChangesText">The text that is shown if there are not changes.</param>
        /// <param name="action">The action.</param>
        /// <param name="succeededMessage">The succeeded message.</param>
        /// <param name="failedMessage">The failed message.</param>
        protected void ExecuteLoadingAction(bool hasChanges, string askText, AlertImage msgBoxImage, AlertResult resultForExec, string noChangesText, Action action, string succeededMessage, string failedMessage)
            => ExecuteLoadingAction(
                hasChanges,
                askText,
                msgBoxImage,
                resultForExec,
                noChangesText,
                () =>
                {
                    action();
                    return true;
                },
                succeededMessage,
                failedMessage);

        /// <summary>
        /// Executes a loading action.
        /// </summary>
        /// <param name="hasChanges">if set to <c>true</c> there were changed and the <paramref name="noChangesText"/> is not displayed.</param>
        /// <param name="noChangesText">The text that is shown if there are not changes.</param>
        /// <param name="action">The action.</param>
        /// <param name="succeededMessage">The succeeded message.</param>
        /// <param name="failedMessage">The failed message.</param>
        protected void ExecuteLoadingAction(bool hasChanges, string noChangesText, Func<bool> action, string succeededMessage, string failedMessage)
            => ExecuteLoadingAction(hasChanges, null, AlertImage.Warning, AlertResult.Yes, noChangesText, action, succeededMessage, failedMessage);

        /// <summary>
        /// Executes a loading action.
        /// </summary>
        /// <param name="hasChanges">if set to <c>true</c> there were changed and the <paramref name="noChangesText"/> is not displayed.</param>
        /// <param name="askText">The text that is displayed in a message box asking the user if he/she wants to proceed with an action.</param>
        /// <param name="noChangesText">The text that is shown if there are not changes.</param>
        /// <param name="action">The action.</param>
        /// <param name="succeededMessage">The succeeded message.</param>
        /// <param name="failedMessage">The failed message.</param>
        protected void ExecuteLoadingAction(bool hasChanges, string askText, string noChangesText, Func<bool> action, string succeededMessage, string failedMessage)
            => ExecuteLoadingAction(hasChanges, askText, AlertImage.Warning, AlertResult.Yes, noChangesText, action, succeededMessage, failedMessage);

        /// <summary>
        /// Executes a loading action.
        /// </summary>
        /// <param name="hasChanges">if set to <c>true</c> there were changed and the <paramref name="noChangesText"/> is not displayed.</param>
        /// <param name="askText">The text that is displayed in a message box asking the user if he/she wants to proceed with an action.</param>
        /// <param name="msgBoxImage">The message box image to use in the question to the user.</param>
        /// <param name="resultForExec">The result that leads to the action beeing executed.</param>
        /// <param name="noChangesText">The text that is shown if there are not changes.</param>
        /// <param name="action">The action.</param>
        /// <param name="succeededMessage">The succeeded message.</param>
        /// <param name="failedMessage">The failed message.</param>
        protected void ExecuteLoadingAction(bool hasChanges, string askText, AlertImage msgBoxImage, AlertResult resultForExec, string noChangesText, Func<bool> action, string succeededMessage, string failedMessage)
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
                    ExecuteLoadingAction(action, succeededMessage, failedMessage);
                }
            }
            else
            {
                RaiseOnMessage(noChangesText, MessageType.Information);
            }
        }

        /// <summary>
        /// Executes a loading action.
        /// </summary>
        /// <param name="loadingText">The text that is showed while the action is executed.</param>
        /// <param name="action">The action.</param>
        /// <param name="succeededMessage">The succeeded message.</param>
        /// <param name="failedMessage">The failed message.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected async Task ExecuteLoadingAction(string loadingText, Func<Task> action, string succeededMessage, string failedMessage)
            => await ExecuteLoadingAction(
                loadingText,
                async () =>
                {
                    await action();
                    return true;
                },
                succeededMessage,
                failedMessage);

        /// <summary>
        /// Executes a loading action.
        /// </summary>
        /// <param name="loadingText">The text that is showed while the action is executed.</param>
        /// <param name="action">The action.</param>
        /// <param name="succeededMessage">The succeeded message.</param>
        /// <param name="failedMessage">The failed message.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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
                if (!string.IsNullOrEmpty(failedMessage))
                    RaiseOnMessage(failedMessage, MessageType.Failure);
                throw;
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Executes a loading action.
        /// </summary>
        /// <param name="hasChanges">if set to <c>true</c> there were changed and the <paramref name="noChangesText"/> is not displayed.</param>
        /// <param name="noChangesText">The text that is shown if there are not changes.</param>
        /// <param name="loadingText">The text that is showed while the action is executed.</param>
        /// <param name="action">The action.</param>
        /// <param name="succeededMessage">The succeeded message.</param>
        /// <param name="failedMessage">The failed message.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected async Task ExecuteLoadingAction(bool hasChanges, string noChangesText, string loadingText, Func<Task> action, string succeededMessage, string failedMessage)
            => await ExecuteLoadingAction(
                hasChanges,
                null,
                AlertImage.Warning,
                AlertResult.Yes,
                noChangesText,
                loadingText,
                async () =>
                {
                    await action();
                    return true;
                },
                succeededMessage,
                failedMessage);

        /// <summary>
        /// Executes a loading action.
        /// </summary>
        /// <param name="hasChanges">if set to <c>true</c> there were changed and the <paramref name="noChangesText"/> is not displayed.</param>
        /// <param name="askText">The text that is displayed in a message box asking the user if he/she wants to proceed with an action.</param>
        /// <param name="noChangesText">The text that is shown if there are not changes.</param>
        /// <param name="loadingText">The text that is showed while the action is executed.</param>
        /// <param name="action">The action.</param>
        /// <param name="succeededMessage">The succeeded message.</param>
        /// <param name="failedMessage">The failed message.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected async Task ExecuteLoadingAction(bool hasChanges, string askText, string noChangesText, string loadingText, Func<Task> action, string succeededMessage, string failedMessage)
            => await ExecuteLoadingAction(
                hasChanges,
                askText,
                AlertImage.Warning,
                AlertResult.Yes,
                noChangesText,
                loadingText,
                async () =>
                {
                    await action();
                    return true;
                },
                succeededMessage,
                failedMessage);

        /// <summary>
        /// Executes a loading action.
        /// </summary>
        /// <param name="hasChanges">if set to <c>true</c> there were changed and the <paramref name="noChangesText"/> is not displayed.</param>
        /// <param name="askText">The text that is displayed in a message box asking the user if he/she wants to proceed with an action.</param>
        /// <param name="msgBoxImage">The message box image to use in the question to the user.</param>
        /// <param name="resultForExec">The result that leads to the action beeing executed.</param>
        /// <param name="noChangesText">The text that is shown if there are not changes.</param>
        /// <param name="loadingText">The text that is showed while the action is executed.</param>
        /// <param name="action">The action.</param>
        /// <param name="succeededMessage">The succeeded message.</param>
        /// <param name="failedMessage">The failed message.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected async Task ExecuteLoadingAction(bool hasChanges, string askText, AlertImage msgBoxImage, AlertResult resultForExec, string noChangesText, string loadingText, Func<Task> action, string succeededMessage, string failedMessage)
            => await ExecuteLoadingAction(
                hasChanges,
                askText,
                msgBoxImage,
                resultForExec,
                noChangesText,
                loadingText,
                async () =>
                {
                    await action();
                    return true;
                },
                succeededMessage,
                failedMessage);

        /// <summary>
        /// Executes a loading action.
        /// </summary>
        /// <param name="hasChanges">if set to <c>true</c> there were changed and the <paramref name="noChangesText"/> is not displayed.</param>
        /// <param name="noChangesText">The text that is shown if there are not changes.</param>
        /// <param name="loadingText">The text that is showed while the action is executed.</param>
        /// <param name="action">The action.</param>
        /// <param name="succeededMessage">The succeeded message.</param>
        /// <param name="failedMessage">The failed message.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected async Task ExecuteLoadingAction(bool hasChanges, string noChangesText, string loadingText, Func<Task<bool>> action, string succeededMessage, string failedMessage)
            => await ExecuteLoadingAction(hasChanges, null, AlertImage.Warning, AlertResult.Yes, noChangesText, loadingText, action, succeededMessage, failedMessage);

        /// <summary>
        /// Executes a loading action.
        /// </summary>
        /// <param name="hasChanges">if set to <c>true</c> there were changed and the <paramref name="noChangesText"/> is not displayed.</param>
        /// <param name="askText">The text that is displayed in a message box asking the user if he/she wants to proceed with an action.</param>
        /// <param name="noChangesText">The text that is shown if there are not changes.</param>
        /// <param name="loadingText">The text that is showed while the action is executed.</param>
        /// <param name="action">The action.</param>
        /// <param name="succeededMessage">The succeeded message.</param>
        /// <param name="failedMessage">The failed message.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected async Task ExecuteLoadingAction(bool hasChanges, string askText, string noChangesText, string loadingText, Func<Task<bool>> action, string succeededMessage, string failedMessage)
            => await ExecuteLoadingAction(hasChanges, askText, AlertImage.Warning, AlertResult.Yes, noChangesText, loadingText, action, succeededMessage, failedMessage);

        /// <summary>
        /// Executes a loading action.
        /// </summary>
        /// <param name="hasChanges">if set to <c>true</c> there were changed and the <paramref name="noChangesText"/> is not displayed.</param>
        /// <param name="askText">The text that is displayed in a message box asking the user if he/she wants to proceed with an action.</param>
        /// <param name="msgBoxImage">The message box image to use in the question to the user.</param>
        /// <param name="resultForExec">The result that leads to the action beeing executed.</param>
        /// <param name="noChangesText">The text that is shown if there are not changes.</param>
        /// <param name="loadingText">The text that is showed while the action is executed.</param>
        /// <param name="action">The action.</param>
        /// <param name="succeededMessage">The succeeded message.</param>
        /// <param name="failedMessage">The failed message.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected async Task ExecuteLoadingAction(bool hasChanges, string askText, AlertImage msgBoxImage, AlertResult resultForExec, string noChangesText, string loadingText, Func<Task<bool>> action, string succeededMessage, string failedMessage)
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
            {
                RaiseOnMessage(noChangesText, MessageType.Information);
            }
        }
    }
}
