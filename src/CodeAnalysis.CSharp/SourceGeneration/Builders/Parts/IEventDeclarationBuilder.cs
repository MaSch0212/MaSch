using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    /// <summary>
    /// Represents a <see cref="ISourceBuilder"/> used to build an event.
    /// </summary>
    public interface IEventDeclarationBuilder : ISourceBuilder
    {
        /// <summary>
        /// Appends an event to the current context.
        /// </summary>
        /// <param name="eventConfiguration">The configuration of the event.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        IEventDeclarationBuilder Append(IEventConfiguration eventConfiguration);

        /// <summary>
        /// Appends an event with custom add and remove logic to the current context.
        /// </summary>
        /// <param name="eventConfiguration">The configuration of the event.</param>
        /// <param name="addBuilderFunc">The function to add content to the add method of the event.</param>
        /// <param name="removeBuilderFunc">The function to add content to the remove method of the event.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        IEventDeclarationBuilder Append(IEventConfiguration eventConfiguration, Action<ISourceBuilder> addBuilderFunc, Action<ISourceBuilder> removeBuilderFunc);
    }

    /// <inheritdoc cref="IEventDeclarationBuilder"/>
    /// <typeparam name="T">The type of <see cref="ISourceBuilder{T}"/>.</typeparam>
    public interface IEventDeclarationBuilder<T> : IEventDeclarationBuilder, ISourceBuilder<T>
        where T : IEventDeclarationBuilder<T>
    {
        /// <inheritdoc cref="IEventDeclarationBuilder.Append(IEventConfiguration)"/>
        new T Append(IEventConfiguration eventConfiguration);

        /// <inheritdoc cref="IEventDeclarationBuilder.Append(IEventConfiguration, Action{ISourceBuilder}, Action{ISourceBuilder})"/>
        new T Append(IEventConfiguration eventConfiguration, Action<ISourceBuilder> addBuilderFunc, Action<ISourceBuilder> removeBuilderFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : IEventDeclarationBuilder
    {
        /// <inheritdoc/>
        IEventDeclarationBuilder IEventDeclarationBuilder.Append(IEventConfiguration eventConfiguration)
            => Append(eventConfiguration, null, null);

        /// <inheritdoc/>
        IEventDeclarationBuilder IEventDeclarationBuilder.Append(IEventConfiguration eventConfiguration, Action<ISourceBuilder> addBuilderFunc, Action<ISourceBuilder> removeBuilderFunc)
            => Append(eventConfiguration, addBuilderFunc, removeBuilderFunc);
    }
}