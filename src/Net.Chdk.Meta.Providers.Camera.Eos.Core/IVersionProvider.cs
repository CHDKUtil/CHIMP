using System.Collections.Generic;
using Net.Chdk.Meta.Model.Camera.Eos;
using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Model.CameraTree;

namespace Net.Chdk.Meta.Providers.Camera.Eos
{
    public interface IVersionProvider
    {
        IDictionary<string, VersionData> GetVersions(string productName, ListPlatformData list, TreePlatformData tree);
    }
}
