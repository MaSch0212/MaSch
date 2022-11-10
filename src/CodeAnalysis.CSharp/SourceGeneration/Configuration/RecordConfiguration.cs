namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IRecordConfiguration :
    ITypeConfiguration<IRecordConfiguration>,
    IDefinesParametersConfiguration<IRecordConfiguration>
{
    IRecordConfiguration WithSinglelineParameters();
}

internal sealed class RecordConfiguration : TypeConfiguration<IRecordConfiguration>, IRecordConfiguration
{
    private readonly List<IParameterConfiguration> _parameters = new();

    public RecordConfiguration(string recordName)
        : base(recordName)
    {
    }

    public bool MultilineParameters { get; set; } = true;

    protected override IRecordConfiguration This => this;

    public IRecordConfiguration WithParameter(string type, string name, Action<IParameterConfiguration> parameterConfiguration)
    {
        ParameterConfiguration.AddParameter(_parameters, type, name, parameterConfiguration);
        return This;
    }

    IDefinesParametersConfiguration IDefinesParametersConfiguration.WithParameter(string type, string name, Action<IParameterConfiguration> parameterConfiguration)
        => WithParameter(type, name, parameterConfiguration);

    public IRecordConfiguration WithSinglelineParameters()
    {
        MultilineParameters = false;
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
        if (_parameters.Count > 0)
            ParameterConfiguration.WriteParametersTo(_parameters, sourceBuilder, MultilineParameters);
    }
}
