using Net.Chdk.Model.Category;

namespace Net.Chdk.Providers.Category
{
    public interface ICategoryProvider
    {
        string[] GetCategoryNames();
        CategoryInfo[] GetCategories();
    }
}
