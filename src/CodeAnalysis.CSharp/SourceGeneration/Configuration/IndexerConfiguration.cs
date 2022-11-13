namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IIndexerConfigurationBase : IPropertyConfigurationBase, IDefinesParametersConfiguration
{
}

public interface IIndexerConfigurationBase<T> : IIndexerConfigurationBase, IPropertyConfigurationBase<T>, IDefinesParametersConfiguration<T>
    where T : IIndexerConfigurationBase<T>
{
}

public interface IReadOnlyIndexerConfiguration : IReadOnlyPropertyConfigurationBase<IReadOnlyIndexerConfiguration>, IIndexerConfigurationBase<IReadOnlyIndexerConfiguration>
{
}

public interface IWriteOnlyIndexerConfiguration : IWriteOnlyPropertyConfigurationBase<IWriteOnlyIndexerConfiguration>, IIndexerConfigurationBase<IWriteOnlyIndexerConfiguration>
{
}

public interface IIndexerConfiguration : IFullPropertyConfigurationBase<IIndexerConfiguration, IReadOnlyIndexerConfiguration, IWriteOnlyIndexerConfiguration>, IIndexerConfigurationBase<IIndexerConfiguration>
{
}

internal sealed class IndexerConfiguration :
    PropertyConfiguration<IIndexerConfiguration, IReadOnlyIndexerConfiguration, IWriteOnlyIndexerConfiguration>,
    IIndexerConfiguration,
    IReadOnlyIndexerConfiguration,
    IWriteOnlyIndexerConfiguration
{
    private readonly List<IParameterConfiguration> _parameters = new();

    public IndexerConfiguration(string propertyType)
        : base(propertyType, "this")
    {
    }

    public bool MultilineParameters { get; set; }

    protected override IIndexerConfiguration This => this;
    protected override int StartCapacity => 48;

    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        WriteCodeAttributesTo(sourceBuilder);
        WriteKeywordsTo(sourceBuilder);
        WriteNameTo(sourceBuilder);
        sourceBuilder.Append('[');
        ParameterConfiguration.WriteParametersTo(_parameters, sourceBuilder, false);
        sourceBuilder.Append(']');
    }

    protected override IReadOnlyIndexerConfiguration AsReadOnly() => this;

    protected override IWriteOnlyIndexerConfiguration AsWriteOnly() => this;

    IReadOnlyIndexerConfiguration IReadOnlyPropertyConfigurationBase<IReadOnlyIndexerConfiguration>.AsExpression(bool placeInNewLine)
    {
        AsExpression(placeInNewLine);
        return this;
    }

    IReadOnlyPropertyConfigurationBase IReadOnlyPropertyConfigurationBase.AsExpression(bool placeInNewLine)
    {
        AsExpression(placeInNewLine);
        return this;
    }

    IWriteOnlyIndexerConfiguration IWriteOnlyPropertyConfigurationBase<IWriteOnlyIndexerConfiguration>.AsExpression()
    {
        AsExpression(false);
        return this;
    }

    IWriteOnlyPropertyConfigurationBase IWriteOnlyPropertyConfigurationBase.AsExpression()
    {
        AsExpression(false);
        return this;
    }

    IWriteOnlyIndexerConfiguration IPropertyHasSetConfiguration<IWriteOnlyIndexerConfiguration>.AsInitOnly()
    {
        AsInitOnly();
        return this;
    }

    IReadOnlyIndexerConfiguration IPropertyHasGetConfiguration<IReadOnlyIndexerConfiguration>.ConfigureGet(Action<IPropertyMethodConfiguration> configurationFunc)
    {
        ConfigureGet(configurationFunc);
        return this;
    }

    IWriteOnlyIndexerConfiguration IPropertyHasSetConfiguration<IWriteOnlyIndexerConfiguration>.ConfigureSet(Action<IPropertyMethodConfiguration> configurationFunc)
    {
        ConfigureSet(configurationFunc);
        return this;
    }

    IReadOnlyIndexerConfiguration ISupportsAccessModifierConfiguration<IReadOnlyIndexerConfiguration>.WithAccessModifier(AccessModifier accessModifier)
    {
        WithAccessModifier(accessModifier);
        return this;
    }

    IWriteOnlyIndexerConfiguration ISupportsAccessModifierConfiguration<IWriteOnlyIndexerConfiguration>.WithAccessModifier(AccessModifier accessModifier)
    {
        WithAccessModifier(accessModifier);
        return this;
    }

    IReadOnlyIndexerConfiguration ISupportsCodeAttributeConfiguration<IReadOnlyIndexerConfiguration>.WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration> attributeConfiguration)
    {
        WithCodeAttribute(attributeTypeName, attributeConfiguration);
        return this;
    }

    IWriteOnlyIndexerConfiguration ISupportsCodeAttributeConfiguration<IWriteOnlyIndexerConfiguration>.WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration> attributeConfiguration)
    {
        WithCodeAttribute(attributeTypeName, attributeConfiguration);
        return this;
    }

    IReadOnlyIndexerConfiguration ISupportsCodeAttributeConfiguration<IReadOnlyIndexerConfiguration>.WithCodeAttribute(string attributeTypeName)
    {
        WithCodeAttribute(attributeTypeName);
        return this;
    }

    IWriteOnlyIndexerConfiguration ISupportsCodeAttributeConfiguration<IWriteOnlyIndexerConfiguration>.WithCodeAttribute(string attributeTypeName)
    {
        WithCodeAttribute(attributeTypeName);
        return this;
    }

    IReadOnlyIndexerConfiguration IMemberConfiguration<IReadOnlyIndexerConfiguration>.WithKeyword(MemberKeyword keyword)
    {
        WithKeyword(keyword);
        return this;
    }

    IWriteOnlyIndexerConfiguration IMemberConfiguration<IWriteOnlyIndexerConfiguration>.WithKeyword(MemberKeyword keyword)
    {
        WithKeyword(keyword);
        return this;
    }

    IReadOnlyIndexerConfiguration IPropertyHasGetConfiguration<IReadOnlyIndexerConfiguration>.WithValue(string value)
    {
        WithValue(value);
        return this;
    }

    IIndexerConfiguration IDefinesParametersConfiguration<IIndexerConfiguration>.WithParameter(string type, string name)
        => WithParameter(type, name, null);

    IIndexerConfiguration IDefinesParametersConfiguration<IIndexerConfiguration>.WithParameter(string type, string name, Action<IParameterConfiguration> parameterConfiguration)
        => WithParameter(type, name, parameterConfiguration);

    IDefinesParametersConfiguration IDefinesParametersConfiguration.WithParameter(string type, string name)
        => WithParameter(type, name, null);

    IDefinesParametersConfiguration IDefinesParametersConfiguration.WithParameter(string type, string name, Action<IParameterConfiguration> parameterConfiguration)
        => WithParameter(type, name, parameterConfiguration);

    IReadOnlyIndexerConfiguration IDefinesParametersConfiguration<IReadOnlyIndexerConfiguration>.WithParameter(string type, string name)
        => WithParameter(type, name, null);

    IReadOnlyIndexerConfiguration IDefinesParametersConfiguration<IReadOnlyIndexerConfiguration>.WithParameter(string type, string name, Action<IParameterConfiguration> parameterConfiguration)
        => WithParameter(type, name, parameterConfiguration);

    IWriteOnlyIndexerConfiguration IDefinesParametersConfiguration<IWriteOnlyIndexerConfiguration>.WithParameter(string type, string name)
        => WithParameter(type, name, null);

    IWriteOnlyIndexerConfiguration IDefinesParametersConfiguration<IWriteOnlyIndexerConfiguration>.WithParameter(string type, string name, Action<IParameterConfiguration> parameterConfiguration)
        => WithParameter(type, name, parameterConfiguration);

    private IndexerConfiguration WithParameter(string type, string name, Action<IParameterConfiguration>? parameterConfiguration)
    {
        ParameterConfiguration.AddParameter(_parameters, type, name, parameterConfiguration);
        return this;
    }
}
