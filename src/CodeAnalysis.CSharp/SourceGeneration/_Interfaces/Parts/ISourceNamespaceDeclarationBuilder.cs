namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface ISourceNamespaceDeclarationBuilder<T> : ISourceBuilder
    where T : ISourceNamespaceDeclarationBuilder<T>
{
    SourceBuilderCodeBlock AppendNamespace(string @namespace, out ISourceNamespaceBuilder namespaceBuilder);
}

/// <summary>
/// Provides extension methods for the <see cref="ISourceNamespaceDeclarationBuilder{T}"/> interface.
/// </summary>
public static class SourceNamespaceDeclarationBuilderExtensions
{
    public static TBuilder AppendNamespace<TBuilder>(this TBuilder builder, string @namespace, Action<ISourceNamespaceBuilder> action)
        where TBuilder : ISourceNamespaceDeclarationBuilder<TBuilder>
    {
        using (builder.AppendNamespace(@namespace, out var namespaceBuilder))
            action(namespaceBuilder);
        return builder;
    }

    public static TBuilder AppendNamespace<TBuilder, TParams>(this TBuilder builder, string @namespace, TParams actionParams, Action<ISourceNamespaceBuilder, TParams> action)
        where TBuilder : ISourceNamespaceDeclarationBuilder<TBuilder>
    {
        using (builder.AppendNamespace(@namespace, out var namespaceBuilder))
            action(namespaceBuilder, actionParams);
        return builder;
    }
}