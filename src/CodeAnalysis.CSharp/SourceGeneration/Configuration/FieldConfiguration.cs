namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IFieldConfiguration : IMemberConfiguration<IFieldConfiguration>
{
    IFieldConfiguration WithValue(string value);
}

internal sealed class FieldConfiguration : MemberConfiguration<IFieldConfiguration>, IFieldConfiguration
{
    private readonly string _fieldTypeName;
    private string _value;

    public FieldConfiguration(string fieldTypeName, string fieldName)
        : base(fieldName)
    {
        _fieldTypeName = fieldTypeName;
    }

    /// <inheritdoc/>
    protected override IFieldConfiguration This => this;

    /// <inheritdoc/>
    protected override int StartCapacity => 32;

    /// <inheritdoc/>
    public IFieldConfiguration WithValue(string value)
    {
        _value = value;
        return This;
    }

    /// <inheritdoc/>
    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        WriteCommentsTo(sourceBuilder);
        WriteCodeAttributesTo(sourceBuilder);
        WriteKeywordsTo(sourceBuilder);
        sourceBuilder.Append($"{_fieldTypeName} ");
        WriteNameTo(sourceBuilder);

        if (_value is not null)
            sourceBuilder.Append($" = {_value}");
    }
}
