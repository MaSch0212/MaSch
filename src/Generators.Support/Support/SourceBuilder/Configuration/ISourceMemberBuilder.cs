using System;

namespace MaSch.Generators.Support
{
    public interface ISourceMemberBuilder<T>
        where T : ISourceMemberBuilder<T>
    {
        T WithAccessModifier(AccessModifier accessModifier);
        T WithCodeAttribute<TParams>(string attributeTypeName, TParams @params, Action<ISourceCodeAttributeBuilder, TParams> attributeConfiguration);
    }

    /// <summary>
    /// Provides extension methods for the <see cref="ISourceMemberBuilder{T}"/> interface.
    /// </summary>
    public static class SourceMemberBuilderExtensions
    {
        public static TBuilder AsPublic<TBuilder>(this TBuilder builder)
            where TBuilder : ISourceMemberBuilder<TBuilder>
            => builder.WithAccessModifier(AccessModifier.Public);

        public static TBuilder AsPrivate<TBuilder>(this TBuilder builder)
            where TBuilder : ISourceMemberBuilder<TBuilder>
            => builder.WithAccessModifier(AccessModifier.Private);

        public static TBuilder AsProtected<TBuilder>(this TBuilder builder)
            where TBuilder : ISourceMemberBuilder<TBuilder>
            => builder.WithAccessModifier(AccessModifier.Protected);

        public static TBuilder AsInternal<TBuilder>(this TBuilder builder)
            where TBuilder : ISourceMemberBuilder<TBuilder>
            => builder.WithAccessModifier(AccessModifier.Internal);

        public static TBuilder AsProtectedInternal<TBuilder>(this TBuilder builder)
            where TBuilder : ISourceMemberBuilder<TBuilder>
            => builder.WithAccessModifier(AccessModifier.ProtectedInternal);

        public static TBuilder AsPrivateProtected<TBuilder>(this TBuilder builder)
            where TBuilder : ISourceMemberBuilder<TBuilder>
            => builder.WithAccessModifier(AccessModifier.PrivateProtected);

        public static TBuilder WithCodeAttribute<TBuilder>(this TBuilder builder, string attributeTypeName)
            where TBuilder : ISourceMemberBuilder<TBuilder>
            => builder.WithCodeAttribute<object?>(attributeTypeName, null, static (_, _) => { });

        public static TBuilder WithCodeAttribute<TBuilder>(this TBuilder builder, string attributeTypeName, Action<ISourceCodeAttributeBuilder> attributeConfiguration)
            where TBuilder : ISourceMemberBuilder<TBuilder>
            => builder.WithCodeAttribute(attributeTypeName, attributeConfiguration, static (builder, action) => action(builder));

        public static TBuilder WithCodeAttribute<TBuilder>(this TBuilder builder, string attributeTypeName, Action<ISourceCodeAttributeBuilder> attributeConfiguration)
            where TBuilder : ISourceMemberBuilder<TBuilder>
            => builder.WithCodeAttribute(attributeTypeName, attributeConfiguration, static (builder, action) => action(builder));
    }
}