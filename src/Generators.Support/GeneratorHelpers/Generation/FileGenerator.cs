using MaSch.Generators.GeneratorHelpers.Models;
using MaSch.Generators.Support;
using Microsoft.CodeAnalysis;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MaSch.Generators.GeneratorHelpers.Generation;

internal readonly struct FileGenerator
{
    private readonly GeneratorHelpersContext _context;

    private FileGenerator(GeneratorHelpersContext context)
    {
        _context = context;
    }

    public static FileGenerator Generate(GeneratorHelpersContext context) => new(context);

    public void FileGenerator_(FileGeneratorAttributeData data)
    {
        var builder = new SourceBuilder();
        using (AddNamespaceAndClass(builder, data.TypeSymbol))
        {
            builder.AppendLine($"private readonly {data.ContextTypeSymbol.ToUsageString()} _context;")
                   .AppendLine();
            using (builder.AddBlock($"private {data.TypeSymbol.Name}({data.ContextTypeSymbol.ToUsageString()} context)"))
                builder.AppendLine("_context = context;");
            builder.AppendLine()
                   .AppendLine($"public static {data.TypeSymbol.Name} Generate({data.ContextTypeSymbol.ToUsageString()} context) => new {data.TypeSymbol.Name}(context);");
        }

        _context.AddSource(builder.ToSourceText(), data.TypeSymbol);
    }

    public void MemberGenerator(MemberGeneratorAttributeData data)
    {
        var builder = new SourceBuilder();
        using (AddNamespaceAndClass(builder, data.TypeSymbol))
        {
            builder.AppendLine($"private readonly {data.ContextTypeSymbol.ToUsageString()} _context;")
                   .AppendLine($"private readonly global::MaSch.Generators.Support.SourceBuilder _builder;")
                   .AppendLine();
            using (builder.AddBlock($"private {data.TypeSymbol.Name}({data.ContextTypeSymbol.ToUsageString()} context, global::MaSch.Generators.Support.SourceBuilder builder)"))
            {
                builder.AppendLine("_context = context;")
                       .AppendLine("_builder = builder;");
            }

            builder.AppendLine()
                   .AppendLine($"public static {data.TypeSymbol.Name} Generate({data.ContextTypeSymbol.ToUsageString()} context, global::MaSch.Generators.Support.SourceBuilder builder) => new {data.TypeSymbol.Name}(context, builder);");
        }

        _context.AddSource(builder.ToSourceText(), data.TypeSymbol);
    }

    public void SyntaxValidator(SyntaxValidatorAttributeData data)
    {
        var builder = new SourceBuilder();
        using (AddNamespaceAndClass(builder, data.TypeSymbol))
        {
            builder.AppendLine($"private readonly {data.SyntaxTypeSymbol.ToUsageString()} _syntaxNode;");
            if (data.NeedsSemanticModel)
                builder.AppendLine($"private readonly global::Microsoft.CodeAnalysis.SemanticModel _semanticModel;");
            builder.AppendLine();

            using (builder.AddBlock($"private {data.TypeSymbol.Name}({data.SyntaxTypeSymbol.ToUsageString()} context{(data.NeedsSemanticModel ? ", global::Microsoft.CodeAnalysis.SemanticModel semanticModel" : string.Empty)})"))
            {
                builder.AppendLine("_context = context;");
                if (data.NeedsSemanticModel)
                    builder.AppendLine("_semanticModel = semanticModel;");
            }

            builder.AppendLine()
                   .AppendLine($"public static {data.TypeSymbol.Name} Validate({data.SyntaxTypeSymbol.ToUsageString()} context{(data.NeedsSemanticModel ? ", global::Microsoft.CodeAnalysis.SemanticModel semanticModel" : string.Empty)}) => new {data.TypeSymbol.Name}(context{(data.NeedsSemanticModel ? ", semanticModel" : string.Empty)});");
        }

        _context.AddSource(builder.ToSourceText(), data.TypeSymbol);
    }

    public void IncrementalValueProviderFactory(IncrementalValueProviderFactoryAttributeData data)
    {
        var builder = new SourceBuilder();
        using (AddNamespaceAndClass(builder, data.TypeSymbol))
        {
            builder.AppendLine($"private readonly global::Microsoft.CodeAnalysis.IncrementalGeneratorInitializationContext _context;")
                   .AppendLine();
            using (builder.AddBlock($"private {data.TypeSymbol.Name}(global::Microsoft.CodeAnalysis.IncrementalGeneratorInitializationContext context)"))
                builder.AppendLine("_context = context;");
            builder.AppendLine()
                   .AppendLine($"public static {data.TypeSymbol.Name} From(global::Microsoft.CodeAnalysis.IncrementalGeneratorInitializationContext context) => new {data.TypeSymbol.Name}(context);");
        }

        _context.AddSource(builder.ToSourceText(), data.TypeSymbol);
    }

    private NamespaceAndClassScope AddNamespaceAndClass(SourceBuilder builder, INamedTypeSymbol typeSymbol)
    {
        var namespaceBlock = builder.AddBlock($"namespace {typeSymbol.ContainingNamespace}");
        var keywords = string.Empty;
        if (typeSymbol.IsReadOnly)
            keywords += " readonly";
        keywords += " partial";
        keywords += typeSymbol.TypeKind == TypeKind.Struct ? " struct" : " class";
        var classBlock = builder.AddBlock($"{keywords.Trim()} {typeSymbol.ToTypeDefinitionString()}");
        return new NamespaceAndClassScope(namespaceBlock, classBlock);
    }

    private readonly struct NamespaceAndClassScope : IDisposable
    {
        private readonly SourceBuilder.CodeBlock _namespaceBlock;
        private readonly SourceBuilder.CodeBlock _classBlock;

        public NamespaceAndClassScope(SourceBuilder.CodeBlock namespaceBlock, SourceBuilder.CodeBlock classBlock)
        {
            _namespaceBlock = namespaceBlock;
            _classBlock = classBlock;
        }

        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP007:Don't dispose injected", Justification = "This is the whole reson this struct exists.")]
        public void Dispose()
        {
            _classBlock.Dispose();
            _namespaceBlock.Dispose();
        }
    }
}
