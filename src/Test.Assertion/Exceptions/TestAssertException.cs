#if !MSTEST

using System;
using System.Diagnostics.CodeAnalysis;

namespace MaSch.Test.Assertion
{
    /// <summary>
    /// Base class for exceptions during a test run.
    /// </summary>
    [SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "Serialization not necessary")]
    public abstract class TestAssertException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestAssertException"/> class.
        /// </summary>
        protected TestAssertException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestAssertException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        protected TestAssertException(string? message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestAssertException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The exception.</param>
        protected TestAssertException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}

#endif