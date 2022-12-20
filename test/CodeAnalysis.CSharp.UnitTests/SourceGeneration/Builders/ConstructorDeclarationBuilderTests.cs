using MaSch.CodeAnalysis.CSharp.SourceGeneration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace MaSch.CodeAnalysis.CSharp.UnitTests.SourceGeneration.Builders;

[TestClass]
public class ConstructorDeclarationBuilderTests : SourceBuilderTestBase<IConstructorDeclarationBuilder>
{
    [TestMethod]
    [SourceBuilderOptions(EnsureEmptyLineBeforeConstructors = true)]
    public async Task Append_Constructor_WithLineSeparation()
    {
        Builder.Append(Constructor("MyType"), DummyConstructorContent);
        Builder.Append(Constructor("MyType"), DummyConstructorContent);

        await VerifyBuilder();
    }

    [TestMethod]
    [SourceBuilderOptions(EnsureEmptyLineBeforeConstructors = false)]
    public async Task Append_Constructor_WithoutLineSeparation()
    {
        Builder.Append(Constructor("MyType"), DummyConstructorContent);
        Builder.Append(Constructor("MyType"), DummyConstructorContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Constructor_AutoTypeName()
    {
        Builder.CurrentTypeName = "AutoTypeNameType";
        Builder.Append(Constructor(), DummyConstructorContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Constructor_WithKeywords()
    {
        Builder.Append(Constructor("MyType1").WithAccessModifier(AccessModifier.Public), DummyConstructorContent);
        Builder.Append(Constructor("MyType2").WithKeyword(MemberKeyword.ReadOnly), DummyConstructorContent);
        Builder.Append(Constructor("MyType3").WithAccessModifier(AccessModifier.Private).WithKeyword(MemberKeyword.New).WithKeyword(MemberKeyword.Extern), DummyConstructorContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Constructor_WithCodeAttributes()
    {
        Builder.Append(Constructor("MyType1").WithCodeAttribute("Obsolete"), DummyConstructorContent);
        Builder.Append(Constructor("MyType2").WithCodeAttribute("MyAttr", x => x.WithParameters("4711", "\"Test\"")).WithCodeAttribute("Obsolete"), DummyConstructorContent);
        Builder.Append(Constructor("MyType3").WithCodeAttribute("MyAttr3", x => x.OnTarget(CodeAttributeTarget.Method)), DummyConstructorContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Constructor_WithParameters()
    {
        Builder.Append(Constructor("MyType1").WithParameter("string", "p1"), DummyConstructorContent);
        Builder.Append(Constructor("MyType2").WithParameter("string", "p1").WithParameter("int", "p2"), DummyConstructorContent);
        Builder.Append(Constructor("MyType3").WithParameter("string", "p1", p => p.WithCodeAttribute("MyAttr1")), DummyConstructorContent);
        Builder.Append(Constructor("MyType4").WithParameter("string", "p1").WithParameter("string", "p2", p => p.WithCodeAttribute("MyAttr1").WithCodeAttribute("MyAttr2", a => a.WithParameters("4711", "\"Test\""))), DummyConstructorContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Constructor_WithParameters_Multiline()
    {
        Builder.Append(Constructor("MyType1").WithMultilineParameters(), DummyConstructorContent);
        Builder.Append(Constructor("MyType2").WithMultilineParameters().WithParameter("string", "p1"), DummyConstructorContent);
        Builder.Append(Constructor("MyType3").WithMultilineParameters().WithParameter("string", "p1").WithParameter("int", "p2"), DummyConstructorContent);
        Builder.Append(Constructor("MyType4").WithMultilineParameters().WithParameter("string", "p1", p => p.WithCodeAttribute("MyAttr1")), DummyConstructorContent);
        Builder.Append(Constructor("MyType5").WithMultilineParameters().WithParameter("string", "p1").WithParameter("string", "p2", p => p.WithCodeAttribute("MyAttr1").WithCodeAttribute("MyAttr2", a => a.WithParameters("4711", "\"Test\""))), DummyConstructorContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Constructor_CallingBase()
    {
        Builder.Append(Constructor("MyType1").CallsBase(), DummyConstructorContent);
        Builder.Append(Constructor("MyType2").WithParameter("string", "p1").CallsBase(s => s.WithParameter("p1")), DummyConstructorContent);
        Builder.Append(Constructor("MyType3").WithParameter("string", "p1").WithParameter("int", "p2").CallsBase(s => s.WithParameters("p1", "p2")), DummyConstructorContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Constructor_CallingThis()
    {
        Builder.Append(Constructor("MyType1").CallsThis(), DummyConstructorContent);
        Builder.Append(Constructor("MyType2").WithParameter("string", "p1").CallsThis(s => s.WithParameter("p1")), DummyConstructorContent);
        Builder.Append(Constructor("MyType3").WithParameter("string", "p1").WithParameter("int", "p2").CallsThis(s => s.WithParameters("p1", "p2")), DummyConstructorContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Constructor_AsExpression()
    {
        Builder.Append(Constructor("MyType1").AsExpression(false), x => x.Append("_myData = \"Hello\""));
        Builder.Append(Constructor("MyType2").AsExpression(true), x => x.Append("_myData = \"Hello\""));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Constructor_WithComments()
    {
        Builder.Append(Constructor("MyType1").WithLineComment("Single Line Line Comment"), DummyConstructorContent);
        Builder.Append(Constructor("MyType2").WithLineComment("Multi Line Line Comment (Line 1)\r\nMulti Line Line Comment (Line 2)\nMulti Line Line Comment (Line 3)"), DummyConstructorContent);
        Builder.Append(Constructor("MyType3").WithBlockComment("Single Line Block Comment"), DummyConstructorContent);
        Builder.Append(Constructor("MyType4").WithBlockComment("Multi Line Block Comment (Line 1)\r\nMulti Line Block Comment (Line 2)\nMulti Line Block Comment (Line 3)"), DummyConstructorContent);
        Builder.Append(Constructor("MyType5").WithDocComment("Single Line Doc Comment"), DummyConstructorContent);
        Builder.Append(Constructor("MyType6").WithDocComment("Multi Line Doc Comment (Line 1)\r\nMulti Line Doc Comment (Line 2)\nMulti Line Doc Comment (Line 3)"), DummyConstructorContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Constructor_WithEverything()
    {
        Builder.CurrentTypeName = "MyType";
        Builder.Append(
            Constructor()
                .WithAccessModifier(AccessModifier.Private)
                .WithKeyword(MemberKeyword.New)
                .WithKeyword(MemberKeyword.Extern)
                .WithCodeAttribute("MyAttr", x => x.WithParameters("4711", "\"Test\""))
                .WithCodeAttribute("Obsolete")
                .WithCodeAttribute("MyAttr3", x => x.OnTarget(CodeAttributeTarget.Method))
                .WithParameter("string", "p1")
                .WithParameter("string", "p2", p => p.WithCodeAttribute("MyAttr1").WithCodeAttribute("MyAttr2", a => a.WithParameters("4711", "\"Test\"")))
                .WithMultilineParameters()
                .CallsBase(x => x.WithParameters("p1", "p2"))
                .AsExpression(),
            x => x.Append("_myData = 4711"));

        await VerifyBuilder();
    }

    [TestMethod]
    [SourceBuilderOptions(EnsureEmptyLineBeforeConstructors = true)]
    public async Task Append_StaticConstructor_WithLineSeparation()
    {
        Builder.Append(StaticConstructor("MyType"), DummyConstructorContent);
        Builder.Append(StaticConstructor("MyType"), DummyConstructorContent);

        await VerifyBuilder();
    }

    [TestMethod]
    [SourceBuilderOptions(EnsureEmptyLineBeforeConstructors = false)]
    public async Task Append_StaticConstructor_WithoutLineSeparation()
    {
        Builder.Append(StaticConstructor("MyType"), DummyConstructorContent);
        Builder.Append(StaticConstructor("MyType"), DummyConstructorContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_StaticConstructor_AutoTypeName()
    {
        Builder.CurrentTypeName = "AutoTypeNameType";
        Builder.Append(StaticConstructor(), DummyConstructorContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_StaticConstructor_WithCodeAttributes()
    {
        Builder.Append(StaticConstructor("MyType1").WithCodeAttribute("Obsolete"), DummyConstructorContent);
        Builder.Append(StaticConstructor("MyType2").WithCodeAttribute("MyAttr", x => x.WithParameters("4711", "\"Test\"")).WithCodeAttribute("Obsolete"), DummyConstructorContent);
        Builder.Append(StaticConstructor("MyType3").WithCodeAttribute("MyAttr3", x => x.OnTarget(CodeAttributeTarget.Method)), DummyConstructorContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_StaticConstructor_AsExpression()
    {
        Builder.Append(StaticConstructor("MyType1").AsExpression(false), x => x.Append("_myData = \"Hello\""));
        Builder.Append(StaticConstructor("MyType2").AsExpression(true), x => x.Append("_myData = \"Hello\""));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_StaticConstructor_WithEverything()
    {
        Builder.Append(
            StaticConstructor("MyType")
                .WithCodeAttribute("MyAttr", x => x.WithParameters("4711", "\"Test\""))
                .WithCodeAttribute("Obsolete")
                .WithCodeAttribute("MyAttr3", x => x.OnTarget(CodeAttributeTarget.Method))
                .AsExpression(true),
            x => x.Append("_myData = \"Hello\""));

        await VerifyBuilder();
    }

    private void DummyConstructorContent(ISourceBuilder builder)
    {
        builder.AppendLine("// Content");
    }
}
