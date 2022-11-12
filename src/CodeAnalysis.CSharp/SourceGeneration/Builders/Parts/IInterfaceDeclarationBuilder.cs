using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IInterfaceDeclarationBuilder : ISourceBuilder
    {
        IInterfaceDeclarationBuilder Append(IInterfaceConfguration interfaceConfguration, Action<IInterfaceBuilder> builderFunc);
    }

    public interface IInterfaceDeclarationBuilder<T> : IInterfaceDeclarationBuilder, ISourceBuilder<T>
        where T : IInterfaceDeclarationBuilder<T>
    {
        new T Append(IInterfaceConfguration interfaceConfguration, Action<IInterfaceBuilder> builderFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : IInterfaceDeclarationBuilder
    {
        /// <inheritdoc/>
        IInterfaceDeclarationBuilder IInterfaceDeclarationBuilder.Append(IInterfaceConfguration interfaceConfguration, Action<IInterfaceBuilder> builderFunc)
            => Append(interfaceConfguration, builderFunc);
    }
}