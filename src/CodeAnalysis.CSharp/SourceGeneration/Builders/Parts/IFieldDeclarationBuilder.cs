using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IFieldDeclarationBuilder : ISourceBuilder
    {
        IFieldDeclarationBuilder Append(IFieldConfiguration fieldConfiguration);
    }

    public interface IFieldDeclarationBuilder<T> : IFieldDeclarationBuilder, ISourceBuilder<T>
        where T : IFieldDeclarationBuilder<T>
    {
        new T Append(IFieldConfiguration fieldConfiguration);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : IFieldDeclarationBuilder
    {
        /// <inheritdoc/>
        IFieldDeclarationBuilder IFieldDeclarationBuilder.Append(IFieldConfiguration fieldConfiguration)
            => Append(fieldConfiguration);
    }
}