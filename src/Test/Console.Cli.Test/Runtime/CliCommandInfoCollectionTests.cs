using MaSch.Console.Cli.Configuration;
using MaSch.Console.Cli.Runtime;
using MaSch.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MaSch.Console.Cli.Test.Runtime
{
    [TestClass]
    public class CliCommandInfoCollectionTests : TestClassBase
    {
        private CliCommandInfoCollection Collection => Cache.GetValue(() => new CliCommandInfoCollection())!;

        [TestMethod]
        public void Ctor()
        {
            var c = new CliCommandInfoCollection();

            Assert.IsNotNull(c);
            Assert.AreCollectionsEqual(Array.Empty<ICliCommandInfo>(), c);
        }

        [TestMethod]
        public void Ctor_Collection_Null()
        {
            var c = new CliCommandInfoCollection(null!);

            Assert.IsNotNull(c);
            Assert.AreCollectionsEqual(Array.Empty<ICliCommandInfo>(), c);
        }

        [TestMethod]
        public void Ctor_Collection_Empty()
        {
            var c = new CliCommandInfoCollection(Array.Empty<ICliCommandInfo>());

            Assert.IsNotNull(c);
            Assert.AreCollectionsEqual(Array.Empty<ICliCommandInfo>(), c);
        }

        [TestMethod]
        public void Ctor_Collection_WithItems()
        {
            var commands = new List<ICliCommandInfo>
            {
                CreateCliCommandInfo<DummyClass1>(new CliCommandAttribute("blub1"), false).Object,
                CreateCliCommandInfo<DummyClass2>(new CliCommandAttribute("blub2"), false).Object,
                CreateCliCommandInfo<DummyClass3>(new CliCommandAttribute("blub3"), false).Object,
            };
            var initialCommands = commands.ToArray();
            var c = new CliCommandInfoCollection(commands);

            Assert.IsNotNull(c);
            Assert.AreCollectionsEqual(commands, c);

            commands.RemoveAt(1);
            Assert.AreCollectionsEqual(initialCommands, c);

            commands.Clear();
            Assert.AreCollectionsEqual(initialCommands, c);
        }

        [TestMethod]
        public void Add_Success_Root()
        {
            var command1 = CreateCliCommandInfo<DummyClass1>(new CliCommandAttribute("blub"), false);

            Collection.Add(command1.Object);

            Assert.AreCollectionsEqual(new[] { command1.Object }, Collection);
        }

        [TestMethod]
        public void Add_Success_DefaultCommand()
        {
            var command1 = CreateCliCommandInfo<DummyClass1>(new CliCommandAttribute("blub"), true);

            Collection.Add(command1.Object);

            Assert.AreCollectionsEqual(new[] { command1.Object }, Collection);
            Assert.AreSame(command1.Object, Collection.DefaultCommand);
        }

        [TestMethod]
        public void Add_Success_SubDefaultCommand()
        {
            var command1 = CreateCliCommandInfo<DummyClass1>(new CliCommandAttribute("blub"), true);
            var command2 = CreateCliCommandInfo<DummyClass2>(new CliCommandAttribute("blub2") { ParentCommand = typeof(DummyClass1) }, true);
            command2.Setup(x => x.ParentCommand).Returns(command1.Object);
            command1.Setup(x => x.ChildCommands).Returns(Array.Empty<ICliCommandInfo>());
            command1.Setup(x => x.AddChildCommand(command2.Object)).Verifiable(Verifiables, Times.Once());

            Collection.Add(command1.Object);
            Collection.Add(command2.Object);

            Assert.AreCollectionsEqual(new[] { command1.Object, command2.Object }, Collection);
            Assert.AreCollectionsEqual(new[] { command1.Object }, Collection.GetRootCommands());
            Assert.AreSame(command1.Object, Collection.DefaultCommand);
        }

        [TestMethod]
        public void Add_Success_ChildCommand()
        {
            var command1 = CreateCliCommandInfo<DummyClass1>(new CliCommandAttribute("blub"), false);
            var command2 = CreateCliCommandInfo<DummyClass2>(new CliCommandAttribute("blub") { ParentCommand = typeof(DummyClass1) }, false);
            command1.Setup(x => x.ChildCommands).Returns(Array.Empty<ICliCommandInfo>());
            command1.Setup(x => x.AddChildCommand(command2.Object)).Verifiable(Verifiables, Times.Once());

            Collection.Add(command1.Object);
            Collection.Add(command2.Object);

            Assert.AreCollectionsEqual(new[] { command1.Object, command2.Object }, Collection);
        }

        [TestMethod]
        public void Add_Null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Collection.Add(null!));
        }

        [TestMethod]
        public void Add_NullCommandType()
        {
            var command = CreateCliCommandInfo(null!, new CliCommandAttribute("blub"), false);

            Assert.ThrowsException<ArgumentException>(() => Collection.Add(command.Object));
        }

        [TestMethod]
        public void Add_NullAttribute()
        {
            var command = CreateCliCommandInfo<DummyClass1>(null!, false);

            Assert.ThrowsException<ArgumentException>(() => Collection.Add(command.Object));
        }

        [TestMethod]
        public void Add_DuplicateCommandType()
        {
            var command1 = CreateCliCommandInfo<DummyClass1>(new CliCommandAttribute("blub"), false);
            var command2 = CreateCliCommandInfo<DummyClass1>(new CliCommandAttribute("blub2"), false);

            Collection.Add(command1.Object);
            var ex = Assert.ThrowsException<ArgumentException>(() => Collection.Add(command2.Object));
            Assert.Contains(typeof(DummyClass1).FullName!, ex.Message);
        }

        [TestMethod]
        public void Add_DuplicateCommandName()
        {
            var command1 = CreateCliCommandInfo<DummyClass1>(new CliCommandAttribute("blub", "blib", "blab"), false);
            var command2 = CreateCliCommandInfo<DummyClass2>(new CliCommandAttribute("blob", "BLUB", "blab"), false);

            Collection.Add(command1.Object);
            var ex = Assert.ThrowsException<ArgumentException>(() => Collection.Add(command2.Object));
            Assert.ContainsAll(
                new[] { "BLUB", "blab", typeof(DummyClass1).FullName!, typeof(DummyClass2).FullName! },
                ex.Message);
        }

        [TestMethod]
        public void Add_DuplicateDefaultCommand()
        {
            var command1 = CreateCliCommandInfo<DummyClass1>(new CliCommandAttribute("blub"), true);
            var command2 = CreateCliCommandInfo<DummyClass2>(new CliCommandAttribute("blub2"), true);

            Collection.Add(command1.Object);
            var ex = Assert.ThrowsException<ArgumentException>(() => Collection.Add(command2.Object));
            Assert.ContainsAll(new[] { "blub2", typeof(DummyClass1).FullName! }, ex.Message);
        }

        [TestMethod]
        public void Add_ChildCommand_MissingParentCommand()
        {
            var command1 = CreateCliCommandInfo<DummyClass2>(new CliCommandAttribute("blub") { ParentCommand = typeof(DummyClass1) }, false);

            var ex = Assert.ThrowsException<ArgumentException>(() => Collection.Add(command1.Object));
            Assert.Contains(typeof(DummyClass1).FullName!, ex.Message);
        }

        [TestMethod]
        public void Count()
        {
            var commands = new List<ICliCommandInfo>
            {
                CreateCliCommandInfo<DummyClass1>(new CliCommandAttribute("blub1"), false).Object,
                CreateCliCommandInfo<DummyClass2>(new CliCommandAttribute("blub2"), false).Object,
                CreateCliCommandInfo<DummyClass3>(new CliCommandAttribute("blub3"), false).Object,
            };
            var c = new CliCommandInfoCollection(commands);

            Assert.AreEqual(3, c.Count);
        }

        [TestMethod]
        public void Remove_Null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Collection.Remove(null!));
        }

        [TestMethod]
        public void Remove_CommandTypeNull()
        {
            var command = CreateCliCommandInfo(null!, new CliCommandAttribute("blub"), false);

            var result = Collection.Remove(command.Object);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Remove_DoesNotContainCommandType()
        {
            var command1 = CreateCliCommandInfo<DummyClass1>(new CliCommandAttribute("blub"), false);
            var command2 = CreateCliCommandInfo<DummyClass2>(new CliCommandAttribute("blub"), false);

            Collection.Add(command1.Object);

            var result = Collection.Remove(command2.Object);

            Assert.IsFalse(result);
            Assert.AreCollectionsEqual(new[] { command1.Object }, Collection);
        }

        [TestMethod]
        public void Remove_ContainsCommandType_UnequalInstance()
        {
            var command1 = CreateCliCommandInfo<DummyClass1>(new CliCommandAttribute("blub"), false);
            var command2 = CreateCliCommandInfo<DummyClass1>(new CliCommandAttribute("blub"), false);

            Collection.Add(command1.Object);

            var result = Collection.Remove(command2.Object);

            Assert.IsFalse(result);
            Assert.AreCollectionsEqual(new[] { command1.Object }, Collection);
        }

        [TestMethod]
        public void Remove_RootCommand()
        {
            var command1 = CreateCliCommandInfo<DummyClass1>(new CliCommandAttribute("blub"), false);
            command1.Setup(x => x.ChildCommands).Returns(Array.Empty<ICliCommandInfo>());
            command1.Setup(x => x.ParentCommand).Returns((ICliCommandInfo?)null);

            Collection.Add(command1.Object);

            var result = Collection.Remove(command1.Object);

            Assert.IsTrue(result);
            Assert.AreCollectionsEqual(Array.Empty<ICliCommandInfo>(), Collection);
        }

        [TestMethod]
        public void Remove_ChildCommand()
        {
            var command1 = CreateCliCommandInfo<DummyClass1>(new CliCommandAttribute("blub"), false);
            var command2 = CreateCliCommandInfo<DummyClass2>(new CliCommandAttribute("blub") { ParentCommand = typeof(DummyClass1) }, false);
            command1.Setup(x => x.ChildCommands).Returns(Array.Empty<ICliCommandInfo>());
            command1.Setup(x => x.AddChildCommand(command2.Object));
            command1.Setup(x => x.RemoveChildCommand(command2.Object)).Verifiable(Verifiables, Times.Once());
            command2.Setup(x => x.ChildCommands).Returns(Array.Empty<ICliCommandInfo>());

            Collection.Add(command1.Object);
            Collection.Add(command2.Object);

            command1.Setup(x => x.ChildCommands).Returns(new[] { command2.Object });
            command2.Setup(x => x.ParentCommand).Returns(command1.Object);

            var result = Collection.Remove(command2.Object);

            Assert.IsTrue(result);
            Assert.AreCollectionsEqual(new[] { command1.Object }, Collection);
        }

        [TestMethod]
        public void Remove_ChildCommand_ParentCommandNull()
        {
            var command1 = CreateCliCommandInfo<DummyClass1>(new CliCommandAttribute("blub"), false);
            var command2 = CreateCliCommandInfo<DummyClass2>(new CliCommandAttribute("blub") { ParentCommand = typeof(DummyClass1) }, false);
            command1.Setup(x => x.ChildCommands).Returns(Array.Empty<ICliCommandInfo>());
            command1.Setup(x => x.AddChildCommand(command2.Object));
            command1.Setup(x => x.RemoveChildCommand(command2.Object)).Verifiable(Verifiables, Times.Once());
            command2.Setup(x => x.ChildCommands).Returns(Array.Empty<ICliCommandInfo>());

            Collection.Add(command1.Object);
            Collection.Add(command2.Object);

            command1.Setup(x => x.ChildCommands).Returns(new[] { command2.Object });
            command2.Setup(x => x.ParentCommand).Returns((ICliCommandInfo?)null);

            var result = Collection.Remove(command2.Object);

            Assert.IsTrue(result);
            Assert.AreCollectionsEqual(new[] { command1.Object }, Collection);
        }

        [TestMethod]
        public void Remove_Recursive()
        {
            var command1 = CreateCliCommandInfo<DummyClass1>(new CliCommandAttribute("blub"), false);
            var command2 = CreateCliCommandInfo<DummyClass2>(new CliCommandAttribute("blub") { ParentCommand = typeof(DummyClass1) }, false);
            var command3 = CreateCliCommandInfo<DummyClass3>(new CliCommandAttribute("blub") { ParentCommand = typeof(DummyClass2) }, false);
            command1.Setup(x => x.ChildCommands).Returns(Array.Empty<ICliCommandInfo>());
            command1.Setup(x => x.AddChildCommand(command2.Object));
            command1.Setup(x => x.RemoveChildCommand(command2.Object)).Verifiable(Verifiables, Times.Once());
            command2.Setup(x => x.ChildCommands).Returns(Array.Empty<ICliCommandInfo>());
            command2.Setup(x => x.AddChildCommand(command3.Object));
            command2.Setup(x => x.RemoveChildCommand(command3.Object)).Verifiable(Verifiables, Times.Once());
            command3.Setup(x => x.ChildCommands).Returns(Array.Empty<ICliCommandInfo>());

            Collection.Add(command1.Object);
            Collection.Add(command2.Object);
            Collection.Add(command3.Object);

            command1.Setup(x => x.ChildCommands).Returns(new[] { command2.Object });
            command1.Setup(x => x.ParentCommand).Returns((ICliCommandInfo?)null);
            command2.Setup(x => x.ChildCommands).Returns(new[] { command3.Object });
            command2.Setup(x => x.ParentCommand).Returns(command1.Object);
            command3.Setup(x => x.ParentCommand).Returns(command2.Object);

            var result = Collection.Remove(command1.Object);

            Assert.IsTrue(result);
            Assert.AreCollectionsEqual(Array.Empty<ICliCommandInfo>(), Collection);
        }

        [TestMethod]
        public void Clear()
        {
            var command1 = CreateCliCommandInfo<DummyClass1>(new CliCommandAttribute("blub"), false);
            var command2 = CreateCliCommandInfo<DummyClass2>(new CliCommandAttribute("blub") { ParentCommand = typeof(DummyClass1) }, false);
            var command3 = CreateCliCommandInfo<DummyClass3>(new CliCommandAttribute("blub2"), false);
            command1.Setup(x => x.ChildCommands).Returns(Array.Empty<ICliCommandInfo>());
            command1.Setup(x => x.AddChildCommand(command2.Object));
            command1.Setup(x => x.RemoveChildCommand(command2.Object)).Verifiable(Verifiables, Times.Once());
            command2.Setup(x => x.ChildCommands).Returns(Array.Empty<ICliCommandInfo>());
            command3.Setup(x => x.ChildCommands).Returns(Array.Empty<ICliCommandInfo>());
            command3.Setup(x => x.ParentCommand).Returns((ICliCommandInfo?)null);

            Collection.Add(command1.Object);
            Collection.Add(command2.Object);
            Collection.Add(command3.Object);

            command1.Setup(x => x.ChildCommands).Returns(new[] { command2.Object });
            command1.Setup(x => x.ParentCommand).Returns((ICliCommandInfo?)null);
            command2.Setup(x => x.ParentCommand).Returns(command1.Object);

            Collection.Clear();

            Assert.AreCollectionsEqual(Array.Empty<ICliCommandInfo>(), Collection);
        }

        [TestMethod]
        public void Contains_Null()
        {
            var result = Collection.Contains(null!);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Contains_CommandTypeNull()
        {
            var command = CreateCliCommandInfo(null!, new CliCommandAttribute("blub"), false);

            var result = Collection.Contains(command.Object);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Contains_DoesNotContainCommandType()
        {
            var command1 = CreateCliCommandInfo<DummyClass1>(new CliCommandAttribute("blub"), false);
            var command2 = CreateCliCommandInfo<DummyClass2>(new CliCommandAttribute("blub"), false);

            Collection.Add(command1.Object);

            var result = Collection.Contains(command2.Object);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Contains_ContainsCommandType_UnequalInstance()
        {
            var command1 = CreateCliCommandInfo<DummyClass1>(new CliCommandAttribute("blub"), false);
            var command2 = CreateCliCommandInfo<DummyClass1>(new CliCommandAttribute("blub"), false);

            Collection.Add(command1.Object);

            var result = Collection.Contains(command2.Object);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CopyTo()
        {
            var arr = new ICliCommandInfo?[4];
            var command1 = CreateCliCommandInfo<DummyClass1>(new CliCommandAttribute("blub"), false);
            var command2 = CreateCliCommandInfo<DummyClass2>(new CliCommandAttribute("blub2"), false);

            Collection.Add(command1.Object);
            Collection.Add(command2.Object);

            Collection.CopyTo(arr!, 1);

            Assert.AreCollectionsEqual(new[] { null, command1.Object, command2.Object, null }, arr);
        }

        [TestMethod]
        public void IEnumerable_GetEnumerator()
        {
            using var enum1 = Collection.GetEnumerator();
            var enum2 = ((IEnumerable)Collection).GetEnumerator();

            Assert.IsInstanceOfType(enum2, enum1.GetType());
        }

        [TestMethod]
        public void GetRootCommands()
        {
            var command1 = CreateCliCommandInfo<DummyClass1>(new CliCommandAttribute("blub"), false);
            var command2 = CreateCliCommandInfo<DummyClass2>(new CliCommandAttribute("blub") { ParentCommand = typeof(DummyClass1) }, false);
            var command3 = CreateCliCommandInfo<DummyClass3>(new CliCommandAttribute("blub2"), false);
            command1.Setup(x => x.ChildCommands).Returns(Array.Empty<ICliCommandInfo>());
            command1.Setup(x => x.AddChildCommand(command2.Object));
            command2.Setup(x => x.ChildCommands).Returns(Array.Empty<ICliCommandInfo>());
            command3.Setup(x => x.ChildCommands).Returns(Array.Empty<ICliCommandInfo>());

            Collection.Add(command1.Object);
            Collection.Add(command2.Object);
            Collection.Add(command3.Object);

            var rootCommands = Collection.GetRootCommands();

            Assert.AreCollectionsEqual(new[] { command1.Object, command3.Object }, rootCommands);
        }

        [TestMethod]
        public void AsReadOnly()
        {
            var command1 = CreateCliCommandInfo<DummyClass1>(new CliCommandAttribute("blub"), false);
            var command2 = CreateCliCommandInfo<DummyClass2>(new CliCommandAttribute("blub2"), false);

            Collection.Add(command1.Object);
            Collection.Add(command2.Object);

            var readOnly = Collection.AsReadOnly();

            Assert.IsNotInstanceOfType<ICollection>(readOnly);
            Assert.IsNotInstanceOfType<ICollection<ICliCommandInfo>>(readOnly);
            Assert.AreEqual(2, readOnly.Count);
            Assert.AreCollectionsEqual(Collection, readOnly);

            using var enum1 = readOnly.GetEnumerator();
            var enum2 = ((IEnumerable)readOnly).GetEnumerator();

            Assert.IsInstanceOfType(enum2, enum1.GetType());
        }

        private Mock<ICliCommandInfo> CreateCliCommandInfo<TCommand>(CliCommandAttribute attribute, bool isDefault)
            => CreateCliCommandInfo(typeof(TCommand), attribute, isDefault);
        private Mock<ICliCommandInfo> CreateCliCommandInfo(Type commandType, CliCommandAttribute attribute, bool isDefault)
        {
            var command = Mocks.Create<ICliCommandInfo>();
            command.Setup(x => x.CommandType).Returns(commandType);
            command.Setup(x => x.Attribute).Returns(attribute);
            command.Setup(x => x.Aliases).Returns(attribute?.Aliases!);
            command.Setup(x => x.Name).Returns(attribute?.Name!);
            command.Setup(x => x.IsDefault).Returns(isDefault);
            command.Setup(x => x.ParentCommand).Returns((ICliCommandInfo?)null);
            return command;
        }

        public class DummyClass1
        {
        }

        public class DummyClass2
        {
        }

        public class DummyClass3
        {
        }
    }
}
