using MaSch.CodeAnalysis.CSharp.SourceGeneration;

namespace MaSch.CodeAnalysis.CSharp.UnitTests.SourceGeneration.Builders;

[TestClass]
public class SourceFileBuilderTests : SourceBuilderTestBase<ISourceFileBuilder>
{
    [TestMethod]
    [SourceBuilderOptions(EnsureEmptyLineBeforeNamespaces = true)]
    public async Task Append_FileScopedNamespace_WithLineSeparation()
    {
        Builder.AppendLine("// Header");
        Builder.Append(Namespace("My.Fancy.Namespace"));

        await VerifyBuilder();
    }

    [TestMethod]
    [SourceBuilderOptions(EnsureEmptyLineBeforeNamespaces = false)]
    public async Task Append_FileScopedNamespace_WithoutLineSeparation()
    {
        Builder.AppendLine("// Header");
        Builder.Append(Namespace("My.Fancy.Namespace"));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_AssemblyAttributes()
    {
        Builder.Append(AssemblyAttribute("MyAttr1"));
        Builder.Append(AssemblyAttribute("MyAttr2").WithParameter("4711"));
        Builder.Append(AssemblyAttribute("MyAttr3").WithParameters("4711", "\"Test\""));

        await VerifyBuilder();
    }
}
