using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;

namespace Net.Chdk.Json
{
    public static class JsonObject
    {
        private static readonly Lazy<JsonSerializer> serializer = new Lazy<JsonSerializer>(GetSerializer);

        public static JsonSerializer Serializer => serializer.Value;

        public static void Serialize<T>(Stream stream, T obj)
        {
            using (var writer = new StreamWriter(stream))
            {
                Serializer.Serialize(writer, obj, typeof(T));
            }
        }

        public static T Deserialize<T>(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                return (T)Serializer.Deserialize(reader, typeof(T));
            }
        }

        private static JsonSerializer GetSerializer()
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new[] { new VersionConverter() },
            };
            return JsonSerializer.CreateDefault(settings);
        }
    }
}
