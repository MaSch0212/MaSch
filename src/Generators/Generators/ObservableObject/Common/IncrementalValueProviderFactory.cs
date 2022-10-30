using MaSch.Generators.Generators.ObservableObject.Models;
using MaSch.Generators.Support;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading;

namespace MaSch.Generators.Generators.ObservableObject.Common;

[IncrementalValueProviderFactory]
internal readonly partial struct IncrementalValueProviderFactory
{
    public IncrementalValuesProvider<GeneratorData> GetClassGenerationInfo()
    {
        return _context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: IsSyntaxTargetForGeneration,
                transform: GetSemanticTargetForGeneration)
            .Where(static c => c is not null)!;
    }

    private static bool IsSyntaxTargetForGeneration(SyntaxNode node, CancellationToken cancellationToken)
    {
        return node is ClassDeclarationSyntax c && c.AttributeLists.Count > 0;
    }

    private static GeneratorData? GetSemanticTargetForGeneration(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        try
        {
            var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;
            var validator = ClassDeclarationValidator.Validate(classDeclarationSyntax, context.SemanticModel);

            var interfaceType = validator.GetGenerateAttributeType();
            if (interfaceType == InterfaceType.None)
                return null;

            var declaredSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax);
            if (declaredSymbol is not INamedTypeSymbol typeSymbol)
                return null;

            return new GeneratorData(typeSymbol, interfaceType);
        }
        catch
        {
            return null;
        }
    }
}
