namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IConstructorConfiguration : IMemberConfiguration<IConstructorConfiguration>, IDefinesParametersConfiguration<IConstructorConfiguration>
{
}

internal sealed class ConstructorConfiguration : MemberConfiguration<IConstructorConfiguration>, IConstructorConfiguration
{
    private readonly List<IParameterConfiguration> _parameters = new();

    public ConstructorConfiguration(string containingTypeName)
        : base(containingTypeName)
    {
    }

    /// <inheritdoc/>
    protected override IConstructorConfiguration This => this;

    /// <inheritdoc/>
    protected override int StartCapacity => 64;

    /// <inheritdoc/>
    public IConstructorConfiguration WithParameter(string type, string name, Action<IParameterConfiguration> parameterConfiguration)
    {
        ParameterConfiguration.AddParameter(_parameters, type, name, parameterConfiguration);
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
        WriteNameTo(sourceBuilder);
        ParameterConfiguration.WriteParametersTo(_parameters, sourceBuilder);
    }
}
