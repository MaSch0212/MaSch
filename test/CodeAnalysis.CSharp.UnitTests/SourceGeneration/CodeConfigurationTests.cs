using MaSch.CodeAnalysis.CSharp.SourceGeneration;

namespace MaSch.CodeAnalysis.CSharp.UnitTests.SourceGeneration;

[TestClass]
public class CodeConfigurationTests
{
    [TestMethod]
    public void NamespaceImport()
    {
        CodeConfiguration.NamespaceImport("Test").ToString().Should().Be("using Test");
    }

    [TestMethod]
    public void Namespace()
    {
        CodeConfiguration.Namespace("Test").ToString().Should().Be("namespace Test");
    }

    [TestMethod]
    public void Class()
    {
        CodeConfiguration.Class("Test").ToString().Should().Be("class Test");
    }

    [TestMethod]
    public void Record()
    {
        CodeConfiguration.Record("Test").ToString().Should().Be("record Test");
    }

    [TestMethod]
    public void Struct()
    {
        CodeConfiguration.Struct("Test").ToString().Should().Be("struct Test");
    }

    [TestMethod]
    public void Interface()
    {
        CodeConfiguration.Interface("Test").ToString().Should().Be("interface Test");
    }

    [TestMethod]
    public void Enum()
    {
        CodeConfiguration.Enum("Test").ToString().Should().Be("enum Test");
    }

    [TestMethod]
    public void Delegate()
    {
        CodeConfiguration.Delegate("Test").ToString().Should().Be("delegate void Test()");
    }

    [TestMethod]
    public void Field()
    {
        CodeConfiguration.Field("string", "Test").ToString().Should().Be("string Test");
    }

    [TestMethod]
    public void Constructor_WithoutTypeName()
    {
        CodeConfiguration.Constructor().ToString().Should().Be("[ClassName]()");
    }

    [TestMethod]
    public void Constructor_WithTypeName()
    {
        CodeConfiguration.Constructor("Test").ToString().Should().Be("Test()");
    }

    [TestMethod]
    public void StaticConstructor_WithoutTypeName()
    {
        CodeConfiguration.StaticConstructor().ToString().Should().Be("static [ClassName]()");
    }

    [TestMethod]
    public void StaticConstructor_WithTypeName()
    {
        CodeConfiguration.StaticConstructor("Test").ToString().Should().Be("static Test()");
    }

    [TestMethod]
    public void Finalizer_WithoutTypeName()
    {
        CodeConfiguration.Finalizer().ToString().Should().Be("~[ClassName]()");
    }

    [TestMethod]
    public void Finalizer_WithTypeName()
    {
        CodeConfiguration.Finalizer("Test").ToString().Should().Be("~Test()");
    }

    [TestMethod]
    public void Event()
    {
        CodeConfiguration.Event("EventHandler", "Test").ToString().Should().Be("event EventHandler Test");
    }

    [TestMethod]
    public void Property()
    {
        CodeConfiguration.Property("string", "Test").ToString().Should().Be("string Test");
    }

    [TestMethod]
    public void Indexer()
    {
        CodeConfiguration.Indexer("string").ToString().Should().Be("string this[]");
    }

    [TestMethod]
    public void Method_WithoutReturnType()
    {
        CodeConfiguration.Method("Test").ToString().Should().Be("void Test()");
    }

    [TestMethod]
    public void Method_WithReturnType()
    {
        CodeConfiguration.Method("string", "Test").ToString().Should().Be("string Test()");
    }

    [TestMethod]
    public void EnumValue_WithoutValue()
    {
        CodeConfiguration.EnumValue("Test").ToString().Should().Be("Test");
    }

    [TestMethod]
    public void EnumValue_WithValue()
    {
        CodeConfiguration.EnumValue("Test", "4711").ToString().Should().Be("Test = 4711");
    }

    [TestMethod]
    public void AssemblyAttribute()
    {
        CodeConfiguration.AssemblyAttribute("Test").ToString().Should().Be("[assembly: Test]");
    }
}
