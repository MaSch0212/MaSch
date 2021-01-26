using System;
using MaSch.Presentation.Wpf.ThemeValues;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MaSch.Presentation.Wpf.JsonConverters
{
    public class ThemeValueJsonConverter : JsonConverter<IThemeValue>
    {
        private bool _canWrite = true;
        private bool _canRead = true;

        public override bool CanWrite => _canWrite;
        public override bool CanRead => _canRead;

        public ThemeValueJsonConverter() : this (true) { }
        public ThemeValueJsonConverter(bool canRead)
        {
            _canRead = canRead;
        }

        public override void WriteJson(JsonWriter writer, IThemeValue value, JsonSerializer serializer)
        {
            _canWrite = false;
            try
            {
                var jObject = JObject.FromObject(value, serializer);
                var type = ThemeValueRegistry.GetValueTypeEnum(value.GetType());
                jObject.AddFirst(new JProperty("Type", type));
                jObject.WriteTo(writer);
            }
            finally
            {
                _canWrite = true;
            }
        }

        public override IThemeValue ReadJson(JsonReader reader, Type objectType, IThemeValue existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            _canRead = false;
            try
            {
                var jToken = JToken.ReadFrom(reader);
                if (jToken.Type == JTokenType.Null)
                    return null;

                var type = jToken.Value<string>("Type");
                var runtimeType = ThemeValueRegistry.GetRuntimeValueType(type);
                return (IThemeValue)jToken.ToObject(runtimeType, serializer);
            }
            finally
            {
                _canRead = true;
            }
        }
    }
}
