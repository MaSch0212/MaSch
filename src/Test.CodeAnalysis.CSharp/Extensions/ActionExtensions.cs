using MaSch.Test.CodeAnalysis.CSharp.Validators;
using Microsoft.CodeAnalysis;

namespace MaSch.Test.CodeAnalysis.CSharp.Extensions;

internal static class ActionExtensions
{
    public static void TryInvoke<T>(this Action<ISyntaxNodeValidator<T>>? action, CompilationValidator parentValidator, T value)
        where T : SyntaxNode
    {
        if (action is not null)
        {
            var validator = new SyntaxNodeValidator<T>(value, parentValidator);
            action(validator);
        }
    }

    public static void TryInvoke<T>(this Action<ISyntaxNodeValidator<T>>? action, IValidator parentValidator, T value)
        where T : SyntaxNode
    {
        if (action is not null)
        {
            var validator = new SyntaxNodeValidator<T>(value, parentValidator);
            action(validator);
        }
    }

    public static void TryInvoke<T>(this Action<ISymbolValidator<T>>? action, CompilationValidator parentValidator, T value)
        where T : ISymbol
    {
        if (action is not null)
        {
            var validator = new SymbolValidator<T>(value, parentValidator);
            action(validator);
        }
    }

    public static void TryInvoke<T>(this Action<ISymbolValidator<T>>? action, IValidator parentValidator, T value)
        where T : ISymbol
    {
        if (action is not null)
        {
            var validator = new SymbolValidator<T>(value, parentValidator);
            action(validator);
        }
    }
}
