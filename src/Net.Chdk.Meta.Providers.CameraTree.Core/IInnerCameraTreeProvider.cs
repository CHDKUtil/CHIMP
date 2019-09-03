using Net.Chdk.Meta.Model.CameraTree;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers.CameraTree
{
    public interface IInnerCameraTreeProvider : IExtensionProvider
    {
        IDictionary<string, TreePlatformData> GetCameraTree(string path);
    }
}
