namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IEnumValueConfiguration : ISupportsCodeAttributeConfiguration<IEnumValueConfiguration>, ISupportsLineCommentsConfiguration<IEnumValueConfiguration>
{
}

internal sealed class EnumValueConfiguration : CodeConfigurationBase, IEnumValueConfiguration
{
    private readonly List<ICodeAttributeConfiguration> _codeAttributes = new();
    private readonly List<CommentConfiguration> _comments = new();
    private readonly string _name;
    private readonly string? _value;

    public EnumValueConfiguration(string name)
        : this(name, null)
    {
    }

    public EnumValueConfiguration(string name, string? value)
    {
        _name = name;
        _value = value;
    }

    public bool HasComments => _comments.Count > 0;
    public bool HasAttributes => _codeAttributes.Count > 0;

    protected override int StartCapacity => 16;

    public IEnumValueConfiguration WithCodeAttribute(string attributeTypeName)
        => WithCodeAttribute(attributeTypeName, null);

    ISupportsCodeAttributeConfiguration ISupportsCodeAttributeConfiguration.WithCodeAttribute(string attributeTypeName)
        => WithCodeAttribute(attributeTypeName, null);

    public IEnumValueConfiguration WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration>? attributeConfiguration)
    {
        CodeAttributeConfiguration.AddCodeAttribute(_codeAttributes, attributeTypeName, attributeConfiguration);
        return this;
    }

    ISupportsCodeAttributeConfiguration ISupportsCodeAttributeConfiguration.WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration> attributeConfiguration)
        => WithCodeAttribute(attributeTypeName, attributeConfiguration);

    public IEnumValueConfiguration WithLineComment(string comment)
    {
        _comments.Add(new CommentConfiguration(CommentConfiguration.CommentType.Line, comment));
        return this;
    }

    ISupportsLineCommentsConfiguration ISupportsLineCommentsConfiguration.WithLineComment(string comment)
        => WithLineComment(comment);

    public IEnumValueConfiguration WithBlockComment(string comment)
    {
        _comments.Add(new CommentConfiguration(CommentConfiguration.CommentType.Block, comment));
        return this;
    }

    ISupportsLineCommentsConfiguration ISupportsLineCommentsConfiguration.WithBlockComment(string comment)
        => WithBlockComment(comment);

    public IEnumValueConfiguration WithDocComment(string comment)
    {
        _comments.Add(new CommentConfiguration(CommentConfiguration.CommentType.Doc, comment));
        return this;
    }

    ISupportsLineCommentsConfiguration ISupportsLineCommentsConfiguration.WithDocComment(string comment)
        => WithDocComment(comment);

    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        foreach (var comment in _comments)
            comment.WriteTo(sourceBuilder);

        foreach (var attribute in _codeAttributes)
        {
            attribute.WriteTo(sourceBuilder);
            sourceBuilder.AppendLine();
        }

        sourceBuilder.Append(_name);

        if (_value is not null)
            sourceBuilder.Append($" = {_value}");
    }
}
