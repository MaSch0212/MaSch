using MaSch.Core;
using MaSch.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MaSch.Presentation.Translation
{
    /// <summary>
    /// A default implementation for the ITranslationManager interface.
    /// </summary>
    public class TranslationManager : ITranslationManager
    {
        /// <summary>
        /// Occurs when the current language has changed.
        /// </summary>
        public event LanguageChangedEventHandler? LanguageChanged;

        private readonly IDictionary<string, ITranslationProvider> _registeredProviders;
        private CultureInfo? _currentLanguage;

        /// <summary>
        /// Gets the provider key of the default provider.
        /// </summary>
        public string DefaultProviderKey => "(default)";

        /// <summary>
        /// Gets or sets the current language. Default is the CultureInfo.CurrentUICulture. Set to null to use CultureInfo.CurrentUICulture.
        /// </summary>
        /// <value>
        /// The current language.
        /// </value>
        public CultureInfo CurrentLanguage
        {
            get => _currentLanguage ?? CultureInfo.CurrentUICulture;
            set
            {
                if (Equals(_currentLanguage, value))
                    return;
                var oldLang = _currentLanguage;
                _currentLanguage = value;
                LanguageChanged?.Invoke(this, new LanguageChangedEventArgs(oldLang, CurrentLanguage));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationManager"/> class.
        /// </summary>
        public TranslationManager()
            : this(null, (IEnumerable<INamedTranslationProvider>?)null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationManager"/> class.
        /// </summary>
        /// <param name="defaultProvider">The default provider to use.</param>
        public TranslationManager(ITranslationProvider? defaultProvider)
            : this(defaultProvider, (IEnumerable<INamedTranslationProvider>?)null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationManager"/> class.
        /// </summary>
        /// <param name="defaultProvider">The default provider to use.</param>
        /// <param name="providers">The named providers to register with this <see cref="TranslationManager"/>.</param>
        public TranslationManager(ITranslationProvider? defaultProvider, params INamedTranslationProvider[] providers)
            : this(defaultProvider, (IEnumerable<INamedTranslationProvider>)providers)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationManager"/> class.
        /// </summary>
        /// <param name="defaultProvider">The default provider to use.</param>
        /// <param name="providers">The named providers to register with this <see cref="TranslationManager"/>.</param>
        public TranslationManager(ITranslationProvider? defaultProvider, IEnumerable<INamedTranslationProvider>? providers)
        {
            _registeredProviders = new Dictionary<string, ITranslationProvider>();
            if (defaultProvider != null)
                RegisterDefaultTranslationProvider(defaultProvider);
            if (providers != null)
                _registeredProviders.Add(providers, x => x.ProviderKey);
        }

        /// <summary>
        /// Registers a new translation provider.
        /// </summary>
        /// <param name="provider">The provider to register.</param>
        /// <param name="providerKey">The provider key used to register the provider.</param>
        public void RegisterTranslationProvider(ITranslationProvider provider, string providerKey)
        {
            Guard.NotNull(provider, nameof(provider));
            _registeredProviders.Add(providerKey, provider);
        }

        /// <summary>
        /// Registers the default translation provider.
        /// </summary>
        /// <param name="provider">The provider to register as default provider.</param>
        public void RegisterDefaultTranslationProvider(ITranslationProvider provider) => RegisterTranslationProvider(provider, DefaultProviderKey);

        /// <summary>
        /// Translates a resource key with the default provider into the current language.
        /// </summary>
        /// <param name="resourceKey">The resource key to translate.</param>
        /// <returns>Returns the translated representation of the resource key with the default provider in the current language.</returns>
        public string GetTranslation(string resourceKey)
            => GetTranslation(resourceKey, DefaultProviderKey, CurrentLanguage);

        /// <summary>
        /// Translates a resource key with the default provider into a given language.
        /// </summary>
        /// <param name="resourceKey">The resource key to translate.</param>
        /// <param name="language">The language in which the resource key is translated in.</param>
        /// <returns>Returns the translated representation of the resource key with the default provider in the given language.</returns>
        public string GetTranslation(string resourceKey, CultureInfo language)
            => GetTranslation(resourceKey, DefaultProviderKey, language);

        /// <summary>
        /// Translates a resource key with a given provider into the current language.
        /// </summary>
        /// <param name="resourceKey">The resource key to translate.</param>
        /// <param name="providerKey">The provider that is used for translation.</param>
        /// <returns>Returns the translated representation of the resource key with the given provider in the current language.</returns>
        public string GetTranslation(string resourceKey, string providerKey)
            => GetTranslation(resourceKey, providerKey, CurrentLanguage);

        /// <summary>
        /// Translates a resource key with a given provider into a given language.
        /// </summary>
        /// <param name="resourceKey">The resource key to translate.</param>
        /// <param name="providerKey">The provider that is used for translation.</param>
        /// <param name="language">The language in which the resource key is translated in.</param>
        /// <returns>Returns the translated representation of the resource key with the given provider in the given language.</returns>
        public string GetTranslation(string resourceKey, string providerKey, CultureInfo language)
        {
            if (providerKey == null || !_registeredProviders.ContainsKey(providerKey))
                throw new ArgumentException($"A provider with the key \"{providerKey}\" was not registered yet.");
            return _registeredProviders[providerKey].GetTranslation(resourceKey, language ?? CurrentLanguage);
        }

        /// <summary>
        /// Translates multiple resource keys with the default provider into the current language.
        /// </summary>
        /// <param name="resourceKeys">The resource keys to translate.</param>
        /// <returns>Returns the translated representations of the resource keys with the default provider in the current language.</returns>
        public IReadOnlyDictionary<string, string> GetTranslations(IEnumerable<string> resourceKeys)
            => GetTranslations(resourceKeys, DefaultProviderKey, CurrentLanguage);

        /// <summary>
        /// Translates multiple resource keys with the default provider into a given language.
        /// </summary>
        /// <param name="resourceKeys">The resource keys to translate.</param>
        /// <param name="language">The language in which the resource keys are translated in.</param>
        /// <returns>Returns the translated representations of the resource keys with the default provider in the given language.</returns>
        public IReadOnlyDictionary<string, string> GetTranslations(IEnumerable<string> resourceKeys, CultureInfo language)
            => GetTranslations(resourceKeys, DefaultProviderKey, language);

        /// <summary>
        /// Translates multiple resource keys with a given provider into the current language.
        /// </summary>
        /// <param name="resourceKeys">The resource keys to translate.</param>
        /// <param name="providerKey">The provider that is used for translation.</param>
        /// <returns>Returns the translated representations of the resource keys with the given provider in the current language.</returns>
        public IReadOnlyDictionary<string, string> GetTranslations(IEnumerable<string> resourceKeys, string providerKey)
            => GetTranslations(resourceKeys, providerKey, CurrentLanguage);

        /// <summary>
        /// Translates multiple resource keys with a given provider into a given language.
        /// </summary>
        /// <param name="resourceKeys">The resource keys to translate.</param>
        /// <param name="providerKey">The provider that is used for translation.</param>
        /// <param name="language">The language in which the resource keys are translated in.</param>
        /// <returns>Returns the translated representations of the resource keys with the given provider in the given language.</returns>
        public IReadOnlyDictionary<string, string> GetTranslations(IEnumerable<string> resourceKeys, string providerKey, CultureInfo language)
        {
            if (providerKey == null || !_registeredProviders.ContainsKey(providerKey))
                throw new ArgumentException($"A provider with the key \"{providerKey}\" was not registered yet.");
            return _registeredProviders[providerKey].GetTranslations(resourceKeys, language ?? CurrentLanguage);
        }

        /// <summary>
        /// Translates all resource keys of the default provider into a given language.
        /// </summary>
        /// <param name="language">The language in which the resource keys are translated in.</param>
        /// <returns>Returns the translated representations of all resource keys of the default provider in the given language.</returns>
        public IReadOnlyDictionary<string, string> GetAllTranslations(CultureInfo language)
            => GetAllTranslations(DefaultProviderKey, language);

        /// <summary>
        /// Translates all resource keys of a given provider into the current language.
        /// </summary>
        /// <param name="providerKey">The provider that is used for translation.</param>
        /// <returns>Returns the translated representations of all resource keys of the given provider in the current language.</returns>
        public IReadOnlyDictionary<string, string> GetAllTranslations(string providerKey)
            => GetAllTranslations(providerKey, CurrentLanguage);

        /// <summary>
        /// Translates all resource keys of a given provider into a given language.
        /// </summary>
        /// <param name="providerKey">The provider that is used for translation.</param>
        /// <param name="language">The language in which the resource keys are translated in.</param>
        /// <returns>Returns the translated representations of all resource keys of the given provider in the given language.</returns>
        public IReadOnlyDictionary<string, string> GetAllTranslations(string providerKey, CultureInfo language)
        {
            if (providerKey == null || !_registeredProviders.ContainsKey(providerKey))
                throw new ArgumentException($"A provider with the key \"{providerKey}\" was not registered yet.");
            return _registeredProviders[providerKey].GetAllTranslations(language ?? CurrentLanguage);
        }

        /// <summary>
        /// Checks if a translation of the given resource key exists in the current language of the default provider.
        /// </summary>
        /// <param name="resourceKey">The resource key.</param>
        /// <returns>Returns true if the translated representation of the resource key exists with the default provider in the current language. Otherwise false.</returns>
        public bool IsTranslationDefined(string resourceKey)
            => IsTranslationDefined(resourceKey, DefaultProviderKey, CurrentLanguage);

        /// <summary>
        /// Checks if a translation of the given resource key exists in the given language of the default provider.
        /// </summary>
        /// <param name="resourceKey">The resource key.</param>
        /// <param name="language">The language.</param>
        /// <returns>Returns true if the translated representation of the resource key exists with the default provider in the given language. Otherwise false.</returns>
        public bool IsTranslationDefined(string resourceKey, CultureInfo language)
            => IsTranslationDefined(resourceKey, DefaultProviderKey, language);

        /// <summary>
        /// Checks if a translation of the given resource key exists in the current language of the provider with the given key.
        /// </summary>
        /// <param name="resourceKey">The resource key.</param>
        /// <param name="providerKey">The provider.</param>
        /// <returns>Returns true if the translated representation of the resource key exists ith the given provider in the current language. Otherwise false.</returns>
        public bool IsTranslationDefined(string resourceKey, string providerKey)
            => IsTranslationDefined(resourceKey, providerKey, CurrentLanguage);

        /// <summary>
        /// Checks if a translation of the given resource key exists in the given language of the provider with the given key.
        /// </summary>
        /// <param name="resourceKey">The resource key.</param>
        /// <param name="providerKey">The provider.</param>
        /// <param name="language">The language.</param>
        /// <returns>Returns true if the translated representation of the resource key exists with the given provider in the given language. Otherwise false.</returns>
        public bool IsTranslationDefined(string resourceKey, string providerKey, CultureInfo language)
        {
            if (providerKey == null || !_registeredProviders.ContainsKey(providerKey))
                throw new ArgumentException($"A provider with the key \"{providerKey}\" was not registered yet.");
            return _registeredProviders[providerKey].IsTranslationDefined(resourceKey, language ?? CurrentLanguage);
        }

        /// <summary>
        /// Looks for all languages for that at least one translation is available in any provider.
        /// </summary>
        /// <returns>Returns a list of languages that at least have one translation.</returns>
        public IEnumerable<CultureInfo> GetAvailableLanguages()
            => _registeredProviders.SelectMany(x => x.Value.GetAvailableLanguages()).Select(x => x.LCID).Distinct().Select(CultureInfo.GetCultureInfo);

        /// <summary>
        /// Looks for all languages for that at least one translation is available in the given provider.
        /// </summary>
        /// <param name="providerKey">The provider.</param>
        /// <returns>Returns a list of languages that at least have one translation.</returns>
        public IEnumerable<CultureInfo> GetAvailableLanguages(string providerKey)
        {
            if (providerKey == null || !_registeredProviders.ContainsKey(providerKey))
                throw new ArgumentException($"A provider with the key \"{providerKey}\" was not registered yet.");
            return _registeredProviders[providerKey].GetAvailableLanguages();
        }
    }
}
