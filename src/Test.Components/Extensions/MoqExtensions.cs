﻿using MaSch.Test.Models;
using Moq;
using Moq.Language;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace MaSch.Test.Extensions
{
    /// <summary>
    /// Provides extensions for classes/interfacs of the Moq library.
    /// </summary>
    public static class MoqExtensions
    {
#pragma warning disable SA1600 // Elements should be documented
        internal static readonly Type? SetupPhraseType = Assembly.Load("Moq").GetType("Moq.Language.Flow.SetupPhrase", true);
        internal static readonly PropertyInfo? SetupProperty = SetupPhraseType?.GetProperty("Setup");
        internal static readonly PropertyInfo? ExpressionProperty = SetupProperty?.PropertyType.GetProperty("Expression");
        internal static readonly PropertyInfo? MockProperty = SetupProperty?.PropertyType.GetProperty("Mock");
        internal static readonly MethodInfo? GeneralVerifyMethod = typeof(Mock).GetMethod("Verify", BindingFlags.NonPublic | BindingFlags.Static, null, new[] { typeof(Mock), typeof(LambdaExpression), typeof(Times), typeof(string) }, null);
#pragma warning restore SA1600 // Elements should be documented

        /// <summary>
        /// Creates a verifiable object for this setup.
        /// </summary>
        /// <param name="setup">The setup to verify.</param>
        /// <param name="times">The number of times a method is expected to be called.</param>
        /// <returns>A <see cref="IMockVerifiable"/> that verifies this setup.</returns>
        public static IMockVerifiable Verifiable(this IVerifies setup, Func<Times> times)
            => CreateVerifiable(setup, times, null);

        /// <summary>
        /// Creates a verifiable object for this setup.
        /// </summary>
        /// <param name="setup">The setup to verify.</param>
        /// <param name="times">The number of times a method is expected to be called.</param>
        /// <param name="failMessage">Message to show if verification fails.</param>
        /// <returns>A <see cref="IMockVerifiable"/> that verifies this setup.</returns>
        public static IMockVerifiable Verifiable(this IVerifies setup, Func<Times> times, string? failMessage)
            => CreateVerifiable(setup, times, failMessage);

        /// <summary>
        /// Creates a verifiable object for this setup.
        /// </summary>
        /// <param name="setup">The setup to verify.</param>
        /// <param name="verifiable">The created verifiable object.</param>
        /// <param name="times">The number of times a method is expected to be called.</param>
        /// <returns>The same instance as this method is called on.</returns>
        public static IVerifies Verifiable(this IVerifies setup, out IMockVerifiable verifiable, Func<Times> times)
            => VerifiableImpl(setup, out verifiable, times, null);

        /// <summary>
        /// Creates a verifiable object for this setup.
        /// </summary>
        /// <param name="setup">The setup to verify.</param>
        /// <param name="verifiable">The created verifiable object.</param>
        /// <param name="times">The number of times a method is expected to be called.</param>
        /// <param name="failMessage">Message to show if verification fails.</param>
        /// <returns>The same instance as this method is called on.</returns>
        public static IVerifies Verifiable(this IVerifies setup, out IMockVerifiable verifiable, Func<Times> times, string? failMessage)
            => VerifiableImpl(setup, out verifiable, times, failMessage);

        /// <summary>
        /// Creates a verifiable object for this setup.
        /// </summary>
        /// <param name="setup">The setup to verify.</param>
        /// <param name="verifiableCollection">The verifiable collection to which the created verifiable object is added to.</param>
        /// <param name="times">The number of times a method is expected to be called.</param>
        /// <returns>The same instance as this method is called on.</returns>
        public static IVerifies Verifiable(this IVerifies setup, MockVerifiableCollection verifiableCollection, Func<Times> times)
            => VerifiableImpl(setup, verifiableCollection, times, null);

        /// <summary>
        /// Creates a verifiable object for this setup.
        /// </summary>
        /// <param name="setup">The setup to verify.</param>
        /// <param name="verifiableCollection">The verifiable collection to which the created verifiable object is added to.</param>
        /// <param name="times">The number of times a method is expected to be called.</param>
        /// <param name="failMessage">Message to show if verification fails.</param>
        /// <returns>The same instance as this method is called on.</returns>
        public static IVerifies Verifiable(this IVerifies setup, MockVerifiableCollection verifiableCollection, Func<Times> times, string? failMessage)
            => VerifiableImpl(setup, verifiableCollection, times, failMessage);

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
            => verifiable.Verify(() => times, null);

        /// <summary>
        /// Verifies this <see cref="IMockVerifiable"/>.
        /// </summary>
        /// <param name="verifiable">The object to verify.</param>
        /// <param name="times">The number of times a method is expected to be called.</param>
        /// <param name="failMessage">Message to show if verification fails.</param>
        public static void Verify(this IMockVerifiable verifiable, Times times, string failMessage)
            => verifiable.Verify(() => times, failMessage);

        /// <summary>
        /// Verifies this <see cref="IMockVerifiable"/>.
        /// </summary>
        /// <param name="verifiable">The object to verify.</param>
        /// <param name="times">The number of times a method is expected to be called.</param>
        public static void Verify(this IMockVerifiable verifiable, Func<Times> times)
            => verifiable.Verify(times, null);

        private static T VerifiableImpl<T>(T setup, out IMockVerifiable verifiable, Func<Times> times, string? failMessage)
            where T : class
        {
            verifiable = CreateVerifiable(setup, times, failMessage);
            return setup;
        }

        private static T VerifiableImpl<T>(T setup, MockVerifiableCollection verifiableCollection, Func<Times> times, string? failMessage)
            where T : class
        {
            verifiableCollection.Add(CreateVerifiable(setup, times, failMessage));
            return setup;
        }

        [ExcludeFromCodeCoverage]
        private static IMockVerifiable CreateVerifiable(object setupObj, Func<Times> defaultTimes, string? defaultFailMessage)
        {
            var setup = SetupProperty?.GetValue(setupObj) ?? throw new Exception("Could not retrieve setup property.");
            var expression = (LambdaExpression)(ExpressionProperty?.GetValue(setup) ?? throw new Exception("Could not retrieve expression property."));
            var innerMock = (Mock)(MockProperty?.GetValue(setup) ?? throw new Exception("Could not retrieve mock property."));

            void Verification(Func<Times> times, string? msg)
            {
                if (GeneralVerifyMethod == null)
                    throw new Exception("Could not retrieve verify method from mock object.");
                GeneralVerifyMethod.Invoke(null, new object?[] { innerMock, expression, times(), msg });
            }

            return new MockVerifiable(Verification, defaultTimes, defaultFailMessage);
        }
    }
}