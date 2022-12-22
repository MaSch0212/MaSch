namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

/// <summary>
/// Represents configuration of a method code element. This is used to generate code in the <see cref="ISourceBuilder"/>.
/// </summary>
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Interface")]
public interface IMethodConfiguration : IGenericMemberConfiguration<IMethodConfiguration>, IDefinesParametersConfiguration<IMethodConfiguration>
{
    /// <summary>
    /// Gets the body type of the method represented by this <see cref="IMethodConfiguration"/>.
    /// </summary>
    MethodBodyType BodyType { get; }

    /// <summary>
    /// Gets the return type of the method represented by this <see cref="IMethodConfiguration"/>.
    /// </summary>
    string ReturnTypeName { get; }

    /// <summary>
    /// Sets the return type of the method represented by this <see cref="IMethodConfiguration"/>.
    /// </summary>
    /// <param name="typeName">The return type to use.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    IMethodConfiguration WithReturnType(string typeName);

    /// <summary>
    /// Sets the body type of the method represented by this <see cref="IMethodConfiguration"/> to <see cref="MethodBodyType.Expression"/>/<see cref="MethodBodyType.ExpressionNewLine"/>.
    /// </summary>
    /// <param name="placeInNewLine">Determines whether to use <see cref="MethodBodyType.Expression"/> or <see cref="MethodBodyType.ExpressionNewLine"/>.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <remarks>
    /// If <paramref name="placeInNewLine"/> is set to <c>true</c> <see cref="MethodBodyType.ExpressionNewLine"/> is used; otherwise <see cref="MethodBodyType.Expression"/>.
    /// </remarks>
    IMethodConfiguration AsExpression(bool placeInNewLine = true);

    /// <summary>
    /// Sets <see cref="IDefinesParametersConfiguration.MultilineParameters"/> to <c>true</c>.
    /// </summary>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    IMethodConfiguration WithMultilineParameters();
}

internal sealed class MethodConfiguration : GenericMemberConfiguration<IMethodConfiguration>, IMethodConfiguration
{
    private readonly List<IParameterConfiguration> _parameters = new();

    public MethodConfiguration(string memberName)
        : base(memberName)
    {
    }

    public MethodBodyType BodyType { get; private set; } = MethodBodyType.Block;
    public string ReturnTypeName { get; private set; } = "void";
    public bool MultilineParameters { get; private set; }
    public IReadOnlyList<IParameterConfiguration> Parameters => new ReadOnlyCollection<IParameterConfiguration>(_parameters);

    protected override int StartCapacity => 128;
    protected override IMethodConfiguration This => this;

    public IMethodConfiguration WithParameter(string type, string name)
        => WithParameter(type, name, null);

    public IMethodConfiguration WithParameter(string type, string name, Action<IParameterConfiguration>? parameterConfiguration)
    {
        ParameterConfiguration.AddParameter(_parameters, type, name, parameterConfiguration);
        return This;
    }

    public IMethodConfiguration WithReturnType(string typeName)
    {
        ReturnTypeName = typeName;
        return This;
    }

    public IMethodConfiguration AsExpression(bool placeInNewLine = true)
    {
        BodyType = placeInNewLine ? MethodBodyType.ExpressionNewLine : MethodBodyType.Expression;
        return This;
    }

    public IMethodConfiguration WithMultilineParameters()
    {
        MultilineParameters = true;
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
        WriteReturnTypeTo(sourceBuilder);
        WriteNameTo(sourceBuilder);

        sourceBuilder.Indent(sourceBuilder =>
        {
            WriteParametersTo(sourceBuilder);
            WriteGenericConstraintsTo(sourceBuilder);
        });
    }

    private void WriteReturnTypeTo(ISourceBuilder sourceBuilder)
    {
        sourceBuilder.Append(ReturnTypeName).Append(' ');
    }

    private void WriteParametersTo(ISourceBuilder sourceBuilder)
    {
        sourceBuilder.Append('(');
        ParameterConfiguration.WriteParametersTo(_parameters, sourceBuilder, MultilineParameters);
        sourceBuilder.Append(')');
    }
}