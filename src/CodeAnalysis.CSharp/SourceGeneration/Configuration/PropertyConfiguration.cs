using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration
{
    /// <summary>
    /// Represents configuration of a property code element. This is used to generate code in the <see cref="ISourceBuilder"/>.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Interface")]
    public interface IPropertyConfiguration : IMemberConfiguration
    {
        /// <summary>
        /// Gets the type of the property represented by this <see cref="IPropertyConfiguration"/>.
        /// </summary>
        string Type { get; }
    }

    /// <summary>
    /// Represents configuration of a property code element. This is used to generate code in the <see cref="ISourceBuilder"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IPropertyConfiguration{T}"/>.</typeparam>
    public interface IPropertyConfiguration<T> : IMemberConfiguration<T>, IPropertyConfiguration
        where T : IPropertyConfiguration<T>
    {
    }

    /// <summary>
    /// Represents configuration of a property code element that has a getter. This is used to generate code in the <see cref="ISourceBuilder"/>.
    /// </summary>
    public interface IPropertyWithGetConfiguration : IPropertyConfiguration
    {
        /// <summary>
        /// Gets the get method of the property represented by this <see cref="IPropertyWithGetConfiguration"/>.
        /// </summary>
        IPropertyMethodConfiguration GetMethod { get; }

        /// <summary>
        /// Gets the initial value of the property represented by this <see cref="IPropertyWithGetConfiguration"/>.
        /// </summary>
        string? Value { get; }

        /// <summary>
        /// Configures the get method of the property represented by this <see cref="IPropertyWithGetConfiguration"/>.
        /// </summary>
        /// <param name="configurationFunc">A function to configure the get method.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        IPropertyWithGetConfiguration ConfigureGet(Action<IPropertyMethodConfiguration> configurationFunc);

        /// <summary>
        /// Sets the initial value of the property represented by this <see cref="IPropertyWithGetConfiguration"/>.
        /// </summary>
        /// <param name="value">The value to use.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        IPropertyWithGetConfiguration WithValue(string value);
    }

    /// <summary>
    /// Represents configuration of a property code element that has a getter. This is used to generate code in the <see cref="ISourceBuilder"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IPropertyWithGetConfiguration{T}"/>.</typeparam>
    public interface IPropertyWithGetConfiguration<T> : IPropertyWithGetConfiguration, IPropertyConfiguration<T>
        where T : IPropertyWithGetConfiguration<T>
    {
        /// <inheritdoc cref="IPropertyWithGetConfiguration.ConfigureGet(Action{IPropertyMethodConfiguration})"/>
        new T ConfigureGet(Action<IPropertyMethodConfiguration> configurationFunc);

        /// <inheritdoc cref="IPropertyWithGetConfiguration.WithValue(string)"/>
        new T WithValue(string value);
    }

    /// <summary>
    /// Represents configuration of a property code element that has a setter. This is used to generate code in the <see cref="ISourceBuilder"/>.
    /// </summary>
    public interface IPropertyWithSetConfiguration : IPropertyConfiguration
    {
        /// <summary>
        /// Gets the set method of the property represented by this <see cref="IPropertyWithSetConfiguration"/>.
        /// </summary>
        IPropertyMethodConfiguration SetMethod { get; }

        /// <summary>
        /// Configures the set method of the property represented by this <see cref="IPropertyWithSetConfiguration"/>.
        /// </summary>
        /// <param name="configurationFunc">A function to configure the set method.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        IPropertyWithSetConfiguration ConfigureSet(Action<IPropertyMethodConfiguration> configurationFunc);

        /// <summary>
        /// Changes the set method keyword to <c>init</c> for the property represented by this <see cref="IPropertyWithSetConfiguration"/>.
        /// </summary>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        IPropertyWithSetConfiguration AsInit();
    }

    /// <summary>
    /// Represents configuration of a property code element that has a setter. This is used to generate code in the <see cref="ISourceBuilder"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IPropertyWithSetConfiguration{T}"/>.</typeparam>
    public interface IPropertyWithSetConfiguration<T> : IPropertyWithSetConfiguration, IPropertyConfiguration<T>
        where T : IPropertyWithSetConfiguration<T>
    {
        /// <inheritdoc cref="IPropertyWithSetConfiguration.ConfigureSet(Action{IPropertyMethodConfiguration})"/>
        new T ConfigureSet(Action<IPropertyMethodConfiguration> configurationFunc);

        /// <inheritdoc cref="IPropertyWithSetConfiguration.AsInit"/>
        new T AsInit();
    }

    /// <summary>
    /// Represents configuration of a read-only property code element. This is used to generate code in the <see cref="ISourceBuilder"/>.
    /// </summary>
    public interface IReadOnlyPropertyConfiguration : IPropertyWithGetConfiguration<IReadOnlyPropertyConfiguration>
    {
    }

    /// <summary>
    /// Represents configuration of a write-only property code element. This is used to generate code in the <see cref="ISourceBuilder"/>.
    /// </summary>
    public interface IWriteOnlyPropertyConfiguration : IPropertyWithSetConfiguration<IWriteOnlyPropertyConfiguration>
    {
    }

    /// <summary>
    /// Represents configuration of a property code element. This is used to generate code in the <see cref="ISourceBuilder"/>.
    /// </summary>
    public interface IReadWritePropertyConfiguration : IPropertyWithGetConfiguration<IReadWritePropertyConfiguration>, IPropertyWithSetConfiguration<IReadWritePropertyConfiguration>
    {
        /// <summary>
        /// Changes the property represented by this <see cref="IReadWritePropertyConfiguration"/> to a read-only property.
        /// </summary>
        /// <returns>An instance of <see cref="IReadOnlyPropertyConfiguration"/> representing this <see cref="IReadWritePropertyConfiguration"/> as a read-only property.</returns>
        IReadOnlyPropertyConfiguration AsReadOnly();

        /// <summary>
        /// Changes the property represented by this <see cref="IReadWritePropertyConfiguration"/> to a write-only property.
        /// </summary>
        /// <returns>An instance of <see cref="IWriteOnlyPropertyConfiguration"/> representing this <see cref="IReadWritePropertyConfiguration"/> as a write-only property.</returns>
        IWriteOnlyPropertyConfiguration AsWriteOnly();
    }

    internal sealed class PropertyConfiguration :
        MemberConfiguration<PropertyConfiguration>,
        IReadWritePropertyConfiguration,
        IReadOnlyPropertyConfiguration,
        IWriteOnlyPropertyConfiguration
    {
        private readonly PropertyMethodConfiguration _setMethod;
        private readonly PropertyMethodConfiguration _getMethod;

        public PropertyConfiguration(string propertyType, string propertyName)
            : base(propertyName)
        {
            Type = propertyType;
            _getMethod = new PropertyMethodConfiguration("get");
            _setMethod = new PropertyMethodConfiguration("set");
        }

        public string Type { get; }
        public string? Value { get; private set; }
        public IPropertyMethodConfiguration GetMethod => _getMethod;
        public IPropertyMethodConfiguration SetMethod => _setMethod;

        protected override PropertyConfiguration This => this;
        protected override int StartCapacity => 32;

        public PropertyConfiguration AsInit()
        {
            _setMethod.MethodKeyword = "init";
            return this;
        }

        public PropertyConfiguration ConfigureGet(Action<IPropertyMethodConfiguration> configurationFunc)
        {
            configurationFunc.Invoke(_getMethod);
            return this;
        }

        public PropertyConfiguration ConfigureSet(Action<IPropertyMethodConfiguration> configurationFunc)
        {
            configurationFunc.Invoke(_setMethod);
            return this;
        }

        public PropertyConfiguration WithValue(string value)
        {
            Value = value;
            return this;
        }

        IReadOnlyPropertyConfiguration IReadWritePropertyConfiguration.AsReadOnly()
            => this;

        IWriteOnlyPropertyConfiguration IReadWritePropertyConfiguration.AsWriteOnly()
            => this;

        IPropertyWithSetConfiguration IPropertyWithSetConfiguration.AsInit()
            => AsInit();

        IReadWritePropertyConfiguration IPropertyWithSetConfiguration<IReadWritePropertyConfiguration>.AsInit()
            => AsInit();

        IWriteOnlyPropertyConfiguration IPropertyWithSetConfiguration<IWriteOnlyPropertyConfiguration>.AsInit()
            => AsInit();

        IPropertyWithGetConfiguration IPropertyWithGetConfiguration.ConfigureGet(Action<IPropertyMethodConfiguration> configurationFunc)
            => ConfigureGet(configurationFunc);

        IReadWritePropertyConfiguration IPropertyWithGetConfiguration<IReadWritePropertyConfiguration>.ConfigureGet(Action<IPropertyMethodConfiguration> configurationFunc)
            => ConfigureGet(configurationFunc);

        IReadOnlyPropertyConfiguration IPropertyWithGetConfiguration<IReadOnlyPropertyConfiguration>.ConfigureGet(Action<IPropertyMethodConfiguration> configurationFunc)
            => ConfigureGet(configurationFunc);

        IPropertyWithSetConfiguration IPropertyWithSetConfiguration.ConfigureSet(Action<IPropertyMethodConfiguration> configurationFunc)
            => ConfigureSet(configurationFunc);

        IReadWritePropertyConfiguration IPropertyWithSetConfiguration<IReadWritePropertyConfiguration>.ConfigureSet(Action<IPropertyMethodConfiguration> configurationFunc)
            => ConfigureSet(configurationFunc);

        IWriteOnlyPropertyConfiguration IPropertyWithSetConfiguration<IWriteOnlyPropertyConfiguration>.ConfigureSet(Action<IPropertyMethodConfiguration> configurationFunc)
            => ConfigureSet(configurationFunc);

        IReadOnlyPropertyConfiguration ISupportsAccessModifierConfiguration<IReadOnlyPropertyConfiguration>.WithAccessModifier(AccessModifier accessModifier)
            => WithAccessModifier(accessModifier);

        IWriteOnlyPropertyConfiguration ISupportsAccessModifierConfiguration<IWriteOnlyPropertyConfiguration>.WithAccessModifier(AccessModifier accessModifier)
            => WithAccessModifier(accessModifier);

        IReadWritePropertyConfiguration ISupportsAccessModifierConfiguration<IReadWritePropertyConfiguration>.WithAccessModifier(AccessModifier accessModifier)
            => WithAccessModifier(accessModifier);

        IReadOnlyPropertyConfiguration ISupportsLineCommentsConfiguration<IReadOnlyPropertyConfiguration>.WithBlockComment(string comment)
            => WithBlockComment(comment);

        IWriteOnlyPropertyConfiguration ISupportsLineCommentsConfiguration<IWriteOnlyPropertyConfiguration>.WithBlockComment(string comment)
            => WithBlockComment(comment);

        IReadWritePropertyConfiguration ISupportsLineCommentsConfiguration<IReadWritePropertyConfiguration>.WithBlockComment(string comment)
            => WithBlockComment(comment);

        IReadOnlyPropertyConfiguration ISupportsCodeAttributeConfiguration<IReadOnlyPropertyConfiguration>.WithCodeAttribute(string attributeTypeName)
            => WithCodeAttribute(attributeTypeName);

        IReadOnlyPropertyConfiguration ISupportsCodeAttributeConfiguration<IReadOnlyPropertyConfiguration>.WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration> attributeConfiguration)
            => WithCodeAttribute(attributeTypeName, attributeConfiguration);

        IWriteOnlyPropertyConfiguration ISupportsCodeAttributeConfiguration<IWriteOnlyPropertyConfiguration>.WithCodeAttribute(string attributeTypeName)
            => WithCodeAttribute(attributeTypeName);

        IWriteOnlyPropertyConfiguration ISupportsCodeAttributeConfiguration<IWriteOnlyPropertyConfiguration>.WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration> attributeConfiguration)
            => WithCodeAttribute(attributeTypeName, attributeConfiguration);

        IReadWritePropertyConfiguration ISupportsCodeAttributeConfiguration<IReadWritePropertyConfiguration>.WithCodeAttribute(string attributeTypeName)
            => WithCodeAttribute(attributeTypeName);

        IReadWritePropertyConfiguration ISupportsCodeAttributeConfiguration<IReadWritePropertyConfiguration>.WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration> attributeConfiguration)
            => WithCodeAttribute(attributeTypeName, attributeConfiguration);

        IReadOnlyPropertyConfiguration ISupportsLineCommentsConfiguration<IReadOnlyPropertyConfiguration>.WithDocComment(string comment)
            => WithDocComment(comment);

        IWriteOnlyPropertyConfiguration ISupportsLineCommentsConfiguration<IWriteOnlyPropertyConfiguration>.WithDocComment(string comment)
            => WithDocComment(comment);

        IReadWritePropertyConfiguration ISupportsLineCommentsConfiguration<IReadWritePropertyConfiguration>.WithDocComment(string comment)
            => WithDocComment(comment);

        IReadOnlyPropertyConfiguration IMemberConfiguration<IReadOnlyPropertyConfiguration>.WithKeyword(MemberKeyword keyword)
            => WithKeyword(keyword);

        IWriteOnlyPropertyConfiguration IMemberConfiguration<IWriteOnlyPropertyConfiguration>.WithKeyword(MemberKeyword keyword)
            => WithKeyword(keyword);

        IReadWritePropertyConfiguration IMemberConfiguration<IReadWritePropertyConfiguration>.WithKeyword(MemberKeyword keyword)
            => WithKeyword(keyword);

        IReadOnlyPropertyConfiguration ISupportsLineCommentsConfiguration<IReadOnlyPropertyConfiguration>.WithLineComment(string comment)
            => WithLineComment(comment);

        IWriteOnlyPropertyConfiguration ISupportsLineCommentsConfiguration<IWriteOnlyPropertyConfiguration>.WithLineComment(string comment)
            => WithLineComment(comment);

        IReadWritePropertyConfiguration ISupportsLineCommentsConfiguration<IReadWritePropertyConfiguration>.WithLineComment(string comment)
            => WithLineComment(comment);

        IPropertyWithGetConfiguration IPropertyWithGetConfiguration.WithValue(string value)
            => WithValue(value);

        IReadWritePropertyConfiguration IPropertyWithGetConfiguration<IReadWritePropertyConfiguration>.WithValue(string value)
            => WithValue(value);

        IReadOnlyPropertyConfiguration IPropertyWithGetConfiguration<IReadOnlyPropertyConfiguration>.WithValue(string value)
            => WithValue(value);

        public override void WriteTo(ISourceBuilder sourceBuilder)
        {
            WriteCommentsTo(sourceBuilder);
            WriteCodeAttributesTo(sourceBuilder);
            WriteKeywordsTo(sourceBuilder);
            sourceBuilder.Append(Type).Append(' ');
            WriteNameTo(sourceBuilder);
        }
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    /// <summary>
    /// Provides extension methods for the <see cref="IReadOnlyPropertyConfiguration"/>, <see cref="IWriteOnlyPropertyConfiguration"/> and <see cref="IReadWritePropertyConfiguration"/> interfaces.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Extensions")]
    public static class PropertyConfigurationExtensions
    {
        /// <summary>
        /// Sets the body type of the read-only property represented by this <see cref="IReadOnlyPropertyConfiguration"/> to <see cref="MethodBodyType.Expression"/>/<see cref="MethodBodyType.ExpressionNewLine"/>.
        /// </summary>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <param name="placeInNewLine">Determines whether to use <see cref="MethodBodyType.Expression"/> or <see cref="MethodBodyType.ExpressionNewLine"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <remarks>
        /// If <paramref name="placeInNewLine"/> is set to <c>true</c> <see cref="MethodBodyType.ExpressionNewLine"/> is used; otherwise <see cref="MethodBodyType.Expression"/>.
        /// </remarks>
        public static IReadOnlyPropertyConfiguration AsExpression(this IReadOnlyPropertyConfiguration config, bool placeInNewLine = false)
        {
            config.GetMethod.AsExpression(placeInNewLine);
            return config;
        }

        /// <summary>
        /// Sets the body type of the write-only property represented by this <see cref="IWriteOnlyPropertyConfiguration"/> to <see cref="MethodBodyType.Expression"/>/<see cref="MethodBodyType.ExpressionNewLine"/>.
        /// </summary>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <param name="placeInNewLine">Determines whether to use <see cref="MethodBodyType.Expression"/> or <see cref="MethodBodyType.ExpressionNewLine"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <remarks>
        /// If <paramref name="placeInNewLine"/> is set to <c>true</c> <see cref="MethodBodyType.ExpressionNewLine"/> is used; otherwise <see cref="MethodBodyType.Expression"/>.
        /// </remarks>
        public static IWriteOnlyPropertyConfiguration AsExpression(this IWriteOnlyPropertyConfiguration config, bool placeInNewLine = false)
        {
            config.SetMethod.AsExpression(placeInNewLine);
            return config;
        }

        /// <summary>
        /// Sets the body type of the get- and set-methods of the property represented by this <see cref="IReadWritePropertyConfiguration"/> to <see cref="MethodBodyType.Expression"/>/<see cref="MethodBodyType.ExpressionNewLine"/>.
        /// </summary>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <param name="placeInNewLine">Determines whether to use <see cref="MethodBodyType.Expression"/> or <see cref="MethodBodyType.ExpressionNewLine"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <remarks>
        /// If <paramref name="placeInNewLine"/> is set to <c>true</c> <see cref="MethodBodyType.ExpressionNewLine"/> is used; otherwise <see cref="MethodBodyType.Expression"/>.
        /// </remarks>
        public static IReadWritePropertyConfiguration AsExpression(this IReadWritePropertyConfiguration config, bool placeInNewLine = false)
        {
            config.GetMethod.AsExpression(placeInNewLine);
            config.SetMethod.AsExpression(placeInNewLine);
            return config;
        }

        /// <summary>
        /// Changes the property represented by this <see cref="IReadWritePropertyConfiguration"/> to a init-only property.
        /// </summary>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <returns>An instance of <see cref="IWriteOnlyPropertyConfiguration"/> representing this <see cref="IReadWritePropertyConfiguration"/> as a init-only property.</returns>
        public static IWriteOnlyPropertyConfiguration AsInitOnly(this IReadWritePropertyConfiguration config)
            => config.AsWriteOnly().AsInit();
    }
}