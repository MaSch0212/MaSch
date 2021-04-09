using MaSch.Generators.Common;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using static MaSch.Generators.Common.CodeGenerationHelpers;

namespace MaSch.Generators
{
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
            var debugGeneratorSymbol = context.Compilation.GetTypeByMetadataName("MaSch.Core.Attributes.DebugGeneratorAttribute");
            var wrappingAttributeSymbol = context.Compilation.GetTypeByMetadataName("MaSch.Core.Attributes.WrappingAttribute");

            if (wrappingAttributeSymbol == null)
                return;

            var query = from typeSymbol in context.Compilation.SourceModule.GlobalNamespace.GetNamespaceTypes()
                        let attributes = typeSymbol.GetAttributes()
                        let shouldDebug = attributes.Any(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, debugGeneratorSymbol))
                        from attribute in attributes
                        where SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, wrappingAttributeSymbol)
                        group attribute by (typeSymbol, shouldDebug) into g
                        select g;

            foreach (var type in query)
            {
                if (type.Key.shouldDebug)
                    LaunchDebuggerOnBuild();

                var builder = new SourceBuilder();

                using (builder.AddBlock($"namespace {type.Key.typeSymbol.ContainingNamespace}"))
                using (builder.AddBlock($"partial class {type.Key.typeSymbol.Name}"))
                {
                    var existingMembers = type.Key.typeSymbol.GetMembers();
                    var wrappedTypes = new List<(INamedTypeSymbol Type, string PropName)>();
                    bool isFirstRegion = true;
                    foreach (var attribute in type)
                    {
                        if (attribute.ConstructorArguments[0].Value is not INamedTypeSymbol wrappedType)
                            continue;

                        var propName = attribute.NamedArguments.FirstOrDefault(x => x.Key == "WrappingPropName").Value.Value as string ?? $"Wrapped{wrappedType.Name}";
                        wrappedTypes.Add((wrappedType, propName));
                        if (!isFirstRegion)
                            builder.AppendLine();
                        isFirstRegion = false;

                        GenerateWrapperClassRegion(builder, existingMembers, wrappedType, propName);
                    }

                    if (!existingMembers.OfType<IMethodSymbol>().Any(x => x.MethodKind == MethodKind.Constructor && x.Parameters.Length > 0))
                    {
                        builder.AppendLine();
                        using (builder.AddBlock($"public {type.Key.typeSymbol.Name}({string.Join(", ", wrappedTypes.Select(x => $"{x} wrapped{x.Type.Name}"))})"))
                        {
                            foreach (var t in wrappedTypes)
                                builder.AppendLine($"{t.PropName} = wrapped{t.Type.Name};");
                        }
                    }
                }

                context.AddSource(type.Key.typeSymbol, builder, nameof(WrapperClassGenerator));
            }
        }

        private static void GenerateWrapperClassRegion(SourceBuilder builder, IList<ISymbol> existingMembers, ITypeSymbol wrappedType, string wrapperPropName)
        {
            var wName = wrapperPropName;
            using (builder.AddRegion($"{wrappedType.Name} members"))
            {
                builder.AppendLine()
                       .AppendLine($"protected {wrappedType} {wName} {{ get; set; }}");

                var members = wrappedType.GetMembers().Where(x => x.DeclaredAccessibility == Microsoft.CodeAnalysis.Accessibility.Public).ToArray();

                foreach (var p in members.OfType<IPropertySymbol>())
                {
                    if (existingMembers.OfType<IPropertySymbol>().Any(x => x.Name == p.Name))
                        continue;
                    builder.AppendLine();
                    using (builder.AddBlock($"public virtual {p.ToDisplayString(DefinitionFormat)}"))
                    {
                        var usage = p.IsIndexer ? $"{wName}{p.ToDisplayString(UsageFormat)[4..]}" : $"{wName}.{p.ToDisplayString(UsageFormat)}";
                        if (p.GetMethod != null)
                            builder.AppendLine($"get => {usage};");
                        if (p.SetMethod != null)
                            builder.AppendLine($"set => {usage} = value;");
                    }
                }

                foreach (var e in members.OfType<IEventSymbol>())
                {
                    if (existingMembers.OfType<IEventSymbol>().Any(x => x.Name == e.Name))
                        continue;
                    builder.AppendLine();
                    using (builder.AddBlock($"public virtual {e.ToDisplayString(DefinitionFormat)}"))
                    {
                        if (e.AddMethod != null)
                            builder.AppendLine($"add => {wName}.{e.Name} += value;");
                        if (e.RemoveMethod != null)
                            builder.AppendLine($"remove => {wName}.{e.Name} -= value;");
                    }
                }

                builder.AppendLine();
                bool hadMethod = false;
                foreach (var m in members.OfType<IMethodSymbol>().Where(x => x.MethodKind == MethodKind.Ordinary))
                {
                    hadMethod = true;
                    if (existingMembers.OfType<IMethodSymbol>().Any(x => x.Name == m.Name && x.TypeParameters.Length == m.TypeParameters.Length && x.Parameters.Select(y => y.Type).SequenceEqual(m.Parameters.Select(y => y.Type), SymbolEqualityComparer.Default)))
                        continue;
                    builder.AppendLine($"public virtual {m.ToDisplayString(DefinitionFormat)} => {wName}.{m.ToDisplayString(UsageFormat)};");
                }

                if (hadMethod)
                    builder.AppendLine();
            }
        }
    }
}
