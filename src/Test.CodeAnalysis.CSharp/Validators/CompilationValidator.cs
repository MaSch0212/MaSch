using MaSch.Test.Assertion;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace MaSch.Test.CodeAnalysis.CSharp.Validators;

/// <summary>
/// Represents a validator that can be used to validate a <see cref="Microsoft.CodeAnalysis.Compilation"/>.
/// </summary>
public class CompilationValidator : DiagnosticsValidator<CompilationValidator>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CompilationValidator"/> class.
    /// </summary>
    /// <param name="compilation">The compilation to validate.</param>
    /// <param name="diagnostics">The diagnostics of the compilation, source generators and analyzers.</param>
    public CompilationValidator(Compilation compilation, ImmutableArray<Diagnostic> diagnostics)
        : base(diagnostics)
    {
        Compilation = compilation;
    }

    /// <summary>
    /// Gets the compilation to validate.
    /// </summary>
    public Compilation Compilation { get; }

    /// <summary>
    /// Validates that the <see cref="Compilation"/> contains a specific type.
    /// Internally the <see cref="Compilation.GetTypeByMetadataName(string)"/> method is used to resolve types.
    /// </summary>
    /// <param name="fullTypeName">The full name of the expected type.</param>
    /// <returns>A reference to this validator.</returns>
    public CompilationValidator HasType(string fullTypeName)
        => HasType(fullTypeName, out _);

    /// <summary>
    /// Validates that the <see cref="Compilation"/> contains a specific type.
    /// Internally the <see cref="Compilation.GetTypeByMetadataName(string)"/> method is used to resolve types.
    /// </summary>
    /// <param name="fullTypeName">The full name of the expected type.</param>
    /// <param name="typeSymbol">The found type symbol from the <see cref="Compilation"/>.</param>
    /// <returns>A reference to this validator.</returns>
    public CompilationValidator HasType(string fullTypeName, out INamedTypeSymbol typeSymbol)
    {
        var type = Compilation.GetTypeByMetadataName(fullTypeName);
        Assert.Instance.IsNotNull(type, $"The type \"{fullTypeName}\" does not exists in the compilation.");
        typeSymbol = type;

        return this;
    }

    /// <summary>
    /// Validates that the <see cref="Compilation"/> does not contain a specific type.
    /// Internally the <see cref="Compilation.GetTypeByMetadataName(string)"/> method is used to resolve types.
    /// </summary>
    /// <param name="fullTypeName">The full name of the unexpected type.</param>
    /// <returns>A reference to this validator.</returns>
    public CompilationValidator DoesNotHaveType(string fullTypeName)
    {
        var typeSymbol = Compilation.GetTypeByMetadataName(fullTypeName);
        Assert.Instance.IsNull(typeSymbol, $"The type \"{fullTypeName}\" exists unexpectedly in the compilation.");

        return this;
    }

    /// <summary>
    /// Validates a specific type from the <see cref="Compilation"/>. Retrieve the type symbol by calling <see cref="HasType(string, out INamedTypeSymbol)"/> first.
    /// </summary>
    /// <param name="typeSymbol">The type symbol to validate.</param>
    /// <param name="typeValidation">The function that is used to validate the type symbol.</param>
    /// <returns>A reference to this validator.</returns>
    public CompilationValidator ValidateType(INamedTypeSymbol typeSymbol, Action<ISymbolValidator<INamedTypeSymbol>> typeValidation)
    {
        var typeValidator = new SymbolValidator<INamedTypeSymbol>(typeSymbol, this);
        typeValidation(typeValidator);

        return this;
    }

    /// <summary>
    /// Gets a type symbol representing a specific <see cref="Type"/>.
    /// </summary>
    /// <param name="type">The type to get the type symbol for.</param>
    /// <returns>A type symbol representing the <paramref name="type"/>.</returns>
    public INamedTypeSymbol GetTypeSymbolFromType(Type type)
    {
        if (type.IsGenericType)
        {
            var genericType = type.GetGenericTypeDefinition();
            HasType(genericType.FullName!, out var genericTypeSymbol);
            return genericTypeSymbol.Construct(type.GetGenericArguments().Select(GetTypeSymbolFromType).ToArray());
        }
        else
        {
            HasType(type.FullName!, out var typeSymbol);
            return typeSymbol;
        }
    }

    /// <summary>
    /// Gets type symbols representing specific <see cref="Type"/>s.
    /// </summary>
    /// <param name="types">The types to get the type symbols for. Items in the array can be of one of the following types: <see cref="Type"/>, <see cref="INamedTypeSymbol"/>.</param>
    /// <returns>An array of type symbols representing the <paramref name="types"/>.</returns>
    public INamedTypeSymbol[] GetTypeSymbols(params object[]? types)
    {
        if (types is null)
            return Array.Empty<INamedTypeSymbol>();

        return (from objType in types
                select objType switch
                {
                    INamedTypeSymbol typeSymbol => typeSymbol,
                    Type type => GetTypeSymbolFromType(type),
                    _ => throw new InvalidOperationException($"Type \"{objType.GetType().FullName}\" is not allowed."),
                }).ToArray();
    }

    /// <summary>
    /// Gets type symbols representing specific <see cref="Type"/>s.
    /// </summary>
    /// <param name="types">The types to get the type symbols for.</param>
    /// <returns>An array of type symbols representing the <paramref name="types"/>.</returns>
    public INamedTypeSymbol[] GetTypeSymbols(params Type[]? types)
    {
        if (types is null)
            return Array.Empty<INamedTypeSymbol>();

        return types.Select(x => GetTypeSymbolFromType(x)).ToArray();
    }
}
