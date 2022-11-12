using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface IEnumBuilder : ISourceBuilder<IEnumBuilder>
{
    IEnumBuilder Append(IEnumValueConfiguration enumValueConfiguration);
}

partial class SourceBuilder : IEnumBuilder
{
    /// <inheritdoc/>
    IEnumBuilder IEnumBuilder.Append(IEnumValueConfiguration enumValueConfiguration)
        => Append(enumValueConfiguration);

    /// <inheritdoc/>
    IEnumBuilder ISourceBuilder<IEnumBuilder>.Append(string value)
        => Append(value);

    /// <inheritdoc/>
    IEnumBuilder ISourceBuilder<IEnumBuilder>.Append(char value)
        => Append(value);

    /// <inheritdoc/>
    IEnumBuilder ISourceBuilder<IEnumBuilder>.AppendLine()
        => AppendLine();

    /// <inheritdoc/>
    IEnumBuilder ISourceBuilder<IEnumBuilder>.AppendLine(string value)
        => AppendLine(value);

    /// <inheritdoc/>
    IEnumBuilder ISourceBuilder<IEnumBuilder>.EnsurePreviousLineEmpty()
        => EnsurePreviousLineEmpty();
}