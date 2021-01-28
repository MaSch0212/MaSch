using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using MaSch.Core.Extensions;

namespace MaSch.Presentation.Translation
{
    /// <summary>
    /// Represents a translation provider which is using .ini files for translation.
    /// </summary>
    /// <seealso cref="DictionaryTranslationProvider" />
    public class IniTranslationProvider : DictionaryTranslationProvider
    {
        #region Private Fields

        private static readonly Regex IniRegex =
            new Regex(@"^(?<Key>[^\[;=\s]*)\s*=\s*((?<Quote>[""'])(?<Value>(?:\k<Quote>{2}|(?!\k<Quote>).)*)(?:\k<Quote>)|(?<Value>[^\n]*))", RegexOptions.Multiline | RegexOptions.Singleline);

        private readonly string _translationFilesPath;
        private readonly string _fileName;
        private readonly string _fileExtension;
        private readonly IReadOnlyDictionary<string, string> _defaultLanguageValues;
        private readonly IDictionary<CultureInfo, DateTime> _loadedDates = new Dictionary<CultureInfo, DateTime>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IniTranslationProvider"/> class.
        /// </summary>
        /// <param name="translationFilePath">The file path in which the .ini files for translation are contained.</param>
        public IniTranslationProvider(string translationFilePath)
        {
            _translationFilesPath = Path.GetDirectoryName(translationFilePath);
            _fileName = Path.GetFileNameWithoutExtension(translationFilePath);
            _fileExtension = Path.GetExtension(translationFilePath);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IniTranslationProvider"/> class.
        /// </summary>
        /// <param name="translationFilePath">The file path in which the .ini files for translation are contained.</param>
        /// <param name="defaultLanguageIniContent">Content of the .ini file for the default language.</param>
        public IniTranslationProvider(string translationFilePath, string defaultLanguageIniContent)
            : this(translationFilePath)
        {
            _defaultLanguageValues = LoadIniContent(defaultLanguageIniContent);
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Looks for all languages for that at least one translation is available.
        /// </summary>
        /// <returns>
        /// Returns a list of languages that at least have one translation.
        /// </returns>
        public override IEnumerable<CultureInfo> GetAvailableLanguages()
        {
            static CultureInfo GetCulture(string ieft)
            {
                try
                {
                    return CultureInfo.GetCultureInfoByIetfLanguageTag(ieft);
                }
                catch
                {
                    return null;
                }
            }

            return from x in Directory.EnumerateFiles(_translationFilesPath, $"{_fileName}*{_fileExtension}", SearchOption.TopDirectoryOnly)
                   let match = Regex.Match(x, $".*{Regex.Escape(_fileName)}[.]?(?<lang>.*){_fileExtension}$")
                   where match.Success
                   let culture = GetCulture(match.Groups["lang"].Value)
                   where culture != null
                   select culture;
        }

        /// <summary>
        /// Gets the translation dictionary for a specified language.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="oldDict">The translation dictionary previously loaded. If no dictionary was loaded yet, this parameter is <c>null</c>.</param>
        /// <returns>The translation dictionary for the specified language.</returns>
        protected override IReadOnlyDictionary<string, string> GetDictionaryForLanguage(CultureInfo language, IReadOnlyDictionary<string, string> oldDict)
        {
            IReadOnlyDictionary<string, string> result = null;
            if (language.LCID == CultureInfo.InvariantCulture.LCID && _defaultLanguageValues != null)
            {
                result = _defaultLanguageValues;
            }
            else
            {
                var targetFile = Path.Combine(_translationFilesPath, _fileName + "." + (language.IetfLanguageTag + _fileExtension).TrimStart('.'));
                if (File.Exists(targetFile) && (oldDict == null || !_loadedDates.ContainsKey(language) || File.GetLastWriteTime(targetFile) > _loadedDates[language]))
                    result = LoadIniContent(File.ReadAllText(targetFile));
            }

            if (result != null)
                _loadedDates[language] = DateTime.Now;
            return result;
        }

        #endregion

        #region Private Functions

        private static IReadOnlyDictionary<string, string> LoadIniContent(string content)
        {
            var matches = from x in IniRegex.Matches(content).OfType<Match>()
                          let quote = x.Groups["Quote"].Value
                          let value = x.Groups["Value"].Value
                          select (key: x.Groups["Key"].Value, value: quote.In("\"", "'") ? value.Replace(quote + quote, quote) : value);
            return new ReadOnlyDictionary<string, string>(matches.ToDictionary(x => x.key, x => x.value));
        }

        #endregion
    }
}
