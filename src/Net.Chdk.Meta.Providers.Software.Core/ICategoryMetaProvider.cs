using Net.Chdk.Model.Category;
using Net.Chdk.Model.Software;

namespace Net.Chdk.Meta.Providers.Software
{
    public interface ICategoryMetaProvider
    {
        CategoryInfo GetCategory(SoftwareInfo software);
    }
}
