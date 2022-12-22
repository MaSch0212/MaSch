namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

/// <summary>
/// Represents configuration of a enum value code element. This is used to generate code in the <see cref="ISourceBuilder"/>.
/// </summary>
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Interface")]
public interface IEnumValueConfiguration : ISupportsCodeAttributeConfiguration<IEnumValueConfiguration>, ISupportsLineCommentsConfiguration<IEnumValueConfiguration>
{
}

internal sealed class EnumValueConfiguration : CodeConfigurationBase, IEnumValueConfiguration
{
    private readonly List<ICodeAttributeConfiguration> _codeAttributes = new();
    private readonly List<ICommentConfiguration> _comments = new();
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

    public IReadOnlyList<ICodeAttributeConfiguration> Attributes => new ReadOnlyCollection<ICodeAttributeConfiguration>(_codeAttributes);
    public IReadOnlyList<ICommentConfiguration> Comments => new ReadOnlyCollection<ICommentConfiguration>(_comments);

    protected override int StartCapacity => 16;

    public IEnumValueConfiguration WithCodeAttribute(string attributeTypeName)
        => WithCodeAttribute(attributeTypeName, null);

    public IEnumValueConfiguration WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration>? attributeConfiguration)
    {
        CodeAttributeConfiguration.AddCodeAttribute(_codeAttributes, attributeTypeName, attributeConfiguration);
        return this;
    }

    public IEnumValueConfiguration WithLineComment(string comment)
    {
        _comments.Add(new CommentConfiguration(CommentType.Line, comment));
        return this;
    }

    public IEnumValueConfiguration WithBlockComment(string comment)
    {
        _comments.Add(new CommentConfiguration(CommentType.Block, comment));
        return this;
    }

    public IEnumValueConfiguration WithDocComment(string comment)
    {
        _comments.Add(new CommentConfiguration(CommentType.Doc, comment));
        return this;
    }

    ISupportsCodeAttributeConfiguration ISupportsCodeAttributeConfiguration.WithCodeAttribute(string attributeTypeName)
        => WithCodeAttribute(attributeTypeName, null);

    ISupportsCodeAttributeConfiguration ISupportsCodeAttributeConfiguration.WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration> attributeConfiguration)
        => WithCodeAttribute(attributeTypeName, attributeConfiguration);

    ISupportsLineCommentsConfiguration ISupportsLineCommentsConfiguration.WithLineComment(string comment)
        => WithLineComment(comment);

    ISupportsLineCommentsConfiguration ISupportsLineCommentsConfiguration.WithBlockComment(string comment)
        => WithBlockComment(comment);

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
