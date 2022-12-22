namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

/// <summary>
/// Represents configuration of a code element that can define generic parameters. This is used to generate code in the <see cref="ISourceBuilder"/>.
/// </summary>
public interface IGenericMemberConfiguration : IMemberConfiguration
{
    /// <summary>
    /// Gets the member name of this <see cref="IGenericMemberConfiguration"/> without any generic parameters.
    /// </summary>
    string MemberNameWithoutGenericParameters { get; }

    /// <summary>
    /// Gets a read-only list of generic parameter that are defined for this <see cref="IGenericMemberConfiguration"/>.
    /// </summary>
    IReadOnlyList<IGenericParameterConfiguration> GenericParameters { get; }

    /// <summary>
    /// Adds a generic parameter to this <see cref="IGenericMemberConfiguration"/>.
    /// </summary>
    /// <param name="name">The name of the generic parameter.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    IGenericMemberConfiguration WithGenericParameter(string name);

    /// <summary>
    /// Adds a generic parameter to this <see cref="IGenericMemberConfiguration"/>.
    /// </summary>
    /// <param name="name">The name of the generic parameter.</param>
    /// <param name="parameterConfiguration">A function to configure the added generic parameter.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    IGenericMemberConfiguration WithGenericParameter(string name, Action<IGenericParameterConfiguration> parameterConfiguration);
}

/// <inheritdoc cref="IGenericMemberConfiguration"/>
/// <typeparam name="T">The type of <see cref="ICodeConfiguration"/>.</typeparam>
public interface IGenericMemberConfiguration<T> : IGenericMemberConfiguration, IMemberConfiguration<T>
    where T : IGenericMemberConfiguration<T>
{
    /// <inheritdoc cref="IGenericMemberConfiguration.WithGenericParameter(string)"/>
    new T WithGenericParameter(string name);

    /// <inheritdoc cref="IGenericMemberConfiguration.WithGenericParameter(string, Action{IGenericParameterConfiguration})"/>
    new T WithGenericParameter(string name, Action<IGenericParameterConfiguration> parameterConfiguration);
}