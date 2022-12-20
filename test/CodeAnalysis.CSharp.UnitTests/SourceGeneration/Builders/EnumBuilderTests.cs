using MaSch.CodeAnalysis.CSharp.SourceGeneration;

namespace MaSch.CodeAnalysis.CSharp.UnitTests.SourceGeneration.Builders;

[TestClass]
public class EnumBuilderTests : SourceBuilderTestBase<IEnumBuilder>
{
    [TestMethod]
    public async Task Append_SimpleEnumValues()
    {
        using (Builder.AppendBlock("enum MyEnum"))
        {
            Builder.Append(EnumValue("Value1"));
            Builder.Append(EnumValue("Value2"));
            Builder.Append(EnumValue("Value3"));
        }

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_EnumValuesWithValues()
    {
        using (Builder.AppendBlock("enum MyEnum"))
        {
            Builder.Append(EnumValue("Value1", "0x1"));
            Builder.Append(EnumValue("Value2", "0x2"));
            Builder.Append(EnumValue("Value3", "0x4"));
        }

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_EnumValue_WithCodeAttribute()
    {
        using (Builder.AppendBlock("enum MyEnum"))
        {
            Builder.Append(EnumValue("Value1").WithCodeAttribute("Obsolete"));
            Builder.Append(EnumValue("Value2").WithCodeAttribute("MyAttribute", x => x.WithParameters("4711", "\"Hello\"")).WithCodeAttribute("Obsolete"));
            Builder.Append(EnumValue("Value3").WithCodeAttribute("MyAttr3", x => x.OnTarget(CodeAttributeTarget.Field)));
        }

        await VerifyBuilder();
    }

    [TestMethod]
    [SourceBuilderOptions(EnsureEmptyLineBeforeEnumValues = true)]
    public async Task Append_EnumValuesWithSpaces()
    {
        await Append_EnumValue_WithCodeAttribute();
    }

    [TestMethod]
    public async Task Append_EnumValues_WithComments()
    {
        using (Builder.AppendBlock("enum MyEnum"))
        {
            Builder.Append(EnumValue("Value1").WithLineComment("Single Line Line Comment"));
            Builder.Append(EnumValue("Value2").WithLineComment("Multi Line Line Comment (Line 1)\r\nMulti Line Line Comment (Line 2)\nMulti Line Line Comment (Line 3)"));
            Builder.Append(EnumValue("Value3").WithBlockComment("Single Line Block Comment"));
            Builder.Append(EnumValue("Value4").WithBlockComment("Multi Line Block Comment (Line 1)\r\nMulti Line Block Comment (Line 2)\nMulti Line Block Comment (Line 3)"));
            Builder.Append(EnumValue("Value5").WithDocComment("Single Line Doc Comment"));
            Builder.Append(EnumValue("Value6").WithDocComment("Multi Line Doc Comment (Line 1)\r\nMulti Line Doc Comment (Line 2)\nMulti Line Doc Comment (Line 3)"));
        }

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_EnumValue_WithEverything()
    {
        using (Builder.AppendBlock("enum MyEnum"))
        {
            Builder.Append(
                EnumValue("Value2", "1")
                    .WithCodeAttribute("MyAttribute", x => x.WithParameters("4711", "\"Hello\""))
                    .WithCodeAttribute("Obsolete")
                    .WithCodeAttribute("MyAttr3", x => x.OnTarget(CodeAttributeTarget.Field)));
        }

        await VerifyBuilder();
    }
}
