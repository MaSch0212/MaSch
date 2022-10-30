namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

/// <summary>
/// Provides extension methods for the <see cref="SourceBuilder"/> class and <see cref="ISourceBuilder"/> and <see cref="ISourceBuilder{T}"/> interfaces.
/// </summary>
public static class SourceBuilderExtensions
{
    public static TBuilder AppendRegion<TBuilder>(this TBuilder builder, string regionName, Action<TBuilder> action)
        where TBuilder : ISourceBuilder
    {
        using (builder.AppendRegion(regionName))
            action(builder);
        return builder;
    }

    public static TBuilder AppendRegion<TBuilder, TParams>(this TBuilder builder, string regionName, TParams actionParams, Action<TBuilder, TParams> action)
        where TBuilder : ISourceBuilder
    {
        using (builder.AppendRegion(regionName))
            action(builder, actionParams);
        return builder;
    }

    public static TBuilder AppendBlock<TBuilder>(this TBuilder builder, string blockLine, Action<TBuilder> action)
        where TBuilder : ISourceBuilder
    {
        using (builder.AppendBlock(blockLine))
            action(builder);
        return builder;
    }

    public static TBuilder AppendBlock<TBuilder, TParams>(this TBuilder builder, string blockLine, TParams actionParams, Action<TBuilder, TParams> action)
        where TBuilder : ISourceBuilder
    {
        using (builder.AppendBlock(blockLine))
            action(builder, actionParams);
        return builder;
    }

    public static TBuilder AppendBlock<TBuilder>(this TBuilder builder, string blockLine, bool addSemicolon, Action<TBuilder> action)
        where TBuilder : ISourceBuilder
    {
        using (builder.AppendBlock(blockLine, addSemicolon))
            action(builder);
        return builder;
    }

    public static TBuilder AppendBlock<TBuilder, TParams>(this TBuilder builder, string blockLine, bool addSemicolon, TParams actionParams, Action<TBuilder, TParams> action)
        where TBuilder : ISourceBuilder
    {
        using (builder.AppendBlock(blockLine, addSemicolon))
            action(builder, actionParams);
        return builder;
    }

    public static TBuilder AppendBlock<TBuilder>(this TBuilder builder, Action<TBuilder> action)
        where TBuilder : ISourceBuilder
    {
        using (builder.AppendBlock())
            action(builder);
        return builder;
    }

    public static TBuilder AppendBlock<TBuilder, TParams>(this TBuilder builder, TParams actionParams, Action<TBuilder, TParams> action)
        where TBuilder : ISourceBuilder
    {
        using (builder.AppendBlock())
            action(builder, actionParams);
        return builder;
    }

    public static TBuilder AppendBlock<TBuilder>(this TBuilder builder, bool addSemicolon, Action<TBuilder> action)
        where TBuilder : ISourceBuilder
    {
        using (builder.AppendBlock(addSemicolon))
            action(builder);
        return builder;
    }

    public static TBuilder AppendBlock<TBuilder, TParams>(this TBuilder builder, bool addSemicolon, TParams actionParams, Action<TBuilder, TParams> action)
        where TBuilder : ISourceBuilder
    {
        using (builder.AppendBlock(addSemicolon))
            action(builder, actionParams);
        return builder;
    }
}