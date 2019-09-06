using Net.Chdk.Model.Category;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Category;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Meta.Providers.Software
{
    sealed class CategoryMetaProvider : ICategoryMetaProvider
    {
        private Dictionary<string, CategoryInfo> Categories { get; }

        public CategoryMetaProvider(ICategoryProvider categoryProvider)
        {
            Categories = categoryProvider.GetCategories().ToDictionary(
                c => c.Name,
                c => c);
        }

        public CategoryInfo GetCategory(SoftwareInfo software)
        {
            return Categories[software.Category.Name];
        }
    }
}
