using MaSch.Generators.Shims;

namespace MaSch.Generators.UnitTests;

[TestClass]
public class ShimsGeneratorTests : GeneratorTestBase<Generator>
{
    protected override string[] FixedSourceFiles { get; } = new[] { "Shims.g.cs", "ShimsAttribute.g.cs" };

    [TestMethod]
    public async Task NoShimsAttribute()
    {
        await CompileAndVerify(
            string.Empty,
            new CompilationVerifyOptions
            {
                ExpectedSourceFiles = FixedSourceFiles,
            });
    }

    [TestMethod]
    public async Task None()
    {
        await RunTestWithShims("Shims.None");
    }

    [TestMethod]
    public async Task Records()
    {
        await RunTestWithShims("Shims.Records", "Records.g.cs");
    }

    [TestMethod]
    public async Task IndexAndRange()
    {
        await RunTestWithShims("Shims.IndexAndRange", "IndexAndRange.g.cs");
    }

    [TestMethod]
    public async Task NullableReferenceTypes()
    {
        await RunTestWithShims("Shims.NullableReferenceTypes", "NullableReferenceTypes.g.cs");
    }

    [TestMethod]
    public async Task OSVersioning()
    {
        await RunTestWithShims("Shims.OSVersioning", "OSVersioning.g.cs");
    }

    [TestMethod]
    public async Task CallerArgumentExpression()
    {
        await RunTestWithShims("Shims.CallerArgumentExpression", "CallerArgumentExpression.g.cs");
    }

    [TestMethod]
    public async Task All()
    {
        await RunTestWithShimsAndSkipVerify("Shims.All", "Records.g.cs", "IndexAndRange.g.cs", "NullableReferenceTypes.g.cs", "OSVersioning.g.cs", "CallerArgumentExpression.g.cs");
    }

    [TestMethod]
    public async Task Multiple()
    {
        await RunTestWithShimsAndSkipVerify("Shims.Records | Shims.IndexAndRange", "Records.g.cs", "IndexAndRange.g.cs");
    }

    private async Task RunTestWithShimsAndSkipVerify(string shimsDefinition, params string[] sourceFiles)
        => await RunTestWithShimsImpl(shimsDefinition, sourceFiles, true);

    private async Task RunTestWithShims(string shimsDefinition, params string[] sourceFiles)
        => await RunTestWithShimsImpl(shimsDefinition, sourceFiles, false);

    private async Task RunTestWithShimsImpl(string shimsDefinition, string[] sourceFiles, bool skipVerify)
    {
        await CompileAndVerify(
            $"""
            using MaSch.Core;
            [assembly: Shims({shimsDefinition})]
            """,
            new CompilationVerifyOptions
            {
                ExpectedSourceFiles = GetExpectedSourceFiles(sourceFiles),
                SkipVerifyForFiles = skipVerify ? GetExpectedSourceFiles(sourceFiles) : FixedSourceFiles,
            });
    }
}
