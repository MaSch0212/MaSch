﻿#if MSTEST
using MSAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
#endif

namespace MaSch.Test.Assertion;

/// <summary>
/// A collection of helper classes to test various conditions within unit tests.
/// If the condition being tested is not met, an <see cref="AssertInconclusiveException"/> is thrown.
/// </summary>
/// <seealso cref="AssertBase" />
public class InconclusiveAssert : AssertBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InconclusiveAssert"/> class.
    /// </summary>
    protected InconclusiveAssert()
    {
    }

    /// <summary>
    /// Gets the singleton instance of the <see cref="InconclusiveAssert"/>.
    /// </summary>
    public static InconclusiveAssert Instance { get; } = new InconclusiveAssert();

    /// <inheritdoc/>
    protected override string? AssertNamePrefix { get; } = "Assert.Inc";

#if MSTEST
    /// <summary>
    /// Runs an assertion from an extension method for <see cref="Microsoft.VisualStudio.TestTools.UnitTesting.Assert"/>.
    /// If an <see cref="AssertFailedException"/> is thrown, it will be rethrown as an <see cref="AssertInconclusiveException"/>.
    /// </summary>
    /// <param name="assertAction">The assert action.</param>
    public virtual void That(Action<MSAssert> assertAction)
    {
        CatchAssertException(() => assertAction(MSAssert.That));
    }

    /// <summary>
    /// Runs an assertion from an extension method for <see cref="Microsoft.VisualStudio.TestTools.UnitTesting.Assert"/>.
    /// If an <see cref="AssertFailedException"/> is thrown, it will be rethrown as an <see cref="AssertInconclusiveException"/>.
    /// </summary>
    /// <typeparam name="T">The type of the result of the action.</typeparam>
    /// <param name="assertAction">The assert action.</param>
    /// <returns>The result of the action.</returns>
    public virtual T That<T>(Func<MSAssert, T> assertAction)
    {
        return CatchAssertException(() => assertAction(MSAssert.That));
    }

    /// <summary>
    /// Runs an assertion from an extension method for <see cref="Microsoft.VisualStudio.TestTools.UnitTesting.Assert"/>.
    /// If an <see cref="AssertFailedException"/> is thrown, it will be rethrown as an <see cref="AssertInconclusiveException"/>.
    /// </summary>
    /// <param name="assertAction">The assert action.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public virtual async Task That(Func<MSAssert, Task> assertAction)
    {
        await CatchAssertException(() => assertAction(MSAssert.That));
    }

    /// <summary>
    /// Runs an assertion from an extension method for <see cref="Microsoft.VisualStudio.TestTools.UnitTesting.Assert"/>.
    /// If an <see cref="AssertFailedException"/> is thrown, it will be rethrown as an <see cref="AssertInconclusiveException"/>.
    /// </summary>
    /// <typeparam name="T">The type of the result of the action.</typeparam>
    /// <param name="assertAction">The assert action.</param>
    /// <returns>The result of the action.</returns>
    public virtual async Task<T> That<T>(Func<MSAssert, Task<T>> assertAction)
    {
        return await CatchAssertException(() => assertAction(MSAssert.That));
    }
#endif

    /// <inheritdoc/>
    [DoesNotReturn]
    protected override void HandleFailedAssertion(string message)
    {
        throw new AssertInconclusiveException("[Inconclusive] " + message);
    }
}
