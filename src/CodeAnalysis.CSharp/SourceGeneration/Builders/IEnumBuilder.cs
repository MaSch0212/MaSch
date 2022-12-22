using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

/// <summary>
/// Represents a <see cref="ISourceBuilder"/> used to build the content of an enum.
/// </summary>
public interface IEnumBuilder : ISourceBuilder<IEnumBuilder>
{
    /// <summary>
    /// Appends an enum value to the current enum.
    /// </summary>
    /// <param name="enumValueConfiguration">The configuration of the enum value.</param>
    /// <returns>A reference to this instance after the append operation has completed.</returns>
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
    IEnumBuilder ISourceBuilder<IEnumBuilder>.Append(IRegionConfiguration regionConfiguration, Action<IEnumBuilder> builderFunc)
        => Append(regionConfiguration, builderFunc);

    /// <inheritdoc/>
    IEnumBuilder ISourceBuilder<IEnumBuilder>.Append(ICodeBlockConfiguration codeBlockConfiguration, Action<IEnumBuilder> builderFunc)
        => Append(codeBlockConfiguration, builderFunc);

    /// <inheritdoc/>
    IEnumBuilder ISourceBuilder<IEnumBuilder>.AppendLine()
        => AppendLine();

    /// <inheritdoc/>
    IEnumBuilder ISourceBuilder<IEnumBuilder>.AppendLine(string value)
        => AppendLine(value);

    /// <inheritdoc/>
    IEnumBuilder ISourceBuilder<IEnumBuilder>.EnsureCurrentLineEmpty()
        => EnsureCurrentLineEmpty();

    /// <inheritdoc/>
    IEnumBuilder ISourceBuilder<IEnumBuilder>.EnsurePreviousLineEmpty()
        => EnsurePreviousLineEmpty();

    /// <inheritdoc/>
    IEnumBuilder ISourceBuilder<IEnumBuilder>.Indent(Action<IEnumBuilder> builderFunc)
        => Indent(builderFunc);
}