using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration
{
    public interface ISupportsAccessModifierConfiguration : ICodeConfiguration
    {
        ISupportsAccessModifierConfiguration WithAccessModifier(AccessModifier accessModifier);
    }

    public interface ISupportsAccessModifierConfiguration<T> : ISupportsAccessModifierConfiguration
        where T : ISupportsAccessModifierConfiguration<T>
    {
        new T WithAccessModifier(AccessModifier accessModifier);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    /// <summary>
    /// Provides extension methods for the <see cref="IMemberConfiguration{T}"/> interface.
    /// </summary>
    public static class SupportsAccessModifierConfigurationExtensions
    {
        public static TConfig AsPublic<TConfig>(this TConfig builder)
            where TConfig : ISupportsAccessModifierConfiguration
        {
            builder.WithAccessModifier(AccessModifier.Public);
            return builder;
        }

        public static TConfig AsPrivate<TConfig>(this TConfig builder)
            where TConfig : ISupportsAccessModifierConfiguration
        {
            builder.WithAccessModifier(AccessModifier.Private);
            return builder;
        }

        public static TConfig AsProtected<TConfig>(this TConfig builder)
            where TConfig : ISupportsAccessModifierConfiguration
        {
            builder.WithAccessModifier(AccessModifier.Protected);
            return builder;
        }

        public static TConfig AsInternal<TConfig>(this TConfig builder)
            where TConfig : ISupportsAccessModifierConfiguration
        {
            builder.WithAccessModifier(AccessModifier.Internal);
            return builder;
        }

        public static TConfig AsProtectedInternal<TConfig>(this TConfig builder)
            where TConfig : ISupportsAccessModifierConfiguration
        {
            builder.WithAccessModifier(AccessModifier.ProtectedInternal);
            return builder;
        }

        public static TConfig AsPrivateProtected<TConfig>(this TConfig builder)
            where TConfig : ISupportsAccessModifierConfiguration
        {
            builder.WithAccessModifier(AccessModifier.PrivateProtected);
            return builder;
        }
    }
}