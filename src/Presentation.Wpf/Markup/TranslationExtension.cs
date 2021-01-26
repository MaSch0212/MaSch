﻿using System;
using System.Globalization;
using MaSch.Common.Helper;
using MaSch.Presentation.Translation;

namespace MaSch.Presentation.Wpf.Markup
{
    /// <summary>
    /// Is used for translation of a string with a <see cref="ITranslationManager"/>. 
    /// You need to reference the Microsoft.Practices.ServiceLocator Assembly and have to initialize a ServiceLocator to use this markup extension
    /// </summary>
    /// <seealso cref="UpdateableMarkupExtension" />
    public class TranslationExtension : UpdateableMarkupExtension
    {
        private ITranslationManager _translationManager;

        #region Properties

        /// <summary>
        /// Gets or sets the resource key.
        /// </summary>
        /// <value>
        /// The resource key.
        /// </value>
        public string ResourceKey { get; set; }

        /// <summary>
        /// Gets or sets the provider key.
        /// </summary>
        /// <value>
        /// The provider key.
        /// </value>
        public string ProviderKey { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        public CultureInfo Language { get; set; }

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationExtension"/> class.
        /// </summary>
        public TranslationExtension() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationExtension"/> class.
        /// </summary>
        /// <param name="resourceKey">The resource key of the translation.</param>
        public TranslationExtension(string resourceKey) : this()
        {
            ResourceKey = resourceKey;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationExtension"/> class.
        /// </summary>
        /// <param name="resourceKey">The resource key of the translation.</param>
        /// <param name="providerKey">The key of the provider that should be used.</param>
        public TranslationExtension(string resourceKey, string providerKey) : this(resourceKey)
        {
            ProviderKey = providerKey;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationExtension"/> class.
        /// </summary>
        /// <param name="resourceKey">The resource key of the translation.</param>
        /// <param name="providerKey">The key of the provider that should be used.</param>
        /// <param name="language">The language in which should be translated in.</param>
        public TranslationExtension(string resourceKey, string providerKey, CultureInfo language) : this(resourceKey, providerKey)
        {
            Language = language;
        }

        #endregion

        private void TranslationManager_LanguageChanged(object sender, LanguageChangedEventArgs e)
        {
            if(Language == null)
                UpdateValue(ProvideValueInternal(null));
        }

        /// <summary>
        /// Provides the value internal.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        protected override object ProvideValueInternal(IServiceProvider serviceProvider)
        {
            if (_translationManager == null)
            {
                try { _translationManager = ServiceLocatorHelper.GetInstance<ITranslationManager>(); }
                catch (NullReferenceException) { }

                if (_translationManager != null)
                    _translationManager.LanguageChanged += TranslationManager_LanguageChanged;
            }

            if (_translationManager == null)
                return $"### {ResourceKey} ###";
            if (string.IsNullOrEmpty(ProviderKey))
                return Language == null ? _translationManager.GetTranslation(ResourceKey) : _translationManager.GetTranslation(ResourceKey, Language);
            return Language == null ? _translationManager.GetTranslation(ResourceKey, ProviderKey) : _translationManager.GetTranslation(ResourceKey, ProviderKey, Language);
        }
    }
}
