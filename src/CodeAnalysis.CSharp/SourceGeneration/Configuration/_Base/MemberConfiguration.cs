namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

internal abstract class MemberConfiguration<T> : CodeConfigurationBase, IMemberConfiguration<T>
    where T : IMemberConfiguration<T>
{
    private readonly string _memberName;
    private readonly List<ICodeAttributeConfiguration> _codeAttributes = new();
    private readonly List<ICommentConfiguration> _comments = new();

    protected MemberConfiguration(string memberName)
    {
        _memberName = memberName;
    }

    public virtual string MemberName => _memberName;
    public IReadOnlyList<ICodeAttributeConfiguration> Attributes => new ReadOnlyCollection<ICodeAttributeConfiguration>(_codeAttributes);
    public IReadOnlyList<ICommentConfiguration> Comments => new ReadOnlyCollection<ICommentConfiguration>(_comments);
    public AccessModifier AccessModifier { get; private set; } = AccessModifier.Default;
    public MemberKeyword Keywords { get; private set; } = MemberKeyword.None;

    protected abstract T This { get; }

    public T WithAccessModifier(AccessModifier accessModifier)
    {
        AccessModifier = accessModifier;
        return This;
    }

    public T WithCodeAttribute(string attributeTypeName)
        => WithCodeAttribute(attributeTypeName, null);

    public T WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration>? attributeConfiguration)
    {
        var config = new CodeAttributeConfiguration(attributeTypeName);
        attributeConfiguration?.Invoke(config);
        _codeAttributes.Add(config);
        return This;
    }

    public T WithKeyword(MemberKeyword keyword)
    {
        Keywords |= keyword;
        return This;
    }

    public T WithLineComment(string comment)
    {
        _comments.Add(new CommentConfiguration(CommentType.Line, comment));
        return This;
    }

    public T WithBlockComment(string comment)
    {
        _comments.Add(new CommentConfiguration(CommentType.Block, comment));
        return This;
    }

    public T WithDocComment(string comment)
    {
        _comments.Add(new CommentConfiguration(CommentType.Doc, comment));
        return This;
    }

    ISupportsAccessModifierConfiguration ISupportsAccessModifierConfiguration.WithAccessModifier(AccessModifier accessModifier)
        => WithAccessModifier(accessModifier);

    ISupportsCodeAttributeConfiguration ISupportsCodeAttributeConfiguration.WithCodeAttribute(string attributeTypeName)
        => WithCodeAttribute(attributeTypeName, null);

    ISupportsCodeAttributeConfiguration ISupportsCodeAttributeConfiguration.WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration> attributeConfiguration)
        => WithCodeAttribute(attributeTypeName, attributeConfiguration);

    IMemberConfiguration IMemberConfiguration.WithKeyword(MemberKeyword keyword)
        => WithKeyword(keyword);

    ISupportsLineCommentsConfiguration ISupportsLineCommentsConfiguration.WithLineComment(string comment)
        => WithLineComment(comment);

    ISupportsLineCommentsConfiguration ISupportsLineCommentsConfiguration.WithBlockComment(string comment)
        => WithBlockComment(comment);

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
        if (AccessModifier is not AccessModifier.Default)
            sourceBuilder.Append(AccessModifier.ToMemberPrefix());
        if (Keywords is not MemberKeyword.None)
            sourceBuilder.Append(Keywords.ToMemberPrefix());
    }

    protected virtual void WriteNameTo(ISourceBuilder sourceBuilder)
    {
        sourceBuilder.Append(_memberName);
    }
}
