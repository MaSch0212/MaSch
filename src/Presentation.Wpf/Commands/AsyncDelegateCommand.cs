using System;
using System.Threading.Tasks;

namespace MaSch.Presentation.Wpf.Commands
{
    /// <summary>
    /// Represents na asynchronous command without parameters which behavior is given by delegates
    /// </summary>
    public class AsyncDelegateCommand : AsyncCommandBase
    {
        private readonly Func<bool> _canExecute;
        private readonly Func<Task> _execute;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncDelegateCommand"/> class with the given execute behavior without parameters.
        /// </summary>
        /// <param name="execute">The asynchronous execute behavior</param>
        /// <param name="requerySuggested">Activate the requery suggested event for automatic updates of the command.</param>
        public AsyncDelegateCommand(Func<Task> execute, bool requerySuggested = false) : base(requerySuggested)
        {
            _execute = execute;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncDelegateCommand"/> class with the given execute behavior without parameters with a validation check.
        /// </summary>
        /// <param name="canExecute">The validation check</param>
        /// <param name="execute">The asynchronous execute behavior</param>
        /// <param name="requerySuggested">Activate the requery suggested event for automatic updates of the command.</param>
        public AsyncDelegateCommand(Func<bool> canExecute, Func<Task> execute, bool requerySuggested = false)
            : this(execute, requerySuggested)
        { _canExecute = canExecute; }

        /// <summary>
        /// Checks if the Execute method can be executed.
        /// </summary>
        /// <returns>true if the Execute method can be executed otherwise false</returns>
        public override bool CanExecute()
        {
            if (_canExecute != null)
                return _canExecute();
            return base.CanExecute();
        }

        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        public override async Task Execute()
        {
            if(_execute != null)
                await _execute();
        }

        /// <summary>
        /// Raises the CanExecuteChanged Event. So the UI gets notified that the CanExecute method could changed its return value.
        /// </summary>
        public void RaiseCanExecuteChanged() => base.RaiseCanExecuteChanged(this, new EventArgs());
    }

    /// <summary>
    /// Represents an asynchronous command with parameters which behavior is given by delegates
    /// </summary>
    public class AsyncDelegateCommand<T> : AsyncCommandBase<T>
    {
        private readonly Func<T, bool> _canExecute;
        private readonly Func<T, Task> _execute;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncDelegateCommand{T}"/> class with the given execute behavior with parameters.
        /// </summary>
        /// <param name="execute">The asynchronous execute behavior</param>
        /// <param name="throwOnWrongParamType">Determines if an exception should be thrown if a wrong parameter type is passed to the <see cref="Execute"/> or <see cref="CanExecute"/> method.</param>
        /// <param name="requerySuggested">Activate the requery suggested event for automatic updates of the command.</param>
        public AsyncDelegateCommand(Func<T, Task> execute, bool throwOnWrongParamType = true, bool requerySuggested = true) : base(requerySuggested)
        {
            _execute = execute;
            ThrowExceptionOnWrongParamType = throwOnWrongParamType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncDelegateCommand{T}"/> class with the given execute behavior with parameters with a validation check.
        /// </summary>
        /// <param name="canExecute">The validation check</param>
        /// <param name="execute">The asynchronous execute behavior</param>
        /// <param name="throwOnWrongParamType">Determines if an exception should be thrown if a wrong parameter type is passed to the <see cref="Execute"/> or <see cref="CanExecute"/> method.</param>
        /// <param name="requerySuggested">Activate the requery suggested event for automatic updates of the command.</param>
        public AsyncDelegateCommand(Func<T, bool> canExecute, Func<T, Task> execute, bool throwOnWrongParamType = true, bool requerySuggested = true)
            : this(execute, throwOnWrongParamType, requerySuggested)
        { _canExecute = canExecute; }

        /// <summary>
        /// Checks if the Execute method can be executed.
        /// </summary>
        /// <param name="parameter">The parameter for the command.</param>
        /// <returns>true if the Execute method can be executed otherwise false</returns>
        public override bool CanExecute(T parameter)
        {
            return _canExecute?.Invoke(parameter) ?? base.CanExecute(parameter);
        }

        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        /// <param name="parameter">The parameter for the command.</param>
        public override async Task Execute(T parameter)
        {
            if(_execute != null)
                await _execute(parameter);
        }

        /// <summary>
        /// Raises the CanExecuteChanged Event. So the UI gets notified that the CanExecute method could changed its return value.
        /// </summary>
        public void RaiseCanExecuteChanged() => base.RaiseCanExecuteChanged(this, new EventArgs());

        protected override bool ThrowExceptionOnWrongParamType { get; }
    }
}
