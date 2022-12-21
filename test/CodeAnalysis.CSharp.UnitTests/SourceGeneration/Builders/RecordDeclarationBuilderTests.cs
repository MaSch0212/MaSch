using MaSch.CodeAnalysis.CSharp.SourceGeneration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

namespace MaSch.CodeAnalysis.CSharp.UnitTests.SourceGeneration.Builders;

[TestClass]
public class RecordDeclarationBuilderTests : SourceBuilderTestBase<IRecordDeclarationBuilder>
{
    [TestMethod]
    public async Task Append_SimpleRecord()
    {
        Builder.Append(Record("MyRecord"), RecordDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    [SourceBuilderOptions(EnsureEmptyLineBeforeTypes = true)]
    public async Task Append_RecordsWithLineSeparation()
    {
        Builder.Append(Record("MyRecord1"), RecordDummyContent);
        Builder.Append(Record("MyRecord2"), RecordDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    [SourceBuilderOptions(EnsureEmptyLineBeforeTypes = false)]
    public async Task Append_RecordsWithoutLineSeparation()
    {
        Builder.Append(Record("MyRecord1"), RecordDummyContent);
        Builder.Append(Record("MyRecord2"), RecordDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Records_WithCodeAttributes()
    {
        Builder.Append(Record("MyRecord1").WithCodeAttribute("Obsolete"), RecordDummyContent);
        Builder.Append(Record("MyRecord2").WithCodeAttribute("MyAttr", x => x.WithParameters("4711", "\"Test\"")).WithCodeAttribute("Obsolete"), RecordDummyContent);
        Builder.Append(Record("MyRecord3").WithCodeAttribute("MyAttr3", x => x.OnTarget(CodeAttributeTarget.Type)), RecordDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Records_WithAccessModifiersAndKeywords()
    {
        Builder.Append(Record("MyRecord1").WithAccessModifier(AccessModifier.Public), RecordDummyContent);
        Builder.Append(Record("MyRecord2").WithKeyword(MemberKeyword.Static), RecordDummyContent);
        Builder.Append(Record("MyRecord3").WithAccessModifier(AccessModifier.Internal).WithKeyword(MemberKeyword.Sealed), RecordDummyContent);
        Builder.Append(Record("MyRecord3").WithAccessModifier(AccessModifier.Internal).WithKeyword(MemberKeyword.Sealed).WithKeyword(MemberKeyword.Partial), RecordDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Records_WithGenericParameters()
    {
        Builder.Append(Record("MyRecord1").WithGenericParameter("T"), RecordDummyContent);
        Builder.Append(Record("MyRecord2").WithGenericParameter("T", g => g.WithConstraint("class")), RecordDummyContent);
        Builder.Append(Record("MyRecord3").WithGenericParameter("T", g => g.WithConstraint("class").WithConstraint("IDisposable")), RecordDummyContent);
        Builder.Append(Record("MyRecord4").WithGenericParameter("T1", g => g.WithConstraint("class")).WithGenericParameter("T2", g => g.WithConstraint("struct")), RecordDummyContent);
        Builder.Append(Record("MyRecord5").WithGenericParameter("T1").WithGenericParameter("T2"), RecordDummyContent);
        Builder.Append(Record("MyRecord6").WithGenericParameter("T", g => g.WithVariance(GenericParameterVariance.In)), RecordDummyContent);
        Builder.Append(Record("MyRecord7").WithGenericParameter("T", g => g.WithVariance(GenericParameterVariance.Out)), RecordDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Records_WithBaseTypes()
    {
        Builder.Append(Record("MyRecord1").DerivesFrom("object"), RecordDummyContent);
        Builder.Append(Record("MyRecord2").Implements("IDisposable"), RecordDummyContent);
        Builder.Append(Record("MyRecord3").Implements("IDisposable").Implements("ISerializable"), RecordDummyContent);
        Builder.Append(Record("MyRecord4").DerivesFrom("object").Implements("IDisposable"), RecordDummyContent);
        Builder.Append(Record("MyRecord5").DerivesFrom("object").Implements("IDisposable").Implements("ISerializable"), RecordDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Records_WithParameters()
    {
        Builder.Append(Record("MyRecord1").WithParameter("string", "p1"), RecordDummyContent);
        Builder.Append(Record("MyRecord2").WithParameter("string", "p1").WithParameter("int", "p2"), RecordDummyContent);
        Builder.Append(Record("MyRecord3").WithParameter("string", "p1", p => p.WithCodeAttribute("MyAttr1")), RecordDummyContent);
        Builder.Append(Record("MyRecord4").WithParameter("string", "p1").WithParameter("string", "p2", p => p.WithCodeAttribute("MyAttr1").WithCodeAttribute("MyAttr2", a => a.WithParameters("4711", "\"Test\""))), RecordDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Records_WithSinglelineParameters()
    {
        Builder.Append(Record("MyRecord2").WithSinglelineParameters().WithParameter("string", "p1"), RecordDummyContent);
        Builder.Append(Record("MyRecord3").WithSinglelineParameters().WithParameter("string", "p1").WithParameter("int", "p2"), RecordDummyContent);
        Builder.Append(Record("MyRecord4").WithSinglelineParameters().WithParameter("string", "p1", p => p.WithCodeAttribute("MyAttr1")), RecordDummyContent);
        Builder.Append(Record("MyRecord5").WithSinglelineParameters().WithParameter("string", "p1").WithParameter("string", "p2", p => p.WithCodeAttribute("MyAttr1").WithCodeAttribute("MyAttr2", a => a.WithParameters("4711", "\"Test\""))), RecordDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Records_WithoutBody()
    {
        Builder.Append(Record("MyRecord1"));
        Builder.Append(Record("MyRecord2").WithParameter("string", "DisplayName"));
        Builder.Append(Record("MyRecord3").WithGenericParameter("T", x => x.WithConstraint("class")));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Records_WithComments()
    {
        Builder.Append(Record("MyRecord1").WithLineComment("Single Line Line Comment"), RecordDummyContent);
        Builder.Append(Record("MyRecord2").WithLineComment("Multi Line Line Comment (Line 1)\r\nMulti Line Line Comment (Line 2)\nMulti Line Line Comment (Line 3)"), RecordDummyContent);
        Builder.Append(Record("MyRecord3").WithBlockComment("Single Line Block Comment"), RecordDummyContent);
        Builder.Append(Record("MyRecord4").WithBlockComment("Multi Line Block Comment (Line 1)\r\nMulti Line Block Comment (Line 2)\nMulti Line Block Comment (Line 3)"), RecordDummyContent);
        Builder.Append(Record("MyRecord5").WithDocComment("Single Line Doc Comment"), RecordDummyContent);
        Builder.Append(Record("MyRecord6").WithDocComment("Multi Line Doc Comment (Line 1)\r\nMulti Line Doc Comment (Line 2)\nMulti Line Doc Comment (Line 3)"), RecordDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Record_WithEverything()
    {
        Builder.Append(
            Record("MyRecord")
                .WithCodeAttribute("MyAttr", x => x.WithParameters("4711", "\"Test\""))
                .WithCodeAttribute("Obsolete")
                .WithCodeAttribute("MyAttr3", x => x.OnTarget(CodeAttributeTarget.Type))
                .WithAccessModifier(AccessModifier.Internal)
                .WithKeyword(MemberKeyword.Sealed)
                .WithKeyword(MemberKeyword.Partial)
                .WithParameter("string", "p1", p => p.WithCodeAttribute("MyAttr3", c => c.OnTarget(CodeAttributeTarget.Property)))
                .WithParameter("string", "p2", p => p.WithCodeAttribute("MyAttr1").WithCodeAttribute("MyAttr2", a => a.WithParameters("4711", "\"Test\"")))
                .WithGenericParameter("T1", g => g.WithVariance(GenericParameterVariance.In).WithConstraint("class").WithConstraint("IDisposable"))
                .WithGenericParameter("T2", g => g.WithVariance(GenericParameterVariance.Out).WithConstraint("struct"))
                .DerivesFrom("object")
                .Implements("IDisposable")
                .Implements("ISerializable"),
            RecordDummyContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public void Append_Record_SetsCurrentTypeName()
    {
        Builder.Append(Record("MyRecord"), x =>
        {
            x.CurrentTypeName.Should().Be("MyRecord");
        });
    }

    private void RecordDummyContent(IRecordBuilder builder)
    {
        builder.AppendLine("// Record Content");
    }
}
