namespace MaSch.Test.CodeAnalysis.CSharp;

/// <summary>
/// Represents options for the verification of a <see cref="CompilationResult"/>.
/// </summary>
public class CompilationVerifyOptions
{
    /// <summary>
    /// Gets or sets the source file names (hint name) of the files expected to be generated.
    /// </summary>
    public string[] ExpectedSourceFiles { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Gets or sets the source file names (hint name) of the files that should not be verified.
    /// </summary>
    public string[] SkipVerifyForFiles { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Gets or sets the name of this test run (useful for parameterized tests).
    /// </summary>
    public string? TestRunName { get; set; }

    internal string GetTextForParameters(string name)
    {
        if (TestRunName is null or "")
            return name;
        return $"{TestRunName}_{name}";
    }
}