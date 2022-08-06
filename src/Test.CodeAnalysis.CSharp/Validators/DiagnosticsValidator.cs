using MaSch.Test.Assertion;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace MaSch.Test.CodeAnalysis.CSharp.Validators;

/// <summary>
/// Represents a validator that can be used to validate <see cref="Diagnostic"/>s produced by compilation, source generators and/or analyzers.
/// </summary>
public class DiagnosticsValidator : DiagnosticsValidator<DiagnosticsValidator>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DiagnosticsValidator"/> class.
    /// </summary>
    /// <param name="diagnostics">The diagnostics to validate.</param>
    public DiagnosticsValidator(ImmutableArray<Diagnostic> diagnostics)
        : base(diagnostics)
    {
    }
}

/// <summary>
/// Represents a validator that can be used to validate <see cref="Diagnostic"/>s produced by compilation, source generators and/or analyzers.
/// </summary>
/// <typeparam name="T">The type of the derived validator.</typeparam>
[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Generic representation of DiagnosticsValidator.")]
public abstract class DiagnosticsValidator<T>
    where T : DiagnosticsValidator<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DiagnosticsValidator{T}"/> class.
    /// </summary>
    /// <param name="diagnostics">The diagnostics to validate.</param>
    protected DiagnosticsValidator(ImmutableArray<Diagnostic> diagnostics)
    {
        Diagnostics = diagnostics;
    }

    /// <summary>
    /// Gets the diagnostis to validate.
    /// </summary>
    public ImmutableArray<Diagnostic> Diagnostics { get; }

    /// <summary>
    /// Validates that the <see cref="Diagnostics"/> do not contain any errors.
    /// </summary>
    /// <returns>A reference to this validator.</returns>
    public T HasNoErrors()
    {
        var errors = Diagnostics.Where(x => x.Severity == DiagnosticSeverity.Error).ToArray();
        Assert.Instance.AreEqual(0, errors.Length, $"There were errors during compilation:\n{string.Join("\n", errors.Select(x => x.ToString()))}");
        return (T)this;
    }

    /// <summary>
    /// Validates that the <see cref="Diagnostics"/> do not contain any warnings.
    /// </summary>
    /// <returns>A reference to this validator.</returns>
    public T HasNoWarnings()
    {
        var errors = Diagnostics.Where(x => x.Severity == DiagnosticSeverity.Warning).ToArray();
        Assert.Instance.AreEqual(0, errors.Length, $"There were warnings during compilation:\n{string.Join("\n", errors.Select(x => x.ToString()))}");
        return (T)this;
    }

    /// <summary>
    /// Validates that the <see cref="Diagnostics"/> do not contain any warnings or errors.
    /// </summary>
    /// <returns>A reference to this validator.</returns>
    public T HasNoWarningsOrErrors()
    {
        var errors = Diagnostics.Where(x => x.Severity == DiagnosticSeverity.Warning || x.Severity == DiagnosticSeverity.Error).ToArray();
        Assert.Instance.AreEqual(0, errors.Length, $"There were warnings and/or errors during compilation:\n{string.Join("\n", errors.Select(x => x.ToString()))}");
        return (T)this;
    }

    /// <summary>
    /// Validates that the <see cref="Diagnostics"/> contain a specific error diagnostic at least once.
    /// </summary>
    /// <param name="id">The id of the expected diagnostic.</param>
    /// <returns>A reference to this validator.</returns>
    public T HasError(string id)
        => HasError(id, -1);

    /// <summary>
    /// Validates that the <see cref="Diagnostics"/> contain a specific error diagnostic.
    /// </summary>
    /// <param name="id">The id of the expected diagnostic.</param>
    /// <param name="count">The expected amount of error diagnostics with <paramref name="id"/>.</param>
    /// <returns>A reference to this validator.</returns>
    public T HasError(string id, int count)
        => HasDiagnostic(DiagnosticSeverity.Error, id, count);

    /// <summary>
    /// Validates that the <see cref="Diagnostics"/> contain a specific warning diagnostic at least once.
    /// </summary>
    /// <param name="id">The id of the expected diagnostic.</param>
    /// <returns>A reference to this validator.</returns>
    public T HasWarning(string id)
        => HasWarning(id, -1);

    /// <summary>
    /// Validates that the <see cref="Diagnostics"/> contain a specific warning diagnostic.
    /// </summary>
    /// <param name="id">The id of the expected diagnostic.</param>
    /// <param name="count">The expected amount of warning diagnostics with <paramref name="id"/>.</param>
    /// <returns>A reference to this validator.</returns>
    public T HasWarning(string id, int count)
        => HasDiagnostic(DiagnosticSeverity.Warning, id, count);

    /// <summary>
    /// Validates that the <see cref="Diagnostics"/> contain a specific diagnostic.
    /// </summary>
    /// <param name="severity">The severity of the expected diagnostic.</param>
    /// <param name="id">The id of the expected diagnostic.</param>
    /// <param name="count">The expected amount of diagnostics with <paramref name="severity"/> and <paramref name="id"/>.</param>
    /// <returns>A reference to this validator.</returns>
    public T HasDiagnostic(DiagnosticSeverity severity, string id, int count)
    {
        var errors = Diagnostics.Where(x => x.Severity == severity && x.Id == id).ToArray();
        if (count >= 0)
            Assert.Instance.AreEqual(count, errors.Length, $"Wrong number of {id} {severity}s.");
        else
            Assert.Instance.IsGreaterThan(0, errors.Length, $"At least one {severity} with id {id} was expected, but got none.");

        return (T)this;
    }
}
