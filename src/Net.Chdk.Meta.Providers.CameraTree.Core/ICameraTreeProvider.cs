using Net.Chdk.Meta.Model.CameraTree;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers.CameraTree
{
    public interface ICameraTreeProvider
    {
        IDictionary<string, TreePlatformData> GetCameraTree(string path);
    }
}
