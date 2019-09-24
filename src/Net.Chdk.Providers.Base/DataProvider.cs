using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Net.Chdk.Providers
{
    public abstract class DataProvider<TData>
    {
        #region Fields

        private ILogger Logger { get; }

        #endregion

        #region Constructor

        protected DataProvider(ILogger logger)
        {
            Logger = logger;

            _serializer = new Lazy<JsonSerializer>(GetSerializer);
            _data = new Lazy<TData>(GetData);
        }

        #endregion

        #region Serializer

        private readonly Lazy<JsonSerializer> _serializer;

        private JsonSerializer Serializer => _serializer.Value;

        private JsonSerializer GetSerializer()
        {
            return JsonSerializer.CreateDefault(Settings);
        }

        private JsonSerializerSettings Settings => new JsonSerializerSettings
        {
            Converters = GetConverters().ToArray()
        };

        protected virtual IEnumerable<JsonConverter> GetConverters()
        {
            yield break;
        }

        #endregion

        #region Data

        private readonly Lazy<TData> _data;

        protected TData Data => _data.Value;

        private TData GetData()
        {
            var filePath = GetFilePath();
            using var reader = File.OpenText(filePath);
            using var jsonReader = new JsonTextReader(reader);
            var data = Serializer.Deserialize<TData>(jsonReader);
            if (LogLevel < LogLevel.None)
                Logger.Log(LogLevel, default, data, null, GetFormat);
            return data;
        }

        private string GetFormat(TData data, Exception ex)
        {
            return string.Format(Format, JsonConvert.SerializeObject(data));
        }

        protected abstract string GetFilePath();

        protected virtual LogLevel LogLevel => LogLevel.None;

        protected virtual string? Format => null;

        #endregion
    }
}
