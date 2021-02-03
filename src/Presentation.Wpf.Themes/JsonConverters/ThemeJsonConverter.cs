using MaSch.Core.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Net;

namespace MaSch.Presentation.Wpf.JsonConverters
{
    /// <summary>
    /// <see cref="JsonConverter"/> that is used to convert a <see cref="ITheme"/> to and from json.
    /// </summary>
    /// <seealso cref="Newtonsoft.Json.JsonConverter{T}" />
    public class ThemeJsonConverter : JsonConverter<ITheme>
    {
        private static readonly IWebRequestCreate PackRequestFactory = new PackWebRequestFactory();
        private static readonly WebClient WebClient = new WebClient();

        /// <inheritdoc/>
        public override bool CanWrite => false;

        /// <inheritdoc/>
        public override ITheme ReadJson(JsonReader reader, Type objectType, ITheme existingValue, bool hasExtistingValue, JsonSerializer serializer)
        {
            var jToken = JToken.ReadFrom(reader);
            var result = serializer.Deserialize<Theme>(jToken.CreateReader());

            var mergedThemesToken = jToken["MergedThemes"];

            if (mergedThemesToken is JArray mergedThemesArray)
            {
                var baseUri = new Uri(jToken["BaseUri"] is JValue fpv ? (string)fpv.Value : AppDomain.CurrentDomain.BaseDirectory);

                foreach (var item in mergedThemesArray.OfType<JValue>().Where(x => x.Type == JTokenType.String))
                {
                    var json = DownloadString(new Uri((string)item.Value, UriKind.RelativeOrAbsolute), baseUri, out var fileUri);
                    foreach (var value in Theme.FromJson(json, new Uri(fileUri, ".").ToString()).Values)
                    {
                        if (!result.Values.ContainsKey(value.Key))
                            result.Values.Add(value.Key, value.Value);
                    }
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, ITheme value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds the BaseUri property to a json document.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <param name="baseUri">The base URI.</param>
        /// <returns>The json document with the attached BaseUri property.</returns>
        public static string AddBaseUriToJson(string json, string baseUri)
        {
            if (JToken.Parse(json) is JObject obj)
            {
                obj.Add("BaseUri", new JValue(baseUri));
                json = obj.ToString();
            }

            return json;
        }

        /// <summary>
        /// Downloads a string from a <see cref="Uri"/>. Supported Uri schemes are: http, https, file and pack.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="baseUri">The base URI.</param>
        /// <param name="absoluteUri">The absolute URI.</param>
        /// <returns>The downloaded string.</returns>
        /// <exception cref="NotSupportedException">Url with the Scheme is not supported.</exception>
        internal static string DownloadString(Uri uri, Uri baseUri, out Uri absoluteUri)
        {
            var uriToCheck = absoluteUri = GetAbsoluteUri(uri, baseUri);

            bool TestScheme(params string[] acceptedSchemes) => acceptedSchemes.Any(x => string.Equals(uriToCheck.Scheme, x, StringComparison.OrdinalIgnoreCase));

            string result;
            if (TestScheme(Uri.UriSchemeFile))
                result = File.ReadAllText(absoluteUri.LocalPath);
            else if (TestScheme(Uri.UriSchemeHttp, Uri.UriSchemeHttps))
                result = WebClient.DownloadString(absoluteUri);
            else if (TestScheme(PackUriHelper.UriSchemePack))
                result = ReadJsonFromPackUri(absoluteUri);
            else
                throw new NotSupportedException($"Url with the Scheme \"{absoluteUri.Scheme}\" is not supported.");

            return result;
        }

        private static Uri GetAbsoluteUri(Uri uri, Uri baseUri)
        {
            Uri result;
            if (uri.OriginalString.StartsWith("#DefaultThemes/", StringComparison.OrdinalIgnoreCase))
            {
                var themeName = uri.OriginalString.Split('/')[1];
                var allThemes = Enum.GetNames(typeof(DefaultTheme));
                if (!allThemes.Contains(themeName))
                {
                    if (allThemes.TryFirst(x => string.Equals(x, themeName, StringComparison.OrdinalIgnoreCase), out var actualName))
                        themeName = actualName;
                    else
                        throw new InvalidOperationException($"A default theme with the name \"{themeName}\" does not exist.");
                }

                result = new Uri($"pack://application:,,,/MaSch.Presentation.Wpf.Themes;component/Themes/{themeName}/Theme.json");
            }
            else
            {
                result = uri.IsAbsoluteUri ? uri : new Uri(baseUri, uri);
            }

            return result;
        }

        private static string ReadJsonFromPackUri(Uri packUri)
        {
            var request = PackRequestFactory.Create(packUri);
            using var response = request.GetResponse();
            using var reader = new StreamReader(response.GetResponseStream());
            return reader.ReadToEnd();
        }
    }
}
