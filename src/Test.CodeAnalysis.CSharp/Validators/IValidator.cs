namespace MaSch.Test.CodeAnalysis.CSharp.Validators;

/// <summary>
/// Represents a validator.
/// </summary>
public interface IValidator
{
    /// <summary>
    /// Gets the parent validator.
    /// </summary>
    IValidator? Parent { get; }

    /// <summary>
    /// Gets the compilation this validator is validating.
    /// </summary>
    CompilationValidator Compilation { get; }
}

/// <summary>
/// Provides extension methods for types that implement the <see cref="IValidator"/> interface.
/// </summary>
public static class ValidatorExtensions
{
    /// <summary>
    /// Executes a given validation if a given condition is met.
    /// </summary>
    /// <typeparam name="T">The type of this validator.</typeparam>
    /// <param name="validator">This validator.</param>
    /// <param name="condition">The condition.</param>
    /// <param name="validation">The validation to execute if <paramref name="condition"/> is met.</param>
    /// <returns>A reference to this validator.</returns>
    public static T Conditional<T>(this T validator, Func<T, bool> condition, Action<T> validation)
        where T : IValidator
        => Conditional(validator, condition(validator), validation);

    /// <summary>
    /// Executes a given validation if a given condition is met.
    /// </summary>
    /// <typeparam name="T">The type of this validator.</typeparam>
    /// <param name="validator">This validator.</param>
    /// <param name="condition">The condition.</param>
    /// <param name="validation">The validation to execute if <paramref name="condition"/> is met.</param>
    /// <returns>A reference to this validator.</returns>
    public static T Conditional<T>(this T validator, bool condition, Action<T> validation)
        where T : IValidator
    {
        if (condition)
            validation(validator);
        return validator;
    }
}
