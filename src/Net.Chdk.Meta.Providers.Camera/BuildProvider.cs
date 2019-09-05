using System.Collections.Generic;
using Net.Chdk.Meta.Model.Camera;
using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Model.CameraTree;
using Net.Chdk.Meta.Model.Platform;
using Net.Chdk.Providers.Product;

namespace Net.Chdk.Meta.Providers.Camera
{
    sealed class BuildProvider : SingleCategoryProvider<ICategoryBuildProvider>, IBuildProvider
    {
        private IProductProvider ProductProvider { get; }

        public BuildProvider(IEnumerable<ICategoryBuildProvider> innerProviders, IProductProvider productProvider)
            : base(innerProviders)
        {
            ProductProvider = productProvider;
        }

        public IDictionary<string, ICameraData> GetCameras(IDictionary<string, PlatformData> platforms, IDictionary<string, ListPlatformData> list, IDictionary<string, TreePlatformData> tree,
            string productName)
        {
            var categoryName = ProductProvider.GetCategoryName(productName);
            return GetInnerProvider(categoryName)
                .GetCameras(platforms, list, tree, productName);
        }
    }
}
