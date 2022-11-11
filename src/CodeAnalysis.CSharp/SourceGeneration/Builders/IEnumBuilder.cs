using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IEnumBuilder : ISourceBuilder<IEnumBuilder>
    {
        IEnumBuilder Append(Func<IEnumMemberFactory, IEnumValueConfiguration> createFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : IEnumBuilder
    {
        /// <inheritdoc/>
        IEnumBuilder IEnumBuilder.Append(Func<IEnumMemberFactory, IEnumValueConfiguration> createFunc)
            => Append(createFunc(_configurationFactory));

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
}