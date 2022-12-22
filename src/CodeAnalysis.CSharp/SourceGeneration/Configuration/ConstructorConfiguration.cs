namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

/// <summary>
/// Represents configuration of a constructor code element. This is used to generate code in the <see cref="ISourceBuilder"/>.
/// </summary>
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Interface")]
public interface IConstructorConfiguration : IMemberConfiguration<IConstructorConfiguration>, IDefinesParametersConfiguration<IConstructorConfiguration>
{
    /// <summary>
    /// Gets the containing type name for the constructor represented by this <see cref="IConstructorConfiguration"/>.
    /// </summary>
    /// <remarks>
    /// If this is <c>null</c>, the containing type name is determined using the <see cref="ISourceBuilder.CurrentTypeName"/> property of the <see cref="ISourceBuilder"/> this <see cref="IConstructorConfiguration"/> is written to.
    /// </remarks>
    string? ContainingTypeName { get; }

    /// <summary>
    /// Gets the body type of the constructor represented by this <see cref="IConstructorConfiguration"/>.
    /// </summary>
    MethodBodyType BodyType { get; }

    /// <summary>
    /// Gets the configuration for the constructor the constructor represented by this <see cref="IConstructorConfiguration"/> calls.
    /// </summary>
    /// <remarks>
    /// This is referencing the <c> : this()</c> and <c> : base()</c> syntax of a constructor. If this is <c>null</c> no constructor is called.
    /// </remarks>
    ISuperConstructorConfiguration? CalledConstructor { get; }

    /// <summary>
    /// Sets <see cref="IDefinesParametersConfiguration.MultilineParameters"/> to <c>true</c>.
    /// </summary>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    IConstructorConfiguration WithMultilineParameters();

    /// <summary>
    /// Sets the constructor, the constructor represented by this <see cref="IConstructorConfiguration"/> calls, to the parameterless base constructor.
    /// </summary>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    IConstructorConfiguration CallsBase();

    /// <summary>
    /// Sets the constructor, the constructor represented by this <see cref="IConstructorConfiguration"/> calls, to a base constructor.
    /// </summary>
    /// <param name="configuration">A function to configure the called constructor.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    IConstructorConfiguration CallsBase(Action<ISuperConstructorConfiguration> configuration);

    /// <summary>
    /// Sets the constructor, the constructor represented by this <see cref="IConstructorConfiguration"/> calls, to the parameterless constructor of the current context.
    /// </summary>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    IConstructorConfiguration CallsThis();

    /// <summary>
    /// Sets the constructor, the constructor represented by this <see cref="IConstructorConfiguration"/> calls, to a constructor of the current context.
    /// </summary>
    /// <param name="configuration">A function to configure the called constructor.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    IConstructorConfiguration CallsThis(Action<ISuperConstructorConfiguration> configuration);

    /// <summary>
    /// Sets the body type of the constructor represented by this <see cref="IConstructorConfiguration"/> to <see cref="MethodBodyType.Expression"/>/<see cref="MethodBodyType.ExpressionNewLine"/>.
    /// </summary>
    /// <param name="placeInNewLine">Determines whether to use <see cref="MethodBodyType.Expression"/> or <see cref="MethodBodyType.ExpressionNewLine"/>.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <remarks>
    /// If <paramref name="placeInNewLine"/> is set to <c>true</c> <see cref="MethodBodyType.ExpressionNewLine"/> is used; otherwise <see cref="MethodBodyType.Expression"/>.
    /// </remarks>
    IConstructorConfiguration AsExpression(bool placeInNewLine = true);
}

internal sealed class ConstructorConfiguration : MemberConfiguration<IConstructorConfiguration>, IConstructorConfiguration
{
    private readonly List<IParameterConfiguration> _parameters = new();

    public ConstructorConfiguration(string? containingTypeName)
        : base(containingTypeName!)
    {
        ContainingTypeName = containingTypeName;
    }

    public bool MultilineParameters { get; set; }
    public IReadOnlyList<IParameterConfiguration> Parameters => new ReadOnlyCollection<IParameterConfiguration>(_parameters);
    public string? ContainingTypeName { get; }
    public ISuperConstructorConfiguration? CalledConstructor { get; private set; }
    public MethodBodyType BodyType { get; private set; } = MethodBodyType.Block;

    protected override IConstructorConfiguration This => this;
    protected override int StartCapacity => 64;

    public IConstructorConfiguration WithParameter(string type, string name)
        => WithParameter(type, name, null);

    public IConstructorConfiguration WithParameter(string type, string name, Action<IParameterConfiguration>? parameterConfiguration)
    {
        ParameterConfiguration.AddParameter(_parameters, type, name, parameterConfiguration);
        return This;
    }

    public IConstructorConfiguration WithMultilineParameters()
    {
        MultilineParameters = true;
        return This;
    }

    public IConstructorConfiguration CallsBase()
    {
        CalledConstructor = new SuperConstructorConfiguration("base");
        return this;
    }

    public IConstructorConfiguration CallsBase(Action<ISuperConstructorConfiguration> configuration)
    {
        var config = new SuperConstructorConfiguration("base");
        configuration(config);
        CalledConstructor = config;
        return this;
    }

    public IConstructorConfiguration CallsThis()
    {
        CalledConstructor = new SuperConstructorConfiguration("this");
        return this;
    }

    public IConstructorConfiguration CallsThis(Action<ISuperConstructorConfiguration> configuration)
    {
        var config = new SuperConstructorConfiguration("this");
        configuration(config);
        CalledConstructor = config;
        return this;
    }

    public IConstructorConfiguration AsExpression(bool placeInNewLine = true)
    {
        BodyType = placeInNewLine ? MethodBodyType.ExpressionNewLine : MethodBodyType.Expression;
        return this;
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
        WriteNameTo(sourceBuilder);
        sourceBuilder.Indent(sourceBuilder =>
        {
            sourceBuilder.Append('(');
            ParameterConfiguration.WriteParametersTo(_parameters, sourceBuilder, MultilineParameters);
            sourceBuilder.Append(')');

            if (CalledConstructor is not null)
            {
                sourceBuilder.AppendLine();
                CalledConstructor.WriteTo(sourceBuilder);
            }
        });
    }

    protected override void WriteNameTo(ISourceBuilder sourceBuilder)
    {
        if (ContainingTypeName is not null)
            base.WriteNameTo(sourceBuilder);
        else
            sourceBuilder.Append(sourceBuilder.CurrentTypeName);
    }
}
