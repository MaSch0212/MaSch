using MaSch.CodeAnalysis.CSharp.SourceGeneration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

namespace MaSch.CodeAnalysis.CSharp.UnitTests.SourceGeneration.Builders;

[TestClass]
public class FieldDeclarationBuilderTests : SourceBuilderTestBase<IFieldDeclarationBuilder>
{
    [TestMethod]
    [SourceBuilderOptions(EnsureEmptyLineBeforeFields = true)]
    public async Task Append_Field_WithLineSeparation()
    {
        Builder.Append(Field("string", "MyField1"));
        Builder.Append(Field("string", "MyField2"));

        await VerifyBuilder();
    }

    [TestMethod]
    [SourceBuilderOptions(EnsureEmptyLineBeforeFields = false)]
    public async Task Append_Field_WithoutLineSeparation()
    {
        Builder.Append(Field("string", "MyField1"));
        Builder.Append(Field("string", "MyField2"));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Field_WithCodeAttributes()
    {
        Builder.Append(Field("string", "MyField1").WithCodeAttribute("Obsolete"));
        Builder.Append(Field("string", "MyField2").WithCodeAttribute("MyAttr", x => x.WithParameters("4711", "\"Test\"")).WithCodeAttribute("Obsolete"));
        Builder.Append(Field("string", "MyField1").WithCodeAttribute("MyAttr3", x => x.OnTarget(CodeAttributeTarget.Field)));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Field_WithAccessModifiersAndKeywords()
    {
        Builder.Append(Field("string", "MyField1").WithAccessModifier(AccessModifier.Public));
        Builder.Append(Field("string", "MyField2").WithKeyword(MemberKeyword.Static));
        Builder.Append(Field("string", "MyField3").WithAccessModifier(AccessModifier.Internal).WithKeyword(MemberKeyword.Sealed));
        Builder.Append(Field("string", "MyField4").WithAccessModifier(AccessModifier.Internal).WithKeyword(MemberKeyword.Sealed).WithKeyword(MemberKeyword.Partial));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Field_WithValue()
    {
        Builder.Append(Field("int", "MyField").WithValue("4711"));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Field_WithEverything()
    {
        Builder.Append(
            Field("int", "MyField")
                .WithCodeAttribute("MyAttr", x => x.WithParameters("4711", "\"Test\""))
                .WithCodeAttribute("Obsolete")
                .WithCodeAttribute("MyAttr3", x => x.OnTarget(CodeAttributeTarget.Field))
                .WithAccessModifier(AccessModifier.Internal)
                .WithKeyword(MemberKeyword.Sealed)
                .WithKeyword(MemberKeyword.Partial)
                .WithValue("4711"));

        await VerifyBuilder();
    }
}
