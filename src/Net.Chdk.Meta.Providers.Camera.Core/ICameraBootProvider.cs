using Net.Chdk.Meta.Model.Camera;

namespace Net.Chdk.Meta.Providers.Camera
{
    public interface ICameraBootProvider
    {
        BootData GetBoot(uint modelId, string productName);
    }
}
