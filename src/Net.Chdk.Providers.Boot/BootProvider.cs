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

        public string? GetFileName(string categoryName)
        {
            return GetProvider(categoryName)?.FileName;
        }

        public int[][]? GetOffsets(string categoryName)
        {
            return GetProvider(categoryName)?.Offsets;
        }

        public byte[]? GetPrefix(string categoryName)
        {
            return GetProvider(categoryName)?.Prefix;
        }

        public uint GetBlockSize(string categoryName, string fileSystem)
        {
            var provider = GetProvider(categoryName);
            return provider != null
                ? provider.GetBlockSize(fileSystem)
                : 0;
        }

        public IDictionary<int, byte[]>? GetBytes(string categoryName, string fileSystem)
        {
            return GetProvider(categoryName)?.GetBytes(fileSystem);
        }

        public IDictionary<string, byte[]>? GetFiles(string categoryName)
        {
            return GetProvider(categoryName)?.Files;
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
