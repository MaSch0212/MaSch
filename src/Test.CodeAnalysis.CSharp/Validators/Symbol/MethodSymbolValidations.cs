using MaSch.Test.Assertion;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
#if MSTEST
using AssertFailedException = Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException;
#endif

namespace MaSch.Test.CodeAnalysis.CSharp.Validators;

/// <summary>
/// Provides extension methods for <see cref="ISymbolValidator{T}"/> of <see cref="IMethodSymbol"/>.
/// </summary>
public static class MethodSymbolValidations
{
    /// <summary>
    /// Validates that the <see cref="IMethodSymbol"/> has a specific modifier.
    /// </summary>
    /// <param name="validator">This validator.</param>
    /// <param name="modifier">The modifier expected to exist on the <see cref="IMethodSymbol"/>.</param>
    /// <returns>A reference to this validator.</returns>
    public static ISymbolValidator<IMethodSymbol> HasModifier(this ISymbolValidator<IMethodSymbol> validator, SyntaxKind modifier)
    {
        bool isPartial = (from syntaxRef in validator.Symbol.DeclaringSyntaxReferences
                          let syntax = syntaxRef.GetSyntax()
                          where syntax is MethodDeclarationSyntax declarationSyntax
                              && declarationSyntax.Modifiers.Any(SyntaxKind.PartialKeyword)
                          select syntax).Any();
        Assert.Instance.IsTrue(isPartial);
        return validator;
    }

    /// <summary>
    /// Validates that the <see cref="IMethodSymbol"/> has a method body of a specific type.
    /// </summary>
    /// <typeparam name="T">The type of expression the body is expected to have.</typeparam>
    /// <param name="validator">This validator.</param>
    /// <returns>A reference to this validator.</returns>
    public static ISymbolValidator<IMethodSymbol> HasBodyExpression<T>(this ISymbolValidator<IMethodSymbol> validator)
        where T : ExpressionSyntax
        => HasBodyExpression<T>(validator, null, null);

    /// <summary>
    /// Validates that the <see cref="IMethodSymbol"/> has a method body of a specific type and condition.
    /// </summary>
    /// <typeparam name="T">The type of expression the body is expected to have.</typeparam>
    /// <param name="validator">This validator.</param>
    /// <param name="validation">Function used to validate the body expression.</param>
    /// <returns>A reference to this validator.</returns>
    public static ISymbolValidator<IMethodSymbol> HasBodyExpression<T>(this ISymbolValidator<IMethodSymbol> validator, Action<ISyntaxNodeValidator<T>>? validation)
        where T : ExpressionSyntax
        => HasBodyExpression(validator, null, validation);

    /// <summary>
    /// Validates that the <see cref="IMethodSymbol"/> has a method body of a specific type and condition.
    /// </summary>
    /// <typeparam name="T">The type of expression the body is expected to have.</typeparam>
    /// <param name="validator">This validator.</param>
    /// <param name="logName">The name that should be displayed in the error message if the validation fails.</param>
    /// <returns>A reference to this validator.</returns>
    public static ISymbolValidator<IMethodSymbol> HasBodyExpression<T>(this ISymbolValidator<IMethodSymbol> validator, string? logName)
        where T : ExpressionSyntax
        => HasBodyExpression<T>(validator, logName, null);

    /// <summary>
    /// Validates that the <see cref="IMethodSymbol"/> has a method body of a specific type and condition.
    /// </summary>
    /// <typeparam name="T">The type of expression the body is expected to have.</typeparam>
    /// <param name="validator">This validator.</param>
    /// <param name="logName">The name that should be displayed in the error message if the validation fails.</param>
    /// <param name="validation">Function used to validate the body expression.</param>
    /// <returns>A reference to this validator.</returns>
    public static ISymbolValidator<IMethodSymbol> HasBodyExpression<T>(this ISymbolValidator<IMethodSymbol> validator, string? logName, Action<ISyntaxNodeValidator<T>>? validation)
        where T : ExpressionSyntax
    {
        foreach (var syntaxRef in validator.Symbol.DeclaringSyntaxReferences)
        {
            if (syntaxRef.GetSyntax() is not MethodDeclarationSyntax declarationSyntax)
                continue;

            if (declarationSyntax.ExpressionBody is not null &&
                declarationSyntax.ExpressionBody.Expression is T expressionBodyExpression &&
                TryValidate(validator, expressionBodyExpression, validation))
            {
                return validator;
            }

            if (declarationSyntax.Body is not null)
            {
                foreach (var statement in declarationSyntax.Body.Statements)
                {
                    if (statement is ExpressionStatementSyntax expressionStatement &&
                        expressionStatement.Expression is T statementExpression &&
                        TryValidate(validator, statementExpression, validation))
                    {
                        return validator;
                    }
                }
            }
        }

        Assert.Instance.Fail("No body expression found with the given validation." + (string.IsNullOrWhiteSpace(logName) ? string.Empty : $" ({logName})"));
        return validator;
    }

    /// <summary>
    /// Validates that the <see cref="IMethodSymbol"/> does not have any parameters.
    /// </summary>
    /// <param name="validator">This validator.</param>
    /// <returns>A reference to this validator.</returns>
    public static ISymbolValidator<IMethodSymbol> HasNoParameters(this ISymbolValidator<IMethodSymbol> validator)
        => HasParameters(validator, Array.Empty<INamedTypeSymbol>());

    /// <summary>
    /// Validates that the <see cref="IMethodSymbol"/> has parameters of specified types.
    /// </summary>
    /// <param name="validator">This validator.</param>
    /// <param name="types">The expected parameter types. Can be on of the following types: <see cref="Type"/>, <see cref="INamedTypeSymbol"/>.</param>
    /// <returns>A reference to this validator.</returns>
    public static ISymbolValidator<IMethodSymbol> HasParameters(this ISymbolValidator<IMethodSymbol> validator, params object[] types)
        => HasParameters(validator, validator.Compilation.GetTypeSymbols(types));

    /// <summary>
    /// Validates that the <see cref="IMethodSymbol"/> has parameters of specified types.
    /// </summary>
    /// <param name="validator">This validator.</param>
    /// <param name="types">The expected parameter types.</param>
    /// <returns>A reference to this validator.</returns>
    public static ISymbolValidator<IMethodSymbol> HasParameters(this ISymbolValidator<IMethodSymbol> validator, params Type[]? types)
        => HasParameters(validator, validator.Compilation.GetTypeSymbols(types));

    /// <summary>
    /// Validates that the <see cref="IMethodSymbol"/> has parameters of specified types.
    /// </summary>
    /// <param name="validator">This validator.</param>
    /// <param name="typeSymbols">The expected parameter types.</param>
    /// <returns>A reference to this validator.</returns>
    public static ISymbolValidator<IMethodSymbol> HasParameters(this ISymbolValidator<IMethodSymbol> validator, params INamedTypeSymbol[]? typeSymbols)
    {
        Assert.Instance.AreCollectionsEqual(
            validator.Symbol.Parameters.Select(x => x.Type),
            typeSymbols ?? Array.Empty<INamedTypeSymbol>(),
            SymbolEqualityComparer.Default);
        return validator;
    }

    private static bool TryValidate<T>(ISymbolValidator<IMethodSymbol> validator, T syntaxNode, Action<ISyntaxNodeValidator<T>>? validation)
        where T : SyntaxNode
    {
        if (validation is null)
            return true;

        try
        {
            var syntaxValidator = new SyntaxNodeValidator<T>(syntaxNode, validator.Compilation);
            validation(syntaxValidator);
            return true;
        }
        catch (AssertFailedException)
        {
            return false;
        }
    }
}
