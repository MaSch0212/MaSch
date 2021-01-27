using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MaSch.Test.Extensions
{
    /// <summary>
    /// Contains extensions for the <see cref="Assert"/> class.
    /// </summary>
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Assert object is not really needed for these extensions.")]
    public static class AssertExtensions
    {
        /// <summary>
        /// Verifies that an array contains specific elements.
        /// </summary>
        /// <typeparam name="TValue">The type of the expected values.</typeparam>
        /// <typeparam name="TArray">The type of the actual values.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expectedValues">The expected values.</param>
        /// <param name="actualArray">The actual array.</param>
        /// <param name="itemAssertFunction">The function that compares values between <typeparamref name="TArray"/> and <typeparamref name="TValue"/>.</param>
        public static void AllElements<TValue, TArray>(this Assert assert, TValue[] expectedValues, IEnumerable<TArray> actualArray, Action<TValue, TArray> itemAssertFunction)
        {
            if (expectedValues == null)
            {
                Assert.IsNull(actualArray, "The actual enumerable is null.");
                return;
            }

            Assert.IsNotNull(actualArray, "The actual enumerable is not null.");

            if (actualArray is ICollection collection)
                Assert.AreEqual(expectedValues.Length, collection.Count, "The lengths of the arrays do not match.");

            int index = 0;
            foreach (var actual in actualArray)
            {
                if (index >= expectedValues.Length)
                    Assert.Fail("The actual enumerable has more elements than the expected values array.");

                try
                {
                    itemAssertFunction(expectedValues[index], actual);
                }
                catch (AssertFailedException ex)
                {
                    Assert.Fail($"The assert for item {index} failed with the following message: " + ex.Message);
                }

                index++;
            }

            if (index < expectedValues.Length)
                Assert.Fail("The actual enumerable has less elements than the expected values array.");
        }
    }
}
