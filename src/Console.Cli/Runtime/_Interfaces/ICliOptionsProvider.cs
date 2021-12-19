namespace MaSch.Console.Cli.Runtime;

/// <summary>
/// Represents a provider that provides the options of the currently executed application.
/// </summary>
public interface ICliOptionsProvider
{
    /// <summary>
    /// Provides a method to set the options of a <see cref="ICliOptionsProvider"/>.
    /// </summary>
    public interface ISettable
    {
        /// <summary>
        /// Sets the current options of the provider.
        /// </summary>
        /// <param name="options">The new options to use.</param>
        void SetOptions(object? options);
    }

    /// <summary>
    /// Gets the uncasted options object.
    /// </summary>
    object? Options { get; }

    /// <summary>
    /// Gets the options of a specific type.
    /// </summary>
    /// <typeparam name="TOptions">The type of options to retrieve.</typeparam>
    /// <returns>The casted options object.</returns>
    TOptions GetOptions<TOptions>()
        where TOptions : class;
}