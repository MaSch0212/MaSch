namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface ISupportsLineCommentsConfiguration : ICodeConfiguration
{
    bool HasComments { get; }

    ISupportsLineCommentsConfiguration WithLineComment(string comment);
    ISupportsLineCommentsConfiguration WithBlockComment(string comment);
    ISupportsLineCommentsConfiguration WithDocComment(string comment);
}

public interface ISupportsLineCommentsConfiguration<T> : ISupportsLineCommentsConfiguration
    where T : ISupportsLineCommentsConfiguration<T>
{
    new T WithLineComment(string comment);
    new T WithBlockComment(string comment);
    new T WithDocComment(string comment);
}