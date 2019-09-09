using Net.Chdk.Detectors.Software.Product;
using Net.Chdk.Providers.Boot;
using Net.Chdk.Providers.Product;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Net.Chdk.Detectors.Software.Chdk
{
    sealed class ChdkProductDetector : ProductDetector
    {
        private static readonly Dictionary<string, string> ChdkVersions = new Dictionary<string, string>
        {
            ["CCHDK4.CFG"] = "1.4",
            ["OSD__4.CFG"] = "1.4",
            ["CCHDK3.CFG"] = "1.3",
            ["OSD__3.CFG"] = "1.3",
            ["CCHDK2.CFG"] = "1.2",
            ["OSD__2.CFG"] = "1.2",
            ["CCHDK1.CFG"] = "1.1",
            ["OSD__1.CFG"] = "1.1",
            ["CCHDK.CFG"] = "1.0",
        };

        private static readonly Dictionary<string, string> DataLanguages = new Dictionary<string, string>
        {
            ["logo.dat"] = "en",
            ["logo_de.dat"] = "de",
        };

        public ChdkProductDetector(IProductProvider productProvider, IBootProvider bootProvider)
            : base(productProvider, bootProvider)
        {
        }

        protected override string ProductName => "CHDK";

        protected override Version GetVersion(string rootPath)
        {
            var chdkPath = Path.Combine(rootPath, ProductName);
            return GetValue(chdkPath, ChdkVersions, Version.Parse);
        }

        protected override CultureInfo GetLanguage(string rootPath)
        {
            var dataPath = Path.Combine(rootPath, ProductName, "DATA");
            return GetValue(dataPath, DataLanguages, GetCultureInfo);
        }
    }
}
