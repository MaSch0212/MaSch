using MaSch.Test.Assertion;
using Microsoft.CodeAnalysis;

// TODO
#pragma warning disable SA1600 // Elements should be documented

namespace MaSch.Test.CodeAnalysis.CSharp.Validators;

public static class EventSymbolValidations
{
    public static ISymbolValidator<IEventSymbol> IsOfType<T>(this ISymbolValidator<IEventSymbol> validator)
        => IsOfType(validator, typeof(T));

    public static ISymbolValidator<IEventSymbol> IsOfType(this ISymbolValidator<IEventSymbol> validator, Type type)
        => IsOfType(validator, validator.Compilation.GetTypeSymbolFromType(type));

    public static ISymbolValidator<IEventSymbol> IsOfType(this ISymbolValidator<IEventSymbol> validator, ITypeSymbol typeSymbol)
    {
        Assert.Instance.AreEqual(typeSymbol, validator.Symbol.Type, $"The type of event \"{validator.Symbol.Name}\" in type \"{validator.TryGetParent<ISymbolValidator<INamedTypeSymbol>>()?.GetTypeName()}\" is not correct.");
        return validator;
    }

    public static ISymbolValidator<IEventSymbol> CanAddAndRemoveHandler(this ISymbolValidator<IEventSymbol> validator)
        => CanAddRemoveHandler(validator, true, true);

    public static ISymbolValidator<IEventSymbol> CanOnlyAddHandler(this ISymbolValidator<IEventSymbol> validator)
        => CanAddRemoveHandler(validator, true, false);

    public static ISymbolValidator<IEventSymbol> CanOnlyRemoveHandler(this ISymbolValidator<IEventSymbol> validator)
        => CanAddRemoveHandler(validator, false, true);

    private static ISymbolValidator<IEventSymbol> CanAddRemoveHandler(ISymbolValidator<IEventSymbol> validator, bool canAdd, bool canRemove)
    {
        if (canAdd)
            Assert.Instance.IsNotNull(validator.Symbol.AddMethod, $"The event \"{validator.Symbol.Name}\" in type \"{validator.TryGetParent<ISymbolValidator<INamedTypeSymbol>>()?.GetTypeName()}\" does not have an add method.");
        else
            Assert.Instance.IsNull(validator.Symbol.AddMethod, $"The event \"{validator.Symbol.Name}\" in type \"{validator.TryGetParent<ISymbolValidator<INamedTypeSymbol>>()?.GetTypeName()}\" does have an add method.");

        if (canRemove)
            Assert.Instance.IsNotNull(validator.Symbol.RemoveMethod, $"The event \"{validator.Symbol.Name}\" in type \"{validator.TryGetParent<ISymbolValidator<INamedTypeSymbol>>()?.GetTypeName()}\" does not have an remove method.");
        else
            Assert.Instance.IsNull(validator.Symbol.RemoveMethod, $"The event \"{validator.Symbol.Name}\" in type \"{validator.TryGetParent<ISymbolValidator<INamedTypeSymbol>>()?.GetTypeName()}\" does have an remove method.");

        return validator;
    }
}
