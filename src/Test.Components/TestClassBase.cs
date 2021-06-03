using MaSch.Core;
using MaSch.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MaSch.Test
{
    /// <summary>
    /// Represents a base class for unit test classes.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public abstract class TestClassBase : IDisposable
    {
        /// <summary>
        /// Gets or sets the default mock behavior.
        /// </summary>
        public static MockBehavior DefaultMockBehavior { get; set; } = MockBehavior.Default;

        /// <summary>
        /// Gets an object to execute assertions.
        /// </summary>
        protected static Assertion.Assert Assert => Assertion.Assert.Instance;

        /// <summary>
        /// Gets or sets a value indicating whether the cache of this instance should be cleared after each test.
        /// </summary>
        protected virtual bool CleanupCacheAfterTest { get; set; } = true;

        /// <summary>
        /// Gets the cache of the current test class.
        /// </summary>
        protected virtual Cache Cache { get; } = new Cache();

        /// <summary>
        /// Gets the mock behavior that is used to initialize the <see cref="Mocks"/> property..
        /// </summary>
        protected virtual MockBehavior MockBehavior => DefaultMockBehavior;

        /// <summary>
        /// Gets the <see cref="MockRepository"/> with which mocks should be created in this <see cref="TestClassBase"/>.
        /// </summary>
        protected virtual MockRepository Mocks => Cache.GetValue(() => new MockRepository(MockBehavior))!;

        /// <summary>
        /// Gets the verifiables of the current test.
        /// </summary>
        protected internal virtual MockVerifiableCollection Verifiables { get; } = new MockVerifiableCollection();

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        /// <summary>
        /// Gets or sets the test context.
        /// </summary>
        public TestContext TestContext { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        /// <summary>
        /// Initializes the test execution.
        /// </summary>
        [TestInitialize]
        public void InitializeTest()
        {
            OnInitializeTest();
        }

        /// <summary>
        /// Cleans up after test execution.
        /// </summary>
        [TestCleanup]
        public void CleanupTest()
        {
            OnCleanupTest();

            if (TestContext.CurrentTestOutcome == UnitTestOutcome.Passed || TestContext.CurrentTestOutcome == UnitTestOutcome.Inconclusive)
                Verifiables.Verify();
            Verifiables.Clear();

            if (CleanupCacheAfterTest)
                Cache.Clear();
        }

        /// <summary>
        /// Called when the test has been initialized.
        /// </summary>
        protected virtual void OnInitializeTest()
        {
        }

        /// <summary>
        /// Called before the test is cleaned up.
        /// </summary>
        protected virtual void OnCleanupTest()
        {
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Cache.Clear();
                Cache.Dispose();

                Verifiables.Clear();
                ((IDisposable)Verifiables).Dispose();
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
