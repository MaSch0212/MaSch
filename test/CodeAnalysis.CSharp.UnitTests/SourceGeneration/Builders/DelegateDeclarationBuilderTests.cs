using MaSch.CodeAnalysis.CSharp.SourceGeneration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace MaSch.CodeAnalysis.CSharp.UnitTests.SourceGeneration.Builders;

[TestClass]
public class DelegateDeclarationBuilderTests : SourceBuilderTestBase<IDelegateDeclarationBuilder>
{
    [TestMethod]
    [SourceBuilderOptions(EnsureEmptyLineBeforeTypes = true)]
    public async Task Append_Delegate_WithLineSeparation()
    {
        Builder.Append(Delegate("MyDelegate1"));
        Builder.Append(Delegate("MyDelegate2"));

        await VerifyBuilder();
    }

    [TestMethod]
    [SourceBuilderOptions(EnsureEmptyLineBeforeTypes = false)]
    public async Task Append_Delegate_WithoutLineSeparation()
    {
        Builder.Append(Delegate("MyDelegate1"));
        Builder.Append(Delegate("MyDelegate2"));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Delegate_WithReturnType()
    {
        Builder.Append(Delegate("MyDelegate").WithReturnType("string"));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Delegate_WithCodeAttributes()
    {
        Builder.Append(Delegate("MyDelegate1").WithCodeAttribute("Obsolete"));
        Builder.Append(Delegate("MyDelegate2").WithCodeAttribute("MyAttr", x => x.WithParameters("4711", "\"Test\"")).WithCodeAttribute("Obsolete"));
        Builder.Append(Delegate("MyDelegate3").WithCodeAttribute("MyAttr3", x => x.OnTarget(CodeAttributeTarget.Type)));
        Builder.Append(Delegate("MyDelegate4").WithCodeAttribute("MyAttr4", x => x.OnTarget(CodeAttributeTarget.Return)));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Delegate_WithAccessModifiersAndKeywords()
    {
        Builder.Append(Delegate("MyDelegate1").WithAccessModifier(AccessModifier.Public));
        Builder.Append(Delegate("MyDelegate2").WithKeyword(MemberKeyword.Static));
        Builder.Append(Delegate("MyDelegate3").WithAccessModifier(AccessModifier.Internal).WithKeyword(MemberKeyword.Sealed));
        Builder.Append(Delegate("MyDelegate4").WithAccessModifier(AccessModifier.Internal).WithKeyword(MemberKeyword.Sealed).WithKeyword(MemberKeyword.Partial));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Delegate_WithGenericParameters()
    {
        Builder.Append(Delegate("MyDelegate1").WithGenericParameter("T"));
        Builder.Append(Delegate("MyDelegate2").WithGenericParameter("T", g => g.WithConstraint("class")));
        Builder.Append(Delegate("MyDelegate3").WithGenericParameter("T", g => g.WithConstraint("class").WithConstraint("IDisposable")));
        Builder.Append(Delegate("MyDelegate4").WithGenericParameter("T1", g => g.WithConstraint("class")).WithGenericParameter("T2", g => g.WithConstraint("struct")));
        Builder.Append(Delegate("MyDelegate5").WithGenericParameter("T1").WithGenericParameter("T2"));
        Builder.Append(Delegate("MyDelegate6").WithGenericParameter("T", g => g.WithVariance(GenericParameterVariance.In)));
        Builder.Append(Delegate("MyDelegate7").WithGenericParameter("T", g => g.WithVariance(GenericParameterVariance.Out)));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Delegate_WithParameters()
    {
        Builder.Append(Delegate("MyDelegate1").WithParameter("string", "p1"));
        Builder.Append(Delegate("MyDelegate2").WithParameter("string", "p1").WithParameter("int", "p2"));
        Builder.Append(Delegate("MyDelegate3").WithParameter("string", "p1", p => p.WithCodeAttribute("MyAttr1")));
        Builder.Append(Delegate("MyDelegate4").WithParameter("string", "p1").WithParameter("string", "p2", p => p.WithCodeAttribute("MyAttr1").WithCodeAttribute("MyAttr2", a => a.WithParameters("4711", "\"Test\""))));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Delegate_WithParameters_Multiline()
    {
        Builder.Append(Delegate("MyDelegate1").WithMultilineParameters());
        Builder.Append(Delegate("MyDelegate2").WithMultilineParameters().WithParameter("string", "p1"));
        Builder.Append(Delegate("MyDelegate3").WithMultilineParameters().WithParameter("string", "p1").WithParameter("int", "p2"));
        Builder.Append(Delegate("MyDelegate4").WithMultilineParameters().WithParameter("string", "p1", p => p.WithCodeAttribute("MyAttr1")));
        Builder.Append(Delegate("MyDelegate5").WithMultilineParameters().WithParameter("string", "p1").WithParameter("string", "p2", p => p.WithCodeAttribute("MyAttr1").WithCodeAttribute("MyAttr2", a => a.WithParameters("4711", "\"Test\""))));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Delegate_WithComments()
    {
        Builder.Append(Delegate("MyDelegate1").WithLineComment("Single Line Line Comment"));
        Builder.Append(Delegate("MyDelegate2").WithLineComment("Multi Line Line Comment (Line 1)\r\nMulti Line Line Comment (Line 2)\nMulti Line Line Comment (Line 3)"));
        Builder.Append(Delegate("MyDelegate3").WithBlockComment("Single Line Block Comment"));
        Builder.Append(Delegate("MyDelegate4").WithBlockComment("Multi Line Block Comment (Line 1)\r\nMulti Line Block Comment (Line 2)\nMulti Line Block Comment (Line 3)"));
        Builder.Append(Delegate("MyDelegate5").WithDocComment("Single Line Doc Comment"));
        Builder.Append(Delegate("MyDelegate6").WithDocComment("Multi Line Doc Comment (Line 1)\r\nMulti Line Doc Comment (Line 2)\nMulti Line Doc Comment (Line 3)"));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Delegate_WithEverything()
    {
        Builder.Append(
            Delegate("MyDelegate")
                .WithReturnType("string")
                .WithCodeAttribute("MyAttr", x => x.WithParameters("4711", "\"Test\""))
                .WithCodeAttribute("Obsolete")
                .WithCodeAttribute("MyAttr3", x => x.OnTarget(CodeAttributeTarget.Type))
                .WithCodeAttribute("MyAttr4", x => x.OnTarget(CodeAttributeTarget.Return))
                .WithAccessModifier(AccessModifier.Internal)
                .WithKeyword(MemberKeyword.Sealed)
                .WithKeyword(MemberKeyword.Partial)
                .WithGenericParameter("T1", g => g.WithConstraint("class"))
                .WithGenericParameter("T2", g => g.WithConstraint("struct"))
                .WithParameter("string", "p1")
                .WithParameter("string", "p2", p => p.WithCodeAttribute("MyAttr1").WithCodeAttribute("MyAttr2", a => a.WithParameters("4711", "\"Test\"")))
                .WithMultilineParameters());

        await VerifyBuilder();
    }
}
