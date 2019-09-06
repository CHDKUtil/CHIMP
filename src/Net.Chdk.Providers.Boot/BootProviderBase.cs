using Microsoft.Extensions.Logging;
using Net.Chdk.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Chdk.Providers.Boot
{
    abstract class BootProvider<TData> : DataProvider<TData>
        where TData : BootProvider<TData>.DataBase
    {
        #region Constructor

        protected BootProvider(ILogger logger)
            : base(logger)
        {
            bytes = new Lazy<Dictionary<string, Dictionary<int, byte[]>>>(GetBytes);
            files = new Lazy<Dictionary<string, byte[]>>(DoGetFiles);
        }

        #endregion

        #region Public Methods

        public uint GetBlockSize(string fileSystem)
        {
            Data.Sizes.TryGetValue(fileSystem, out uint size);
            return size;
        }

        public IDictionary<int, byte[]> GetBytes(string fileSystem)
        {
            Bytes.TryGetValue(fileSystem, out Dictionary<int, byte[]> bytes);
            return bytes;
        }

        #endregion

        #region Converters

        protected override IEnumerable<JsonConverter> GetConverters()
        {
            yield return new HexStringJsonConverter();
        }

        #endregion

        #region Data

        internal abstract class DataBase
        {
            public Dictionary<string, uint> Sizes { get; set; }
            public Dictionary<string, Dictionary<string, string>> Strings { get; set; }
            public Dictionary<string, string> Files { get; set; }
        }

        #endregion

        #region Bytes

        private readonly Lazy<Dictionary<string, Dictionary<int, byte[]>>> bytes;

        private Dictionary<string, Dictionary<int, byte[]>> Bytes => bytes.Value;

        private Dictionary<string, Dictionary<int, byte[]>> GetBytes()
        {
            return Data.Strings.ToDictionary(kvp => kvp.Key, GetBytes);
        }

        private static Dictionary<int, byte[]> GetBytes(KeyValuePair<string, Dictionary<string, string>> kvp)
        {
            return kvp.Value.ToDictionary(GetStartIndex, GetBytes);
        }

        #endregion

        #region Files

        private Lazy<Dictionary<string, byte[]>> files;

        protected Dictionary<string, byte[]> Files => files.Value;

        private Dictionary<string, byte[]> DoGetFiles()
        {
            return Data.Files.ToDictionary(kvp => kvp.Key, GetBytes);
        }

        #endregion

        #region Helper Methods

        private static int GetStartIndex(KeyValuePair<string, string> kvp)
        {
            return Convert.ToInt32(kvp.Key, 16);
        }

        private static byte[] GetBytes(KeyValuePair<string, string> kvp)
        {
            return Encoding.ASCII.GetBytes(kvp.Value);
        }

        #endregion
    }
}
