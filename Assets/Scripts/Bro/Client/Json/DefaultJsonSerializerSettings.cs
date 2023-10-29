using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace Bro.Client.Json
{
    public static class DefaultJsonSerializerSettings
    {
        public static JsonSerializerSettings AutoSettings { get; }
        public static JsonSerializerSettings ObjectsSettings { get; }
        public static JsonSerializerSettings NoObjectSettings { get; }

        private static IEnumerable<JsonSerializerSettings> _settings =>
            new JsonSerializerSettings[]
            {
                AutoSettings, ObjectsSettings, NoObjectSettings
            };


        static DefaultJsonSerializerSettings()
        {
            NoObjectSettings = CreateJsonSerializerSettings(TypeNameHandling.None);
            AutoSettings = CreateJsonSerializerSettings(TypeNameHandling.Auto);
            ObjectsSettings = CreateJsonSerializerSettings(TypeNameHandling.Objects);

            AddConverter(new RectJsonConverter());
            AddConverter(new Vector2JsonConverter());
            AddConverter(new Vector3JsonConverter());
        }

        static JsonSerializerSettings CreateJsonSerializerSettings(TypeNameHandling typeNameHandling)
        {
            var binder = new KnownTypesSerializationBinder();

            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var t in types)
            {
                if (t.GetCustomAttribute(typeof(JsonTypeAttribute)) is JsonTypeAttribute attribute)
                {
                    binder.RegisterType(attribute.TypeName, t);
                }
            }

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                TypeNameHandling = typeNameHandling,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                SerializationBinder = binder
            };

            return settings;
        }


        public static void AddConverter(JsonConverter converter)
        {
            foreach (var setting in _settings)
            {
                if (!setting.Converters.Contains(converter))
                {
                    setting.Converters.Add(converter);
                }
            }
        }
    }
}