using MaSch.Generators.Common;
using Microsoft.CodeAnalysis;
using System.Linq;
using static MaSch.Generators.Common.CodeGenerationHelpers;

namespace MaSch.Generators
{
    /// <summary>
    /// A C# 9 Source Generator that generates properties for observable objects.
    /// </summary>
    /// <seealso cref="ISourceGenerator" />
    [Generator]
    public class ObservableObjectGenerator : ISourceGenerator
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
            var observableObjectAttributeSymbol = context.Compilation.GetTypeByMetadataName("MaSch.Core.Attributes.ObservableObjectAttribute");

            if (observableObjectAttributeSymbol == null)
                return;

            var query = from typeSymbol in context.Compilation.SourceModule.GlobalNamespace.GetNamespaceTypes()
                        let attributes = typeSymbol.GetAttributes()
                        let shouldDebug = attributes.Any(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, debugGeneratorSymbol))
                        from attribute in attributes
                        where SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, observableObjectAttributeSymbol)
                        group attribute by (typeSymbol, shouldDebug) into g
                        select g;

            foreach (var type in query)
            {
                if (type.Key.shouldDebug)
                    LaunchDebuggerOnBuild();

                var builder = new SourceBuilder();
                builder.AppendLine("using System.ComponentModel;")
                       .AppendLine("using System.Runtime.CompilerServices;")
                       .AppendLine();

                using (builder.AddBlock($"namespace {type.Key.typeSymbol.ContainingNamespace}"))
                using (builder.AddBlock($"partial class {type.Key.typeSymbol.Name} : INotifyPropertyChanged"))
                {
                    builder.AppendLine("public event PropertyChangedEventHandler PropertyChanged;")
                           .AppendLine();

                    using (builder.AddBlock("public virtual void SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = null)"))
                    {
                        builder.AppendLine("property = value;")
                               .AppendLine("OnPropertyChanged(propertyName);");
                    }

                    builder.AppendLine();
                    using (builder.AddBlock("protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)"))
                        builder.AppendLine("PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));");
                }

                context.AddSource(type.Key.typeSymbol, builder, nameof(ObservableObjectGenerator));
            }
        }
    }
}
