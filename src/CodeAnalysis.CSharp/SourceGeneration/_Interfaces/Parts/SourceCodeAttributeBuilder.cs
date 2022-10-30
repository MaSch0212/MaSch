namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

/// <summary>
/// Represents a C# code attribute.
/// </summary>
public class SourceCodeAttributeBuilder : ISourceCodeAttributeBuilder
{
    private readonly string _typeName;
    private readonly List<string> _parameters = new();
    private CodeAttributeTarget _target;

    /// <summary>
    /// Initializes a new instance of the <see cref="SourceCodeAttributeBuilder"/> class.
    /// </summary>
    /// <param name="typeName">The type name of the code attribute.</param>
    public SourceCodeAttributeBuilder(string typeName)
    {
        _typeName = typeName;
    }

    /// <inheritdoc/>
    public void WriteTo(ISourceBuilder sourceBuilder)
    {
        sourceBuilder.Append('[');

        if (_target is not CodeAttributeTarget.Default)
            sourceBuilder.Append(GetAttributePrefix(_target));

        sourceBuilder.Append(_typeName);

        if (_parameters.Count > 0)
        {
            sourceBuilder.Append('(');
            bool isFirst = true;
            foreach (var p in _parameters)
            {
                if (!isFirst)
                    sourceBuilder.Append(", ");
                sourceBuilder.Append(p);
                isFirst = false;
            }

            sourceBuilder.Append(')');
        }

        sourceBuilder.Append(']');
    }

    /// <inheritdoc/>
    public ISourceCodeAttributeBuilder OnTarget(CodeAttributeTarget target)
    {
        _target = target;
        return this;
    }

    /// <inheritdoc/>
    public ISourceCodeAttributeBuilder WithParameter(string value)
    {
        _parameters.Add(value);
        return this;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        var builder = SourceBuilder.Create(capacity: 128, autoAddFileHeader: false);
        WriteTo(builder);
        return builder.ToString();
    }

    private static string GetAttributePrefix(CodeAttributeTarget target)
    {
        return target switch
        {
            CodeAttributeTarget.Assembly => "assembly: ",
            CodeAttributeTarget.Module => "module: ",
            CodeAttributeTarget.Field => "field: ",
            CodeAttributeTarget.Event => "event: ",
            CodeAttributeTarget.Method => "method: ",
            CodeAttributeTarget.Parameter => "param: ",
            CodeAttributeTarget.Property => "property: ",
            CodeAttributeTarget.Return => "return: ",
            CodeAttributeTarget.Type => "type: ",
            _ => string.Empty,
        };
    }
}
