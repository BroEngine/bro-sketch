using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Bro.Client.Json
{
    public class RectJsonConverter : JsonConverter<Rect>
    {
        private static class PropertyName
        {
            public const string x = "x";
            public const string y = "y";
            public const string width = "w";
            public const string height = "h";
        }


        public override void WriteJson(JsonWriter writer, Rect value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName(PropertyName.x);
            writer.WriteValue(value.x);
            writer.WritePropertyName(PropertyName.y);
            writer.WriteValue(value.y);
            writer.WritePropertyName(PropertyName.width);
            writer.WriteValue(value.width);
            writer.WritePropertyName(PropertyName.height);
            writer.WriteValue(value.height);
            writer.WriteEndObject();
        }

        public override Rect ReadJson(JsonReader reader, System.Type objectType, Rect existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            float x = (float)jsonObject[PropertyName.x];
            float y = (float)jsonObject[PropertyName.y];
            float width = (float)jsonObject[PropertyName.width];
            float height = (float)jsonObject[PropertyName.height];
            return new Rect(x, y, width, height);
        }
    }
}