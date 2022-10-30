namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public abstract class MemberConfiguration<T> : CodeConfiguration, IMemberConfiguration<T>
    where T : IMemberConfiguration<T>
{
    private readonly string _memberName;
    private readonly List<ICodeAttributeConfiguration> _codeAttributes = new();
    private readonly List<IGenericParameterConfiguration> _genericParameters = new();
    private AccessModifier _accessModifier = AccessModifier.Default;
    private MemberKeyword _keywords = MemberKeyword.None;

    protected MemberConfiguration(string memberName)
    {
        _memberName = memberName;
    }

    protected abstract T This { get; }

    public T WithAccessModifier(AccessModifier accessModifier)
    {
        _accessModifier = accessModifier;
        return This;
    }

    public T WithCodeAttribute<TParams>(string attributeTypeName, TParams @params, Action<ICodeAttributeConfiguration, TParams> attributeConfiguration)
    {
        var config = new CodeAttributeConfiguration(attributeTypeName);
        attributeConfiguration?.Invoke(config, @params);
        _codeAttributes.Add(config);
        return This;
    }

    public T WithGenericParameter<TParams>(string name, TParams @params, Action<IGenericParameterConfiguration, TParams> parameterConfiguration)
    {
        var config = new GenericParameterConfiguration(name);
        parameterConfiguration?.Invoke(config, @params);
        _genericParameters.Add(config);
        return This;
    }

    public T WithKeyword(MemberKeyword keyword)
    {
        _keywords |= keyword;
        return This;
    }

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

    protected void WriteNameTo(ISourceBuilder sourceBuilder)
    {
        sourceBuilder.Append(_memberName);

        if (_genericParameters.Count > 0)
        {
            sourceBuilder.Append('<');

            bool isFirst = true;
            foreach (var genericParameter in _genericParameters)
            {
                if (!isFirst)
                    sourceBuilder.Append(", ");
                genericParameter.WriteParameterTo(sourceBuilder);

                isFirst = false;
            }

            sourceBuilder.Append('>');
        }
    }

    protected void WriteGenericConstraintsTo(ISourceBuilder sourceBuilder)
    {
        if (_genericParameters.Count == 0)
            return;

        using (sourceBuilder.Indent())
        {
            foreach (var genericParameter in _genericParameters.Where(x => x.HasConstraints))
            {
                sourceBuilder.AppendLine();
                genericParameter.WriteConstraintTo(sourceBuilder);
            }
        }
    }
}
