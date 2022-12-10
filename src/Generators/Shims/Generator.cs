using MaSch.CodeAnalysis.CSharp.SourceGeneration;
using MaSch.Core;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Threading;
using ShimsEnum = MaSch.Core.Shims;

namespace MaSch.Generators.Shims;

/// <summary>
/// A C# 9 Source Generator that generates shims.
/// </summary>
/// <seealso cref="ISourceGenerator" />
[Generator]
public class Generator : IIncrementalGenerator
{
    /// <inheritdoc/>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(AddPostInitializationSources);

        var shims = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: FilterSyntax,
                transform: Transform)
            .Where(x => x != ShimsEnum.None);

        context.RegisterSourceOutput(shims, Execute);
    }

    private static void AddPostInitializationSources(IncrementalGeneratorPostInitializationContext context)
    {
        new StaticSourceCreator(context.AddSource)
            .AddSource(Resources.Shims)
            .AddSource(Resources.ShimsAttribute);
    }

    private static bool FilterSyntax(SyntaxNode node, CancellationToken cancellation)
    {
        return node is AttributeSyntax attributeSyntax &&
               attributeSyntax.Parent is AttributeListSyntax attributeListSyntax &&
               attributeListSyntax.Target?.Identifier.ValueText == "assembly";
    }

    private static ShimsEnum Transform(GeneratorSyntaxContext context, CancellationToken cancellation)
    {
        if (context.Node is not AttributeSyntax attributeSyntax ||
            context.SemanticModel.GetSymbolInfo(attributeSyntax, cancellation).Symbol is not IMethodSymbol methodSymbol ||
            methodSymbol.ContainingType.ToDisplayString() != typeof(ShimsAttribute).FullName)
        {
            return 0;
        }

        var shimsAttributeData = context.SemanticModel.Compilation.Assembly.GetAttributes().FirstOrDefault(x => SymbolEqualityComparer.Default.Equals(x.AttributeConstructor, methodSymbol));
        return (ShimsEnum)(shimsAttributeData?.ConstructorArguments.FirstOrDefault().Value as int? ?? 0);
    }

    private static void Execute(SourceProductionContext context, ShimsEnum shims)
    {
        var creator = new StaticSourceCreator(context.AddSource);
        if (shims.HasFlag(ShimsEnum.Records))
            creator.AddSource(Resources.Records);
        if (shims.HasFlag(ShimsEnum.IndexAndRange))
            creator.AddSource(Resources.IndexAndRange);
        if (shims.HasFlag(ShimsEnum.NullableReferenceTypes))
            creator.AddSource(Resources.NullableReferenceTypes);
        if (shims.HasFlag(ShimsEnum.OSVersioning))
            creator.AddSource(Resources.OSVersioning);
        if (shims.HasFlag(ShimsEnum.CallerArgumentExpression))
            creator.AddSource(Resources.CallerArgumentExpression);
    }
}
