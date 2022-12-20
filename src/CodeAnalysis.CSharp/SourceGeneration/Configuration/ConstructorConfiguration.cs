namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IConstructorConfiguration : IMemberConfiguration<IConstructorConfiguration>, IDefinesParametersConfiguration<IConstructorConfiguration>
{
    MethodBodyType BodyType { get; }

    IConstructorConfiguration WithMultilineParameters();
    IConstructorConfiguration CallsBase();
    IConstructorConfiguration CallsBase(Action<ISuperConstructorConfiguration> configuration);
    IConstructorConfiguration CallsThis();
    IConstructorConfiguration CallsThis(Action<ISuperConstructorConfiguration> configuration);
    IConstructorConfiguration AsExpression(bool placeInNewLine = true);
}

internal sealed class ConstructorConfiguration : MemberConfiguration<IConstructorConfiguration>, IConstructorConfiguration
{
    private readonly List<IParameterConfiguration> _parameters = new();
    private readonly string? _containingTypeName;
    private ISuperConstructorConfiguration? _superConstructor;

    public ConstructorConfiguration(string? containingTypeName)
        : base(containingTypeName!)
    {
        _containingTypeName = containingTypeName;
    }

    public bool MultilineParameters { get; set; }

    protected override IConstructorConfiguration This => this;
    protected override int StartCapacity => 64;

    public MethodBodyType BodyType { get; private set; } = MethodBodyType.Block;

    public IConstructorConfiguration WithParameter(string type, string name)
        => WithParameter(type, name, null);

    IDefinesParametersConfiguration IDefinesParametersConfiguration.WithParameter(string type, string name)
        => WithParameter(type, name, null);

    public IConstructorConfiguration WithParameter(string type, string name, Action<IParameterConfiguration>? parameterConfiguration)
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

    public IConstructorConfiguration CallsBase()
    {
        _superConstructor = new SuperConstructorConfiguration("base");
        return this;
    }

    public IConstructorConfiguration CallsBase(Action<ISuperConstructorConfiguration> configuration)
    {
        var config = new SuperConstructorConfiguration("base");
        configuration(config);
        _superConstructor = config;
        return this;
    }

    public IConstructorConfiguration CallsThis()
    {
        _superConstructor = new SuperConstructorConfiguration("this");
        return this;
    }

    public IConstructorConfiguration CallsThis(Action<ISuperConstructorConfiguration> configuration)
    {
        var config = new SuperConstructorConfiguration("this");
        configuration(config);
        _superConstructor = config;
        return this;
    }

    public IConstructorConfiguration AsExpression(bool placeInNewLine = true)
    {
        BodyType = placeInNewLine ? MethodBodyType.ExpressionNewLine : MethodBodyType.Expression;
        return this;
    }

    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        WriteCommentsTo(sourceBuilder);
        WriteCodeAttributesTo(sourceBuilder);
        WriteKeywordsTo(sourceBuilder);
        WriteNameTo(sourceBuilder);
        using (sourceBuilder.Indent())
        {
            sourceBuilder.Append('(');
            ParameterConfiguration.WriteParametersTo(_parameters, sourceBuilder, MultilineParameters);
            sourceBuilder.Append(')');

            if (_superConstructor is not null)
            {
                sourceBuilder.AppendLine();
                _superConstructor.WriteTo(sourceBuilder);
            }
        }
    }

    protected override void WriteNameTo(ISourceBuilder sourceBuilder)
    {
        if (_containingTypeName is not null)
            base.WriteNameTo(sourceBuilder);
        else
            sourceBuilder.Append(sourceBuilder.CurrentTypeName);
    }
}
