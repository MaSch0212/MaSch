namespace MaSch.Core.Logging
{
    /// <summary>
    /// Describes a logging service.
    /// </summary>
    /// <seealso cref="ILoggingProvider" />
    public interface ILoggingService : ILoggingProvider
    {
        /// <summary>
        /// Gets a value indicating whether this instance has any logging providers.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has any logging providers; otherwise, <c>false</c>.
        /// </value>
        bool HasLoggingProvider { get; }

        /// <summary>
        /// Adds a specified logging provider.
        /// </summary>
        /// <param name="provider">The provider to add.</param>
        /// <returns><see langword="true"/> if the provider was added successfully; otherwise, <see langword="false"/>.</returns>
        bool AddLoggingProvider(ILoggingProvider provider);

        /// <summary>
        /// Removes a specified logging provider.
        /// </summary>
        /// <param name="provider">The provider to remove.</param>
        /// <returns><see langword="true"/> if the provider was removed successfully; otherwise, <see langword="false"/>.</returns>
        bool RemoveLoggingProvider(ILoggingProvider provider);
    }
}
