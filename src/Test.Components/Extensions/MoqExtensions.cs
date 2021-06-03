using MaSch.Test.Models;
using Moq;
using Moq.Language;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace MaSch.Test
{
    /// <summary>
    /// Provides extensions for classes/interfacs of the Moq library.
    /// </summary>
    public static class MoqExtensions
    {
        private static Type? _setupPhraseType = Assembly.Load("Moq").GetType("Moq.Language.Flow.SetupPhrase", true);
        private static PropertyInfo? _setupProperty = _setupPhraseType?.GetProperty("Setup");
        private static PropertyInfo? _expressionProperty = _setupProperty?.PropertyType.GetProperty("Expression");
        private static PropertyInfo? _mockProperty = _setupProperty?.PropertyType.GetProperty("Mock");
        private static MethodInfo? _generalVerifyMethod = typeof(Mock).GetMethod("Verify", BindingFlags.NonPublic | BindingFlags.Static, null, new[] { typeof(Mock), typeof(LambdaExpression), typeof(Times), typeof(string) }, null);

        /// <summary>
        /// Creates a verifiable object for this setup.
        /// </summary>
        /// <param name="setup">The setup to verify.</param>
        /// <returns>A <see cref="IMockVerifiable"/> that verifies this setup.</returns>
        public static IMockVerifiable Verifiable(this IVerifies setup)
            => CreateVerifiable(setup, Times.AtLeastOnce(), null);

        /// <summary>
        /// Creates a verifiable object for this setup.
        /// </summary>
        /// <param name="setup">The setup to verify.</param>
        /// <param name="times">The number of times a method is expected to be called.</param>
        /// <returns>A <see cref="IMockVerifiable"/> that verifies this setup.</returns>
        public static IMockVerifiable Verifiable(this IVerifies setup, Times times)
            => CreateVerifiable(setup, times, null);

        /// <summary>
        /// Creates a verifiable object for this setup.
        /// </summary>
        /// <param name="setup">The setup to verify.</param>
        /// <param name="times">The number of times a method is expected to be called.</param>
        /// <param name="failMessage">Message to show if verification fails.</param>
        /// <returns>A <see cref="IMockVerifiable"/> that verifies this setup.</returns>
        public static IMockVerifiable Verifiable(this IVerifies setup, Times times, string? failMessage)
            => CreateVerifiable(setup, times, failMessage);

        /// <summary>
        /// Creates a verifiable object for this setup.
        /// </summary>
        /// <typeparam name="T">The type of setup.</typeparam>
        /// <param name="setup">The setup to verify.</param>
        /// <param name="verifiable">The created verifiable object.</param>
        /// <returns>The same instance as this method is called on.</returns>
        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP003:Dispose previous before re-assigning.", Justification = "The caller would not expect this.")]
        public static T Verifiable<T>(this T setup, out IMockVerifiable verifiable)
            where T : class, IVerifies
            => VerifiableImpl(setup, out verifiable, Times.AtLeastOnce(), null);

        /// <summary>
        /// Creates a verifiable object for this setup.
        /// </summary>
        /// <typeparam name="T">The type of setup.</typeparam>
        /// <param name="setup">The setup to verify.</param>
        /// <param name="verifiable">The created verifiable object.</param>
        /// <param name="times">The number of times a method is expected to be called.</param>
        /// <returns>The same instance as this method is called on.</returns>
        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP003:Dispose previous before re-assigning.", Justification = "The caller would not expect this.")]
        public static T Verifiable<T>(this T setup, out IMockVerifiable verifiable, Times times)
            where T : class, IVerifies
            => VerifiableImpl(setup, out verifiable, times, null);

        /// <summary>
        /// Creates a verifiable object for this setup.
        /// </summary>
        /// <typeparam name="T">The type of setup.</typeparam>
        /// <param name="setup">The setup to verify.</param>
        /// <param name="verifiable">The created verifiable object.</param>
        /// <param name="times">The number of times a method is expected to be called.</param>
        /// <param name="failMessage">Message to show if verification fails.</param>
        /// <returns>The same instance as this method is called on.</returns>
        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP003:Dispose previous before re-assigning.", Justification = "The caller would not expect this.")]
        public static T Verifiable<T>(this T setup, out IMockVerifiable verifiable, Times times, string? failMessage)
            where T : class, IVerifies
            => VerifiableImpl(setup, out verifiable, times, failMessage);

        /// <summary>
        /// Creates a verifiable object for this setup.
        /// </summary>
        /// <typeparam name="T">The type of setup.</typeparam>
        /// <param name="setup">The setup to verify.</param>
        /// <param name="verifiableCollection">The verifiable collection to which the created verifiable object is added to.</param>
        /// <returns>The same instance as this method is called on.</returns>
        public static T Verifiable<T>(this T setup, MockVerifiableCollection verifiableCollection)
            where T : class, IVerifies
            => VerifiableImpl(setup, verifiableCollection, Times.AtLeastOnce(), null);

        /// <summary>
        /// Creates a verifiable object for this setup.
        /// </summary>
        /// <typeparam name="T">The type of setup.</typeparam>
        /// <param name="setup">The setup to verify.</param>
        /// <param name="verifiableCollection">The verifiable collection to which the created verifiable object is added to.</param>
        /// <param name="times">The number of times a method is expected to be called.</param>
        /// <returns>The same instance as this method is called on.</returns>
        public static T Verifiable<T>(this T setup, MockVerifiableCollection verifiableCollection, Times times)
            where T : class, IVerifies
            => VerifiableImpl(setup, verifiableCollection, times, null);

        /// <summary>
        /// Creates a verifiable object for this setup.
        /// </summary>
        /// <typeparam name="T">The type of setup.</typeparam>
        /// <param name="setup">The setup to verify.</param>
        /// <param name="verifiableCollection">The verifiable collection to which the created verifiable object is added to.</param>
        /// <param name="times">The number of times a method is expected to be called.</param>
        /// <param name="failMessage">Message to show if verification fails.</param>
        /// <returns>The same instance as this method is called on.</returns>
        public static T Verifiable<T>(this T setup, MockVerifiableCollection verifiableCollection, Times times, string? failMessage)
            where T : class, IVerifies
            => VerifiableImpl(setup, verifiableCollection, times, failMessage);

        /// <summary>
        /// Creates a verifiable object for this setup.
        /// </summary>
        /// <typeparam name="T">The type of setup.</typeparam>
        /// <param name="setup">The setup to verify.</param>
        /// <param name="testClass">The test class to which the created verifiable object is added to.</param>
        /// <returns>The same instance as this method is called on.</returns>
        public static T Verifiable<T>(this T setup, TestClassBase testClass)
            where T : class, IVerifies
            => VerifiableImpl(setup, testClass.Verifiables, Times.AtLeastOnce(), null);

        /// <summary>
        /// Creates a verifiable object for this setup.
        /// </summary>
        /// <typeparam name="T">The type of setup.</typeparam>
        /// <param name="setup">The setup to verify.</param>
        /// <param name="testClass">The test class to which the created verifiable object is added to.</param>
        /// <param name="times">The number of times a method is expected to be called.</param>
        /// <returns>The same instance as this method is called on.</returns>
        public static T Verifiable<T>(this T setup, TestClassBase testClass, Times times)
            where T : class, IVerifies
            => VerifiableImpl(setup, testClass.Verifiables, times, null);

        /// <summary>
        /// Creates a verifiable object for this setup.
        /// </summary>
        /// <typeparam name="T">The type of setup.</typeparam>
        /// <param name="setup">The setup to verify.</param>
        /// <param name="testClass">The test class to which the created verifiable object is added to.</param>
        /// <param name="times">The number of times a method is expected to be called.</param>
        /// <param name="failMessage">Message to show if verification fails.</param>
        /// <returns>The same instance as this method is called on.</returns>
        public static T Verifiable<T>(this T setup, TestClassBase testClass, Times times, string? failMessage)
            where T : class, IVerifies
            => VerifiableImpl(setup, testClass.Verifiables, times, failMessage);

        /// <summary>
        /// Verifies this <see cref="IMockVerifiable"/>.
        /// </summary>
        /// <param name="verifiable">The object to verify.</param>
        public static void Verify(this IMockVerifiable verifiable)
            => verifiable.Verify(null, null);

        /// <summary>
        /// Verifies this <see cref="IMockVerifiable"/>.
        /// </summary>
        /// <param name="verifiable">The object to verify.</param>
        /// <param name="failMessage">Message to show if verification fails.</param>
        public static void Verify(this IMockVerifiable verifiable, string failMessage)
            => verifiable.Verify(null, failMessage);

        /// <summary>
        /// Verifies this <see cref="IMockVerifiable"/>.
        /// </summary>
        /// <param name="verifiable">The object to verify.</param>
        /// <param name="times">The number of times a method is expected to be called.</param>
        public static void Verify(this IMockVerifiable verifiable, Times times)
            => verifiable.Verify(times, null);

        private static T VerifiableImpl<T>(T setup, out IMockVerifiable verifiable, Times times, string? failMessage)
            where T : class
        {
            verifiable = CreateVerifiable(setup, times, failMessage);
            return setup;
        }

        private static T VerifiableImpl<T>(T setup, MockVerifiableCollection verifiableCollection, Times times, string? failMessage)
            where T : class
        {
            verifiableCollection.Add(CreateVerifiable(setup, times, failMessage));
            return setup;
        }

        [ExcludeFromCodeCoverage]
        private static IMockVerifiable CreateVerifiable(object setupObj, Times defaultTimes, string? defaultFailMessage)
        {
            var setup = _setupProperty?.GetValue(setupObj) ?? throw new Exception("Could not retrieve setup property.");
            var expression = (LambdaExpression)(_expressionProperty?.GetValue(setup) ?? throw new Exception("Could not retrieve expression property."));
            var innerMock = (Mock)(_mockProperty?.GetValue(setup) ?? throw new Exception("Could not retrieve mock property."));

            void Verification(Times times, string? msg)
            {
                if (_generalVerifyMethod == null)
                    throw new Exception("Could not retrieve verify method from mock object.");

                try
                {
                    _generalVerifyMethod.Invoke(null, new object?[] { innerMock, expression, times, msg });
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException ?? ex;
                }
            }

            return new MockVerifiable(Verification, defaultTimes, defaultFailMessage);
        }
    }
}
