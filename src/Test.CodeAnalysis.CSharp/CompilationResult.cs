﻿using MaSch.Test.Assertion;
using MaSch.Test.CodeAnalysis.CSharp.Validators;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using VerifyTests;

namespace MaSch.Test.CodeAnalysis.CSharp;

/// <summary>
/// Represents the result of <see cref="ICompilationBuilder{TBuilder}.Build"/>.
/// </summary>
public class CompilationResult
{
    private static readonly Regex HintNameExtensionRegex = new(@"(?<name>.*?)(\.g)?(\.cs)?$", RegexOptions.Compiled);
    private static readonly Regex NewVerifyErrorMessageRegex = new("New:\\s*Received:", RegexOptions.Compiled);

    internal CompilationResult(
        Compilation initialCompilation,
        Compilation finalCompilation,
        ImmutableArray<Diagnostic> diagnostics,
        ImmutableArray<GeneratedSourceResult> generatedSourceResults)
    {
        InitialCompilation = initialCompilation;
        FinalCompilation = finalCompilation;
        Diagnostics = diagnostics;
        GeneratedSourceResults = generatedSourceResults;
    }

    /// <summary>
    /// Gets the compilation before executing any source generators.
    /// </summary>
    public Compilation InitialCompilation { get; }

    /// <summary>
    /// Gets the compilation after executing all source generators.
    /// </summary>
    public Compilation FinalCompilation { get; }

    /// <summary>
    /// Gets the diagnostics produced by the compilation, source generators and analyzers.
    /// </summary>
    public ImmutableArray<Diagnostic> Diagnostics { get; }

    /// <summary>
    /// Gets the sources generated by all source generators.
    /// </summary>
    public ImmutableArray<GeneratedSourceResult> GeneratedSourceResults { get; }

    /// <summary>
    /// Creates a validator that can be used to validate the <see cref="InitialCompilation"/>.
    /// </summary>
    /// <returns>A validator that can be used to validate the <see cref="InitialCompilation"/>.</returns>
    public CompilationValidator ValidateInitialCompilation() => new(InitialCompilation, InitialCompilation.GetDiagnostics());

    /// <summary>
    /// Creates a validator that can be used to validate the <see cref="FinalCompilation"/>.
    /// </summary>
    /// <returns>A validator that can be used to validate the <see cref="FinalCompilation"/>.</returns>
    public CompilationValidator ValidateFinalCompilation() => new(FinalCompilation, Diagnostics);

    /// <summary>
    /// Creates a validator that can be used to validate the <see cref="Diagnostics"/>.
    /// </summary>
    /// <returns>A validator that can be used to validate the <see cref="Diagnostics"/>.</returns>
    public DiagnosticsValidator ValidateDiagnostics() => new(Diagnostics);

    /// <summary>
    /// Writes the diagnostics and all generated sources to the trace listeners in the <see cref="Trace.Listeners"/> collection.
    /// </summary>
    public void Trace()
    {
        TraceDiagnostics();
        System.Diagnostics.Trace.WriteLine(string.Empty);
        TraceGeneratedSources();
    }

    /// <summary>
    /// Writes the diagnostics to the trace listeners in the <see cref="Trace.Listeners"/> collection.
    /// </summary>
    public void TraceDiagnostics()
    {
        if (Diagnostics.Length == 0)
            System.Diagnostics.Trace.WriteLine("Diagnostics: <None>");
        else
            System.Diagnostics.Trace.WriteLine($"Diagnostics:\n{string.Join("\n", Diagnostics.Select(x => x.ToString()))}");
    }

    /// <summary>
    /// Writes all generated sources to the trace listeners in the <see cref="Trace.Listeners"/> collection.
    /// </summary>
    public void TraceGeneratedSources()
    {
        var genLog = new StringBuilder().AppendLine("Generated Code:");
        foreach (var result in GeneratedSourceResults)
        {
            genLog.AppendLine(result.HintName);
            genLog.AppendLine(result.SourceText.ToString());
        }

        System.Diagnostics.Trace.WriteLine(genLog.ToString());
    }

    /// <summary>
    /// Loads the assembly of the final compilation.
    /// </summary>
    /// <returns>The assembly of the final compilation.</returns>
    /// <exception cref="InvalidOperationException">Emitting assembly from compilation failed.</exception>
    public Assembly GetFinalAssembly()
    {
        using var stream = new MemoryStream();
        var emitResult = FinalCompilation.Emit(stream);

        if (!emitResult.Success)
            throw new InvalidOperationException($"Emitting assembly from compilation failed:\n{string.Join("\n", emitResult.Diagnostics.Select(x => x.ToString()))}");

        stream.Seek(0, SeekOrigin.Begin);
        return Assembly.Load(stream.ToArray());
    }

    /// <summary>
    /// Verify this <see cref="CompilationResult"/> using the Verify NuGet package.
    /// </summary>
    /// <param name="verifyFunc">The function to verify (depends on used test framework).</param>
    /// <param name="options">The options for the verification.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task Verify(Func<object?, VerifySettings, Task> verifyFunc, CompilationVerifyOptions options)
    {
        await Verify(verifyFunc, options, new VerifySettings());
    }

    /// <summary>
    /// Verify this <see cref="CompilationResult"/> using the Verify NuGet package.
    /// </summary>
    /// <param name="verifyFunc">The function to verify (depends on used test framework).</param>
    /// <param name="options">The options for the verification.</param>
    /// <param name="verifySettings">The verification settings.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task Verify(Func<object?, VerifySettings, Task> verifyFunc, CompilationVerifyOptions options, VerifySettings verifySettings)
    {
        var exceptions = new List<Exception>();

        try
        {
            var settings = new VerifySettings(verifySettings);
            settings.UseTextForParameters(options.GetTextForParameters("Diagnostics"));
            await verifyFunc(Diagnostics.Select(x => x.ToString()), settings);
        }
        catch (Exception ex)
        {
            exceptions.Add(ex);
        }

        foreach (var source in GeneratedSourceResults)
        {
            if (!options.ExpectedSourceFiles.Contains(source.HintName) || options.SkipVerifyForFiles.Contains(source.HintName))
                continue;

            try
            {
                var settings = new VerifySettings(verifySettings);
                settings.UseTextForParameters(options.GetTextForParameters(TrimExtension(source.HintName)));
                await verifyFunc(source.SourceText.ToString(), settings);
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
        }

        try
        {
            Assert.Instance.AreCollectionsEquivalent(options.ExpectedSourceFiles, GeneratedSourceResults.Select(x => x.HintName));
        }
        catch (Exception ex)
        {
            exceptions.Add(ex);
        }

        AssertBase assert = Assert.Instance;
        if (exceptions.All(ex => ex.GetType().Name == "VerifyException" && NewVerifyErrorMessageRegex.IsMatch(ex.Message)))
            assert = Assert.Instance.Inc;

        if (exceptions.Count > 0)
        {
            const string separator = "------------";
            assert.Fail($"One or more assertions failed.\n{separator}\n{string.Join($"\n{separator}\n", exceptions.Select(x => x.Message))}");
        }

        static string TrimExtension(string hintName)
        {
            return HintNameExtensionRegex.Match(hintName).Groups["name"].Value;
        }
    }
}