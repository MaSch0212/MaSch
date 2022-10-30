namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

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

    public static TBuilder WithCodeAttribute<TBuilder>(this TBuilder builder, string attributeTypeName, string param1)
        where TBuilder : ISourceMemberBuilder<TBuilder>
        => builder.WithCodeAttribute(attributeTypeName, param1, static (b, p) => b.WithParameter(p));

    public static TBuilder WithCodeAttribute<TBuilder>(this TBuilder builder, string attributeTypeName, string param1, string param2)
        where TBuilder : ISourceMemberBuilder<TBuilder>
        => builder.WithCodeAttribute(attributeTypeName, (param1, param2), static (b, p) => b.WithParameters(p.param1, p.param2));

    public static TBuilder WithCodeAttribute<TBuilder>(this TBuilder builder, string attributeTypeName, string param1, string param2, string param3)
        where TBuilder : ISourceMemberBuilder<TBuilder>
        => builder.WithCodeAttribute(attributeTypeName, (param1, param2, param3), static (b, p) => b.WithParameters(p.param1, p.param2, p.param3));

    public static TBuilder WithCodeAttribute<TBuilder>(this TBuilder builder, string attributeTypeName, string param1, string param2, string param3, string param4)
        where TBuilder : ISourceMemberBuilder<TBuilder>
        => builder.WithCodeAttribute(attributeTypeName, (param1, param2, param3, param4), static (b, p) => b.WithParameters(p.param1, p.param2, p.param3, p.param4));

    public static TBuilder WithCodeAttribute<TBuilder>(this TBuilder builder, string attributeTypeName, params string[] @params)
        where TBuilder : ISourceMemberBuilder<TBuilder>
        => builder.WithCodeAttribute(attributeTypeName, @params, static (b, p) => b.WithParameters(p));
}