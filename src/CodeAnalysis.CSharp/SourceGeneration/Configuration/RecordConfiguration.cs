namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

/// <summary>
/// Represents configuration of a record code element. This is used to generate code in the <see cref="ISourceBuilder"/>.
/// </summary>
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Interface")]
public interface IRecordConfiguration :
    ITypeConfiguration<IRecordConfiguration>,
    IDefinesParametersConfiguration<IRecordConfiguration>
{
    /// <summary>
    /// Sets <see cref="IDefinesParametersConfiguration.MultilineParameters"/> to <c>false</c>.
    /// </summary>
    /// <returns>A reference to this instance after the operation has completed.</returns>
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
    public IReadOnlyList<IParameterConfiguration> Parameters => new ReadOnlyCollection<IParameterConfiguration>(_parameters);

    protected override IRecordConfiguration This => this;

    public IRecordConfiguration WithParameter(string type, string name)
        => WithParameter(type, name, null);

    public IRecordConfiguration WithParameter(string type, string name, Action<IParameterConfiguration>? parameterConfiguration)
    {
        ParameterConfiguration.AddParameter(_parameters, type, name, parameterConfiguration);
        return This;
    }

    public IRecordConfiguration WithSinglelineParameters()
    {
        MultilineParameters = false;
        return This;
    }

    IDefinesParametersConfiguration IDefinesParametersConfiguration.WithParameter(string type, string name)
        => WithParameter(type, name, null);

    IDefinesParametersConfiguration IDefinesParametersConfiguration.WithParameter(string type, string name, Action<IParameterConfiguration> parameterConfiguration)
        => WithParameter(type, name, parameterConfiguration);

    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        WriteCommentsTo(sourceBuilder);
        WriteCodeAttributesTo(sourceBuilder);
        WriteKeywordsTo(sourceBuilder);
        sourceBuilder.Append("record ");
        WriteNameTo(sourceBuilder);

        sourceBuilder.Indent(sourceBuilder =>
        {
            WriteParametersTo(sourceBuilder);

            if (IsDerivingOrImplementingInterface)
            {
                if (_parameters.Count > 0 && MultilineParameters)
                    sourceBuilder.AppendLine();
                else
                    sourceBuilder.Append(' ');
            }

            WriteBaseTypesTo(sourceBuilder);
            WriteGenericConstraintsTo(sourceBuilder);
        });
    }

    private void WriteParametersTo(ISourceBuilder sourceBuilder)
    {
        if (_parameters.Count > 0)
        {
            sourceBuilder.Append('(');
            ParameterConfiguration.WriteParametersTo(_parameters, sourceBuilder, MultilineParameters);
            sourceBuilder.Append(')');
        }
    }
}
