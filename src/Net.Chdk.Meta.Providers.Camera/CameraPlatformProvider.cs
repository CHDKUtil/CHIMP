using Net.Chdk.Meta.Model.CameraTree;
using Net.Chdk.Meta.Model.Platform;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers.Camera
{
    sealed class CameraPlatformProvider : SingleProductProvider<IProductCameraPlatformProvider>, ICameraPlatformProvider
    {
        public CameraPlatformProvider(IEnumerable<IProductCameraPlatformProvider> innerProviders)
            : base(innerProviders)
        {
        }

        public PlatformData GetPlatform(string key, IDictionary<string, PlatformData> platform, string productName)
        {
            return GetInnerProvider(productName).GetPlatform(key, platform);
        }

        public TreePlatformData GetTree(string key, IDictionary<string, TreePlatformData> tree, string productName)
        {
            return GetInnerProvider(productName).GetTree(key, tree);
        }
    }
}
