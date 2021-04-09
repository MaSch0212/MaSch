using MaSch.Test.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace MaSch.Test.Components.Test.Extensions
{
    [TestClass]
    public class MoqExtensionsTests : UnitTestBase
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
    }
}
