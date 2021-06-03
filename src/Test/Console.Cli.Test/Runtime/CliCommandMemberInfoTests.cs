using MaSch.Console.Cli.Runtime;
using MaSch.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace MaSch.Console.Cli.Test.Runtime
{
    [TestClass]
    public class CliCommandMemberInfoTests : TestClassBase
    {
        [TestMethod]
        public void Ctor_NullChecks()
        {
            var command = Mocks.Create<ICliCommandInfo>();
            var property = typeof(AbstractDummyClass).GetProperty(nameof(AbstractDummyClass.NormalProperty), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            var ex = Assert.ThrowsException<TargetInvocationException>(() => Mocks.Create<CliCommandMemberInfo>(command.Object, null).Object);
            Assert.IsInstanceOfType<ArgumentNullException>(ex.InnerException);
            ex = Assert.ThrowsException<TargetInvocationException>(() => Mocks.Create<CliCommandMemberInfo>(null, property).Object);
            Assert.IsInstanceOfType<ArgumentNullException>(ex.InnerException);
        }

        [TestMethod]
        public void Ctor_IndexerProperty()
        {
            var command = Mocks.Create<ICliCommandInfo>();
            var property = typeof(AbstractDummyClass).GetProperty("Item", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            Exception ex;
            ex = Assert.ThrowsException<TargetInvocationException>(() => Mocks.Create<CliCommandMemberInfo>(command.Object, property).Object);
            ex = Assert.IsInstanceOfType<ArgumentException>(ex.InnerException);
            Assert.Contains("indexer", ex.Message);
        }

        [TestMethod]
        public void Ctor_StaticProperty()
        {
            var command = Mocks.Create<ICliCommandInfo>();
            var property = typeof(AbstractDummyClass).GetProperty(nameof(AbstractDummyClass.StaticProperty), BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            Exception ex;
            ex = Assert.ThrowsException<TargetInvocationException>(() => Mocks.Create<CliCommandMemberInfo>(command.Object, property).Object);
            ex = Assert.IsInstanceOfType<ArgumentException>(ex.InnerException);
            Assert.ContainsAll(new[] { "static", nameof(AbstractDummyClass.StaticProperty) }, ex.Message);
        }

        [TestMethod]
        public void Ctor_AbstractProperty()
        {
            var command = Mocks.Create<ICliCommandInfo>();
            var property = typeof(AbstractDummyClass).GetProperty(nameof(AbstractDummyClass.AbstractProperty), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            Exception ex;
            ex = Assert.ThrowsException<TargetInvocationException>(() => Mocks.Create<CliCommandMemberInfo>(command.Object, property).Object);
            ex = Assert.IsInstanceOfType<ArgumentException>(ex.InnerException);
            Assert.ContainsAll(new[] { "abstract", nameof(AbstractDummyClass.AbstractProperty) }, ex.Message);
        }

        [TestMethod]
        [DataRow(nameof(AbstractDummyClass.GetOnlyProperty), DisplayName = "Read-Only")]
        [DataRow(nameof(AbstractDummyClass.SetOnlyProperty), DisplayName = "Write-Only")]
        public void Ctor_ReadOrWriteOnlyProperty(string propertyName)
        {
            var command = Mocks.Create<ICliCommandInfo>();
            var property = typeof(AbstractDummyClass).GetProperty(propertyName);

            Exception ex;
            ex = Assert.ThrowsException<TargetInvocationException>(() => Mocks.Create<CliCommandMemberInfo>(command.Object, property).Object);
            ex = Assert.IsInstanceOfType<ArgumentException>(ex.InnerException);
            Assert.ContainsAll(new[] { "setter", "getter", propertyName }, ex.Message);
        }

        [TestMethod]
        public void Ctor_NormalProperty()
        {
            var command = Mocks.Create<ICliCommandInfo>();
            var property = typeof(AbstractDummyClass).GetProperty(nameof(AbstractDummyClass.NormalProperty), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            var member = Mocks.Create<CliCommandMemberInfo>(command.Object, property);
            var po = new PrivateObject(member.Object);

            Assert.AreSame(command.Object, member.Object.Command);
            Assert.AreSame(property, po.GetProperty("Property"));
        }

        [TestMethod]
        public void Ctor_PrivateProperty()
        {
            var command = Mocks.Create<ICliCommandInfo>();
            var property = typeof(AbstractDummyClass).GetProperty("PrivateProperty", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            var member = Mocks.Create<CliCommandMemberInfo>(command.Object, property);
            var po = new PrivateObject(member.Object);

            Assert.AreSame(command.Object, member.Object.Command);
            Assert.AreSame(property, po.GetProperty("Property"));
        }

        [TestMethod]
        public void PropertyInfos()
        {
            var command = Mocks.Create<ICliCommandInfo>();
            var property = typeof(AbstractDummyClass).GetProperty(nameof(AbstractDummyClass.NormalProperty), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            var member = Mocks.Create<CliCommandMemberInfo>(command.Object, property);

            Assert.AreEqual("NormalProperty", member.Object.PropertyName);
            Assert.AreEqual(typeof(int), member.Object.PropertyType);
        }

        [TestMethod]
        public void GetValue_Null()
        {
            var command = Mocks.Create<ICliCommandInfo>();
            var property = typeof(AbstractDummyClass).GetProperty(nameof(AbstractDummyClass.NormalProperty), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            var member = Mocks.Create<CliCommandMemberInfo>(Moq.MockBehavior.Loose, command.Object, property);
            member.CallBase = true;

            Assert.ThrowsException<ArgumentNullException>(() => member.Object.GetValue(null!));
        }

        [TestMethod]
        public void GetValue_WithObject()
        {
            var command = Mocks.Create<ICliCommandInfo>();
            var property = typeof(AbstractDummyClass).GetProperty(nameof(AbstractDummyClass.NormalProperty), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var obj = new DummyClass { NormalProperty = 4711 };

            var member = Mocks.Create<CliCommandMemberInfo>(Moq.MockBehavior.Loose, command.Object, property);
            member.CallBase = true;

            var result = member.Object.GetValue(obj);
            Assert.AreEqual(4711, result);
        }

        [TestMethod]
        public void SetValue_Null()
        {
            var command = Mocks.Create<ICliCommandInfo>();
            var property = typeof(AbstractDummyClass).GetProperty(nameof(AbstractDummyClass.NormalProperty), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            var member = Mocks.Create<CliCommandMemberInfo>(Moq.MockBehavior.Loose, command.Object, property);
            member.CallBase = true;

            Assert.ThrowsException<ArgumentNullException>(() => member.Object.SetValue(null!, null));
        }

        [TestMethod]
        public void SetValue_WithObject_SameType()
        {
            var command = Mocks.Create<ICliCommandInfo>();
            var property = typeof(AbstractDummyClass).GetProperty(nameof(AbstractDummyClass.NormalProperty), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var obj = new DummyClass();

            var member = Mocks.Create<CliCommandMemberInfo>(Moq.MockBehavior.Loose, command.Object, property);
            member.CallBase = true;

            member.Object.SetValue(obj, 1337);
            Assert.AreEqual(1337, obj.NormalProperty);
        }

        [TestMethod]
        public void SetValue_WithObject_Convert()
        {
            var command = Mocks.Create<ICliCommandInfo>();
            var property = typeof(AbstractDummyClass).GetProperty(nameof(AbstractDummyClass.NormalProperty), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var obj = new DummyClass();

            var member = Mocks.Create<CliCommandMemberInfo>(Moq.MockBehavior.Loose, command.Object, property);
            member.CallBase = true;

            member.Object.SetValue(obj, "1337");
            Assert.AreEqual(1337, obj.NormalProperty);
        }

        [TestMethod]
        public void SetValue_WithObject_Enumerable_PrevNull()
        {
            var command = Mocks.Create<ICliCommandInfo>();
            var property = typeof(AbstractDummyClass).GetProperty(nameof(AbstractDummyClass.FloatList), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var obj = new DummyClass();

            var member = Mocks.Create<CliCommandMemberInfo>(Moq.MockBehavior.Loose, command.Object, property);
            member.CallBase = true;

            member.Object.SetValue(obj, GetElements());
            Assert.IsNotNull(obj.FloatList);
            Assert.AreCollectionsEqual(new[] { 1337.1415f, 4711.1415f, 4561.1415f }, obj.FloatList);

            IEnumerable<string> GetElements()
            {
                yield return "1337.1415";
                yield return "4711.1415";
                yield return "4561.1415";
            }
        }

        [TestMethod]
        public void SetValue_WithObject_Enumerable_PrevWithElements()
        {
            var command = Mocks.Create<ICliCommandInfo>();
            var property = typeof(AbstractDummyClass).GetProperty(nameof(AbstractDummyClass.FloatList), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var obj = new DummyClass
            {
                FloatList = new List<float>
                {
                    123.456f,
                    456.789f,
                    789.123f,
                },
            };

            var member = Mocks.Create<CliCommandMemberInfo>(Moq.MockBehavior.Loose, command.Object, property);
            member.CallBase = true;

            member.Object.SetValue(obj, GetElements());
            Assert.IsNotNull(obj.FloatList);
            Assert.AreCollectionsEqual(
                new[]
                {
                    123.456f,
                    456.789f,
                    789.123f,
                    1337.1415f,
                    4711.1415f,
                    4561.1415f,
                },
                obj.FloatList);

            IEnumerable<string> GetElements()
            {
                yield return "1337.1415";
                yield return "4711.1415";
                yield return "4561.1415";
            }
        }

        [TestMethod]
        public void SetDefaultValue_Null()
        {
            var command = Mocks.Create<ICliCommandInfo>();
            var property = typeof(AbstractDummyClass).GetProperty(nameof(AbstractDummyClass.NormalProperty), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            var member = Mocks.Create<CliCommandMemberInfo>(Moq.MockBehavior.Loose, command.Object, property);
            member.CallBase = true;

            Assert.ThrowsException<ArgumentNullException>(() => member.Object.SetDefaultValue(null!));
        }

        [TestMethod]
        public void SetDefaultValue_WithObject_Enumerable()
        {
            var command = Mocks.Create<ICliCommandInfo>();
            var property = typeof(AbstractDummyClass).GetProperty(nameof(AbstractDummyClass.FloatList), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var obj = new DummyClass
            {
                FloatList = new List<float>
                {
                    123.456f,
                    456.789f,
                    789.123f,
                },
            };

            var member = Mocks.Create<CliCommandMemberInfo>(Moq.MockBehavior.Loose, command.Object, property);
            member.CallBase = true;

            member.Object.SetDefaultValue(obj);
            Assert.IsNotNull(obj.FloatList);
            Assert.AreCollectionsEqual(Array.Empty<float>(), obj.FloatList);
        }

        [TestMethod]
        public void SetDefaultValue_WithObject_ReferenceType()
        {
            var command = Mocks.Create<ICliCommandInfo>();
            var property = typeof(AbstractDummyClass).GetProperty(nameof(AbstractDummyClass.ObjectProperty), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var obj = new DummyClass { ObjectProperty = new object() };

            var member = Mocks.Create<CliCommandMemberInfo>(Moq.MockBehavior.Loose, command.Object, property);
            member.CallBase = true;

            member.Object.SetDefaultValue(obj);
            Assert.IsNull(obj.ObjectProperty);
        }

        [TestMethod]
        public void SetDefaultValue_WithObject_ValueType()
        {
            var command = Mocks.Create<ICliCommandInfo>();
            var property = typeof(AbstractDummyClass).GetProperty(nameof(AbstractDummyClass.NormalProperty), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var obj = new DummyClass { NormalProperty = 4711 };

            var member = Mocks.Create<CliCommandMemberInfo>(Moq.MockBehavior.Loose, command.Object, property);
            member.CallBase = true;

            member.Object.SetDefaultValue(obj);
            Assert.AreEqual(0, obj.NormalProperty);
        }

        [TestMethod]
        public void HasValue_Null()
        {
            var command = Mocks.Create<ICliCommandInfo>();
            var property = typeof(AbstractDummyClass).GetProperty(nameof(AbstractDummyClass.NormalProperty), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            var member = Mocks.Create<CliCommandMemberInfo>(Moq.MockBehavior.Loose, command.Object, property);
            member.CallBase = true;

            Assert.ThrowsException<ArgumentNullException>(() => member.Object.HasValue(null!));
        }

        [TestMethod]
        public void HasValue_NeverSet()
        {
            var command = Mocks.Create<ICliCommandInfo>();
            var property = typeof(AbstractDummyClass).GetProperty(nameof(AbstractDummyClass.NormalProperty), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var obj = new DummyClass();

            var member = Mocks.Create<CliCommandMemberInfo>(Moq.MockBehavior.Loose, command.Object, property);
            member.CallBase = true;

            var result = member.Object.HasValue(obj);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HasValue_DefaultSet()
        {
            var command = Mocks.Create<ICliCommandInfo>();
            var property = typeof(AbstractDummyClass).GetProperty(nameof(AbstractDummyClass.NormalProperty), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var obj = new DummyClass();

            var member = Mocks.Create<CliCommandMemberInfo>(Moq.MockBehavior.Loose, command.Object, property);
            member.CallBase = true;

            member.Object.SetDefaultValue(obj);
            var result = member.Object.HasValue(obj);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HasValue_SetValueCalled()
        {
            var command = Mocks.Create<ICliCommandInfo>();
            var property = typeof(AbstractDummyClass).GetProperty(nameof(AbstractDummyClass.NormalProperty), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var obj = new DummyClass();

            var member = Mocks.Create<CliCommandMemberInfo>(Moq.MockBehavior.Loose, command.Object, property);
            member.CallBase = true;

            member.Object.SetValue(obj, 1337);
            var result = member.Object.HasValue(obj);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HasValue_SetValueCalledWithDefaultValue()
        {
            var command = Mocks.Create<ICliCommandInfo>();
            var property = typeof(AbstractDummyClass).GetProperty(nameof(AbstractDummyClass.NormalProperty), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var obj = new DummyClass();

            var member = Mocks.Create<CliCommandMemberInfo>(Moq.MockBehavior.Loose, command.Object, property);
            member.CallBase = true;

            member.Object.SetValue(obj, 0);
            var result = member.Object.HasValue(obj);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HasValue_DefaultSetAfterSetValueCalled()
        {
            var command = Mocks.Create<ICliCommandInfo>();
            var property = typeof(AbstractDummyClass).GetProperty(nameof(AbstractDummyClass.NormalProperty), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var obj = new DummyClass();

            var member = Mocks.Create<CliCommandMemberInfo>(Moq.MockBehavior.Loose, command.Object, property);
            member.CallBase = true;

            member.Object.SetValue(obj, 1337);
            member.Object.SetDefaultValue(obj);
            var result = member.Object.HasValue(obj);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HasValue_DifferentReferenceObjectsWithSameHashCode()
        {
            var command = Mocks.Create<ICliCommandInfo>();
            var property = typeof(AbstractDummyClass).GetProperty(nameof(AbstractDummyClass.NormalProperty), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var obj1 = new DummyClass();
            var obj2 = new DummyClass();

            var member = Mocks.Create<CliCommandMemberInfo>(Moq.MockBehavior.Loose, command.Object, property);
            member.CallBase = true;

            member.Object.SetValue(obj1, 1337);
            var result = member.Object.HasValue(obj2);
            Assert.IsFalse(result);
        }

        [ExcludeFromCodeCoverage]
        private abstract class AbstractDummyClass
        {
            public static int StaticProperty { get; set; }
            public abstract int AbstractProperty { get; set; }
            public int NormalProperty { get; set; }
            public List<float>? FloatList { get; set; }
            public object? ObjectProperty { get; set; }
            public int GetOnlyProperty { get; }
            public int SetOnlyProperty
            {
                set { }
            }

            private int PrivateProperty { get; set; }

            public int this[int idx]
            {
                get => PrivateProperty + idx;
                set { PrivateProperty = value - idx; }
            }

            public override int GetHashCode() => 4711;
            public override bool Equals(object? obj) => true;
        }

        [ExcludeFromCodeCoverage]
        private class DummyClass : AbstractDummyClass
        {
            public override int AbstractProperty { get; set; }
        }
    }
}
