using Chimp.Model;
using Chimp.Providers.Tips;
using Net.Chdk;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Chimp.Providers
{
    sealed class ProductTipProvider : DataProvider<string, ITipProvider>, ITipProvider
    {
        private const string DataFileName = "tips.json";

        private string ProductName { get; }

        public ProductTipProvider(IServiceActivator serviceActivator, string productName = null)
            : base(serviceActivator)
        {
            ProductName = productName;
        }

        public IEnumerable<Tip> GetTips(string productText)
        {
            return Data
                .Select(kvp => CreateProvider(kvp.Key, kvp.Value, kvp.Key))
                .SelectMany(p => p.GetTips(productText));
        }

        protected override string GetFilePath()
        {
            return ProductName != null
                ? Path.Combine(Directories.Data, Directories.Product, ProductName, DataFileName)
                : Path.Combine(Directories.Data, DataFileName);
        }

        protected override string GetNamespace(string _) => typeof(TipProvider).Namespace;

        protected override string GetTypeSuffix() => nameof(TipProvider);
    }
}
