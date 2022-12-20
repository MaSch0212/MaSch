using MaSch.Core;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace MaSch.Generators.ObservableObject.Models;

internal record TargetTypeInfo(
    string ContainingNamespace,
    string Name,
    InterfaceType InterfaceType,
    ImmutableArray<AttributeData> Attributes)
{
    public static TargetTypeInfo Get(ITypeSymbol typeSymbol)
    {
        var attributes = typeSymbol.GetAttributes();
        var interfaceType = GetInterfaceType(attributes);

        return new TargetTypeInfo(typeSymbol.ContainingNamespace.ToString(), typeSymbol.Name, interfaceType, attributes);
    }

    private static InterfaceType GetInterfaceType(ImmutableArray<AttributeData> attributes)
    {
        var result = InterfaceType.None;

        foreach (var attribute in attributes)
        {
            var attributeTypeFullName = attribute.AttributeClass.ToDisplayString();

            if (attributeTypeFullName == typeof(GenerateObservableObjectAttribute).FullName)
                result |= InterfaceType.ObservableObject;
            if (attributeTypeFullName == typeof(GenerateNotifyPropertyChangedAttribute).FullName)
                result |= InterfaceType.NotifyPropertyChanged;
        }

        return result;
    }
}
