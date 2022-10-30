using MaSch.Generators.GeneratorHelpers.Common;
using MaSch.Generators.GeneratorHelpers.Generation;
using MaSch.Generators.GeneratorHelpers.Models;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace MaSch.Generators.GeneratorHelpers;

/// <summary>
/// Generator that generated boilerplate code for classes or structs having one of the attributes:
/// <see cref="T:MaSch.Generators.Support.FileGeneratorAttribute"/>,
/// <see cref="T:MaSch.Generators.Support.MemberGeneratorAttribute"/>,
/// <see cref="T:MaSch.Generators.Support.SyntaxValidatorAttribute"/>,
/// <see cref="T:MaSch.Generators.Support.IncrementalValueProviderFactoryAttribute"/>.
/// </summary>
[Generator]
public class Generator : IIncrementalGenerator
{
    /// <inheritdoc/>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(
            IncrementalValueProviderFactory.From(context).GetClassDeclarations(),
            static (context, data) => Execute(context, data.Compilation, data.Data));
    }

    private static void Execute(SourceProductionContext context, Compilation compilation, ImmutableArray<ValueProviderData> dataCollection)
    {
        try
        {
            if (dataCollection.Length == 0)
                return;

            if (!TypeSymbols.TryCreate(compilation, out var typeSymbols))
                return;

            var generationContext = new GeneratorHelpersContext(context.AddSource);
            var fileGenerator = FileGenerator.Generate(generationContext);
            foreach (var data in dataCollection)
            {
                try
                {
                    GeneratorData.FromValueProviderData(data, compilation, typeSymbols)
                        .Match(
                            fileGenerator.FileGenerator_,
                            fileGenerator.MemberGenerator,
                            fileGenerator.SyntaxValidator,
                            fileGenerator.IncrementalValueProviderFactory);
                }
                catch
                {
                    // Do not generate file if an error occured.
                }
            }
        }
        catch
        {
            // Do not generate any files if an error occured.
        }
    }
}
