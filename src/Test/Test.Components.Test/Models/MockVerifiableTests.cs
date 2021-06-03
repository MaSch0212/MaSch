using MaSch.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace MaSch.Test.Components.Test.Models
{
    [TestClass]
    public class MockVerifiableTests : TestClassBase
    {
        private static readonly FieldInfo? _verifyActionField = typeof(MockVerifiable).GetField("_verifyAction", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        private static readonly FieldInfo? _defaultTimesField = typeof(MockVerifiable).GetField("_defaultTimes", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        private static readonly FieldInfo? _defaultFailMessageField = typeof(MockVerifiable).GetField("_defaultFailMessage", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        [TestMethod]
        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP001:Dispose created.", Justification = "Would fail test.")]
        public void Ctor_Action()
        {
            var actionMock = new Mock<MockVerification>(MockBehavior.Strict);

            var verifiable = new MockVerifiable(actionMock.Object);

            AssertCtor(verifiable, actionMock.Object, Times.AtLeastOnce(), null);
        }

        [TestMethod]
        public void Ctor_ActionNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => ((IDisposable)new MockVerifiable(null!)).Dispose());
        }

        [TestMethod]
        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP001:Dispose created.", Justification = "Would fail test.")]
        public void Ctor_Action_Times()
        {
            var actionMock = new Mock<MockVerification>(MockBehavior.Strict);

            var verifiable = new MockVerifiable(actionMock.Object, Times.Once());

            AssertCtor(verifiable, actionMock.Object, Times.Once(), null);
        }

        [TestMethod]
        public void Ctor_ActionNull_Times()
        {
            Assert.ThrowsException<ArgumentNullException>(() => ((IDisposable)new MockVerifiable(null!, Times.Once())).Dispose());
        }

        [TestMethod]
        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP001:Dispose created.", Justification = "Would fail test.")]
        public void Ctor_Action_Times_Message()
        {
            var actionMock = new Mock<MockVerification>(MockBehavior.Strict);

            var verifiable = new MockVerifiable(actionMock.Object, Times.Once(), "My message");

            AssertCtor(verifiable, actionMock.Object, Times.Once(), "My message");
        }

        [TestMethod]
        public void Ctor_ActionNull_Times_Message()
        {
            Assert.ThrowsException<ArgumentNullException>(() => ((IDisposable)new MockVerifiable(null!, Times.Once(), "My message")).Dispose());
        }

        [TestMethod]
        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP001:Dispose created.", Justification = "Would fail test.")]
        public void Ctor_Action_Times_MessageNull()
        {
            var actionMock = new Mock<MockVerification>(MockBehavior.Strict);

            var verifiable = new MockVerifiable(actionMock.Object, Times.Once(), null);

            AssertCtor(verifiable, actionMock.Object, Times.Once(), null);
        }

        [TestMethod]
        public void Verify()
        {
            var actionMock = new Mock<MockVerification>(MockBehavior.Strict);
            actionMock.Setup(x => x(Times.Once(), "My message"));
            using var verifiable = new MockVerifiable(actionMock.Object, Times.Once(), "My message");

            verifiable.Verify(null, null);

            actionMock.Verify(x => x(Times.Once(), "My message"), Times.Once);
        }

        [TestMethod]
        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP001:Dispose created.", Justification = "Would fail test.")]
        public void Verify_WithTimes()
        {
            var actionMock = new Mock<MockVerification>(MockBehavior.Strict);
            actionMock.Setup(x => x(Times.Never(), "My message"));
            var verifiable = new MockVerifiable(actionMock.Object, Times.Once(), "My message");

            verifiable.Verify(Times.Never(), null);

            actionMock.Verify(x => x(Times.Never(), "My message"), Times.Once);
        }

        [TestMethod]
        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP001:Dispose created.", Justification = "Would fail test.")]
        public void Verify_WithMessage()
        {
            var actionMock = new Mock<MockVerification>(MockBehavior.Strict);
            actionMock.Setup(x => x(Times.Once(), "blub"));
            var verifiable = new MockVerifiable(actionMock.Object, Times.Once(), "My message");

            verifiable.Verify(null, "blub");

            actionMock.Verify(x => x(Times.Once(), "blub"), Times.Once);
        }

        [TestMethod]
        public void Dispose_()
        {
            var actionMock = new Mock<MockVerification>(MockBehavior.Strict);
            actionMock.Setup(x => x(Times.Once(), "My message"));
            var verifiable = new MockVerifiable(actionMock.Object, Times.Once(), "My message");

            ((IDisposable)verifiable).Dispose();

            actionMock.Verify(x => x(Times.Once(), "My message"), Times.Once);
        }

        private static void AssertCtor(MockVerifiable verifiable, MockVerification expectedVerifyAction, Times expectedDefaultTimes, string? expectedDefaultFailMessage)
        {
            Assert.Inc.IsNotNull(_verifyActionField);
            Assert.Inc.IsNotNull(_defaultTimesField);
            Assert.Inc.IsNotNull(_defaultFailMessageField);

            Assert.AreEqual(expectedVerifyAction, _verifyActionField.GetValue(verifiable));
            Assert.AreEqual(expectedDefaultTimes, _defaultTimesField.GetValue(verifiable));
            Assert.AreEqual(expectedDefaultFailMessage, _defaultFailMessageField.GetValue(verifiable));
        }
    }
}
