using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MaSch.Presentation.Wpf.Commands;

namespace MaSch.Presentation.Wpf.Views.SplitView
{
    public delegate Task AsyncCancelEventHandler(object sender, CancelEventArgs e);
    public class SplitViewContent : ContentControl
    {
        public static readonly DependencyProperty OpenCommandProperty =
            DependencyProperty.Register("OpenCommand", typeof(ICommand), typeof(SplitViewContent), new PropertyMetadata(null));
        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.Register("CloseCommand", typeof(ICommand), typeof(SplitViewContent), new PropertyMetadata(null));
        public static readonly DependencyProperty CallOpenAsyncProperty =
            DependencyProperty.Register("CallOpenAsync", typeof(bool), typeof(SplitViewContent), new PropertyMetadata(false));
        public static readonly DependencyProperty CallCloseAsyncProperty =
            DependencyProperty.Register("CallCloseAsync", typeof(bool), typeof(SplitViewContent), new PropertyMetadata(false));
        
        public ICommand OpenCommand
        {
            get => (ICommand)GetValue(OpenCommandProperty);
            set => SetValue(OpenCommandProperty, value);
        }
        public ICommand CloseCommand
        {
            get => (ICommand)GetValue(CloseCommandProperty);
            set => SetValue(CloseCommandProperty, value);
        }
        public bool CallOpenAsync
        {
            get => GetValue(CallOpenAsyncProperty) as bool? ?? false;
            set => SetValue(CallOpenAsyncProperty, value);
        }
        public bool CallCloseAsync
        {
            get => GetValue(CallCloseAsyncProperty) as bool? ?? false;
            set => SetValue(CallCloseAsyncProperty, value);
        }

        public event CancelEventHandler ViewOpened;
        public event AsyncCancelEventHandler ViewOpenedAsync;
        public event CancelEventHandler ViewClosed;
        public event AsyncCancelEventHandler ViewClosedAsync;

        public virtual async Task<bool> Open()
        {
            var e = new CancelEventArgs(false);
            if (CallOpenAsync)
                await OpenInternalAsync(e, OpenCommand, DataContext);
            else
                OpenInternal(e, OpenCommand, DataContext);
            return !e.Cancel;
        }

        protected virtual void OpenInternal(CancelEventArgs e, ICommand command, object dataContext) => Task.Run(async () => await OpenInternalAsync(e, command, dataContext)).Wait();

        protected virtual async Task OpenInternalAsync(CancelEventArgs e, ICommand command, object dataContext)
        {
            await Task.Run(() => ViewOpened?.Invoke(this, e));
            await Task.WhenAll(ViewOpenedAsync?.GetInvocationList().Cast<AsyncCancelEventHandler>().Select(x => x(this, e)) ?? new Task[0]);
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

        public virtual async Task<bool> Close()
        {
            var e = new CancelEventArgs(false);
            if (CallCloseAsync)
                await CloseInternalAsync(e, CloseCommand, DataContext);
            else
                CloseInternal(e, CloseCommand, DataContext);
            return !e.Cancel;
        }

        protected virtual void CloseInternal(CancelEventArgs e, ICommand command, object dataContext) => Task.Run(async () => await CloseInternalAsync(e, command, dataContext)).Wait();

        protected virtual async Task CloseInternalAsync(CancelEventArgs e, ICommand command, object dataContext)
        {
            await Task.Run(() => ViewClosed?.Invoke(this, e));
            await Task.WhenAll(ViewClosedAsync?.GetInvocationList().Cast<AsyncCancelEventHandler>().Select(x => x(this, e)) ?? new Task[0]);
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
