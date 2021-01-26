﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSch.Presentation.Wpf.JsonConverters
{
    // https://stackoverflow.com/questions/39738714/selectively-use-default-json-converter/39739105#39739105
    public class NoJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            // Note - not called when attached directly via [JsonConverter(typeof(NoJsonConverter))]
            throw new NotImplementedException();
        }

        public override bool CanRead { get { return false; } }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanWrite { get { return false; } }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
