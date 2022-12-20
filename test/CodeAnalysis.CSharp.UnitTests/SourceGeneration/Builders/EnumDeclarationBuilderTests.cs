using MaSch.CodeAnalysis.CSharp.SourceGeneration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

namespace MaSch.CodeAnalysis.CSharp.UnitTests.SourceGeneration.Builders;

[TestClass]
public class EnumDeclarationBuilderTests : SourceBuilderTestBase<IEnumDeclarationBuilder>
{
    [TestMethod]
    [SourceBuilderOptions(EnsureEmptyLineBeforeTypes = true)]
    public async Task Append_Enums_WithLineSeparation()
    {
        Builder.Append(Enum("MyEnum1"), EnumDummyContent);
        Builder.Append(Enum("MyEnum2"), EnumDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    [SourceBuilderOptions(EnsureEmptyLineBeforeTypes = false)]
    public async Task Append_Enums_WithoutLineSeparation()
    {
        Builder.Append(Enum("MyEnum1"), EnumDummyContent);
        Builder.Append(Enum("MyEnum2"), EnumDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Enums_WithCodeAttributes()
    {
        Builder.Append(Enum("MyEnum1").WithCodeAttribute("Obsolete"), EnumDummyContent);
        Builder.Append(Enum("MyEnum2").WithCodeAttribute("MyAttr", x => x.WithParameters("4711", "\"Test\"")).WithCodeAttribute("Obsolete"), EnumDummyContent);
        Builder.Append(Enum("MyEnum3").WithCodeAttribute("MyAttr3", x => x.OnTarget(CodeAttributeTarget.Type)), EnumDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Enums_WithAccessModifiersAndKeywords()
    {
        Builder.Append(Enum("MyEnum1").WithAccessModifier(AccessModifier.Public), EnumDummyContent);
        Builder.Append(Enum("MyEnum2").WithKeyword(MemberKeyword.Static), EnumDummyContent);
        Builder.Append(Enum("MyEnum3").WithAccessModifier(AccessModifier.Internal).WithKeyword(MemberKeyword.Sealed), EnumDummyContent);
        Builder.Append(Enum("MyEnum3").WithAccessModifier(AccessModifier.Internal).WithKeyword(MemberKeyword.Sealed).WithKeyword(MemberKeyword.Partial), EnumDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Enum_WithBaseType()
    {
        Builder.Append(Enum("MyEnum1").DerivesFrom("uint"), EnumDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Delegate_WithComments()
    {
        Builder.Append(Enum("MyEnum1").WithLineComment("Single Line Line Comment"), EnumDummyContent);
        Builder.Append(Enum("MyEnum2").WithLineComment("Multi Line Line Comment (Line 1)\r\nMulti Line Line Comment (Line 2)\nMulti Line Line Comment (Line 3)"), EnumDummyContent);
        Builder.Append(Enum("MyEnum3").WithBlockComment("Single Line Block Comment"), EnumDummyContent);
        Builder.Append(Enum("MyEnum4").WithBlockComment("Multi Line Block Comment (Line 1)\r\nMulti Line Block Comment (Line 2)\nMulti Line Block Comment (Line 3)"), EnumDummyContent);
        Builder.Append(Enum("MyEnum5").WithDocComment("Single Line Doc Comment"), EnumDummyContent);
        Builder.Append(Enum("MyEnum6").WithDocComment("Multi Line Doc Comment (Line 1)\r\nMulti Line Doc Comment (Line 2)\nMulti Line Doc Comment (Line 3)"), EnumDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Enum_WithEverything()
    {
        Builder.Append(
            Enum("MyEnum")
                .WithCodeAttribute("MyAttr", x => x.WithParameters("4711", "\"Test\""))
                .WithCodeAttribute("Obsolete")
                .WithCodeAttribute("MyAttr3", x => x.OnTarget(CodeAttributeTarget.Type))
                .WithAccessModifier(AccessModifier.Internal)
                .WithKeyword(MemberKeyword.Sealed)
                .WithKeyword(MemberKeyword.Partial)
                .DerivesFrom("uint"),
            EnumDummyContent);

        await VerifyBuilder();
    }

    private void EnumDummyContent(IEnumBuilder builder)
    {
        builder.AppendLine("// Enum Content");
    }
}
