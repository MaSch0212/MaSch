using MaSch.Core;
using MaSch.Generators.Common;
using MaSch.Generators.Properties;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Threading;

namespace MaSch.Generators;

/// <summary>
/// A C# 9 Source Generator that generates shims.
/// </summary>
/// <seealso cref="ISourceGenerator" />
[Generator]
public class ShimsGenerator : IIncrementalGenerator
{
    /// <inheritdoc/>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var shims = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: FilterSyntax,
                transform: Transform)
            .Where(x => x != Shims.None);

        context.RegisterSourceOutput(shims, (ctx, source) => Execute(ctx, source));
    }

    private static bool FilterSyntax(SyntaxNode node, CancellationToken cancellation)
    {
        return node is AttributeSyntax attributeSyntax &&
               attributeSyntax.Parent is AttributeListSyntax attributeListSyntax &&
               attributeListSyntax.Target?.Identifier.ValueText == "assembly";
    }

    private static Shims Transform(GeneratorSyntaxContext context, CancellationToken cancellation)
    {
        if (context.Node is not AttributeSyntax attributeSyntax ||
            context.SemanticModel.GetSymbolInfo(attributeSyntax, cancellation).Symbol is not IMethodSymbol methodSymbol ||
            methodSymbol.ContainingType.ToDisplayString() != typeof(ShimsAttribute).FullName)
        {
            return 0;
        }

        var shimsAttributeData = context.SemanticModel.Compilation.Assembly.GetAttributes().FirstOrDefault(x => SymbolEqualityComparer.Default.Equals(x.AttributeConstructor, methodSymbol));
        return (Shims)(shimsAttributeData?.ConstructorArguments.FirstOrDefault().Value as int? ?? 0);
    }

    private static void Execute(SourceProductionContext context, Shims shims)
    {
        var creator = new StaticSourceCreator(context.AddSource);
        if (shims.HasFlag(Shims.Records))
            creator.AddSource(Resources.Records);
        if (shims.HasFlag(Shims.IndexAndRange))
            creator.AddSource(Resources.IndexAndRange);
        if (shims.HasFlag(Shims.NullableReferenceTypes))
            creator.AddSource(Resources.NullableReferenceTypes);
        if (shims.HasFlag(Shims.OSVersioning))
            creator.AddSource(Resources.OSVersioning);
        if (shims.HasFlag(Shims.CallerArgumentExpression))
            creator.AddSource(Resources.CallerArgumentExpression);
    }
}
