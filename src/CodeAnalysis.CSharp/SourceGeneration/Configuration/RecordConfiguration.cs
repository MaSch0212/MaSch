namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IRecordConfiguration :
    ITypeConfiguration<IRecordConfiguration>,
    IDefinesParametersConfiguration<IRecordConfiguration>
{
}

internal sealed class RecordConfiguration : TypeConfiguration<IRecordConfiguration>, IRecordConfiguration
{
    private readonly List<IParameterConfiguration> _parameters = new();

    public RecordConfiguration(string recordName)
        : base(recordName)
    {
    }

    /// <inheritdoc/>
    protected override IRecordConfiguration This => this;

    /// <inheritdoc/>
    public IRecordConfiguration WithParameter(string type, string name, Action<IParameterConfiguration> parameterConfiguration)
    {
        var config = ParameterConfiguration.AddParameter(_parameters, type, name, parameterConfiguration);
        config.LineBreakAfterCodeAttribute = true;
        return This;
    }

    /// <inheritdoc/>
    IDefinesParametersConfiguration IDefinesParametersConfiguration.WithParameter(string type, string name, Action<IParameterConfiguration> parameterConfiguration)
        => WithParameter(type, name, parameterConfiguration);

    /// <inheritdoc/>
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
        ParameterConfiguration.WriteParametersTo(_parameters, sourceBuilder);
    }
}
