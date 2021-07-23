using MaSch.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace MaSch.Test.UnitTests.Extensions
{
    [TestClass]
    public class MoqExtensionsTests : MsTestClassBase
    {
        private static readonly FieldInfo SetupPhraseTypeField = typeof(MoqExtensions).GetField("_setupPhraseType", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly)!;
        private static readonly FieldInfo SetupPropertyField = typeof(MoqExtensions).GetField("_setupProperty", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly)!;
        private static readonly FieldInfo ExpressionPropertyField = typeof(MoqExtensions).GetField("_expressionProperty", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly)!;
        private static readonly FieldInfo MockPropertyField = typeof(MoqExtensions).GetField("_mockProperty", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly)!;
        private static readonly FieldInfo GeneralVerifyMethodField = typeof(MoqExtensions).GetField("_generalVerifyMethod", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly)!;

        protected override void OnInitializeTest()
        {
            base.OnInitializeTest();
            if (TestContext.TestName.StartsWith("Verifiable_Null"))
            {
                Cache.SetValue(SetupPropertyField.GetValue(null), "_setupProperty");
                Cache.SetValue(ExpressionPropertyField.GetValue(null), "_expressionProperty");
                Cache.SetValue(MockPropertyField.GetValue(null), "_mockProperty");
                Cache.SetValue(GeneralVerifyMethodField.GetValue(null), "_generalVerifyMethod");
            }
        }

        protected override void OnCleanupTest()
        {
            base.OnCleanupTest();
            if (TestContext.TestName.StartsWith("Verifiable_Null"))
            {
                SetupPropertyField.SetValue(null, Cache.GetValue<object?>("_setupProperty"));
                ExpressionPropertyField.SetValue(null, Cache.GetValue<object?>("_expressionProperty"));
                MockPropertyField.SetValue(null, Cache.GetValue<object?>("_mockProperty"));
                GeneralVerifyMethodField.SetValue(null, Cache.GetValue<object?>("_generalVerifyMethod"));
            }
        }

        [TestMethod]
        public void SetupPhraseType()
        {
            var mock = new Mock<Action>();
            var setup = mock.Setup(x => x());

            var setupPhraseType = (Type?)SetupPhraseTypeField.GetValue(null);
            Assert.IsNotNull(setupPhraseType);
            Assert.IsInstanceOfType(setup, setupPhraseType);
        }

        [TestMethod]
        public void SetupProperty()
        {
            var mock = new Mock<Action>();
            var setup = mock.Setup(x => x());

            var setupProperty = (PropertyInfo?)SetupPropertyField.GetValue(null);
            Assert.IsNotNull(setupProperty);

            var value = setupProperty.GetValue(setup);
            Assert.IsNotNull(value);
        }

        [TestMethod]
        public void ExpressionProperty()
        {
            var mock = new Mock<Action>();
            var setup = mock.Setup(x => x());

            var setupProperty = (PropertyInfo?)SetupPropertyField.GetValue(null);
            var expressionProperty = (PropertyInfo?)ExpressionPropertyField.GetValue(null);
            Assert.Inc.IsNotNull(setupProperty);
            Assert.IsNotNull(expressionProperty);

            var sp = setupProperty.GetValue(setup);
            Assert.Inc.IsNotNull(sp);

            var expression = expressionProperty.GetValue(sp);
            Assert.IsNotNull(expression);
        }

        [TestMethod]
        public void MockProperty()
        {
            var mock = new Mock<Action>();
            var setup = mock.Setup(x => x());

            var setupProperty = (PropertyInfo?)SetupPropertyField.GetValue(null);
            var mockProperty = (PropertyInfo?)MockPropertyField.GetValue(null);
            Assert.Inc.IsNotNull(setupProperty);
            Assert.IsNotNull(mockProperty);

            var sp = setupProperty.GetValue(setup);
            Assert.Inc.IsNotNull(sp);

            var mock2 = mockProperty.GetValue(sp);
            Assert.IsNotNull(mock2);
            Assert.AreSame(mock, mock2);
        }

        [TestMethod]
        public void GeneralVerifyMethod()
        {
            Assert.IsNotNull(GeneralVerifyMethodField.GetValue(null));
        }

        [TestMethod]
        public void Verifiable_NullSetupProperty()
        {
            SetupPropertyField.SetValue(null, null);
            var mock = new Mock<Action>();

            Assert.ThrowsException<Exception>(() => MoqExtensions.Verifiable(mock.Setup(x => x()), Times.Once()).Dispose());
        }

        [TestMethod]
        public void Verifiable_NullExpressionProperty()
        {
            ExpressionPropertyField.SetValue(null, null);
            var mock = new Mock<Action>();

            Assert.ThrowsException<Exception>(() => MoqExtensions.Verifiable(mock.Setup(x => x()), Times.Once()).Dispose());
        }

        [TestMethod]
        public void Verifiable_NullMockProperty()
        {
            MockPropertyField.SetValue(null, null);
            var mock = new Mock<Action>();

            Assert.ThrowsException<Exception>(() => MoqExtensions.Verifiable(mock.Setup(x => x()), Times.Once()).Dispose());
        }

        [TestMethod]
        public void Verifiable_NullGeneralVerifyMethod()
        {
            GeneralVerifyMethodField.SetValue(null, null);
            var mock = new Mock<Action>();

            var verifiable = MoqExtensions.Verifiable(mock.Setup(x => x()), Times.Once());

            Assert.ThrowsException<Exception>(() => verifiable.Verify(null, null));
        }

        [TestMethod]
        public void Verifiable_Success()
        {
            var mock = new Mock<Action>();

            var verifiable = MoqExtensions.Verifiable(mock.Setup(x => x()), Times.Once());
            mock.Object();

            verifiable.Verify(null, null);
        }

        [TestMethod]
        public void Verifiable_NoParams()
        {
            var mock = new Mock<Action>();

            var verifiable = MoqExtensions.Verifiable(mock.Setup(x => x()));

            var ex = Assert.ThrowsException<MockException>(() => verifiable.Verify(null, null));
            Assert.StartsWith($"{Environment.NewLine}Expected invocation on the mock at least once", ex.Message);
        }

        [TestMethod]
        public void Verifiable_Times()
        {
            var mock = new Mock<Action>();

            var verifiable = MoqExtensions.Verifiable(mock.Setup(x => x()), Times.Once());

            var ex = Assert.ThrowsException<MockException>(() => verifiable.Verify(null, null));
            Assert.StartsWith($"{Environment.NewLine}Expected invocation on the mock once", ex.Message);
        }

        [TestMethod]
        public void Verifiable_Times_FailMessage()
        {
            var mock = new Mock<Action>();

            var verifiable = MoqExtensions.Verifiable(mock.Setup(x => x()), Times.Once(), "My fail message");

            var ex = Assert.ThrowsException<MockException>(() => verifiable.Verify(null, null));
            Assert.StartsWith($"My fail message{Environment.NewLine}Expected invocation on the mock once", ex.Message);
        }

        [TestMethod]
        public void Verifiable_Out_NoParams()
        {
            var mock = new Mock<Action>();

            MoqExtensions.Verifiable(mock.Setup(x => x()), out var verifiable);

            var ex = Assert.ThrowsException<MockException>(() => verifiable.Verify(null, null));
            Assert.StartsWith($"{Environment.NewLine}Expected invocation on the mock at least once", ex.Message);
        }

        [TestMethod]
        public void Verifiable_Out_Times()
        {
            var mock = new Mock<Action>();

            MoqExtensions.Verifiable(mock.Setup(x => x()), out var verifiable, Times.Once());

            var ex = Assert.ThrowsException<MockException>(() => verifiable.Verify(null, null));
            Assert.StartsWith($"{Environment.NewLine}Expected invocation on the mock once", ex.Message);
        }

        [TestMethod]
        public void Verifiable_Out_Times_FailMessage()
        {
            var mock = new Mock<Action>();

            MoqExtensions.Verifiable(mock.Setup(x => x()), out var verifiable, Times.Once(), "My fail message");

            var ex = Assert.ThrowsException<MockException>(() => verifiable.Verify(null, null));
            Assert.StartsWith($"My fail message{Environment.NewLine}Expected invocation on the mock once", ex.Message);
        }

        [TestMethod]
        public void Verifiable_Collection_NoParams()
        {
            var mock = new Mock<Action>();
            var collection = new MockVerifiableCollection();

            MoqExtensions.Verifiable(mock.Setup(x => x()), collection);

            var ex = Assert.ThrowsException<MockException>(() => collection.Verify(null, null));
            Assert.StartsWith($"{Environment.NewLine}Expected invocation on the mock at least once", ex.Message);
        }

        [TestMethod]
        public void Verifiable_Collection_Times()
        {
            var mock = new Mock<Action>();
            var collection = new MockVerifiableCollection();

            MoqExtensions.Verifiable(mock.Setup(x => x()), collection, Times.Once());

            var ex = Assert.ThrowsException<MockException>(() => collection.Verify(null, null));
            Assert.StartsWith($"{Environment.NewLine}Expected invocation on the mock once", ex.Message);
        }

        [TestMethod]
        public void Verifiable_Collection_Times_FailMessage()
        {
            var mock = new Mock<Action>();
            var collection = new MockVerifiableCollection();

            MoqExtensions.Verifiable(mock.Setup(x => x()), collection, Times.Once(), "My fail message");

            var ex = Assert.ThrowsException<MockException>(() => collection.Verify(null, null));
            Assert.StartsWith($"My fail message{Environment.NewLine}Expected invocation on the mock once", ex.Message);
        }

        [TestMethod]
        public void Verifiable_TestClass_NoParams()
        {
            var mock = new Mock<Action>();
            var testClass = new Mock<TestClassBase>(MockBehavior.Loose) { CallBase = true };

            MoqExtensions.Verifiable(mock.Setup(x => x()), testClass.Object);

            var ex = Assert.ThrowsException<MockException>(() => testClass.Object.Verifiables.Verify(null, null));
            Assert.StartsWith($"{Environment.NewLine}Expected invocation on the mock at least once", ex.Message);
        }

        [TestMethod]
        public void Verifiable_TestClass_Times()
        {
            var mock = new Mock<Action>();
            var testClass = new Mock<TestClassBase>(MockBehavior.Loose) { CallBase = true };

            MoqExtensions.Verifiable(mock.Setup(x => x()), testClass.Object, Times.Once());

            var ex = Assert.ThrowsException<MockException>(() => testClass.Object.Verifiables.Verify(null, null));
            Assert.StartsWith($"{Environment.NewLine}Expected invocation on the mock once", ex.Message);
        }

        [TestMethod]
        public void Verifiable_TestClass_Times_FailMessage()
        {
            var mock = new Mock<Action>();
            var testClass = new Mock<TestClassBase>(MockBehavior.Loose) { CallBase = true };

            MoqExtensions.Verifiable(mock.Setup(x => x()), testClass.Object, Times.Once(), "My fail message");

            var ex = Assert.ThrowsException<MockException>(() => testClass.Object.Verifiables.Verify(null, null));
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
            mock.Setup(x => x.Verify(It.IsAny<Times>(), null));

            MoqExtensions.Verify(mock.Object, expectedTimes);

            mock.Verify(x => x.Verify(It.Is<Times>(y => y == expectedTimes), null), Times.Once);
        }

        [TestMethod]
        public void Verify_FuncTimes()
        {
            var mock = new Mock<IMockVerifiable>(MockBehavior.Strict);
            mock.Setup(x => x.Verify(Times.Never(), null));

            MoqExtensions.Verify(mock.Object, Times.Never());

            mock.Verify(x => x.Verify(Times.Never(), null), Times.Once);
        }
    }
}
