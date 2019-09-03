using Net.Chdk.Meta.Model.CameraList;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers.CameraList
{
    public interface ICameraListProvider
    {
        IDictionary<string, ListPlatformData> GetCameraList(string path, string productName);
    }
}
