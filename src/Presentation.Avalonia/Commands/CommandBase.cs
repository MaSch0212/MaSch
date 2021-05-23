using Avalonia.Threading;
using MaSch.Core.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

namespace MaSch.Presentation.Avalonia.Commands
{
    /// <summary>
    /// This class is an easy way to implement a command without parameters.
    /// </summary>
    public abstract class CommandBase : ICommand
    {
        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Checks if the Execute method can be executed.
        /// </summary>
        /// <param name="parameter">The parameter for the command. Is not used in this command, so it should be null.</param>
        /// <returns>true if the Execute method can be executed otherwise false.</returns>
        public bool CanExecute(object? parameter) => CanExecute();

        /// <summary>
        /// Checks if the Execute method can be executed.
        /// </summary>
        /// <returns><c>true</c>.</returns>
        public virtual bool CanExecute() => true;

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">The parameter for the command. Is not used in this command, so it should be null.</param>
        public void Execute(object? parameter)
        {
            if (IgnoreCanExecuteOnExecute || CanExecute())
                Execute();
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// Raises the CanExecuteChanged Event. So the UI gets notified that the CanExecute method could changed its return value.
        /// </summary>
        /// <param name="sender">The sender for the event.</param>
        /// <param name="e">The event arguments for the event.</param>
        protected void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            Dispatcher.UIThread.InvokeAsync(() => CanExecuteChanged?.Invoke(sender, e));
        }

        /// <summary>
        /// Gets a value indicating whether to ignore the result of the <see cref="CanExecute(object)"/> method.
        /// </summary>
        protected virtual bool IgnoreCanExecuteOnExecute => false;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBase"/> class.
        /// </summary>
        protected CommandBase()
        {
        }
    }

    /// <summary>
    /// This class is an easy way to implement a command with parameters of a specific type.
    /// </summary>
    /// <typeparam name="T">The parameter type for this <see cref="IAsyncCommand"/>.</typeparam>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Generic representation can be in same file.")]
    public abstract class CommandBase<T> : ICommand
    {
        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Checks if the Execute method can be executed.
        /// </summary>
        /// <param name="parameter">The parameter for the command.</param>
        /// <returns>true if the Execute method can be executed otherwise false.</returns>
        public bool CanExecute(object? parameter) => CanExecute(GetParameterValue(parameter));

        /// <summary>
        /// Checks if the Execute method can be executed.
        /// </summary>
        /// <param name="parameter">The parameter for the command.</param>
        /// <returns>true if the Execute method can be executed otherwise false.</returns>
        public virtual bool CanExecute(T? parameter) => true;

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">The parameter for the command.</param>
        public void Execute(object? parameter)
        {
            var tParam = GetParameterValue(parameter);
            if (IgnoreCanExecuteOnExecute || CanExecute(tParam))
                Execute(tParam);
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">The parameter for the command.</param>
        public abstract void Execute(T? parameter);

        /// <summary>
        /// Raises the CanExecuteChanged Event. So the UI gets notified that the CanExecute method could changed its return value.
        /// </summary>
        /// <param name="sender">The sender for the event.</param>
        /// <param name="e">The event arguments for the event.</param>
        protected void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            Dispatcher.UIThread.InvokeAsync(() => CanExecuteChanged?.Invoke(sender, e));
        }

        /// <summary>
        /// Gets a value indicating whether to ignore the result of the <see cref="CanExecute(object)"/> method.
        /// </summary>
        protected virtual bool IgnoreCanExecuteOnExecute => false;

        /// <summary>
        /// Gets a value indicating whether to throw an exception when the wrong parameter type is given.
        /// </summary>
        protected virtual bool ThrowExceptionOnWrongParamType => true;

        private T? GetParameterValue(object? parameter)
        {
            if (ThrowExceptionOnWrongParamType)
                return (T?)parameter;
            return parameter?.GetType().IsCastableTo(typeof(T)) == true ? (T)parameter : default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBase{T}"/> class.
        /// </summary>
        protected CommandBase()
        {
        }
    }
}
