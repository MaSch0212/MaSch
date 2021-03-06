using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MaSch.Core.Extensions;

namespace MaSch.Presentation.Translation
{
    /// <summary>
    /// Represents a method that will handle the loading of the language dictionary for the specified language.
    /// </summary>
    /// <param name="language">The language.</param>
    /// <param name="oldDict">The dictionary that already has been loaded. If no dictionary has been loaded yet, this value is <c>null</c>.</param>
    /// <returns>The dictionary which contains translations for the specified language.</returns>
    public delegate IReadOnlyDictionary<string, string> RetrieveDictionaryHandler(CultureInfo language, IReadOnlyDictionary<string, string>? oldDict);

    /// <summary>
    /// Represents a translation provider which is using a dictionary for translation.
    /// </summary>
    /// <seealso cref="ITranslationProvider" />
    public class DictionaryTranslationProvider : ITranslationProvider
    {
        private readonly IDictionary<CultureInfo, IReadOnlyDictionary<string, string>?> _translationCache = new Dictionary<CultureInfo, IReadOnlyDictionary<string, string>?>();

        /// <summary>
        /// Gets the handler for dictionary loading.
        /// </summary>
        protected RetrieveDictionaryHandler? Handler { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryTranslationProvider"/> class.
        /// </summary>
        protected DictionaryTranslationProvider()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryTranslationProvider"/> class.
        /// </summary>
        /// <param name="handler">The handler for dictionary loading.</param>
        public DictionaryTranslationProvider(RetrieveDictionaryHandler? handler)
        {
            Handler = handler;
        }

        /// <summary>
        /// Translates a resource key into a given language.
        /// </summary>
        /// <param name="resourceKey">The resource key to translate.</param>
        /// <param name="language">The language in which the resource key is translated in.</param>
        /// <returns>
        /// Returns a translated representation of the given resource key in the given language.
        /// </returns>
        public string GetTranslation(string resourceKey, CultureInfo language)
        {
            return GetTranslationInternal(resourceKey, language, null);
        }

        /// <summary>
        /// Translates multiple resource keys into a given language.
        /// </summary>
        /// <param name="resourceKeys">The resource keys to translate.</param>
        /// <param name="language">The language in which the resource keys are translated in.</param>
        /// <returns>
        /// Returns the translated representations of the resource keys in the given language.
        /// </returns>
        public IReadOnlyDictionary<string, string> GetTranslations(IEnumerable<string> resourceKeys, CultureInfo language)
        {
            var cache = new Dictionary<CultureInfo, IReadOnlyDictionary<string, string>>();
            return resourceKeys.ToDictionary(x => x, x => GetTranslationInternal(x, language, cache));
        }

        /// <summary>
        /// Translates all resource keys into a given language.
        /// </summary>
        /// <param name="language">The language in which the resource keys are translated in.</param>
        /// <returns>
        /// Returns the translated representations of all resource keys in the given language.
        /// </returns>
        public IReadOnlyDictionary<string, string> GetAllTranslations(CultureInfo language)
        {
            var result = new Dictionary<string, string>();
            var c = language;
            do
            {
                var dict = GetDictionaryForLanguageInternal(c);
                if (dict != null)
                    result.AddIfNotExists(dict);
            }
            while (c.LCID != CultureInfo.InvariantCulture.LCID);
            return result;
        }

        /// <summary>
        /// Checks if a translation of the given resource key exists in the given language.
        /// </summary>
        /// <param name="resourceKey">The resource key to translate.</param>
        /// <param name="language">The language in which the resource key is translated in.</param>
        /// <returns>
        /// Returns <c>true</c> if a translated representation of the given resource key exists in the given language; otherwise <c>false</c>.
        /// </returns>
        public bool IsTranslationDefined(string resourceKey, CultureInfo language)
        {
            var c = language;
            do
            {
                var dict = GetDictionaryForLanguageInternal(c);
                if (dict?.ContainsKey(resourceKey) == true)
                    return true;
            }
            while (c.LCID != CultureInfo.InvariantCulture.LCID);
            return false;
        }

        /// <summary>
        /// Looks for all languages for that at least one translation is available.
        /// </summary>
        /// <returns>
        /// Returns a list of languages that at least have one translation.
        /// </returns>
        public virtual IEnumerable<CultureInfo> GetAvailableLanguages()
        {
            return Array.Empty<CultureInfo>();
        }

        /// <summary>
        /// Gets the translation dictionary for a specified language.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="oldDict">The translation dictionary previously loaded. If no dictionary was loaded yet, this parameter is <c>null</c>.</param>
        /// <returns>The translation dictionary for the specified language.</returns>
        protected virtual IReadOnlyDictionary<string, string>? GetDictionaryForLanguage(CultureInfo language, IReadOnlyDictionary<string, string>? oldDict)
        {
            return Handler?.Invoke(language, oldDict);
        }

        private string GetTranslationInternal(string resourceKey, CultureInfo language, IDictionary<CultureInfo, IReadOnlyDictionary<string, string>>? cache)
        {
            IReadOnlyDictionary<string, string>? dict;
            if (cache == null || !cache.ContainsKey(language))
            {
                dict = GetDictionaryForLanguageInternal(language);
                if (dict != null)
                    cache?.Add(language, dict);
            }
            else
            {
                dict = cache[language];
            }

            if (dict?.ContainsKey(resourceKey) == true)
                return dict[resourceKey];
            if (language.LCID != CultureInfo.InvariantCulture.LCID)
                return GetTranslationInternal(resourceKey, language.Parent, cache);
            return $"### {resourceKey} ###";
        }

        private IReadOnlyDictionary<string, string>? GetDictionaryForLanguageInternal(CultureInfo language)
        {
            var oldDict = _translationCache.TryGetValue(language);
            var newDict = GetDictionaryForLanguage(language, oldDict);
            if (newDict == null)
                _translationCache.TryRemove(language);
            if (oldDict != newDict)
                _translationCache[language] = newDict;
            return newDict;
        }
    }
}