namespace MaSch.CodeAnalysis.CSharp;

/// <summary>
/// Provides an AddSource method that can be used to add source text to a compilation.
/// </summary>
public interface IAddSource
{
    /// <summary>
    /// Adds a source text to the compilation.
    /// </summary>
    /// <param name="hintName">The hint name.</param>
    /// <param name="sourceText">The source text to add.</param>
    void AddSource(string hintName, SourceText sourceText);
}