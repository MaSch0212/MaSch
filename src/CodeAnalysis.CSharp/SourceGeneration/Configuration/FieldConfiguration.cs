namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

/// <summary>
/// Represents configuration of a field code element. This is used to generate code in the <see cref="ISourceBuilder"/>.
/// </summary>
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Interface")]
public interface IFieldConfiguration : IMemberConfiguration<IFieldConfiguration>
{
    /// <summary>
    /// Gets the type name of the field represented by this <see cref="IFieldConfiguration"/>.
    /// </summary>
    string TypeName { get; }

    /// <summary>
    /// Gets the initial value of the field represented by this <see cref="IFieldConfiguration"/>.
    /// </summary>
    string? Value { get; }

    /// <summary>
    /// Sets the initial value of the field represented by this <see cref="IFieldConfiguration"/>.
    /// </summary>
    /// <param name="value">The value to use.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    IFieldConfiguration WithValue(string value);
}

internal sealed class FieldConfiguration : MemberConfiguration<IFieldConfiguration>, IFieldConfiguration
{
    public FieldConfiguration(string fieldTypeName, string fieldName)
        : base(fieldName)
    {
        TypeName = fieldTypeName;
    }

    public string TypeName { get; }
    public string? Value { get; private set; }

    protected override IFieldConfiguration This => this;
    protected override int StartCapacity => 32;

    public IFieldConfiguration WithValue(string value)
    {
        Value = value;
        return This;
    }

    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        WriteCommentsTo(sourceBuilder);
        WriteCodeAttributesTo(sourceBuilder);
        WriteKeywordsTo(sourceBuilder);
        sourceBuilder.Append($"{TypeName} ");
        WriteNameTo(sourceBuilder);

        if (Value is not null)
            sourceBuilder.Append($" = {Value}");
    }
}
