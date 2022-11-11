﻿namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IConstructorConfiguration : IMemberConfiguration<IConstructorConfiguration>, IDefinesParametersConfiguration<IConstructorConfiguration>
{
    MethodBodyType BodyType { get; }

    IConstructorConfiguration WithMultilineParameters();
    IConstructorConfiguration CallsBase(Action<ISuperConstructorConfiguration> configuration);
    IConstructorConfiguration CallsThis(Action<ISuperConstructorConfiguration> configuration);
    IConstructorConfiguration AsExpression(bool placeInNewLine = true);
}

internal sealed class ConstructorConfiguration : MemberConfiguration<IConstructorConfiguration>, IConstructorConfiguration
{
    private readonly List<IParameterConfiguration> _parameters = new();
    private ISuperConstructorConfiguration? _superConstructor;

    public ConstructorConfiguration(string containingTypeName)
        : base(containingTypeName)
    {
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

    public IConstructorConfiguration CallsBase(Action<ISuperConstructorConfiguration> configuration)
    {
        var config = new SuperConstructorConfiguration("base");
        configuration(config);
        _superConstructor = config;
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
        WriteCodeAttributesTo(sourceBuilder);
        WriteKeywordsTo(sourceBuilder);
        WriteNameTo(sourceBuilder);
        ParameterConfiguration.WriteParametersTo(_parameters, sourceBuilder, MultilineParameters);

        if (_superConstructor is not null)
        {
            using (sourceBuilder.Indent())
            {
                sourceBuilder.AppendLine();
                _superConstructor.WriteTo(sourceBuilder);
            }
        }
    }
}
