namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IPropertyHasGetterConfiguration<T> : IMemberConfiguration<T>
    where T : IPropertyHasGetterConfiguration<T>
{
    T ConfigureGetter(Action<IPropertyMethodConfiguration> configurationFunc);
    void WriteGetterTo(ISourceBuilder sourceBuilder);
}

public interface IPropertyHasSetterConfiguration<T> : IMemberConfiguration<T>
    where T : IPropertyHasSetterConfiguration<T>
{
    T ConfigureSetter(Action<IPropertyMethodConfiguration> configurationFunc);
    void WriteSetterTo(ISourceBuilder sourceBuilder);
}

public interface IPropertyConfiguration<TConfig, TReadOnly, TWriteOnly> : IMemberConfiguration<TConfig>, IPropertyHasGetterConfiguration<TConfig>, IPropertyHasSetterConfiguration<TConfig>
    where TConfig : IPropertyConfiguration<TConfig, TReadOnly, TWriteOnly>
    where TReadOnly : IPropertyHasGetterConfiguration<TReadOnly>
    where TWriteOnly : IPropertyHasSetterConfiguration<TWriteOnly>
{
    TReadOnly AsReadOnly();
    TWriteOnly AsWriteOnly();
}

public interface IReadOnlyPropertyConfiguration : IPropertyHasGetterConfiguration<IReadOnlyPropertyConfiguration>
{
}

public interface IWriteOnlyPropertyConfiguration : IPropertyHasSetterConfiguration<IWriteOnlyPropertyConfiguration>
{
}

public interface IPropertyConfiguration : IPropertyConfiguration<IPropertyConfiguration, IReadOnlyPropertyConfiguration, IWriteOnlyPropertyConfiguration>
{
}

internal abstract class PropertyConfiguration<TConfig, TReadOnly, TWriteOnly> : MemberConfiguration<TConfig>, IPropertyConfiguration<TConfig, TReadOnly, TWriteOnly>
    where TConfig : IPropertyConfiguration<TConfig, TReadOnly, TWriteOnly>
    where TReadOnly : IPropertyHasGetterConfiguration<TReadOnly>
    where TWriteOnly : IPropertyHasSetterConfiguration<TWriteOnly>
{
    private readonly string _propertyType;
    private readonly IPropertyMethodConfiguration _getterConfiguration;
    private readonly IPropertyMethodConfiguration _setterConfiguration;

    protected PropertyConfiguration(string propertyType, string propertyName, string getterKeyword, string setterKeyword)
        : base(propertyName)
    {
        _propertyType = propertyType;
        _getterConfiguration = new PropertyMethodConfiguration(getterKeyword);
        _setterConfiguration = new PropertyMethodConfiguration(setterKeyword);
    }

    public TConfig ConfigureGetter(Action<IPropertyMethodConfiguration> configurationFunc)
    {
        configurationFunc(_getterConfiguration);
        return This;
    }

    public TConfig ConfigureSetter(Action<IPropertyMethodConfiguration> configurationFunc)
    {
        configurationFunc(_setterConfiguration);
        return This;
    }

    public abstract TReadOnly AsReadOnly();

    public abstract TWriteOnly AsWriteOnly();

    protected override void WriteKeywordsTo(ISourceBuilder sourceBuilder)
    {
        base.WriteKeywordsTo(sourceBuilder);
        sourceBuilder.Append(_propertyType).Append(' ');
    }

    public virtual void WriteGetterTo(ISourceBuilder sourceBuilder)
    {
        _getterConfiguration.WriteTo(sourceBuilder);
    }

    public virtual void WriteSetterTo(ISourceBuilder sourceBuilder)
    {
        _setterConfiguration.WriteTo(sourceBuilder);
    }
}

internal sealed class PropertyConfiguration :
    PropertyConfiguration<IPropertyConfiguration, IReadOnlyPropertyConfiguration, IWriteOnlyPropertyConfiguration>,
    IPropertyConfiguration,
    IReadOnlyPropertyConfiguration,
    IWriteOnlyPropertyConfiguration
{
    public PropertyConfiguration(string propertyType, string propertyName)
        : base(propertyType, propertyName, "get", "set")
    {
    }

    public string MemberName { get; }

    protected override IPropertyConfiguration This => this;

    protected override int StartCapacity => 32;

    public override IReadOnlyPropertyConfiguration AsReadOnly()
    {
        return this;
    }

    public override IWriteOnlyPropertyConfiguration AsWriteOnly()
    {
        return this;
    }

    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        WriteCodeAttributesTo(sourceBuilder);
        WriteKeywordsTo(sourceBuilder);
        WriteNameTo(sourceBuilder);
    }

    IReadOnlyPropertyConfiguration IPropertyHasGetterConfiguration<IReadOnlyPropertyConfiguration>.ConfigureGetter(Action<IPropertyMethodConfiguration> configurationFunc)
    {
        ConfigureGetter(configurationFunc);
        return this;
    }

    IWriteOnlyPropertyConfiguration IPropertyHasSetterConfiguration<IWriteOnlyPropertyConfiguration>.ConfigureSetter(Action<IPropertyMethodConfiguration> configurationFunc)
    {
        ConfigureSetter(configurationFunc);
        return this;
    }

    IReadOnlyPropertyConfiguration ISupportsAccessModifierConfiguration<IReadOnlyPropertyConfiguration>.WithAccessModifier(AccessModifier accessModifier)
    {
        WithAccessModifier(accessModifier);
        return this;
    }

    IWriteOnlyPropertyConfiguration ISupportsAccessModifierConfiguration<IWriteOnlyPropertyConfiguration>.WithAccessModifier(AccessModifier accessModifier)
    {
        WithAccessModifier(accessModifier);
        return this;
    }

    IReadOnlyPropertyConfiguration ISupportsCodeAttributeConfiguration<IReadOnlyPropertyConfiguration>.WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration> attributeConfiguration)
    {
        WithCodeAttribute(attributeTypeName, attributeConfiguration);
        return this;
    }

    IWriteOnlyPropertyConfiguration ISupportsCodeAttributeConfiguration<IWriteOnlyPropertyConfiguration>.WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration> attributeConfiguration)
    {
        WithCodeAttribute(attributeTypeName, attributeConfiguration);
        return this;
    }

    IReadOnlyPropertyConfiguration IMemberConfiguration<IReadOnlyPropertyConfiguration>.WithKeyword(MemberKeyword keyword)
    {
        WithKeyword(keyword);
        return this;
    }

    IWriteOnlyPropertyConfiguration IMemberConfiguration<IWriteOnlyPropertyConfiguration>.WithKeyword(MemberKeyword keyword)
    {
        WithKeyword(keyword);
        return this;
    }
}
