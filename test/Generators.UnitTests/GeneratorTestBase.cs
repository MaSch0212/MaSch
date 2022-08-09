using Microsoft.CodeAnalysis;

namespace MaSch.Generators.UnitTests;

public abstract class GeneratorTestBase<TGenerator> : TestClassBase
    where TGenerator : class, new()
{
    protected ICreateCompilationBuilder CreateCompilationBuilderWithBuilder()
    {
        var generator = new TGenerator();

        var builder = CompilationBuilder.Create()
            .WithReferencesFromAppDomain(AppDomain.CurrentDomain, x => x.GetName().Name != "MaSch.Generators")
            .WithGenerator(new StaticFilesGenerator());

        return generator switch
        {
            ISourceGenerator sourceGenerator => builder.WithGenerator(sourceGenerator),
            IIncrementalGenerator incrementalGenerator => builder.WithGenerator(incrementalGenerator),
            _ => throw new InvalidOperationException($"Type {typeof(TGenerator).FullName} needs to implement either ISourceGenerator or IIncrementalGenerator."),
        };
    }
}