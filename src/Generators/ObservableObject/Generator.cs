using MaSch.Generators.ObservableObject.Generation;
using MaSch.Generators.ObservableObject.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Threading;

namespace MaSch.Generators.ObservableObject;

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
        context.RegisterPostInitializationOutput(AddPostInitializationSources);

        var targetTypeInfoProvider = context.SyntaxProvider
            .CreateSyntaxProvider(FilterSyntax, TransformSyntax)
            .Where(static x => x != null && x.InterfaceType != InterfaceType.None);

        context.RegisterSourceOutput(targetTypeInfoProvider, Execute);
    }

    private static void AddPostInitializationSources(IncrementalGeneratorPostInitializationContext context)
    {
        new StaticSourceCreator(context.AddSource)
            .AddSource(StaticSources.GenerateNotifyPropertyChangedAttribute)
            .AddSource(StaticSources.GenerateObservableObjectAttribute);
    }

    private static bool FilterSyntax(SyntaxNode node, CancellationToken cancellation)
    {
        return node is ClassDeclarationSyntax classDeclarationSyntax
            && classDeclarationSyntax.AttributeLists.SelectMany(x => x.Attributes).Any();
    }

    private static TargetTypeInfo TransformSyntax(GeneratorSyntaxContext context, CancellationToken cancellation)
    {
        if (context.Node is not ClassDeclarationSyntax classDeclarationSyntax ||
            context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax, cancellation) is not ITypeSymbol typeSymbol)
        {
            return null;
        }

        return TargetTypeInfo.Get(typeSymbol);
    }

    private static void Execute(SourceProductionContext context, TargetTypeInfo targetTypeInfo)
    {
        foreach (var diag in targetTypeInfo.Diagnostics)
            context.ReportDiagnostic(diag);

        var builder = SourceBuilder.Create(new SourceBuilderOptions());

        builder.Append(Namespace(targetTypeInfo.ContainingNamespace), builder =>
        {
            if (targetTypeInfo.InterfaceType.HasFlag(InterfaceType.ObservableObject))
                builder.AppendObservableObjectImplementationClass(targetTypeInfo.Name);
            else
                builder.AppendNotifyPropertyChangedImplementationClass(targetTypeInfo.Name);
        });

        context.AddSource($"{targetTypeInfo.ContainingNamespace}.{targetTypeInfo.Name}.g.cs", builder.ToSourceText());
    }
}
