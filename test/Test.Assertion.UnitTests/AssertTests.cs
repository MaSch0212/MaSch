﻿using MSAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace MaSch.Test.Assertion.UnitTests;

[TestClass]
public class AssertTests
{
    private static Assert AssertUnderTest => MaSch.Test.Assertion.Assert.Instance;

    [TestMethod]
    public void Instance()
    {
        MSAssert.IsNotNull(MaSch.Test.Assertion.Assert.Instance);
    }

#if MSTEST
    [TestMethod]
    public void That()
    {
        MSAssert.AreSame(MSAssert.That, AssertUnderTest.That);
    }
#endif

    [TestMethod]
    public void Inc()
    {
        MSAssert.AreSame(MaSch.Test.Assertion.InconclusiveAssert.Instance, AssertUnderTest.Inc);
    }

    [TestMethod]
    public void AssertNamePrefix()
    {
        var prop = typeof(Assert).GetProperty("AssertNamePrefix", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        MSAssert.IsNotNull(prop);
        MSAssert.AreEqual("Assert", prop.GetValue(AssertUnderTest));
    }

    [TestMethod]
    public void HandleFailedAssertion()
    {
        var method = typeof(Assert).GetMethod("HandleFailedAssertion", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly, null, new[] { typeof(string) }, null);
        MSAssert.IsNotNull(method);
        var ex = MSAssert.ThrowsException<TargetInvocationException>(() => method.Invoke(AssertUnderTest, new object[] { "My test error message" }));
        MSAssert.IsInstanceOfType(ex.InnerException, typeof(AssertFailedException));
        MSAssert.AreEqual("My test error message", ex.InnerException!.Message);
    }
}
