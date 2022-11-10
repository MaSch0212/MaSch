namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IPropertyConfigurationBase : IMemberConfiguration
{
}

public interface IPropertyConfigurationBase<T> : IPropertyConfigurationBase, IMemberConfiguration<T>
    where T : IPropertyConfigurationBase<T>
{
}

public interface IPropertyHasGetConfiguration : IPropertyConfigurationBase
{
    IPropertyMethodConfiguration GetMethod { get; }

    IPropertyHasGetConfiguration ConfigureGet(Action<IPropertyMethodConfiguration> configurationFunc);
}

public interface IPropertyHasGetConfiguration<T> : IPropertyHasGetConfiguration, IPropertyConfigurationBase<T>
    where T : IPropertyHasGetConfiguration<T>
{
    new T ConfigureGet(Action<IPropertyMethodConfiguration> configurationFunc);
}

public interface IPropertyHasSetConfiguration : IPropertyConfigurationBase
{
    IPropertyMethodConfiguration SetMethod { get; }

    IPropertyHasSetConfiguration ConfigureSet(Action<IPropertyMethodConfiguration> configurationFunc);
    IPropertyHasSetConfiguration AsInitOnly();
}

public interface IPropertyHasSetConfiguration<T> : IPropertyHasSetConfiguration, IPropertyConfigurationBase<T>
    where T : IPropertyHasSetConfiguration<T>
{
    new T ConfigureSet(Action<IPropertyMethodConfiguration> configurationFunc);
    new T AsInitOnly();
}

public interface IReadOnlyPropertyConfigurationBase : IPropertyHasGetConfiguration
{
    PropertyGetMethodType GetBodyType { get; }

    IReadOnlyPropertyConfigurationBase AsExpression(bool placeInNewLine = true);
    IReadOnlyPropertyConfigurationBase AsInitialize();
}

public interface IReadOnlyPropertyConfigurationBase<T> : IReadOnlyPropertyConfigurationBase, IPropertyHasGetConfiguration<T>
    where T : IReadOnlyPropertyConfigurationBase<T>
{
    new T AsExpression(bool placeInNewLine = true);
    new T AsInitialize();
}

public interface IWriteOnlyPropertyConfigurationBase : IPropertyHasSetConfiguration
{
}

public interface IWriteOnlyPropertyConfigurationBase<T> : IWriteOnlyPropertyConfigurationBase, IPropertyHasSetConfiguration<T>
    where T : IWriteOnlyPropertyConfigurationBase<T>
{
}

public interface IFullPropertyConfigurationBase : IPropertyHasGetConfiguration, IPropertyHasSetConfiguration
{
    IReadOnlyPropertyConfigurationBase AsReadOnly();
    IWriteOnlyPropertyConfigurationBase AsWriteOnly();
}

public interface IFullPropertyConfigurationBase<TConfig, TReadOnly, TWriteOnly> : IFullPropertyConfigurationBase, IPropertyHasGetConfiguration<TConfig>, IPropertyHasSetConfiguration<TConfig>
    where TConfig : IFullPropertyConfigurationBase<TConfig, TReadOnly, TWriteOnly>
    where TReadOnly : IReadOnlyPropertyConfigurationBase<TReadOnly>
    where TWriteOnly : IWriteOnlyPropertyConfigurationBase<TWriteOnly>
{
    new TReadOnly AsReadOnly();
    new TWriteOnly AsWriteOnly();
}

internal abstract class PropertyConfiguration<TConfig, TReadOnly, TWriteOnly> : MemberConfiguration<TConfig>, IFullPropertyConfigurationBase<TConfig, TReadOnly, TWriteOnly>
    where TConfig : IFullPropertyConfigurationBase<TConfig, TReadOnly, TWriteOnly>
    where TReadOnly : IReadOnlyPropertyConfigurationBase<TReadOnly>
    where TWriteOnly : IWriteOnlyPropertyConfigurationBase<TWriteOnly>
{
    private readonly string _propertyType;
    private readonly PropertyMethodConfiguration _getMethodConfiguration;
    private readonly PropertyMethodConfiguration _setMethodConfiguration;

    protected PropertyConfiguration(string propertyType, string propertyName)
        : base(propertyName)
    {
        _propertyType = propertyType;
        _getMethodConfiguration = new PropertyMethodConfiguration("get");
        _setMethodConfiguration = new PropertyMethodConfiguration("set");
    }

    public IPropertyMethodConfiguration GetMethod => _getMethodConfiguration;
    public IPropertyMethodConfiguration SetMethod => _setMethodConfiguration;

    public PropertyGetMethodType GetBodyType { get; private set; } = PropertyGetMethodType.Block;

    TReadOnly IFullPropertyConfigurationBase<TConfig, TReadOnly, TWriteOnly>.AsReadOnly()
        => AsReadOnly();

    TWriteOnly IFullPropertyConfigurationBase<TConfig, TReadOnly, TWriteOnly>.AsWriteOnly()
        => AsWriteOnly();

    IReadOnlyPropertyConfigurationBase IFullPropertyConfigurationBase.AsReadOnly()
        => AsReadOnly();

    IWriteOnlyPropertyConfigurationBase IFullPropertyConfigurationBase.AsWriteOnly()
        => AsWriteOnly();

    TConfig IPropertyHasGetConfiguration<TConfig>.ConfigureGet(Action<IPropertyMethodConfiguration> configurationFunc)
        => ConfigureGet(configurationFunc);

    IPropertyHasGetConfiguration IPropertyHasGetConfiguration.ConfigureGet(Action<IPropertyMethodConfiguration> configurationFunc)
        => ConfigureGet(configurationFunc);

    TConfig IPropertyHasSetConfiguration<TConfig>.ConfigureSet(Action<IPropertyMethodConfiguration> configurationFunc)
        => ConfigureSet(configurationFunc);

    TConfig IPropertyHasSetConfiguration<TConfig>.AsInitOnly()
        => AsInitOnly();

    IPropertyHasSetConfiguration IPropertyHasSetConfiguration.ConfigureSet(Action<IPropertyMethodConfiguration> configurationFunc)
        => ConfigureSet(configurationFunc);

    IPropertyHasSetConfiguration IPropertyHasSetConfiguration.AsInitOnly()
        => AsInitOnly();

    protected override void WriteKeywordsTo(ISourceBuilder sourceBuilder)
    {
        base.WriteKeywordsTo(sourceBuilder);
        sourceBuilder.Append(_propertyType).Append(' ');
    }

    protected TConfig ConfigureGet(Action<IPropertyMethodConfiguration> configurationFunc)
    {
        configurationFunc(_getMethodConfiguration);
        return This;
    }

    protected TConfig ConfigureSet(Action<IPropertyMethodConfiguration> configurationFunc)
    {
        configurationFunc(_setMethodConfiguration);
        return This;
    }

    protected TConfig AsInitOnly()
    {
        _setMethodConfiguration.MethodKeyword = "init";
        return This;
    }

    protected TConfig AsExpression(bool placeInNewLine = true)
    {
        GetBodyType = placeInNewLine ? PropertyGetMethodType.ExpressionNewLine : PropertyGetMethodType.Expression;
        return This;
    }

    protected TConfig AsInitialize()
    {
        GetBodyType = PropertyGetMethodType.Initialize;
        return This;
    }

    protected abstract TReadOnly AsReadOnly();

    protected abstract TWriteOnly AsWriteOnly();
}

public interface IReadOnlyPropertyConfiguration : IReadOnlyPropertyConfigurationBase<IReadOnlyPropertyConfiguration>
{
}

public interface IWriteOnlyPropertyConfiguration : IWriteOnlyPropertyConfigurationBase<IWriteOnlyPropertyConfiguration>
{
}

public interface IPropertyConfiguration : IFullPropertyConfigurationBase<IPropertyConfiguration, IReadOnlyPropertyConfiguration, IWriteOnlyPropertyConfiguration>
{
}

internal sealed class PropertyConfiguration :
    PropertyConfiguration<IPropertyConfiguration, IReadOnlyPropertyConfiguration, IWriteOnlyPropertyConfiguration>,
    IReadOnlyPropertyConfiguration,
    IWriteOnlyPropertyConfiguration,
    IPropertyConfiguration
{
    public PropertyConfiguration(string propertyType, string propertyName)
        : base(propertyType, propertyName)
    {
    }

    public string MemberName { get; }
    public MethodBodyType BodyType { get; private set; } = MethodBodyType.Block;

    protected override IPropertyConfiguration This => this;

    protected override int StartCapacity => 32;

    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        WriteCodeAttributesTo(sourceBuilder);
        WriteKeywordsTo(sourceBuilder);
        WriteNameTo(sourceBuilder);
    }

    protected override IReadOnlyPropertyConfiguration AsReadOnly()
    {
        return this;
    }

    protected override IWriteOnlyPropertyConfiguration AsWriteOnly()
    {
        return this;
    }

    IReadOnlyPropertyConfiguration IReadOnlyPropertyConfigurationBase<IReadOnlyPropertyConfiguration>.AsExpression(bool placeInNewLine)
    {
        AsExpression(placeInNewLine);
        return this;
    }

    IReadOnlyPropertyConfigurationBase IReadOnlyPropertyConfigurationBase.AsExpression(bool placeInNewLine)
    {
        AsExpression(placeInNewLine);
        return this;
    }

    IReadOnlyPropertyConfiguration IReadOnlyPropertyConfigurationBase<IReadOnlyPropertyConfiguration>.AsInitialize()
    {
        AsInitialize();
        return this;
    }

    IReadOnlyPropertyConfigurationBase IReadOnlyPropertyConfigurationBase.AsInitialize()
    {
        AsInitialize();
        return this;
    }

    IWriteOnlyPropertyConfiguration IPropertyHasSetConfiguration<IWriteOnlyPropertyConfiguration>.AsInitOnly()
    {
        AsInitOnly();
        return this;
    }

    IReadOnlyPropertyConfiguration IPropertyHasGetConfiguration<IReadOnlyPropertyConfiguration>.ConfigureGet(Action<IPropertyMethodConfiguration> configurationFunc)
    {
        ConfigureGet(configurationFunc);
        return this;
    }

    IWriteOnlyPropertyConfiguration IPropertyHasSetConfiguration<IWriteOnlyPropertyConfiguration>.ConfigureSet(Action<IPropertyMethodConfiguration> configurationFunc)
    {
        ConfigureSet(configurationFunc);
        return this;
    }

    IWriteOnlyPropertyConfiguration ISupportsAccessModifierConfiguration<IWriteOnlyPropertyConfiguration>.WithAccessModifier(AccessModifier accessModifier)
    {
        WithAccessModifier(accessModifier);
        return this;
    }

    IReadOnlyPropertyConfiguration ISupportsAccessModifierConfiguration<IReadOnlyPropertyConfiguration>.WithAccessModifier(AccessModifier accessModifier)
    {
        WithAccessModifier(accessModifier);
        return this;
    }

    IWriteOnlyPropertyConfiguration ISupportsCodeAttributeConfiguration<IWriteOnlyPropertyConfiguration>.WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration> attributeConfiguration)
    {
        WithCodeAttribute(attributeTypeName, attributeConfiguration);
        return this;
    }

    IReadOnlyPropertyConfiguration ISupportsCodeAttributeConfiguration<IReadOnlyPropertyConfiguration>.WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration> attributeConfiguration)
    {
        WithCodeAttribute(attributeTypeName, attributeConfiguration);
        return this;
    }

    IWriteOnlyPropertyConfiguration IMemberConfiguration<IWriteOnlyPropertyConfiguration>.WithKeyword(MemberKeyword keyword)
    {
        WithKeyword(keyword);
        return this;
    }

    IReadOnlyPropertyConfiguration IMemberConfiguration<IReadOnlyPropertyConfiguration>.WithKeyword(MemberKeyword keyword)
    {
        WithKeyword(keyword);
        return this;
    }
}
