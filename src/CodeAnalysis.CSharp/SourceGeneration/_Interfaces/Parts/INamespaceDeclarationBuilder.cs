namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface INamespaceDeclarationBuilder : ISourceBuilder
{
    SourceBuilderCodeBlock AppendNamespace(string @namespace, out INamespaceBuilder namespaceBuilder);
}

public interface INamespaceDeclarationBuilder<T> : INamespaceDeclarationBuilder
    where T : INamespaceDeclarationBuilder<T>
{
}

public partial class SourceBuilder : INamespaceDeclarationBuilder
{
    /// <inheritdoc/>
    SourceBuilderCodeBlock INamespaceDeclarationBuilder.AppendNamespace(string @namespace, out INamespaceBuilder namespaceBuilder)
        => AppendNamespace(@namespace, out namespaceBuilder);
}

/// <summary>
/// Provides extension methods for the <see cref="INamespaceDeclarationBuilder{T}"/> interface.
/// </summary>
public static class NamespaceDeclarationBuilderExtensions
{
    public static TBuilder AppendNamespace<TBuilder>(this TBuilder builder, string @namespace, Action<INamespaceBuilder> action)
        where TBuilder : INamespaceDeclarationBuilder
    {
        using (builder.AppendNamespace(@namespace, out var namespaceBuilder))
            action?.Invoke(namespaceBuilder);
        return builder;
    }

    public static TBuilder AppendNamespace<TBuilder, TParams>(this TBuilder builder, string @namespace, TParams actionParams, Action<INamespaceBuilder, TParams> action)
        where TBuilder : INamespaceDeclarationBuilder
    {
        using (builder.AppendNamespace(@namespace, out var namespaceBuilder))
            action?.Invoke(namespaceBuilder, actionParams);
        return builder;
    }
}