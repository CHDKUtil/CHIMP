using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers;
using Net.Chdk.Providers.Product;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Net.Chdk.Meta.Providers.Software
{
    sealed class BuildMetaProvider : DataProvider<Dictionary<Version, SoftwareBuildInfo>[]>, IBuildMetaProvider
    {
        #region Constants

        private const string DataFileName = "builds.json";

        #endregion

        #region Fields

        private IProductProvider ProductProvider { get; }

        #endregion

        #region Constructor

        public BuildMetaProvider(IProductProvider productProvider, ILogger<BuildMetaProvider> logger)
            : base(logger)
        {
            ProductProvider = productProvider;
        }

        #endregion

        #region IBetaBuildProvider Members

        public SoftwareBuildInfo GetBuild(SoftwareInfo software)
        {
            if (software.Camera != null)
                return Data[0][software.Product.Version];
            else
                return Data[1][software.Product.Version];
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
