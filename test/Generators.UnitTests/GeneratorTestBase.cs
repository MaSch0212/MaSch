using Microsoft.CodeAnalysis;
using VerifyMSTest;
using Assert = MaSch.Test.Assertion.Assert;

namespace MaSch.Generators.UnitTests;

public abstract class GeneratorTestBase<TGenerator> : VerifyBase
    where TGenerator : class, new()
{
    protected static Assert Assert => Assert.Instance;

    protected virtual string[] FixedSourceFiles { get; } = Array.Empty<string>();

    protected ICreateCompilationBuilder CreateCompilationBuilderWithBuilder()
    {
        var generator = new TGenerator();

        var builder = OnCreateCompilationBuilder(CompilationBuilder.Create());

        return generator switch
        {
            ISourceGenerator sourceGenerator => builder.WithGenerator(sourceGenerator),
            IIncrementalGenerator incrementalGenerator => builder.WithGenerator(incrementalGenerator),
            _ => throw new InvalidOperationException($"Type {typeof(TGenerator).FullName} needs to implement either ISourceGenerator or IIncrementalGenerator."),
        };
    }

    protected async Task Verify(CompilationResult compilationResult, CompilationVerifyOptions options)
    {
        await compilationResult.Verify((t, s) => Verify(t, s), options);
    }

    protected async Task CompileAndVerify(string sourceCode, CompilationVerifyOptions options)
    {
        var compilationResult = CreateCompilationBuilderWithBuilder()
            .WithSource(sourceCode)
            .Build();

        await Verify(compilationResult, options);
    }

    protected virtual ICreateCompilationBuilder OnCreateCompilationBuilder(ICreateCompilationBuilder builder)
    {
        return builder.WithReference(typeof(string).Assembly);
    }

    protected string[] GetExpectedSourceFiles(params string[] additionalSourceFiles)
    {
        return FixedSourceFiles.Concat(additionalSourceFiles).ToArray();
    }
}