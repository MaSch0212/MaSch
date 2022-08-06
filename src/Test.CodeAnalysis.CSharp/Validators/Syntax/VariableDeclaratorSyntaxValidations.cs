using MaSch.Test.Assertion;
using MaSch.Test.CodeAnalysis.CSharp.Extensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MaSch.Test.CodeAnalysis.CSharp.Validators;

public static class VariableDeclaratorSyntaxValidations
{
    public static ISyntaxNodeValidator<VariableDeclaratorSyntax> HasInitializer(this ISyntaxNodeValidator<VariableDeclaratorSyntax> validator)
        => HasInitializer(validator, null);

    public static ISyntaxNodeValidator<VariableDeclaratorSyntax> HasInitializer(this ISyntaxNodeValidator<VariableDeclaratorSyntax> validator, Action<ISyntaxNodeValidator<EqualsValueClauseSyntax>>? validation)
    {
        Assert.Instance.IsNotNull(validator.SyntaxNode.Initializer, "The variable declarator does not have an initializer.");
        validation.TryInvoke(validator, validator.SyntaxNode.Initializer);
        return validator;
    }
}
