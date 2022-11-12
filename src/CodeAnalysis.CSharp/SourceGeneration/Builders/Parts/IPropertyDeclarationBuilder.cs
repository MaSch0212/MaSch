using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IPropertyDeclarationBuilder : ISourceBuilder
    {
        IPropertyDeclarationBuilder Append(IPropertyConfiguration propertyConfiguration);
        IPropertyDeclarationBuilder Append(IPropertyConfiguration propertyConfiguration, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc);
        IPropertyDeclarationBuilder Append(IReadOnlyPropertyConfiguration propertyConfiguration);
        IPropertyDeclarationBuilder Append(IReadOnlyPropertyConfiguration propertyConfiguration, Action<ISourceBuilder> getBuilderFunc);
        IPropertyDeclarationBuilder Append(IWriteOnlyPropertyConfiguration propertyConfiguration, Action<ISourceBuilder> setBuilderFunc);
    }

    public interface IPropertyDeclarationBuilder<T> : IPropertyDeclarationBuilder, ISourceBuilder<T>
        where T : IPropertyDeclarationBuilder<T>
    {
        new T Append(IPropertyConfiguration propertyConfiguration);
        new T Append(IPropertyConfiguration propertyConfiguration, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc);
        new T Append(IReadOnlyPropertyConfiguration propertyConfiguration);
        new T Append(IReadOnlyPropertyConfiguration propertyConfiguration, Action<ISourceBuilder> getBuilderFunc);
        new T Append(IWriteOnlyPropertyConfiguration propertyConfiguration, Action<ISourceBuilder> setBuilderFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : IPropertyDeclarationBuilder
    {
        /// <inheritdoc/>
        IPropertyDeclarationBuilder IPropertyDeclarationBuilder.Append(IPropertyConfiguration propertyConfiguration)
            => Append(propertyConfiguration, null, null);

        /// <inheritdoc/>
        IPropertyDeclarationBuilder IPropertyDeclarationBuilder.Append(IPropertyConfiguration propertyConfiguration, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc)
            => Append(propertyConfiguration, getBuilderFunc, setBuilderFunc);

        /// <inheritdoc/>
        IPropertyDeclarationBuilder IPropertyDeclarationBuilder.Append(IReadOnlyPropertyConfiguration propertyConfiguration)
            => Append(propertyConfiguration, null, null);

        /// <inheritdoc/>
        IPropertyDeclarationBuilder IPropertyDeclarationBuilder.Append(IReadOnlyPropertyConfiguration propertyConfiguration, Action<ISourceBuilder> getBuilderFunc)
            => Append(propertyConfiguration, getBuilderFunc, null);

        /// <inheritdoc/>
        IPropertyDeclarationBuilder IPropertyDeclarationBuilder.Append(IWriteOnlyPropertyConfiguration propertyConfiguration, Action<ISourceBuilder> setBuilderFunc)
            => Append(propertyConfiguration, null, setBuilderFunc);
    }
}