using Net.Chdk.Detectors.Software.Product;
using Net.Chdk.Providers.Boot;
using Net.Chdk.Providers.Product;
using System;
using System.Globalization;

namespace Net.Chdk.Detectors.Software.Ml
{
    sealed class MlProductDetector : ProductDetector
    {
        public MlProductDetector(IProductProvider productProvider, IBootProvider bootProvider)
            : base(productProvider, bootProvider)
        {
        }

        protected override string ProductName => "ML";

        protected override Version GetVersion(string rootPath)
        {
            return null;
        }

        protected override CultureInfo GetLanguage(string rootPath)
        {
            return GetCultureInfo("en");
        }
    }
}
