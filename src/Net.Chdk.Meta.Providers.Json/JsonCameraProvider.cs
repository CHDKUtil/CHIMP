using Net.Chdk.Meta.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Net.Chdk.Meta.Providers.Json
{
    public abstract class JsonCameraProvider<TPlatform, TRevision, TSource>
        where TPlatform : PlatformData<TPlatform, TRevision, TSource>
        where TRevision : RevisionData<TRevision, TSource>
        where TSource : SourceData<TSource>
    {
        #region Constructor

        protected JsonCameraProvider()
        {
            _serializer = new Lazy<JsonSerializer>(GetSerializer);
            _settings = new Lazy<JsonSerializerSettings>(GetSettings);
        }

        #endregion

        #region Extension

        public string Extension => ".json";

        #endregion

        #region ReadJson

        protected IDictionary<string, TPlatform> GetCameras(string path)
        {
            using (var reader = File.OpenText(path))
            using (var jsonReader = new JsonTextReader(reader))
            {
                return Serializer.Deserialize<IDictionary<string, TPlatform>>(jsonReader);
            }
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

        private JsonSerializerSettings Settings => _settings.Value;

        private JsonSerializerSettings GetSettings()
        {
            return new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = GetConverters().ToArray(),
            };
        }

        private IEnumerable<JsonConverter> GetConverters()
        {
            yield break;
        }

        #endregion
    }
}
