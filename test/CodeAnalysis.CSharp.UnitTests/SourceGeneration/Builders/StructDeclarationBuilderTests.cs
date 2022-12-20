using MaSch.CodeAnalysis.CSharp.SourceGeneration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

namespace MaSch.CodeAnalysis.CSharp.UnitTests.SourceGeneration.Builders;

[TestClass]
public class StructDeclarationBuilderTests : SourceBuilderTestBase<IStructDeclarationBuilder>
{
    [TestMethod]
    public async Task Append_SimpleStruct()
    {
        Builder.Append(Struct("MyStruct"), StructDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    [SourceBuilderOptions(EnsureEmptyLineBeforeTypes = true)]
    public async Task Append_StructesWithLineSeparation()
    {
        Builder.Append(Struct("MyStruct1"), StructDummyContent);
        Builder.Append(Struct("MyStruct2"), StructDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    [SourceBuilderOptions(EnsureEmptyLineBeforeTypes = false)]
    public async Task Append_StructesWithoutLineSeparation()
    {
        Builder.Append(Struct("MyStruct1"), StructDummyContent);
        Builder.Append(Struct("MyStruct2"), StructDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Structes_WithCodeAttributes()
    {
        Builder.Append(Struct("MyStruct1").WithCodeAttribute("Obsolete"), StructDummyContent);
        Builder.Append(Struct("MyStruct2").WithCodeAttribute("MyAttr", x => x.WithParameters("4711", "\"Test\"")).WithCodeAttribute("Obsolete"), StructDummyContent);
        Builder.Append(Struct("MyStruct3").WithCodeAttribute("MyAttr3", x => x.OnTarget(CodeAttributeTarget.Type)), StructDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Structes_WithAccessModifiersAndKeywords()
    {
        Builder.Append(Struct("MyStruct1").WithAccessModifier(AccessModifier.Public), StructDummyContent);
        Builder.Append(Struct("MyStruct2").WithKeyword(MemberKeyword.Static), StructDummyContent);
        Builder.Append(Struct("MyStruct3").WithAccessModifier(AccessModifier.Internal).WithKeyword(MemberKeyword.Sealed), StructDummyContent);
        Builder.Append(Struct("MyStruct3").WithAccessModifier(AccessModifier.Internal).WithKeyword(MemberKeyword.Sealed).WithKeyword(MemberKeyword.Partial), StructDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Structes_WithGenericParameters()
    {
        Builder.Append(Struct("MyStruct1").WithGenericParameter("T"), StructDummyContent);
        Builder.Append(Struct("MyStruct2").WithGenericParameter("T", g => g.WithConstraint("class")), StructDummyContent);
        Builder.Append(Struct("MyStruct3").WithGenericParameter("T", g => g.WithConstraint("class").WithConstraint("IDisposable")), StructDummyContent);
        Builder.Append(Struct("MyStruct4").WithGenericParameter("T1", g => g.WithConstraint("class")).WithGenericParameter("T2", g => g.WithConstraint("struct")), StructDummyContent);
        Builder.Append(Struct("MyStruct5").WithGenericParameter("T1").WithGenericParameter("T2"), StructDummyContent);
        Builder.Append(Struct("MyStruct6").WithGenericParameter("T", g => g.WithVariance(GenericParameterVariance.In)), StructDummyContent);
        Builder.Append(Struct("MyStruct7").WithGenericParameter("T", g => g.WithVariance(GenericParameterVariance.Out)), StructDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Structes_WithBaseTypes()
    {
        Builder.Append(Struct("MyStruct2").Implements("IDisposable"), StructDummyContent);
        Builder.Append(Struct("MyStruct3").Implements("IDisposable").Implements("ISerializable"), StructDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Structs_WithComments()
    {
        Builder.Append(Struct("MyStruct1").WithLineComment("Single Line Line Comment"), StructDummyContent);
        Builder.Append(Struct("MyStruct2").WithLineComment("Multi Line Line Comment (Line 1)\r\nMulti Line Line Comment (Line 2)\nMulti Line Line Comment (Line 3)"), StructDummyContent);
        Builder.Append(Struct("MyStruct3").WithBlockComment("Single Line Block Comment"), StructDummyContent);
        Builder.Append(Struct("MyStruct4").WithBlockComment("Multi Line Block Comment (Line 1)\r\nMulti Line Block Comment (Line 2)\nMulti Line Block Comment (Line 3)"), StructDummyContent);
        Builder.Append(Struct("MyStruct5").WithDocComment("Single Line Doc Comment"), StructDummyContent);
        Builder.Append(Struct("MyStruct6").WithDocComment("Multi Line Doc Comment (Line 1)\r\nMulti Line Doc Comment (Line 2)\nMulti Line Doc Comment (Line 3)"), StructDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Struct_WithEverything()
    {
        Builder.Append(
            Struct("MyStruct")
                .WithCodeAttribute("MyAttr", x => x.WithParameters("4711", "\"Test\""))
                .WithCodeAttribute("Obsolete")
                .WithCodeAttribute("MyAttr3", x => x.OnTarget(CodeAttributeTarget.Type))
                .WithAccessModifier(AccessModifier.Internal)
                .WithKeyword(MemberKeyword.Sealed)
                .WithKeyword(MemberKeyword.Partial)
                .WithGenericParameter("T1", g => g.WithVariance(GenericParameterVariance.In).WithConstraint("class").WithConstraint("IDisposable"))
                .WithGenericParameter("T2", g => g.WithVariance(GenericParameterVariance.Out).WithConstraint("struct"))
                .Implements("IDisposable")
                .Implements("ISerializable"),
            StructDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public void Append_Struct_SetsCurrentTypeName()
    {
        Builder.Append(Struct("MyStruct"), x =>
        {
            x.CurrentTypeName.Should().Be("MyStruct");
        });
    }

    private void StructDummyContent(IStructBuilder builder)
    {
        builder.AppendLine("// Struct Content");
    }
}
