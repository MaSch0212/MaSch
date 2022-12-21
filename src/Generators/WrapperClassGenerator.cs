using MaSch.CodeAnalysis.CSharp.Extensions;
using MaSch.Core;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace MaSch.Generators;

/// <summary>
/// A C# 9 Source Generator that generates members to wrap a different class or interface.
/// </summary>
/// <seealso cref="ISourceGenerator" />
[Generator]
public class WrapperClassGenerator : ISourceGenerator
{
    /// <inheritdoc />
    public void Initialize(GeneratorInitializationContext context)
    {
        // No initialization required
    }

    /// <inheritdoc />
    public void Execute(GeneratorExecutionContext context)
    {
        var wrappingAttributeSymbol = context.Compilation.GetTypeByMetadataName(typeof(WrappingAttribute).FullName);

        if (wrappingAttributeSymbol == null)
            return;

#pragma warning disable RS1024 // Symbols should be compared for equality
        var query = from typeSymbol in context.Compilation.SourceModule.GlobalNamespace.EnumerateNamespaceTypes()
                    let attributes = typeSymbol.GetAttributes()
                    from attribute in attributes
                    where SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, wrappingAttributeSymbol)
                    group attribute by typeSymbol into g
                    select g;
#pragma warning restore RS1024 // Symbols should be compared for equality

        foreach (var type in query)
        {
            var builder = SourceBuilder.Create();

            builder.Append(Block($"namespace {type.Key.ContainingNamespace}"), builder =>
            {
                builder.Append(Block($"partial class {type.Key.Name}"), builder =>
                {
                    var existingMembers = type.Key.GetMembers();
                    var wrappedTypes = new List<(INamedTypeSymbol Type, string PropName)>();
                    bool isFirstRegion = true;
                    foreach (var attribute in type)
                    {
                        if (attribute.ConstructorArguments[0].Value is not INamedTypeSymbol wrappedType)
                            continue;

                        var propName = attribute.NamedArguments.FirstOrDefault(x => x.Key == "WrappingPropName").Value.Value as string ?? $"Wrapped{wrappedType.Name}";
                        wrappedTypes.Add((wrappedType, propName));
                        if (!isFirstRegion)
                            _ = builder.AppendLine();
                        isFirstRegion = false;

                        GenerateWrapperClassRegion(builder, existingMembers, wrappedType, propName);
                    }

                    if (!existingMembers.OfType<IMethodSymbol>().Any(x => x.MethodKind == MethodKind.Constructor && x.Parameters.Length > 0))
                    {
                        _ = builder.AppendLine();
                        builder.Append(Block($"public {type.Key.Name}({string.Join(", ", wrappedTypes.Select(x => $"{x} wrapped{x.Type.Name}"))})"), builder =>
                        {
                            foreach (var t in wrappedTypes)
                                _ = builder.AppendLine($"{t.PropName} = wrapped{t.Type.Name};");
                        });
                    }
                });
            });

            context.AddSource(builder.ToSourceText(), type.Key);
        }
    }

    private static void GenerateWrapperClassRegion(ISourceBuilder builder, IList<ISymbol> existingMembers, ITypeSymbol wrappedType, string wrapperPropName)
    {
        var name = wrapperPropName;
        builder.Append(Region($"{wrappedType.Name} members"), builder =>
        {
            _ = builder.AppendLine()
                   .AppendLine($"protected {wrappedType} {name} {{ get; set; }}");

            var members = wrappedType.GetMembers().Where(x => x.DeclaredAccessibility == Microsoft.CodeAnalysis.Accessibility.Public).ToArray();

            foreach (var p in members.OfType<IPropertySymbol>())
            {
                if (existingMembers.OfType<IPropertySymbol>().Any(x => x.Name == p.Name))
                    continue;
                _ = builder.AppendLine();
                builder.Append(Block($"public virtual {p.ToDefinitionString()}"), builder =>
                {
                    var usage = p.IsIndexer ? $"{name}{p.ToUsageString().Substring(4)}" : $"{name}.{p.ToUsageString()}";
                    if (p.GetMethod != null)
                        _ = builder.AppendLine($"get => {usage};");
                    if (p.SetMethod != null)
                        _ = builder.AppendLine($"set => {usage} = value;");
                });
            }

            foreach (var e in members.OfType<IEventSymbol>())
            {
                if (existingMembers.OfType<IEventSymbol>().Any(x => x.Name == e.Name))
                    continue;
                _ = builder.AppendLine();
                builder.Append(Block($"public virtual {e.ToDefinitionString()}"), builder =>
                {
                    if (e.AddMethod != null)
                        _ = builder.AppendLine($"add => {name}.{e.Name} += value;");
                    if (e.RemoveMethod != null)
                        _ = builder.AppendLine($"remove => {name}.{e.Name} -= value;");
                });
            }

            _ = builder.AppendLine();
            bool hadMethod = false;
            foreach (var m in members.OfType<IMethodSymbol>().Where(x => x.MethodKind == MethodKind.Ordinary))
            {
                hadMethod = true;
                if (existingMembers.OfType<IMethodSymbol>().Any(x => x.Name == m.Name && x.TypeParameters.Length == m.TypeParameters.Length && x.Parameters.Select(y => y.Type).SequenceEqual(m.Parameters.Select(y => y.Type), SymbolEqualityComparer.Default)))
                    continue;
                _ = builder.AppendLine($"public virtual {m.ToDefinitionString()} => {name}.{m.ToUsageString()};");
            }

            if (hadMethod)
                _ = builder.AppendLine();
        });
    }
}
