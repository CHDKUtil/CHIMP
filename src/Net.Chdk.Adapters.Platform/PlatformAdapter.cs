using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Adapters.Platform
{
    sealed class PlatformAdapter : IPlatformAdapter
    {
        private IEnumerable<IProductPlatformAdapter> InnerAdapters { get; }

        public PlatformAdapter(IEnumerable<IProductPlatformAdapter> innerAdapters)
        {
            InnerAdapters = innerAdapters;
        }

        public string NormalizePlatform(string productName, string platform)
        {
            return GetInnerAdapter(productName)?
                .NormalizePlatform(platform)
                ?? platform;
        }

        private IProductPlatformAdapter? GetInnerAdapter(string productName)
        {
            return InnerAdapters
                .SingleOrDefault(a => a.ProductName == productName);
        }
    }
}
