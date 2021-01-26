using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MaSch.Core.Extensions;

namespace MaSch.Presentation.Wpf.Commands
{
    public interface IAsyncCommand : ICommand
    {
        /// <summary>
        /// Executes the command asynchronously (awaitable).
        /// </summary>
        /// <param name="parameter">The parameter for the command. Is not used in this command, so it should be null.</param>
        Task ExecuteAsync(object parameter);
    }

    /// <summary>
    /// This class is an easy way to implement an asynchronous command without parameters that can be executed asynchronously.
    /// </summary>
    public abstract class AsyncCommandBase : IAsyncCommand
    {
        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Checks if the Execute method can be executed.
        /// </summary>
        /// <param name="parameter">The parameter for the command. Is not used in this command, so it should be null.</param>
        /// <returns>true if the Execute method can be executed otherwise false</returns>
        public bool CanExecute(object parameter) => CanExecute();

        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        /// <param name="parameter">The parameter for the command. Is not used in this command, so it should be null.</param>
        public async void Execute(object parameter) => await ExecuteAsync(parameter);

        /// <summary>
        /// Executes the command asynchronously (awaitable).
        /// </summary>
        /// <param name="parameter">The parameter for the command. Is not used in this command, so it should be null.</param>
        public async Task ExecuteAsync(object parameter = null)
        {
            if (IgnoreCanExecuteOnExecute || CanExecute())
                await Execute();
        }

        /// <summary>
        /// Raises the CanExecuteChanged Event. So the UI gets notified that the CanExecute method could changed its return value.
        /// </summary>
        /// <param name="sender">The sender for the event</param>
        /// <param name="e">The event arguments for the event</param>
        protected void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            Application.Current?.Dispatcher?.Invoke(() => CanExecuteChanged?.Invoke(sender, e));
        }

        /// <summary>
        /// If set to true the Execute method is called even if CanExecute would return false. 
        /// If set to false the Execute methods checks if CanExecute returns true otherwise it cancels the execution.
        /// </summary>
        protected virtual bool IgnoreCanExecuteOnExecute => false;

        /// <summary>
        /// Checks if the Execute method can be executed.
        /// </summary>
        /// <returns>true</returns>
        public virtual bool CanExecute() => true;

        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        /// <returns>a task that does the execution</returns>
        public abstract Task Execute();

        protected AsyncCommandBase(bool subscribeRequerySuggested = false)
        {
            if (subscribeRequerySuggested)
                CommandManager.RequerySuggested += (s, e) => RaiseCanExecuteChanged(this, new EventArgs());
        }
    }

    /// <summary>
    /// This class is an easy way to implement an asynchronous command with parameters of a specific type
    /// </summary>
    public abstract class AsyncCommandBase<T> : IAsyncCommand
    {
        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Checks if the Execute method can be executed.
        /// </summary>
        /// <param name="parameter">The parameter for the command.</param>
        /// <returns>true if the Execute method can be executed otherwise false</returns>
        public bool CanExecute(object parameter) => CanExecute(GetParameterValue(parameter));

        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        /// <param name="parameter">The parameter for the command.</param>
        public async void Execute(object parameter) => await ExecuteAsync(parameter);

        /// <summary>
        /// Executes the command asynchronously (awaitable).
        /// </summary>
        /// <param name="parameter">The parameter for the command. Is not used in this command, so it should be null.</param>
        public async Task ExecuteAsync(object parameter)
        {
            var tParam = GetParameterValue(parameter);
            if (IgnoreCanExecuteOnExecute || CanExecute(tParam))
                await Execute(tParam);
        }

        /// <summary>
        /// Raises the CanExecuteChanged Event. So the UI gets notified that the CanExecute method could changed its return value.
        /// </summary>
        /// <param name="sender">The sender for the event</param>
        /// <param name="e">The event arguments for the event</param>
        protected void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            Application.Current?.Dispatcher?.Invoke(() => CanExecuteChanged?.Invoke(sender, e));
        }

        /// <summary>
        /// If set to true the Execute method is called even if CanExecute would return false. 
        /// If set to false the Execute methods checks if CanExecute returns true otherwise it cancels the execution.
        /// </summary>
        protected virtual bool IgnoreCanExecuteOnExecute => false;

        /// <summary>
        /// If set to true the Execute and CanExecute method throws an exception if the given object is not castable to the given type. 
        /// If set to false the Execute and CanExecute method uses the default value for the type if the given parameter is not castable to the given type.
        /// </summary>
        protected virtual bool ThrowExceptionOnWrongParamType => true;

        /// <summary>
        /// Checks if the Execute method can be executed.
        /// </summary>
        /// <param name="parameter">The parameter for the command.</param>
        /// <returns>true if the Execute method can be executed otherwise false</returns>
        public virtual bool CanExecute(T parameter) => true;

        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        /// <param name="parameter">The parameter for the command.</param>
        /// <returns>a task that does the execution</returns>
        public abstract Task Execute(T parameter);

        private T GetParameterValue(object parameter)
        {
            if (ThrowExceptionOnWrongParamType)
                return (T)parameter;
            else
                return parameter.GetType().IsCastableTo(typeof(T)) ? (T)parameter : default(T);
        }

        protected AsyncCommandBase(bool subscribeRequerySuggested = false)
        {
            if (subscribeRequerySuggested)
                CommandManager.RequerySuggested += (s, e) => RaiseCanExecuteChanged(this, new EventArgs());
        }
    }
}
