using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Chimp.Providers
{
    abstract class Provider<TData, TValue>
        where TData : class
    {
        protected IServiceActivator ServiceActivator { get; }

        static Provider()
        {
            _serializer = new Lazy<JsonSerializer>(GetSerializer);
        }

        protected Provider(IServiceActivator serviceActivator)
        {
            ServiceActivator = serviceActivator;

            _data = new Lazy<IDictionary<string, TData>>(GetData);
        }

        #region Serializer

        private static readonly Lazy<JsonSerializer> _serializer;

        private static JsonSerializer Serializer => _serializer.Value;

        private static JsonSerializer GetSerializer()
        {
            return JsonSerializer.CreateDefault();
        }

        #endregion

        #region Data

        private readonly Lazy<IDictionary<string, TData>> _data;

        protected virtual IDictionary<string, TData> Data => _data.Value;

        private IDictionary<string, TData> GetData()
        {
            var filePath = GetFilePath();
            using (var reader = File.OpenText(filePath))
            using (var jsonReader = new JsonTextReader(reader))
            {
                return Serializer.Deserialize<IDictionary<string, TData>>(jsonReader);
            }
        }

        #endregion

        protected TValue CreateProvider(string assembly, string type, Type[] argTypes = null, object[] argValues = null)
        {
            return ServiceActivator.Create<TValue>(assembly, $"{Namespace}.{type}{TypeSuffix}", argTypes, argValues);
        }

        protected T Create<T>(string assembly, string type, Type[] argTypes, object[] argValues)
        {
            return ServiceActivator.Create<T>(assembly, $"{Namespace}.{type}{TypeSuffix}", argTypes, argValues);
        }

        protected abstract string GetFilePath();

        protected abstract string Namespace { get; }
        protected abstract string TypeSuffix { get; }
    }

    abstract class Provider<T> : Provider<string, T>
        where T : class
    {
        protected Provider(IServiceActivator serviceProvider)
            : base(serviceProvider)
        {
        }
    }
}
