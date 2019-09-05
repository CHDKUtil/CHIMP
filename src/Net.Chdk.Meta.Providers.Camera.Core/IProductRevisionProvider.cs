using System.Collections.Generic;
using Net.Chdk.Meta.Model.Camera;
using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Model.CameraTree;

namespace Net.Chdk.Meta.Providers.Camera
{
    public interface IProductRevisionProvider : IProductNameProvider
    {
        IDictionary<string, IRevisionData> GetRevisions(ListPlatformData list, TreePlatformData tree);
    }
}
