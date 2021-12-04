using MaSch.Presentation.Wpf.Models;
using Newtonsoft.Json.Linq;

namespace MaSch.Presentation.Wpf.JsonConverters;

/// <summary>
/// Base class for the <see cref="ThemeValuePropertyJsonConverter{T}"/>.
/// </summary>
/// <seealso cref="JsonConverter" />
public abstract class ThemeValuePropertyJsonConverter : JsonConverter
{
    /// <summary>
    /// The regex that is used to get references to other values in the theme.
    /// </summary>
    protected static readonly Regex ReferenceRegex = new(@"^\{Bind (?<key>[a-zA-Z_\-][a-zA-Z0-9_\-]*)(\.(?<property>[a-zA-Z_\-][a-zA-Z0-9_\-]*))?\}$", RegexOptions.Compiled);
}

/// <summary>
/// <see cref="JsonConverter"/> that is used to convert a property of a <see cref="IThemeValue"/> to and from json.
/// </summary>
/// <typeparam name="T">The type of the property.</typeparam>
/// <seealso cref="JsonConverter" />
[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Generic representation can be in same file")]
public class ThemeValuePropertyJsonConverter<T> : ThemeValuePropertyJsonConverter
{
    /// <inheritdoc/>
    public override bool CanConvert(Type objectType)
    {
        return true;
    }

    /// <inheritdoc/>
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        var valueToSerialize = value;
        if (value is ThemeValueReference reference)
        {
            valueToSerialize = $"{{Bind {reference.CustomKey}{(string.IsNullOrEmpty(reference.Property) ? string.Empty : $".{reference.Property}")}}}";
        }

        serializer.Serialize(writer, valueToSerialize);
    }

    /// <inheritdoc/>
    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var token = JToken.ReadFrom(reader);
        if (token.Type == JTokenType.String)
        {
            var value = token.ToObject<string>();
            var match = ReferenceRegex.Match(value ?? string.Empty);
            if (match.Success)
            {
                var key = match.Groups["key"].Value;
                var property = match.Groups["property"].Value;
                return new ThemeValueReference(key, property);
            }
        }

        return token.ToObject<T>();
    }
}
