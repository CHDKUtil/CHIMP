using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Model.CameraTree;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers.Camera
{
    public interface IProductCameraValidator : IProductNameProvider
    {
        void Validate(IDictionary<string, ListPlatformData> list, IDictionary<string, TreePlatformData> tree);
    }
}
