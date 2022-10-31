namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IConstructorConfiguration : IMemberConfiguration<IConstructorConfiguration>, IDefinesParametersConfiguration<IConstructorConfiguration>
{
}

public class ConstructorConfiguration : MemberConfiguration<IConstructorConfiguration>, IConstructorConfiguration
{
    private readonly List<IParameterConfiguration> _parameters;

    public ConstructorConfiguration(string containingTypeName)
        : base(containingTypeName)
    {
    }

    /// <inheritdoc/>
    protected override IConstructorConfiguration This => this;
    /// <inheritdoc/>
    protected override int StartCapacity => 64;

    /// <inheritdoc/>
    public IConstructorConfiguration WithParameter<TParams>(string type, string name, TParams @params, Action<IParameterConfiguration, TParams> parameterConfiguration)
    {
        ParameterConfiguration.AddParameter(_parameters, type, name, @params, parameterConfiguration);
        return This;
    }

    /// <inheritdoc/>
    IDefinesParametersConfiguration IDefinesParametersConfiguration.WithParameter<TParams>(string type, string name, TParams @params, Action<IParameterConfiguration, TParams> parameterConfiguration)
        => WithParameter(type, name, @params, parameterConfiguration);

    /// <inheritdoc/>
    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        throw new NotImplementedException();
    }
}
