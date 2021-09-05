using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

namespace MaSch.Presentation.Wpf.Commands
{
    /// <summary>
    /// Represents a command without parameters which behavior is given by delegates.
    /// </summary>
    public class DelegateCommand : CommandBase
    {
        private readonly Func<bool>? _canExecute;
        private readonly Action _execute;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class with the given execute behavior without parameters.
        /// </summary>
        /// <param name="execute">The execute behavior.</param>
        /// <param name="requerySuggested">Activate the requery suggested event for automatic updates of the command.</param>
        public DelegateCommand(Action execute, bool requerySuggested = false)
            : base(requerySuggested)
        {
            _execute = execute;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class with the given execute behavior without parameters with a validation check.
        /// </summary>
        /// <param name="canExecute">The validation check.</param>
        /// <param name="execute">The execute behavior.</param>
        /// <param name="requerySuggested">Activate the requery suggested event for automatic updates of the command.</param>
        public DelegateCommand(Func<bool> canExecute, Action execute, bool requerySuggested = false)
            : this(execute, requerySuggested)
        {
            _canExecute = canExecute;
        }

        /// <summary>
        /// Checks if the Execute method can be executed.
        /// </summary>
        /// <returns>true if the Execute method can be executed otherwise false.</returns>
        public override bool CanExecute()
        {
            if (_canExecute != null)
                return _canExecute();
            return base.CanExecute();
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        public override void Execute()
        {
            _execute?.Invoke();
        }

        /// <summary>
        /// Raises the CanExecuteChanged Event. So the UI gets notified that the CanExecute method could changed its return value.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            RaiseCanExecuteChanged(this, new EventArgs());
        }
    }

    /// <summary>
    /// Represents a command with parameters which behavior is given by delegates.
    /// </summary>
    /// <typeparam name="T">The parameter type for this <see cref="ICommand"/>.</typeparam>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Generic representation can be in same file.")]
    public class DelegateCommand<T> : CommandBase<T>
    {
        private readonly Func<T?, bool>? _canExecute;
        private readonly Action<T?> _execute;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand{T}"/> class with the given execute behavior with parameters.
        /// </summary>
        /// <param name="execute">The execute behavior.</param>
        /// <param name="throwOnWrongParamType">Determines if an exception should be thrown if a wrong parameter type is passed to the <see cref="Execute"/> or <see cref="CanExecute"/> method.</param>
        /// <param name="requerySuggested">Activate the requery suggested event for automatic updates of the command.</param>
        public DelegateCommand(Action<T?> execute, bool throwOnWrongParamType = true, bool requerySuggested = true)
            : base(requerySuggested)
        {
            _execute = execute;
            ThrowExceptionOnWrongParamType = throwOnWrongParamType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand{T}"/> class with the given execute behavior with parameters with a validation check.
        /// </summary>
        /// <param name="canExecute">The validation check.</param>
        /// <param name="execute">The execute behavior.</param>
        /// <param name="throwOnWrongParamType">Determines if an exception should be thrown if a wrong parameter type is passed to the <see cref="Execute"/> or <see cref="CanExecute"/> method.</param>
        /// <param name="requerySuggested">Activate the requery suggested event for automatic updates of the command.</param>
        public DelegateCommand(Func<T?, bool> canExecute, Action<T?> execute, bool throwOnWrongParamType = true, bool requerySuggested = true)
            : this(execute, throwOnWrongParamType, requerySuggested)
        {
            _canExecute = canExecute;
        }

        /// <summary>
        /// Gets a value indicating whether to throw an exception when the wrong parameter type is given.
        /// </summary>
        protected override bool ThrowExceptionOnWrongParamType { get; }

        /// <summary>
        /// Checks if the Execute method can be executed.
        /// </summary>
        /// <param name="parameter">The parameter for the command.</param>
        /// <returns>true if the Execute method can be executed otherwise false.</returns>
        public override bool CanExecute(T? parameter)
        {
            return _canExecute?.Invoke(parameter) ?? base.CanExecute(parameter);
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">The parameter for the command.</param>
        public override void Execute(T? parameter)
        {
            _execute?.Invoke(parameter);
        }

        /// <summary>
        /// Raises the CanExecuteChanged Event. So the UI gets notified that the CanExecute method could changed its return value.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            RaiseCanExecuteChanged(this, new EventArgs());
        }
    }
}
