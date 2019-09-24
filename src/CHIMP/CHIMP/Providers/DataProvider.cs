using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Chimp.Providers
{
    abstract class DataProvider<TData, TValue> : Provider<TData, TValue>
        where TData : class
    {
        #region Static Constructor

        static DataProvider()
        {
            _serializer = new Lazy<JsonSerializer>(GetSerializer);
        }

        #endregion

        #region Constructor

        protected DataProvider(IServiceActivator serviceProvider)
            : base(serviceProvider)
        {
            _data = new Lazy<IDictionary<string, TData>>(GetData);
        }

        #endregion

        #region Abstract Members

        protected abstract string GetFilePath();

        #endregion

        #region Data

        private readonly Lazy<IDictionary<string, TData>> _data;

        protected override IDictionary<string, TData>? Data => _data.Value;

        private IDictionary<string, TData> GetData()
        {
            var filePath = GetFilePath();
            using var reader = File.OpenText(filePath);
            using var jsonReader = new JsonTextReader(reader);
            return Serializer.Deserialize<IDictionary<string, TData>>(jsonReader);
        }

        #endregion

        #region Serializer

        private static readonly Lazy<JsonSerializer> _serializer;

        private static JsonSerializer Serializer => _serializer.Value;

        private static JsonSerializer GetSerializer()
        {
            return JsonSerializer.CreateDefault();
        }

        #endregion
    }
}
