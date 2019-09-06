using Net.Chdk.Model.Category;
using Net.Chdk.Model.Software;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Providers.Software
{
    sealed class SourceProvider : ISourceProvider
    {
        private IEnumerable<IProductSourceProvider> ProductSourceProviders { get; }

        public SourceProvider(IEnumerable<IProductSourceProvider> productSourceProviders)
        {
            ProductSourceProviders = productSourceProviders;
        }

        public IEnumerable<ProductSource> GetSources(CategoryInfo category)
        {
            return ProductSourceProviders
                .SelectMany(p => p.GetSources(category));
        }

        public IEnumerable<ProductSource> GetSources(SoftwareProductInfo product)
        {
            return ProductSourceProviders
                .SelectMany(p => p.GetSources(product));
        }

        public IEnumerable<SoftwareSourceInfo> GetSources(SoftwareProductInfo product, string sourceName)
        {
            return ProductSourceProviders
                .SelectMany(p => p.GetSources(product, sourceName));
        }
    }
}
