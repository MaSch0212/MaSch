using System;
using System.Globalization;

namespace MaSch.Presentation.Translation
{
    /// <summary>
    /// Represents the method that handles the <see cref="ITranslationManager"/>.LanguageChanged event of a class that implements the <see cref="ITranslationManager"/> interface.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="LanguageChangedEventArgs"/> instance containing the event data.</param>
    public delegate void LanguageChangedEventHandler(object sender, LanguageChangedEventArgs e);

    /// <summary>
    /// Provides data for the <see cref="ITranslationManager"/>.LanguageChanged event.
    /// </summary>
    /// <seealso cref="EventArgs" />
    public class LanguageChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageChangedEventArgs"/> class.
        /// </summary>
        /// <param name="oldLanguage">The old language.</param>
        /// <param name="newLanguage">The new language.</param>
        public LanguageChangedEventArgs(CultureInfo? oldLanguage, CultureInfo newLanguage)
        {
            NewLanguage = newLanguage;
            OldLanguage = oldLanguage;
        }

        /// <summary>
        /// Gets the new language.
        /// </summary>
        /// <value>
        /// The new language.
        /// </value>
        public CultureInfo NewLanguage { get; }

        /// <summary>
        /// Gets the old language.
        /// </summary>
        /// <value>
        /// The old language.
        /// </value>
        public CultureInfo? OldLanguage { get; }
    }
}
