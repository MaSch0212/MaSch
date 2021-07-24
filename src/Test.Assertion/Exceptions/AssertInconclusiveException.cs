#if !MSTEST

using System;
using System.Diagnostics.CodeAnalysis;

namespace MaSch.Test.Assertion
{
    /// <summary>
    /// The assert inconclusive exception.
    /// </summary>
    [SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "Serialization not necessary")]
    [ExcludeFromCodeCoverage]
    public class AssertInconclusiveException : TestAssertException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssertInconclusiveException"/> class.
        /// </summary>
        public AssertInconclusiveException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssertInconclusiveException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public AssertInconclusiveException(string? message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssertInconclusiveException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The exception.</param>
        public AssertInconclusiveException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}

#endif