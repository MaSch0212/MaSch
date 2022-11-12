using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IEventDeclarationBuilder : ISourceBuilder
    {
        IEventDeclarationBuilder Append(IEventConfiguration eventConfiguration);
        IEventDeclarationBuilder Append(IEventConfiguration eventConfiguration, Action<ISourceBuilder> addBuilderFunc, Action<ISourceBuilder> removeBuilderFunc);
    }

    public interface IEventDeclarationBuilder<T> : IEventDeclarationBuilder, ISourceBuilder<T>
        where T : IEventDeclarationBuilder<T>
    {
        new T Append(IEventConfiguration eventConfiguration);
        new T Append(IEventConfiguration eventConfiguration, Action<ISourceBuilder> addBuilderFunc, Action<ISourceBuilder> removeBuilderFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : IEventDeclarationBuilder
    {
        /// <inheritdoc/>
        IEventDeclarationBuilder IEventDeclarationBuilder.Append(IEventConfiguration eventConfiguration)
            => Append(eventConfiguration, null, null);

        /// <inheritdoc/>
        IEventDeclarationBuilder IEventDeclarationBuilder.Append(IEventConfiguration eventConfiguration, Action<ISourceBuilder> addBuilderFunc, Action<ISourceBuilder> removeBuilderFunc)
            => Append(eventConfiguration, addBuilderFunc, removeBuilderFunc);
    }
}