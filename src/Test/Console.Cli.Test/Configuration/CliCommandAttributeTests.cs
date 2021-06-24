using MaSch.Console.Cli.Configuration;
using MaSch.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MaSch.Console.Cli.Test.Configuration
{
    [TestClass]
    public class CliCommandAttributeTests : TestClassBase
    {
        [TestMethod]
        public void Ctor_Name_Success()
        {
            var attr = new CliCommandAttribute("blub");

            Assert.AreCollectionsEqual(new[] { "blub" }, attr.Aliases);
        }

        [TestMethod]
        public void Ctor_Name_Valid_Aliases_Empty()
        {
            var attr = new CliCommandAttribute("blub", Array.Empty<string>());

            Assert.AreCollectionsEqual(new[] { "blub" }, attr.Aliases);
        }

        [TestMethod]
        public void Ctor_Name_Valid_Aliases_NullEmptyAndValid()
        {
            var attr = new CliCommandAttribute("blub", new[] { null!, string.Empty, "blub2" });

            Assert.AreCollectionsEqual(new[] { "blub", "blub2" }, attr.Aliases);
        }

        [TestMethod]
        public void Ctor_Name_Valid_Aliases_SameName()
        {
            var attr = new CliCommandAttribute("blub", new[] { "blub", "blub" });

            Assert.AreCollectionsEqual(new[] { "blub" }, attr.Aliases);
        }

        [TestMethod]
        public void Ctor_Name_Valid_Aliases_SameNameDifferentCasing()
        {
            var attr = new CliCommandAttribute("blub", new[] { "bLub", "blUb" });

            Assert.AreCollectionsEqual(new[] { "blub" }, attr.Aliases);
        }

        [TestMethod]
        public void Ctor_Name_Aliases_Null()
        {
            var attr = new CliCommandAttribute("blub", (string[]?)null!);

            Assert.AreCollectionsEqual(new[] { "blub" }, attr.Aliases);
        }

        [TestMethod]
        public void Name()
        {
            var attr = new CliCommandAttribute("blub", new[] { "blub2", "blub3" });

            Assert.AreEqual("blub", attr.Name);
        }

        [TestMethod]
        public void DefaultValues()
        {
            var attr = new CliCommandAttribute("blub");

            Assert.IsFalse(attr.IsDefault);
            Assert.AreEqual(-1, attr.HelpOrder);
            Assert.IsNull(attr.HelpText);
            Assert.IsNull(attr.ParentCommand);
            Assert.IsFalse(attr.Hidden);
        }
    }
}
