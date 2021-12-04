using MaSch.Presentation.Wpf.ThemeValues;
using Newtonsoft.Json.Linq;

namespace MaSch.Presentation.Wpf.JsonConverters;

/// <summary>
/// <see cref="JsonConverter"/> that is used to convert <see cref="IThemeValue"/> to and from json.
/// </summary>
/// <seealso cref="JsonConverter{T}" />
public class ThemeValueJsonConverter : JsonConverter<IThemeValue>
{
    private bool _canWrite = true;
    private bool _canRead;

    /// <summary>
    /// Initializes a new instance of the <see cref="ThemeValueJsonConverter"/> class.
    /// </summary>
    public ThemeValueJsonConverter()
        : this(true)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ThemeValueJsonConverter"/> class.
    /// </summary>
    /// <param name="canRead">if set to <c>true</c> this convert can be used to read json documents.</param>
    public ThemeValueJsonConverter(bool canRead)
    {
        _canRead = canRead;
    }

    /// <inheritdoc/>
    public override bool CanWrite => _canWrite;

    /// <inheritdoc/>
    public override bool CanRead => _canRead;

    /// <inheritdoc/>
    public override void WriteJson(JsonWriter writer, IThemeValue? value, JsonSerializer serializer)
    {
        _canWrite = false;
        try
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var jsonObject = JObject.FromObject(value, serializer);
            var type = ThemeValueRegistry.GetValueTypeEnum(value.GetType());
            jsonObject.AddFirst(new JProperty("Type", type));
            jsonObject.WriteTo(writer);
        }
        finally
        {
            _canWrite = true;
        }
    }

    /// <inheritdoc/>
    public override IThemeValue ReadJson(JsonReader reader, Type objectType, IThemeValue? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        _canRead = false;
        try
        {
            var token = JToken.ReadFrom(reader);
            if (token.Type == JTokenType.Null)
                return null!;

            var type = token.Value<string>("Type");
            var runtimeType = ThemeValueRegistry.GetRuntimeValueType(type);
            return (IThemeValue)token.ToObject(runtimeType, serializer)!;
        }
        finally
        {
            _canRead = true;
        }
    }
}
