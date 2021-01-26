using System;
using System.Text.RegularExpressions;
using MaSch.Presentation.Wpf.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MaSch.Presentation.Wpf.JsonConverters
{
    public abstract class ThemeValuePropertyJsonConverter : JsonConverter
    {
        protected static readonly Regex ReferenceRegex = new Regex(@"^\{Bind (?<key>[a-zA-Z_\-][a-zA-Z0-9_\-]*)(\.(?<property>[a-zA-Z_\-][a-zA-Z0-9_\-]*))?\}$", RegexOptions.Compiled);
    }
    public class ThemeValuePropertyJsonConverter<T> : ThemeValuePropertyJsonConverter
    {
        public override bool CanConvert(Type objectType) => true;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var valueToSerialize = value;
            if (value is ThemeValueReference reference)
            {
                valueToSerialize = $"{{Bind {reference.CustomKey}{(string.IsNullOrEmpty(reference.Property) ? "" : $".{reference.Property}")}}}";
            }
            serializer.Serialize(writer, valueToSerialize);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.ReadFrom(reader);
            if (token.Type == JTokenType.String)
            {
                var value = token.ToObject<string>();
                var match = ReferenceRegex.Match(value);
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
}
