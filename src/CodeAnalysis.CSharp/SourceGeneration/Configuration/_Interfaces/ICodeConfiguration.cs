namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface ICodeConfiguration
{
    void WriteTo(ISourceBuilder sourceBuilder);
}
