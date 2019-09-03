using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Model.CameraTree;

namespace Net.Chdk.Meta.Providers.Camera
{
    public interface IProductCameraModelValidator : IProductNameProvider
    {
        void Validate(string platform, ListPlatformData list, TreePlatformData tree);
    }
}
