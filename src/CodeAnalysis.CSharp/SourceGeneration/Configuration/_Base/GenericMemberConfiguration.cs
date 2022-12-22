namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

internal abstract class GenericMemberConfiguration<T> : MemberConfiguration<T>, IGenericMemberConfiguration<T>
    where T : IGenericMemberConfiguration<T>
{
    private readonly List<IGenericParameterConfiguration> _genericParameters = new();

    protected GenericMemberConfiguration(string memberName)
        : base(memberName)
    {
    }

    [SuppressMessage("Critical Bug", "S4275:Getters and setters should access the expected fields", Justification = "The meaning is changed here because we want to include the generic parameters")]
    public override string MemberName
    {
        get
        {
            var builder = CreateSourceBuilder(64);
            WriteNameTo(builder);
            return builder.ToString();
        }
    }

    public string MemberNameWithoutGenericParameters => base.MemberName;

    public IReadOnlyList<IGenericParameterConfiguration> GenericParameters => new ReadOnlyCollection<IGenericParameterConfiguration>(_genericParameters);

    public T WithGenericParameter(string name)
        => WithGenericParameter(name, null);

    public T WithGenericParameter(string name, Action<IGenericParameterConfiguration>? parameterConfiguration)
    {
        var config = new GenericParameterConfiguration(name);
        parameterConfiguration?.Invoke(config);
        _genericParameters.Add(config);
        return This;
    }

    IGenericMemberConfiguration IGenericMemberConfiguration.WithGenericParameter(string name)
        => WithGenericParameter(name, null);

    IGenericMemberConfiguration IGenericMemberConfiguration.WithGenericParameter(string name, Action<IGenericParameterConfiguration> parameterConfiguration)
        => WithGenericParameter(name, parameterConfiguration);

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

        foreach (var genericParameter in _genericParameters.Where(x => x.HasConstraints))
        {
            sourceBuilder.AppendLine();
            genericParameter.WriteConstraintTo(sourceBuilder);
        }
    }
}
