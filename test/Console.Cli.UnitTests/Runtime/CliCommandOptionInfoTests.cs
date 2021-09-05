using MaSch.Console.Cli.Configuration;
using MaSch.Console.Cli.Runtime;
using MaSch.Core;
using MaSch.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace MaSch.Console.Cli.UnitTests.Runtime
{
    [TestClass]
    public class CliCommandOptionInfoTests : TestClassBase
    {
        [TestMethod]
        public void Ctor_NullChecks()
        {
            var command = Mocks.Create<ICliCommandInfo>();
            var property = typeof(DummyClass).GetProperty(nameof(DummyClass.NormalProperty), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var extensionStorage = new ObjectExtensionDataStorage();

            _ = Assert.ThrowsException<ArgumentNullException>(() => new CliCommandOptionInfo(extensionStorage, command.Object, property!, null!));
        }

        [TestMethod]
        public void Ctor()
        {
            var command = Mocks.Create<ICliCommandInfo>();
            var property = typeof(DummyClass).GetProperty(nameof(DummyClass.NormalProperty), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)!;
            var attr = new CliCommandOptionAttribute("blub");
            var extensionStorage = new ObjectExtensionDataStorage();

            var option = new CliCommandOptionInfo(extensionStorage, command.Object, property, attr);

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
                Hidden = true,
            };
            var extensionStorage = new ObjectExtensionDataStorage();

            var option = new CliCommandOptionInfo(extensionStorage, command.Object, property, attr);

            Assert.AreSame(attr.Default, option.DefaultValue);
            Assert.IsTrue(option.IsRequired);
            Assert.AreEqual("My Help Text", option.HelpText);
            Assert.AreEqual(4711, option.HelpOrder);
            Assert.IsTrue(option.Hidden);
        }

        [ExcludeFromCodeCoverage]
        private class DummyClass
        {
            public int NormalProperty { get; set; }
        }
    }
}
