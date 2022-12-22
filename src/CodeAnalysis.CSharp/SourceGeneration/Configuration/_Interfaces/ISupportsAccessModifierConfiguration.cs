using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration
{
    /// <summary>
    /// Represents configuration of a code element for which an access modifier can be defined. This is used to generate code in the <see cref="ISourceBuilder"/>.
    /// </summary>
    public interface ISupportsAccessModifierConfiguration : ICodeConfiguration
    {
        /// <summary>
        /// Gets the access modifier of this <see cref="ISupportsAccessModifierConfiguration"/>.
        /// </summary>
        AccessModifier AccessModifier { get; }

        /// <summary>
        /// Sets the access modifier of this <see cref="ISupportsAccessModifierConfiguration"/>.
        /// </summary>
        /// <param name="accessModifier">The access modifier to use.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        ISupportsAccessModifierConfiguration WithAccessModifier(AccessModifier accessModifier);
    }

    /// <inheritdoc cref="ISupportsAccessModifierConfiguration"/>
    /// <typeparam name="T">The type of <see cref="ICodeConfiguration"/>.</typeparam>
    public interface ISupportsAccessModifierConfiguration<T> : ISupportsAccessModifierConfiguration
        where T : ISupportsAccessModifierConfiguration<T>
    {
        /// <inheritdoc cref="ISupportsAccessModifierConfiguration.WithAccessModifier(AccessModifier)"/>
        new T WithAccessModifier(AccessModifier accessModifier);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    /// <summary>
    /// Provides extension methods for the <see cref="ISupportsAccessModifierConfiguration"/> interface.
    /// </summary>
    public static class SupportsAccessModifierConfigurationExtensions
    {
        /// <summary>
        /// Sets the access modifier of this <see cref="ISupportsAccessModifierConfiguration"/> to <see cref="AccessModifier.Public"/>.
        /// </summary>
        /// <typeparam name="TConfig">The type of <see cref="ICodeConfiguration"/>.</typeparam>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static TConfig AsPublic<TConfig>(this TConfig config)
            where TConfig : ISupportsAccessModifierConfiguration
        {
            config.WithAccessModifier(AccessModifier.Public);
            return config;
        }

        /// <summary>
        /// Sets the access modifier of this <see cref="ISupportsAccessModifierConfiguration"/> to <see cref="AccessModifier.Private"/>.
        /// </summary>
        /// <typeparam name="TConfig">The type of <see cref="ICodeConfiguration"/>.</typeparam>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static TConfig AsPrivate<TConfig>(this TConfig config)
            where TConfig : ISupportsAccessModifierConfiguration
        {
            config.WithAccessModifier(AccessModifier.Private);
            return config;
        }

        /// <summary>
        /// Sets the access modifier of this <see cref="ISupportsAccessModifierConfiguration"/> to <see cref="AccessModifier.Protected"/>.
        /// </summary>
        /// <typeparam name="TConfig">The type of <see cref="ICodeConfiguration"/>.</typeparam>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static TConfig AsProtected<TConfig>(this TConfig config)
            where TConfig : ISupportsAccessModifierConfiguration
        {
            config.WithAccessModifier(AccessModifier.Protected);
            return config;
        }

        /// <summary>
        /// Sets the access modifier of this <see cref="ISupportsAccessModifierConfiguration"/> to <see cref="AccessModifier.Internal"/>.
        /// </summary>
        /// <typeparam name="TConfig">The type of <see cref="ICodeConfiguration"/>.</typeparam>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static TConfig AsInternal<TConfig>(this TConfig config)
            where TConfig : ISupportsAccessModifierConfiguration
        {
            config.WithAccessModifier(AccessModifier.Internal);
            return config;
        }

        /// <summary>
        /// Sets the access modifier of this <see cref="ISupportsAccessModifierConfiguration"/> to <see cref="AccessModifier.ProtectedInternal"/>.
        /// </summary>
        /// <typeparam name="TConfig">The type of <see cref="ICodeConfiguration"/>.</typeparam>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static TConfig AsProtectedInternal<TConfig>(this TConfig config)
            where TConfig : ISupportsAccessModifierConfiguration
        {
            config.WithAccessModifier(AccessModifier.ProtectedInternal);
            return config;
        }

        /// <summary>
        /// Sets the access modifier of this <see cref="ISupportsAccessModifierConfiguration"/> to <see cref="AccessModifier.PrivateProtected"/>.
        /// </summary>
        /// <typeparam name="TConfig">The type of <see cref="ICodeConfiguration"/>.</typeparam>
        /// <param name="config">The extended <see cref="ICodeConfiguration"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static TConfig AsPrivateProtected<TConfig>(this TConfig config)
            where TConfig : ISupportsAccessModifierConfiguration
        {
            config.WithAccessModifier(AccessModifier.PrivateProtected);
            return config;
        }
    }
}