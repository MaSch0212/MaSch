using MaSch.CodeAnalysis.CSharp.Extensions;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

/// <summary>
/// Represents configuration of a parameter code element. This is used to generate code in the <see cref="ISourceBuilder"/>.
/// </summary>
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Interface")]
public interface IParameterConfiguration : ISupportsCodeAttributeConfiguration<IParameterConfiguration>
{
    /// <summary>
    /// Gets the type of the parameter represented by this <see cref="IParameterConfiguration"/>.
    /// </summary>
    string Type { get; }

    /// <summary>
    /// Gets the name of the parameter represented by this <see cref="IParameterConfiguration"/>.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the default value of the parameter represented by this <see cref="IParameterConfiguration"/>.
    /// </summary>
    string? DefaultValue { get; }

    /// <summary>
    /// Sets the default value of the parameter represented by this <see cref="IParameterConfiguration"/>.
    /// </summary>
    /// <param name="defaultValue">The default value to use.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <remarks>The <paramref name="defaultValue"/> is interpreted as C# code. If you want to add a string value as a parameter call the <see cref="StringExtensions.ToCSharpLiteral(string?, bool)"/> extension method on the string.</remarks>
    IParameterConfiguration WithDefaultValue(string defaultValue);

    /// <summary>
    /// Writes the code represented by this <see cref="ICodeConfiguration"/> to the target <see cref="ISourceBuilder"/>.
    /// </summary>
    /// <param name="sourceBuilder">The <see cref="ISourceBuilder"/> to write the code to.</param>
    /// <param name="lineBreakAfterCodeAttribute">Determines whether a line break should be appended after each code attribute of the parameter.</param>
    void WriteTo(ISourceBuilder sourceBuilder, bool lineBreakAfterCodeAttribute);
}

internal sealed class ParameterConfiguration : CodeConfigurationBase, IParameterConfiguration
{
    private readonly List<ICodeAttributeConfiguration> _codeAttributes = new();

    public ParameterConfiguration(string type, string name)
    {
        Type = type;
        Name = name;
    }

    public string Type { get; }
    public string Name { get; }
    public string? DefaultValue { get; private set; }
    public IReadOnlyList<ICodeAttributeConfiguration> Attributes => new ReadOnlyCollection<ICodeAttributeConfiguration>(_codeAttributes);

    protected override int StartCapacity => 16;

    public static ParameterConfiguration AddParameter(IList<IParameterConfiguration> parameters, string type, string name, Action<IParameterConfiguration>? parameterConfiguration)
    {
        var config = new ParameterConfiguration(type, name);
        parameterConfiguration?.Invoke(config);
        parameters.Add(config);
        return config;
    }

    public static void WriteParametersTo(IList<IParameterConfiguration> parameters, ISourceBuilder sourceBuilder, bool multiline)
    {
        for (int i = 0; i < parameters.Count; i++)
        {
            if (multiline)
                sourceBuilder.AppendLine();
            else if (i > 0)
                sourceBuilder.Append(' ');

            parameters[i].WriteTo(sourceBuilder, multiline);
            if (i < parameters.Count - 1)
                sourceBuilder.Append(',');
        }
    }

    public IParameterConfiguration WithCodeAttribute(string attributeTypeName)
        => WithCodeAttribute(attributeTypeName, null);

    public IParameterConfiguration WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration>? attributeConfiguration)
    {
        CodeAttributeConfiguration.AddCodeAttribute(_codeAttributes, attributeTypeName, attributeConfiguration);
        return this;
    }

    public IParameterConfiguration WithDefaultValue(string defaultValue)
    {
        DefaultValue = defaultValue;
        return this;
    }

    ISupportsCodeAttributeConfiguration ISupportsCodeAttributeConfiguration.WithCodeAttribute(string attributeTypeName)
        => WithCodeAttribute(attributeTypeName, null);

    ISupportsCodeAttributeConfiguration ISupportsCodeAttributeConfiguration.WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration> attributeConfiguration)
        => WithCodeAttribute(attributeTypeName, attributeConfiguration);

    public override void WriteTo(ISourceBuilder sourceBuilder)
        => WriteTo(sourceBuilder, false);

    public void WriteTo(ISourceBuilder sourceBuilder, bool lineBreakAfterCodeAttribute)
    {
        foreach (var codeAttribute in _codeAttributes)
        {
            codeAttribute.WriteTo(sourceBuilder);
            if (lineBreakAfterCodeAttribute)
                sourceBuilder.AppendLine();
            else
                sourceBuilder.Append(' ');
        }

        sourceBuilder.Append(Type).Append(' ').Append(Name);

        if (DefaultValue != null)
            sourceBuilder.Append(" = ").Append(DefaultValue);
    }
}
