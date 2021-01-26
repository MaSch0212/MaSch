using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MaSch.Test.Extensions
{
    public static class AssertExtensions
    {
        public static void AllElements<TValue, TArray>(this Assert _, TValue[] expectedValues, IEnumerable<TArray> actualArray, Action<TValue, TArray> itemAssertFunction)
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
                if(index >= expectedValues.Length)
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
            if(index < expectedValues.Length)
                Assert.Fail("The actual enumerable has less elements than the expected values array.");
        }
    }
}
