using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Net.Chdk.Meta.Writers.Json
{
    public abstract class JsonMetaWriter
    {
        #region Constructor

        protected JsonMetaWriter()
        {
            _serializer = new Lazy<JsonSerializer>(GetSerializer);
            _settings = new Lazy<JsonSerializerSettings>(GetSettings);
        }

        #endregion

        #region Extension

        public string Extension => ".json";

        #endregion

        #region WriteJson

        protected void WriteJson<T>(string path, T obj)
        {
            using var writer = File.CreateText(path);
            Serializer.Serialize(writer, obj);
        }

        #endregion

        #region Serializer

        private readonly Lazy<JsonSerializer> _serializer;

        private JsonSerializer Serializer => _serializer.Value;

        private JsonSerializer GetSerializer()
        {
            return JsonSerializer.CreateDefault(Settings);
        }

        #endregion

        #region Settings

        private readonly Lazy<JsonSerializerSettings> _settings;

        public JsonSerializerSettings Settings => _settings.Value;

        private JsonSerializerSettings GetSettings()
        {
            return new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = GetConverters().ToArray(),
            };
        }

        protected virtual PreserveReferencesHandling PreserveReferencesHandling => PreserveReferencesHandling.None;

        protected virtual IEnumerable<JsonConverter> GetConverters()
        {
            yield break;
        }

        #endregion
    }
}
