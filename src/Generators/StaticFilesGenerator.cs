using MaSch.Generators.Common;
using MaSch.Generators.Properties;
using Microsoft.CodeAnalysis;

namespace MaSch.Generators;

/// <summary>
/// Generates attributes required by other source generators.
/// </summary>
[Generator]
public class StaticFilesGenerator : IIncrementalGenerator
{
    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(AddSources);
    }

    private static void AddSources(IncrementalGeneratorPostInitializationContext context)
    {
        new StaticSourceCreator(context.AddSource)
            .AddSource(Resources.AccessModifier)
            .AddSource(Resources.GenerateNotifyPropertyChangedAttribute)
            .AddSource(Resources.GenerateObservableObjectAttribute)
            .AddSource(Resources.ObservablePropertyAccessModifierAttribute)
            .AddSource(Resources.ObservablePropertyDefinitionAttribute)
            .AddSource(Resources.Shims)
            .AddSource(Resources.ShimsAttribute)
            .AddSource(Resources.WrappingAttribute);
    }
}
