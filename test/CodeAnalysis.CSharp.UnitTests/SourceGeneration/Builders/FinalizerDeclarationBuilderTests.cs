using MaSch.CodeAnalysis.CSharp.SourceGeneration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

namespace MaSch.CodeAnalysis.CSharp.UnitTests.SourceGeneration.Builders;

[TestClass]
public class FinalizerDeclarationBuilderTests : SourceBuilderTestBase<IFinalizerDeclarationBuilder>
{
    [TestMethod]
    [SourceBuilderOptions(EnsureEmptyLineBeforeConstructors = true)]
    public async Task Append_Finalizer_WithLineSeparation()
    {
        Builder.Append(Finalizer("MyType"), DummyFinalizerContent);
        Builder.Append(Finalizer("MyType"), DummyFinalizerContent);

        await VerifyBuilder();
    }

    [TestMethod]
    [SourceBuilderOptions(EnsureEmptyLineBeforeConstructors = false)]
    public async Task Append_Finalizer_WithoutLineSeparation()
    {
        Builder.Append(Finalizer("MyType"), DummyFinalizerContent);
        Builder.Append(Finalizer("MyType"), DummyFinalizerContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Finalizer_AutoTypeName()
    {
        Builder.CurrentTypeName = "AutoTypeNameType";
        Builder.Append(Finalizer(), DummyFinalizerContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Finalizer_WithCodeAttributes()
    {
        Builder.Append(Finalizer("MyType1").WithCodeAttribute("Obsolete"), DummyFinalizerContent);
        Builder.Append(Finalizer("MyType2").WithCodeAttribute("MyAttr", x => x.WithParameters("4711", "\"Test\"")).WithCodeAttribute("Obsolete"), DummyFinalizerContent);
        Builder.Append(Finalizer("MyType3").WithCodeAttribute("MyAttr3", x => x.OnTarget(CodeAttributeTarget.Method)), DummyFinalizerContent);

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Finalizer_AsExpression()
    {
        Builder.Append(Finalizer("MyType1").AsExpression(false), x => x.Append("_myData = \"Hello\""));
        Builder.Append(Finalizer("MyType2").AsExpression(true), x => x.Append("_myData = \"Hello\""));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_Finalizer_WithEverything()
    {
        Builder.Append(
            Finalizer("MyType")
                .WithCodeAttribute("MyAttr", x => x.WithParameters("4711", "\"Test\""))
                .WithCodeAttribute("Obsolete")
                .WithCodeAttribute("MyAttr3", x => x.OnTarget(CodeAttributeTarget.Method))
                .AsExpression(true),
            x => x.Append("_myData = \"Hello\""));

        await VerifyBuilder();
    }

    private void DummyFinalizerContent(ISourceBuilder builder)
    {
        builder.AppendLine("// Finalizer Content");
    }
}
