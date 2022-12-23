using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration
{
    /// <summary>
    /// Represents configuration of an indexer code element. This is used to generate code in the <see cref="ISourceBuilder"/>.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Interface")]
    public interface IIndexerConfiguration : IPropertyConfiguration, IDefinesParametersConfiguration
    {
    }

    /// <summary>
    /// Represents configuration of an indexer code element. This is used to generate code in the <see cref="ISourceBuilder"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IIndexerConfiguration{T}"/>.</typeparam>
    public interface IIndexerConfiguration<T> : IIndexerConfiguration, IPropertyConfiguration<T>, IDefinesParametersConfiguration<T>
        where T : IIndexerConfiguration<T>
    {
    }

    /// <summary>
    /// Represents configuration of an indexer code element that has a getter. This is used to generate code in the <see cref="ISourceBuilder"/>.
    /// </summary>
    public interface IIndexerWithGetConfiguration : IIndexerConfiguration
    {
        /// <summary>
        /// Gets the get method of the indexer represented by this <see cref="IIndexerWithGetConfiguration"/>.
        /// </summary>
        IPropertyMethodConfiguration GetMethod { get; }

        /// <summary>
        /// Configures the get method of the indexer represented by this <see cref="IIndexerWithGetConfiguration"/>.
        /// </summary>
        /// <param name="configurationFunc">A function to configure the get method.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        IIndexerWithGetConfiguration ConfigureGet(Action<IPropertyMethodConfiguration> configurationFunc);
    }

    /// <summary>
    /// Represents configuration of an indexer code element that has a getter. This is used to generate code in the <see cref="ISourceBuilder"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IIndexerWithGetConfiguration{T}"/>.</typeparam>
    public interface IIndexerWithGetConfiguration<T> : IIndexerWithGetConfiguration, IIndexerConfiguration<T>
        where T : IIndexerWithGetConfiguration<T>
    {
        /// <inheritdoc cref="IIndexerWithGetConfiguration.ConfigureGet(Action{IPropertyMethodConfiguration})"/>
        new T ConfigureGet(Action<IPropertyMethodConfiguration> configurationFunc);
    }

    /// <summary>
    /// Represents configuration of an indexer code element that has a setter. This is used to generate code in the <see cref="ISourceBuilder"/>.
    /// </summary>
    public interface IIndexerWithSetConfiguration : IIndexerConfiguration
    {
        /// <summary>
        /// Gets the set method of the indexer represented by this <see cref="IIndexerWithSetConfiguration"/>.
        /// </summary>
        IPropertyMethodConfiguration SetMethod { get; }

        /// <summary>
        /// Configures the set method of the indexer represented by this <see cref="IIndexerWithSetConfiguration"/>.
        /// </summary>
        /// <param name="configurationFunc">A function to configure the set method.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        IIndexerWithSetConfiguration ConfigureSet(Action<IPropertyMethodConfiguration> configurationFunc);

        /// <summary>
        /// Changes the set method keyword to <c>init</c> for the indexer represented by this <see cref="IIndexerWithSetConfiguration"/>.
        /// </summary>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        IIndexerWithSetConfiguration AsInit();
    }

    /// <summary>
    /// Represents configuration of an indexer code element that has a setter. This is used to generate code in the <see cref="ISourceBuilder"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IIndexerWithSetConfiguration{T}"/>.</typeparam>
    public interface IIndexerWithSetConfiguration<T> : IIndexerWithSetConfiguration, IIndexerConfiguration<T>
        where T : IIndexerWithSetConfiguration<T>
    {
        /// <inheritdoc cref="IIndexerWithSetConfiguration.ConfigureSet(Action{IPropertyMethodConfiguration})"/>
        new T ConfigureSet(Action<IPropertyMethodConfiguration> configurationFunc);

        /// <inheritdoc cref="IIndexerWithSetConfiguration.AsInit"/>
        new T AsInit();
    }

    /// <summary>
    /// Represents configuration of a read-only indexer code element. This is used to generate code in the <see cref="ISourceBuilder"/>.
    /// </summary>
    public interface IReadOnlyIndexerConfiguration : IIndexerWithGetConfiguration<IReadOnlyIndexerConfiguration>
    {
    }

    /// <summary>
    /// Represents configuration of a write-only indexer code element. This is used to generate code in the <see cref="ISourceBuilder"/>.
    /// </summary>
    public interface IWriteOnlyIndexerConfiguration : IIndexerWithSetConfiguration<IWriteOnlyIndexerConfiguration>
    {
    }

    /// <summary>
    /// Represents configuration of an indexer code element. This is used to generate code in the <see cref="ISourceBuilder"/>.
    /// </summary>
    public interface IReadWriteIndexerConfiguration : IIndexerWithGetConfiguration<IReadWriteIndexerConfiguration>, IIndexerWithSetConfiguration<IReadWriteIndexerConfiguration>
    {
        /// <summary>
        /// Changes the indexer represented by this <see cref="IReadWriteIndexerConfiguration"/> to a read-only indexer.
        /// </summary>
        /// <returns>An instance of <see cref="IReadOnlyIndexerConfiguration"/> representing this <see cref="IReadWriteIndexerConfiguration"/> as a read-only indexer.</returns>
        IReadOnlyIndexerConfiguration AsReadOnly();

        /// <summary>
        /// Changes the indexer represented by this <see cref="IReadWriteIndexerConfiguration"/> to a write-only indexer.
        /// </summary>
        /// <returns>An instance of <see cref="IWriteOnlyIndexerConfiguration"/> representing this <see cref="IReadWriteIndexerConfiguration"/> as a write-only indexer.</returns>
        IWriteOnlyIndexerConfiguration AsWriteOnly();
    }

    internal sealed class IndexerConfiguration :
        MemberConfiguration<IndexerConfiguration>,
        IReadWriteIndexerConfiguration,
        IReadOnlyIndexerConfiguration,
        IWriteOnlyIndexerConfiguration
    {
        private readonly PropertyMethodConfiguration _setMethod;
        private readonly PropertyMethodConfiguration _getMethod;
        private readonly List<IParameterConfiguration> _parameters = new();

        public IndexerConfiguration(string indexerType)
            : base("this")
        {
            Type = indexerType;
            _getMethod = new PropertyMethodConfiguration("get");
            _setMethod = new PropertyMethodConfiguration("set");
        }

        public string Type { get; }
        public string? Value { get; private set; }
        public IPropertyMethodConfiguration GetMethod => _getMethod;
        public IPropertyMethodConfiguration SetMethod => _setMethod;
        public IReadOnlyList<IParameterConfiguration> Parameters => new ReadOnlyCollection<IParameterConfiguration>(_parameters);
        public bool MultilineParameters { get; private set; }

        protected override IndexerConfiguration This => this;
        protected override int StartCapacity => 32;

        public IndexerConfiguration AsInit()
        {
            _setMethod.MethodKeyword = "init";
            return this;
        }

        public IndexerConfiguration ConfigureGet(Action<IPropertyMethodConfiguration> configurationFunc)
        {
            configurationFunc.Invoke(_getMethod);
            return this;
        }

        public IndexerConfiguration ConfigureSet(Action<IPropertyMethodConfiguration> configurationFunc)
        {
            configurationFunc.Invoke(_setMethod);
            return this;
        }

        public IndexerConfiguration WithParameter(string type, string name, Action<IParameterConfiguration>? parameterConfiguration = null)
        {
            ParameterConfiguration.AddParameter(_parameters, type, name, parameterConfiguration);
            return this;
        }

        IIndexerWithSetConfiguration IIndexerWithSetConfiguration.AsInit()
            => AsInit();

        IReadWriteIndexerConfiguration IIndexerWithSetConfiguration<IReadWriteIndexerConfiguration>.AsInit()
            => AsInit();

        IWriteOnlyIndexerConfiguration IIndexerWithSetConfiguration<IWriteOnlyIndexerConfiguration>.AsInit()
            => AsInit();

        IReadOnlyIndexerConfiguration IReadWriteIndexerConfiguration.AsReadOnly()
            => this;

        IWriteOnlyIndexerConfiguration IReadWriteIndexerConfiguration.AsWriteOnly()
            => this;

        IIndexerWithGetConfiguration IIndexerWithGetConfiguration.ConfigureGet(Action<IPropertyMethodConfiguration> configurationFunc)
            => ConfigureGet(configurationFunc);

        IReadWriteIndexerConfiguration IIndexerWithGetConfiguration<IReadWriteIndexerConfiguration>.ConfigureGet(Action<IPropertyMethodConfiguration> configurationFunc)
            => ConfigureGet(configurationFunc);

        IReadOnlyIndexerConfiguration IIndexerWithGetConfiguration<IReadOnlyIndexerConfiguration>.ConfigureGet(Action<IPropertyMethodConfiguration> configurationFunc)
            => ConfigureGet(configurationFunc);

        IIndexerWithSetConfiguration IIndexerWithSetConfiguration.ConfigureSet(Action<IPropertyMethodConfiguration> configurationFunc)
            => ConfigureSet(configurationFunc);

        IReadWriteIndexerConfiguration IIndexerWithSetConfiguration<IReadWriteIndexerConfiguration>.ConfigureSet(Action<IPropertyMethodConfiguration> configurationFunc)
            => ConfigureSet(configurationFunc);

        IWriteOnlyIndexerConfiguration IIndexerWithSetConfiguration<IWriteOnlyIndexerConfiguration>.ConfigureSet(Action<IPropertyMethodConfiguration> configurationFunc)
            => ConfigureSet(configurationFunc);

        IReadWriteIndexerConfiguration ISupportsAccessModifierConfiguration<IReadWriteIndexerConfiguration>.WithAccessModifier(AccessModifier accessModifier)
            => WithAccessModifier(accessModifier);

        IReadOnlyIndexerConfiguration ISupportsAccessModifierConfiguration<IReadOnlyIndexerConfiguration>.WithAccessModifier(AccessModifier accessModifier)
            => WithAccessModifier(accessModifier);

        IWriteOnlyIndexerConfiguration ISupportsAccessModifierConfiguration<IWriteOnlyIndexerConfiguration>.WithAccessModifier(AccessModifier accessModifier)
            => WithAccessModifier(accessModifier);

        IReadWriteIndexerConfiguration ISupportsLineCommentsConfiguration<IReadWriteIndexerConfiguration>.WithBlockComment(string comment)
            => WithBlockComment(comment);

        IReadOnlyIndexerConfiguration ISupportsLineCommentsConfiguration<IReadOnlyIndexerConfiguration>.WithBlockComment(string comment)
            => WithBlockComment(comment);

        IWriteOnlyIndexerConfiguration ISupportsLineCommentsConfiguration<IWriteOnlyIndexerConfiguration>.WithBlockComment(string comment)
            => WithBlockComment(comment);

        IReadWriteIndexerConfiguration ISupportsCodeAttributeConfiguration<IReadWriteIndexerConfiguration>.WithCodeAttribute(string attributeTypeName)
            => WithCodeAttribute(attributeTypeName);

        IReadWriteIndexerConfiguration ISupportsCodeAttributeConfiguration<IReadWriteIndexerConfiguration>.WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration> attributeConfiguration)
            => WithCodeAttribute(attributeTypeName, attributeConfiguration);

        IReadOnlyIndexerConfiguration ISupportsCodeAttributeConfiguration<IReadOnlyIndexerConfiguration>.WithCodeAttribute(string attributeTypeName)
            => WithCodeAttribute(attributeTypeName);

        IReadOnlyIndexerConfiguration ISupportsCodeAttributeConfiguration<IReadOnlyIndexerConfiguration>.WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration> attributeConfiguration)
            => WithCodeAttribute(attributeTypeName, attributeConfiguration);

        IWriteOnlyIndexerConfiguration ISupportsCodeAttributeConfiguration<IWriteOnlyIndexerConfiguration>.WithCodeAttribute(string attributeTypeName)
            => WithCodeAttribute(attributeTypeName);

        IWriteOnlyIndexerConfiguration ISupportsCodeAttributeConfiguration<IWriteOnlyIndexerConfiguration>.WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration> attributeConfiguration)
            => WithCodeAttribute(attributeTypeName, attributeConfiguration);

        IReadWriteIndexerConfiguration ISupportsLineCommentsConfiguration<IReadWriteIndexerConfiguration>.WithDocComment(string comment)
            => WithDocComment(comment);

        IReadOnlyIndexerConfiguration ISupportsLineCommentsConfiguration<IReadOnlyIndexerConfiguration>.WithDocComment(string comment)
            => WithDocComment(comment);

        IWriteOnlyIndexerConfiguration ISupportsLineCommentsConfiguration<IWriteOnlyIndexerConfiguration>.WithDocComment(string comment)
            => WithDocComment(comment);

        IReadWriteIndexerConfiguration IMemberConfiguration<IReadWriteIndexerConfiguration>.WithKeyword(MemberKeyword keyword)
            => WithKeyword(keyword);

        IReadOnlyIndexerConfiguration IMemberConfiguration<IReadOnlyIndexerConfiguration>.WithKeyword(MemberKeyword keyword)
            => WithKeyword(keyword);

        IWriteOnlyIndexerConfiguration IMemberConfiguration<IWriteOnlyIndexerConfiguration>.WithKeyword(MemberKeyword keyword)
            => WithKeyword(keyword);

        IReadWriteIndexerConfiguration ISupportsLineCommentsConfiguration<IReadWriteIndexerConfiguration>.WithLineComment(string comment)
            => WithLineComment(comment);

        IReadOnlyIndexerConfiguration ISupportsLineCommentsConfiguration<IReadOnlyIndexerConfiguration>.WithLineComment(string comment)
            => WithLineComment(comment);

        IWriteOnlyIndexerConfiguration ISupportsLineCommentsConfiguration<IWriteOnlyIndexerConfiguration>.WithLineComment(string comment)
            => WithLineComment(comment);

        IReadWriteIndexerConfiguration IDefinesParametersConfiguration<IReadWriteIndexerConfiguration>.WithParameter(string type, string name)
            => WithParameter(type, name);

        IReadWriteIndexerConfiguration IDefinesParametersConfiguration<IReadWriteIndexerConfiguration>.WithParameter(string type, string name, Action<IParameterConfiguration> parameterConfiguration)
            => WithParameter(type, name, parameterConfiguration);

        IDefinesParametersConfiguration IDefinesParametersConfiguration.WithParameter(string type, string name)
            => WithParameter(type, name);

        IDefinesParametersConfiguration IDefinesParametersConfiguration.WithParameter(string type, string name, Action<IParameterConfiguration> parameterConfiguration)
            => WithParameter(type, name, parameterConfiguration);

        IReadOnlyIndexerConfiguration IDefinesParametersConfiguration<IReadOnlyIndexerConfiguration>.WithParameter(string type, string name)
            => WithParameter(type, name);

        IReadOnlyIndexerConfiguration IDefinesParametersConfiguration<IReadOnlyIndexerConfiguration>.WithParameter(string type, string name, Action<IParameterConfiguration> parameterConfiguration)
            => WithParameter(type, name, parameterConfiguration);

        IWriteOnlyIndexerConfiguration IDefinesParametersConfiguration<IWriteOnlyIndexerConfiguration>.WithParameter(string type, string name)
            => WithParameter(type, name);

        IWriteOnlyIndexerConfiguration IDefinesParametersConfiguration<IWriteOnlyIndexerConfiguration>.WithParameter(string type, string name, Action<IParameterConfiguration> parameterConfiguration)
            => WithParameter(type, name, parameterConfiguration);

        public override void WriteTo(ISourceBuilder sourceBuilder)
        {
            WriteCommentsTo(sourceBuilder);
            WriteCodeAttributesTo(sourceBuilder);
            WriteKeywordsTo(sourceBuilder);
            sourceBuilder.Append(Type).Append(' ');
            WriteNameTo(sourceBuilder);
            sourceBuilder.Append('[');
            ParameterConfiguration.WriteParametersTo(_parameters, sourceBuilder, false);
            sourceBuilder.Append(']');
        }
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    /// <summary>
    /// Provides extension methods for the <see cref="IReadOnlyIndexerConfiguration"/>, <see cref="IWriteOnlyIndexerConfiguration"/> and <see cref="IReadWriteIndexerConfiguration"/> interfaces.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Extensions")]
    public static class IndexerConfigurationExtensions
    {
        /// <summary>
        /// Sets the body type of the read-only indexer represented by this <see cref="IReadOnlyIndexerConfiguration"/> to <see cref="MethodBodyType.Expression"/>/<see cref="MethodBodyType.ExpressionNewLine"/>.
        /// </summary>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <param name="placeInNewLine">Determines whether to use <see cref="MethodBodyType.Expression"/> or <see cref="MethodBodyType.ExpressionNewLine"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <remarks>
        /// If <paramref name="placeInNewLine"/> is set to <c>true</c> <see cref="MethodBodyType.ExpressionNewLine"/> is used; otherwise <see cref="MethodBodyType.Expression"/>.
        /// </remarks>
        public static IReadOnlyIndexerConfiguration AsExpression(this IReadOnlyIndexerConfiguration config, bool placeInNewLine = false)
        {
            config.GetMethod.AsExpression(placeInNewLine);
            return config;
        }

        /// <summary>
        /// Sets the body type of the write-only indexer represented by this <see cref="IWriteOnlyIndexerConfiguration"/> to <see cref="MethodBodyType.Expression"/>/<see cref="MethodBodyType.ExpressionNewLine"/>.
        /// </summary>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <param name="placeInNewLine">Determines whether to use <see cref="MethodBodyType.Expression"/> or <see cref="MethodBodyType.ExpressionNewLine"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <remarks>
        /// If <paramref name="placeInNewLine"/> is set to <c>true</c> <see cref="MethodBodyType.ExpressionNewLine"/> is used; otherwise <see cref="MethodBodyType.Expression"/>.
        /// </remarks>
        public static IWriteOnlyIndexerConfiguration AsExpression(this IWriteOnlyIndexerConfiguration config, bool placeInNewLine = false)
        {
            config.SetMethod.AsExpression(placeInNewLine);
            return config;
        }

        /// <summary>
        /// Sets the body type of the get- and set-methods of the indexer represented by this <see cref="IReadWriteIndexerConfiguration"/> to <see cref="MethodBodyType.Expression"/>/<see cref="MethodBodyType.ExpressionNewLine"/>.
        /// </summary>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <param name="placeInNewLine">Determines whether to use <see cref="MethodBodyType.Expression"/> or <see cref="MethodBodyType.ExpressionNewLine"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <remarks>
        /// If <paramref name="placeInNewLine"/> is set to <c>true</c> <see cref="MethodBodyType.ExpressionNewLine"/> is used; otherwise <see cref="MethodBodyType.Expression"/>.
        /// </remarks>
        public static IReadWriteIndexerConfiguration AsExpression(this IReadWriteIndexerConfiguration config, bool placeInNewLine = false)
        {
            config.GetMethod.AsExpression(placeInNewLine);
            config.SetMethod.AsExpression(placeInNewLine);
            return config;
        }

        /// <summary>
        /// Changes the indexer represented by this <see cref="IReadWriteIndexerConfiguration"/> to a init-only indexer.
        /// </summary>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <returns>An instance of <see cref="IWriteOnlyIndexerConfiguration"/> representing this <see cref="IReadWriteIndexerConfiguration"/> as a init-only indexer.</returns>
        public static IWriteOnlyIndexerConfiguration AsInitOnly(this IReadWriteIndexerConfiguration config)
            => config.AsWriteOnly().AsInit();
    }
}