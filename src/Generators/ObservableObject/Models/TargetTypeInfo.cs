using MaSch.Core;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace MaSch.Generators.ObservableObject.Models;

internal record TargetTypeInfo(
    string ContainingNamespace,
    string Name,
    InterfaceType InterfaceType,
    ImmutableArray<Diagnostic> Diagnostics)
{
    public static TargetTypeInfo Get(ITypeSymbol typeSymbol)
    {
        var attributes = typeSymbol.GetAttributes();
        var interfaceType = GetInterfaceType(attributes, out var generateNotifyPropertyChangedAttribute);
        var diagnostics = ImmutableArray.CreateBuilder<Diagnostic>();

        if (interfaceType == (InterfaceType.ObservableObject | InterfaceType.NotifyPropertyChanged))
        {
            var attributeLocation = generateNotifyPropertyChangedAttribute.ApplicationSyntaxReference.GetSyntax().GetLocation();
            diagnostics.Add(DiagnosticsFactory.AttributeIgnored(typeSymbol.Name, nameof(GenerateNotifyPropertyChangedAttribute), nameof(GenerateObservableObjectAttribute), attributeLocation));
        }

        return new TargetTypeInfo(typeSymbol.ContainingNamespace.ToString(), typeSymbol.Name, interfaceType, diagnostics.ToImmutable());
    }

    private static InterfaceType GetInterfaceType(ImmutableArray<AttributeData> attributes, out AttributeData? generateNotifyPropertyChangedAttribute)
    {
        var result = InterfaceType.None;
        generateNotifyPropertyChangedAttribute = null;

        foreach (var attribute in attributes)
        {
            var attributeTypeFullName = attribute.AttributeClass.ToDisplayString();

            if (attributeTypeFullName == typeof(GenerateObservableObjectAttribute).FullName)
                result |= InterfaceType.ObservableObject;
            if (attributeTypeFullName == typeof(GenerateNotifyPropertyChangedAttribute).FullName)
            {
                result |= InterfaceType.NotifyPropertyChanged;
                generateNotifyPropertyChangedAttribute = attribute;
            }
        }

        return result;
    }
}
