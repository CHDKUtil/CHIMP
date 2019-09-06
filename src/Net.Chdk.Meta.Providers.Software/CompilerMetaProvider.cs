using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers;
using Net.Chdk.Providers.Product;
using System.IO;
using System.Linq;

namespace Net.Chdk.Meta.Providers.Software
{
    sealed class CompilerMetaProvider : DataProvider<SoftwareCompilerInfo[]>, ICompilerMetaProvider
    {
        #region Constants

        private const string DataFileName = "compilers.json";

        #endregion

        #region Fields

        private IProductProvider ProductProvider { get; }

        #endregion

        #region Constructor

        public CompilerMetaProvider(IProductProvider productProvider, ILogger<CompilerMetaProvider> logger)
            : base(logger)
        {
            ProductProvider = productProvider;
        }

        #endregion

        #region ICompilerMetaProvider Members

        public SoftwareCompilerInfo GetCompiler(SoftwareInfo software)
        {
            if (software.Camera != null)
                return Data[0];
            else
                return Data[1];
        }

        #endregion

        #region Data

        protected override string GetFilePath()
        {
            var productName = ProductProvider.GetProductNames().Single();
            return Path.Combine(Directories.Data, Directories.Product, productName, DataFileName);
        }

        #endregion
    }
}
