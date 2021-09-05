using MaSch.Core.Attributes;
using MaSch.Generators.Common;
using MaSch.Generators.Properties;
using Microsoft.CodeAnalysis;
using System.Linq;
using static MaSch.Generators.Common.CodeGenerationHelpers;

namespace MaSch.Generators
{
    /// <summary>
    /// A C# 9 Source Generator that generates shims.
    /// </summary>
    /// <seealso cref="ISourceGenerator" />
    [Generator]
    public class ShimsGenerator : ISourceGenerator
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
            var shimsAttributeSymbol = context.Compilation.GetTypeByMetadataName("MaSch.Core.Attributes.ShimsAttribute");

            if (shimsAttributeSymbol == null)
                return;

            var assemblyAttributes = context.Compilation.Assembly.GetAttributes();
            if (assemblyAttributes.Any(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, debugGeneratorSymbol)))
                LaunchDebuggerOnBuild();

            var shimsAttribute = assemblyAttributes.FirstOrDefault(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, shimsAttributeSymbol));
            if (shimsAttribute == null || shimsAttribute.ConstructorArguments.Length == 0)
                return;

            if (shimsAttribute.ConstructorArguments[0].Value is int shimsKey)
            {
                var shims = (Shims)shimsKey;
                if (shims.HasFlag(Shims.Records))
                    AddShim(Shims.Records, Resources.Records);
                if (shims.HasFlag(Shims.IndexAndRange))
                    AddShim(Shims.IndexAndRange, Resources.IndexAndRange);
                if (shims.HasFlag(Shims.NullableReferenceTypes))
                    AddShim(Shims.NullableReferenceTypes, Resources.NullableReferenceTypes);
                if (shims.HasFlag(Shims.OSVersioning))
                    AddShim(Shims.OSVersioning, Resources.OSVersioning);
            }

            void AddShim(Shims shim, string code)
            {
                context.AddSource(context.Compilation.Assembly.CreateHintName(nameof(ShimsGenerator), shim.ToString()), code);
            }
        }
    }
}
