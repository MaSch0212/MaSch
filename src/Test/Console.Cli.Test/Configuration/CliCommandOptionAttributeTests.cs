﻿using MaSch.Console.Cli.Configuration;
using MaSch.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSch.Console.Cli.Test.Configuration
{
    [TestClass]
    public class CliCommandOptionAttributeTests : TestClassBase
    {
        [TestMethod]
        public void Ctor_Name_Null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new CliCommandOptionAttribute((string)null!));
        }

        [TestMethod]
        public void Ctor_Name_Empty()
        {
            Assert.ThrowsException<ArgumentException>(() => new CliCommandOptionAttribute(string.Empty));
        }

        [TestMethod]
        public void Ctor_Name_Valid()
        {
            var attr = new CliCommandOptionAttribute("blub");

            Assert.AreCollectionsEqual(new[] { "blub" }, attr.Aliases);
        }

        [TestMethod]
        public void Ctor_ShortName_Name_Null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new CliCommandOptionAttribute('b', null!));
        }

        [TestMethod]
        public void Ctor_ShortName_Name_Empty()
        {
            Assert.ThrowsException<ArgumentException>(() => new CliCommandOptionAttribute('b', string.Empty));
        }

        [TestMethod]
        public void Ctor_ShortName_Name_Valid()
        {
            var attr = new CliCommandOptionAttribute('b', "blub");

            Assert.AreCollectionsEqual(new[] { 'b' }, attr.ShortAliases);
            Assert.AreCollectionsEqual(new[] { "blub" }, attr.Aliases);
        }

        [TestMethod]
        public void Ctor_Names_NoNames()
        {
            Assert.ThrowsException<ArgumentException>(() => new CliCommandOptionAttribute());
        }

        [TestMethod]
        public void Ctor_Names_NoShortNames()
        {
            var attr = new CliCommandOptionAttribute("blub", "blub2");

            Assert.AreCollectionsEqual(Array.Empty<char>(), attr.ShortAliases);
            Assert.AreCollectionsEqual(new[] { "blub", "blub2" }, attr.Aliases);
        }

        [TestMethod]
        public void Ctor_Names_NullsAndEmptyStrings()
        {
            Assert.ThrowsException<ArgumentException>(() => new CliCommandOptionAttribute(null!, string.Empty, null!, string.Empty));
        }

        [TestMethod]
        public void Ctor_Names_NullEmptyAndValid()
        {
            var attr = new CliCommandOptionAttribute(null!, string.Empty, "blub", 'b');

            Assert.AreCollectionsEqual(new[] { 'b' }, attr.ShortAliases);
            Assert.AreCollectionsEqual(new[] { "blub" }, attr.Aliases);
        }

        [TestMethod]
        public void Ctor_Names_SameValues()
        {
            var attr = new CliCommandOptionAttribute("blub", 'b', "blub", 'b');

            Assert.AreCollectionsEqual(new[] { 'b' }, attr.ShortAliases);
            Assert.AreCollectionsEqual(new[] { "blub" }, attr.Aliases);
        }

        [TestMethod]
        public void Ctor_Names_SameValuesWithDifferentCasing()
        {
            var attr = new CliCommandOptionAttribute("blub", 'b', "blUb", 'B');

            Assert.AreCollectionsEqual(new[] { 'b', 'B' }, attr.ShortAliases);
            Assert.AreCollectionsEqual(new[] { "blub" }, attr.Aliases);
        }

        [TestMethod]
        public void ShortName()
        {
            var attr = new CliCommandOptionAttribute("blub", 'b', "blub2", 'B');

            Assert.AreEqual('b', attr.ShortName);
        }

        [TestMethod]
        public void ShortName_Null()
        {
            var attr = new CliCommandOptionAttribute("blub");

            Assert.IsNull(attr.ShortName);
        }

        [TestMethod]
        public void Name()
        {
            var attr = new CliCommandOptionAttribute("blub", 'b', "blub2", 'B');

            Assert.AreEqual("blub", attr.Name);
        }

        [TestMethod]
        public void DefaultValues()
        {
            var attr = new CliCommandOptionAttribute("blub", 'b', "blub2", 'B');

            Assert.IsNull(attr.Default);
            Assert.IsFalse(attr.Required);
            Assert.AreEqual(-1, attr.HelpOrder);
            Assert.IsNull(attr.HelpText);
        }
    }
}
