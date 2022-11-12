using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IRecordDeclarationBuilder : ISourceBuilder
    {
        IRecordDeclarationBuilder Append(IRecordConfiguration recordConfiguration);
        IRecordDeclarationBuilder Append(IRecordConfiguration recordConfiguration, Action<IRecordBuilder> builderFunc);
    }

    public interface IRecordDeclarationBuilder<T> : IRecordDeclarationBuilder, ISourceBuilder<T>
        where T : IRecordDeclarationBuilder<T>
    {
        new T Append(IRecordConfiguration recordConfiguration);
        new T Append(IRecordConfiguration recordConfiguration, Action<IRecordBuilder> builderFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : IRecordDeclarationBuilder
    {
        /// <inheritdoc/>
        IRecordDeclarationBuilder IRecordDeclarationBuilder.Append(IRecordConfiguration recordConfiguration)
            => Append(recordConfiguration, null);

        /// <inheritdoc/>
        IRecordDeclarationBuilder IRecordDeclarationBuilder.Append(IRecordConfiguration recordConfiguration, Action<IRecordBuilder> builderFunc)
            => Append(recordConfiguration, builderFunc);
    }
}