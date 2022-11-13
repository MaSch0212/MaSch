using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

namespace MaSch.CodeAnalysis.CSharp.UnitTests.SourceGeneration.Builders;

[TestClass]
public class NamespaceImportBuilderTests : SourceBuilderTestBase<INamespaceImportBuilder>
{
    [TestMethod]
    public async Task Append_NamespaceImport_Common()
    {
        Builder.Append(NamespaceImport("System.Text"));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_NamespaceImport_Static()
    {
        Builder.Append(NamespaceImport("System.Console").AsStatic());

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_NamespaceImport_WithAlias()
    {
        Builder.Append(NamespaceImport("System.Console").WithAlias("cmd"));

        await VerifyBuilder();
    }

    [TestMethod]
    public async Task Append_NamespaceImport_Global()
    {
        Builder.Append(NamespaceImport("System.Text").AsGlobal());
        Builder.Append(NamespaceImport("System.Console").AsGlobal().AsStatic());
        Builder.Append(NamespaceImport("System.Console").AsGlobal().WithAlias("cmd"));

        await VerifyBuilder();
    }
}
