using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Bro.Client.Json
{
    public class Vector2JsonConverter : JsonConverter<Vector2>
    {
        private const string _xPropertyName = "x";
        private const string _yPropertyName = "y";
        
        public override void WriteJson(JsonWriter writer, Vector2 value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName(_xPropertyName);
            writer.WriteValue(value.x);
            writer.WritePropertyName(_yPropertyName);
            writer.WriteValue(value.y);
            writer.WriteEndObject();
        }

        public override Vector2 ReadJson(JsonReader reader, System.Type objectType, Vector2 existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            float x = (float)jsonObject[_xPropertyName];
            float y = (float)jsonObject[_yPropertyName];
            return new Vector2(x, y);
        }
    }
}