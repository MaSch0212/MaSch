using MaSch.Test.Assertion;
using MaSch.Test.CodeAnalysis.CSharp.Extensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MaSch.Test.CodeAnalysis.CSharp.Validators;

public static class EqualsValueClauseSyntaxValidations
{
    public static ISyntaxNodeValidator<EqualsValueClauseSyntax> HasValue<T>(this ISyntaxNodeValidator<EqualsValueClauseSyntax> validator)
        where T : ExpressionSyntax
        => HasValue<T>(validator, null);

    public static ISyntaxNodeValidator<EqualsValueClauseSyntax> HasValue<T>(this ISyntaxNodeValidator<EqualsValueClauseSyntax> validator, Action<ISyntaxNodeValidator<T>>? validation)
        where T : ExpressionSyntax
    {
        var valueExpressionSyntax = Assert.Instance.IsInstanceOfType<T>(validator.SyntaxNode.Value, "The value of the EqualsValueClauseSyntax has the wrong type.");
        validation.TryInvoke(validator, valueExpressionSyntax);
        return validator;
    }
}
