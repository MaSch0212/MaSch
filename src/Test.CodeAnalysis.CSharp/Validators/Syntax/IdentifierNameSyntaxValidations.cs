using MaSch.Test.Assertion;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MaSch.Test.CodeAnalysis.CSharp.Validators;

public static class IdentifierNameSyntaxValidations
{
    public static ISyntaxNodeValidator<IdentifierNameSyntax> HasName(this ISyntaxNodeValidator<IdentifierNameSyntax> validator, string name)
    {
        Assert.Instance.AreEqual(name, validator.SyntaxNode.Identifier.ValueText);
        return validator;
    }
}
