using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IDelegateDeclarationBuilder : ISourceBuilder
    {
        IDelegateDeclarationBuilder Append(IDelegateConfiguration delegateConfiguration);
    }

    public interface IDelegateDeclarationBuilder<T> : IDelegateDeclarationBuilder, ISourceBuilder<T>
        where T : IDelegateDeclarationBuilder<T>
    {
        new T Append(IDelegateConfiguration delegateConfiguration);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : IDelegateDeclarationBuilder
    {
        /// <inheritdoc/>
        IDelegateDeclarationBuilder IDelegateDeclarationBuilder.Append(IDelegateConfiguration delegateConfiguration)
            => Append(delegateConfiguration);
    }
}