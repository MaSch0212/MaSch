using MaSch.Core;
using MaSch.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace MaSch.Test
{
    /// <summary>
    /// Represents a base class for unit test classes.
    /// </summary>
    public abstract class TestClassBase
    {
        /// <summary>
        /// Gets an object to execute assertions.
        /// </summary>
        [ExcludeFromCodeCoverage]
        protected static Assertion.Assert Assert => Assertion.Assert.Instance;

        /// <summary>
        /// Gets or sets a value indicating whether the cache of this instance should be cleared after each test.
        /// </summary>
        [ExcludeFromCodeCoverage]
        protected virtual bool CleanupCacheAfterTest { get; set; } = true;

        /// <summary>
        /// Gets the cache of the current test class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        protected virtual Cache Cache { get; } = new Cache();

        /// <summary>
        /// Gets the verifiables of the current test.
        /// </summary>
        protected internal virtual MockVerifiableCollection Verifiables { get; } = new MockVerifiableCollection();

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        /// <summary>
        /// Gets or sets the test context.
        /// </summary>
        [ExcludeFromCodeCoverage]
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

            Verifiables.Verify();
            Verifiables.Clear();

            if (CleanupCacheAfterTest)
                Cache.Clear();
        }

        /// <summary>
        /// Called when the test has been initialized.
        /// </summary>
        [ExcludeFromCodeCoverage]
        protected virtual void OnInitializeTest()
        {
        }

        /// <summary>
        /// Called before the test is cleaned up.
        /// </summary>
        [ExcludeFromCodeCoverage]
        protected virtual void OnCleanupTest()
        {
        }
    }
}
