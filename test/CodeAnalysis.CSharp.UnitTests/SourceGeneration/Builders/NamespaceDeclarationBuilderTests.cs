using MaSch.CodeAnalysis.CSharp.SourceGeneration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

namespace MaSch.CodeAnalysis.CSharp.UnitTests.SourceGeneration.Builders;

[TestClass]
public class NamespaceDeclarationBuilderTests : SourceBuilderTestBase<INamespaceDeclarationBuilder>
{
    [TestMethod]
    [SourceBuilderOptions(EnsureEmptyLineBeforeNamespaces = true)]
    public async Task Append_Namespace_WithLineSeparation()
    {
        Builder.Append(Namespace("MyNamespace1"), DummyNamespaceContent);
        Builder.Append(Namespace("MyNamespace2"), DummyNamespaceContent);

        await VerifyBuilder();
    }

    [TestMethod]
    [SourceBuilderOptions(EnsureEmptyLineBeforeNamespaces = false)]
    public async Task Append_Namespace_WithoutLineSeparation()
    {
        Builder.Append(Namespace("MyNamespace1"), DummyNamespaceContent);
        Builder.Append(Namespace("MyNamespace2"), DummyNamespaceContent);

        await VerifyBuilder();
    }

    private void DummyNamespaceContent(INamespaceBuilder builder)
    {
        builder.AppendLine("// Namespace Content");
    }
}
