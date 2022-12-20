namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

internal abstract class MemberConfiguration<T> : CodeConfigurationBase, IMemberConfiguration<T>
    where T : IMemberConfiguration<T>
{
    private readonly string _memberName;
    private readonly List<ICodeAttributeConfiguration> _codeAttributes = new();
    private readonly List<CommentConfiguration> _comments = new();
    private AccessModifier _accessModifier = AccessModifier.Default;
    private MemberKeyword _keywords = MemberKeyword.None;

    protected MemberConfiguration(string memberName)
    {
        _memberName = memberName;
    }

    public virtual string MemberName => _memberName;
    public virtual bool HasComments => _comments.Count > 0;
    public virtual bool HasAttributes => _codeAttributes.Count > 0;

    protected abstract T This { get; }

    public T WithAccessModifier(AccessModifier accessModifier)
    {
        _accessModifier = accessModifier;
        return This;
    }

    ISupportsAccessModifierConfiguration ISupportsAccessModifierConfiguration.WithAccessModifier(AccessModifier accessModifier)
        => WithAccessModifier(accessModifier);

    public T WithCodeAttribute(string attributeTypeName)
        => WithCodeAttribute(attributeTypeName, null);

    ISupportsCodeAttributeConfiguration ISupportsCodeAttributeConfiguration.WithCodeAttribute(string attributeTypeName)
        => WithCodeAttribute(attributeTypeName, null);

    public T WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration>? attributeConfiguration)
    {
        var config = new CodeAttributeConfiguration(attributeTypeName);
        attributeConfiguration?.Invoke(config);
        _codeAttributes.Add(config);
        return This;
    }

    ISupportsCodeAttributeConfiguration ISupportsCodeAttributeConfiguration.WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration> attributeConfiguration)
        => WithCodeAttribute(attributeTypeName, attributeConfiguration);

    public T WithKeyword(MemberKeyword keyword)
    {
        _keywords |= keyword;
        return This;
    }

    IMemberConfiguration IMemberConfiguration.WithKeyword(MemberKeyword keyword)
        => WithKeyword(keyword);

    public T WithLineComment(string comment)
    {
        _comments.Add(new CommentConfiguration(CommentConfiguration.CommentType.Line, comment));
        return This;
    }

    ISupportsLineCommentsConfiguration ISupportsLineCommentsConfiguration.WithLineComment(string comment)
        => WithLineComment(comment);

    public T WithBlockComment(string comment)
    {
        _comments.Add(new CommentConfiguration(CommentConfiguration.CommentType.Block, comment));
        return This;
    }

    ISupportsLineCommentsConfiguration ISupportsLineCommentsConfiguration.WithBlockComment(string comment)
        => WithBlockComment(comment);

    public T WithDocComment(string comment)
    {
        _comments.Add(new CommentConfiguration(CommentConfiguration.CommentType.Doc, comment));
        return This;
    }

    ISupportsLineCommentsConfiguration ISupportsLineCommentsConfiguration.WithDocComment(string comment)
        => WithDocComment(comment);

    protected virtual void WriteCommentsTo(ISourceBuilder sourceBuilder)
    {
        foreach (var comment in _comments)
            comment.WriteTo(sourceBuilder);
    }

    protected virtual void WriteCodeAttributesTo(ISourceBuilder sourceBuilder)
    {
        foreach (var codeAttribute in _codeAttributes)
        {
            codeAttribute.WriteTo(sourceBuilder);
            sourceBuilder.AppendLine();
        }
    }

    protected virtual void WriteKeywordsTo(ISourceBuilder sourceBuilder)
    {
        if (_accessModifier is not AccessModifier.Default)
            sourceBuilder.Append(_accessModifier.ToMemberPrefix());
        if (_keywords is not MemberKeyword.None)
            sourceBuilder.Append(_keywords.ToMemberPrefix());
    }

    protected virtual void WriteNameTo(ISourceBuilder sourceBuilder)
    {
        sourceBuilder.Append(_memberName);
    }
}
