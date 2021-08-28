using MaSch.Generators.Common;
using Microsoft.CodeAnalysis;
using System;
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
            var debugGeneratorSymbol = context.Compilation.GetTypeByMetadataName("MaSch.Core.Attributes.DebugGeneratorAttribute");
            var observableObjectAttributeSymbol = context.Compilation.GetTypeByMetadataName("MaSch.Core.Attributes.GenerateObservableObjectAttribute");
            var notifyPropChangeAttributeSymbol = context.Compilation.GetTypeByMetadataName("MaSch.Core.Attributes.GenerateNotifyPropertyChangedAttribute");

            if (observableObjectAttributeSymbol == null)
                return;

            var query = from typeSymbol in context.Compilation.SourceModule.GlobalNamespace.GetNamespaceTypes()
                        let attributes = typeSymbol.GetAttributes()
                        let shouldDebug = attributes.Any(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, debugGeneratorSymbol))
                        let oo = attributes.FirstOrDefault(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, observableObjectAttributeSymbol))
                        let npc = attributes.FirstOrDefault(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, notifyPropChangeAttributeSymbol))
                        let interfaceType = (oo is not null ? InterfaceType.ObservableObject : (InterfaceType?)null) ??
                                            (npc is not null ? InterfaceType.NotifyPropertyChanged : (InterfaceType?)null) ??
                                            InterfaceType.None
                        where interfaceType != InterfaceType.None
                        select (typeSymbol, interfaceType, attribute: oo ?? npc, shouldDebug);

            foreach (var (typeSymbol, interfaceType, attribute, shouldDebug) in query)
            {
                if (shouldDebug)
                    LaunchDebuggerOnBuild();

                var interfaceName = interfaceType switch
                {
                    InterfaceType.NotifyPropertyChanged => "System.ComponentModel.INotifyPropertyChanged",
                    InterfaceType.ObservableObject => "MaSch.Core.Observable.IObservableObject",
                    _ => throw new Exception("Unknown interface type: " + interfaceType),
                };

                var builder = new SourceBuilder();

                using (builder.AddBlock($"namespace {typeSymbol.ContainingNamespace}"))
                using (builder.AddBlock($"partial class {typeSymbol.Name} : {interfaceName}"))
                {
                    if (interfaceType == InterfaceType.ObservableObject)
                    {
                        builder.AppendLine("private System.Collections.Generic.Dictionary<string, MaSch.Core.Attributes.NotifyPropertyChangedAttribute> __attributes;")
                               .AppendLine("private MaSch.Core.Observable.Modules.ObservableObjectModule __module;")
                               .AppendLine()
                               .AppendLine("private System.Collections.Generic.Dictionary<string, MaSch.Core.Attributes.NotifyPropertyChangedAttribute> _attributes => __attributes ??= MaSch.Core.Attributes.NotifyPropertyChangedAttribute.InitializeAll(this);")
                               .AppendLine("private MaSch.Core.Observable.Modules.ObservableObjectModule _module => __module ??= new MaSch.Core.Observable.Modules.ObservableObjectModule(this);")
                               .AppendLine();
                    }

                    builder.AppendLine("/// <inheritdoc/>")
                           .AppendLine("public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;")
                           .AppendLine();

                    if (interfaceType == InterfaceType.ObservableObject)
                    {
                        builder.AppendLine("/// <inheritdoc/>")
                               .AppendLine("public virtual bool IsNotifyEnabled { get; set; } = true;")
                               .AppendLine()
                               .AppendLine("/// <inheritdoc/>");
                    }
                    else
                    {
                        builder.AppendLine("/// <summary>Sets the specified property and notifies subscribers about the change.</summary>")
                               .AppendLine("/// <typeparam name=\"T\">The type of the property to set.</typeparam>")
                               .AppendLine("/// <param name=\"property\">The property backing field.</param>")
                               .AppendLine("/// <param name=\"value\">The value to set.</param>")
                               .AppendLine("/// <param name=\"propertyName\">Name of the property.</param>");
                    }

                    using (builder.AddBlock("public virtual void SetProperty<T>(ref T property, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)"))
                    {
                        if (interfaceType == InterfaceType.ObservableObject)
                        {
                            using (builder.AddBlock("if (_attributes.ContainsKey(propertyName))"))
                                builder.AppendLine("_attributes[propertyName].UnsubscribeEvent(this);");
                            builder.AppendLine("property = value;")
                                   .AppendLine("NotifyPropertyChanged(propertyName);");
                            using (builder.AddBlock("if (_attributes.ContainsKey(propertyName))"))
                                builder.AppendLine("_attributes[propertyName].SubscribeEvent(this);");
                        }
                        else
                        {
                            builder.AppendLine("property = value;")
                                   .AppendLine("OnPropertyChanged(propertyName);");
                        }
                    }

                    builder.AppendLine();
                    if (interfaceType == InterfaceType.ObservableObject)
                    {
                        builder.AppendLine("/// <inheritdoc/>");
                        using (builder.AddBlock("public virtual void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = \"\", bool notifyDependencies = true)"))
                        {
                            using (builder.AddBlock("if (IsNotifyEnabled)"))
                            {
                                builder.AppendLine("PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));");
                                using (builder.AddBlock("if (notifyDependencies)"))
                                    builder.AppendLine("_module.NotifyDependentProperties(propertyName);");
                            }
                        }

                        builder.AppendLine()
                               .AppendLine("/// <inheritdoc/>");
                        using (builder.AddBlock("public virtual void NotifyCommandChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = \"\")"))
                        {
                            using (builder.AddBlock("if (IsNotifyEnabled)"))
                                builder.AppendLine("_module.NotifyCommandChanged(propertyName);");
                        }
                    }
                    else
                    {
                        using (builder.AddBlock("protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)"))
                            builder.AppendLine("PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));");
                    }
                }

                context.AddSource(typeSymbol, builder, nameof(ObservableObjectGenerator));
            }
        }
    }
}
