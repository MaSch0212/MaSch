using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace MaSch.Test.Assertion
{
    /// <summary>
    /// Base class for assertion classes to use in unit tests.
    /// </summary>
    public abstract class AssertBase
    {
        /// <summary>
        /// Gets the assert name prefix that is shown in custom assertion messages.
        /// </summary>
        protected abstract string? AssertNamePrefix { get; }

        /// <summary>
        /// Throws an error that an assertion failed.
        /// </summary>
        /// <param name="skipStackFrames">The stack frames to skip when determining the assertion name.</param>
        /// <param name="message">The message.</param>
        /// <param name="values">The values.</param>
        public void ThrowAssertError(int skipStackFrames, string? message, params (string Name, object? Value)[] values)
        {
            var builder = new StringBuilder();
            builder.Append(AssertNamePrefix)
                   .Append(AssertNamePrefix == null ? string.Empty : ".")
                   .Append(new StackFrame(skipStackFrames + 1).GetMethod()?.Name)
                   .Append(" failed.");

            foreach (var (name, value) in values)
            {
                builder.Append(' ')
                       .Append(name)
                       .Append(":<")
                       .Append(value ?? "(null)")
                       .Append(">.");
            }

            if (!string.IsNullOrWhiteSpace(message))
                builder.Append(' ').Append(message);

            HandleFailedAssertion(builder.ToString());
        }

        /// <summary>
        /// Runs a simple assertion and throws an error when the specified condition is false.
        /// </summary>
        /// <param name="message">The message to include in the exception when condition is not met. The message is shown in test results.</param>
        /// <param name="assertFunction">The assert condition function.</param>
        public void RunAssertion(string? message, Func<bool> assertFunction)
        {
            if (!assertFunction())
                ThrowAssertError(1, message);
        }

        /// <summary>
        /// Runs a simple assertion and throws an error when the specified condition is false.
        /// </summary>
        /// <typeparam name="TExpected">The type of the expected value.</typeparam>
        /// <typeparam name="TActual">The type of the actual value.</typeparam>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="message">The message to include in the exception when condition is not met. The message is shown in test results.</param>
        /// <param name="assertFunction">The assert condition function.</param>
        public void RunAssertion<TExpected, TActual>(TExpected expected, TActual actual, string? message, Func<TExpected, TActual, bool> assertFunction)
        {
            if (!assertFunction(expected, actual))
                ThrowAssertError(1, message, ("Expected", expected), ("Actual", actual));
        }

        /// <summary>
        /// Runs a simple assertion and throws an error when the specified condition is true.
        /// </summary>
        /// <typeparam name="TNotExpected">The type of the expected value.</typeparam>
        /// <typeparam name="TActual">The type of the actual value.</typeparam>
        /// <param name="notExpected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="message">The message to include in the exception when condition is not met. The message is shown in test results.</param>
        /// <param name="assertFunction">The assert condition function.</param>
        public void RunNegatedAssertion<TNotExpected, TActual>(TNotExpected notExpected, TActual actual, string? message, Func<TNotExpected, TActual, bool> assertFunction)
        {
            if (assertFunction(notExpected, actual))
                ThrowAssertError(1, message, ("NotExpected", notExpected), ("Actual", actual));
        }

        /// <summary>
        /// Catches <see cref="AssertFailedException"/>s that are potentially thrown by an action and handeled using the <see cref="HandleFailedAssertion(string)"/> method.
        /// </summary>
        /// <typeparam name="T">The type of the result of the action.</typeparam>
        /// <param name="action">The action.</param>
        /// <returns>The result of the action.</returns>
        public T CatchAssertException<T>(Func<T> action)
        {
            try
            {
                return action();
            }
            catch (AssertFailedException ex)
            {
                HandleFailedAssertion(ex.Message);
                return default!;
            }
        }

        /// <summary>
        /// Catches <see cref="AssertFailedException"/>s that are potentially thrown by an action and handeled using the <see cref="HandleFailedAssertion(string)"/> method.
        /// </summary>
        /// <typeparam name="T">The type of the result of the action.</typeparam>
        /// <param name="action">The action.</param>
        /// <returns>The result of the action.</returns>
        public async Task<T> CatchAssertException<T>(Func<Task<T>> action)
        {
            try
            {
                return await action();
            }
            catch (AssertFailedException ex)
            {
                HandleFailedAssertion(ex.Message);
                return default!;
            }
        }

        /// <summary>
        /// Catches <see cref="AssertFailedException"/>s that are potentially thrown by an action and handeled using the <see cref="HandleFailedAssertion(string)"/> method.
        /// </summary>
        /// <param name="action">The action.</param>
        public void CatchAssertException(Action action)
        {
            try
            {
                action();
            }
            catch (AssertFailedException ex)
            {
                HandleFailedAssertion(ex.Message);
            }
        }

        /// <summary>
        /// Catches <see cref="AssertFailedException"/>s that are potentially thrown by an action and handeled using the <see cref="HandleFailedAssertion(string)"/> method.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task CatchAssertException(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (AssertFailedException ex)
            {
                HandleFailedAssertion(ex.Message);
            }
        }

        /// <summary>
        /// Handles a failed assertion.
        /// </summary>
        /// <param name="message">The message to show in the error log.</param>
        protected abstract void HandleFailedAssertion(string message);
    }
}
