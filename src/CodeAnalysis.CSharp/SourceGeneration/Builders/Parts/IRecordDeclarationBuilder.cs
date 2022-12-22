using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    /// <summary>
    /// Represents a <see cref="ISourceBuilder"/> used to build a record.
    /// </summary>
    public interface IRecordDeclarationBuilder : ISourceBuilder
    {
        /// <summary>
        /// Appends a record to the current context.
        /// </summary>
        /// <param name="recordConfiguration">The configuration of the record.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        IRecordDeclarationBuilder Append(IRecordConfiguration recordConfiguration);

        /// <summary>
        /// Appends a record to the current context.
        /// </summary>
        /// <param name="recordConfiguration">The configuration of the record.</param>
        /// <param name="builderFunc">The function to add content to the record.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        IRecordDeclarationBuilder Append(IRecordConfiguration recordConfiguration, Action<IRecordBuilder> builderFunc);
    }

    /// <inheritdoc cref="IRecordDeclarationBuilder"/>
    /// <typeparam name="T">The type of <see cref="ISourceBuilder{T}"/>.</typeparam>
    public interface IRecordDeclarationBuilder<T> : IRecordDeclarationBuilder, ISourceBuilder<T>
        where T : IRecordDeclarationBuilder<T>
    {
        /// <inheritdoc cref="IRecordDeclarationBuilder.Append(IRecordConfiguration)"/>
        new T Append(IRecordConfiguration recordConfiguration);

        /// <inheritdoc cref="IRecordDeclarationBuilder.Append(IRecordConfiguration, Action{IRecordBuilder})"/>
        new T Append(IRecordConfiguration recordConfiguration, Action<IRecordBuilder> builderFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : IRecordDeclarationBuilder
    {
        /// <inheritdoc/>
        IRecordDeclarationBuilder IRecordDeclarationBuilder.Append(IRecordConfiguration recordConfiguration)
            => Append(recordConfiguration, null);

        /// <inheritdoc/>
        IRecordDeclarationBuilder IRecordDeclarationBuilder.Append(IRecordConfiguration recordConfiguration, Action<IRecordBuilder> builderFunc)
            => Append(recordConfiguration, builderFunc);
    }
}