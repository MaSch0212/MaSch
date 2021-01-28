namespace MaSch.Presentation.Translation
{
    /// <summary>
    /// Represents a <see cref="ITranslationProvider"/> that is named.
    /// </summary>
    /// <seealso cref="ITranslationProvider" />
    public interface INamedTranslationProvider : ITranslationProvider
    {
        /// <summary>
        /// Gets the key for this provider.
        /// </summary>
        string ProviderKey { get; }
    }
}
