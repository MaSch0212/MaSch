using MaSch.Generators.Generators.ObservableObject.Common;
using MaSch.Generators.Generators.ObservableObject.Models;
using Microsoft.CodeAnalysis;

namespace MaSch.Generators.Generators.ObservableObject;

/// <summary>
/// A C# 9 Source Generator that generates properties for observable objects.
/// </summary>
/// <seealso cref="IIncrementalGenerator" />
[Generator]
public class Generator : IIncrementalGenerator
{
    /// <inheritdoc/>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(
            IncrementalValueProviderFactory.From(context).GetClassGenerationInfo(),
            Execute);
    }

    private static void Execute(SourceProductionContext context, GeneratorData info)
    {

    }
}
