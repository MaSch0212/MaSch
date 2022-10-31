namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface ICodeConfiguration
{
    void WriteTo(ISourceBuilder sourceBuilder);
}
