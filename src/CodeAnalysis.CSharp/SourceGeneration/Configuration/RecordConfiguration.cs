namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface IRecordConfiguration :
    ITypeConfiguration<IRecordConfiguration>,
    IDefinesParametersConfiguration<IRecordConfiguration>
{
}

public sealed class RecordConfiguration : TypeConfiguration<IRecordConfiguration>, IRecordConfiguration
{
    private readonly List<IParameterConfiguration> _parameters = new();

    public RecordConfiguration(string recordName)
        : base(recordName)
    {
    }

    protected override IRecordConfiguration This => this;

    public IRecordConfiguration WithParameter<TParams>(string type, string name, TParams @params, Action<IParameterConfiguration, TParams> parameterConfiguration)
    {
        var config = new ParameterConfiguration(type, name) { LineBreakAfterCodeAttribute = true };
        parameterConfiguration?.Invoke(config, @params);
        _parameters.Add(config);
        return This;
    }

    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        WriteCodeAttributesTo(sourceBuilder);
        WriteKeywordsTo(sourceBuilder);
        sourceBuilder.Append("record ");
        WriteNameTo(sourceBuilder);
        WriteParametersTo(sourceBuilder);
        WriteBaseTypesTo(sourceBuilder);
        WriteGenericConstraintsTo(sourceBuilder);
    }

    private void WriteParametersTo(ISourceBuilder sourceBuilder)
    {
        if (_parameters.Count == 0)
            return;

        using (sourceBuilder.Indent())
        {
            sourceBuilder.Append('(');
            for (int i = 0; i < _parameters.Count; i++)
            {
                sourceBuilder.AppendLine();
                _parameters[i].WriteTo(sourceBuilder);
                if (i < _parameters.Count - 1)
                    sourceBuilder.Append(',');
            }

            sourceBuilder.Append(')');
        }
    }
}
