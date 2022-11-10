namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IConstructorConfiguration : IMemberConfiguration<IConstructorConfiguration>, IDefinesParametersConfiguration<IConstructorConfiguration>
{
    IConstructorConfiguration WithMultilineParameters();
}

internal sealed class ConstructorConfiguration : MemberConfiguration<IConstructorConfiguration>, IConstructorConfiguration
{
    private readonly List<IParameterConfiguration> _parameters = new();

    public ConstructorConfiguration(string containingTypeName)
        : base(containingTypeName)
    {
    }

    public bool MultilineParameters { get; set; }

    protected override IConstructorConfiguration This => this;

    protected override int StartCapacity => 64;

    public IConstructorConfiguration WithParameter(string type, string name, Action<IParameterConfiguration> parameterConfiguration)
    {
        ParameterConfiguration.AddParameter(_parameters, type, name, parameterConfiguration);
        return This;
    }

    IDefinesParametersConfiguration IDefinesParametersConfiguration.WithParameter(string type, string name, Action<IParameterConfiguration> parameterConfiguration)
        => WithParameter(type, name, parameterConfiguration);

    public IConstructorConfiguration WithMultilineParameters()
    {
        MultilineParameters = true;
        return This;
    }

    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        WriteCodeAttributesTo(sourceBuilder);
        WriteKeywordsTo(sourceBuilder);
        WriteNameTo(sourceBuilder);
        ParameterConfiguration.WriteParametersTo(_parameters, sourceBuilder, MultilineParameters);
    }
}
