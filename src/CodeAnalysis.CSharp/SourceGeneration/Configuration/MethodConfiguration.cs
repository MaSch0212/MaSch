namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IMethodConfiguration<T> : IGenericMemberConfiguration<T>, IDefinesParametersConfiguration<T>
    where T : IMethodConfiguration<T>
{
    T WithReturnType(string typeName);
}

public interface IMethodConfiguration : IMethodConfiguration<IMethodConfiguration>
{
}

public abstract class MethodConfiguration<T> : GenericMemberConfiguration<T>, IMethodConfiguration<T>
    where T : IMethodConfiguration<T>
{
    private readonly List<IParameterConfiguration> _parameters = new();
    private string _returnType = "void";

    protected MethodConfiguration(string memberName)
        : base(memberName)
    {
    }

    /// <inheritdoc/>
    protected override int StartCapacity => 128;

    /// <inheritdoc/>
    public T WithParameter<TParams>(string type, string name, TParams @params, Action<IParameterConfiguration, TParams> parameterConfiguration)
    {
        ParameterConfiguration.AddParameter(_parameters, type, name, @params, parameterConfiguration);
        return This;
    }

    /// <inheritdoc/>
    IDefinesParametersConfiguration IDefinesParametersConfiguration.WithParameter<TParams>(string type, string name, TParams @params, Action<IParameterConfiguration, TParams> parameterConfiguration)
        => WithParameter(type, name, @params, parameterConfiguration);

    /// <inheritdoc/>
    public T WithReturnType(string typeName)
    {
        _returnType = typeName;
        return This;
    }

    protected void WriteReturnTypeTo(ISourceBuilder sourceBuilder)
    {
        sourceBuilder.Append(_returnType).Append(' ');
    }

    protected void WriteParametersTo(ISourceBuilder sourceBuilder)
    {
        ParameterConfiguration.WriteParametersTo(_parameters, sourceBuilder);
    }
}

public sealed class MethodConfiguration : MethodConfiguration<IMethodConfiguration>, IMethodConfiguration
{
    public MethodConfiguration(string memberName)
        : base(memberName)
    {
    }

    /// <inheritdoc/>
    protected override IMethodConfiguration This => this;

    /// <inheritdoc/>
    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        WriteCodeAttributesTo(sourceBuilder);
        WriteKeywordsTo(sourceBuilder);
        WriteReturnTypeTo(sourceBuilder);
        WriteNameTo(sourceBuilder);
        WriteParametersTo(sourceBuilder);
        WriteGenericConstraintsTo(sourceBuilder);
    }
}