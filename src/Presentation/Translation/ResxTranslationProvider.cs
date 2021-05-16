using MaSch.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace MaSch.Presentation.Translation
{
    /// <summary>
    /// Represents a translation provider for Resource-Files (*.resx).
    /// </summary>
    /// <seealso cref="ITranslationProvider" />
    public class ResxTranslationProvider : ITranslationProvider
    {
        private readonly Type _type;
        private readonly ResourceManager _resourceManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResxTranslationProvider"/> class.
        /// </summary>
        /// <param name="resxType">Type of the resource class.</param>
        /// <exception cref="ArgumentException">The given type has to have a static Property with a Type of <see cref="ResourceManager"/>.</exception>
        public ResxTranslationProvider(Type resxType)
        {
            var resManProperty = resxType.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .FirstOrDefault(x => typeof(ResourceManager).IsAssignableFrom(x.PropertyType));
            if (resManProperty == null)
                throw new ArgumentException($"The given type has to have a static Property with a Type of {typeof(ResourceManager).FullName}.");
            _resourceManager = resManProperty.GetValue(null) as ResourceManager ?? throw new ArgumentException($"Could not retrieve ResourceManager from type {typeof(ResourceManager).FullName}.");
            _type = resxType;
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
            return _resourceManager.GetString(resourceKey, language) ?? $"### {resourceKey} ###";
        }

        /// <summary>
        /// Translates multiple resource keys into a given language.
        /// </summary>
        /// <param name="resourceKeys">he resource keys to translate.</param>
        /// <param name="language">The language in which the resource keys are translated in.</param>
        /// <returns>
        /// Returns the translated representations of the resource keys in the given language.
        /// </returns>
        public IReadOnlyDictionary<string, string> GetTranslations(IEnumerable<string> resourceKeys, CultureInfo language)
        {
            return resourceKeys.ToDictionary(x => x, x => GetTranslation(x, language));
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
            var properties = _type.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(x => x.PropertyType == typeof(string));
            return GetTranslations(properties.Select(x => x.Name), language);
        }

        /// <summary>
        /// Check if a translation of the given resource key exists in the given language.
        /// </summary>
        /// <param name="resourceKey">The resource key to translate.</param>
        /// <param name="language">The language in which the resource key is translated in.</param>
        /// <returns>Returns true if a translated representation of the given resource key exists in the given language. Otherwise false.</returns>
        public bool IsTranslationDefined(string resourceKey, CultureInfo language)
        {
            return _resourceManager.GetString(resourceKey, language) != null;
        }

        /// <summary>
        /// Looks for all languages for that at least one translation is available.
        /// </summary>
        /// <returns>Returns a list of languages that at least have one translation.</returns>
        public IEnumerable<CultureInfo> GetAvailableLanguages()
        {
            var baseDir = Path.GetDirectoryName(_type.Assembly.Location) ?? throw new InvalidOperationException();
            foreach (var dir in Directory.GetDirectories(baseDir))
            {
                CultureInfo? info = null;
                try
                {
                    var dirName = Path.GetFileName(dir) ?? throw new InvalidOperationException();
                    info = CultureInfo.GetCultureInfoByIetfLanguageTag(dirName);
                }
                catch (CultureNotFoundException)
                {
                }

                if (info != null && File.Exists(Path.Combine(dir, _type.Assembly.GetName().Name + ".resources.dll")))
                    yield return info;
            }

            yield return CultureInfo.InvariantCulture;
        }
    }
}
