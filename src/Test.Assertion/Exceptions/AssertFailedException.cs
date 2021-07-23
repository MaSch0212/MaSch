#if !MSTEST

using System;
using System.Diagnostics.CodeAnalysis;

namespace MaSch.Test.Assertion
{
    /// <summary>
    /// AssertFailedException class. Used to indicate failure for a test case.
    /// </summary>
    [SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "Serialization not necessary")]
    public class AssertFailedException : TestAssertException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssertFailedException"/> class.
        /// </summary>
        public AssertFailedException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssertFailedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public AssertFailedException(string? message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssertFailedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The exception.</param>
        public AssertFailedException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}

#endif