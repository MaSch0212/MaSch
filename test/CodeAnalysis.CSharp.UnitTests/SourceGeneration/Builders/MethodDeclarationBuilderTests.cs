using MaSch.CodeAnalysis.CSharp.SourceGeneration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

namespace MaSch.CodeAnalysis.CSharp.UnitTests.SourceGeneration.Builders;

[TestClass]
public class MethodDeclarationBuilderTests : SourceBuilderTestBase<IMethodDeclarationBuilder>
{
    [TestMethod]
    [SourceBuilderOptions(EnsureEmptyLineBeforeMethods = true)]
    public async Task Append_Method_WithLineSeparation()
    {
        Builder.Append(Method("MyMethod1"), DummyMethodContent);
        Builder.Append(Method("MyMethod2"), DummyMethodContent);

        await VerifyBuilder();
    }

    [TestMethod]
    [SourceBuilderOptions(EnsureEmptyLineBeforeMethods = false)]
    public async Task Append_Method_WithoutLineSeparation()
    {
        Builder.Append(Method("MyMethod1"), DummyMethodContent);
        Builder.Append(Method("MyMethod2"), DummyMethodContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Method_WithoutBody()
    {
        Builder.Append(Method("MyPartialMethod").WithKeyword(MemberKeyword.Partial));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Method_WithReturnType()
    {
        Builder.Append(Method("string", "MyMethod"), DummyMethodContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Method_WithCodeAttributes()
    {
        Builder.Append(Method("MyMethod1").WithCodeAttribute("Obsolete"), DummyMethodContent);
        Builder.Append(Method("MyMethod2").WithCodeAttribute("MyAttr", x => x.WithParameters("4711", "\"Test\"")).WithCodeAttribute("Obsolete"), DummyMethodContent);
        Builder.Append(Method("MyMethod3").WithCodeAttribute("MyAttr3", x => x.OnTarget(CodeAttributeTarget.Method)), DummyMethodContent);
        Builder.Append(Method("MyMethod4").WithCodeAttribute("MyAttr4", x => x.OnTarget(CodeAttributeTarget.Return)), DummyMethodContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Method_WithAccessModifiersAndKeywords()
    {
        Builder.Append(Method("MyMethod1").WithAccessModifier(AccessModifier.Public), DummyMethodContent);
        Builder.Append(Method("MyMethod2").WithKeyword(MemberKeyword.Static), DummyMethodContent);
        Builder.Append(Method("MyMethod3").WithAccessModifier(AccessModifier.Internal).WithKeyword(MemberKeyword.Sealed), DummyMethodContent);
        Builder.Append(Method("MyMethod4").WithAccessModifier(AccessModifier.Internal).WithKeyword(MemberKeyword.Sealed).WithKeyword(MemberKeyword.Partial), DummyMethodContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Method_WithGenericParameters()
    {
        Builder.Append(Method("MyMethod1").WithGenericParameter("T"), DummyMethodContent);
        Builder.Append(Method("MyMethod2").WithGenericParameter("T", g => g.WithConstraint("class")), DummyMethodContent);
        Builder.Append(Method("MyMethod3").WithGenericParameter("T", g => g.WithConstraint("class").WithConstraint("IDisposable")), DummyMethodContent);
        Builder.Append(Method("MyMethod4").WithGenericParameter("T1", g => g.WithConstraint("class")).WithGenericParameter("T2", g => g.WithConstraint("struct")), DummyMethodContent);
        Builder.Append(Method("MyMethod5").WithGenericParameter("T1").WithGenericParameter("T2"), DummyMethodContent);
        Builder.Append(Method("MyMethod6").WithGenericParameter("T", g => g.WithVariance(GenericParameterVariance.In)), DummyMethodContent);
        Builder.Append(Method("MyMethod7").WithGenericParameter("T", g => g.WithVariance(GenericParameterVariance.Out)), DummyMethodContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Method_WithParameters()
    {
        Builder.Append(Method("MyMethod1").WithParameter("string", "p1"), DummyMethodContent);
        Builder.Append(Method("MyMethod2").WithParameter("string", "p1").WithParameter("int", "p2"), DummyMethodContent);
        Builder.Append(Method("MyMethod3").WithParameter("string", "p1", p => p.WithCodeAttribute("MyAttr1")), DummyMethodContent);
        Builder.Append(Method("MyMethod4").WithParameter("string", "p1").WithParameter("string", "p2", p => p.WithCodeAttribute("MyAttr1").WithCodeAttribute("MyAttr2", a => a.WithParameters("4711", "\"Test\""))), DummyMethodContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Method_WithParameters_Multiline()
    {
        Builder.Append(Method("MyMethod1").WithMultilineParameters(), DummyMethodContent);
        Builder.Append(Method("MyMethod2").WithMultilineParameters().WithParameter("string", "p1"), DummyMethodContent);
        Builder.Append(Method("MyMethod3").WithMultilineParameters().WithParameter("string", "p1").WithParameter("int", "p2"), DummyMethodContent);
        Builder.Append(Method("MyMethod4").WithMultilineParameters().WithParameter("string", "p1", p => p.WithCodeAttribute("MyAttr1")), DummyMethodContent);
        Builder.Append(Method("MyMethod5").WithMultilineParameters().WithParameter("string", "p1").WithParameter("string", "p2", p => p.WithCodeAttribute("MyAttr1").WithCodeAttribute("MyAttr2", a => a.WithParameters("4711", "\"Test\""))), DummyMethodContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Method_WithComments()
    {
        Builder.Append(Method("MyMethod1").WithLineComment("Single Line Line Comment"), DummyMethodContent);
        Builder.Append(Method("MyMethod2").WithLineComment("Multi Line Line Comment (Line 1)\r\nMulti Line Line Comment (Line 2)\nMulti Line Line Comment (Line 3)"), DummyMethodContent);
        Builder.Append(Method("MyMethod3").WithBlockComment("Single Line Block Comment"), DummyMethodContent);
        Builder.Append(Method("MyMethod4").WithBlockComment("Multi Line Block Comment (Line 1)\r\nMulti Line Block Comment (Line 2)\nMulti Line Block Comment (Line 3)"), DummyMethodContent);
        Builder.Append(Method("MyMethod5").WithDocComment("Single Line Doc Comment"), DummyMethodContent);
        Builder.Append(Method("MyMethod6").WithDocComment("Multi Line Doc Comment (Line 1)\r\nMulti Line Doc Comment (Line 2)\nMulti Line Doc Comment (Line 3)"), DummyMethodContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Method_WithEverything()
    {
        Builder.Append(
            Method("MyMethod")
                .WithReturnType("string")
                .WithCodeAttribute("MyAttr", x => x.WithParameters("4711", "\"Test\""))
                .WithCodeAttribute("Obsolete")
                .WithCodeAttribute("MyAttr3", x => x.OnTarget(CodeAttributeTarget.Method))
                .WithCodeAttribute("MyAttr4", x => x.OnTarget(CodeAttributeTarget.Return))
                .WithAccessModifier(AccessModifier.Internal)
                .WithKeyword(MemberKeyword.Sealed)
                .WithKeyword(MemberKeyword.Partial)
                .WithGenericParameter("T1", g => g.WithConstraint("class"))
                .WithGenericParameter("T2", g => g.WithConstraint("struct"))
                .WithParameter("string", "p1")
                .WithParameter("string", "p2", p => p.WithCodeAttribute("MyAttr1").WithCodeAttribute("MyAttr2", a => a.WithParameters("4711", "\"Test\"")))
                .WithParameter("string", "p3", p => p.WithDefaultValue("null"))
                .WithMultilineParameters()
                .AsExpression(),
            x => x.Append("Something()"));

        await VerifyBuilder();
    }

    private void DummyMethodContent(ISourceBuilder builder)
    {
        builder.AppendLine("// Method Content");
    }
}
