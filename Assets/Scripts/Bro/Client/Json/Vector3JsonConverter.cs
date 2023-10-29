using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Bro.Client.Json
{
    public class Vector3JsonConverter : JsonConverter<Vector3>
    {
        private const string _xPropertyName = "x";
        private const string _yPropertyName = "y";
        private const string _zPropertyName = "z";
        
        public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName(_xPropertyName);
            writer.WriteValue(value.x);
            writer.WritePropertyName(_yPropertyName);
            writer.WriteValue(value.y);
            writer.WritePropertyName(_zPropertyName);
            writer.WriteValue(value.z);
            writer.WriteEndObject();
        }

        public override Vector3 ReadJson(JsonReader reader, System.Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            float x = (float)jsonObject[_xPropertyName];
            float y = (float)jsonObject[_yPropertyName];
            float z = (float)jsonObject[_zPropertyName];
            return new Vector3(x, y, z);
        }
    }
}