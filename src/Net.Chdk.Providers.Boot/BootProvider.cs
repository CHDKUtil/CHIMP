using Microsoft.Extensions.Logging;
using Net.Chdk.Providers.Product;
using System.Collections.Generic;

namespace Net.Chdk.Providers.Boot
{
    sealed class BootProvider : ProviderResolver<IInnerBootProvider>, IBootProvider
    {
        #region Fields

        private IProductProvider ProductProvider { get; }

        #endregion

        #region Constructor

        public BootProvider(IProductProvider productProvider, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            ProductProvider = productProvider;
        }

        #endregion

        #region IBootProvider Members

        public string GetFileName(string categoryName)
        {
            return Providers[categoryName].FileName;
        }

        public int[][] GetOffsets(string categoryName)
        {
            return Providers[categoryName].Offsets;
        }

        public byte[] GetPrefix(string categoryName)
        {
            return Providers[categoryName].Prefix;
        }

        public uint GetBlockSize(string categoryName, string fileSystem)
        {
            return Providers[categoryName].GetBlockSize(fileSystem);
        }

        public IDictionary<int, byte[]> GetBytes(string categoryName, string fileSystem)
        {
            return Providers[categoryName].GetBytes(fileSystem);
        }

        #endregion

        #region Providers

        protected override IEnumerable<string> GetNames()
        {
            return ProductProvider.GetCategoryNames();
        }

        protected override IInnerBootProvider CreateProvider(string categoryName)
        {
            return new InnerBootProvider(categoryName, LoggerFactory);
        }

        #endregion
    }
}
