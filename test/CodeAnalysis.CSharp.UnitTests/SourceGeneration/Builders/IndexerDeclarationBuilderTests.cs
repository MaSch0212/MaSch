using MaSch.CodeAnalysis.CSharp.SourceGeneration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

namespace MaSch.CodeAnalysis.CSharp.UnitTests.SourceGeneration.Builders;

[TestClass]
public class IndexerDeclarationBuilderTests : SourceBuilderTestBase<IIndexerDeclarationBuilder>
{
    [TestMethod]
    [SourceBuilderOptions(EnsureEmptyLineBeforeProperties = true)]
    public async Task Append_Indexer_WithLineSeparation()
    {
        Builder.Append(Indexer("string"));
        Builder.Append(Indexer("string"));

        await VerifyBuilder();
    }

    [TestMethod]
    [SourceBuilderOptions(EnsureEmptyLineBeforeProperties = false)]
    public async Task Append_Indexer_WithoutLineSeparation()
    {
        Builder.Append(Indexer("string"));
        Builder.Append(Indexer("string"));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Indexer_WithCodeAttributes()
    {
        Builder.Append(Indexer("string").WithCodeAttribute("Obsolete"));
        Builder.Append(Indexer("string").WithCodeAttribute("MyAttr", x => x.WithParameters("4711", "\"Test\"")).WithCodeAttribute("Obsolete"));
        Builder.Append(Indexer("string").WithCodeAttribute("MyAttr3", x => x.OnTarget(CodeAttributeTarget.Property)));
        Builder.Append(Indexer("string").WithCodeAttribute("MyAttr4", x => x.OnTarget(CodeAttributeTarget.Field)));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Indexer_WithAccessModifiersAndKeywords()
    {
        Builder.Append(Indexer("string").WithAccessModifier(AccessModifier.Public));
        Builder.Append(Indexer("string").WithKeyword(MemberKeyword.Static));
        Builder.Append(Indexer("string").WithAccessModifier(AccessModifier.Internal).WithKeyword(MemberKeyword.Sealed));
        Builder.Append(Indexer("string").WithAccessModifier(AccessModifier.Internal).WithKeyword(MemberKeyword.Sealed).WithKeyword(MemberKeyword.Partial));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Indexer_WithValue()
    {
        Builder.Append(Indexer("int").WithValue("4711"));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Indexer_ReadOnly()
    {
        Builder.Append(Indexer("string").AsReadOnly());
        Builder.Append(Indexer("string").AsReadOnly().WithValue("\"Test\""));
        Builder.Append(Indexer("string").AsReadOnly(), x => x.AppendLine("return _data;"));
        Builder.Append(Indexer("string").AsReadOnly().ConfigureGet(x => x.AsExpression()), x => x.Append("_data"));
        Builder.Append(Indexer("string").AsReadOnly().AsExpression(false), x => x.Append("_data"));
        Builder.Append(Indexer("string").AsReadOnly().AsExpression(true), x => x.Append("_data"));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Indexer_WriteOnly()
    {
        Builder.Append(Indexer("string").AsWriteOnly(), x => x.AppendLine("_data = value;"));
        Builder.Append(Indexer("string").AsWriteOnly().ConfigureSet(x => x.AsExpression()), x => x.Append("_data = value"));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Indexer_WithMethodCodeAttributes()
    {
        Builder.Append(Indexer("string").ConfigureGet(g => g.WithCodeAttribute("MyAttr")));
        Builder.Append(Indexer("string").ConfigureSet(g => g.WithCodeAttribute("MyAttr")));
        Builder.Append(Indexer("string").ConfigureGet(g => g.WithCodeAttribute("MyAttr")).ConfigureSet(g => g.WithCodeAttribute("MyAttr")));
        Builder.Append(Indexer("string").AsReadOnly().ConfigureGet(g => g.WithCodeAttribute("MyAttr")));
        Builder.Append(Indexer("string").AsReadOnly().ConfigureGet(g => g.WithCodeAttribute("MyAttr")).WithValue("\"Test\""));
        Builder.Append(Indexer("string").AsReadOnly().ConfigureGet(g => g.WithCodeAttribute("MyAttr")), x => x.AppendLine("return _data;"));
        Builder.Append(Indexer("string").AsReadOnly().ConfigureGet(g => g.WithCodeAttribute("MyAttr").AsExpression()), x => x.Append("_data"));
        Builder.Append(Indexer("string").AsWriteOnly().ConfigureSet(g => g.WithCodeAttribute("MyAttr")), x => x.Append("_data = value;"));
        Builder.Append(Indexer("string").AsWriteOnly().ConfigureSet(g => g.WithCodeAttribute("MyAttr").AsExpression()), x => x.Append("_data = value"));
        Builder.Append(Indexer("string").ConfigureGet(g => g.WithCodeAttribute("MyAttr", a => a.OnTarget(CodeAttributeTarget.Method))).ConfigureSet(g => g.WithCodeAttribute("MyAttr", a => a.OnTarget(CodeAttributeTarget.Method))));
        Builder.Append(Indexer("string").ConfigureGet(g => g.WithCodeAttribute("MyAttr", a => a.OnTarget(CodeAttributeTarget.Return))).ConfigureSet(g => g.WithCodeAttribute("MyAttr", a => a.OnTarget(CodeAttributeTarget.Return))));
        Builder.Append(Indexer("string").ConfigureGet(g => g.WithCodeAttribute("MyAttr")).ConfigureSet(g => g.WithCodeAttribute("MyAttr")), x => x.AppendLine("return _data;"), x => x.AppendLine("_data = value;"));
        Builder.Append(Indexer("string").AsExpression().ConfigureGet(g => g.WithCodeAttribute("MyAttr")).ConfigureSet(g => g.WithCodeAttribute("MyAttr")), x => x.Append("_data"), x => x.Append("_data = value"));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Indexer_Init()
    {
        Builder.Append(Indexer("string").AsInitOnly());
        Builder.Append(Indexer("string").AsWriteOnly().AsInitOnly(), x => x.AppendLine("_data = value;"));
        Builder.Append(Indexer("string").AsWriteOnly().AsInitOnly().AsExpression(), x => x.Append("_data = value"));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Indexer_WithParameters()
    {
        Builder.Append(Indexer("string").WithParameter("int", "i"));
        Builder.Append(Indexer("string").WithParameter("int", "i", p => p.WithCodeAttribute("MyAttr")));
        Builder.Append(Indexer("string").WithParameter("int", "i", p => p.WithCodeAttribute("MyAttr", c => c.WithParameters("4711", "\"Test\""))));
        Builder.Append(Indexer("string").WithParameter("int", "i").WithParameter("string", "s"));
        Builder.Append(Indexer("string").WithParameter("int", "i", p => p.WithCodeAttribute("MyAttr")).WithParameter("string", "s"));
        Builder.Append(Indexer("string").WithParameter("int", "i", p => p.WithCodeAttribute("MyAttr", c => c.WithParameters("4711", "\"Test\""))).WithParameter("string", "s"));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Indexer_WithComments()
    {
        Builder.Append(Indexer("string").WithLineComment("Single Line Line Comment"));
        Builder.Append(Indexer("string").WithLineComment("Multi Line Line Comment (Line 1)\r\nMulti Line Line Comment (Line 2)\nMulti Line Line Comment (Line 3)"));
        Builder.Append(Indexer("string").WithBlockComment("Single Line Block Comment"));
        Builder.Append(Indexer("string").WithBlockComment("Multi Line Block Comment (Line 1)\r\nMulti Line Block Comment (Line 2)\nMulti Line Block Comment (Line 3)"));
        Builder.Append(Indexer("string").WithDocComment("Single Line Doc Comment"));
        Builder.Append(Indexer("string").WithDocComment("Multi Line Doc Comment (Line 1)\r\nMulti Line Doc Comment (Line 2)\nMulti Line Doc Comment (Line 3)"));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Indexer_WithEverything()
    {
        Builder.Append(
            Indexer("string")
                .AsInitOnly()
                .WithCodeAttribute("MyAttr", x => x.WithParameters("4711", "\"Test\""))
                .WithCodeAttribute("Obsolete")
                .WithCodeAttribute("MyAttr3", x => x.OnTarget(CodeAttributeTarget.Property))
                .WithCodeAttribute("MyAttr4", x => x.OnTarget(CodeAttributeTarget.Field))
                .WithAccessModifier(AccessModifier.Internal)
                .WithKeyword(MemberKeyword.Sealed)
                .WithKeyword(MemberKeyword.Partial)
                .ConfigureGet(g => g
                    .WithCodeAttribute("MyAttr")
                    .WithCodeAttribute("MyAttr3", a => a.OnTarget(CodeAttributeTarget.Method))
                    .WithCodeAttribute("MyAttr4", a => a.OnTarget(CodeAttributeTarget.Return)))
                .ConfigureSet(g => g
                    .WithCodeAttribute("MyAttr")
                    .WithCodeAttribute("MyAttr3", a => a.OnTarget(CodeAttributeTarget.Method))
                    .WithCodeAttribute("MyAttr4", a => a.OnTarget(CodeAttributeTarget.Return)))
                .WithValue("\"Test\""));

        await VerifyBuilder();
    }
}
