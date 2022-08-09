using Microsoft.CodeAnalysis.Text;

#pragma warning disable SA1649 // File name should match first type name

#nullable enable

namespace MaSch.Generators.Support
{
    /// <summary>
    /// Provides a AddSource method.
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
}