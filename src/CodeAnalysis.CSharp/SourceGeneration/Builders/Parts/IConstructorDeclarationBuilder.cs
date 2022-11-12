using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IConstructorDeclarationBuilder : ISourceBuilder
    {
        IConstructorDeclarationBuilder Append(IConstructorConfiguration constructorConfiguration, Action<ISourceBuilder> builderFunc);
        IConstructorDeclarationBuilder Append(IStaticConstructorConfiguration staticConstructorConfiguration, Action<ISourceBuilder> builderFunc);
    }

    public interface IConstructorDeclarationBuilder<T> : IConstructorDeclarationBuilder, ISourceBuilder<T>
        where T : IConstructorDeclarationBuilder<T>
    {
        new T Append(IConstructorConfiguration constructorConfiguration, Action<ISourceBuilder> builderFunc);
        new T Append(IStaticConstructorConfiguration staticConstructorConfiguration, Action<ISourceBuilder> builderFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : IConstructorDeclarationBuilder
    {
        /// <inheritdoc/>
        IConstructorDeclarationBuilder IConstructorDeclarationBuilder.Append(IConstructorConfiguration constructorConfiguration, Action<ISourceBuilder> builderFunc)
            => Append(constructorConfiguration, builderFunc);

        /// <inheritdoc/>
        IConstructorDeclarationBuilder IConstructorDeclarationBuilder.Append(IStaticConstructorConfiguration staticConstructorConfiguration, Action<ISourceBuilder> builderFunc)
            => Append(staticConstructorConfiguration, builderFunc);
    }
}