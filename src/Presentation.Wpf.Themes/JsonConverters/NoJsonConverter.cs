namespace MaSch.Presentation.Wpf.JsonConverters;

/// <summary>
/// <see cref="JsonConverter"/> that when applied to a property uses the default converter by throwing <see cref="NotImplementedException"/> for each method.
/// See https://stackoverflow.com/questions/39738714/selectively-use-default-json-converter/39739105#39739105.
/// </summary>
/// <seealso cref="JsonConverter" />
public class NoJsonConverter : JsonConverter
{
    /// <inheritdoc/>
    public override bool CanRead => false;

    /// <inheritdoc/>
    public override bool CanWrite => false;

    /// <inheritdoc/>
    public override bool CanConvert(Type objectType)
    {
        // Note - not called when attached directly via [JsonConverter(typeof(NoJsonConverter))]
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}
