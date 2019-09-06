using System.Collections.Generic;
using Net.Chdk.Meta.Model.Camera;
using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Model.CameraTree;

namespace Net.Chdk.Meta.Providers.Camera
{
    public interface IRevisionProvider<TRevision>
        where TRevision: IRevisionData
    {
        IDictionary<string, TRevision> GetRevisions(string productName, ListPlatformData list, TreePlatformData tree);
    }
}
