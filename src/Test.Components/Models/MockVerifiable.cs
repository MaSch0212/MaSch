using MaSch.Core;
using Moq;
using System;

namespace MaSch.Test.Models
{
    /// <summary>
    /// The delegate that is called by <see cref="MockVerifiable"/> when it gets verified.
    /// </summary>
    /// <param name="times">The number of times a method is expected to be called.</param>
    /// <param name="failMessage">Message to show if verification fails.</param>
    public delegate void MockVerification(Func<Times> times, string? failMessage);

    /// <summary>
    /// Object that can be verified using a delegate.
    /// </summary>
    /// <seealso cref="MaSch.Test.Models.IMockVerifiable" />
    public class MockVerifiable : IMockVerifiable
    {
        private readonly MockVerification _verifyAction;
        private readonly Func<Times> _defaultTimes;
        private readonly string? _defaultFailMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockVerifiable"/> class.
        /// </summary>
        /// <param name="verifyAction">The action to execute when this <see cref="IMockVerifiable"/> is verified.</param>
        public MockVerifiable(MockVerification verifyAction)
            : this (verifyAction, Times.AtLeastOnce)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MockVerifiable"/> class.
        /// </summary>
        /// <param name="verifyAction">The action to execute when this <see cref="IMockVerifiable"/> is verified.</param>
        /// <param name="defaultTimes">The default number of times a method is expected to be called.</param>
        public MockVerifiable(MockVerification verifyAction, Func<Times> defaultTimes)
        {
            _verifyAction = Guard.NotNull(verifyAction, nameof(verifyAction));
            _defaultTimes = Guard.NotNull(defaultTimes, nameof(defaultTimes));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MockVerifiable"/> class.
        /// </summary>
        /// <param name="verifyAction">The action to execute when this <see cref="IMockVerifiable"/> is verified.</param>
        /// <param name="defaultTimes">The default number of times a method is expected to be called.</param>
        /// <param name="defaultFailMessage">Default message to show if verification fails.</param>
        public MockVerifiable(MockVerification verifyAction, Func<Times> defaultTimes, string? defaultFailMessage)
            : this(verifyAction, defaultTimes)
        {
            _defaultFailMessage = defaultFailMessage;
        }

        /// <inheritdoc/>
        public void Verify(Func<Times>? times, string? failMessage)
        {
            _verifyAction.Invoke(times ?? _defaultTimes, failMessage ?? _defaultFailMessage);
        }

        /// <inheritdoc/>
        void IDisposable.Dispose() => Verify(null, null);
    }

    /// <summary>
    /// Provides a mechanism to verify an object.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IMockVerifiable : IDisposable
    {
        /// <summary>
        /// Verifies this <see cref="IMockVerifiable"/>.
        /// </summary>
        /// <param name="times">The number of times a method is expected to be called.</param>
        /// <param name="failMessage">Message to show if verification fails.</param>
        void Verify(Func<Times>? times, string? failMessage);
    }
}