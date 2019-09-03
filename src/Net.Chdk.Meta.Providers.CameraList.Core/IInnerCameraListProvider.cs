using Net.Chdk.Meta.Model.CameraList;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers.CameraList
{
    public interface IInnerCameraListProvider : IExtensionProvider
    {
        IDictionary<string, ListPlatformData> GetCameraList(string path, string productName);
    }
}
