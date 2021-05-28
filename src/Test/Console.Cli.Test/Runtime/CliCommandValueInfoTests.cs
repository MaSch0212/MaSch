using MaSch.Console.Cli.Configuration;
using MaSch.Console.Cli.Runtime;
using MaSch.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MaSch.Console.Cli.Test.Runtime
{
    [TestClass]
    public class CliCommandValueInfoTests : TestClassBase
    {
        [TestMethod]
        public void Ctor_NullChecks()
        {
            var command = Mocks.Create<ICliCommandInfo>();
            var property = typeof(DummyClass).GetProperty(nameof(DummyClass.NormalProperty));

            Assert.ThrowsException<ArgumentNullException>(() => new CliCommandValueInfo(command.Object, property!, null!));
        }

        [TestMethod]
        public void Ctor()
        {
            var command = Mocks.Create<ICliCommandInfo>();
            var property = typeof(DummyClass).GetProperty(nameof(DummyClass.NormalProperty))!;
            var attr = new CliCommandValueAttribute(0, "blub");

            var option = new CliCommandValueInfo(command.Object, property, attr);

            Assert.AreSame(attr, option.Attribute);
        }

        [TestMethod]
        public void GetterProperties()
        {
            var command = Mocks.Create<ICliCommandInfo>();
            var property = typeof(DummyClass).GetProperty(nameof(DummyClass.NormalProperty))!;
            var attr = new CliCommandValueAttribute(4711, "blub")
            {
                Default = new object(),
                Required = true,
                HelpText = "My Help Text",
            };

            var option = new CliCommandValueInfo(command.Object, property, attr);

            Assert.AreSame(attr.Default, option.DefaultValue);
            Assert.IsTrue(option.IsRequired);
            Assert.AreEqual("My Help Text", option.HelpText);
            Assert.AreEqual(4711, option.Order);
        }

        private class DummyClass
        {
            public int NormalProperty { get; set; }
        }
    }
}
