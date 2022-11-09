namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IMethodConfiguration<T> : IGenericMemberConfiguration<T>, IDefinesParametersConfiguration<T>
    where T : IMethodConfiguration<T>
{
    T WithReturnType(string typeName);
}

public interface IMethodConfiguration : IMethodConfiguration<IMethodConfiguration>
{
}

internal abstract class MethodConfiguration<T> : GenericMemberConfiguration<T>, IMethodConfiguration<T>
    where T : IMethodConfiguration<T>
{
    private readonly List<IParameterConfiguration> _parameters = new();
    private string _returnType = "void";

    protected MethodConfiguration(string memberName)
        : base(memberName)
    {
    }

    protected override int StartCapacity => 128;

    public T WithParameter(string type, string name, Action<IParameterConfiguration> parameterConfiguration)
    {
        ParameterConfiguration.AddParameter(_parameters, type, name, parameterConfiguration);
        return This;
    }

    IDefinesParametersConfiguration IDefinesParametersConfiguration.WithParameter(string type, string name, Action<IParameterConfiguration> parameterConfiguration)
        => WithParameter(type, name, parameterConfiguration);

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

internal sealed class MethodConfiguration : MethodConfiguration<IMethodConfiguration>, IMethodConfiguration
{
    public MethodConfiguration(string memberName)
        : base(memberName)
    {
    }

    protected override IMethodConfiguration This => this;

    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        WriteCodeAttributesTo(sourceBuilder);
        WriteKeywordsTo(sourceBuilder);
        WriteReturnTypeTo(sourceBuilder);
        WriteNameTo(sourceBuilder);
        sourceBuilder.Append('(');
        WriteParametersTo(sourceBuilder);
        sourceBuilder.Append(')');
        WriteGenericConstraintsTo(sourceBuilder);
    }
}