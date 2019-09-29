using Microsoft.Extensions.Logging;
using Net.Chdk.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Net.Chdk.Providers.Boot
{
    sealed class InnerBootProvider : DataProvider<InnerBootProvider.BootData>, IInnerBootProvider
    {
        #region Constants

        private const string DataFileName = "boot.json";

        #endregion

        #region Constructor

        public InnerBootProvider(string categoryName, ILoggerFactory loggerFactory)
            : base(loggerFactory.CreateLogger<InnerBootProvider>())
        {
            CategoryName = categoryName;

            files = new Lazy<Dictionary<string, byte[]>?>(GetFiles);
            bytes = new Lazy<Dictionary<string, Dictionary<int, byte[]>>?>(GetBytes);
        }

        #endregion

        #region IBootProvider Members

        public string? FileName => Data.Files?.FirstOrDefault().Key;
        public int[][]? Offsets => Data.Offsets;
        public byte[]? Prefix => Data.Prefix;

        public uint GetBlockSize(string fileSystem)
        {
            if (Data.Sizes == null)
                return 0;
            Data.Sizes.TryGetValue(fileSystem, out uint size);
            return size;
        }

        public IDictionary<int, byte[]>? GetBytes(string fileSystem)
        {
            if (Bytes == null)
                return null;
            Bytes.TryGetValue(fileSystem, out Dictionary<int, byte[]> bytes);
            return bytes;
        }

        #endregion

        #region Data

        internal sealed class BootData
        {
            public Dictionary<string, uint>? Sizes { get; set; }
            public Dictionary<string, Dictionary<string, string>>? Strings { get; set; }
            public Dictionary<string, string>? Files { get; set; }
            public int[][]? Offsets { get; set; }
            public byte[]? Prefix { get; set; }
        }

        private string CategoryName { get; }

        protected override string GetFilePath()
        {
            return Path.Combine(Directories.Data, Directories.Category, CategoryName, DataFileName);
        }

        #endregion

        #region Converters

        protected override IEnumerable<JsonConverter> GetConverters()
        {
            yield return new HexStringJsonConverter();
        }

        #endregion

        #region Files

        private readonly Lazy<Dictionary<string, byte[]>?> files;

        public IDictionary<string, byte[]>? Files => files.Value;

        private Dictionary<string, byte[]>? GetFiles()
        {
            return Data.Files?
                .Where(kvp => kvp.Value != null)
                .ToDictionary(kvp => kvp.Key, GetBytes);
        }

        #endregion

        #region Bytes

        private readonly Lazy<Dictionary<string, Dictionary<int, byte[]>>?> bytes;

        private Dictionary<string, Dictionary<int, byte[]>>? Bytes => bytes.Value;

        private Dictionary<string, Dictionary<int, byte[]>>? GetBytes()
        {
            return Data.Strings?
                .ToDictionary(kvp => kvp.Key, GetBytes);
        }

        private static Dictionary<int, byte[]> GetBytes(KeyValuePair<string, Dictionary<string, string>> kvp)
        {
            return kvp.Value.ToDictionary(GetStartIndex, GetBytes);
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
