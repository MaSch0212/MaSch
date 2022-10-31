namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IMemberConfiguration : ISupportsCodeAttributeConfiguration
{
    string MemberName { get; }

    IMemberConfiguration WithAccessModifier(AccessModifier accessModifier);
    IMemberConfiguration WithKeyword(MemberKeyword keyword);
}

public interface IMemberConfiguration<T> : IMemberConfiguration, ISupportsCodeAttributeConfiguration<T>
    where T : IMemberConfiguration<T>
{
    new T WithAccessModifier(AccessModifier accessModifier);
    new T WithKeyword(MemberKeyword keyword);
}

/// <summary>
/// Provides extension methods for the <see cref="IMemberConfiguration{T}"/> interface.
/// </summary>
public static class SourceMemberBuilderExtensions
{
    public static TConfig AsPublic<TConfig>(this TConfig builder)
        where TConfig : IMemberConfiguration
    {
        builder.WithAccessModifier(AccessModifier.Public);
        return builder;
    }

    public static TConfig AsPrivate<TConfig>(this TConfig builder)
        where TConfig : IMemberConfiguration
    {
        builder.WithAccessModifier(AccessModifier.Private);
        return builder;
    }

    public static TConfig AsProtected<TConfig>(this TConfig builder)
        where TConfig : IMemberConfiguration
    {
        builder.WithAccessModifier(AccessModifier.Protected);
        return builder;
    }

    public static TConfig AsInternal<TConfig>(this TConfig builder)
        where TConfig : IMemberConfiguration
    {
        builder.WithAccessModifier(AccessModifier.Internal);
        return builder;
    }

    public static TConfig AsProtectedInternal<TConfig>(this TConfig builder)
        where TConfig : IMemberConfiguration
    {
        builder.WithAccessModifier(AccessModifier.ProtectedInternal);
        return builder;
    }

    public static TConfig AsPrivateProtected<TConfig>(this TConfig builder)
        where TConfig : IMemberConfiguration
    {
        builder.WithAccessModifier(AccessModifier.PrivateProtected);
        return builder;
    }

    public static TConfig WithCodeAttribute<TConfig>(this TConfig builder, string attributeTypeName)
        where TConfig : IMemberConfiguration
    {
        builder.WithCodeAttribute<object?>(attributeTypeName, null, static (_, _) => { });
        return builder;
    }

    public static TConfig WithCodeAttribute<TConfig>(this TConfig builder, string attributeTypeName, Action<ICodeAttributeConfiguration> attributeConfiguration)
        where TConfig : IMemberConfiguration
    {
        builder.WithCodeAttribute(attributeTypeName, attributeConfiguration, static (builder, action) => action(builder));
        return builder;
    }

    public static TConfig WithCodeAttribute<TConfig>(this TConfig builder, string attributeTypeName, string param1)
        where TConfig : IMemberConfiguration
    {
        builder.WithCodeAttribute(attributeTypeName, param1, static (b, p) => b.WithParameter(p));
        return builder;
    }

    public static TConfig WithCodeAttribute<TConfig>(this TConfig builder, string attributeTypeName, string param1, string param2)
        where TConfig : IMemberConfiguration
    {
        builder.WithCodeAttribute(attributeTypeName, (param1, param2), static (b, p) => b.WithParameters(p.param1, p.param2));
        return builder;
    }

    public static TConfig WithCodeAttribute<TConfig>(this TConfig builder, string attributeTypeName, string param1, string param2, string param3)
        where TConfig : IMemberConfiguration
    {
        builder.WithCodeAttribute(attributeTypeName, (param1, param2, param3), static (b, p) => b.WithParameters(p.param1, p.param2, p.param3));
        return builder;
    }

    public static TConfig WithCodeAttribute<TConfig>(this TConfig builder, string attributeTypeName, string param1, string param2, string param3, string param4)
        where TConfig : IMemberConfiguration
    {
        builder.WithCodeAttribute(attributeTypeName, (param1, param2, param3, param4), static (b, p) => b.WithParameters(p.param1, p.param2, p.param3, p.param4));
        return builder;
    }

    public static TConfig WithCodeAttribute<TConfig>(this TConfig builder, string attributeTypeName, params string[] @params)
        where TConfig : IMemberConfiguration
    {
        builder.WithCodeAttribute(attributeTypeName, @params, static (b, p) => b.WithParameters(p));
        return builder;
    }
}