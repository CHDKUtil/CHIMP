using Microsoft.Extensions.Logging;
using Net.Chdk.Providers.Product;
using System.Collections.Generic;

namespace Net.Chdk.Providers.Software
{
    sealed class ModuleProvider : ProviderResolver<IProductModuleProvider>, IModuleProvider
    {
        #region Fields

        private IProductProvider ProductProvider { get; }

        #endregion

        #region Constructor

        public ModuleProvider(IProductProvider productProvider, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            ProductProvider = productProvider;
        }

        #endregion

        #region IModuleProvider Members

        public string GetPath(string productName)
        {
            return GetProvider(productName)?.Path;
        }

        public string GetExtension(string productName)
        {
            return GetProvider(productName)?.Extension;
        }

        public string GetModuleName(string productName, string filePath)
        {
            return GetProvider(productName)?.GetModuleName(filePath);
        }

        public string GetModuleTitle(string productName, string moduleName)
        {
            return GetProvider(productName)?.GetModuleTitle(moduleName);
        }

        public string GetModuleId(string productName, string moduleName)
        {
            return GetProvider(productName)?.GetModuleId(moduleName);
        }

        #endregion

        #region Providers

        protected override IEnumerable<string> GetNames()
        {
            return ProductProvider.GetProductNames();
        }

        protected override IProductModuleProvider CreateProvider(string productName)
        {
            return new ProductModuleProvider(productName, LoggerFactory);
        }

        #endregion
    }
}
