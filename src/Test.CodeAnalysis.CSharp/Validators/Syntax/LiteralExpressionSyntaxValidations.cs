using MaSch.Test.Assertion;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

// TODO
#pragma warning disable SA1600 // Elements should be documented

namespace MaSch.Test.CodeAnalysis.CSharp.Validators;

public static class LiteralExpressionSyntaxValidations
{
    public static ISyntaxNodeValidator<LiteralExpressionSyntax> IsEqualTo(this ISyntaxNodeValidator<LiteralExpressionSyntax> validator, string expectedValue)
    {
        Assert.Instance.AreEqual((int)SyntaxKind.StringLiteralToken, validator.SyntaxNode.Token.RawKind);
        Assert.Instance.AreEqual(expectedValue, validator.SyntaxNode.Token.ValueText);
        return validator;
    }
}
