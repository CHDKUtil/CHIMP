using Net.Chdk.Meta.Model.Camera;

namespace Net.Chdk.Meta.Providers.Camera
{
    public interface IProductCameraBootProvider : IProductNameProvider
    {
        BootData GetBoot(uint modelId);
    }
}
