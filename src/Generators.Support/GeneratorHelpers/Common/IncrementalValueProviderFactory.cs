using MaSch.Generators.GeneratorHelpers.Models;
using MaSch.Generators.Support;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace MaSch.Generators.GeneratorHelpers.Common;

internal readonly struct IncrementalValueProviderFactory
{
    private static readonly string FileGeneratorAttributeName = typeof(FileGeneratorAttribute).FullName;
    private static readonly string MemberGeneratorAttributeName = typeof(MemberGeneratorAttribute).FullName;
    private static readonly string SyntaxValidatorAttributeName = typeof(SyntaxValidatorAttribute).FullName;
    private static readonly string IncrementalValueProviderFactoryAttributeName = typeof(IncrementalValueProviderFactoryAttribute).FullName;

    private readonly IncrementalGeneratorInitializationContext _context;

    public IncrementalValueProviderFactory(IncrementalGeneratorInitializationContext context)
    {
        _context = context;
    }

    public static IncrementalValueProviderFactory From(IncrementalGeneratorInitializationContext context) => new(context);

    public IncrementalValueProvider<(Compilation Compilation, ImmutableArray<ValueProviderData> Data)> GetClassDeclarations()
    {
        var data = _context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: IsSyntaxTargetForGeneration,
                transform: GetSemanticTargetForGeneration)
            .Where(static c => c is not null)!;

        return _context.CompilationProvider.Combine(data.Collect())!;
    }

    private static bool IsSyntaxTargetForGeneration(SyntaxNode node, CancellationToken cancellationToken)
    {
        return (node is ClassDeclarationSyntax c && c.AttributeLists.Count > 0) ||
               (node is StructDeclarationSyntax s && s.AttributeLists.Count > 0);
    }

    private static ValueProviderData GetSemanticTargetForGeneration(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        try
        {
            var typeDeclarationSyntax = (TypeDeclarationSyntax)context.Node;
            var attributeType = GetAttributeType(typeDeclarationSyntax, context.SemanticModel);
            if (attributeType == AttributeType.None)
                return null;
            return new ValueProviderData(attributeType, typeDeclarationSyntax);
        }
        catch
        {
            return null;
        }
    }

    private static AttributeType GetAttributeType(TypeDeclarationSyntax typeDeclarationSyntax, SemanticModel semanticModel)
    {
        foreach (var attributeListSyntax in typeDeclarationSyntax.AttributeLists)
        {
            foreach (var attributeSyntax in attributeListSyntax.Attributes)
            {
                if (semanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
                    continue;

                INamedTypeSymbol attributeContainingTypeSymbol = attributeSymbol.ContainingType;
                string fullName = attributeContainingTypeSymbol.ToDisplayString();

                if (fullName == FileGeneratorAttributeName)
                    return AttributeType.FileGenerator;
                if (fullName == MemberGeneratorAttributeName)
                    return AttributeType.MemberGenerator;
                if (fullName == SyntaxValidatorAttributeName)
                    return AttributeType.SyntaxValidator;
                if (fullName == IncrementalValueProviderFactoryAttributeName)
                    return AttributeType.IncrementalValueProviderFactory;
            }
        }

        return AttributeType.None;
    }
}
