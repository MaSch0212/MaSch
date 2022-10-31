namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public abstract class MemberConfiguration<T> : CodeConfiguration, IMemberConfiguration<T>
    where T : IMemberConfiguration<T>
{
    private readonly string _memberName;
    private readonly List<ICodeAttributeConfiguration> _codeAttributes = new();
    private AccessModifier _accessModifier = AccessModifier.Default;
    private MemberKeyword _keywords = MemberKeyword.None;

    protected MemberConfiguration(string memberName)
    {
        _memberName = memberName;
    }

    /// <inheritdoc/>
    public virtual string MemberName => _memberName;

    protected abstract T This { get; }

    /// <inheritdoc/>
    public T WithAccessModifier(AccessModifier accessModifier)
    {
        _accessModifier = accessModifier;
        return This;
    }

    /// <inheritdoc/>
    IMemberConfiguration IMemberConfiguration.WithAccessModifier(AccessModifier accessModifier)
        => WithAccessModifier(accessModifier);

    /// <inheritdoc/>
    public T WithCodeAttribute<TParams>(string attributeTypeName, TParams @params, Action<ICodeAttributeConfiguration, TParams> attributeConfiguration)
    {
        var config = new CodeAttributeConfiguration(attributeTypeName);
        attributeConfiguration?.Invoke(config, @params);
        _codeAttributes.Add(config);
        return This;
    }

    /// <inheritdoc/>
    ISupportsCodeAttributeConfiguration ISupportsCodeAttributeConfiguration.WithCodeAttribute<TParams>(string attributeTypeName, TParams @params, Action<ICodeAttributeConfiguration, TParams> attributeConfiguration)
        => WithCodeAttribute(attributeTypeName, @params, attributeConfiguration);

    /// <inheritdoc/>
    public T WithKeyword(MemberKeyword keyword)
    {
        _keywords |= keyword;
        return This;
    }

    /// <inheritdoc/>
    IMemberConfiguration IMemberConfiguration.WithKeyword(MemberKeyword keyword)
        => WithKeyword(keyword);

    protected void WriteCodeAttributesTo(ISourceBuilder sourceBuilder)
    {
        foreach (var codeAttribute in _codeAttributes)
        {
            codeAttribute.WriteTo(sourceBuilder);
            sourceBuilder.AppendLine();
        }
    }

    protected void WriteKeywordsTo(ISourceBuilder sourceBuilder)
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
