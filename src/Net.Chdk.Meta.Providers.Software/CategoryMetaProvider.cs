using Net.Chdk.Model.Category;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Product;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Meta.Providers.Software
{
    sealed class CategoryMetaProvider : ICategoryMetaProvider
    {
        private Dictionary<string, CategoryInfo> Categories { get; }

        public CategoryMetaProvider(IProductProvider productProvider)
        {
            Categories = productProvider.GetCategoryNames().ToDictionary(
                n => n,
                CreateCategoryInfo);
        }

        private CategoryInfo CreateCategoryInfo(string name)
        {
            return new CategoryInfo
            {
                Name = name
            };
        }

        public CategoryInfo GetCategory(SoftwareInfo software)
        {
            return Categories[software.Category.Name];
        }
    }
}
