using Microsoft.CodeAnalysis.Text;
using System;
using System.Text;

namespace MaSch.Generators.Support
{
    public interface ISourceBuilder
    {
        /// <summary>
        /// Gets the size of the indentation.
        /// </summary>
        int IndentSize { get; }

        /// <summary>
        /// Gets or sets the current indentation level.
        /// </summary>
        int CurrentIndentLevel { get; set; }

        /// <summary>
        /// Appends a new region to the source file.
        /// </summary>
        /// <param name="regionName">Name of the region.</param>
        /// <returns>Returns an <see cref="IDisposable"/> object, which closes the region when disposed.</returns>
        SourceBuilderCodeBlock AppendRegion(string regionName);

        /// <summary>
        /// Appends a new code block to the source file.
        /// </summary>
        /// <param name="blockLine">The line before the code block.</param>
        /// <returns>Returns an <see cref="IDisposable"/> object, which closes the code block when disposed.</returns>
        SourceBuilderCodeBlock AppendBlock(string blockLine);

        /// <summary>
        /// Appends a new code block to the source file.
        /// </summary>
        /// <param name="blockLine">The line before the code block.</param>
        /// <param name="addSemicolon">if <c>true</c> adds a semicolon after the end of the block.</param>
        /// <returns>Returns an <see cref="IDisposable"/> object, which closes the code block when disposed.</returns>
        SourceBuilderCodeBlock AppendBlock(string blockLine, bool addSemicolon);

        /// <summary>
        /// Appends a new code block to the source file.
        /// </summary>
        /// <returns>Returns an <see cref="IDisposable"/> object, which closes the code block when disposed.</returns>
        SourceBuilderCodeBlock AppendBlock();

        /// <summary>
        /// Appends a new code block to the source file.
        /// </summary>
        /// <param name="addSemicolon">if <c>true</c> adds a semicolon after the end of the block.</param>
        /// <returns>Returns an <see cref="IDisposable"/> object, which closes the code block when disposed.</returns>
        SourceBuilderCodeBlock AppendBlock(bool addSemicolon);

        /// <summary>
        /// Appends the default line terminator to the end of the current <see cref="SourceBuilder"/> object.
        /// </summary>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        ISourceBuilder AppendLine();

        /// <summary>
        /// Appends a copy of the specified string followed by the default line terminator to the end of the current <see cref="SourceBuilder"/> object.
        /// </summary>
        /// <param name="value">The string to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        ISourceBuilder AppendLine(string value);

        /// <summary>
        /// Appends a copy of the specified string to this instance.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        ISourceBuilder Append(string value);

        /// <summary>
        /// Appends the string representation of a specified <see cref="char"/> object to this instance.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        ISourceBuilder Append(char value);

        /// <summary>
        /// Converts the value of this instance to a <see cref="SourceText"/>.
        /// </summary>
        /// <param name="encoding">The encoding to use.</param>
        /// <param name="checksumAlgorithm">The source hash algorithm to use.</param>
        /// <returns>A <see cref="SourceText"/> whose value is the same as this instance.</returns>
        SourceText ToSourceText(Encoding? encoding = null, SourceHashAlgorithm checksumAlgorithm = SourceHashAlgorithm.Sha1);

        T As<T>()
            where T : ISourceBuilder;
    }

    public interface ISourceBuilder<T> : ISourceBuilder
        where T : ISourceBuilder<T>
    {
        /// <inheritdoc cref="ISourceBuilder.AppendLine()" />
        new T AppendLine();

        /// <inheritdoc cref="ISourceBuilder.AppendLine(string)" />
        new T AppendLine(string value);

        /// <inheritdoc cref="ISourceBuilder.Append(string)" />
        new T Append(string value);

        /// <inheritdoc cref="ISourceBuilder.Append(char)" />
        new T Append(char value);
    }

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
}