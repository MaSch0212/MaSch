using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

public interface IRecordDeclarationBuilder<TBuilder>
    where TBuilder : IRecordDeclarationBuilder<TBuilder>
{
    TBuilder Append(IRecordConfiguration recordConfiguration);
    TBuilder Append(IRecordConfiguration recordConfiguration, Action<IRecordBuilder> builderFunc);
}