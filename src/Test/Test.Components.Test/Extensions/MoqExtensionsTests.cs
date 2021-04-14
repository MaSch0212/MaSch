using MaSch.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace MaSch.Test.Components.Test.Extensions
{
    [TestClass]
    public class MoqExtensionsTests : TestClassBase
    {
        [TestMethod]
        public void SetupPhraseType()
        {
            var mock = new Mock<Action>();
            var setup = mock.Setup(x => x());

            Assert.IsNotNull(MoqExtensions.SetupPhraseType);
            Assert.IsInstanceOfType(setup, MoqExtensions.SetupPhraseType);
        }

        [TestMethod]
        public void SetupProperty()
        {
            var mock = new Mock<Action>();
            var setup = mock.Setup(x => x());

            Assert.IsNotNull(MoqExtensions.SetupProperty);

            var value = MoqExtensions.SetupProperty.GetValue(setup);
            Assert.IsNotNull(value);
        }

        [TestMethod]
        public void ExpressionProperty()
        {
            var mock = new Mock<Action>();
            var setup = mock.Setup(x => x());

            Assert.Inc.IsNotNull(MoqExtensions.SetupProperty);
            Assert.IsNotNull(MoqExtensions.ExpressionProperty);

            var sp = MoqExtensions.SetupProperty.GetValue(setup);
            Assert.Inc.IsNotNull(sp);

            var expression = MoqExtensions.ExpressionProperty.GetValue(sp);
            Assert.IsNotNull(expression);
        }

        [TestMethod]
        public void MockProperty()
        {
            var mock = new Mock<Action>();
            var setup = mock.Setup(x => x());

            Assert.Inc.IsNotNull(MoqExtensions.SetupProperty);
            Assert.IsNotNull(MoqExtensions.MockProperty);

            var sp = MoqExtensions.SetupProperty.GetValue(setup);
            Assert.Inc.IsNotNull(sp);

            var mock2 = MoqExtensions.MockProperty.GetValue(sp);
            Assert.IsNotNull(mock2);
            Assert.AreSame(mock, mock2);
        }

        [TestMethod]
        public void GeneralVerifyMethod()
        {
            Assert.IsNotNull(MoqExtensions.GeneralVerifyMethod);
        }

        [TestMethod]
        public void Verifiable_Times()
        {
            var mock = new Mock<Action>();

            var verifiable = MoqExtensions.Verifiable(mock.Setup(x => x()), Times.Once);

            var ex = Assert.ThrowsException<MockException>(() => verifiable.Verify(null, null));
            Assert.StartsWith($"{Environment.NewLine}Expected invocation on the mock once", ex.Message);
        }

        [TestMethod]
        public void Verifiable_Times_FailMessage()
        {
            var mock = new Mock<Action>();

            var verifiable = MoqExtensions.Verifiable(mock.Setup(x => x()), Times.Once, "My fail message");

            var ex = Assert.ThrowsException<MockException>(() => verifiable.Verify(null, null));
            Assert.StartsWith($"My fail message{Environment.NewLine}Expected invocation on the mock once", ex.Message);
        }

        [TestMethod]
        public void Verifiable_Out_Times()
        {
            var mock = new Mock<Action>();

            MoqExtensions.Verifiable(mock.Setup(x => x()), out var verifiable, Times.Once);

            var ex = Assert.ThrowsException<MockException>(() => verifiable.Verify(null, null));
            Assert.StartsWith($"{Environment.NewLine}Expected invocation on the mock once", ex.Message);
        }

        [TestMethod]
        public void Verifiable_Out_Times_FailMessage()
        {
            var mock = new Mock<Action>();

            MoqExtensions.Verifiable(mock.Setup(x => x()), out var verifiable, Times.Once, "My fail message");

            var ex = Assert.ThrowsException<MockException>(() => verifiable.Verify(null, null));
            Assert.StartsWith($"My fail message{Environment.NewLine}Expected invocation on the mock once", ex.Message);
        }

        [TestMethod]
        public void Verifiable_Collection_Times()
        {
            var mock = new Mock<Action>();
            var collection = new MockVerifiableCollection();

            MoqExtensions.Verifiable(mock.Setup(x => x()), collection, Times.Once);

            var ex = Assert.ThrowsException<MockException>(() => collection.Verify(null, null));
            Assert.StartsWith($"{Environment.NewLine}Expected invocation on the mock once", ex.Message);
        }

        [TestMethod]
        public void Verifiable_Collection_Times_FailMessage()
        {
            var mock = new Mock<Action>();
            var collection = new MockVerifiableCollection();

            MoqExtensions.Verifiable(mock.Setup(x => x()), collection, Times.Once, "My fail message");

            var ex = Assert.ThrowsException<MockException>(() => collection.Verify(null, null));
            Assert.StartsWith($"My fail message{Environment.NewLine}Expected invocation on the mock once", ex.Message);
        }

        [TestMethod]
        public void Verify()
        {
            var mock = new Mock<IMockVerifiable>(MockBehavior.Strict);
            mock.Setup(x => x.Verify(null, null));

            MoqExtensions.Verify(mock.Object);

            mock.Verify(x => x.Verify(null, null), Times.Once);
        }

        [TestMethod]
        public void Verify_FailMessage()
        {
            var mock = new Mock<IMockVerifiable>(MockBehavior.Strict);
            mock.Setup(x => x.Verify(null, "My message"));

            MoqExtensions.Verify(mock.Object, "My message");

            mock.Verify(x => x.Verify(null, "My message"), Times.Once);
        }

        [TestMethod]
        public void Verify_Times()
        {
            var mock = new Mock<IMockVerifiable>(MockBehavior.Strict);
            var expectedTimes = Times.Never();
            mock.Setup(x => x.Verify(It.IsAny<Func<Times>>(), null));

            MoqExtensions.Verify(mock.Object, expectedTimes);

            mock.Verify(x => x.Verify(It.Is<Func<Times>>(y => y() == expectedTimes), null), Times.Once);
        }

        [TestMethod]
        public void Verify_Times_FailMessage()
        {
            var mock = new Mock<IMockVerifiable>(MockBehavior.Strict);
            var expectedTimes = Times.Never();
            mock.Setup(x => x.Verify(It.IsAny<Func<Times>>(), "My message"));

            MoqExtensions.Verify(mock.Object, expectedTimes, "My message");

            mock.Verify(x => x.Verify(It.Is<Func<Times>>(y => y() == expectedTimes), "My message"), Times.Once);
        }

        [TestMethod]
        public void Verify_FuncTimes()
        {
            var mock = new Mock<IMockVerifiable>(MockBehavior.Strict);
            mock.Setup(x => x.Verify(Times.Never, null));

            MoqExtensions.Verify(mock.Object, Times.Never);

            mock.Verify(x => x.Verify(Times.Never, null), Times.Once);
        }
    }
}
