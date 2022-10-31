namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public abstract class GenericMemberConfiguration<T> : MemberConfiguration<T>, IGenericMemberConfiguration<T>
    where T : IGenericMemberConfiguration<T>
{
    private readonly List<IGenericParameterConfiguration> _genericParameters = new();

    protected GenericMemberConfiguration(string memberName)
        : base(memberName)
    {
    }

    /// <inheritdoc/>
    public override string MemberName
    {
        get
        {
            var builder = SourceBuilder.Create(capacity: 64, autoAddFileHeader: false);
            WriteNameTo(builder);
            return builder.ToString();
        }
    }

    /// <inheritdoc/>
    public string MemberNameWithoutGenericParameters => base.MemberName;

    /// <inheritdoc/>
    public T WithGenericParameter<TParams>(string name, TParams @params, Action<IGenericParameterConfiguration, TParams> parameterConfiguration)
    {
        var config = new GenericParameterConfiguration(name);
        parameterConfiguration?.Invoke(config, @params);
        _genericParameters.Add(config);
        return This;
    }

    /// <inheritdoc/>
    IGenericMemberConfiguration IGenericMemberConfiguration.WithGenericParameter<TParams>(string name, TParams @params, Action<IGenericParameterConfiguration, TParams> parameterConfiguration)
        => WithGenericParameter(name, @params, parameterConfiguration);

    /// <inheritdoc/>
    protected override void WriteNameTo(ISourceBuilder sourceBuilder)
    {
        base.WriteNameTo(sourceBuilder);

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
