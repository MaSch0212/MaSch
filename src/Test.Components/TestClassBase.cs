﻿using MaSch.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace MaSch.Test
{
    /// <summary>
    /// Represents a base class for unit test classes.
    /// </summary>
    /// <seealso cref="MaSch.Core.Cache" />
    [ExcludeFromCodeCoverage]
    public abstract class TestClassBase : Cache
    {
        /// <summary>
        /// Gets an object to execute assertions.
        /// </summary>
        protected static Assertion.Assert Assert => Assertion.Assert.Instance;

        /// <summary>
        /// Gets or sets a value indicating whether the cache of this instance should be cleared after each test.
        /// </summary>
        protected bool CleanupCacheAfterTest { get; set; } = true;

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

            if (CleanupCacheAfterTest && Objects.Count > 0)
                Clear();
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
    }
}