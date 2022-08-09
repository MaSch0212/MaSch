using MaSch.Test.Assertion;
using MaSch.Test.CodeAnalysis.CSharp.Extensions;
using Microsoft.CodeAnalysis;

// TODO
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1601 // Partial elements should be documented

namespace MaSch.Test.CodeAnalysis.CSharp.Validators;

[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "File name matches (except the 'I')")]
public interface ISymbolValidator : IValidator
{
    new ISymbolValidator? Parent { get; }
    ISymbol Symbol { get; }
}

public interface ISymbolValidator<T> : ISymbolValidator
    where T : ISymbol
{
    new T Symbol { get; }

    ISymbolValidator<T> HasSyntax<TSyntax>(Action<ISyntaxNodeValidator<TSyntax>>? validation)
        where TSyntax : SyntaxNode;

    ISymbolValidator<T> Is<TSymbol>(Action<ISymbolValidator<TSymbol>>? validation)
        where TSymbol : T;
}

public class SymbolValidator<T> : ISymbolValidator<T>
    where T : ISymbol
{
    private readonly IValidator? _parent;

    public SymbolValidator(T symbol, CompilationValidator compilation)
    {
        Symbol = symbol;
        Parent = null;
        _parent = null;
        Compilation = compilation;
    }

    public SymbolValidator(T symbol, IValidator parent)
    {
        Symbol = symbol;
        if (parent is ISymbolValidator symbolValidator)
            Parent = symbolValidator;
        _parent = parent;
        Compilation = parent.Compilation;
    }

    public T Symbol { get; }
    public ISymbolValidator? Parent { get; }
    public CompilationValidator Compilation { get; }

    ISymbol ISymbolValidator.Symbol => Symbol;
    IValidator? IValidator.Parent => _parent;

    public ISymbolValidator<T> HasSyntax<TSyntax>(Action<ISyntaxNodeValidator<TSyntax>>? validation)
        where TSyntax : SyntaxNode
    {
        bool hasSyntax = false;
        foreach (var syntaxReference in Symbol.DeclaringSyntaxReferences)
        {
            if (syntaxReference.GetSyntax() is TSyntax syntax)
            {
                validation.TryInvoke(this, syntax);
                hasSyntax = true;
                break;
            }
        }

        Assert.Instance.IsTrue(hasSyntax, $"The symbol '{Symbol}' does not have a declared syntax of type '{typeof(TSyntax).Name}'.");
        return this;
    }

    public ISymbolValidator<T> Is<TSymbol>(Action<ISymbolValidator<TSymbol>>? validation)
        where TSymbol : T
    {
        var castedSymbol = Assert.Instance.IsInstanceOfType<TSymbol>(Symbol);
        validation.TryInvoke(this, castedSymbol);
        return this;
    }
}

[SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1204:Static elements should appear before instance elements", Justification = "Extended class definition should come first")]
[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Extension methods are fine")]
public static partial class SymbolValidatorExtensions
{
    public static T? TryGetParent<T>(this ISymbolValidator validator)
        where T : ISymbolValidator
    {
        if (validator.Parent is T parent)
            return parent;
        return default;
    }

    public static ISymbolValidator<T> HasAccessibility<T>(this ISymbolValidator<T> validator, Access expectedAccessibility)
    where T : ISymbol
    {
        Assert.Instance.AreEqual(expectedAccessibility, validator.Symbol.DeclaredAccessibility, $"Wrong accessiblity for symbol \"{validator.Symbol}\".");
        return validator;
    }

    public static ISymbolValidator<T> IsStatic<T>(this ISymbolValidator<T> validator)
        where T : ISymbol
    {
        Assert.Instance.IsTrue(validator.Symbol.IsStatic, $"The symbol '{validator.Symbol}' is not static.");
        return validator;
    }

    public static ISymbolValidator<T> IsNotStatic<T>(this ISymbolValidator<T> validator)
        where T : ISymbol
    {
        Assert.Instance.IsFalse(validator.Symbol.IsStatic, $"The symbol '{validator.Symbol}' is static.");
        return validator;
    }

    public static ISymbolValidator<T> HasName<T>(this ISymbolValidator<T> validator, string name)
        where T : ISymbol
    {
        Assert.Instance.AreEqual(name, validator.Symbol.Name, $"The symbol '{validator.Symbol}' has the wrong name.");
        return validator;
    }

    public static ISymbolValidator<T> IsContainedIn<T>(this ISymbolValidator<T> validator, Type typeSymbol)
        where T : ISymbol
        => IsContainedIn(validator, validator.Compilation.GetTypeSymbolFromType(typeSymbol));

    public static ISymbolValidator<T> IsContainedIn<T>(this ISymbolValidator<T> validator, INamedTypeSymbol typeSymbol)
        where T : ISymbol
    {
        Assert.Instance.AreEqual(typeSymbol, validator.Symbol.ContainingType);
        return validator;
    }
}
