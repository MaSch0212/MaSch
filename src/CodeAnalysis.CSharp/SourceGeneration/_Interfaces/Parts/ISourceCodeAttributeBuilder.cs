namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface ISourceCodeAttributeBuilder
{
    ISourceCodeAttributeBuilder WithParameter(string value);
    ISourceCodeAttributeBuilder OnTarget(CodeAttributeTarget target);
    void WriteTo(ISourceBuilder sourceBuilder);
}
