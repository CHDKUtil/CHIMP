using System.Collections.Generic;
using Net.Chdk.Meta.Model.Camera.Ps;
using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Model.CameraTree;

namespace Net.Chdk.Meta.Providers.Camera.Ps
{
    public interface IRevisionProvider
    {
        IDictionary<string, RevisionData> GetRevisions(string productName, ListPlatformData list, TreePlatformData tree);
    }
}
