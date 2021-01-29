using MaSch.Core.Observable.Collections;
using MaSch.Presentation.Wpf.JsonConverters;
using Newtonsoft.Json;

namespace MaSch.Presentation.Wpf
{
    /// <summary>
    /// Provides methods to handle a theme.
    /// </summary>
    [JsonConverter(typeof(ThemeJsonConverter))]
    public interface ITheme
    {
        /// <summary>
        /// Gets or sets the name of the theme.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the theme.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets the theme values.
        /// </summary>
        ObservableDictionary<string, IThemeValue> Values { get; }

        /// <summary>
        /// Saves the theme to a file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        void SaveToFile(string filePath);

        /// <summary>
        /// Converts the theme to json.
        /// </summary>
        /// <returns>Json representation of the theme.</returns>
        string ToJson();
    }
}
