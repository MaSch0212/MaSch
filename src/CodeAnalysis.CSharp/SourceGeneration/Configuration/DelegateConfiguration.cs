namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

/// <summary>
/// Represents configuration of a delegate code element. This is used to generate code in the <see cref="ISourceBuilder"/>.
/// </summary>
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Interface")]
public interface IDelegateConfiguration : IGenericMemberConfiguration<IDelegateConfiguration>, IDefinesParametersConfiguration<IDelegateConfiguration>
{
    /// <summary>
    /// Gets the body type of the delegate represented by this <see cref="IDelegateConfiguration"/>.
    /// </summary>
    MethodBodyType BodyType { get; }

    /// <summary>
    /// Gets the return type of the delegate represented by this <see cref="IDelegateConfiguration"/>.
    /// </summary>
    string ReturnTypeName { get; }

    /// <summary>
    /// Sets the return type of the delegate represented by this <see cref="IDelegateConfiguration"/>.
    /// </summary>
    /// <param name="typeName">The return type to use.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    IDelegateConfiguration WithReturnType(string typeName);

    /// <summary>
    /// Sets <see cref="IDefinesParametersConfiguration.MultilineParameters"/> to <c>true</c>.
    /// </summary>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    IDelegateConfiguration WithMultilineParameters();
}

internal sealed class DelegateConfiguration : GenericMemberConfiguration<IDelegateConfiguration>, IDelegateConfiguration
{
    private readonly List<IParameterConfiguration> _parameters = new();

    public DelegateConfiguration(string delegateName)
        : base(delegateName)
    {
    }

    public MethodBodyType BodyType { get; private set; } = MethodBodyType.Block;
    public string ReturnTypeName { get; private set; } = "void";
    public bool MultilineParameters { get; private set; }
    public IReadOnlyList<IParameterConfiguration> Parameters => new ReadOnlyCollection<IParameterConfiguration>(_parameters);

    protected override int StartCapacity => 128;
    protected override IDelegateConfiguration This => this;

    public IDelegateConfiguration WithReturnType(string typeName)
    {
        ReturnTypeName = typeName;
        return this;
    }

    public IDelegateConfiguration WithMultilineParameters()
    {
        MultilineParameters = true;
        return this;
    }

    public IDelegateConfiguration WithParameter(string type, string name)
        => WithParameter(type, name, null);

    public IDelegateConfiguration WithParameter(string type, string name, Action<IParameterConfiguration>? parameterConfiguration)
    {
        ParameterConfiguration.AddParameter(_parameters, type, name, parameterConfiguration);
        return this;
    }

    IDefinesParametersConfiguration IDefinesParametersConfiguration.WithParameter(string type, string name)
        => WithParameter(type, name);

    IDefinesParametersConfiguration IDefinesParametersConfiguration.WithParameter(string type, string name, Action<IParameterConfiguration> parameterConfiguration)
        => WithParameter(type, name, parameterConfiguration);

    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        WriteCommentsTo(sourceBuilder);
        WriteCodeAttributesTo(sourceBuilder);
        WriteKeywordsTo(sourceBuilder);
        sourceBuilder.Append("delegate ");
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
