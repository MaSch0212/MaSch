using MaSch.Test.Assertion;
using MaSch.Test.CodeAnalysis.CSharp.Extensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

// TODO
#pragma warning disable SA1600 // Elements should be documented

namespace MaSch.Test.CodeAnalysis.CSharp.Validators;

public static class ArrowExpressionClauseSyntaxValidations
{
    public static ISyntaxNodeValidator<ArrowExpressionClauseSyntax> HasExpression<T>(this ISyntaxNodeValidator<ArrowExpressionClauseSyntax> validator)
        where T : ExpressionSyntax
        => HasExpression<T>(validator, null);

    public static ISyntaxNodeValidator<ArrowExpressionClauseSyntax> HasExpression<T>(this ISyntaxNodeValidator<ArrowExpressionClauseSyntax> validator, Action<ISyntaxNodeValidator<T>>? validation)
        where T : ExpressionSyntax
    {
        var valueExpressionSyntax = Assert.Instance.IsInstanceOfType<T>(validator.SyntaxNode.Expression, "The expression of the ArrowExpressionClauseSyntax has the wrong type.");
        validation.TryInvoke(validator, valueExpressionSyntax);
        return validator;
    }
}
