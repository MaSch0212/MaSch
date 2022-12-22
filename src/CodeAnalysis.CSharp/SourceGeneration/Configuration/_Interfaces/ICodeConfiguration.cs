namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

/// <summary>
/// Represents configuration of a code element. This is used to generate code in the <see cref="ISourceBuilder"/>.
/// </summary>
public interface ICodeConfiguration
{
    /// <summary>
    /// Writes the code represented by this <see cref="ICodeConfiguration"/> to the target <see cref="ISourceBuilder"/>.
    /// </summary>
    /// <param name="sourceBuilder">The <see cref="ISourceBuilder"/> to write the code to.</param>
    void WriteTo(ISourceBuilder sourceBuilder);
}