namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface INamespaceDeclarationBuilder<T> : ISourceBuilder
    where T : INamespaceDeclarationBuilder<T>
{
    SourceBuilderCodeBlock AppendNamespace(string @namespace, out INamespaceBuilder namespaceBuilder);
}

/// <summary>
/// Provides extension methods for the <see cref="INamespaceDeclarationBuilder{T}"/> interface.
/// </summary>
public static class NamespaceDeclarationBuilderExtensions
{
    public static TBuilder AppendNamespace<TBuilder>(this TBuilder builder, string @namespace, Action<INamespaceBuilder> action)
        where TBuilder : INamespaceDeclarationBuilder<TBuilder>
    {
        using (builder.AppendNamespace(@namespace, out var namespaceBuilder))
            action(namespaceBuilder);
        return builder;
    }

    public static TBuilder AppendNamespace<TBuilder, TParams>(this TBuilder builder, string @namespace, TParams actionParams, Action<INamespaceBuilder, TParams> action)
        where TBuilder : INamespaceDeclarationBuilder<TBuilder>
    {
        using (builder.AppendNamespace(@namespace, out var namespaceBuilder))
            action(namespaceBuilder, actionParams);
        return builder;
    }
}