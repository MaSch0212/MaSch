using System.Collections.Generic;
using System.Globalization;

namespace MaSch.Presentation.Translation
{
    /// <summary>
    /// Represents a class that provides translations to classes that implement the <see cref="ITranslationManager"/> interface
    /// </summary>
    public interface ITranslationProvider
    {
        /// <summary>
        /// Translates a resource key into a given language.
        /// </summary>
        /// <param name="resourceKey">The resource key to translate.</param>
        /// <param name="language">The language in which the resource key is translated in.</param>
        /// <returns>Returns a translated representation of the given resource key in the given language.</returns>
        string GetTranslation(string resourceKey, CultureInfo language);

        /// <summary>
        /// Translates multiple resource keys into a given language.
        /// </summary>
        /// <param name="resourceKeys">The resource keys to translate.</param>
        /// <param name="language">The language in which the resource keys are translated in.</param>
        /// <returns>Returns the translated representations of the resource keys in the given language.</returns>
        IReadOnlyDictionary<string, string> GetTranslations(IEnumerable<string> resourceKeys, CultureInfo language);

        /// <summary>
        /// Translates all resource keys into a given language.
        /// </summary>
        /// <param name="language">The language in which the resource keys are translated in.</param>
        /// <returns>Returns the translated representations of all resource keys in the given language.</returns>
        IReadOnlyDictionary<string, string> GetAllTranslations(CultureInfo language);

        /// <summary>
        /// Checks if a translation of the given resource key exists in the given language.
        /// </summary>
        /// <param name="resourceKey">The resource key to translate.</param>
        /// <param name="language">The language in which the resource key is translated in.</param>
        /// Returns <c>true</c> if a translated representation of the given resource key exists in the given language; otherwise <c>false</c>.
        bool IsTranslationDefined(string resourceKey, CultureInfo language);

        /// <summary>
        /// Looks for all languages for that at least one translation is available.
        /// </summary>
        /// <returns>Returns a list of languages that at least have one translation.</returns>
        IEnumerable<CultureInfo> GetAvailableLanguages();
    }
}
