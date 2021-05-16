using MaSch.Core.Helper;
using System;
using System.ComponentModel.DataAnnotations;

namespace MaSch.Presentation.Translation.Validation
{
    /// <summary>
    /// Base class for translatable validation attributes.
    /// </summary>
    /// <seealso cref="ValidationAttribute" />
    public abstract class TranslatableValidationAttribute : ValidationAttribute
    {
        /// <summary>
        /// Gets or sets the translation provider key which is used for translation.
        /// </summary>
        /// <value>
        /// The translation provider key which is used for translation.
        /// </value>
        public string? TranslationProviderKey { get; set; }

        /// <summary>
        /// Gets the translated error message.
        /// </summary>
        /// <returns>A translated representation of the <see cref="ValidationAttribute.ErrorMessageResourceName"/> property value.</returns>
        public string GetTranslatedErrorMessage()
        {
            if (ErrorMessageResourceName == null)
                return string.Empty;
            return GetTranslatedErrorMessage(ErrorMessageResourceName);
        }

        /// <summary>
        /// Gets the translated error message.
        /// </summary>
        /// <param name="errorMessageResourceName">Name of the error message resource.</param>
        /// <returns>A translated representation of the specified error message.</returns>
        /// <exception cref="InvalidOperationException">The TranslationsManager was not found in the service locator.</exception>
        public string GetTranslatedErrorMessage(string errorMessageResourceName)
        {
            var translationManager = ServiceLocatorHelper.GetInstance<ITranslationManager>();
            if (translationManager == null)
                throw new InvalidOperationException("The TranslationsManager was not found in the service locator.");
            return string.IsNullOrEmpty(TranslationProviderKey)
                ? translationManager.GetTranslation(errorMessageResourceName)
                : translationManager.GetTranslation(errorMessageResourceName, TranslationProviderKey);
        }
    }
}