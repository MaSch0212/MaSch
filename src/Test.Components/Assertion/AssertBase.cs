using MaSch.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
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

        #region MSTest Assert Methods

        /// <summary>
        /// Tests whether the specified condition is true and throws an exception if the condition is false.
        /// </summary>
        /// <param name="condition">The condition the test expects to be true.</param>
        public virtual void IsTrue(bool condition)
            => IsTrue(condition, null);

        /// <summary>
        /// Tests whether the specified condition is true and throws an exception if the condition is false.
        /// </summary>
        /// <param name="condition">The condition the test expects to be true.</param>
        public virtual void IsTrue(bool? condition)
            => IsTrue(condition == true, null);

        /// <summary>
        /// Tests whether the specified condition is true and throws an exception if the condition is false.
        /// </summary>
        /// <param name="condition">The condition the test expects to be true.</param>
        /// <param name="message">The message to include in the exception when <paramref name="condition" /> is false. The message is shown in test results.</param>
        public virtual void IsTrue(bool condition, string? message)
            => RunAssertion(message, () => condition);

        /// <summary>
        /// Tests whether the specified condition is true and throws an exception if the condition is false.
        /// </summary>
        /// <param name="condition">The condition the test expects to be true.</param>
        /// <param name="message">The message to include in the exception when <paramref name="condition" /> is false. The message is shown in test results.</param>
        public virtual void IsTrue(bool? condition, string? message)
            => IsTrue(condition == true, message);

        /// <summary>
        /// Tests whether the specified condition is false and throws an exception if the condition is true.
        /// </summary>
        /// <param name="condition">The condition the test expects to be false.</param>
        public virtual void IsFalse(bool condition)
            => IsFalse(condition, null);

        /// <summary>
        /// Tests whether the specified condition is false and throws an exception if the condition is true.
        /// </summary>
        /// <param name="condition">The condition the test expects to be false.</param>
        public virtual void IsFalse(bool? condition)
            => IsFalse(condition != false);

        /// <summary>
        /// Tests whether the specified condition is false and throws an exception if the condition is true.
        /// </summary><param name="condition">The condition the test expects to be false.</param>
        /// <param name="message">The message to include in the exception when <paramref name="condition" /> is true. The message is shown in test results.</param>
        public virtual void IsFalse(bool condition, string? message)
            => RunAssertion(message, () => !condition);

        /// <summary>
        /// Tests whether the specified condition is false and throws an exception if the condition is true.
        /// </summary>
        /// <param name="condition">The condition the test expects to be false.</param>
        /// <param name="message">The message to include in the exception when <paramref name="condition" /> is true. The message is shown in test results.</param>
        public virtual void IsFalse(bool? condition, string? message)
            => IsFalse(condition != false, message);

        /// <summary>
        /// Tests whether the specified object is null and throws an exception if it is not.
        /// </summary>
        /// <param name="value">The object the test expects to be null.</param>
        public virtual void IsNull(object? value)
            => IsNull(value, null);

        /// <summary>
        /// Tests whether the specified object is null and throws an exception if it is not.
        /// </summary>
        /// <param name="value">The object the test expects to be null.</param>
        /// <param name="message">The message to include in the exception when <paramref name="value" /> is not null. The message is shown in test results.</param>
        public virtual void IsNull(object? value, string? message)
            => RunAssertion(message, () => value == null);

        /// <summary>
        /// Tests whether the specified object is non-null and throws an exception if it is null.
        /// </summary>
        /// <param name="value">The object the test expects not to be null.</param>
        public virtual void IsNotNull([NotNull] object? value)
            => IsNotNull(value, null);

#pragma warning disable CS8777 // Parameter must have a non-null value when exiting.
        /// <summary>
        /// Tests whether the specified object is non-null and throws an exception if it is null.
        /// </summary>
        /// <param name="value">The object the test expects not to be null.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="value" /> is null. The message is shown in test results.</param>
        public virtual void IsNotNull([NotNull] object? value, string? message)
            => RunAssertion(message, () => value != null);
#pragma warning restore CS8777 // Parameter must have a non-null value when exiting.

        /// <summary>
        /// Tests whether the specified objects both refer to the same object and throws an exception if the two inputs do not refer to the same object.
        /// </summary>
        /// <param name="expected">The first object to compare. This is the value the test expects.</param>
        /// <param name="actual">The second object to compare. This is the value produced by the code under test.</param>
        public virtual void AreSame(object? expected, object? actual)
            => AreSame(expected, actual, null);

        /// <summary>
        /// Tests whether the specified objects both refer to the same object and throws an exception if the two inputs do not refer to the same object.
        /// </summary>
        /// <param name="expected">The first object to compare. This is the value the test expects.</param>
        /// <param name="actual">The second object to compare. This is the value produced by the code under test.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> is not the same as <paramref name="expected" />. The message is shown in test results.</param>
        public virtual void AreSame(object? expected, object? actual, string? message)
            => RunAssertion(expected, actual, message, (e, a) => ReferenceEquals(a, e));

        /// <summary>
        /// Tests whether the specified objects refer to different objects and throws an exception if the two inputs refer to the same object.
        /// </summary>
        /// <param name="notExpected">The first object to compare. This is the value the test expects not to match <paramref name="actual" />.</param>
        /// <param name="actual">The second object to compare. This is the value produced by the code under test.</param>
        public virtual void AreNotSame(object? notExpected, object? actual)
            => AreNotSame(notExpected, actual, null);

        /// <summary>
        /// Tests whether the specified objects refer to different objects and throws an exception if the two inputs refer to the same object.
        /// </summary>
        /// <param name="notExpected">The first object to compare. This is the value the test expects not to match <paramref name="actual" />.</param>
        /// <param name="actual">The second object to compare. This is the value produced by the code under test.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> is the same as <paramref name="notExpected" />. The message is shown in test results.</param>
        public virtual void AreNotSame(object? notExpected, object? actual, string? message)
            => RunNegatedAssertion(notExpected, actual, message, (e, a) => ReferenceEquals(a, e));

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are not equal. Different numeric types are treated as unequal even if the logical values are equal. 42L is not equal to 42.
        /// </summary><typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        public virtual void AreEqual<T>(T? expected, T? actual)
            => AreEqual(expected, actual, null);

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are not equal. Different numeric types are treated as unequal even if the logical values are equal. 42L is not equal to 42.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> is not equal to <paramref name="expected" />. The message is shown in test results.</param>
        public virtual void AreEqual<T>(T? expected, T? actual, string? message)
        {
            if (!Equals(actual, expected))
            {
                if (actual != null && expected != null && !actual.GetType().Equals(expected.GetType()))
                    ThrowAssertError(0, message, ("Expected", expected), ("ExpectedType", expected.GetType().FullName), ("Actual", actual), ("ActualType", actual.GetType().FullName));
                else
                    ThrowAssertError(0, message, ("Expected", expected), ("Actual", actual));
            }
        }

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. Different numeric types are treated as unequal even if the logical values are equal. 42L is not equal to 42.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual" />.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        public virtual void AreNotEqual<T>(T? notExpected, T? actual)
            => AreNotEqual(notExpected, actual, null);

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. Different numeric types are treated as unequal even if the logical values are equal. 42L is not equal to 42.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual" />.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> is equal to <paramref name="notExpected" />. The message is shown in test results.</param>
        public virtual void AreNotEqual<T>(T? notExpected, T? actual, string? message)
            => RunNegatedAssertion(notExpected, actual, message, (e, a) => Equals(a, e));

        /// <summary>
        /// Tests whether the specified objects are equal and throws an exception if the two objects are not equal. Different numeric types are treated as unequal even if the logical values are equal. 42L is not equal to 42.
        /// </summary>
        /// <param name="expected">The first object to compare. This is the object the tests expects.</param>
        /// <param name="actual">The second object to compare. This is the object produced by the code under test.</param>
        public virtual void AreEqual(object? expected, object? actual)
            => AreEqual<object?>(expected, actual, null);

        /// <summary>
        /// Tests whether the specified objects are equal and throws an exception if the two objects are not equal. Different numeric types are treated as unequal even if the logical values are equal. 42L is not equal to 42.
        /// </summary>
        /// <param name="expected">The first object to compare. This is the object the tests expects.</param>
        /// <param name="actual">The second object to compare. This is the object produced by the code under test.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> is not equal to <paramref name="expected" />. The message is shown in test results.</param>
        public virtual void AreEqual(object? expected, object? actual, string? message)
            => AreEqual<object?>(expected, actual, message);

        /// <summary>
        /// Tests whether the specified objects are unequal and throws an exception if the two objects are equal. Different numeric types are treated as unequal even if the logical values are equal. 42L is not equal to 42.
        /// </summary>
        /// <param name="notExpected">The first object to compare. This is the value the test expects not to match <paramref name="actual" />.</param>
        /// <param name="actual">The second object to compare. This is the object produced by the code under test.</param>
        public virtual void AreNotEqual(object? notExpected, object? actual)
            => AreNotEqual<object?>(notExpected, actual, null);

        /// <summary>
        /// Tests whether the specified objects are unequal and throws an exception if the two objects are equal. Different numeric types are treated as unequal even if the logical values are equal. 42L is not equal to 42.
        /// </summary>
        /// <param name="notExpected">The first object to compare. This is the value the test expects not to match <paramref name="actual" />.</param>
        /// <param name="actual">The second object to compare. This is the object produced by the code under test.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> is equal to <paramref name="notExpected" />. The message is shown in test results.</param>
        public virtual void AreNotEqual(object? notExpected, object? actual, string? message)
            => AreNotEqual<object?>(notExpected, actual, message);

        /// <summary>
        /// Tests whether the specified floats are equal and throws an exception if they are not equal.
        /// </summary>
        /// <param name="expected">The first float to compare. This is the float the tests expects.</param>
        /// <param name="actual">The second float to compare. This is the float produced by the code under test.</param>
        /// <param name="delta">The required accuracy. An exception will be thrown only if <paramref name="actual" /> is different than <paramref name="expected" /> by more than <paramref name="delta" />.</param>
        public virtual void AreEqual(float expected, float actual, float delta)
            => AreEqual(expected, actual, delta, null);

        /// <summary>
        /// Tests whether the specified floats are equal and throws an exception if they are not equal.
        /// </summary>
        /// <param name="expected">The first float to compare. This is the float the tests expects.</param>
        /// <param name="actual">The second float to compare. This is the float produced by the code under test.</param>
        /// <param name="delta">The required accuracy. An exception will be thrown only if <paramref name="actual" /> is different than <paramref name="expected" /> by more than <paramref name="delta" />.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> is different than <paramref name="expected" /> by more than <paramref name="delta" />. The message is shown in test results.</param>
        public virtual void AreEqual(float expected, float actual, float delta, string? message)
        {
            if (float.IsNaN(expected) || float.IsNaN(actual) || float.IsNaN(delta) || Math.Abs(expected - actual) > delta)
                ThrowAssertError(0, message, ("Expected", expected), ("Actual", actual), ("Delta", delta));
        }

        /// <summary>
        /// Tests whether the specified floats are unequal and throws an exception if they are equal.
        /// </summary>
        /// <param name="notExpected">The first float to compare. This is the float the test expects not to match <paramref name="actual" />.</param>
        /// <param name="actual">The second float to compare. This is the float produced by the code under test.</param>
        /// <param name="delta">The required accuracy. An exception will be thrown only if <paramref name="actual" /> is different than <paramref name="notExpected" /> by at most <paramref name="delta" />.</param>
        public virtual void AreNotEqual(float notExpected, float actual, float delta)
            => AreNotEqual(notExpected, actual, delta, null);

        /// <summary>
        /// Tests whether the specified floats are unequal and throws an exception if they are equal.
        /// </summary>
        /// <param name="notExpected">The first float to compare. This is the float the test expects not to match <paramref name="actual" />.</param>
        /// <param name="actual">The second float to compare. This is the float produced by the code under test.</param>
        /// <param name="delta">The required accuracy. An exception will be thrown only if <paramref name="actual" /> is different than <paramref name="notExpected" /> by at most <paramref name="delta" />.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> is equal to <paramref name="notExpected" /> or different by less than <paramref name="delta" />. The message is shown in test results.</param>
        public virtual void AreNotEqual(float notExpected, float actual, float delta, string? message)
        {
            if (Math.Abs(notExpected - actual) <= delta)
                ThrowAssertError(0, message, ("NotExpected", notExpected), ("Actual", actual), ("Delta", delta));
        }

        /// <summary>
        /// Tests whether the specified doubles are equal and throws an exception if they are not equal.
        /// </summary>
        /// <param name="expected">The first double to compare. This is the double the tests expects.</param>
        /// <param name="actual">The second double to compare. This is the double produced by the code under test.</param>
        /// <param name="delta">The required accuracy. An exception will be thrown only if <paramref name="actual" /> is different than <paramref name="expected" /> by more than <paramref name="delta" />.</param>
        public virtual void AreEqual(double expected, double actual, double delta)
            => AreEqual(expected, actual, delta, null);

        /// <summary>
        /// Tests whether the specified doubles are equal and throws an exception if they are not equal.
        /// </summary>
        /// <param name="expected">The first double to compare. This is the double the tests expects.</param>
        /// <param name="actual">The second double to compare. This is the double produced by the code under test.</param>
        /// <param name="delta">The required accuracy. An exception will be thrown only if <paramref name="actual" /> is different than <paramref name="expected" /> by more than <paramref name="delta" />.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> is different than <paramref name="expected" /> by more than <paramref name="delta" />. The message is shown in test results.</param>
        public virtual void AreEqual(double expected, double actual, double delta, string? message)
        {
            if (double.IsNaN(expected) || double.IsNaN(actual) || double.IsNaN(delta) || Math.Abs(expected - actual) > delta)
                ThrowAssertError(0, message, ("Expected", expected), ("Actual", actual), ("Delta", delta));
        }

        /// <summary>
        /// Tests whether the specified doubles are unequal and throws an exception if they are equal.
        /// </summary>
        /// <param name="notExpected">The first double to compare. This is the double the test expects not to match <paramref name="actual" />.</param>
        /// <param name="actual">The second double to compare. This is the double produced by the code under test.</param>
        /// <param name="delta">The required accuracy. An exception will be thrown only if <paramref name="actual" /> is different than <paramref name="notExpected" /> by at most <paramref name="delta" />.</param>
        public virtual void AreNotEqual(double notExpected, double actual, double delta)
            => AreNotEqual(notExpected, actual, delta, null);

        /// <summary>
        /// Tests whether the specified doubles are unequal and throws an exception if they are equal.
        /// </summary>
        /// <param name="notExpected">The first double to compare. This is the double the test expects not to match <paramref name="actual" />.</param>
        /// <param name="actual">The second double to compare. This is the double produced by the code under test.</param>
        /// <param name="delta">The required accuracy. An exception will be thrown only if <paramref name="actual" /> is different than <paramref name="notExpected" /> by at most <paramref name="delta" />.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> is equal to <paramref name="notExpected" /> or different by less than <paramref name="delta" />. The message is shown in test results.</param>
        public virtual void AreNotEqual(double notExpected, double actual, double delta, string? message)
        {
            if (Math.Abs(notExpected - actual) <= delta)
                ThrowAssertError(0, message, ("NotExpected", notExpected), ("Actual", actual), ("Delta", delta));
        }

        /// <summary>
        /// Tests whether the specified strings are equal and throws an exception if they are not equal. The invariant culture is used for the comparison.
        /// </summary>
        /// <param name="expected">The first string to compare. This is the string the tests expects.</param>
        /// <param name="actual">The second string to compare. This is the string produced by the code under test.</param>
        /// <param name="ignoreCase">A Boolean indicating a case-sensitive or insensitive comparison. (true indicates a case-insensitive comparison.)</param>
        public virtual void AreEqual(string? expected, string? actual, bool ignoreCase)
            => AreEqual(expected, actual, ignoreCase, CultureInfo.InvariantCulture, null);

        /// <summary>
        /// Tests whether the specified strings are equal and throws an exception if they are not equal. The invariant culture is used for the comparison.
        /// </summary>
        /// <param name="expected">The first string to compare. This is the string the tests expects.</param>
        /// <param name="actual">The second string to compare. This is the string produced by the code under test.</param>
        /// <param name="ignoreCase">A Boolean indicating a case-sensitive or insensitive comparison. (true indicates a case-insensitive comparison.)</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> is not equal to <paramref name="expected" />. The message is shown in test results.</param>
        public virtual void AreEqual(string? expected, string? actual, bool ignoreCase, string? message)
            => AreEqual(expected, actual, ignoreCase, CultureInfo.InvariantCulture, message);

        /// <summary>
        /// Tests whether the specified strings are equal and throws an exception if they are not equal.
        /// </summary>
        /// <param name="expected">The first string to compare. This is the string the tests expects.</param>
        /// <param name="actual">The second string to compare. This is the string produced by the code under test.</param>
        /// <param name="ignoreCase">A Boolean indicating a case-sensitive or insensitive comparison. (true indicates a case-insensitive comparison.)</param>
        /// <param name="culture">A CultureInfo object that supplies culture-specific comparison information.</param>
        public virtual void AreEqual(string? expected, string? actual, bool ignoreCase, CultureInfo culture)
            => AreEqual(expected, actual, ignoreCase, culture, null);

        /// <summary>
        /// Tests whether the specified strings are equal and throws an exception if they are not equal.
        /// </summary>
        /// <param name="expected">The first string to compare. This is the string the tests expects.</param>
        /// <param name="actual">The second string to compare. This is the string produced by the code under test.</param>
        /// <param name="ignoreCase">A Boolean indicating a case-sensitive or insensitive comparison. (true indicates a case-insensitive comparison.)</param>
        /// <param name="culture">A CultureInfo object that supplies culture-specific comparison information.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> is not equal to <paramref name="expected" />. The message is shown in test results.</param>
        public virtual void AreEqual(string? expected, string? actual, bool ignoreCase, CultureInfo culture, string? message)
            => RunAssertion(expected, actual, message, (e, a) => culture.CompareInfo.Compare(a, e, ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None) != 0);

        /// <summary>
        /// Tests whether the specified strings are unequal and throws an exception if they are equal. The invariant culture is used for the comparison.
        /// </summary>
        /// <param name="notExpected">The first string to compare. This is the string the test expects not to match <paramref name="actual" />.</param>
        /// <param name="actual">The second string to compare. This is the string produced by the code under test.</param>
        /// <param name="ignoreCase">A Boolean indicating a case-sensitive or insensitive comparison. (true indicates a case-insensitive comparison.)</param>
        public virtual void AreNotEqual(string? notExpected, string? actual, bool ignoreCase)
            => AreNotEqual(notExpected, actual, ignoreCase, CultureInfo.InvariantCulture, null);

        /// <summary>
        /// Tests whether the specified strings are unequal and throws an exception if they are equal. The invariant culture is used for the comparison.
        /// </summary>
        /// <param name="notExpected">The first string to compare. This is the string the test expects not to match <paramref name="actual" />.</param>
        /// <param name="actual">The second string to compare. This is the string produced by the code under test.</param>
        /// <param name="ignoreCase">A Boolean indicating a case-sensitive or insensitive comparison. (true indicates a case-insensitive comparison.)</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> is equal to <paramref name="notExpected" />. The message is shown in test results.</param>
        public virtual void AreNotEqual(string? notExpected, string? actual, bool ignoreCase, string? message)
            => AreNotEqual(notExpected, actual, ignoreCase, CultureInfo.InvariantCulture, message);

        /// <summary>
        /// Tests whether the specified strings are unequal and throws an exception if they are equal.
        /// </summary>
        /// <param name="notExpected">The first string to compare. This is the string the test expects not to match <paramref name="actual" />.</param>
        /// <param name="actual">The second string to compare. This is the string produced by the code under test.</param>
        /// <param name="ignoreCase">A Boolean indicating a case-sensitive or insensitive comparison. (true indicates a case-insensitive comparison.)</param>
        /// <param name="culture">A CultureInfo object that supplies culture-specific comparison information.</param>
        public virtual void AreNotEqual(string? notExpected, string? actual, bool ignoreCase, CultureInfo culture)
            => AreNotEqual(notExpected, actual, ignoreCase, culture, null);

        /// <summary>
        /// Tests whether the specified strings are unequal and throws an exception if they are equal.
        /// </summary>
        /// <param name="notExpected">The first string to compare. This is the string the test expects not to match <paramref name="actual" />.</param>
        /// <param name="actual">The second string to compare. This is the string produced by the code under test.</param>
        /// <param name="ignoreCase">A Boolean indicating a case-sensitive or insensitive comparison. (true indicates a case-insensitive comparison.)</param>
        /// <param name="culture">A CultureInfo object that supplies culture-specific comparison information.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> is equal to <paramref name="notExpected" />. The message is shown in test results.</param>
        public virtual void AreNotEqual(string? notExpected, string? actual, bool ignoreCase, CultureInfo culture, string? message)
            => RunNegatedAssertion(notExpected, actual, message, (e, a) => culture.CompareInfo.Compare(a, e, ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None) != 0);

        /// <summary>
        /// Tests whether the specified object is an instance of the expected type and throws an exception if the expected type is not in the
        /// inheritance hierarchy of the object.
        /// </summary>
        /// <param name="value">The object the test expects to be of the specified type.</param>
        /// <param name="expectedType">The expected type of <paramref name="value" />.</param>
        public virtual void IsInstanceOfType([NotNull] object? value, Type expectedType)
            => IsInstanceOfType(value, expectedType, null);

#pragma warning disable CS8777 // Parameter must have a non-null value when exiting.
        /// <summary>
        /// Tests whether the specified object is an instance of the expected type and throws an exception if the expected type is not in the
        /// inheritance hierarchy of the object.
        /// </summary>
        /// <param name="value">The object the test expects to be of the specified type.</param>
        /// <param name="expectedType">The expected type of <paramref name="value" />.</param>
        /// <param name="message">The message to include in the exception when <paramref name="value" /> is not an instance of <paramref name="expectedType" />. The message is shown in test results.</param>
        public virtual void IsInstanceOfType([NotNull] object? value, Type expectedType, string? message)
            => RunAssertion(expectedType, value?.GetType(), message, (e, a) => e.IsAssignableFrom(a));
#pragma warning restore CS8777 // Parameter must have a non-null value when exiting.

        /// <summary>
        /// Tests whether the specified object is not an instance of the wrong type and throws an exception if the specified type is in the
        /// inheritance hierarchy of the object.
        /// </summary>
        /// <param name="value">The object the test expects not to be of the specified type.</param>
        /// <param name="wrongType">The type that <paramref name="value" /> should not be.</param>
        public virtual void IsNotInstanceOfType(object? value, Type wrongType)
            => IsNotInstanceOfType(value, wrongType, null);

        /// <summary>
        /// Tests whether the specified object is not an instance of the wrong type and throws an exception if the specified type is in the
        /// inheritance hierarchy of the object.
        /// </summary>
        /// <param name="value">The object the test expects not to be of the specified type.</param>
        /// <param name="wrongType">The type that <paramref name="value" /> should not be.</param>
        /// <param name="message">The message to include in the exception when <paramref name="value" /> is an instance of <paramref name="wrongType" />. The message is shown in test results.</param>
        public virtual void IsNotInstanceOfType(object? value, Type wrongType, string? message)
            => RunNegatedAssertion(wrongType, value?.GetType(), message, (e, a) => e.IsAssignableFrom(a));

        /// <summary>
        /// Throws an AssertFailedException.
        /// </summary>
        public virtual void Fail()
            => ThrowAssertError(0, null);

        /// <summary>
        /// Throws an AssertFailedException.
        /// </summary>
        /// <param name="message">The message to include in the exception. The message is shown in test results.</param>
        public virtual void Fail(string? message)
            => ThrowAssertError(0, message);

        /// <summary>
        /// Tests whether the code specified by delegate <paramref name="action" /> throws exact given exception of type <typeparamref name="T" /> (and not of derived type)
        /// and throws <c>AssertFailedException</c> if code does not throws exception or throws exception of type other than <typeparamref name="T" />.
        /// </summary>
        /// <param name="action">Delegate to code to be tested and which is expected to throw exception.</param>
        /// <typeparam name="T">Type of exception expected to be thrown.</typeparam>
        /// <returns>The exception that was thrown.</returns>
        public virtual T ThrowsException<T>(Action action)
            where T : Exception
            => ThrowsException<T>(action, null);

        /// <summary>
        /// Tests whether the code specified by delegate <paramref name="action" /> throws exact given exception of type <typeparamref name="T" /> (and not of derived type)
        /// and throws <c>AssertFailedException</c> if code does not throws exception or throws exception of type other than <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">Type of exception expected to be thrown.</typeparam>
        /// <param name="action">Delegate to code to be tested and which is expected to throw exception.</param>
        /// <param name="message">The message to include in the exception when <paramref name="action" /> does not throws exception of type <typeparamref name="T" />.</param>
        /// <returns>The exception that was thrown.</returns>
        public virtual T ThrowsException<T>(Action action, string? message)
            where T : Exception
        {
            Guard.NotNull(action, nameof(action));

            Exception? exception = null;
            try
            {
                action();
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            if (exception == null || !typeof(T).Equals(exception.GetType()))
                ThrowAssertError(0, message, ("ExpectedException", typeof(T).Name), ("ActualException", exception?.GetType().Name));

            return (exception as T)!;
        }

        /// <summary>
        /// Tests whether the code specified by delegate <paramref name="action" /> throws exact given exception of type <typeparamref name="T" /> (and not of derived type)
        /// and throws <c>AssertFailedException</c> if code does not throws exception or throws exception of type other than <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">Type of exception expected to be thrown.</typeparam>
        /// <param name="action">Delegate to code to be tested and which is expected to throw exception.</param>
        /// <returns>The exception that was thrown.</returns>
        public virtual T ThrowsException<T>(Func<object?> action)
            where T : Exception
            => ThrowsException<T>(() => { action(); }, null);

        /// <summary>
        /// Tests whether the code specified by delegate <paramref name="action" /> throws exact given exception of type <typeparamref name="T" /> (and not of derived type)
        /// and throws <c>AssertFailedException</c> if code does not throws exception or throws exception of type other than <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">Type of exception expected to be thrown.</typeparam>
        /// <param name="action">Delegate to code to be tested and which is expected to throw exception.</param>
        /// <param name="message">The message to include in the exception when <paramref name="action" /> does not throws exception of type <typeparamref name="T" />.</param>
        /// <returns>The exception that was thrown.</returns>
        public virtual T ThrowsException<T>(Func<object?> action, string? message)
            where T : Exception
            => ThrowsException<T>(() => { action(); }, message);

        /// <summary>
        /// Tests whether the code specified by delegate <paramref name="action" /> throws exact given exception of type <typeparamref name="T" /> (and not of derived type)
        /// and throws <c>AssertFailedException</c> if code does not throws exception or throws exception of type other than <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">Type of exception expected to be thrown.</typeparam>
        /// <param name="action">Delegate to code to be tested and which is expected to throw exception.</param>
        /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> executing the delegate.</returns>
        public virtual async Task<T> ThrowsExceptionAsync<T>(Func<Task> action)
            where T : Exception
            => await ThrowsExceptionAsync<T>(action, null);

        /// <summary>
        /// Tests whether the code specified by delegate <paramref name="action" /> throws exact given exception of type <typeparamref name="T" /> (and not of derived type)
        /// and throws <c>AssertFailedException</c> if code does not throws exception or throws exception of type other than <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">Type of exception expected to be thrown.</typeparam>
        /// <param name="action">Delegate to code to be tested and which is expected to throw exception.</param>
        /// <param name="message">The message to include in the exception when <paramref name="action" /> does not throws exception of type <typeparamref name="T" />.</param>
        /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> executing the delegate.</returns>
        public virtual async Task<T> ThrowsExceptionAsync<T>(Func<Task> action, string? message)
            where T : Exception
        {
            Guard.NotNull(action, nameof(action));

            Exception? exception = null;
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            if (exception == null || !typeof(T).Equals(exception.GetType()))
                ThrowAssertError(0, message, ("ExpectedException", typeof(T).Name), ("ActualException", exception?.GetType().Name));

            return (exception as T)!;
        }

        #endregion

        /// <summary>
        /// Handles a failed assertion.
        /// </summary>
        /// <param name="message">The message to show in the error log.</param>
        protected abstract void HandleFailedAssertion(string message);
    }
}
