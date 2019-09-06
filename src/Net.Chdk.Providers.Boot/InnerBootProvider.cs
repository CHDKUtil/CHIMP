using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;

namespace Net.Chdk.Providers.Boot
{
    sealed class InnerBootProvider : BootProvider<InnerBootProvider.BootData>, IInnerBootProvider
    {
        #region Constants

        private const string DataFileName = "boot.json";

        #endregion

        #region Constructor

        public InnerBootProvider(string categoryName, ILoggerFactory loggerFactory)
            : base(loggerFactory.CreateLogger<InnerBootProvider>())
        {
            CategoryName = categoryName;
        }

        #endregion

        #region IBootProvider Members

        public string FileName => Data.Files.First().Key;
        public int[][] Offsets => Data.Offsets;
        public byte[] Prefix => Data.Prefix;

        #endregion

        #region Data

        internal sealed class BootData : DataBase
        {
            public int[][] Offsets { get; set; }
            public byte[] Prefix { get; set; }
        }

        private string CategoryName { get; }

        protected override string GetFilePath()
        {
            return Path.Combine(Directories.Data, Directories.Category, CategoryName, DataFileName);
        }

        #endregion
    }
}
