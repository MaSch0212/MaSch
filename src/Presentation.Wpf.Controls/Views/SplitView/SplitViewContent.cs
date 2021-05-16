using MaSch.Presentation.Wpf.Commands;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MaSch.Presentation.Wpf.Views.SplitView
{
    /// <summary>
    /// An async handler for a <see cref="CancelEventHandler"/>.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public delegate Task AsyncCancelEventHandler(object sender, CancelEventArgs e);

    /// <summary>
    /// Content control that can be used inside a <see cref="SplitView"/>.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.ContentControl" />
    public class SplitViewContent : ContentControl
    {
        /// <summary>
        /// Dependency property. Gets or sets the command that is executed when the page this <see cref="SplitViewContent"/> is on is opened.
        /// </summary>
        public static readonly DependencyProperty OpenCommandProperty =
            DependencyProperty.Register("OpenCommand", typeof(ICommand), typeof(SplitViewContent), new PropertyMetadata(null));

        /// <summary>
        /// Dependency property. Gets or sets the command that is executed when the page this <see cref="SplitViewContent"/> is on is closed.
        /// </summary>
        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.Register("CloseCommand", typeof(ICommand), typeof(SplitViewContent), new PropertyMetadata(null));

        /// <summary>
        /// Dependency property. Gets or sets a value indicating whether the <see cref="OpenCommand"/> should be executes asynchronously.
        /// </summary>
        public static readonly DependencyProperty CallOpenAsyncProperty =
            DependencyProperty.Register("CallOpenAsync", typeof(bool), typeof(SplitViewContent), new PropertyMetadata(false));

        /// <summary>
        /// Dependency property. Gets or sets a value indicating whether the <see cref="CloseCommand"/> should be executes asynchronously.
        /// </summary>
        public static readonly DependencyProperty CallCloseAsyncProperty =
            DependencyProperty.Register("CallCloseAsync", typeof(bool), typeof(SplitViewContent), new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets the command that is executed when the page this <see cref="SplitViewContent"/> is on is opened.
        /// </summary>
        public ICommand OpenCommand
        {
            get => (ICommand)GetValue(OpenCommandProperty);
            set => SetValue(OpenCommandProperty, value);
        }

        /// <summary>
        /// Gets or sets the command that is executed when the page this <see cref="SplitViewContent"/> is on is closed.
        /// </summary>
        public ICommand CloseCommand
        {
            get => (ICommand)GetValue(CloseCommandProperty);
            set => SetValue(CloseCommandProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="OpenCommand"/> should be executes asynchronously.
        /// </summary>
        public bool CallOpenAsync
        {
            get => GetValue(CallOpenAsyncProperty) as bool? ?? false;
            set => SetValue(CallOpenAsyncProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="CloseCommand"/> should be executes asynchronously.
        /// </summary>
        public bool CallCloseAsync
        {
            get => GetValue(CallCloseAsyncProperty) as bool? ?? false;
            set => SetValue(CallCloseAsyncProperty, value);
        }

        /// <summary>
        /// Occurs when the view opened.
        /// </summary>
        public event CancelEventHandler? ViewOpened;

        /// <summary>
        /// Asynchronous event that occurs when the view opened.
        /// </summary>
        public event AsyncCancelEventHandler? ViewOpenedAsync;

        /// <summary>
        /// Occurs when the view closed.
        /// </summary>
        public event CancelEventHandler? ViewClosed;

        /// <summary>
        /// Asynchronous event that occurs when the view closed.
        /// </summary>
        public event AsyncCancelEventHandler? ViewClosedAsync;

        /// <summary>
        /// Opens this page.
        /// </summary>
        /// <returns><c>true</c> if the page has been opened successfully; otherwise, <c>false</c>.</returns>
        public virtual async Task<bool> Open()
        {
            var e = new CancelEventArgs(false);
            if (CallOpenAsync)
                await OpenInternalAsync(e, OpenCommand, DataContext);
            else
                OpenInternal(e, OpenCommand, DataContext);
            return !e.Cancel;
        }

        /// <summary>
        /// Called when the <see cref="Open"/> method has been called.
        /// </summary>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        /// <param name="command">The command.</param>
        /// <param name="dataContext">The data context.</param>
        protected virtual void OpenInternal(CancelEventArgs e, ICommand command, object dataContext)
            => Task.Run(async () => await OpenInternalAsync(e, command, dataContext)).Wait();

        /// <summary>
        /// Called when the <see cref="Open"/> method has been called.
        /// </summary>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        /// <param name="command">The command.</param>
        /// <param name="dataContext">The data context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected virtual async Task OpenInternalAsync(CancelEventArgs e, ICommand command, object dataContext)
        {
            await Task.Run(() => ViewOpened?.Invoke(this, e));
            await Task.WhenAll(ViewOpenedAsync?.GetInvocationList().Cast<AsyncCancelEventHandler>().Select(x => x(this, e)) ?? Array.Empty<Task>());
            if (command?.CanExecute(e) ?? false)
            {
                if (command is IAsyncCommand asyncCommand)
                    await asyncCommand.ExecuteAsync(null);
                else
                    await Task.Run(() => command.Execute(e));
            }

            if (dataContext is SplitViewContentViewModel svcvm)
                await svcvm.OnOpen(e);
        }

        /// <summary>
        /// Closes the page.
        /// </summary>
        /// <returns><c>true</c> if the page has been closed successfully; otherwise, <c>false</c>.</returns>
        public virtual async Task<bool> Close()
        {
            var e = new CancelEventArgs(false);
            if (CallCloseAsync)
                await CloseInternalAsync(e, CloseCommand, DataContext);
            else
                CloseInternal(e, CloseCommand, DataContext);
            return !e.Cancel;
        }

        /// <summary>
        /// Called when the <see cref="Close"/> method has been called.
        /// </summary>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        /// <param name="command">The command.</param>
        /// <param name="dataContext">The data context.</param>
        protected virtual void CloseInternal(CancelEventArgs e, ICommand command, object dataContext)
            => Task.Run(async () => await CloseInternalAsync(e, command, dataContext)).Wait();

        /// <summary>
        /// Called when the <see cref="Close"/> method has been called.
        /// </summary>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        /// <param name="command">The command.</param>
        /// <param name="dataContext">The data context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected virtual async Task CloseInternalAsync(CancelEventArgs e, ICommand command, object dataContext)
        {
            await Task.Run(() => ViewClosed?.Invoke(this, e));
            await Task.WhenAll(ViewClosedAsync?.GetInvocationList().Cast<AsyncCancelEventHandler>().Select(x => x(this, e)) ?? Array.Empty<Task>());
            if (command?.CanExecute(e) ?? false)
            {
                if (command is IAsyncCommand asyncCommand)
                    await asyncCommand.ExecuteAsync(null);
                else
                    await Task.Run(() => command.Execute(e));
            }

            if (dataContext is SplitViewContentViewModel svcvm)
                await svcvm.OnClose(e);
        }
    }
}
