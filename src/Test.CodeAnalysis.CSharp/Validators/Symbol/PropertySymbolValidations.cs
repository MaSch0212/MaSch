using MaSch.Test.Assertion;
using Microsoft.CodeAnalysis;

// TODO
#pragma warning disable SA1600 // Elements should be documented

namespace MaSch.Test.CodeAnalysis.CSharp.Validators;

public static class PropertySymbolValidations
{
    public static ISymbolValidator<IPropertySymbol> IsOfType<T>(this ISymbolValidator<IPropertySymbol> validator)
        => IsOfType(validator, typeof(T));

    public static ISymbolValidator<IPropertySymbol> IsOfType(this ISymbolValidator<IPropertySymbol> validator, Type type)
        => IsOfType(validator, validator.Compilation.GetTypeSymbolFromType(type));

    public static ISymbolValidator<IPropertySymbol> IsOfType(this ISymbolValidator<IPropertySymbol> validator, ITypeSymbol typeSymbol)
    {
        Assert.Instance.AreEqual(typeSymbol, validator.Symbol.Type, $"The type of property \"{validator.Symbol.Name}\" in type \"{validator.TryGetParent<ISymbolValidator<INamedTypeSymbol>>()?.GetTypeName()}\" is not correct.");
        return validator;
    }

    public static ISymbolValidator<IPropertySymbol> HasGetterAndSetter(this ISymbolValidator<IPropertySymbol> validator)
        => HasGetterSetter(validator, true, true);

    public static ISymbolValidator<IPropertySymbol> HasOnlyGetter(this ISymbolValidator<IPropertySymbol> validator)
        => HasGetterSetter(validator, true, false);

    public static ISymbolValidator<IPropertySymbol> HasOnlySetter(this ISymbolValidator<IPropertySymbol> validator)
        => HasGetterSetter(validator, false, true);

    private static ISymbolValidator<IPropertySymbol> HasGetterSetter(ISymbolValidator<IPropertySymbol> validator, bool hasGetter, bool hasSetter)
    {
        if (hasGetter)
            Assert.Instance.IsNotNull(validator.Symbol.GetMethod, $"The property \"{validator.Symbol.Name}\" in type \"{validator.TryGetParent<ISymbolValidator<INamedTypeSymbol>>()?.GetTypeName()}\" does not have a get method.");
        else
            Assert.Instance.IsNull(validator.Symbol.GetMethod, $"The property \"{validator.Symbol.Name}\" in type \"{validator.TryGetParent<ISymbolValidator<INamedTypeSymbol>>()?.GetTypeName()}\" does have a get method.");

        if (hasSetter)
            Assert.Instance.IsNotNull(validator.Symbol.SetMethod, $"The property \"{validator.Symbol.Name}\" in type \"{validator.TryGetParent<ISymbolValidator<INamedTypeSymbol>>()?.GetTypeName()}\" does not have a set method.");
        else
            Assert.Instance.IsNull(validator.Symbol.SetMethod, $"The property \"{validator.Symbol.Name}\" in type \"{validator.TryGetParent<ISymbolValidator<INamedTypeSymbol>>()?.GetTypeName()}\" does have a set method.");

        return validator;
    }
}
