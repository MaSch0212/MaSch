namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

/// <summary>
/// Represents configuration of a code element that can define parameters. This is used to generate code in the <see cref="ISourceBuilder"/>.
/// </summary>
public interface IDefinesParametersConfiguration : ICodeConfiguration
{
    /// <summary>
    /// Gets a value indicating whether the parameters span over multiple lines.
    /// </summary>
    bool MultilineParameters { get; }

    /// <summary>
    /// Gets a read-only list of all parameters defined by this <see cref="IDefinesParametersConfiguration"/>.
    /// </summary>
    IReadOnlyList<IParameterConfiguration> Parameters { get; }

    /// <summary>
    /// Adds a parameter to this <see cref="ICodeConfiguration"/>.
    /// </summary>
    /// <param name="type">The type of the parameter.</param>
    /// <param name="name">The name of the parameter.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    IDefinesParametersConfiguration WithParameter(string type, string name);

    /// <summary>
    /// Adds a parameter to this <see cref="ICodeConfiguration"/>.
    /// </summary>
    /// <param name="type">The type of the parameter.</param>
    /// <param name="name">The name of the parameter.</param>
    /// <param name="parameterConfiguration">A function to configure the added parameter.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    IDefinesParametersConfiguration WithParameter(string type, string name, Action<IParameterConfiguration> parameterConfiguration);
}

/// <inheritdoc cref="IDefinesParametersConfiguration"/>
/// <typeparam name="T">The type of <see cref="ICodeConfiguration"/>.</typeparam>
public interface IDefinesParametersConfiguration<T> : IDefinesParametersConfiguration
    where T : IDefinesParametersConfiguration<T>
{
    /// <inheritdoc cref="IDefinesParametersConfiguration.WithParameter(string, string)"/>
    new T WithParameter(string type, string name);

    /// <inheritdoc cref="IDefinesParametersConfiguration.WithParameter(string, string, Action{IParameterConfiguration})"/>
    new T WithParameter(string type, string name, Action<IParameterConfiguration> parameterConfiguration);
}