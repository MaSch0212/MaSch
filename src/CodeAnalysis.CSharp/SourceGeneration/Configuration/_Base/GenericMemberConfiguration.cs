﻿namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

internal abstract class GenericMemberConfiguration<T> : MemberConfiguration<T>, IGenericMemberConfiguration<T>
    where T : IGenericMemberConfiguration<T>
{
    private readonly List<IGenericParameterConfiguration> _genericParameters = new();

    protected GenericMemberConfiguration(string memberName)
        : base(memberName)
    {
    }

    public override string MemberName
    {
        get
        {
            var options = new SourceBuilderOptions
            {
                Capacity = 64,
                IncludeFileHeader = false,
            };
            var builder = SourceBuilder.Create(options);
            WriteNameTo(builder);
            return builder.ToString();
        }
    }

    public string MemberNameWithoutGenericParameters => base.MemberName;

    public T WithGenericParameter(string name)
        => WithGenericParameter(name, null);

    IGenericMemberConfiguration IGenericMemberConfiguration.WithGenericParameter(string name)
        => WithGenericParameter(name, null);

    public T WithGenericParameter(string name, Action<IGenericParameterConfiguration>? parameterConfiguration)
    {
        var config = new GenericParameterConfiguration(name);
        parameterConfiguration?.Invoke(config);
        _genericParameters.Add(config);
        return This;
    }

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
