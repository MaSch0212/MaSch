﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MaSch.Core.Extensions;

namespace MaSch.Presentation.Wpf.Commands
{
    /// <summary>
    /// Defines an asynchronous command.
    /// </summary>
    /// <seealso cref="ICommand" />
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Does not make sense in this case.")]
    public interface IAsyncCommand : ICommand
    {
        /// <summary>
        /// Executes the command asynchronously (awaitable).
        /// </summary>
        /// <param name="parameter">The parameter for the command. Is not used in this command, so it should be null.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task ExecuteAsync(object? parameter);
    }

    /// <summary>
    /// This class is an easy way to implement an asynchronous command without parameters that can be executed asynchronously.
    /// </summary>
    public abstract class AsyncCommandBase : IAsyncCommand
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
        /// Executes the command asynchronously.
        /// </summary>
        /// <param name="parameter">The parameter for the command. Is not used in this command, so it should be null.</param>
        public async void Execute(object? parameter) => await ExecuteAsync(parameter);

        /// <summary>
        /// Executes the command asynchronously (awaitable).
        /// </summary>
        /// <param name="parameter">The parameter for the command. Is not used in this command, so it should be null.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task ExecuteAsync(object? parameter = null)
        {
            if (IgnoreCanExecuteOnExecute || CanExecute())
                await Execute();
        }

        /// <summary>
        /// Raises the CanExecuteChanged Event. So the UI gets notified that the CanExecute method could changed its return value.
        /// </summary>
        /// <param name="sender">The sender for the event.</param>
        /// <param name="e">The event arguments for the event.</param>
        protected void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            Application.Current?.Dispatcher?.Invoke(() => CanExecuteChanged?.Invoke(sender, e));
        }

        /// <summary>
        /// Gets a value indicating whether to ignore the result of the <see cref="CanExecute(object)"/> method.
        /// </summary>
        protected virtual bool IgnoreCanExecuteOnExecute => false;

        /// <summary>
        /// Checks if the Execute method can be executed.
        /// </summary>
        /// <returns><c>true</c>.</returns>
        public virtual bool CanExecute() => true;

        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public abstract Task Execute();

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncCommandBase"/> class.
        /// </summary>
        /// <param name="subscribeRequerySuggested">if set to <c>true</c> the <see cref="CommandManager.RequerySuggested"/> event is subscribed.</param>
        protected AsyncCommandBase(bool subscribeRequerySuggested = false)
        {
            if (subscribeRequerySuggested)
                CommandManager.RequerySuggested += (s, e) => RaiseCanExecuteChanged(this, new EventArgs());
        }
    }

    /// <summary>
    /// This class is an easy way to implement an asynchronous command with parameters of a specific type.
    /// </summary>
    /// <typeparam name="T">The parameter type for this <see cref="IAsyncCommand"/>.</typeparam>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Generic representation can be in same file.")]
    public abstract class AsyncCommandBase<T> : IAsyncCommand
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
        /// Executes the command asynchronously.
        /// </summary>
        /// <param name="parameter">The parameter for the command.</param>
        public async void Execute(object? parameter) => await ExecuteAsync(parameter);

        /// <summary>
        /// Executes the command asynchronously (awaitable).
        /// </summary>
        /// <param name="parameter">The parameter for the command. Is not used in this command, so it should be null.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task ExecuteAsync(object? parameter)
        {
            var tParam = GetParameterValue(parameter);
            if (IgnoreCanExecuteOnExecute || CanExecute(tParam))
                await Execute(tParam);
        }

        /// <summary>
        /// Raises the CanExecuteChanged Event. So the UI gets notified that the CanExecute method could changed its return value.
        /// </summary>
        /// <param name="sender">The sender for the event.</param>
        /// <param name="e">The event arguments for the event.</param>
        protected void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            Application.Current?.Dispatcher?.Invoke(() => CanExecuteChanged?.Invoke(sender, e));
        }

        /// <summary>
        /// Gets a value indicating whether to ignore the result of the <see cref="CanExecute(object)"/> method.
        /// </summary>
        protected virtual bool IgnoreCanExecuteOnExecute => false;

        /// <summary>
        /// Gets a value indicating whether to throw an exception when the wrong parameter type is given.
        /// </summary>
        protected virtual bool ThrowExceptionOnWrongParamType => true;

        /// <summary>
        /// Checks if the Execute method can be executed.
        /// </summary>
        /// <param name="parameter">The parameter for the command.</param>
        /// <returns>true if the Execute method can be executed otherwise false.</returns>
        public virtual bool CanExecute(T? parameter) => true;

        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        /// <param name="parameter">The parameter for the command.</param>
        /// <returns>a task that does the execution.</returns>
        public abstract Task Execute(T? parameter);

        private T? GetParameterValue(object? parameter)
        {
            if (ThrowExceptionOnWrongParamType)
                return (T?)parameter;
            else
                return parameter?.GetType().IsCastableTo(typeof(T)) == true ? (T)parameter : default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncCommandBase{T}"/> class.
        /// </summary>
        /// <param name="subscribeRequerySuggested">if set to <c>true</c> the <see cref="CommandManager.RequerySuggested"/> event is subscribed.</param>
        protected AsyncCommandBase(bool subscribeRequerySuggested = false)
        {
            if (subscribeRequerySuggested)
                CommandManager.RequerySuggested += (s, e) => RaiseCanExecuteChanged(this, new EventArgs());
        }
    }
}
