using MaSch.Generators.Generators.ObservableObject.Common;
using Microsoft.CodeAnalysis;

namespace MaSch.Generators.Generators.ObservableObject.Models;

internal class GeneratorData
{
    public GeneratorData(INamedTypeSymbol typeSymbol, InterfaceType interfaceType)
    {
        TypeSymbol = typeSymbol;
        InterfaceType = interfaceType;
    }

    public INamedTypeSymbol TypeSymbol { get; }
    public InterfaceType InterfaceType { get; }
}
