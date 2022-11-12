using MaSch.CodeAnalysis.CSharp.Extensions;
using MaSch.CodeAnalysis.CSharp.SourceGeneration;
using MaSch.Core;
using Microsoft.CodeAnalysis;
using System;
using System.Linq;

namespace MaSch.Generators;

/// <summary>
/// A C# 9 Source Generator that generates properties for observable objects.
/// </summary>
/// <seealso cref="ISourceGenerator" />
[Generator]
public class ObservableObjectGenerator : ISourceGenerator
{
    private enum InterfaceType
    {
        None,
        ObservableObject,
        NotifyPropertyChanged,
    }

    /// <inheritdoc />
    public void Initialize(GeneratorInitializationContext context)
    {
        // No initialization required
    }

    /// <inheritdoc />
    public void Execute(GeneratorExecutionContext context)
    {
        var query = from typeSymbol in context.Compilation.SourceModule.GlobalNamespace.EnumerateNamespaceTypes()
                    let attributes = typeSymbol.GetAttributes()
                    let oo = attributes.FirstOrDefault(x => x.AttributeClass.ToDisplayString() == typeof(GenerateObservableObjectAttribute).FullName)
                    let npc = attributes.FirstOrDefault(x => x.AttributeClass.ToDisplayString() == typeof(GenerateNotifyPropertyChangedAttribute).FullName)
                    let interfaceType = (oo is not null ? InterfaceType.ObservableObject : (InterfaceType?)null) ??
                                        (npc is not null ? InterfaceType.NotifyPropertyChanged : (InterfaceType?)null) ??
                                        InterfaceType.None
                    where interfaceType != InterfaceType.None
                    select (typeSymbol, interfaceType, attribute: oo ?? npc);

        foreach (var (typeSymbol, interfaceType, attribute) in query)
        {
            var interfaceName = interfaceType switch
            {
                InterfaceType.NotifyPropertyChanged => "System.ComponentModel.INotifyPropertyChanged",
                InterfaceType.ObservableObject => "MaSch.Core.Observable.IObservableObject",
                _ => throw new Exception("Unknown interface type: " + interfaceType),
            };

            var builder = SourceBuilder.Create();

            using (builder.AppendBlock($"namespace {typeSymbol.ContainingNamespace}"))
            using (builder.AppendBlock($"partial class {typeSymbol.Name} : {interfaceName}"))
            {
                if (interfaceType == InterfaceType.ObservableObject)
                {
                    _ = builder.AppendLine("private System.Collections.Generic.Dictionary<string, MaSch.Core.Attributes.NotifyPropertyChangedAttribute> __attributes;")
                           .AppendLine("private MaSch.Core.Observable.Modules.ObservableObjectModule __module;")
                           .AppendLine()
                           .AppendLine("private System.Collections.Generic.Dictionary<string, MaSch.Core.Attributes.NotifyPropertyChangedAttribute> _attributes => __attributes ??= MaSch.Core.Attributes.NotifyPropertyChangedAttribute.InitializeAll(this);")
                           .AppendLine("private MaSch.Core.Observable.Modules.ObservableObjectModule _module => __module ??= new MaSch.Core.Observable.Modules.ObservableObjectModule(this);")
                           .AppendLine();
                }

                _ = builder.AppendLine("/// <inheritdoc/>")
                       .AppendLine("public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;")
                       .AppendLine();

                if (interfaceType == InterfaceType.ObservableObject)
                {
                    _ = builder.AppendLine("/// <inheritdoc/>")
                           .AppendLine("public virtual bool IsNotifyEnabled { get; set; } = true;")
                           .AppendLine()
                           .AppendLine("/// <inheritdoc/>");
                }
                else
                {
                    _ = builder.AppendLine("/// <summary>Sets the specified property and notifies subscribers about the change.</summary>")
                           .AppendLine("/// <typeparam name=\"T\">The type of the property to set.</typeparam>")
                           .AppendLine("/// <param name=\"property\">The property backing field.</param>")
                           .AppendLine("/// <param name=\"value\">The value to set.</param>")
                           .AppendLine("/// <param name=\"propertyName\">Name of the property.</param>");
                }

                using (builder.AppendBlock("public virtual void SetProperty<T>(ref T property, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)"))
                {
                    if (interfaceType == InterfaceType.ObservableObject)
                    {
                        using (builder.AppendBlock("if (_attributes.ContainsKey(propertyName))"))
                            _ = builder.AppendLine("_attributes[propertyName].UnsubscribeEvent(this);");
                        _ = builder.AppendLine("property = value;")
                               .AppendLine("NotifyPropertyChanged(propertyName);");
                        using (builder.AppendBlock("if (_attributes.ContainsKey(propertyName))"))
                            _ = builder.AppendLine("_attributes[propertyName].SubscribeEvent(this);");
                    }
                    else
                    {
                        _ = builder.AppendLine("property = value;")
                               .AppendLine("OnPropertyChanged(propertyName);");
                    }
                }

                _ = builder.AppendLine();
                if (interfaceType == InterfaceType.ObservableObject)
                {
                    _ = builder.AppendLine("/// <inheritdoc/>");
                    using (builder.AppendBlock("public virtual void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = \"\", bool notifyDependencies = true)"))
                    {
                        using (builder.AppendBlock("if (IsNotifyEnabled)"))
                        {
                            _ = builder.AppendLine("PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));");
                            using (builder.AppendBlock("if (notifyDependencies)"))
                                _ = builder.AppendLine("_module.NotifyDependentProperties(propertyName);");
                        }
                    }

                    _ = builder.AppendLine()
                           .AppendLine("/// <inheritdoc/>");
                    using (builder.AppendBlock("public virtual void NotifyCommandChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = \"\")"))
                    {
                        using (builder.AppendBlock("if (IsNotifyEnabled)"))
                            _ = builder.AppendLine("_module.NotifyCommandChanged(propertyName);");
                    }
                }
                else
                {
                    using (builder.AppendBlock("protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)"))
                        _ = builder.AppendLine("PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));");
                }
            }

            context.AddSource(builder.ToSourceText(), typeSymbol);
        }
    }
}
