using Net.Chdk.Model.Category;
using Net.Chdk.Model.Software;
using System.Collections.Generic;

namespace Net.Chdk.Providers.Software
{
    public interface IProductSourceProvider
    {
        IEnumerable<ProductSource> GetSources(CategoryInfo category);
        IEnumerable<ProductSource> GetSources(SoftwareProductInfo product);
        IEnumerable<SoftwareSourceInfo> GetSources(SoftwareProductInfo product, string sourceName);
    }
}
