using MaSch.Console.Cli.Configuration;
using MaSch.Console.Cli.Runtime;
using MaSch.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace MaSch.Console.Cli.Test.Runtime
{
    [TestClass]
    public class CliCommandOptionInfoTests : TestClassBase
    {
        [TestMethod]
        public void Ctor_NullChecks()
        {
            var command = Mocks.Create<ICliCommandInfo>();
            var property = typeof(DummyClass).GetProperty(nameof(DummyClass.NormalProperty), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            Assert.ThrowsException<ArgumentNullException>(() => new CliCommandOptionInfo(command.Object, property!, null!));
        }

        [TestMethod]
        public void Ctor()
        {
            var command = Mocks.Create<ICliCommandInfo>();
            var property = typeof(DummyClass).GetProperty(nameof(DummyClass.NormalProperty), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)!;
            var attr = new CliCommandOptionAttribute("blub");

            var option = new CliCommandOptionInfo(command.Object, property, attr);

            Assert.AreSame(attr, option.Attribute);
        }

        [TestMethod]
        public void GetterProperties()
        {
            var command = Mocks.Create<ICliCommandInfo>();
            var property = typeof(DummyClass).GetProperty(nameof(DummyClass.NormalProperty), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)!;
            var attr = new CliCommandOptionAttribute("blub")
            {
                Default = new object(),
                Required = true,
                HelpText = "My Help Text",
                HelpOrder = 4711,
            };

            var option = new CliCommandOptionInfo(command.Object, property, attr);

            Assert.AreSame(attr.Default, option.DefaultValue);
            Assert.IsTrue(option.IsRequired);
            Assert.AreEqual("My Help Text", option.HelpText);
            Assert.AreEqual(4711, option.HelpOrder);
        }

        private class DummyClass
        {
            public int NormalProperty { get; set; }
        }
    }
}
