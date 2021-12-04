namespace MaSch.Presentation.Translation;

/// <summary>
/// Defines a translation manager in which resource keys can be translated with translation providers in different languages.
/// </summary>
public interface ITranslationManager
{
    /// <summary>
    /// Occurs when the current language has changed.
    /// </summary>
    event LanguageChangedEventHandler LanguageChanged;

    /// <summary>
    /// Gets or sets the current language.
    /// </summary>
    /// <value>
    /// The current language.
    /// </value>
    CultureInfo CurrentLanguage { get; set; }

    /// <summary>
    /// Gets the key of the default translation provider.
    /// </summary>
    string DefaultProviderKey { get; }

    /// <summary>
    /// Registers a new translation provider.
    /// </summary>
    /// <param name="provider">The provider to register.</param>
    /// <param name="providerKey">The provider key used to register the provider.</param>
    void RegisterTranslationProvider(ITranslationProvider provider, string providerKey);

    /// <summary>
    /// Registers the default translation provider.
    /// </summary>
    /// <param name="provider">The provider to register as default provider.</param>
    void RegisterDefaultTranslationProvider(ITranslationProvider provider);

    /// <summary>
    /// Translates a resource key with the default provider into the current language.
    /// </summary>
    /// <param name="resourceKey">The resource key to translate.</param>
    /// <returns>Returns the translated representation of the resource key with the default provider in the current language.</returns>
    string GetTranslation(string resourceKey);

    /// <summary>
    /// Translates a resource key with the default provider into a given language.
    /// </summary>
    /// <param name="resourceKey">The resource key to translate.</param>
    /// <param name="language">The language in which the resource key is translated in.</param>
    /// <returns>Returns the translated representation of the resource key with the default provider in the given language.</returns>
    string GetTranslation(string resourceKey, CultureInfo language);

    /// <summary>
    /// Translates a resource key with a given provider into the current language.
    /// </summary>
    /// <param name="resourceKey">The resource key to translate.</param>
    /// <param name="providerKey">The provider that is used for translation.</param>
    /// <returns>Returns the translated representation of the resource key with the given provider in the current language.</returns>
    string GetTranslation(string resourceKey, string providerKey);

    /// <summary>
    /// Translates a resource key with a given provider into a given language.
    /// </summary>
    /// <param name="resourceKey">The resource key to translate.</param>
    /// <param name="providerKey">The provider that is used for translation.</param>
    /// <param name="language">The language in which the resource key is translated in.</param>
    /// <returns>Returns the translated representation of the resource key with the given provider in the given language.</returns>
    string GetTranslation(string resourceKey, string providerKey, CultureInfo language);

    /// <summary>
    /// Translates multiple resource keys with the default provider into the current language.
    /// </summary>
    /// <param name="resourceKeys">The resource keys to translate.</param>
    /// <returns>Returns the translated representations of the resource keys with the default provider in the current language.</returns>
    IReadOnlyDictionary<string, string> GetTranslations(IEnumerable<string> resourceKeys);

    /// <summary>
    /// Translates multiple resource keys with the default provider into a given language.
    /// </summary>
    /// <param name="resourceKeys">The resource keys to translate.</param>
    /// <param name="language">The language in which the resource keys are translated in.</param>
    /// <returns>Returns the translated representations of the resource keys with the default provider in the given language.</returns>
    IReadOnlyDictionary<string, string> GetTranslations(IEnumerable<string> resourceKeys, CultureInfo language);

    /// <summary>
    /// Translates multiple resource keys with a given provider into the current language.
    /// </summary>
    /// <param name="resourceKeys">The resource keys to translate.</param>
    /// <param name="providerKey">The provider that is used for translation.</param>
    /// <returns>Returns the translated representations of the resource keys with the given provider in the current language.</returns>
    IReadOnlyDictionary<string, string> GetTranslations(IEnumerable<string> resourceKeys, string providerKey);

    /// <summary>
    /// Translates multiple resource keys with a given provider into a given language.
    /// </summary>
    /// <param name="resourceKeys">The resource keys to translate.</param>
    /// <param name="providerKey">The provider that is used for translation.</param>
    /// <param name="language">The language in which the resource keys are translated in.</param>
    /// <returns>Returns the translated representations of the resource keys with the given provider in the given language.</returns>
    IReadOnlyDictionary<string, string> GetTranslations(IEnumerable<string> resourceKeys, string providerKey, CultureInfo language);

    /// <summary>
    /// Translates all resource keys of the default provider into a given language.
    /// </summary>
    /// <param name="language">The language in which the resource keys are translated in.</param>
    /// <returns>Returns the translated representations of all resource keys of the default provider in the given language.</returns>
    IReadOnlyDictionary<string, string> GetAllTranslations(CultureInfo language);

    /// <summary>
    /// Translates all resource keys of a given provider into the current language.
    /// </summary>
    /// <param name="providerKey">The provider that is used for translation.</param>
    /// <returns>Returns the translated representations of all resource keys of the given provider in the current language.</returns>
    IReadOnlyDictionary<string, string> GetAllTranslations(string providerKey);

    /// <summary>
    /// Translates all resource keys of a given provider into a given language.
    /// </summary>
    /// <param name="providerKey">The provider that is used for translation.</param>
    /// <param name="language">The language in which the resource keys are translated in.</param>
    /// <returns>Returns the translated representations of all resource keys of the given provider in the given language.</returns>
    IReadOnlyDictionary<string, string> GetAllTranslations(string providerKey, CultureInfo language);

    /// <summary>
    /// Checks if a translation of the given resource key exists in the current language of the default provider.
    /// </summary>
    /// <param name="resourceKey">The resource key.</param>
    /// <returns>Returns true if the translated representation of the resource key exists with the default provider in the current language. Otherwise false.</returns>
    bool IsTranslationDefined(string resourceKey);

    /// <summary>
    /// Checks if a translation of the given resource key exists in the given language of the default provider.
    /// </summary>
    /// <param name="resourceKey">The resource key.</param>
    /// <param name="language">The language.</param>
    /// <returns>Returns true if the translated representation of the resource key exists with the default provider in the given language. Otherwise false.</returns>
    bool IsTranslationDefined(string resourceKey, CultureInfo language);

    /// <summary>
    /// Checks if a translation of the given resource key exists in the current language of the provider with the given key.
    /// </summary>
    /// <param name="resourceKey">The resource key.</param>
    /// <param name="providerKey">The provider.</param>
    /// <returns>Returns true if the translated representation of the resource key exists ith the given provider in the current language. Otherwise false.</returns>
    bool IsTranslationDefined(string resourceKey, string providerKey);

    /// <summary>
    /// Checks if a translation of the given resource key exists in the given language of the provider with the given key.
    /// </summary>
    /// <param name="resourceKey">The resource key.</param>
    /// <param name="providerKey">The provider.</param>
    /// <param name="language">The language.</param>
    /// <returns>Returns true if the translated representation of the resource key exists with the given provider in the given language. Otherwise false.</returns>
    bool IsTranslationDefined(string resourceKey, string providerKey, CultureInfo language);

    /// <summary>
    /// Looks for all languages for that at least one translation is available in the default provider.
    /// </summary>
    /// <returns>Returns a list of languages that at least have one translation.</returns>
    IEnumerable<CultureInfo> GetAvailableLanguages();

    /// <summary>
    /// Looks for all languages for that at least one translation is available in the given provider.
    /// </summary>
    /// <param name="providerKey">The provider.</param>
    /// <returns>Returns a list of languages that at least have one translation.</returns>
    IEnumerable<CultureInfo> GetAvailableLanguages(string providerKey);
}
