using Net.Chdk.Meta.Model.CameraTree;
using Net.Chdk.Meta.Model.Platform;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers.Camera
{
    public interface IProductCameraPlatformProvider : IProductNameProvider
    {
        PlatformData GetPlatform(string key, IDictionary<string, PlatformData> platform);
        TreePlatformData GetTree(string key, IDictionary<string, TreePlatformData> tree);
    }
}
