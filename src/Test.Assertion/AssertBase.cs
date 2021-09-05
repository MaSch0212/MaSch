using System;
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if MSTEST
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace MaSch.Test.Assertion
{
    /// <summary>
    /// Base class for assertion classes to use in unit tests.
    /// </summary>
    public abstract partial class AssertBase
    {
        /// <summary>
        /// Gets the assert name prefix that is shown in custom assertion messages.
        /// </summary>
        protected abstract string? AssertNamePrefix { get; }

        /// <summary>
        /// Throws an error that an assertion failed.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="values">The values.</param>
        [DoesNotReturn]
        public virtual void ThrowAssertError(string? message, params (string Name, object? Value)?[]? values)
        {
            ThrowAssertError(1, message, values);
        }

        /// <summary>
        /// Throws an error that an assertion failed.
        /// </summary>
        /// <param name="skipStackFrames">The stack frames to skip when determining the assertion name.</param>
        /// <param name="message">The message.</param>
        /// <param name="values">The values.</param>
        [DoesNotReturn]
        public virtual void ThrowAssertError(int skipStackFrames, string? message, params (string Name, object? Value)?[]? values)
        {
            if (skipStackFrames < 0)
                throw new ArgumentOutOfRangeException(nameof(skipStackFrames), "The parameter can not be smaller than zero.");
            ThrowAssertError(new StackFrame(skipStackFrames + 1).GetMethod()?.Name, message, values);
        }

        /// <summary>
        /// Throws an error that an assertion failed.
        /// </summary>
        /// <param name="assertMethodName">The name of the assert method.</param>
        /// <param name="message">The message.</param>
        /// <param name="values">The values.</param>
        [DoesNotReturn]
        public virtual void ThrowAssertError(string? assertMethodName, string? message, params (string Name, object? Value)?[]? values)
        {
            var builder = new StringBuilder();
            _ = builder.Append(AssertNamePrefix)
                   .Append(AssertNamePrefix == null ? string.Empty : ".")
                   .Append(assertMethodName ?? "<unknown>")
                   .Append(" failed.");

            foreach (var (name, value) in values?.Where(x => x.HasValue).Select(x => x!.Value) ?? Array.Empty<(string, object?)>())
            {
                _ = builder.Append(' ')
                       .Append(name)
                       .Append(":<")
                       .Append(FormatObject(value, 0))
                       .Append(">.");
            }

            if (!string.IsNullOrWhiteSpace(message))
                _ = builder.Append(' ').Append(message);

            HandleFailedAssertion(builder.ToString());
        }

        /// <summary>
        /// Runs a simple assertion and throws an error when the specified condition is false.
        /// </summary>
        /// <param name="message">The message to include in the exception when condition is not met. The message is shown in test results.</param>
        /// <param name="assertFunction">The assert condition function.</param>
        public virtual void RunAssertion(string? message, Func<bool> assertFunction)
        {
            if (!assertFunction())
                ThrowAssertError(1, message);
        }

        /// <summary>
        /// Runs a simple assertion and throws an error when the specified condition is false.
        /// </summary>
        /// <typeparam name="T">The type of the actual value.</typeparam>
        /// <param name="actual">The actual value.</param>
        /// <param name="message">The message to include in the exception when condition is not met. The message is shown in test results.</param>
        /// <param name="assertFunction">The assert condition function.</param>
        public virtual void RunAssertion<T>(T actual, string? message, Func<T, bool> assertFunction)
        {
            if (!assertFunction(actual))
                ThrowAssertError(1, message, ("Actual", actual));
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
        public virtual void RunAssertion<TExpected, TActual>(TExpected expected, TActual actual, string? message, Func<TExpected, TActual, bool> assertFunction)
        {
            if (!assertFunction(expected, actual))
                ThrowAssertError(1, message, ("Expected", expected), ("Actual", actual));
        }

        /// <summary>
        /// Runs a simple assertion and throws an error when the specified condition is true.
        /// </summary>
        /// <param name="message">The message to include in the exception when condition is not met. The message is shown in test results.</param>
        /// <param name="assertFunction">The assert condition function.</param>
        public virtual void RunNegatedAssertion(string? message, Func<bool> assertFunction)
        {
            if (assertFunction())
                ThrowAssertError(1, message);
        }

        /// <summary>
        /// Runs a simple assertion and throws an error when the specified condition is true.
        /// </summary>
        /// <typeparam name="T">The type of the actual value.</typeparam>
        /// <param name="actual">The actual value.</param>
        /// <param name="message">The message to include in the exception when condition is not met. The message is shown in test results.</param>
        /// <param name="assertFunction">The assert condition function.</param>
        public virtual void RunNegatedAssertion<T>(T actual, string? message, Func<T, bool> assertFunction)
        {
            if (assertFunction(actual))
                ThrowAssertError(1, message, ("Actual", actual));
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
        public virtual void RunNegatedAssertion<TNotExpected, TActual>(TNotExpected notExpected, TActual actual, string? message, Func<TNotExpected, TActual, bool> assertFunction)
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
        public virtual T CatchAssertException<T>(Func<T> action)
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
        public virtual async Task<T> CatchAssertException<T>(Func<Task<T>> action)
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
        public virtual void CatchAssertException(Action action)
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
        public virtual async Task CatchAssertException(Func<Task> action)
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
        [DoesNotReturn]
        protected abstract void HandleFailedAssertion(string message);

        private static string FormatObject(object? obj, int indentation)
        {
            return obj switch
            {
                null => "(null)",
                string str => str,
                IEnumerable enumerable => FormatEnumerable(enumerable, indentation + 1),
                IFormattable formattable => formattable.ToString(null, CultureInfo.InvariantCulture),
                _ => obj.ToString() ?? "(null)",
            };
        }

        private static string FormatEnumerable(IEnumerable enumerable, int indentation)
        {
            var result = new StringBuilder("[");

            bool isFirst = true;
            foreach (object item in enumerable)
            {
                if (!isFirst)
                {
                    _ = result.Append(',');
                }

                _ = result.AppendLine().Append('\t', indentation).Append(FormatObject(item, indentation));
                isFirst = false;
            }

            if (!isFirst)
                _ = result.AppendLine();
            _ = result.Append('\t', indentation - 1).Append(']');
            return result.ToString();
        }
    }
}
